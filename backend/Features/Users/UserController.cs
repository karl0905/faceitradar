using Microsoft.AspNetCore.Mvc;
using FaceItRadar.Data;

namespace FaceItRadar.Features.Users
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;

        public UserController(
            AppDbContext context,
            ILogger<UserController> logger,
            IUserService userService
            )
        {
            _context = context;
            _logger = logger;
            _userService = userService;
        }

        [HttpGet]
        public IActionResult GetUsers()
        {
            try
            {
                // Simple status response until DB is fully configured
                return Ok(new { status = "User endpoint working" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetUsers");
                return StatusCode(500, new { error = "Internal server error" });
            }
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUser(string userId)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(userId);
                if (user == null)
                {
                    return NotFound(new { error = "User not found" });
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving user {userId}");
                return StatusCode(500, new { error = "Internal server error" });
            }
        }
    }
}
