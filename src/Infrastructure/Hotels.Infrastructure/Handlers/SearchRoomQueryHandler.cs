using Hotels.Dto;
using Hotels.Dto.Models;
using Hotels.Dto.Queries;
using Hotels.Infrastructure.Domain;

namespace Hotels.Infrastructure.Handlers;

public class SearchRoomQueryHandler : QueryHandler<SearchRoomQuery, SearchRoomQuery.Result>
{
    public SearchRoomQueryHandler(IHotelsContext db) : base(db)
    {
    }

    public override SearchRoomQuery.Result Handle(SearchRoomQuery query)
    {
        var hotelRooms = db.Hotels
            .FirstOrDefault(w => string.Equals(w.Id, query.HotelId, StringComparison.CurrentCultureIgnoreCase))?.Rooms;
        if (hotelRooms == null)
        {
            throw new HotelsException(HotelsException.HotelDoesNotExist,
                $"Hotel with id {query.HotelId} not exist or rooms are unavailable.");
        }

        var availableRoomsInHotel = hotelRooms.Count(r => string.Equals(r.RoomType, query.RoomType, StringComparison.CurrentCultureIgnoreCase));
        if (availableRoomsInHotel == 0)
        {
            return new SearchRoomQuery.Result
            {
                DateRanges = new List<DateRange>()
            };
        }

        var currentDate = query.CurrentDate;
        var endDate = query.EndDate;

        List<RoomAvailability> unavailableRooms = GetUnavailableRooms(query.RoomType);

        var results = new List<DateRange>();
        var startDate = currentDate;
        var itemToAdd = new DateRange
        {
            Count = availableRoomsInHotel,
            StartDate = startDate
        };

        for (DateTime date = currentDate; date <= endDate; date = date.AddDays(1))
        {
            var existBooking = unavailableRooms.FirstOrDefault(w => w.Date == date);
            var rooms = availableRoomsInHotel;

            if (existBooking != null)
            {
                rooms = availableRoomsInHotel - existBooking.Count;
            }

            if (itemToAdd.Count == rooms)
            {
                itemToAdd.EndDate = date;
                continue;
            }

            itemToAdd = new DateRange()
            {
                Count = rooms,
                StartDate = date,
                EndDate = date
            };

            results.Add(itemToAdd);
        }

        return new SearchRoomQuery.Result
        {
            DateRanges = results.Where(w => w.Count > 0).ToList()
        };
    }
}