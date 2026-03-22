using projekt_obiektowy.Domain;

namespace projekt_obiektowy.Services;

public interface IRentalService
{
    void Rent(User user, Hardware hardware, int daysToRent);
    void Return(Hardware hardware);
}