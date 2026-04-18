# Single-File Apps Split Across Files

In .NET, you can write a complete application in a single `.cs` file using [file-based programs](https://learn.microsoft.com/en-us/dotnet/core/tutorials/top-level-programs). However, as your app grows, a single file can become hard to manage. The `#:include` directive lets you split the code into multiple files while still running it as a single-file app with `dotnet run`.

## The Experimental Feature Error

When you first add a `#:include` directive, you will encounter this error:

```
app.cs(1): error: This is an experimental feature, set MSBuild property
'ExperimentalFileBasedProgramEnableIncludeDirective' to 'true' to enable it.
```

This happens because `#:include` is an **experimental** feature in .NET and is disabled by default. You must explicitly opt in.

### Fix: Use the `#:property` Directive

Add the following line at the very top of your entry-point `.cs` file, **before** any `#:include` directives:

```csharp
#:property ExperimentalFileBasedProgramEnableIncludeDirective=true
```

This sets the MSBuild property inline without needing a `.csproj` file, which is the whole point of file-based programs.

## Project Structure

```
single-file-apps-split-across-files/
├── app/
│   └── app.cs               ← entry point, run with: dotnet run app.cs
├── models/
│   ├── IPerson.cs
│   ├── Student.cs
│   └── Teacher.cs
└── services/
    ├── IPersonRepository.cs
    └── PersonRepository.cs
```

## Including Files

Use `#:include` to pull in other `.cs` files relative to the entry-point file:

```csharp
#:property ExperimentalFileBasedProgramEnableIncludeDirective=true

#:include ../models/IPerson.cs
#:include ../models/Teacher.cs
#:include ../models/Student.cs
#:include ../services/IPersonRepository.cs
#:include ../services/PersonRepository.cs
using models;
using services;
```

> **Important:** `#:property` and `#:include` directives must appear before any `using` statements or executable code.

## Models

**`IPerson`** defines the shared contract:

```csharp
namespace models;

interface IPerson
{
    string FirstName { get; set; }
    string LastName { get; set; }
    string FullName => $"{FirstName} {LastName}";
    DateOnly DateOfBirth { get; set; }
    string Email { get; set; }
}
```

**`Student`** and **`Teacher`** both implement `IPerson`.

## Repository Service

`PersonRepository` is an in-memory CRUD service keyed on `Email` (case-insensitive). It implements `IPersonRepository`:

```csharp
interface IPersonRepository
{
    void Add(IPerson person);
    IPerson? GetByEmail(string email);
    IEnumerable<IPerson> GetAll();
    bool Update(string email, IPerson updated);
    bool Delete(string email);
}
```

### Usage

```csharp
var repo = new PersonRepository();

// Create
repo.Add(new Teacher { FirstName = "John", LastName = "Doe", DateOfBirth = new DateOnly(1980, 1, 1), Email = "john.doe@example.com" });
repo.Add(new Student { FirstName = "Jane", LastName = "Smith", DateOfBirth = new DateOnly(2000, 5, 15), Email = "jane.smith@example.com" });

// Read all
foreach (var person in repo.GetAll())
    Console.WriteLine($"{person.FullName} - ({person.Email})");

// Read by email
var found = repo.GetByEmail("john.doe@example.com");

// Update (supports changing the email key)
repo.Update("jane.smith@example.com", new Student
{
    FirstName = "Jane", LastName = "Johnson",
    DateOfBirth = new DateOnly(2000, 5, 15),
    Email = "jane.johnson@example.com"
});

// Delete
repo.Delete("john.doe@example.com");
```

## Running the App

From the `app/` directory:

```bash
dotnet run app.cs
```

No `.csproj`, no `Program.cs`, no build step — just run the file directly.
