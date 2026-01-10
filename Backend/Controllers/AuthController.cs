using Backend.Data;
using Backend.Models;
using Backend.Models.Auth;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly SchoolContext _context;
        private readonly JwtService _jwtService;

        public AuthController(SchoolContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            try
            {
                // 查找用户
                var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == loginDto.Username);

                if (user == null)
                {
                    return Unauthorized("用户名或密码错误");
                }

                // 验证密码 (实际项目中应该使用密码哈希验证)
                bool isPasswordValid = false;
                if (user.Role == "Admin")
                {
                    // 管理员密码验证：
                    // 1. 如果是默认密码哈希，允许使用admin123登录
                    // 2. 否则只能使用数据库中存储的密码
                    bool isDefaultPasswordHash = user.PasswordHash == "AQAAAAEAACcQAAAAEF54t0J1cM6G4v9c3M0Z6X5Y4W3V2U1T0S9R8Q7P6O5N4M3L2K1J0I";
                    isPasswordValid = (isDefaultPasswordHash && loginDto.Password == "admin123") || (loginDto.Password == user.PasswordHash);
                }
                else
                {
                    // 学生密码验证：只使用数据库中存储的密码
                    isPasswordValid = loginDto.Password == user.PasswordHash;
                }

                if (!isPasswordValid)
                {
                    return Unauthorized("用户名或密码错误");
                }

                // 生成JWT令牌
                var token = _jwtService.GenerateToken(user.Username, user.Role);

                return Ok(new { Token = token, Username = user.Username, Role = user.Role });
            }
            catch (Exception ex)
            {
                // 记录详细错误信息
                System.Diagnostics.Debug.WriteLine($"Login error: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                return StatusCode(500, $"服务器内部错误: {ex.Message}");
                
            }
        }

        [HttpPost("student-login")]
        public async Task<IActionResult> StudentLogin(LoginDto loginDto)
        {
            try
            {
                // 查找学生
                var student = await _context.Students.SingleOrDefaultAsync(s => s.Id.ToString() == loginDto.Username);

                if (student == null)
                {
                    // 如果学生不存在，直接返回错误
                    return Unauthorized("学生ID或密码错误");
                }

                // 检查是否已有对应的用户记录
                var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == student.Id.ToString());
                
                if (user == null)
                {
                    // 如果用户记录不存在，尝试查找是否有使用StudentId关联的用户
                    user = await _context.Users.SingleOrDefaultAsync(u => u.StudentId == student.Id);
                    
                    if (user == null)
                    {
                        // 如果确实没有用户记录，创建新的学生用户记录
                        user = new User
                        {
                            Username = student.Id.ToString(),
                            PasswordHash = "student123", // 实际项目中应该使用密码哈希
                            Role = "Student",
                            StudentId = student.Id
                        };
                        _context.Users.Add(user);
                        await _context.SaveChangesAsync();
                    }
                }

                // 验证密码 (实际项目中应该使用密码哈希验证)
                // 支持使用注册时设置的密码或默认密码student123
                bool isPasswordValid = false;
                if (loginDto.Password == user.PasswordHash || loginDto.Password == "student123")
                {
                    isPasswordValid = true;
                }

                if (!isPasswordValid)
                {
                    return Unauthorized("学生ID或密码错误");
                }

                // 生成JWT令牌
                var token = _jwtService.GenerateToken(user.Username, user.Role);

                return Ok(new {
                    Token = token,
                    Username = student.Name,
                    Role = user.Role,
                    StudentId = student.Id
                });
            }
            catch (Exception ex)
            {
                // 记录详细错误信息
                System.Diagnostics.Debug.WriteLine($"Student login error: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                return StatusCode(500, $"服务器内部错误: {ex.Message}");
            }
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
        {
            try
            {
                // 从请求中获取用户名（实际项目中应该从JWT令牌中解析）
                var username = changePasswordDto.Username;
                if (string.IsNullOrEmpty(username))
                {
                    return BadRequest("用户名不能为空");
                }

                // 查找用户
                var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == username);
                if (user == null)
                {
                    return NotFound("用户不存在");
                }

                // 验证旧密码
                bool isOldPasswordValid = false;
                if (user.Role == "Admin")
                {
                    // 管理员旧密码验证：支持初始密码或数据库中存储的密码
                    isOldPasswordValid = (changePasswordDto.OldPassword == "admin123") || (changePasswordDto.OldPassword == user.PasswordHash);
                }
                else
                {
                    // 学生旧密码验证：只使用数据库中存储的密码
                    isOldPasswordValid = changePasswordDto.OldPassword == user.PasswordHash;
                }

                if (!isOldPasswordValid)
                {
                    return Unauthorized("旧密码错误");
                }

                // 更新密码
                user.PasswordHash = changePasswordDto.NewPassword;
                await _context.SaveChangesAsync();

                return Ok(new { message = "密码修改成功" });
            }
            catch (Exception ex)
            {
                // 记录详细错误信息
                System.Diagnostics.Debug.WriteLine($"Change password error: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                return StatusCode(500, $"服务器内部错误: {ex.Message}");
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                // 检查学生ID是否已存在
                var existingStudent = await _context.Students.SingleOrDefaultAsync(s => s.Id == registerDto.StudentId);
                if (existingStudent != null)
                {
                    return BadRequest("该学生ID已存在");
                }

                // 检查用户表中是否已存在该学生ID
                var existingUser = await _context.Users.SingleOrDefaultAsync(u => u.Username == registerDto.StudentId.ToString());
                if (existingUser != null)
                {
                    return BadRequest("该学生ID已注册");
                }

                // 创建新学生
                var student = new Student
                {
                    Id = registerDto.StudentId,
                    Name = registerDto.Name,
                    Age = registerDto.Age,
                    Email = registerDto.Email
                };

                // 创建新用户
                var user = new User
                {
                    Username = registerDto.StudentId.ToString(),
                    PasswordHash = registerDto.Password, // 实际项目中应该使用密码哈希
                    Role = "Student",
                    StudentId = registerDto.StudentId
                };

                // 添加到数据库
                _context.Students.Add(student);
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return Ok(new { message = "注册成功" });
            }
            catch (Exception ex)
            {
                // 记录详细错误信息
                System.Diagnostics.Debug.WriteLine($"Register error: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                return StatusCode(500, $"服务器内部错误: {ex.Message}");
            }
        }
    }
}