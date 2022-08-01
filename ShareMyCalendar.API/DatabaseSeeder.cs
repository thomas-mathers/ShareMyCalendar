using Microsoft.AspNetCore.Identity;
using ThomasMathers.Infrastructure.IAM.Data;

namespace ShareMyCalendar.API
{
    public interface IDatabaseSeeder
    {
        public Task Seed();
    }

    public class DatabaseSeeder : IDatabaseSeeder
    {
        public static readonly User Admin = new()
        {
            Id = Guid.Parse("76b20592-a25e-4d5a-bd4c-0a8ba4e00f16"),
            UserName = "admin",
            NormalizedUserName = "ADMIN",
            Email = "admin@sharemycalendar.ca",
            NormalizedEmail = "ADMIN@SHAREMYCALENDAR.CA"
        };

        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly string AdminPassword;

        public DatabaseSeeder(IConfiguration configuration, UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;

            AdminPassword = configuration["AdminPassword"];

            if (string.IsNullOrEmpty(AdminPassword))
            {
                throw new ArgumentNullException(AdminPassword, "AdminPassword must be specified in app configuration in order for Seed function to execute");
            }
        }

        public async Task Seed()
        {
            if (await _roleManager.RoleExistsAsync(Roles.Admin.Name) == false)
            {
                await _roleManager.CreateAsync(Roles.Admin);
            }

            if (await _roleManager.RoleExistsAsync(Roles.User.Name) == false)
            {
                await _roleManager.CreateAsync(Roles.User);
            }

            if (await _userManager.FindByIdAsync(Admin.Id.ToString()) == null)
            {
                await _userManager.CreateAsync(Admin, AdminPassword);
                await _userManager.AddToRoleAsync(Admin, Roles.Admin.Name);
            }
        }
    }
}
