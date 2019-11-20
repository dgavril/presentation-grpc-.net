using System.Collections.Generic;
using Protos.User;

namespace GeoLocation.Storage
{
    public static class UsersStorage
    {
        private static IList<UserModel> Users { get; set; }

        static UsersStorage()
        {
            Users = new List<UserModel>();
        }

        public static void AddUser(UserModel user)
        {
            Users.Add(user);
        }

        public static IList<UserModel> GetUsers()
        {
            return Users;
        }
    }
}