using Hotels.Dto;
using Hotels.Dto.Queries;
using Hotels.Infrastructure.Handlers;
using Microsoft.Extensions.DependencyInjection;

namespace Hotels.Infrastructure.Tests.Handlers;

public class SearchRoomQueryHandlerTests
{
    private readonly ServiceProvider provider;

    public SearchRoomQueryHandlerTests()
    {
        provider = new ServiceCollection()
            .AddTransient<IHotelsContext>((p) => new HotelContextMockForSearchRoomQueryHandlerTests())
            .AddTransient<SearchRoomQueryHandler>()
            .BuildServiceProvider();
    }

    [Theory]
    [InlineData("20241113", "20241115", 1)]
    [InlineData("20241114", "20241117", 1)]
    [InlineData("20241101", "20241230", 5)]
    public void SearchRoomQueryRange_Success(string currentDate, string endDate, int expected)
    {
        var query = new SearchRoomQuery
        {
            HotelId = "H1",
            RoomType = "DBL",
            CurrentDate = DateTime.ParseExact(currentDate, "yyyyMMdd", null),
            EndDate = DateTime.ParseExact(endDate, "yyyyMMdd", null),
        };

        var handler = provider.GetRequiredService<SearchRoomQueryHandler>();
        var res = handler.Handle(query);
        Assert.True(res.DateRanges.Count == expected);
    }

    [Fact]
    public void SearchRoomQuerNoRooms_Success()
    {
        var query = new SearchRoomQuery
        {
            HotelId = "H1",
            RoomType = "DBL",
            CurrentDate = new DateTime(2024, 11, 14),
            EndDate = new DateTime(2024, 11, 15)
        };

        var handler = provider.GetRequiredService<SearchRoomQueryHandler>();
        var res = handler.Handle(query);
        Assert.True(res.DateRanges.Count == 0);
    }

    [Fact]
    public void SearchRoomQuerHotelDoesNotExist_Fail()
    {
        var query = new SearchRoomQuery
        {
            HotelId = "H2",
            RoomType = "DBL",
        };

        var handler = provider.GetRequiredService<SearchRoomQueryHandler>();
        var ex = Assert.Throws<HotelsException>(() => { _ = handler.Handle(query); });
        Assert.True(ex.Code == HotelsException.HotelDoesNotExist);
    }
}