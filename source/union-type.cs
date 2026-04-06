// File-based program directives — enable C# 15 preview on .NET 11
#:property LangVersion=preview
#:property TargetFramework=net11.0

// ============================================================
// Union Types in C# 15 (.NET 11 Preview 2+)
// Source: https://devblogs.microsoft.com/dotnet/csharp-15-union-types/
// Run:    dotnet run .\union-type.cs
// ============================================================
//
// WHAT ARE UNION TYPES?
// A union declares a closed set of types that a variable can hold.
// The compiler enforces exhaustive pattern matching — no catch-all needed.
// Unlike object/interface/abstract class approaches, unions:
//   - Restrict to a fixed set of types (compiler-verified)
//   - Don't require the types to share a common ancestor
//   - Enable exhaustive switch expressions without a discard (_) arm
// ============================================================
// NOTE: In file-based programs, all executable top-level statements must
//       appear BEFORE any type or namespace declarations.
// ============================================================

// ============================================================
// EXAMPLE 1 — Basic union declaration
// ============================================================
// Pet can hold a Cat, Dog, or Bird — implicit conversion from each case type.

Pet pet = new Dog("Rex");
Console.WriteLine(pet.Value);           // Dog { Name = Rex }

Pet pet2 = new Cat("Whiskers");
Console.WriteLine(pet2.Value);          // Cat { Name = Whiskers }

// Exhaustive switch — no discard (_) arm needed; all cases are covered
string petName = pet switch
{
    Dog d => d.Name,
    Cat c => c.Name,
    Bird b => b.Name,
};
Console.WriteLine(petName);             // Rex

// ============================================================
// EXAMPLE 2 — Handling the default (null) value
// ============================================================
// The default value of a union struct has Value == null.
// When the union may be default, add a null arm for exhaustiveness.

Pet defaultPet = default;

string description = defaultPet switch
{
    Dog d => d.Name,
    Cat c => c.Name,
    Bird b => b.Name,
    null => "no pet",
};
Console.WriteLine(description);         // no pet

// ============================================================
// EXAMPLE 3 — is operator with union types
// ============================================================

Pet myPet = new Cat("Luna");

if (myPet is Cat cat)
    Console.WriteLine($"It's a cat named {cat.Name}");  // It's a cat named Luna

// ============================================================
// EXAMPLE 4 — Generic union with body members
// OneOrMore<T>: accepts a single item OR a collection.
// ============================================================

OneOrMore<string> singleTag = "dotnet";
OneOrMore<string> manyTags = new[] { "csharp", "unions", "preview" };

foreach (var tag in singleTag.AsEnumerable())
    Console.Write($"[{tag}] ");
Console.WriteLine();                    // [dotnet]

foreach (var tag in manyTags.AsEnumerable())
    Console.Write($"[{tag}] ");
Console.WriteLine();                    // [csharp] [unions] [preview]

// ============================================================
// EXAMPLE 5 — Result-like union (success or error)
// ============================================================

Result r1 = Divide(10, 2);
Result r2 = Divide(5, 0);

Console.WriteLine(r1 switch
{
    Success s => s.Message,
    Failure f => $"Error: {f.Error}",
});                                     // 10 / 2 = 5

Console.WriteLine(r2 switch
{
    Success s => s.Message,
    Failure f => $"Error: {f.Error}",
});                                     // Error: Division by zero

// Local function — stays in the top-level section
Result Divide(int a, int b) =>
    b == 0
        ? new Failure("Division by zero")
        : new Success($"{a} / {b} = {a / b}");

// ============================================================
// TYPE DECLARATIONS  (must follow all top-level statements)
// ============================================================

// -- Runtime polyfill: required until .NET 11 ships these types natively --
namespace System.Runtime.CompilerServices
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false)]
    public sealed class UnionAttribute : Attribute;

    public interface IUnion
    {
        object? Value { get; }
    }
}

// Example 1 — case types & union
public record class Cat(string Name);
public record class Dog(string Name);
public record class Bird(string Name);

public union Pet(Cat, Dog, Bird);

// Example 4 — generic union with a body member
public union OneOrMore<T>(T, IEnumerable<T>)
{
    public IEnumerable<T> AsEnumerable() => Value switch
    {
        T single => [single],
        IEnumerable<T> many => many,
        null => [],
    };
}

// Example 5 — result union
public record class Success(string Message);
public record class Failure(string Error);

public union Result(Success, Failure);

// ============================================================
// KEY POINTS
// ============================================================
// - Declare:   public union MyUnion(TypeA, TypeB, TypeC);
// - Assign:    MyUnion x = new TypeA(...);   // implicit conversion
// - Match:     x switch { TypeA a => ..., TypeB b => ..., TypeC c => ... }
// - No discard arm needed when union is non-null & all cases are covered
// - Add a null arm when the union may be default or contains nullable types
// - Use 'is' for single-type checks: if (x is TypeA a) { ... }
// - Add a body { } to include helper methods alongside the case types
// - Available: .NET 11 Preview 2+ with <LangVersion>preview</LangVersion>
