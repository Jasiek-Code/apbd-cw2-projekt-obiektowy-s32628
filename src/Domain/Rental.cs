using System.Text.Json.Serialization;
using projekt_obiektowy.Domain.Equipment;
using projekt_obiektowy.Domain.Users;

namespace projekt_obiektowy.Domain;

public class Rental(User user, Hardware hardware, int rentedForDays)
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public User User { get; } = user;
    public Hardware Hardware { get; } = hardware;
    public int RentedForDays { get; } = rentedForDays;
    
    [JsonInclude]
    public decimal Penalty { get; private set; }
    
    public DateTime RentDate { get; init; } = DateTime.Now;
    public DateTime DueDate => RentDate.AddDays(RentedForDays);
    public DateTime? ReturnDate { get; set; }
    
    private const decimal DailyPenaltyRate = 10m; 
    public void CalculateAndSetPenalty(DateTime actualReturnDate)
    {
        if (actualReturnDate <= DueDate)
        {
            Penalty = 0;
            return;
        }

        var daysLate = (actualReturnDate - DueDate).Days;
        Penalty = daysLate * DailyPenaltyRate;
    }

}