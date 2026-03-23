using projekt_obiektowy.Domain;
using projekt_obiektowy.Domain.Equipment;
using projekt_obiektowy.Domain.Users;

namespace projekt_obiektowy.Services;

public interface IRentalService
{
    IReadOnlyList<Rental> Rentals { get; }
    
    void Rent(User user, Hardware hardware, int daysToRent);
    void Return(Guid hardwareId, DateTime? returnDate = null);
    
    void SaveData(string filePath = "Data/rentals.json");
    void LoadData(string filePath = "Data/rentals.json");
}