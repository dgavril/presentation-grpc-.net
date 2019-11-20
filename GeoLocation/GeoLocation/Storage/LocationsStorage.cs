using System.Collections.Generic;
using System.Linq;
using Protos.Location;

namespace GeoLocation.Storage
{
    public static class LocationsStorage
    {
        private static IList<UserLocations> Locations { get; set; }

        static LocationsStorage()
        {
            Locations = new List<UserLocations>();
        }

        public static void Register(RegisterLocationRequest registerLocationRequest)
        {
            var userLocationsRegistration = Locations.ToList().FirstOrDefault(location => location.User.UserId == registerLocationRequest.User.UserId);

            // make sure user exists in the list
            if (userLocationsRegistration == null)
            {
                Locations.Add(new UserLocations { User = registerLocationRequest.User, Locations = { registerLocationRequest.Location } });
            }
            else
            {
                userLocationsRegistration.Locations.Add(registerLocationRequest.Location);
            }
        }

        public static IList<UserLocations> GetAllLocations()
        {
            return Locations;
        }
    }
}