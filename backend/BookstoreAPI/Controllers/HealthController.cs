using BookstoreAPI.Data;
using Dapper;
using Microsoft.AspNetCore.Mvc;

namespace BookstoreAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController : ControllerBase
    {
        private readonly DapperContext _context;

        public HealthController(DapperContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new { status = "API is running", timestamp = DateTime.UtcNow });
        }

        [HttpGet("database")]
        public async Task<IActionResult> CheckDatabase()
        {
            try
            {
                using var connection = _context.CreateConnection();
                var result = await connection.QueryFirstAsync<string>("SELECT DATABASE()");
                return Ok(new
                {
                    status = "Database connection successful",
                    database = result,
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    status = "Database connection failed",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }
    }
}
