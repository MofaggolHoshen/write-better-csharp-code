namespace models;

public class Student : IPerson
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public string Email { get; set; }
}