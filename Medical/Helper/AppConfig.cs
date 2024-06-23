using Medical.Data;
using Medical.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Medical.Helper
{
    public static class AppConfig
    {
        public static async Task<WebApplication> AddAppConfig(this WebApplication app)
        {
            var scope = app.Services.CreateScope().ServiceProvider;

            var dbContext = scope.GetRequiredService<ApplicationDbContext>();
            var userManager = scope.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = scope.GetRequiredService<RoleManager<IdentityRole<int>>>();
            var config = scope.GetRequiredService<IConfiguration>();

            await dbContext.Database.MigrateAsync();

            ///// Seed Data 
            await Seeder.Seed(dbContext, userManager, roleManager,config);

            return app;
        }

    }
}
