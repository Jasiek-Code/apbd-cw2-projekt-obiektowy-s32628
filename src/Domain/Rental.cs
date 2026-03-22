using projekt_obiektowy.Domain.Equipment;
using projekt_obiektowy.Domain.Users;

namespace projekt_obiektowy.Domain;

public class Rental(User user, Hardware hardware, int rentedForDays)
{
    public Guid Id { get; init; } = Guid.NewGuid();
    
    public User User { get; } = user;
    public Hardware Hardware { get; } = hardware;
    
    public int RentedForDays { get; } = rentedForDays;
    
    public DateTime RentDate { get; } = DateTime.Now;
    public DateTime DueDate { get; } = DateTime.Now.AddDays(rentedForDays);
    
    public DateTime? ReturnDate { get; set; }

    public decimal Penalty { get; set; } = 0;
}