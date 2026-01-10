using Backend.Data;
using Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly SchoolContext _context;
        private readonly ILogger<StudentController> _logger;

        public StudentController(SchoolContext context, ILogger<StudentController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // 管理员获取所有学生
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
        {
            _logger.LogInformation("管理员获取所有学生列表");
            var students = await _context.Students.ToListAsync();
            _logger.LogInformation("成功获取 {Count} 名学生", students.Count);
            return students;
        }

        // 管理员可以获取任何学生信息，学生只能获取自己的信息
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Student>> GetStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);

            if (student == null)
            {
                return NotFound();
            }

            // 检查当前用户角色和权限
            var username = User.Identity?.Name;
            var userRole = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;

            // 如果是学生，只能获取自己的信息
            if (userRole == "Student")
            {
                // 验证学生ID是否匹配当前登录用户
                if (!int.TryParse(username, out int currentStudentId) || currentStudentId != id)
                {
                    return Forbid("您没有权限访问其他学生的信息");
                }
            }

            return student;
        }

        // 学生获取自己的信息
        [HttpGet("profile")]
        [Authorize(Roles = "Student")]
        public async Task<ActionResult<Student>> GetStudentProfile()
        {
            try
            {
                // 从JWT令牌中获取学生ID
                var username = User.Identity?.Name;
                if (string.IsNullOrEmpty(username))
                {
                    return Unauthorized("无法获取用户信息");
                }

                // 查找当前登录的学生
                var student = await _context.Students.SingleOrDefaultAsync(s => s.Id.ToString() == username);

                if (student == null)
                {
                    return NotFound("学生信息未找到");
                }

                return student;
            }
            catch (Exception ex)
            {
                return StatusCode(500, "服务器内部错误");
            }
        }

        // 管理员添加学生
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Student>> PostStudent(Student student)
        {
            _logger.LogInformation("管理员添加学生: {StudentName}", student.Name);
            _context.Students.Add(student);
            await _context.SaveChangesAsync();
            _logger.LogInformation("成功添加学生: {StudentName} (ID: {StudentId})", student.Name, student.Id);
            return CreatedAtAction(nameof(GetStudent), new { id = student.Id }, student);
        }

        // 管理员更新学生信息
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutStudent(int id, Student student)
        {
            // 检查学生是否存在
            var existingStudent = await _context.Students.FindAsync(id);
            if (existingStudent == null)
            {
                _logger.LogWarning("尝试更新不存在的学生 (ID: {StudentId})", id);
                return NotFound();
            }

            // 如果要修改ID，先删除旧记录再创建新记录
            if (id != student.Id)
            {
                _logger.LogInformation("管理员修改学生ID: 从 {OldId} 到 {NewId}", id, student.Id);
                
                // 删除旧学生记录
                _context.Students.Remove(existingStudent);
                
                // 创建新学生记录
                _context.Students.Add(student);
            }
            else
            {
                // 更新现有学生记录的属性，避免实体跟踪冲突
                _logger.LogInformation("管理员更新学生信息: {StudentName} (ID: {StudentId})", student.Name, student.Id);
                
                // 更新现有实体的属性
                _context.Entry(existingStudent).CurrentValues.SetValues(student);
            }

            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation("成功更新学生信息: {StudentName} (ID: {StudentId})", student.Name, student.Id);
            }
            catch (DbUpdateConcurrencyException)
            {
                _logger.LogError("更新学生信息并发冲突: {StudentName} (ID: {StudentId})", student.Name, student.Id);
                throw;
            }

            return NoContent();
        }

        // 管理员删除学生
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            _logger.LogInformation("管理员删除学生 (ID: {StudentId})", id);
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                _logger.LogWarning("尝试删除不存在的学生 (ID: {StudentId})", id);
                return NotFound();
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            _logger.LogInformation("成功删除学生: {StudentName} (ID: {StudentId})", student.Name, student.Id);
            return NoContent();
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.Id == id);
        }
    }
}