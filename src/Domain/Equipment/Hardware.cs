namespace projekt_obiektowy.Domain.Equipment;

public abstract class Hardware(string name)
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Name { get; set; } = name;

    public bool IsAvailable { get; set; } = true;
}