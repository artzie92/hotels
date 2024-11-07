using Hotels.Dto.Models;

namespace Hotels.Dto.Queries;

public class SearchRoomQuery : IQuery<SearchRoomQuery.Result>
{
    public string HotelId { get; set; }
    public string RoomType { get; set; }
    public DateTime CurrentDate { get; set; }
    public DateTime EndDate { get; set; }

    public class Result
    {
        public List<DateRange> DateRanges { get; set; }
    }
}