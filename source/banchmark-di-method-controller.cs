/// <summary>
/// Benchmarks DI in Controller Constructor vs Action Method Injection
/// Based on: https://learn.microsoft.com/en-us/aspnet/core/mvc/controllers/dependency-injection
/// </summary>

#:package BenchmarkDotNet@0.14.0
#:package Microsoft.Extensions.DependencyInjection@9.0.0

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Toolchains.InProcess.NoEmit;
using Microsoft.Extensions.DependencyInjection;

// ============================================================
// Benchmark: Constructor Injection vs Action Method Injection
// ============================================================
// Constructor Injection  – service is injected once when the controller
//                          is created by the DI container (per request).
//
// Action Method Injection – service is resolved separately and passed
//                           as an action parameter ([FromServices]).
//                           Useful for services used in few actions only.
//
// Run in Release mode to get accurate results:
//   dotnet run --configuration Release banchmark-di-method-controller.cs
// ============================================================

#if DEBUG
// Quick run for debugging — skips the full BenchmarkDotNet harness
var quick = new DiBenchmark();
quick.Setup();
Console.WriteLine("=== Quick Debug Run ===");
quick.ConstructorInjection();
quick.ActionMethodInjection();
Console.WriteLine("Both benchmarks ran successfully.");
#else
// InProcessNoEmitToolchain runs benchmarks in the same process without Reflection.Emit.
// Required for single-file apps (no .csproj) and AOT runtimes.
var config = DefaultConfig.Instance
    .AddJob(Job.MediumRun.WithToolchain(InProcessNoEmitToolchain.Instance));
BenchmarkRunner.Run<DiBenchmark>(config);
#endif

// ============================================================
// Service interface and implementation
// ============================================================

public interface IOrderService
{
    IEnumerable<string> GetOrders();
}

public class OrderService : IOrderService
{
    public IEnumerable<string> GetOrders() =>
        ["Order #1001", "Order #1002", "Order #1003"];
}

// ============================================================
// Controller — Constructor Injection
// ============================================================
// The DI container resolves IOrderService once when it builds
// the controller instance.  All actions share the same service
// instance for the lifetime of the request.

public class OrderControllerConstructor(IOrderService orderService)
{
    private readonly IOrderService _orderService = orderService;

    public IEnumerable<string> GetOrders() => _orderService.GetOrders();
}

// ============================================================
// Controller — Action Method Injection
// ============================================================
// The DI container resolves IOrderService on every action call.
// Equivalent to using [FromServices] on an action parameter in ASP.NET Core.
// Ideal for large controllers where most actions don't need the service.

public class OrderControllerAction
{
    public IEnumerable<string> GetOrders(IOrderService orderService) =>
        orderService.GetOrders();
}

// ============================================================
// Benchmark class
// ============================================================

[MemoryDiagnoser]
public class DiBenchmark
{
    private ServiceProvider _serviceProvider = null!;
    // Consumer materializes IEnumerable results so BenchmarkDotNet
    // can measure real work rather than deferred iterator creation.
    private readonly Consumer _consumer = new();

    [GlobalSetup]
    public void Setup()
    {
        var services = new ServiceCollection();
        services.AddTransient<IOrderService, OrderService>();
        services.AddTransient<OrderControllerConstructor>();
        _serviceProvider = services.BuildServiceProvider();
    }

    // Simulates ASP.NET Core resolving the controller from the DI container,
    // which automatically satisfies the IOrderService constructor parameter.
    [Benchmark(Baseline = true)]
    public void ConstructorInjection()
    {
        var controller = _serviceProvider.GetRequiredService<OrderControllerConstructor>();
        controller.GetOrders().Consume(_consumer);
    }

    // Simulates action method injection: controller is newed up separately,
    // and IOrderService is resolved independently then passed to the action.
    [Benchmark]
    public void ActionMethodInjection()
    {
        var controller = new OrderControllerAction();
        var service = _serviceProvider.GetRequiredService<IOrderService>();
        controller.GetOrders(service).Consume(_consumer);
    }

    [GlobalCleanup]
    public void Cleanup() => _serviceProvider?.Dispose();
}
