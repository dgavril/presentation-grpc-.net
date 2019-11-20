using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.Client;
using Protos.Location;
using Protos.User;

namespace GeoLocation.ConsoleClient
{
    class Program
    {
        private static async Task LoginAndRegisterLocations(string userId, int numberOfLocations,
            UserService.UserServiceClient userServiceClient, LocatorService.LocatorServiceClient locationServiceClient)
        {
            var currentUser = new UserModel
            {
                UserId = userId,
                UserName = $"User {userId}"
            };

            // call login
            var loginResult = await userServiceClient.LoginAsync(new LoginRequest { User = currentUser });
            Console.WriteLine($"Login for user {userId} is successful: {loginResult.Success}");

            var random = new Random();
            var nrRegistrations = numberOfLocations;
            while (nrRegistrations > 0)
            {
                await locationServiceClient.RegisterLocationAsync(new RegisterLocationRequest
                {
                    User = currentUser,
                    Location = new Protos.Location.GeoLocation { X = random.Next(0, 100), Y = random.Next(0, 100) }
                });

                nrRegistrations--;
            }
        }

        static async Task BeginUsersRegistrations(UserService.UserServiceClient userServiceClient, LocatorService.LocatorServiceClient locationServiceClient)
        {
            var numberOfUsers = 5;
            while (numberOfUsers > 0)
            {
                await LoginAndRegisterLocations(numberOfUsers.ToString(), 50, userServiceClient, locationServiceClient);
                numberOfUsers--;
            }
        }

        static void DisplayInfo(UserLocations currentUserLocations)
        {
            Console.WriteLine($"User {currentUserLocations.User.UserName} => Locations {currentUserLocations.Locations.Count}");

            var firstLocation = currentUserLocations.Locations.First();
            var lastLocation = currentUserLocations.Locations.Last();

            Console.WriteLine($"User {currentUserLocations.User.UserName} => First location {firstLocation}");
            Console.WriteLine($"User {currentUserLocations.User.UserName} => Last location {lastLocation}");

        }

        static async Task ShowAllLocationRegistrations(LocatorService.LocatorServiceClient locatorServiceClient)
        {
            Console.WriteLine("Show all locations registrations");
            var result = locatorServiceClient.GetAllLocations(new GetAllLocationsRequest());

            while (await result.ResponseStream.MoveNext())
            {
                var currentUserLocations = result.ResponseStream.Current;

                DisplayInfo(currentUserLocations);
            }
        }

        static async Task ShowAllLocationsRegistrationsAllAsync(LocatorService.LocatorServiceClient locatorServiceClient)
        {
            Console.WriteLine("Show all locations registrations allSync");
            var result = locatorServiceClient.GetAllLocations(new GetAllLocationsRequest());

            await foreach (var userLocations in result.ResponseStream.ReadAllAsync())
            {
                DisplayInfo(userLocations);
            }
        }

        static async Task Main(string[] args)
        {
            var channel = GrpcChannel.ForAddress("https://localhost:5001");

            var userServiceClient = new UserService.UserServiceClient(channel);
            var locationServiceClient = new LocatorService.LocatorServiceClient(channel);

            await BeginUsersRegistrations(userServiceClient, locationServiceClient); 
            await ShowAllLocationRegistrations(locationServiceClient);
            await ShowAllLocationsRegistrationsAllAsync(locationServiceClient);

            Console.ReadKey();
        }
    }
}
