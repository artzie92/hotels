namespace Hotels.Dto.Queries;

public class CheckAvailabilityQuery : IQuery<CheckAvailabilityQuery.Result>
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string RoomType { get; set; }
    public string HotelId { get; set; }

    public class Result
    {
        public int AvailableRooms { get; set; }
    }
}