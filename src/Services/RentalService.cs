using projekt_obiektowy.Domain;
using projekt_obiektowy.Domain.Equipment;
using projekt_obiektowy.Domain.Users;
using System.Text.Json;

namespace projekt_obiektowy.Services;

public class RentalService : IRentalService
{
    private readonly List<Rental> _activeRentals = [];
    
    public IReadOnlyList<Rental> Rentals => _activeRentals.AsReadOnly();

    public void Rent(User user, Hardware hardware, int daysToRent)
    {
        var activeRentalsForUser = _activeRentals.Count(
            rental => rental.User.Equals(user) 
                      && rental.ReturnDate == null);

        if (activeRentalsForUser >= user.MaxRentals)
        {
            throw new Exception($"Rental limit for {user.Name} {user.Surname} reached.");
        }

        if (!hardware.IsAvailable)
        {
            throw new Exception($"Hardware {hardware.Name} is not available.");
        }
        
        hardware.IsAvailable = false;
        _activeRentals.Add(new Rental(user, hardware, daysToRent));
    }

    public void Return(Guid hardwareId, DateTime? returnDate = null)
    {
        var userRental = _activeRentals.FirstOrDefault(
            rental => rental.Hardware.Id == hardwareId 
                      && rental.ReturnDate == null);

        if (userRental is null)
        {
            throw new Exception($"No active rental found for hardware ID {hardwareId}.");
        }
        
        var actualReturnDate = returnDate ?? DateTime.Now;
        
        var dueDate = userRental.DueDate;
        
        if (actualReturnDate > dueDate)
        {
            var delay = actualReturnDate - dueDate;
            var daysLate = delay.Days;
            
            var penalty = CalculatePenalty(daysLate);
            
            userRental.Penalty = penalty;
        }
        
        userRental.ReturnDate = actualReturnDate;
        userRental.Hardware.IsAvailable = true;
    }

    private static decimal CalculatePenalty(int daysLate)
    {
        return daysLate == 0 ? 0 : daysLate * 10m;
    }
    
    public void SaveData(string filePath = "/Data/rentals.json")
    {
        Directory.CreateDirectory("Data"); 
        
        var options = new JsonSerializerOptions { WriteIndented = true };
        var jsonString = JsonSerializer.Serialize(_activeRentals, options);
        File.WriteAllText(filePath, jsonString);
    }

    public void LoadData(string filePath = "/Data/rentals.json")
    {
        if (!File.Exists(filePath)) return;
        
        var jsonString = File.ReadAllText(filePath);
        
        if (string.IsNullOrWhiteSpace(jsonString)) return;

        var loadedRentals = JsonSerializer.Deserialize<List<Rental>>(jsonString);

        if (loadedRentals == null) return;
        _activeRentals.Clear();
        _activeRentals.AddRange(loadedRentals);
    }
}