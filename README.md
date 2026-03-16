# Write Better C# Code

## Missing Strongly-Typed-ID Section

In many scenarios, using strongly-typed IDs can help enforce type safety and prevent errors associated with using primitive types. For example, instead of using an `int` for IDs, it's better to create a dedicated type for them:

```csharp
public class OrderId
{
    public int Value { get; }

    public OrderId(int value)
    {
        Value = value;
    }
}
```

## Fix Typo

In the previous documentation, the word "sigle" was incorrectly written and has now been corrected to "single".

## CompareStrings.cs Section

### Overview

The `CompareStrings.cs` class provides utility methods for string comparisons, allowing developers to perform comparisons in a more readable and maintainable way.

### Example Usage

```csharp
var comparisonResult = CompareStrings.Alphabetically("apple", "banana");
if (comparisonResult == 0)
{
    Console.WriteLine("Both strings are equal.");
}
```

This allows for a more human-readable approach to string comparisons, making your code more expressive.
