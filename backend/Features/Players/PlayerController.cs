using Microsoft.AspNetCore.Mvc;

namespace FaceItRadar.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlayerController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetStatus()
        {
            // This is a very simple response with a single key-value pair
            return Ok(new { status = "online" });
        }

        [HttpGet("{playerId}")]
        public IActionResult GetPlayer(string playerId)
        {
            // Simple response with a player placeholder
            // Later you can replace this with actual FACEIT API data
            return Ok(new { playerId = playerId, name = "Krist", elo = 1500 });
        }
    }
}
