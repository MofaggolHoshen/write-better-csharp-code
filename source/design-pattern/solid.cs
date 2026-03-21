// ============================================================
// SOLID Design Principles — C# Examples
// ============================================================
//
// ┌────────┬──────────────────────────────────────┬──────────────────────────────────────────────────┐
// │ Letter │ Principle                            │ One-line summary                                 │
// ├────────┼──────────────────────────────────────┼──────────────────────────────────────────────────┤
// │   S    │ Single Responsibility Principle (SRP) │ A class should have only one reason to change    │
// │   O    │ Open/Closed Principle          (OCP) │ Open for extension, closed for modification      │
// │   L    │ Liskov Substitution Principle  (LSP) │ Subtypes must be substitutable for their base    │
// │   I    │ Interface Segregation Principle(ISP) │ Don't force clients to depend on unused members  │
// │   D    │ Dependency Inversion Principle (DIP) │ Depend on abstractions, not concrete types       │
// └────────┴──────────────────────────────────────┴──────────────────────────────────────────────────┘

// ─────────────────────────────────────────────────────────────
// S — Single Responsibility Principle (SRP)
// A class should have only one reason to change.
// ─────────────────────────────────────────────────────────────

// BAD: one class handles both order logic AND persistence
class OrderServiceBad
{
    public void PlaceOrder(string item) { /* business logic */ }
    public void SaveToDatabase(string item) { /* DB concern mixed in */ }
}

// GOOD: responsibilities are separated
class OrderService
{
    private readonly OrderRepository _repo;

    public OrderService(OrderRepository repo) => _repo = repo;

    public void PlaceOrder(string item)
    {
        // pure business logic
        Console.WriteLine($"Order placed for: {item}");
        _repo.Save(item);
    }
}

class OrderRepository
{
    public void Save(string item) =>
        Console.WriteLine($"Saving '{item}' to database.");
}


// ─────────────────────────────────────────────────────────────
// O — Open/Closed Principle (OCP)
// Open for extension, closed for modification.
// ─────────────────────────────────────────────────────────────

abstract class Discount
{
    public abstract decimal Apply(decimal price);
}

class NoDiscount : Discount
{
    public override decimal Apply(decimal price) => price;
}

class PercentageDiscount : Discount
{
    private readonly decimal _percent;
    public PercentageDiscount(decimal percent) => _percent = percent;

    public override decimal Apply(decimal price) =>
        price - price * _percent / 100;
}

class SeasonalDiscount : Discount
{
    public override decimal Apply(decimal price) => price * 0.85m; // 15% off
}

class PriceCalculator
{
    // No modification needed when adding a new discount type
    public decimal Calculate(decimal price, Discount discount) =>
        discount.Apply(price);
}


// ─────────────────────────────────────────────────────────────
// L — Liskov Substitution Principle (LSP)
// Subtypes must be substitutable for their base types.
// ─────────────────────────────────────────────────────────────

abstract class Shape
{
    public abstract double Area();
}

class Rectangle : Shape
{
    public double Width { get; set; }
    public double Height { get; set; }

    public override double Area() => Width * Height;
}

class Circle : Shape
{
    public double Radius { get; set; }

    public override double Area() => Math.PI * Radius * Radius;
}

// Any Shape subtype works here — LSP satisfied
static class AreaPrinter
{
    public static void Print(Shape shape) =>
        Console.WriteLine($"Area: {shape.Area():F2}");
}


// ─────────────────────────────────────────────────────────────
// I — Interface Segregation Principle (ISP)
// Clients should not be forced to depend on interfaces they
// do not use.
// ─────────────────────────────────────────────────────────────

// BAD: one fat interface forces all implementors to define everything
interface IWorkerBad
{
    void Work();
    void Eat();
    void Sleep();
}

// GOOD: small, focused interfaces
interface IWorkable { void Work(); }
interface IEatable { void Eat(); }
interface ISleepable { void Sleep(); }

class HumanWorker : IWorkable, IEatable, ISleepable
{
    public void Work() => Console.WriteLine("Human working.");
    public void Eat() => Console.WriteLine("Human eating.");
    public void Sleep() => Console.WriteLine("Human sleeping.");
}

// A robot only implements what it actually does
class RobotWorker : IWorkable
{
    public void Work() => Console.WriteLine("Robot working.");
}


// ─────────────────────────────────────────────────────────────
// D — Dependency Inversion Principle (DIP)
// High-level modules should not depend on low-level modules.
// Both should depend on abstractions.
// ─────────────────────────────────────────────────────────────

interface IMessageSender
{
    void Send(string message);
}

class EmailSender : IMessageSender
{
    public void Send(string message) =>
        Console.WriteLine($"Email: {message}");
}

class SmsSender : IMessageSender
{
    public void Send(string message) =>
        Console.WriteLine($"SMS: {message}");
}

// High-level class depends on the abstraction, not a concrete sender
class NotificationService
{
    private readonly IMessageSender _sender;

    public NotificationService(IMessageSender sender) => _sender = sender;

    public void Notify(string message) => _sender.Send(message);
}


// ─────────────────────────────────────────────────────────────
// Usage
// ─────────────────────────────────────────────────────────────
var repo = new OrderRepository();
var orders = new OrderService(repo);
orders.PlaceOrder("Laptop");

var calculator = new PriceCalculator();
Console.WriteLine(calculator.Calculate(200m, new PercentageDiscount(10)));

AreaPrinter.Print(new Rectangle { Width = 4, Height = 5 });
AreaPrinter.Print(new Circle { Radius = 3 });

IWorkable robot = new RobotWorker();
robot.Work();

var notifier = new NotificationService(new EmailSender());
notifier.Notify("Your order has shipped!");

notifier = new NotificationService(new SmsSender());
notifier.Notify("One-time password: 4821");
