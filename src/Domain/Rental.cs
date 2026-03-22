namespace projekt_obiektowy.Domain;

public class Rental(User user, Hardware hardware, int daysToRent)
{
    public Guid Id { get; init; } = Guid.NewGuid();
    
    public User User { get; init; } = user;
    public Hardware Hardware { get; init; } = hardware;
    
    public DateTime RentDate { get; init; } = DateTime.Now;
    public DateTime DueDate { get; set; } = DateTime.Now.AddDays(daysToRent);
    
    public DateTime? ReturnDate { get; set; }

    public decimal Penalty { get; set; } = 0;
}