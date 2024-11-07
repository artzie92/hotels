using Hotels.Dto;
using Hotels.Dto.Queries;
using Hotels.Infrastructure.Handlers;
using Microsoft.Extensions.DependencyInjection;

namespace Hotels.Infrastructure.Tests.Handlers;

public class CheckAvailabilityQueryHandlerTests
{
    private readonly ServiceProvider provider;

    public CheckAvailabilityQueryHandlerTests()
    {
        provider = new ServiceCollection()
            .AddTransient<IHotelsContext>((p) => new HotelContextMockForCheckAvailabilityueryHandlerTests())
            .AddTransient<CheckAvailabilityQueryHandler>()
            .BuildServiceProvider();
    }

    [Fact]
    public void CheckAvailabilityNoRooms_Success()
    {
        var query = new CheckAvailabilityQuery
        {
            HotelId = "H1",
            RoomType = "DBL",
            StartDate = new DateTime(2024, 11, 07),
            EndDate = new DateTime(2024, 12, 07),
        };

        var handler = provider.GetRequiredService<CheckAvailabilityQueryHandler>();
        var results = handler.Handle(query);

        Assert.True(results.AvailableRooms == -1);
    }

    [Fact]
    public void CheckAvailabilityOneRoom_Success()
    {
        var query = new CheckAvailabilityQuery
        {
            HotelId = "H1",
            RoomType = "DBL",
            StartDate = new DateTime(2024, 11, 07),
            EndDate = new DateTime(2024, 11, 12),
        };

        var handler = provider.GetRequiredService<CheckAvailabilityQueryHandler>();
        var results = handler.Handle(query);

        Assert.True(results.AvailableRooms == 1);
    }

    [Fact]
    public void CheckAvailabilityTwoRooms_Success()
    {
        var query = new CheckAvailabilityQuery
        {
            HotelId = "H1",
            RoomType = "DBL",
            StartDate = new DateTime(2024, 11, 26),
            EndDate = new DateTime(2024, 12, 07),
        };

        var handler = provider.GetRequiredService<CheckAvailabilityQueryHandler>();
        var results = handler.Handle(query);

        Assert.True(results.AvailableRooms == 2);
    }

    [Fact]
    public void CheckAvailabilityHotelDoesNotExist_Fail()
    {
        var query = new CheckAvailabilityQuery
        {
            HotelId = "H2",
            RoomType = "DBL",
            StartDate = new DateTime(2024, 11, 26),
            EndDate = new DateTime(2024, 12, 07),
        };

        var handler = provider.GetRequiredService<CheckAvailabilityQueryHandler>();
        var ex = Assert.Throws<HotelsException>(() => { _ = handler.Handle(query); });
        Assert.True(ex.Code == HotelsException.HotelDoesNotExist);
    }
}