using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AuthAPI.Static;
using AuthAPI.Repository.Contracts;
using AuthAPI.Models;

namespace AuthAPI.Repository
{
    public class RegisterRepository : IRegisterRepository
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private static bool initialStatus = true;
        public RegisterRepository(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<ActionResult> Register([FromBody] RegisterModel model, params string[] roles)
        {
            if(initialStatus)
            {
                if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
                    await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                if (!await _roleManager.RoleExistsAsync(UserRoles.User))
                    await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));
                Console.WriteLine(initialStatus);
                initialStatus = false;  
            }
            

            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return new BadRequestObjectResult(new Response { Status = "Error", Message = "User already exists!" });

            IdentityUser user = new()
            {
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return new BadRequestObjectResult(new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });

            foreach (var role in roles)
                await _userManager.AddToRoleAsync(user, role);

            return new OkObjectResult(new Response { Status = "Success", Message = "User created successfully!" });
        }
    }
}
