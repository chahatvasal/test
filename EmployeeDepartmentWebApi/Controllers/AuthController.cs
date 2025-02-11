


namespace EmployeeDepartmentWebApi.Controllers
{

    [Microsoft.AspNetCore.Components.Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly EmployeeContext _context;
        private readonly JwtTokenService _jwtTokenService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(EmployeeContext context, JwtTokenService jwtTokenService, ILogger<AuthController> logger)
        {
            _context = context;
            _jwtTokenService = jwtTokenService;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User login)
        {
            _logger.LogInformation("Login request received for user: {Email}", login.Email);

            try
            {
                var user = await _context.User.FirstOrDefaultAsync(u => u.Email == login.Email && u.Password == login.Password);

                if (user == null)
                {
                    _logger.LogWarning("Invalid login attempt for email: {Email}", login.Email);
                    return Unauthorized("Invalid email");
                }

                var token = _jwtTokenService.GenerateToken(user.Email);
                _logger.LogInformation("Token successfully generated for user: {Email}", login.Email);
                return Ok(new { Token = token });
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured during login for user: {Email}", login.Email);
                return StatusCode(500, "Internal server error");
            }
        
        }

    }
}
