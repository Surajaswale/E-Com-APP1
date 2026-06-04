using BCrypt.Net;
using ECommerceSolution.Entities;

namespace ECommerceSolution.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAdminAsync(
            AppDbContext context)
        {
            if (!context.Users.Any())
            {
                var admin = new User
                {
                    FirstName = "System",
                    LastName = "Admin",
                    Email = "admin@gmail.com",
                    PasswordHash =
                        BCrypt.Net.BCrypt
                        .HashPassword(
                            "Admin@123"),

                    Role = "Admin"
                };

                context.Users.Add(admin);

                await context.SaveChangesAsync();
            }
        }
    }
}