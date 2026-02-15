# 🔐 C# 11 `required` Keyword Explained

C# 11 introduced the `required` keyword to ensure that a property **must
be initialized** during object creation.

It helps prevent incomplete objects and reduces runtime bugs.

------------------------------------------------------------------------

## ✅ Basic Example

``` csharp
public class User
{
    public required string Username { get; init; }
}
```

Now, when creating an object:

``` csharp
var user = new User(); // ❌ Compile-time error
```

You'll get a compile-time error because `Username` is required.

✔ Correct way:

``` csharp
var user = new User
{
    Username = "mofaggol"
};
```

------------------------------------------------------------------------

## ⚠ What if I assign `Username` inside constructor?

``` csharp
public class User
{
    public required string Username { get; init; }

    public User(string username)
    {
        Username = username;
    }
}
```

If you create:

``` csharp
var user = new User("mofaggol");
```

You might still see a warning like:

> Required member 'Username' must be set in the object initializer.

### 🤔 Why?

Because the compiler doesn't automatically assume that your constructor
satisfies the `required` contract.

------------------------------------------------------------------------

## ✅ How to Fix the Warning

Use the `[SetsRequiredMembers]` attribute on the constructor.

``` csharp
using System.Diagnostics.CodeAnalysis;

public class User
{
    public required string Username { get; init; }

    [SetsRequiredMembers]
    public User(string username)
    {
        Username = username;
    }
}
```

Now the warning is gone ✔

You're explicitly telling the compiler:

👉 "This constructor properly initializes all required members."

------------------------------------------------------------------------

## 🚀 Why `required` is Useful?

-   Prevents partially initialized objects\
-   Improves code safety\
-   Makes intent clear\
-   Great for DTOs and immutable models

------------------------------------------------------------------------

#️⃣ C# keeps getting better!
