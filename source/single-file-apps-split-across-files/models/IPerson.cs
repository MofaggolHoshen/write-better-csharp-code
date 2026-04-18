namespace models;

interface IPerson
{
    string FirstName { get; set; }
    string LastName { get; set; }
    string FullName => $"{FirstName} {LastName}";
    DateOnly DateOfBirth { get; set; }
    string Email { get; set; }
}

