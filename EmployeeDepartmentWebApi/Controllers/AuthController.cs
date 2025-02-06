using empDeptWebApi.Models;
using EmployeeDepartmentWebApi.Authentication;
using EmployeeDepartmentWebApi.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace EmployeeDepartmentWebApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly EmployeeContext _context;
        private readonly JwtTokenService _jwtTokenService;

        public AuthController(EmployeeContext context, JwtTokenService jwtTokenService)
        {
            _context = context;
            _jwtTokenService = jwtTokenService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User login)
        {
            var user = await _context.User
                .FirstOrDefaultAsync(u => u.Email == login.Email && u.Password == login.Password);

            if (user == null) return Unauthorized("Invalid email or password.");

            var token = _jwtTokenService.GenerateToken(user.Email);

            return Ok(new { Token = token });
        }

    }
}
