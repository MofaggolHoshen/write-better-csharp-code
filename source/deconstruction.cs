using System;
using System.Collections.Generic;

namespace WriteBetterCSharpCode;

// ✅ Example 1: Records (automatic support)
public record Person(string FirstName, string LastName);

// ✅ Example 2: Class with Deconstruct method
public class Employee
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Department { get; set; }
    
    public Employee(string firstName, string lastName, string department)
    {
        FirstName = firstName;
        LastName = lastName;
        Department = department;
    }
    
    // Deconstruct into 2 values
    public void Deconstruct(out string first, out string last)
    {
        first = FirstName;
        last = LastName;
    }
    
    // Deconstruct into 3 values (overload)
    public void Deconstruct(out string first, out string last, out string dept)
    {
        first = FirstName;
        last = LastName;
        dept = Department;
    }
}

// ✅ Example 3: Struct with Deconstruct method
public struct Point
{
    public int X { get; set; }
    public int Y { get; set; }
    
    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }
    
    public void Deconstruct(out int x, out int y)
    {
        x = X;
        y = Y;
    }
}

public class DeconstructionExamples
{
    public static void Main()
    {
        Console.WriteLine("=== Deconstruction in C# Examples ===\n");
        
        // Example 1: Tuples (most common use)
        Console.WriteLine("1. Tuple Deconstruction:");
        var (firstName, middleName, lastName) = GetFirstMiddleLastName();
        Console.WriteLine($"   Name: {firstName} {middleName} {lastName}");
        
        // Discarding values with _
        var (_, _, last) = GetFirstMiddleLastName();
        Console.WriteLine($"   Last name only: {last}\n");
        
        // Example 2: Records (automatic support)
        Console.WriteLine("2. Record Deconstruction:");
        var person = new Person("John", "Doe");
        var (first, personLast) = person;
        Console.WriteLine($"   Person: {first} {personLast}\n");
        
        // Example 3: Class with Deconstruct method
        Console.WriteLine("3. Class Deconstruction:");
        var employee = new Employee("Jane", "Smith", "Engineering");
        
        // Using 2-value deconstruct
        var (empFirst, empLast) = employee;
        Console.WriteLine($"   Employee: {empFirst} {empLast}");
        
        // Using 3-value deconstruct
        var (empFirst2, empLast2, dept) = employee;
        Console.WriteLine($"   Employee with dept: {empFirst2} {empLast2} - {dept}\n");
        
        // Example 4: Struct with Deconstruct method
        Console.WriteLine("4. Struct Deconstruction:");
        var point = new Point(10, 20);
        var (x, y) = point;
        Console.WriteLine($"   Point: X={x}, Y={y}\n");
        
        // Example 5: Dictionary iteration
        Console.WriteLine("5. Dictionary Deconstruction:");
        var dictionary = new Dictionary<string, int>
        {
            ["Apple"] = 5,
            ["Banana"] = 3,
            ["Orange"] = 7
        };
        
        foreach (var (key, value) in dictionary)
        {
            Console.WriteLine($"   {key} - {value}");
        }
        Console.WriteLine();
        
        // Example 6: Pattern matching with deconstruction
        Console.WriteLine("6. Pattern Matching with Deconstruction:");
        var testPerson = new Person("Alice", "Johnson");
        
        if (testPerson is Person(var firstMatch, var lastMatch))
        {
            Console.WriteLine($"   Matched person: {firstMatch} {lastMatch}");
        }
        
        // Switch expression with deconstruction
        var greeting = testPerson switch
        {
            Person("Alice", var l) => $"Hello Alice {l}!",
            Person(var f, "Johnson") => $"Hello {f} from the Johnson family!",
            Person(var f, var l) => $"Hello {f} {l}!",
            _ => "Hello stranger!"
        };
        Console.WriteLine($"   {greeting}\n");
        
        // Example 7: Nested deconstruction
        Console.WriteLine("7. Nested Deconstruction:");
        var personWithPoint = (new Person("Bob", "Wilson"), new Point(100, 200));
        var ((pFirst, pLast), (px, py)) = personWithPoint;
        Console.WriteLine($"   Person: {pFirst} {pLast}, Location: ({px}, {py})\n");
        
        // ❌ Examples that DON'T work
        Console.WriteLine("=== What Does NOT Work ===");
        Console.WriteLine("- Regular classes without Deconstruct method");
        Console.WriteLine("- Arrays (cannot deconstruct array[0], array[1])");
        Console.WriteLine("- Lists (cannot deconstruct list[0], list[1])");
        Console.WriteLine("- Anonymous objects (no Deconstruct support)");
    }
    
    private static (string, string, string) GetFirstMiddleLastName()
    {
        return ("John", "Michael", "Doe");
    }
}
