namespace projekt_obiektowy.Domain.Users;

public class Student(string name, string surname) : User(name, surname)
{
    public override int MaxRentals => 2;
}