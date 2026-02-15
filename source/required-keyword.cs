using System.Diagnostics.CodeAnalysis;

// ✅ Basic Example - User class with required property
public class User
{
    public required string Username { get; init; }
}

// ✅ User class with constructor and [SetsRequiredMembers] attribute
public class UserWithConstructor
{
    public required string Username { get; init; }

    [SetsRequiredMembers]
    public UserWithConstructor(string username)
    {
        Username = username;
    }
}

// 🚀 Example: DTO with multiple required properties
public class ProductDto
{
    public required string Name { get; init; }
    public required decimal Price { get; init; }
    public string? Description { get; init; } // Optional property
}

// Usage examples
public class RequiredKeywordExamples
{
    public static void Main(string[] args)
    {
        var examples = new RequiredKeywordExamples();

        Console.WriteLine("=== Basic Usage ===");
        examples.BasicUsage();

        Console.WriteLine("\n=== Constructor Usage ===");
        examples.ConstructorUsage();

        Console.WriteLine("\n=== DTO Usage ===");
        examples.DtoUsage();
    }

    public void BasicUsage()
    {
        // ❌ This would cause a compile-time error:
        // var user = new User();

        // ✔ Correct way - must initialize required properties
        var user = new User
        {
            Username = "mofaggol"
        };

        Console.WriteLine($"User: {user.Username}");
    }

    public void ConstructorUsage()
    {
        // ✔ Using constructor with [SetsRequiredMembers]
        var user = new UserWithConstructor("mofaggol");

        Console.WriteLine($"User: {user.Username}");
    }

    public void DtoUsage()
    {
        // ✔ Creating a DTO with required properties
        var product = new ProductDto
        {
            Name = "Laptop",
            Price = 999.99m,
            Description = "A powerful laptop" // Optional
        };

        // ✔ Description is optional, so this is also valid
        var anotherProduct = new ProductDto
        {
            Name = "Mouse",
            Price = 29.99m
        };

        Console.WriteLine($"Product: {product.Name}, Price: {product.Price}");
    }
}
