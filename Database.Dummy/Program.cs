using System;
using System.Linq;
using User.System.Core;
using User.System.Core.Model;
using User.System.Core.Services;
using User.System.Core.Services.Interfaces;

namespace Database.Dummy
{
    class Program
    {
        static void Main(string[] args)
        {
            var context = new SocketDbContext();

            var user = new SocketUser();
            user.Name = "Billy Mumby";
            user.Email = "billy@test.com";            
                
            IPasswordProtectionService passwordProtectionService = new PasswordProtectionService();

            var (password, salt) =passwordProtectionService.Encrypt("Test123");

            user.Password = password;
            user.Salt = salt;

            context.SocketUsers.Add(user);
            context.SaveChanges();
        }
    }
}
