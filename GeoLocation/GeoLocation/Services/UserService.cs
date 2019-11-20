using System.Threading.Tasks;
using GeoLocation.Storage;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Protos.User;

namespace GeoLocation.Services
{
    public class UserService : Protos.User.UserService.UserServiceBase
    {
        private readonly ILogger<UserService> logger;

        public UserService(ILogger<UserService> logger)
        {
            this.logger = logger;
        }

        public override Task<LoginResponse> Login(LoginRequest request, ServerCallContext context)
        {
            UsersStorage.AddUser(request.User);
            return Task.FromResult(new LoginResponse {Success = true});
        }
    }
}
