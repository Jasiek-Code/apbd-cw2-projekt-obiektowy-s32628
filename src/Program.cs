using projekt_obiektowy.Domain;
using projekt_obiektowy.Domain.Equipment;
using projekt_obiektowy.Domain.Users;
using projekt_obiektowy.Services;

Console.WriteLine("------Renting hardware project-------\n");

var rentalService = new RentalService();

var student = new Student("Jan", "Kowalski");
var employee = new Employee("Anna", "Nowak");

Console.WriteLine("* Added Users *\n");
Console.WriteLine($"student added: {student.GetType().Name} {student.Name} {student.Surname}");
Console.WriteLine($"employee added: {employee.GetType().Name} {employee.Name} {employee.Surname}\n");

var laptop = new Laptop("Dell XPS 15", "Intel i7", 16);
var camera = new Camera("Sony A7 III", true, 64);
var projector = new Projector("Epson 1080p", 3000, "1920x1080");

Console.WriteLine("* Added items *\n");
Console.WriteLine($"Laptop added: {laptop.GetType()} {laptop.Name}");
Console.WriteLine($"Camera added: {camera.GetType()} {camera.Name}");
Console.WriteLine($"Projector added: {projector.GetType()} {projector.Name}");

rentalService.Rent(student, laptop, 7);

Console.WriteLine("\n* Renting test #1 *\n");

Console.WriteLine("Rented items list:");

foreach (var rental in rentalService.Rentals)
{
    Console.WriteLine($"{rental.User.Name} rented {rental.Hardware.Name} for {rental.RentedForDays} days and has {(rental.DueDate - DateTime.Now).TotalDays} to return it");
}

Console.WriteLine();

// Console.WriteLine("* Renting unavailable item test *\n");
// rentalService.Rent(student, laptop, 7);

rentalService.Rent(student, projector, 14);

Console.WriteLine("* Renting test #2 *\n");

foreach (var rental in rentalService.Rentals)
{
    Console.WriteLine($"{rental.User.Name} rented {rental.Hardware.Name} for {rental.RentedForDays} days and has {(rental.DueDate - DateTime.Now).TotalDays} to return it");
}

//Console.WriteLine("* Exciding renting limit test *\n");
//rentalService.Rent(student, camera, 7);


rentalService.Return(laptop);
Console.WriteLine("\n* Returning item test #1 *\n");

foreach (var rental in rentalService.Rentals)
{
    Console.WriteLine($"{rental.User.Name} rented {rental.Hardware.Name} for {rental.RentedForDays} days and has {(rental.DueDate - DateTime.Now).TotalDays} to return it");
}


