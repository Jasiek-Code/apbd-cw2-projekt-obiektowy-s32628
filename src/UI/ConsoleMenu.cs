using projekt_obiektowy.Domain.Equipment;
using projekt_obiektowy.Domain.Users;
using projekt_obiektowy.Services;

namespace projekt_obiektowy.UI;

public static class ConsoleMenu
{
    public static void Run(IRentalService rentalService)
    {
        rentalService.LoadData();
        
        // objects for testing
        var student = new Student("Jan", "Kowalski");
        var laptop = new Laptop("Dell XPS 15", "Intel i7", 16);
        var projector = new Projector("Epson 1080p", 3000, "1920x1080");
        
        var isRunning = true;
        while (isRunning)
        {
            Console.Clear();
            
            Console.WriteLine("-------------------------------");
            Console.WriteLine("1. Show entire rental database history");
            Console.WriteLine("2. Show all active rentals");
            Console.WriteLine("3. Show finished rentals with no penalty");
            Console.WriteLine("4. Show finished rentals with penalty");
            Console.WriteLine("5. Rent Laptop to Student");
            Console.WriteLine("6. Return Laptop");
            Console.WriteLine("7. (TEST) Rent Projector & Return it late");
            Console.WriteLine("8. Exit");
            Console.WriteLine("-------------------------------");
            Console.Write("Choose an action: ");
            
            var choice = Console.ReadLine();
            Console.WriteLine();

            try
            {
                switch (choice)
                {
                    case "1":
                    {
                        Console.Clear();
                        Console.WriteLine("--- RENTAL DATABASE HISTORY ---\n");

                        var count = rentalService.Rentals.Count;

                        if (count == 0)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.WriteLine("[Info] DATABASE IS EMPTY.");
                            Console.ResetColor();
                            break; 
                        }
                        
                        Console.WriteLine($"   Client {null,-6} Rental {null,-8} Status {null, -3} Penalty(PLN) {null,-7} Date");
                        foreach (var rental in rentalService.Rentals)
                        {
                            var status = rental.ReturnDate == null ? "(active)" : "(returned)";
                            Console.WriteLine($"{rental.User.Name} {rental.User.Surname,-10} {rental.Hardware.Name,-15} {status,-16} {rental.Penalty, -10} {rental.RentDate:dd.MM.yyyy HH:mm}");
                        }
                        break;
                    }
                    
                    case "2":
                    {
                        Console.Clear();
                        Console.WriteLine("--- ACTIVE RENTALS ---\n");
                        
                        var activeRentals = rentalService.Rentals
                            .Where(r => r.ReturnDate == null)
                            .ToList();

                        if (activeRentals.Count == 0)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.WriteLine("[Info] No active rentals found.");
                            Console.ResetColor();
                            break;
                        }
                        
                        foreach (var rental in activeRentals)
                        {
                            Console.WriteLine(
                                $"{rental.User.Name} rented {rental.Hardware.Name} for {rental.RentedForDays} days and has {Math.Round((rental.DueDate - DateTime.Now).TotalDays, 1)} days to return it.");
                        }

                        break;
                    }

                    case "3":
                    {
                        Console.Clear();
                        Console.WriteLine("--- FINISHED RENTALS ON TIME ---\n");
                        
                        var finishedRentalsNoPenalty = rentalService.Rentals
                            .Where(r => r is { ReturnDate: not null, Penalty: 0 })
                            .ToList();

                        if (finishedRentalsNoPenalty.Count == 0)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.WriteLine("[Info] No finished rentals with no penalty found.");
                            Console.ResetColor();
                            break;
                        }
                        
                        foreach (var rental in finishedRentalsNoPenalty)
                        {
                            Console.WriteLine(
                                $"{rental.User.Name} returned {rental.Hardware.Name} on time. Penalty: {rental.Penalty} PLN");
                        }

                        break;
                    }

                    case "4":
                    {
                        Console.Clear();
                        Console.WriteLine("--- LATE FINISHED RENTALS ---\n");

                        var activeRentalsWithPenalty = rentalService.Rentals
                            .Where(r => r is { ReturnDate: not null, Penalty: > 0 })
                            .ToList();

                        if (activeRentalsWithPenalty.Count == 0)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.WriteLine("[Info] No finished rentals with penalty found.");
                            Console.ResetColor();
                            break;
                        }
                        
                        foreach (var rental in activeRentalsWithPenalty)
                        {
                            Console.WriteLine(
                                $"{rental.User.Name} returned {rental.Hardware.Name} late. Penalty applied: {rental.Penalty} PLN");
                        }

                        break;
                    }

                    case "5":
                    {
                        Console.Clear();
                        Console.WriteLine("--- RENT LAPTOP ---");

                        try
                        {
                            rentalService.Rent(student, laptop, 7);
                            
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            Console.WriteLine("\n[Success] Laptop successfully rented to Student for 7 days.");
                            Console.ResetColor();
                        }
                        catch (Exception e)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"\n[Error] Could not rent laptop: {e.Message}");
                            Console.ResetColor();
                        }
                        

                        break;
                    }

                    case "6":
                    {
                        Console.Clear();
                        
                        var activeLaptopRental = rentalService.Rentals
                            .FirstOrDefault(r => r.ReturnDate == null && r.Hardware is Laptop);
                        
                        if (activeLaptopRental != null)
                        {
                            rentalService.Return(activeLaptopRental.Hardware.Id, DateTime.Now);
                            
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            Console.WriteLine("[Success] Laptop returned");
                            Console.ResetColor();
                            break;
                        }

                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.WriteLine("[Info] No active laptop rentals found to return.");
                        Console.ResetColor();
                        
                        break;
                    }

                    case "7":
                    {
                        Console.Clear();

                        try
                        {
                            rentalService.Rent(student, projector, 7);
                            
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            Console.WriteLine("\n[Success] Projector successfully rented to Student for 7 days.");
                            Console.ResetColor();
                        }
                        catch (Exception e)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"\n[Error] Could not rent Projector: {e.Message}");
                            Console.ResetColor();
                            break;
                        }
                        
                        
                        var lateDate = DateTime.Now.AddDays(30); 
                        rentalService.Return(projector.Id, lateDate);
                        
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.WriteLine("[Success] Projector returned");
                        Console.ResetColor();
                        
                        break;
                    }

                    case "8": isRunning = false; break;

                    default:
                        Console.Clear();
                        
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }
            catch (Exception e)
            {
                Console.Clear();
                
                Console.WriteLine($"[Error] {e.Message}");
            }

            if (isRunning) 
            {
                rentalService.SaveData();
                
                Console.WriteLine("\nPress Enter to continue...");
                Console.ReadLine(); 
            }
        }
    }
}