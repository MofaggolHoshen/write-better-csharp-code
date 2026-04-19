
// How to run:
//   dotnet run app.cs
//
// Requirements:
//   - .NET 10 SDK or later (file-based programs with #:include support)
//   - Run from the directory containing app.cs:
//       cd source/single-file-apps-split-across-files/app

#:property ExperimentalFileBasedProgramEnableIncludeDirective=true

#:include ../models/IPerson.cs
#:include ../models/Teacher.cs
#:include ../models/Student.cs
#:include ../services/IPersonRepository.cs
#:include ../services/PersonRepository.cs
using models;
using services;

var repo = new PersonRepository();

// Create
repo.Add(new Teacher { FirstName = "John", LastName = "Doe", DateOfBirth = new DateOnly(1980, 1, 1), Email = "john.doe@example.com" });
repo.Add(new Student { FirstName = "Jane", LastName = "Smith", DateOfBirth = new DateOnly(2000, 5, 15), Email = "jane.smith@example.com" });

// Read all
Console.WriteLine("=== All People ===");
foreach (var person in repo.GetAll())
{
    Console.WriteLine($"{person.FullName} - ({person.Email})");
}

// Read by email
Console.WriteLine("\n=== Get by Email ===");
var found = repo.GetByEmail("john.doe@example.com");
Console.WriteLine(found is not null ? $"Found: {found.FullName}" : "Not found");

// Update
Console.WriteLine("\n=== After Update ===");
repo.Update("jane.smith@example.com", new Student
{
    FirstName = "Jane",
    LastName = "Johnson",
    DateOfBirth = new DateOnly(2000, 5, 15),
    Email = "jane.johnson@example.com"
});
foreach (var person in repo.GetAll())
{
    Console.WriteLine($"{person.FullName} - ({person.Email})");
}

// Delete
Console.WriteLine("\n=== After Delete ===");
repo.Delete("john.doe@example.com");
foreach (var person in repo.GetAll())
{
    Console.WriteLine($"{person.FullName} - ({person.Email})");
}

