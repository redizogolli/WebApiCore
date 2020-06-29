using System.Threading.Tasks;
using AutoMapper;
using Entities.DataTransferObjects;
using Entities.Models;
using Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebApiCore.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IAuthenticationManager _authManager;

        public AuthenticationController(ILoggerManager logger, IMapper mapper,
            UserManager<User> userManager, IAuthenticationManager authManager)
        {
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
            _authManager = authManager;
        }

        /// <summary>
        /// Register new User
        /// </summary>
        /// <param name="userForRegistration"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegisterDto
            userForRegistration)
        {
            var user = _mapper.Map<User>(userForRegistration);
            var result = await _userManager.CreateAsync(user, userForRegistration.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }
                return BadRequest(ModelState);
            }
            await _userManager.AddToRolesAsync(user, userForRegistration.Roles);
            return StatusCode(201);
        }

        /// <summary>
        /// login with username password
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<IActionResult> Authenticate([FromBody] UserAuthenticationDto
            user)
        {
            if (!await _authManager.ValidateUser(user))
            {
                _logger.LogWarn($"{nameof(Authenticate)}: Authentication failed. Wrong user name or password.");
                return Unauthorized();
            }
            return Ok(new { Token = await _authManager.CreateToken() });
        }
    }
}
