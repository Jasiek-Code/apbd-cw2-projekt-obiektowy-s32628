namespace projekt_obiektowy.Domain.Users;

public class Employee(string name, string surname) : User(name, surname)
{
    public override int MaxRentals => 5;
}