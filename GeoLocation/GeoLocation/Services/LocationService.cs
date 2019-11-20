using System.Threading.Tasks;
using GeoLocation.Storage;
using Grpc.Core;
using Protos.Location;

namespace GeoLocation.Services
{
    public class LocationService : LocatorService.LocatorServiceBase
    {
        public override Task<RegisterLocationResponse> RegisterLocation(RegisterLocationRequest request, ServerCallContext context)
        {
            LocationsStorage.Register(request);
            return Task.FromResult(new RegisterLocationResponse());
        }

        public override async Task GetAllLocations(GetAllLocationsRequest request, IServerStreamWriter<UserLocations> responseStream, ServerCallContext context)
        {
            foreach (var userLocations in LocationsStorage.GetAllLocations())
            {
                await responseStream.WriteAsync(userLocations);

                await Task.Delay(1000);
            }
        }
    }
}