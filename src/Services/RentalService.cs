using projekt_obiektowy.Domain;
using projekt_obiektowy.Domain.Equipment;
using projekt_obiektowy.Domain.Users;

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

    public void Return(Hardware hardware)
    {
        var userRental = _activeRentals.FirstOrDefault(
            rental => rental.Hardware.Equals(hardware)
                      && rental.ReturnDate == null);

        if (userRental is null)
        {
            throw new Exception($"No user rental found for hardware {hardware}");
        }

        var actualReturnDate = DateTime.Now;
        var dueDate = userRental.DueDate;
        
        if (actualReturnDate > dueDate)
        {
            var delay = actualReturnDate - dueDate;
            var daysLate = delay.Days;
            
            var penalty = CalculatePenalty(daysLate);
            
            userRental.Penalty = penalty;
        }
        
        userRental.ReturnDate = actualReturnDate;
        hardware.IsAvailable = true;
    }

    private static decimal CalculatePenalty(int daysLate)
    {
        return daysLate == 0 ? 0 : daysLate * 10m;
    }
}