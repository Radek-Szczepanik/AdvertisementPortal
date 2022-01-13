using AdvertisementPortal.Entities;
using AdvertisementPortal.Models;
using Microsoft.AspNetCore.Identity;

namespace AdvertisementPortal.Services
{
    public class AccountService : IAccountService
    {
        private readonly AdvertisementDbContext context;
        private readonly IPasswordHasher<User> passwordHasher;

        public AccountService(AdvertisementDbContext context, IPasswordHasher<User> passwordHasher)
        {
            this.context = context;
            this.passwordHasher = passwordHasher;
        }
        public void RegisterUser(RegisterUserDto dto)
        {
            var newUser = new User()
            {
                Email = dto.Email,
                RoleId = dto.RoleId
            };

            var hashedPassword = passwordHasher.HashPassword(newUser, dto.Password);
            newUser.PasswordHash = hashedPassword;
            context.Users.Add(newUser);
            context.SaveChanges();
        }
    }
}
