using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LogController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetLogs()
        {
            try
            {
                // 日志文件路径
                string logDirectory = Path.Combine(Directory.GetCurrentDirectory(), "logs");
                
                // 检查日志目录是否存在
                if (!Directory.Exists(logDirectory))
                {
                    return Ok(new { logs = new List<string>(), message = "日志目录不存在" });
                }
                
                // 获取所有日志文件
                var logFiles = Directory.GetFiles(logDirectory, "*.txt")
                    .OrderByDescending(f => new FileInfo(f).LastWriteTime)
                    .Select(f => new
                    {
                        filename = Path.GetFileName(f),
                        path = f,
                        lastWriteTime = new FileInfo(f).LastWriteTime
                    })
                    .ToList();
                
                return Ok(new { logs = logFiles });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "获取日志文件失败", error = ex.Message });
            }
        }
        
        [HttpGet("{filename}")]
        public IActionResult GetLogContent(string filename)
        {
            try
            {
                // 日志文件路径
                string logPath = Path.Combine(Directory.GetCurrentDirectory(), "logs", filename);
                
                // 检查日志文件是否存在
                if (!System.IO.File.Exists(logPath))
                {
                    return NotFound(new { message = "日志文件不存在" });
                }
                
                // 读取日志文件内容
                string content = System.IO.File.ReadAllText(logPath);
                
                return Ok(new { content });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "读取日志文件失败", error = ex.Message });
            }
        }
    }
}