using Hotels.Infrastructure.Domain.Entities;

namespace Hotels.Infrastructure.Tests;

/// <summary>
/// Be careful when you modify this collection, the business logic for the tests may change!
/// </summary>
public class HotelContextMockForCheckAvailabilityueryHandlerTests : IHotelsContext
{

    public List<BookingEntity> Bookings { get; } =
    [
        new BookingEntity
        {
            HotelId = "H1",
            RoomType = "DBL",
            Arrival = 20241107,
            Departure = 20241111,
            RoomRate = "Prepaid"
        },
        new BookingEntity
        {
            HotelId = "H1",
            RoomType = "DBL",
            Arrival = 20241113,
            Departure = 20241116,
            RoomRate = "Prepaid"
        },
        new BookingEntity
        {
            HotelId = "H1",
            RoomType = "DBL",
            Arrival = 20241113,
            Departure = 20241117,
            RoomRate = "Prepaid"
        },
        new BookingEntity
        {
            HotelId = "H1",
            RoomType = "DBL",
            Arrival = 20241115,
            Departure = 20241126,
            RoomRate = "Prepaid"
        },
        new BookingEntity
        {
            HotelId = "H1",
            RoomType = "SGL",
            Arrival = 20240902,
            Departure = 20240905,
            RoomRate = "Standard"
        }
    ];

    public List<HotelEntity> Hotels { get; } =
    [
        new HotelEntity
        {
            Id = "H1",
            Name = "Hotel California Mock",
            Rooms =
            [
                new RoomEntity
                {
                    RoomType = "SGL",
                    RoomId = 101
                },
                new RoomEntity
                {
                    RoomType = "SGL",
                    RoomId = 102
                },
                new RoomEntity
                {
                    RoomType = "DBL",
                    RoomId = 201
                },
                new RoomEntity
                {
                    RoomType = "DBL",
                    RoomId = 202
                }
            ]
        }
    ];
}