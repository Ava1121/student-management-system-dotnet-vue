namespace Backend.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Credits { get; set; }
        public string Code { get; set; } // 课程代码
        public string Instructor { get; set; } // 任课教师
        public List<Enrollment>? Enrollments { get; set; }
    }
}