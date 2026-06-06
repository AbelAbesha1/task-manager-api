using TaskManager.API.DTOs.Auth;
using TaskManager.API.Helpers;
using TaskManager.API.Models;
using TaskManager.API.Repositories.Interfaces;

namespace TaskManager.API.Services
{
    public class AuthService
    {
        private readonly IUserRepository _userRepo;
        private readonly JwtHelper _jwtHelper;

        public AuthService(IUserRepository userRepo, JwtHelper jwtHelper)
        {
            _userRepo = userRepo;
            _jwtHelper = jwtHelper;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
        {
            var emailExists = await _userRepo.EmailExistsAsync(dto.Email);
            if (emailExists)
                throw new ConflictException("Email is already in use.");

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                PasswordHash = passwordHash,
                Role = "User"
            };

            await _userRepo.CreateAsync(user);

            return new AuthResponseDto
            {
                Token = _jwtHelper.GenerateToken(user),
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role
            };
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            var user = await _userRepo.GetByEmailAsync(dto.Email);

            if (user == null ||
                !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                throw new BadRequestException("Invalid email or password.");

            return new AuthResponseDto
            {
                Token = _jwtHelper.GenerateToken(user),
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role
            };
        }
    }
}