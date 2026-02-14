# 🔥 Deconstruction in C# --- It's Not Just for Tuples

Most developers use deconstruction like this:

``` csharp
var (_, _, lastName) = GetFirstMiddleLastName();
```

But here's what many don't realize 👇

Deconstruction in C# works with much more than just tuples.

------------------------------------------------------------------------

## ✅ It works with:

### 1️⃣ Records (automatic support)

``` csharp
public record Person(string FirstName, string LastName);

var (first, last) = person;
```

### 2️⃣ Any class/struct with a `Deconstruct` method

``` csharp
public void Deconstruct(out string first, out string last)
{
    first = FirstName;
    last = LastName;
}
```

### 3️⃣ Dictionary iteration

``` csharp
foreach (var (key, value) in dictionary)
{
    Console.WriteLine($"{key} - {value}");
}
```

### 4️⃣ Pattern matching

``` csharp
if (person is Person(var first, var last))
{
    Console.WriteLine(first);
}
```

------------------------------------------------------------------------

## ❌ It does NOT work with:

-   Regular classes without `Deconstruct`
-   Arrays
-   Lists
-   Anonymous objects

------------------------------------------------------------------------

## 💡 Why this matters

-   Cleaner method returns\
-   More readable DTO handling\
-   Less boilerplate code\
-   More expressive modern C#

Modern C# is designed for clarity and intent.

------------------------------------------------------------------------

👉 Are you using deconstruction beyond tuples in your projects?
