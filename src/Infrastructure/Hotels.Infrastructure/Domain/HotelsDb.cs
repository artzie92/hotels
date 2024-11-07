using Hotels.Infrastructure.Domain.Entities;
using Newtonsoft.Json;

namespace Hotels.Infrastructure.Domain;

public class HotelsDb : IHotelsContext
{
    protected HotelsDb()
    {
    }

    public static HotelsDb BuildContextFromJson(string hotelsJson, string bookingsJson)
    {
        var bookings = JsonConvert.DeserializeObject<List<BookingEntity>>(bookingsJson);
        var hotels = JsonConvert.DeserializeObject<List<HotelEntity>>(hotelsJson);

        if (bookings == null || hotels == null)
        {
            throw new ArgumentException("Hotels and bookings cannot be null.");
        }

        var db = new HotelsDb
        {
            Bookings = bookings,
            Hotels = hotels
        };

        return db;
    }

    public List<BookingEntity> Bookings { get; protected set; }
    public List<HotelEntity> Hotels { get; protected set; }
}