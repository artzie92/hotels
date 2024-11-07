using Hotels.Dto;
using Hotels.Dto.Queries;
using Hotels.Infrastructure.Domain;

namespace Hotels.Infrastructure.Handlers;

public class CheckAvailabilityQueryHandler : QueryHandler<CheckAvailabilityQuery, CheckAvailabilityQuery.Result>
{
    public CheckAvailabilityQueryHandler(IHotelsContext db) : base(db)
    {
    }

    public override CheckAvailabilityQuery.Result Handle(CheckAvailabilityQuery query)
    {
        List<RoomAvailability> unavailableRooms = GetUnavailableRooms(query.RoomType);

        var hotelRooms = db.Hotels.FirstOrDefault(w => w.Id == query.HotelId)?.Rooms;
        if (hotelRooms == null)
        {
            throw new HotelsException(HotelsException.HotelDoesNotExist,
                $"Hotel with id {query.HotelId} not exist or rooms are unavailable.");
        }

        var availableRoomsInHotel = hotelRooms.Count(r => r.RoomType == query.RoomType);
        
        var bookingsInRange =
            unavailableRooms.Where(w => w.Date >= query.StartDate && w.Date <= query.EndDate).ToList();
        
        var maxCount = bookingsInRange.Any() ? bookingsInRange.Max(m => m.Count) : 0;
        var availableRooms = availableRoomsInHotel - maxCount;
        return new CheckAvailabilityQuery.Result
        {
            AvailableRooms = availableRooms
        };
    }
}