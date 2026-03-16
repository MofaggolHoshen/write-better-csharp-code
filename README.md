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

- **📖 Documentation:** [docs/multiple-nuget-sources-in-single-file-app.md](docs/multiple-nuget-sources-in-single-file-app.md)
- **💻 Code Example:** [source/multiple-nuget-sources-in-single-file-app.cs](source/multiple-nuget-sources-in-single-file-app.cs)
- **▶�� Run:** `cd source && dotnet run multiple-nuget-sources-in-single-file-app.cs`

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

### ⚡ Blazor vs React

Choosing between Blazor and React for your next web project? This guide compares both frameworks across language, ecosystem, component model, state management, performance, and developer experience — helping you make the right call for your team.

- **📖 Documentation:** [docs/blazor-vs-react.md](docs/blazor-vs-react.md)

**What you'll learn:**
- When to choose Blazor vs React
- Hosting models: Blazor Server, WebAssembly, and Auto
- Component model, state management, and interop patterns
- Side-by-side code comparisons (C# Razor vs TypeScript JSX)
- Tooling, testing, and ecosystem differences

---

### 🔤 String Comparison Best Practices

Master the art of string comparison in C#. Learn about different comparison methods, culture-sensitive operations, and performance implications.

- **💻 Code Example:** [source/CompareStrings.cs](source/CompareStrings.cs)
- **▶️ Run:** `cd source && dotnet run CompareStrings.cs`

**What you'll learn:**
- Ordinal vs. culture-sensitive comparisons
- Case-sensitive and case-insensitive operations
- Performance characteristics of different methods
- When to use StringComparison enumeration

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