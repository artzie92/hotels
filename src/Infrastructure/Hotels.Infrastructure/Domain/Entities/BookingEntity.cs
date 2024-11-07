namespace Hotels.Infrastructure.Domain.Entities;

public class BookingEntity
{
    public string HotelId { get;  set; }
    public long Arrival { get;  set; }
    public long Departure { get;  set; }
    public string RoomType { get;  set; }
    public string RoomRate { get;  set; }
}