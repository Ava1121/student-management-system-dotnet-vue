# 学生管理系统 - 业务流程与功能说明

## 一、当前系统设计（管理员端）

### 1. 业务流程

**管理员登录流程**：
1. 管理员访问登录页面 `/login`
2. 输入用户名和密码（admin/admin123）
3. 系统验证身份，生成JWT令牌
4. 管理员进入系统，可以访问所有管理功能
5. 管理员可以切换不同的管理模块（学生、课程、成绩）
6. 管理员可以退出登录，清除令牌

**学生管理流程**：
1. 管理员进入学生管理页面
2. 可以查看所有学生列表
3. 可以添加新学生
4. 可以编辑现有学生信息
5. 可以删除学生

**课程管理流程**：
1. 管理员进入课程管理页面
2. 可以查看所有课程列表
3. 可以添加新课程
4. 可以编辑现有课程信息
5. 可以删除课程

**成绩管理流程**：
1. 管理员进入成绩管理页面
2. 可以查看所有学生成绩列表
3. 可以为学生添加新成绩
4. 可以编辑现有成绩
5. 可以删除成绩

### 2. 当前系统功能

| 模块 | 功能 | 权限 |
| --- | --- | --- |
| 认证模块 | 管理员登录/退出 | 公共 |
| 学生管理 | 添加/编辑/删除/查看学生 | 管理员 |
| 课程管理 | 添加/编辑/删除/查看课程 | 管理员 |
| 成绩管理 | 添加/编辑/删除/查看成绩 | 管理员 |

## 二、系统扩展建议（学生端）

### 1. 扩展目标

将系统扩展为包含管理员端和学生端的完整学生管理系统，允许学生登录查看自己的信息和成绩。

### 2. 学生端业务流程

**学生登录流程**：
1. 学生访问登录页面 `/login`
2. 输入学生ID和密码
3. 系统验证身份，生成JWT令牌
4. 学生进入学生端首页
5. 学生可以查看自己的基本信息、课程和成绩
6. 学生可以退出登录

**学生查看信息流程**：
1. 学生登录后，进入个人中心
2. 可以查看自己的基本信息
3. 可以查看自己选修的课程
4. 可以查看自己的课程成绩

### 3. 扩展功能设计

#### 后端扩展

1. **扩展User模型**：
   - 添加学生角色
   - 添加学生ID字段
   - 支持学生密码哈希存储

2. **扩展API端点**：
   - `/api/Auth/student-login`：学生登录
   - `/api/Student/profile`：获取当前学生信息
   - `/api/Enrollment/student`：获取当前学生成绩

3. **扩展认证逻辑**：
   - 支持学生登录
   - 基于角色的权限控制

#### 前端扩展

1. **添加学生端页面**：
   - 学生个人中心
   - 学生成绩查询
   - 学生课程查询

2. **扩展路由**：
   - `/student/profile`：学生个人中心
   - `/student/courses`：学生课程
   - `/student/grades`：学生成绩

3. **扩展导航**：
   - 学生端导航菜单
   - 根据角色显示不同的导航选项

### 4. 扩展实现示例

#### 后端实现示例

```csharp
// 扩展User模型，添加学生ID
public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public string Role { get; set; }
    public int? StudentId { get; set; } // 关联学生ID
    public Student? Student { get; set; } // 导航属性
}

// 扩展StudentController，添加学生个人中心API
[HttpGet("profile")]
[Authorize(Roles = "Student")]
public async Task<IActionResult> GetStudentProfile()
{
    // 获取当前登录学生信息
    var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    var user = await _context.Users.Include(u => u.Student).SingleOrDefaultAsync(u => u.Username == username);
    return Ok(user?.Student);
}
```

#### 前端实现示例

```vue
<!-- 学生个人中心组件 -->
<template>
  <div class="student-profile">
    <h2>个人中心</h2>
    <div class="profile-info">
      <p><strong>姓名:</strong> {{ student?.name }}</p>
      <p><strong>年龄:</strong> {{ student?.age }}</p>
      <p><strong>邮箱:</strong> {{ student?.email }}</p>
    </div>
    <!-- 学生课程和成绩展示 -->
  </div>
</template>

<script>
export default {
  name: 'StudentProfile',
  // 实现学生个人中心逻辑
}
</script>
```

## 三、系统架构设计

### 1. 分层架构

**后端分层**：
- 表现层（Controllers）：处理HTTP请求和响应
- 业务逻辑层（Services）：实现业务逻辑
- 数据访问层（Data）：与数据库交互
- 模型层（Models）：定义数据结构

**前端分层**：
- 视图层（Components）：展示UI
- 业务逻辑层（Services）：处理API调用和业务逻辑
- 路由层（Router）：管理页面导航
- 状态管理：管理用户认证状态

### 2. 安全设计

- **JWT认证**：基于令牌的无状态认证
- **角色授权**：基于角色的访问控制
- **密码哈希**：存储密码哈希值，不存储明文
- **API保护**：需要认证的API添加`[Authorize]`属性
- **CORS配置**：限制跨域请求（当前允许所有）

## 四、改进建议

1. **添加学生角色**：实现学生登录和查看功能
2. **密码哈希存储**：使用BCrypt等算法存储密码哈希
3. **添加权限管理**：细化不同角色的权限
4. **添加数据验证**：增强前端和后端的数据验证
5. **添加分页功能**：处理大量数据
6. **添加搜索功能**：便于查找学生、课程等
7. **添加导出功能**：支持导出学生列表、成绩等
8. **添加日志记录**：记录系统操作日志

## 五、使用说明

### 管理员登录
- 访问地址：http://localhost:5173/login
- 用户名：admin
- 密码：admin123

### 学生端扩展

要扩展学生端功能，需要：
1. 修改User模型，添加学生角色
2. 扩展AuthController，添加学生登录
3. 添加学生端API端点
4. 创建学生端页面组件
5. 扩展路由和导航

通过以上扩展，可以将系统完善为包含管理员端和学生端的完整学生管理系统，满足不同角色的需求。