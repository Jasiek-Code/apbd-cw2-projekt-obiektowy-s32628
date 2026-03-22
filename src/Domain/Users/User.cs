namespace projekt_obiektowy.Domain.Users;

public abstract class User(string name, string surname)
{
    public Guid Id { get; } = Guid.NewGuid();
    public string Name { get; } = name;
    public string Surname { get; } = surname;

    public abstract int MaxRentals { get; }
}