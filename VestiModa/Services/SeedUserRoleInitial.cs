using Microsoft.AspNetCore.Identity;

namespace VestiModa.Services
{
    public class SeedUserRoleInitial : ISeedUserRoleInitial
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public SeedUserRoleInitial(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task CreateRoleAsync()
        {
           if(!await _roleManager.RoleExistsAsync("Member"))
            {
                IdentityRole role = new IdentityRole();
                role.Name = "Member";
                role.NormalizedName = "MEMBER";

                IdentityResult roleResult = await _roleManager.CreateAsync(role);

                if (!roleResult.Succeeded)
                {
                    throw new Exception("Falha ao criar o perfil 'Member'.");
                }
            }

            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                IdentityRole role = new IdentityRole();
                role.Name = "Admin";
                role.NormalizedName = "ADMIN";

                IdentityResult roleResult = await _roleManager.CreateAsync(role);

                if (!roleResult.Succeeded)
                {
                    throw new Exception("Falha ao criar o perfil 'Admin'.");
                }
            }
        }

        public async Task CreateUserAsync()
        {
            if (await _userManager.FindByEmailAsync("usuario@localhost") == null)
            {
                IdentityUser user = new IdentityUser();
                user.UserName = "usuario@localhost";
                user.Email = "usuario@localhost";
                user.NormalizedUserName = "USUARIO@LOCALHOST";
                user.NormalizedEmail = "USUARIO@LOCALHOST";
                user.EmailConfirmed = true;
                user.LockoutEnabled = false;
                user.SecurityStamp = Guid.NewGuid().ToString();

                IdentityResult result = await _userManager.CreateAsync(user, "Penedo#2024");

                if (result.Succeeded)
                {
                    _userManager.AddToRoleAsync(user, "Member").Wait();
                }
                else
                {
                    throw new Exception("Falha ao criar o usuario.");
                }
            }

            if (await _userManager.FindByEmailAsync("admin@localhost") == null)
            {
                IdentityUser user = new IdentityUser();
                user.UserName = "admin@localhost";
                user.Email = "admin@localhost";
                user.NormalizedUserName = "ADMIN@LOCALHOST";
                user.NormalizedEmail = "ADMIN@LOCALHOST";
                user.EmailConfirmed = true;
                user.LockoutEnabled = false;
                user.SecurityStamp = Guid.NewGuid().ToString();

                IdentityResult result = await _userManager.CreateAsync(user, "Penedo#2024");

                if (result.Succeeded)
                {
                    _userManager.AddToRoleAsync(user, "Admin").Wait();
                }
                else
                {
                    throw new Exception("Falha ao criar o usuario.");
                }
            }
        }
    }
}
