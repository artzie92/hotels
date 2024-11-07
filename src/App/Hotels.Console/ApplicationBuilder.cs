using Hotels.Infrastructure;
using Hotels.Infrastructure.Domain;
using Hotels.Infrastructure.Handlers;
using Microsoft.Extensions.DependencyInjection;

namespace Hotels.Console;

public class ApplicationBuilder
{
    private IServiceCollection services = new ServiceCollection();
    private IServiceProvider provider;

    public ApplicationBuilder LoadHotelsContext(string hotelsPath, string bookingsPath)
    {
        var bookings = File.ReadAllText(bookingsPath);
        var hotels = File.ReadAllText(hotelsPath);

        services.AddTransient<IHotelsContext>((p) => HotelsDb.BuildContextFromJson(hotels, bookings));
        services.AddTransient<CheckAvailabilityQueryHandler>();
        services.AddTransient<SearchRoomQueryHandler>();
        
        return this;
    }

    public IServiceScope Build()
    {
        
        provider = services.BuildServiceProvider();
        return provider.CreateScope();
    }


   
}