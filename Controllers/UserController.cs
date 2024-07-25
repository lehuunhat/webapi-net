using HienTangToc.Data;
using HienTangToc.Models;
using HienTangToc.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HienTangToc.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        private readonly MyDbContext _context;


        public UserController(UserService userService, MyDbContext context)
        {
            _userService = userService;
            _context = context;

        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] UserModel user)
        {
            var result = await _userService.CreateUserAsync(user);

            if (result.user == null)
            {
                return BadRequest(new { message = result.message });
            }

            return Ok(new
            {
                data = result.user,
                message = result.message
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(string emailorusername, string Password)
        {

            var result = await _userService.LoginAsync(emailorusername, Password);
            if (result.user == null)
            {
                return BadRequest(new { message = result.message });
            }

            return Ok(new
            {
                data = result.user,
                message = result.message
            }); ;


        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(int Id, [FromBody] UserModel user)
        {
            var result = await _userService.UpdateUserAsync(Id, user);

            if (result.updatedUser == null)
            {
                return BadRequest(new { message = result.message });
            }

            return Ok(new
            {
                originalData = result.originalUser,
                data = result.updatedUser,
                message = result.message
            });
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int Id)
        {
            var result = await _userService.DeleteUserAsync(Id);

            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }

            return Ok(new { message = result.Message });
        }
    }
}
