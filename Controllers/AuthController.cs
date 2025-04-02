namespace courier_scam_finder_back_end.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using courier_scam_finder_back_end.Data;
using courier_scam_finder_back_end.Models;
using Microsoft.AspNetCore.Cors;

[Route("api/auth")]
[ApiController]
[EnableCors("AllowSpecificOrigins")]
public class AuthController : ControllerBase
{
    private readonly ScamFinderDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthController(ScamFinderDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    [HttpGet("test")]
    public IActionResult Test()
    {
        return Ok("Server is running!");
    }
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] User user)
    {
        if (await _context.Users.AnyAsync(u => u.Email == user.Email))
            return BadRequest("User already exists");

        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return Ok("User registered successfully");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
        if (existingUser == null || !BCrypt.Net.BCrypt.Verify(request.Password, existingUser.Password))
            return Unauthorized("Invalid credentials");

        var token = GenerateJwtToken(existingUser);
        return Ok(new { token });
    }

    private string GenerateJwtToken(User user)
    {
        var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
        var claims = new[]
        {
        new Claim(ClaimTypes.Name, user.Email)
    };
        var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(claims:claims, expires: DateTime.UtcNow.AddHours(3), signingCredentials: credentials);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    // DTOs (Data Transfer Objects) for API requests
    public class RegisterRequest
    {
        public string Name { get; set; }  // Full Name
        public string Email { get; set; } // Email
        public string Password { get; set; } // Password
    }

    public class LoginRequest
    {
        public string Email { get; set; } // Email
        public string Password { get; set; } // Password
    }
}

