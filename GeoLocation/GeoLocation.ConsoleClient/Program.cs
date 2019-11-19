using System;
using System.Threading.Tasks;
using GeoLocation.Protos;
using Grpc.Net.Client;

namespace GeoLocation.ConsoleClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var channel = GrpcChannel.ForAddress("https://localhost:5001");

            var client = new Greeter.GreeterClient(channel);

            var result = await client.SayHelloAsync(new HelloRequest
            {
                Name = "Daniel"
            });

            Console.WriteLine(result.Message);
            Console.ReadKey();
        }
    }
}
