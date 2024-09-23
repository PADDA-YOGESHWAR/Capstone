
using AuthAPI.Models;
using AuthAPI.Repository.Contracts;
using AuthAPI.Static;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace AuthAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILoginRepository _loginRepository;
        private readonly IRegisterRepository _registerRepository;

        public AuthController(ILoginRepository loginRepository,IRegisterRepository registerRepository)
        {
            _loginRepository = loginRepository;
            _registerRepository = registerRepository;
        }


        [Authorize(Roles = UserRoles.Admin)]
        [HttpGet]
        [Route("Hey")]

        public IActionResult Get()
        {
            return Ok("You are logged in");
        }
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            string[] roles =
            [
                UserRoles.User
            ];
            return await _registerRepository.Register(model, roles);

        }

        [HttpPost]
        [Route("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel model)
        {
            string[] roles =
            [
                UserRoles.Admin,
                UserRoles.User
            ];
            return await _registerRepository.Register(model, roles);

        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var login = await _loginRepository.Login(model);
            return Ok(login);
        }
    }
}
