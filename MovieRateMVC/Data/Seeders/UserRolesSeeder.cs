using Microsoft.AspNetCore.Identity;
using MovieRateMVC.Enums;

namespace MovieRateMVC.Data.Seeders
{
	public class UserRolesSeeder
	{
		public static async Task SeedRolesAsync(IServiceProvider services)
		{
			var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
			var roleNames = Enum.GetNames(typeof(UserRoles));

			foreach (var roleName in roleNames)
			{
				if (!await roleManager.RoleExistsAsync(roleName))
					await roleManager.CreateAsync(new IdentityRole(roleName));
			}
		}
	}
}
