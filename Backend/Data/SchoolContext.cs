using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data
{
    public class SchoolContext : DbContext
    {
        public SchoolContext(DbContextOptions<SchoolContext> options) : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 配置Student表，Id不是标识列，而是手动指定
            modelBuilder.Entity<Student>()
                .ToTable("Student")
                .Property(s => s.Id)
                .ValueGeneratedNever();
            
            modelBuilder.Entity<Course>().ToTable("Course");
            modelBuilder.Entity<Enrollment>().ToTable("Enrollment");
            modelBuilder.Entity<User>().ToTable("User");

            // 添加种子数据
            // 1. 管理员用户（密码：admin123）
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Username = "admin",
                    PasswordHash = "AQAAAAEAACcQAAAAEF54t0J1cM6G4v9c3M0Z6X5Y4W3V2U1T0S9R8Q7P6O5N4M3L2K1J0I",
                    Role = "Admin"
                },
                // 学生用户（密码：password123）
                new User
                {
                    Id = 2,
                    Username = "1001",
                    PasswordHash = "AQAAAAEAACcQAAAAEF54t0J1cM6G4v9c3M0Z6X5Y4W3V2U1T0S9R8Q7P6O5N4M3L2K1J0I",
                    Role = "Student"
                },
                new User
                {
                    Id = 3,
                    Username = "1002",
                    PasswordHash = "AQAAAAEAACcQAAAAEF54t0J1cM6G4v9c3M0Z6X5Y4W3V2U1T0S9R8Q7P6O5N4M3L2K1J0I",
                    Role = "Student"
                },
                new User
                {
                    Id = 4,
                    Username = "1003",
                    PasswordHash = "AQAAAAEAACcQAAAAEF54t0J1cM6G4v9c3M0Z6X5Y4W3V2U1T0S9R8Q7P6O5N4M3L2K1J0I",
                    Role = "Student"
                },
                // 新添加的学生用户（密码：student123）
                new User
                {
                    Id = 5,
                    Username = "1004",
                    PasswordHash = "AQAAAAEAACcQAAAAEF54t0J1cM6G4v9c3M0Z6X5Y4W3V2U1T0S9R8Q7P6O5N4M3L2K1J0I",
                    Role = "Student"
                }
            );

            // 2. 学生信息
            modelBuilder.Entity<Student>().HasData(
                new Student
                {
                    Id = 1001,
                    Name = "张三",
                    Age = 20,
                    Email = "zhangsan@example.com"
                },
                new Student
                {
                    Id = 1002,
                    Name = "李四",
                    Age = 21,
                    Email = "lisi@example.com"
                },
                new Student
                {
                    Id = 1003,
                    Name = "王五",
                    Age = 20,
                    Email = "wangwu@example.com"
                },
                new Student
                {
                    Id = 1004,
                    Name = "张艳艳",
                    Age = 20,
                    Email = "zhangyanyan@example.com"
                }
            );

            // 3. 课程信息
            modelBuilder.Entity<Course>().HasData(
                new Course
                {
                    Id = 1,
                    Title = "计算机网络",
                    Description = "计算机网络基础，包括网络协议、TCP/IP模型、路由与交换、网络安全等",
                    Credits = 4,
                    Code = "CS205",
                    Instructor = "赵教授"
                },
                new Course
                {
                    Id = 2,
                    Title = "数据库系统",
                    Description = "数据库原理与应用，包括关系型数据库设计、SQL语言、事务管理、索引优化等",
                    Credits = 3,
                    Code = "CS305",
                    Instructor = "钱老师"
                },
                new Course
                {
                    Id = 3,
                    Title = "人工智能基础",
                    Description = "人工智能导论，包括机器学习、深度学习、自然语言处理、计算机视觉等基础概念",
                    Credits = 4,
                    Code = "CS401",
                    Instructor = "孙教授"
                },
                new Course
                {
                    Id = 4,
                    Title = "前端开发技术",
                    Description = "现代前端开发技术，包括HTML5、CSS3、JavaScript、Vue.js框架及响应式设计",
                    Credits = 3,
                    Code = "CS310",
                    Instructor = "周老师"
                }
            );

            // 4. 学生选课信息
            modelBuilder.Entity<Enrollment>().HasData(
                new Enrollment
                {
                    Id = 1,
                    StudentId = 1001,
                    CourseId = 1,
                    Grade = Grade.A
                },
                new Enrollment
                {
                    Id = 2,
                    StudentId = 1001,
                    CourseId = 2,
                    Grade = Grade.A
                },
                new Enrollment
                {
                    Id = 3,
                    StudentId = 1001,
                    CourseId = 3,
                    Grade = Grade.B
                },
                new Enrollment
                {
                    Id = 4,
                    StudentId = 1002,
                    CourseId = 1,
                    Grade = Grade.C
                },
                new Enrollment
                {
                    Id = 5,
                    StudentId = 1002,
                    CourseId = 3,
                    Grade = Grade.A
                },
                new Enrollment
                {
                    Id = 6,
                    StudentId = 1002,
                    CourseId = 4,
                    Grade = Grade.B
                },
                new Enrollment
                {
                    Id = 7,
                    StudentId = 1003,
                    CourseId = 2,
                    Grade = Grade.B
                },
                new Enrollment
                {
                    Id = 8,
                    StudentId = 1003,
                    CourseId = 3,
                    Grade = Grade.A
                },
                new Enrollment
                {
                    Id = 9,
                    StudentId = 1003,
                    CourseId = 4,
                    Grade = Grade.A
                }
            );
        }
    }
}