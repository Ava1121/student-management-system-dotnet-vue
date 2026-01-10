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
    public class CourseController : ControllerBase
    {
        private readonly SchoolContext _context;
        private readonly ILogger<CourseController> _logger;

        public CourseController(SchoolContext context, ILogger<CourseController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // 管理员和学生都可以查看所有课程
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Course>>> GetCourses()
        {
            _logger.LogInformation("用户获取课程列表");
            var courses = await _context.Courses.ToListAsync();
            _logger.LogInformation("成功获取 {Count} 门课程", courses.Count);
            return courses;
        }

        // 管理员和学生都可以查看单个课程
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Course>> GetCourse(int id)
        {
            _logger.LogInformation("用户获取课程详情 (ID: {CourseId})", id);
            var course = await _context.Courses.FindAsync(id);

            if (course == null)
            {
                _logger.LogWarning("尝试获取不存在的课程 (ID: {CourseId})", id);
                return NotFound();
            }

            return course;
        }

        // 只有管理员可以添加课程
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Course>> PostCourse(Course course)
        {
            _logger.LogInformation("管理员添加课程: {CourseTitle}", course.Title);
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();
            _logger.LogInformation("成功添加课程: {CourseTitle} (ID: {CourseId})", course.Title, course.Id);
            return CreatedAtAction(nameof(GetCourse), new { id = course.Id }, course);
        }

        // 只有管理员可以更新课程
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutCourse(int id, Course course)
        {
            if (id != course.Id)
            {
                return BadRequest();
            }

            _logger.LogInformation("管理员更新课程: {CourseTitle} (ID: {CourseId})", course.Title, course.Id);
            _context.Entry(course).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation("成功更新课程: {CourseTitle} (ID: {CourseId})", course.Title, course.Id);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseExists(id))
                {
                    _logger.LogWarning("尝试更新不存在的课程 (ID: {CourseId})", id);
                    return NotFound();
                }
                else
                {
                    _logger.LogError("更新课程并发冲突: {CourseTitle} (ID: {CourseId})", course.Title, course.Id);
                    throw;
                }
            }

            return NoContent();
        }

        // 只有管理员可以删除课程
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            _logger.LogInformation("管理员删除课程 (ID: {CourseId})", id);
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                _logger.LogWarning("尝试删除不存在的课程 (ID: {CourseId})", id);
                return NotFound();
            }

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            _logger.LogInformation("成功删除课程: {CourseTitle} (ID: {CourseId})", course.Title, course.Id);
            return NoContent();
        }

        // 学生获取自己的课程
        [HttpGet("my-courses")]
        [Authorize(Roles = "Student")]
        public async Task<ActionResult<IEnumerable<Course>>> GetMyCourses()
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

                // 查找该学生的所有课程，明确加载最新的课程数据
                var courses = await GetStudentCourses(studentId);
                return courses;
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // 管理员查看指定学生的课程
        [HttpGet("student/{studentId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<Course>>> GetStudentCourses(int studentId)
        {
            try
            {
                _logger.LogInformation("管理员查看学生课程 (学生ID: {StudentId})", studentId);
                
                // 查找该学生的所有课程，明确加载最新的课程数据
                var courses = await _context.Courses
                    .Join(
                        _context.Enrollments,
                        course => course.Id,
                        enrollment => enrollment.CourseId,
                        (course, enrollment) => new { course, enrollment }
                    )
                    .Where(ce => ce.enrollment.StudentId == studentId)
                    .Select(ce => ce.course)
                    .ToListAsync();

                _logger.LogInformation("成功获取学生 {StudentId} 的 {Count} 门课程", studentId, courses.Count);
                return courses;
            }
            catch (Exception ex)
            {
                _logger.LogError("获取学生课程失败: {ErrorMessage}", ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.Id == id);
        }
    }
}