using Hotels.Dto.Queries;
using Hotels.Infrastructure.Domain;

namespace Hotels.Infrastructure.Handlers;


public abstract class QueryHandler<TQuery, TResult>
    where TQuery : IQuery<TResult>
{
    protected readonly IHotelsContext db;

    public QueryHandler(IHotelsContext db)
    {
        this.db = db;
    }

    public abstract TResult Handle(TQuery query);

    protected List<RoomAvailability> GetUnavailableRooms(string roomType)
    {
        List<RoomAvailability> unavailableRooms = new List<RoomAvailability>();

        foreach (var booking in db.Bookings.Where(b => b.RoomType == roomType))
        {
            DateTime bookingStart = DateTime.ParseExact(booking.Arrival.ToString(), "yyyyMMdd", null);
            DateTime bookingEnd = DateTime.ParseExact(booking.Departure.ToString(), "yyyyMMdd", null);

            for (DateTime date = bookingStart; date < bookingEnd; date = date.AddDays(1))
            {
                var existItem = unavailableRooms.FirstOrDefault(w => w.Date == date);
                if (existItem != null)
                {
                    existItem.Count++;
                    continue;
                }

                unavailableRooms.Add(new RoomAvailability
                {
                    RoomType = roomType,
                    Count = 1,
                    Date = date
                });
            }
        }

        return unavailableRooms;
    }
}

public class RoomAvailability
{
    public string RoomType { get; set; }
    public DateTime Date { get; set; }
    public int Count { get; set; }
}