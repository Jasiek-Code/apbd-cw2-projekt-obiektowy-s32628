namespace projekt_obiektowy;

public abstract class User(string name, string surname)
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Name { get; set; } = name;
    public string Surname { get; set; } = surname;

    public abstract int MaxRentals { get; }
}

public class Student(string name, string surname) : User(name, surname)
{
    public override int MaxRentals => 2;
}

public class Employee(string name, string surname) : User(name, surname)
{
    public override int MaxRentals => 5;
}