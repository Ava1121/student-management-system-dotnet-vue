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
    public class EnrollmentController : ControllerBase
    {
        private readonly SchoolContext _context;
        private readonly ILogger<EnrollmentController> _logger;

        public EnrollmentController(SchoolContext context, ILogger<EnrollmentController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<object>>> GetEnrollments()
        {
            _logger.LogInformation("管理员获取所有成绩记录");
            var enrollments = await _context.Enrollments
                .Include(e => e.Student)
                .Include(e => e.Course)
                .Select(e => new {
                    id = e.Id,
                    studentId = e.StudentId,
                    courseId = e.CourseId,
                    grade = e.Grade,
                    student = new {
                        id = e.Student.Id,
                        name = e.Student.Name,
                        age = e.Student.Age,
                        email = e.Student.Email
                    },
                    course = new {
                        id = e.Course.Id,
                        title = e.Course.Title,
                        description = e.Course.Description,
                        credits = e.Course.Credits,
                        code = e.Course.Code,
                        instructor = e.Course.Instructor
                    }
                })
                .ToListAsync();
            _logger.LogInformation("成功获取 {Count} 条成绩记录", enrollments.Count);
            return enrollments;
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Enrollment>> GetEnrollment(int id)
        {
            _logger.LogInformation("用户获取成绩详情 (ID: {EnrollmentId})", id);
            var enrollment = await _context.Enrollments
                .Include(e => e.Student)
                .Include(e => e.Course)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (enrollment == null)
            {
                _logger.LogWarning("尝试获取不存在的成绩记录 (ID: {EnrollmentId})", id);
                return NotFound();
            }

            return enrollment;
        }

        [HttpGet("student/{studentId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<object>>> GetEnrollmentsByStudentId(int studentId)
        {
            _logger.LogInformation("管理员获取学生成绩记录 (学生ID: {StudentId})", studentId);
            var enrollments = await _context.Enrollments
                .Where(e => e.StudentId == studentId)
                .Include(e => e.Course)
                .Select(e => new {
                    id = e.Id,
                    studentId = e.StudentId,
                    courseId = e.CourseId,
                    grade = e.Grade.HasValue ? e.Grade.Value.ToString() : "未评分",
                    course = new {
                        id = e.Course.Id,
                        title = e.Course.Title,
                        code = e.Course.Code,
                        instructor = e.Course.Instructor,
                        credits = e.Course.Credits
                    }
                })
                .ToListAsync();
            _logger.LogInformation("成功获取学生 {StudentId} 的 {Count} 条成绩记录", studentId, enrollments.Count);
            return enrollments;
        }

        // 学生获取自己的成绩
        [HttpGet("my-grades")]
        [Authorize(Roles = "Student")]
        public async Task<ActionResult<IEnumerable<object>>> GetMyGrades()
        {
            try
            {
                // 从JWT令牌中获取学生ID
                var username = User.Identity?.Name;
                if (string.IsNullOrEmpty(username))
                {
                    return Unauthorized("无法获取用户信息");
                }

                // 解析学生ID
                if (!int.TryParse(username, out int studentId))
                {
                    return BadRequest("无效的学生ID");
                }

                // 查找该学生的所有成绩，返回详细的成绩信息
                var grades = await _context.Enrollments
                    .Where(e => e.StudentId == studentId)
                    .Include(e => e.Course)
                    .Select(e => new {
                        id = e.Id,
                        courseId = e.CourseId,
                        courseTitle = e.Course.Title,
                        courseCode = e.Course.Code,
                        credits = e.Course.Credits,
                        instructor = e.Course.Instructor ?? "未知教师",
                        description = e.Course.Description,
                        grade = e.Grade.ToString(), // 将枚举类型转换为字符串
                        semester = "2026春" // 模拟学期数据
                    })
                    .ToListAsync();

                return Ok(grades);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Enrollment>> PostEnrollment(Enrollment enrollment)
        {
            _logger.LogInformation("管理员添加成绩记录: 学生ID {StudentId}, 课程ID {CourseId}", enrollment.StudentId, enrollment.CourseId);
            _context.Enrollments.Add(enrollment);
            await _context.SaveChangesAsync();
            _logger.LogInformation("成功添加成绩记录 (ID: {EnrollmentId})", enrollment.Id);
            return CreatedAtAction(nameof(GetEnrollment), new { id = enrollment.Id }, enrollment);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutEnrollment(int id, Enrollment enrollment)
        {
            if (id != enrollment.Id)
            {
                return BadRequest();
            }

            _logger.LogInformation("管理员更新成绩记录 (ID: {EnrollmentId})", id);
            _context.Entry(enrollment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation("成功更新成绩记录 (ID: {EnrollmentId})", enrollment.Id);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EnrollmentExists(id))
                {
                    _logger.LogWarning("尝试更新不存在的成绩记录 (ID: {EnrollmentId})", id);
                    return NotFound();
                }
                else
                {
                    _logger.LogError("更新成绩记录并发冲突 (ID: {EnrollmentId})", id);
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteEnrollment(int id)
        {
            _logger.LogInformation("管理员删除成绩记录 (ID: {EnrollmentId})", id);
            var enrollment = await _context.Enrollments.FindAsync(id);
            if (enrollment == null)
            {
                _logger.LogWarning("尝试删除不存在的成绩记录 (ID: {EnrollmentId})", id);
                return NotFound();
            }

            _context.Enrollments.Remove(enrollment);
            await _context.SaveChangesAsync();
            _logger.LogInformation("成功删除成绩记录 (ID: {EnrollmentId})", enrollment.Id);
            return NoContent();
        }

        private bool EnrollmentExists(int id)
        {
            return _context.Enrollments.Any(e => e.Id == id);
        }
    }
}