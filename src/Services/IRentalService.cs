using projekt_obiektowy.Domain;
using projekt_obiektowy.Domain.Equipment;
using projekt_obiektowy.Domain.Users;

namespace projekt_obiektowy.Services;

public interface IRentalService
{
    void Rent(User user, Hardware hardware, int daysToRent);
    void Return(Hardware hardware);
}