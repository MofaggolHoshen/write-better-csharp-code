# Write Better C# Code

A practical guide to clean, maintainable, and production-ready C# code. This repository demonstrates modern C# best practices, architectural patterns, performance optimization techniques, and real-world examples.

## 📚 Topics

### 🔥 Deconstruction - It's Not Just for Tuples

Learn how to use deconstruction beyond tuples in C#. Discover how it works with records, classes, structs, dictionaries, and pattern matching to write cleaner, more expressive code.

- **📖 Documentation:** [docs/deconstruction.md](docs/deconstruction.md)
- **💻 Code Example:** [source/deconstruction.cs](source/deconstruction.cs)
- **▶️ Run:** `cd source && dotnet run deconstruction.cs`

**What you'll learn:**
- Deconstruction with records (automatic support)
- Adding `Deconstruct` methods to classes and structs
- Dictionary iteration with deconstruction
- Pattern matching with deconstruction
- What doesn't work with deconstruction

---

### 🔐 C# 11 `required` Keyword

Learn how `required` enforces initialization of important properties, how constructors interact with it, and when to use `[SetsRequiredMembers]` to keep the compiler happy.

- **📖 Documentation:** [docs/required-keyword.md](docs/required-keyword.md)
- **💻 Code Example:** [source/required-keyword.cs](source/required-keyword.cs)
- **▶️ Run:** `cd source && dotnet run required-keyword.cs`

**What you'll learn:**
- Why `required` prevents partially initialized objects
- Object initializers vs. constructors
- Using `[SetsRequiredMembers]` correctly
- Practical DTO usage patterns

---

### 📦 Multiple NuGet Sources in Single-File Apps

See how to restore packages from nuget.org and a private feed in a single-file C# app using `#:property RestoreAdditionalProjectSources`.

- **📖 Documentation:** [docs/multiple-nuget-sources-in-sigle-file-app.md](docs/multiple-nuget-sources-in-sigle-file-app.md)
- **💻 Code Example:** [source/multiple-nuget-sources-in-sigle-file-app.cs](source/multiple-nuget-sources-in-sigle-file-app.cs)
- **▶️ Run:** `cd source && dotnet run multiple-nuget-sources-in-sigle-file-app.cs`

**What you'll learn:**
- Adding extra package sources in a single file
- Authenticating against private feeds
- When to use `nuget.config` instead

---

### 🏷️ Strongly Typed IDs Guide

Learn how to create robust, type-safe ID types that prevent bugs and improve code clarity. Discover patterns for implementing strongly typed IDs with value objects, records, and source generators.

- **📖 Documentation:** [docs/strongly-typed-id-guide.md](docs/strongly-typed-id-guide.md)
- **💻 Code Example:** [source/strongly-typed-id.cs](source/strongly-typed-id.cs)
- **▶️ Run:** `cd source && dotnet run strongly-typed-id.cs`

**What you'll learn:**
- Why primitive obsession is problematic
- Implementing strongly typed IDs with records
- Using source generators for ID types
- Performance considerations and best practices

---

### ⚡ DI: Constructor vs Action Method Injection Benchmark

Benchmark the memory and time overhead of constructor injection vs. `[FromServices]` action-method injection in ASP.NET Core controllers using BenchmarkDotNet.

- **📖 Documentation:** [docs/benchmark-di-constructor-vs-action-method.md](docs/benchmark-di-constructor-vs-action-method.md)
- **💻 Code Example:** [source/banchmark-di-method-controller.cs](source/banchmark-di-method-controller.cs)
- **▶️ Run:** `cd source && dotnet run --configuration Release banchmark-di-method-controller.cs`

**What you'll learn:**
- How constructor injection resolves dependencies once per request
- How `[FromServices]` resolves dependencies per action call
- DI resolution overhead measured with BenchmarkDotNet
- When to prefer each approach

---

### 🔀 Union Types in C# 15

Explore the new union types feature in C# 15 (.NET 11 preview). Learn how to declare a closed set of types and leverage exhaustive pattern matching enforced by the compiler.

- **💻 Code Example:** [source/union-type.cs](source/union-type.cs)
- **▶️ Run:** `cd source && dotnet run union-type.cs`

**What you'll learn:**
- What union types are and how they differ from interfaces/abstract classes
- Declaring unions and the compiler-enforced exhaustive switch
- Runtime polyfill usage before .NET 11 ships natively

---

### 🔤 String Comparison in .NET

Explore the different string comparison methods in .NET and understand when to use each one for correct, performant, and culture-aware results.

- **💻 Code Example:** [source/CompareStrings.cs](source/CompareStrings.cs)
- **▶️ Run:** `cd source && dotnet run CompareStrings.cs`

**What you'll learn:**
- `String.Compare` for ordering and sorting
- `String.CompareOrdinal` for culture-insensitive byte-level comparison
- `String.CompareTo` for instance-based comparisons
- Choosing the right `StringComparison` option (`Ordinal`, `CurrentCulture`, `OrdinalIgnoreCase`, etc.)

---

## 🚀 Getting Started

Each topic includes:
- **Documentation** (`docs/`) - Detailed explanations and best practices
- **Code Examples** (`source/`) - Runnable C# examples you can execute and modify

## 📖 How to Use This Repository

1. Browse topics above and choose what interests you
2. Read the documentation in the `docs/` folder
3. Study the code examples in the `source/` folder
4. Run the examples using `dotnet run <filename>.cs`
5. Experiment and modify the code to deepen your understanding

## 🎯 Contributing

More topics coming soon! Stay tuned for updates on:
- LINQ best practices
- Async/await patterns
- Dependency injection
- Performance optimization
- And much more...