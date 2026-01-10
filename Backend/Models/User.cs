namespace Backend.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; }
        public int? StudentId { get; set; } // 学生ID，关联Student表
        public Student? Student { get; set; } // 导航属性，关联Student实体
    }
}