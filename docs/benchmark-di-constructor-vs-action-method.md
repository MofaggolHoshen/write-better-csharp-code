# ⚡ DI in Controller Constructor vs Action Method — Benchmark

ASP.NET Core supports two ways to inject a service into a controller:

1. **Constructor Injection** — the DI container resolves the service when
   it builds the controller instance (once per request).
2. **Action Method Injection** — the service is resolved and passed directly
   to a specific action method via `[FromServices]` (per action call).

This benchmark measures the memory and time overhead of each approach.

------------------------------------------------------------------------

## ✅ Constructor Injection

``` csharp
public class OrderController(IOrderService orderService)
{
    private readonly IOrderService _orderService = orderService;

    public IEnumerable<string> GetOrders() => _orderService.GetOrders();
}
```

The controller is registered in the DI container. When a request arrives,
ASP.NET Core resolves `OrderController` from the container, which
automatically satisfies `IOrderService` through the constructor.

**Best for:** services used by most or all actions in a controller.

------------------------------------------------------------------------

## ✅ Action Method Injection

``` csharp
public class OrderController
{
    public IEnumerable<string> GetOrders([FromServices] IOrderService orderService) =>
        orderService.GetOrders();
}
```

The controller has no constructor dependency on the service. Instead, the
service is resolved by ASP.NET Core and injected as an action parameter
only when that specific action is called.

**Best for:** services used by only a few actions in a large controller,
keeping the constructor lean.

------------------------------------------------------------------------

## 🔬 What the Benchmark Measures

The benchmark isolates the **DI resolution overhead** of each strategy
using `Microsoft.Extensions.DependencyInjection` and `BenchmarkDotNet`.

| Strategy | What is resolved from DI |
|---|---|
| Constructor Injection | The entire controller (which resolves the service) |
| Action Method Injection | Only `IOrderService` (controller is `new`ed manually) |

``` csharp
[Benchmark(Baseline = true)]
public IEnumerable<string> ConstructorInjection()
{
    // DI builds the controller and injects IOrderService via constructor
    var controller = _serviceProvider.GetRequiredService<OrderControllerConstructor>();
    return controller.GetOrders();
}

[Benchmark]
public IEnumerable<string> ActionMethodInjection()
{
    // Controller is created directly; service is resolved separately
    var controller = new OrderControllerAction();
    var service = _serviceProvider.GetRequiredService<IOrderService>();
    return controller.GetOrders(service);
}
```

------------------------------------------------------------------------

## ▶ How to Run

``` bash
dotnet run --configuration Release source/banchmark-di-method-controller.cs
```

> ⚠ Always run benchmarks in **Release** mode. Debug builds include
> extra overhead that makes results unreliable.

------------------------------------------------------------------------

## 📊 Results

Measured on Intel Core i7-7500U, .NET 11.0.0 AOT, Windows 11:

| Method | Mean | Error | StdDev | Ratio | Allocated | Alloc Ratio |
|---|---:|---:|---:|---:|---:|---:|
| ConstructorInjection *(baseline)* | 392.7 ns | 12.99 ns | 18.21 ns | 1.00 | 184 B | 1.00 |
| ActionMethodInjection | 180.2 ns | 4.10 ns | 6.14 ns | **0.46** | 128 B | **0.70** |

**Action method injection is ~2× faster and allocates ~30% less memory.**

### Why is constructor injection slower?

Constructor injection resolves **two** objects from the DI container per
call — the controller itself *and* the service injected through its
constructor. The container must:

1. Look up `OrderControllerConstructor` in its registry
2. Resolve its dependency (`IOrderService`)
3. Allocate and construct both objects

This costs **392.7 ns** and **184 B** of managed heap per operation.

### Why is action method injection faster?

Action method injection bypasses the controller registration entirely.
The controller is `new`ed directly (zero container overhead), and only
`IOrderService` is resolved from the container:

1. Allocate the controller with `new` (cheap, no container lookup)
2. Look up and resolve only `IOrderService`

This costs **180.2 ns** and **128 B** — saving **~56 B** of allocation
(the controller object + its internal field reference) and roughly half
the resolution time.

> ⚠ **Important context:** In a real ASP.NET Core request, the framework
> always resolves the controller from the DI container regardless of which
> injection style you use. The benchmark isolates *container resolution
> overhead only*. In practice, the difference per request is negligible
> for typical workloads. Choose based on readability and design, not
> micro-benchmarked nanoseconds.

------------------------------------------------------------------------

## 🤔 Which Should You Use?

| Scenario | Recommendation |
|---|---|
| Service needed by most actions | ✅ Constructor Injection |
| Service needed by 1–2 actions in a large controller | ✅ Action Method Injection |
| You want to keep the controller constructor lean | ✅ Action Method Injection |
| You follow SOLID / Single Responsibility Principle | Consider splitting the controller instead |

------------------------------------------------------------------------

## 📚 References

- [Dependency injection into controllers — Microsoft Docs](https://learn.microsoft.com/en-us/aspnet/core/mvc/controllers/dependency-injection)
- [BenchmarkDotNet](https://benchmarkdotnet.org)
