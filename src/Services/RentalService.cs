using projekt_obiektowy.Domain;

namespace projekt_obiektowy.Services;

public class RentalService : IRentalService
{
    private readonly List<Rental> _activeRentals = [];

    public void Rent(User user, Hardware hardware, int daysToRent)
    {
        var activeRentalsForUser = _activeRentals.Count(rental => rental.User.Equals(user));

        if (activeRentalsForUser < user.MaxRentals)
        {
            throw new Exception("Rental limit for " +  user.Name + " " + user.Surname + " reached");
        }

        if (!hardware.IsAvailable)
        {
            throw new Exception("Hardware not available");
        }
        
        _activeRentals.Add(new Rental(user, hardware, daysToRent));
    }

    public void Return(Hardware hardware)
    {
        
    }
}