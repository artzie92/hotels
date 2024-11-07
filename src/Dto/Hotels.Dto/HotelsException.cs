namespace Hotels.Dto;

public class HotelsException : Exception
{
    public const string HotelDoesNotExist = "hotel_does_not_exist";

    public string Code { get; protected set; }


    public HotelsException(string code, string message) : base(message)
    {
        Code = code;
    }
}