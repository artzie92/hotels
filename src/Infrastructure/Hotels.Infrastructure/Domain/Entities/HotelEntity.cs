namespace Hotels.Infrastructure.Domain.Entities;

public class HotelEntity
{
    public string Id { get;  set; }
    public string Name { get;  set; }
    public IEnumerable<RoomTypeEntity> RoomTypes { get;  set; }
    public IEnumerable<RoomEntity> Rooms { get;  set; }
}