namespace projekt_obiektowy.Domain.Equipment;

public class Camera(string name, bool isDigital, int memoryGb) : Hardware(name)
{
    public bool IsDigital { get; set; } = isDigital;
    public int MemoryGb { get; set; } = memoryGb;
}