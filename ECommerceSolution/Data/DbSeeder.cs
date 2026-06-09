using ECommerceSolution.Entities;

namespace ECommerceSolution.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAdminAsync(
            AppDbContext context)
        {
            if (!context.Users.Any(
                u => u.Email == "admin@gmail.com"))
            {
                var admin = new User
                {
                    FirstName = "System",
                    LastName = "Admin",
                    Email = "admin@gmail.com",
                    PasswordHash =
                        BCrypt.Net.BCrypt.HashPassword(
                            "Admin@123"),

                    Role = "Admin"
                };

                context.Users.Add(admin);

                await context.SaveChangesAsync();
            }
        }
    }
}