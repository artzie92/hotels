using Hotels.Infrastructure.Domain.Entities;

namespace Hotels.Infrastructure;

public interface IHotelsContext
{
    List<BookingEntity> Bookings { get; }
    List<HotelEntity> Hotels { get; }
}