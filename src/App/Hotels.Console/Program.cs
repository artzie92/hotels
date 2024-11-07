// See https://aka.ms/new-console-template for more information

using Hotels.Console;
using Hotels.Dto.Queries;
using Hotels.Infrastructure.Handlers;
using Microsoft.Extensions.DependencyInjection;

var userArgs = Environment.GetCommandLineArgs().ToList();

var indexOfHotels = userArgs.IndexOf("--hotels");
var indexOfBookings = userArgs.IndexOf("--bookings");

var hotelsPath = indexOfHotels == -1 ? Path.Combine("Storage", "hotels.json") : userArgs[indexOfHotels + 1];
var bookingsPath = indexOfBookings == -1 ? Path.Combine("Storage", "bookings.json") : userArgs[indexOfBookings + 1];

Console.WriteLine($"Path for hotels: {hotelsPath}");
Console.WriteLine($"Path for bookings: {bookingsPath}");

using var app = new ApplicationBuilder()
    .LoadHotelsContext(hotelsPath, bookingsPath)
    .Build();

while (true)
{
    ShowMenu();

    var option = System.Console.ReadLine();

    if (string.IsNullOrWhiteSpace(option))
    {
        // close app
        break;
    }


    if (option == "1")
    {
        Availability();
    }
    else if (option == "2")
    {
        Search();
    }
    else
    {
        Console.WriteLine("Unknown option. Try again...");
    }
}

void ShowMenu()
{
    Console.WriteLine("--- Menu ---");
    Console.WriteLine("[1] Check availability");
    Console.WriteLine("[2] Search room");
    Console.WriteLine("* Blank line to exit");
    Console.WriteLine("---------");
    Console.Write("Your option: ");
}

void Search()
{
    var hotelId = GetHotelId();
    var roomType = GetRoomType();
    int daysAhead = GetDaysAhead();

    DateTime currentDate = DateTime.Today;
    DateTime endDate = currentDate.AddDays(daysAhead);
    var availableDates = new List<string>();

    var handler = app.ServiceProvider.GetRequiredService<SearchRoomQueryHandler>();
    var results = handler.Handle(new SearchRoomQuery()
    {
        HotelId = hotelId,
        RoomType = roomType,
        EndDate = endDate,
        CurrentDate = currentDate
    });

    foreach (var dateRange in results.DateRanges)
    {
        availableDates.Add($"{dateRange.StartDate:yyyyMMdd}-{dateRange.EndDate:yyyyMMdd},{dateRange.Count}");
    }

    var message = string.Join(", ", availableDates);
    Console.WriteLine(message);
}

void Availability()
{
    var hotelId = GetHotelId();
    var roomType = GetRoomType();
    var dateRange = GetDateRange();

    var dates = dateRange.Split("-");
    var startDateString = dates.First();
    var endDateString = dates.Length > 1 ? dates.Last() : startDateString;

    DateTime startDate = DateTime.ParseExact(startDateString, "yyyyMMdd", null);
    DateTime endDate = DateTime.ParseExact(endDateString, "yyyyMMdd", null);

    var handler = app.ServiceProvider.GetRequiredService<CheckAvailabilityQueryHandler>();
    var results = handler.Handle(new CheckAvailabilityQuery
    {
        HotelId = hotelId,
        RoomType = roomType,
        EndDate = endDate,
        StartDate = startDate
    });

    Console.WriteLine($"Available rooms: {results.AvailableRooms}");
}

string GetDateRange()
{
    Console.WriteLine("Data range: (ex. 20241108-20241125) ");
    var input = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(input))
    {
        Console.WriteLine("Please provide correct value...");
        return GetDateRange();
    }

    return input;
}

int GetDaysAhead()
{
    Console.WriteLine("Days ahead: ");
    var input = Console.ReadLine();
    int value;
    if (string.IsNullOrWhiteSpace(input) || !int.TryParse(input, out value))
    {
        Console.WriteLine("Please provide correct value...");
        return GetDaysAhead();
    }

    return value;
}

string GetRoomType()
{
    Console.Write("Room type: (ex. DBL): ");
    var input = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(input))
    {
        Console.WriteLine("Please provide correct value...");
        return GetRoomType();
    }

    return input;
}

string GetHotelId()
{
    Console.Write("Hotel id: (ex. H1): ");
    var input = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(input))
    {
        Console.WriteLine("Please provide correct value...");
        return GetHotelId();
    }

    return input;
}