using projekt_obiektowy.Domain.Equipment;
using projekt_obiektowy.Domain.Users;
using projekt_obiektowy.Services;

namespace projekt_obiektowy.UI;

public static class ConsoleMenu
{
    public static void Run(IRentalService rentalService)
    {
        Console.WriteLine("------Renting hardware system-------");
        
        rentalService.LoadData();
        Console.WriteLine($"[RentalService] Loaded {rentalService.Rentals.Count} previous rentals from json.\n");
        
        // objects for testing
        var student = new Student("Jan", "Kowalski");
        var laptop = new Laptop("Dell XPS 15", "Intel i7", 16);
        var projector = new Projector("Epson 1080p", 3000, "1920x1080");
        
        var isRunning = true;
        while (isRunning)
        {
            Console.Clear();
            
            Console.WriteLine("\n-------------------------------");
            Console.WriteLine("1. Show all active rentals");
            Console.WriteLine("2. Show finished rentals with no penalty");
            Console.WriteLine("3. Show finishes rentals with penalty");
            Console.WriteLine("4. Rent Laptop to Student");
            Console.WriteLine("5. Return Laptop");
            Console.WriteLine("6. (TEST) Rent Projector & Return it late");
            Console.WriteLine("7. Show entire rental history");
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
                        foreach (var rental in rentalService.Rentals.Where(r => r.ReturnDate == null))
                        {
                            Console.WriteLine(
                                $"{rental.User.Name} rented {rental.Hardware.Name} for {rental.RentedForDays} days and has {Math.Round((rental.DueDate - DateTime.Now).TotalDays, 1)} days to return it.");
                        }

                        break;
                    }

                    case "2":
                    {
                        foreach (var rental in rentalService.Rentals.Where(r => r.ReturnDate != null && r.Penalty == 0))
                        {
                            Console.WriteLine(
                                $"{rental.User.Name} returned {rental.Hardware.Name} on time. Penalty: {rental.Penalty} PLN");
                        }

                        break;
                    }

                    case "3":
                    {
                        foreach (var rental in rentalService.Rentals.Where(r => r.ReturnDate != null && r.Penalty > 0))
                        {
                            Console.WriteLine(
                                $"{rental.User.Name} returned {rental.Hardware.Name} late. Penalty applied: {rental.Penalty} PLN");
                        }

                        break;
                    }

                    case "4":
                    {
                        rentalService.Rent(student, laptop, 7);
                        break;
                    }

                    case "5":
                    {
                        rentalService.Return(laptop, DateTime.Now);
                        break;
                    }

                    case "6":
                    {
                        rentalService.Rent(student, projector, 7);
                        
                        var lateDate = DateTime.Now.AddDays(30); 
                        rentalService.Return(projector, lateDate);
                        break;
                    }

                    case "7":
                    {
                        foreach (var rental in rentalService.Rentals)
                        {
                            var status = rental.ReturnDate == null ? "(active)" : $"(returned) (Penalty: {rental.Penalty} PLN)";
                            Console.WriteLine($"- {rental.User.Name} rented {rental.Hardware.Name}. Status: {status}");
                        }
                        break;
                    }

                    case "8": isRunning = false; break;

                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"[ERROR] {e.Message}");
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