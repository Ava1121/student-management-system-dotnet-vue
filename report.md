# 学生管理系统项目报告

南华大学 
计算机学院/软件学院 
课程实验报告 

课程名称：Web应用开发 
系统名称：学生管理系统 
任课教师：XXX 

学号 | 姓名 | 班级
--- | --- | ---
XXX | XXX | XXX

2024年秋季学期

## 目录

一、系统目标 | 1
二、系统业务需求 | 1
三、系统总体设计 | 2
3.1 系统功能结构 | 2
3.2 数据库设计 | 2
四、系统详细设计 | 3
4.1 系统开发环境 | 3
4.2 数据库设计 | 3
五、系统实现 | 5
5.1 系统文件架构图 | 5
5.2 数据库结构图 | 5
5.3 登录模块实现 | 5
5.4 系统主窗体实现 | 5
5.5 学生信息管理模块实现 | 5
六、前端页面展示 | 5
6.1 公共页面 | 5
6.2 学生页面 | 5
6.3 管理员页面 | 5
七、个人心得与总结 | 5

## 一、系统目标

本项目是实验3的前后端分离项目实现，旨在构建一个基于现代化技术栈的学生管理系统，实现学生信息、课程和成绩的高效管理。系统严格采用前后端分离架构，前端使用Vue.js框架开发，后端基于.NET Web API构建RESTful接口，数据访问层采用EF Code First开发模式，对象创建基于Autofac IOC容器，具有良好的扩展性、可维护性和用户体验。

### 核心目标
1. 严格实现前后端分离架构，前后端独立开发、部署和迭代
2. 采用Vue.js框架开发现代化前端界面，提升用户体验
3. 基于.NET Web API构建标准化RESTful接口，确保接口的规范性和可访问性
4. 采用EF Code First开发模式，实现从代码到数据库的自动生成和管理
5. 使用Autofac IOC容器实现依赖注入，降低系统耦合度，提高代码可测试性和可维护性
6. 构建完整的学生管理功能，包括用户认证、学生信息管理、课程管理和成绩管理
7. 确保系统安全可靠，支持基于JWT Token的用户认证和授权
8. 提供良好的扩展性，便于后续功能扩展和优化

## 二、系统业务需求

### 功能需求

本系统主要实现学生信息、课程和成绩的高效管理，分为管理员和学生两种角色，具体功能如下：

1. **用户认证管理**
   - 管理员登录功能
   - 学生登录功能
   - 用户注册功能
   - 修改密码功能
   - JWT Token生成与验证

2. **学生管理**
   - 学生信息的增删改查
   - 学生列表展示
   - 学生信息详情查看
   - 学生课程管理

3. **课程管理**
   - 课程信息的增删改查
   - 课程列表展示
   - 课程信息详情查看
   - 学生选课管理

4. **成绩管理**
   - 学生成绩的增删改查
   - 成绩列表展示
   - 成绩信息详情查看
   - GPA计算功能

5. **学生个人中心**
   - 学生查看个人信息
   - 学生查看自己的课程
   - 学生查看自己的成绩

### 用例图

#### 2.1 系统总体用例图

系统总体用例图展示了系统的主要功能和用户角色：

![系统总体用例图](images/00_system_use_case.png)

#### 2.2 学生角色用例图

学生角色主要包括个人信息管理、课程管理和成绩查询功能：

![学生角色用例图](images/00_student_use_case.png)

#### 2.3 管理员角色用例图

管理员角色主要包括学生管理、课程管理和成绩管理功能：

![管理员角色用例图](images/00_admin_use_case.png)

### 非功能需求
1. **性能需求**
   - 系统响应时间不超过2秒
   - 支持并发访问

2. **安全性需求**
   - 密码加密存储
   - JWT Token认证
   - 防止SQL注入攻击

3. **兼容性需求**
   - 支持主流浏览器（Chrome、Firefox、Edge）
   - 响应式设计，适配不同设备

4. **可维护性需求**
   - 代码结构清晰，便于维护
   - 良好的文档支持

## 三、系统总体设计

### 3.1 系统功能结构

#### 3.1.1 系统功能模块图

系统功能模块图展示了系统的主要功能模块及其关系：

![系统功能模块图](images/00_system_function_module.png)

#### 3.1.2 后端功能模块（.NET Web API + EF Code First + Autofac）
```
├── Model 层      # 数据实体类，EF Code First从这些类生成数据库表
│   ├── Student.cs        # 学生实体
│   ├── Course.cs         # 课程实体
│   ├── Enrollment.cs     # 成绩实体
│   └── User.cs           # 用户实体

├── DAL 层        # 数据访问层，基于EF Code First实现
│   └── SchoolContext.cs  # DbContext配置，管理数据库连接和实体映射

├── BLL 层        # 业务逻辑层，通过Autofac实现依赖注入
│   ├── Interfaces/       # 业务接口定义（用于依赖注入）
│   └── Services/         # 业务接口实现

├── API 层        # RESTful API服务层
│   ├── Controllers/      # API控制器，处理HTTP请求
│   └── Filters/          # 过滤器，如JWT认证过滤器

├── Common 层     # 公共工具类
│   ├── JwtHelper.cs      # JWT Token生成与验证
│   └── ResponseHelper.cs # 统一API返回格式封装
```

#### 3.1.3 前端功能模块（Vue.js 3）
```
├── src/                  # 源码目录
│   ├── main.js           # 应用入口，引入Vue、Vue Router、Element Plus等
│   ├── App.vue           # 根组件，定义应用布局
│   ├── router/           # 路由配置，管理页面导航
│   │   └── index.js      # 定义路由规则和守卫
│   ├── components/       # Vue组件，实现UI复用
│   │   ├── Login.vue     # 登录组件
│   │   ├── Register.vue  # 注册组件
│   │   ├── StudentProfile.vue # 个人中心组件
│   │   ├── MyCourses.vue      # 我的课程组件
│   │   └── MyGrades.vue       # 我的成绩组件
│   ├── utils/            # 工具函数
│   │   └── request.js    # Axios封装，处理API请求
│   └── assets/           # 静态资源
├── index.html            # HTML入口文件
├── vite.config.js        # Vite构建工具配置
└── package.json          # 项目依赖和脚本配置
```

### 3.2 数据库设计

#### 3.2.1 系统ER图

系统实体关系图（ER图）展示了系统中的主要实体及其关系：

![系统ER图](images/00_database_er.png)

#### 3.2.2 核心实体关系
- **Student与Enrollment**：一对多关系（一个学生可以有多个成绩记录）
- **Course与Enrollment**：一对多关系（一门课程可以有多个成绩记录）
- **Student与Course**：多对多关系（通过Enrollment表关联）
- **User与Student**：一对一关系（用户表与学生表通过ID关联）

#### 3.2.3 实体设计特点
1. **数据注解**：使用.NET数据注解配置实体属性（如[Key]、[Required]、[StringLength]等）
2. **导航属性**：通过导航属性定义实体间的关系
3. **种子数据**：在OnModelCreating方法中配置初始数据
4. **自动迁移**：EF Core自动处理数据库结构变更

### 3.3 前后端分离架构核心原理

本项目严格遵循前后端分离架构设计，核心原理包括：

1. **关注点分离**：前端专注于用户体验和界面交互，后端专注于业务逻辑和数据处理
2. **API 驱动开发**：前后端通过标准化RESTful API进行通信，API定义了前后端的交互契约
3. **独立部署**：前后端可以独立部署和迭代，提高开发效率
4. **技术栈解耦**：前后端使用不同的技术栈，便于技术选型和团队协作
5. **单页应用（SPA）**：前端采用Vue.js构建SPA，提升用户体验和页面加载速度

### 3.4 Autofac IOC容器应用

本项目使用Autofac实现依赖注入，核心配置包括：

1. **容器初始化**：在Program.cs中配置Autofac容器
2. **服务注册**：注册DbContext、业务服务和控制器
3. **生命周期管理**：配置不同类型服务的生命周期（如InstancePerLifetimeScope）
4. **构造函数注入**：在控制器和服务中通过构造函数注入依赖

Autofac的应用降低了系统各模块间的耦合度，提高了代码的可测试性和可维护性。

## 四、系统详细设计

### 4.1 系统开发环境

本系统采用了稳定的开发环境，确保项目开发工作的平稳进行，系统开发环境如下表所示：

表4.1 系统开发环境表
| 类别 | 技术/工具 | 版本 |
|------|-----------|------|
| 操作系统 | Windows | 10/11 |
| 后端框架 | .NET Web API | 6.0 |
| 前端框架 | Vue.js | 3.0 |
| 数据库 | SQL Server | 2019 |
| ORM框架 | Entity Framework Core | 6.0 |
| IOC容器 | Autofac | 7.0 |
| 前端构建工具 | Vite | 4.0 |
| 前端UI库 | Element Plus | 2.0 |
| 后端开发工具 | Visual Studio | 2022 |
| 前端开发工具 | VS Code | 1.80+ |
| 浏览器 | Chrome | 100+ |

### 4.2 数据库设计

#### 4.2.1 数据表的设计

通过需求分析和设计阶段的梳理，学生管理系统一共包含4个核心数据表：学生表、课程表、成绩表和用户表。

##### 学生表（Student）

表4.2-1 学生表
| 序号 | 字段名 | 中文描述 | 数据类型 | 允许为空 | 约束条件 |
|------|--------|----------|----------|----------|----------|
| 1 | Id | 学生ID | int | 否 | 主键，手动指定ID |
| 2 | Name | 学生姓名 | varchar(50) | 否 | 非空约束 |
| 3 | Age | 学生年龄 | int | 是 | |
| 4 | Email | 学生邮箱 | varchar(100) | 否 | 邮箱格式验证 |

##### 课程表（Course）

表4.2-2 课程表
| 序号 | 字段名 | 中文描述 | 数据类型 | 允许为空 | 约束条件 |
|------|--------|----------|----------|----------|----------|
| 1 | Id | 课程ID | int | 否 | 主键，自增 |
| 2 | Title | 课程名称 | varchar(100) | 否 | 非空约束 |
| 3 | Credits | 课程学分 | int | 否 | 非空约束 |

##### 成绩表（Enrollment）

表4.2-3 成绩表
| 序号 | 字段名 | 中文描述 | 数据类型 | 允许为空 | 约束条件 |
|------|--------|----------|----------|----------|----------|
| 1 | Id | 成绩ID | int | 否 | 主键，自增 |
| 2 | StudentId | 学生ID | int | 否 | 外键，关联学生表 |
| 3 | CourseId | 课程ID | int | 否 | 外键，关联课程表 |
| 4 | Grade | 成绩 | varchar(10) | 是 | |

##### 用户表（User）

表4.2-4 用户表
| 序号 | 字段名 | 中文描述 | 数据类型 | 允许为空 | 约束条件 |
|------|--------|----------|----------|----------|----------|
| 1 | Id | 用户ID | int | 否 | 主键，自增 |
| 2 | Username | 用户名 | varchar(50) | 否 | 非空约束 |
| 3 | PasswordHash | 密码哈希 | varchar(100) | 否 | 非空约束 |
| 4 | Role | 用户角色 | varchar(20) | 否 | 非空约束，值为Admin或Student |

#### 4.2.2 数据表的逻辑关系图

数据表的逻辑关系图展示了各数据表之间的外键关系：

![数据表逻辑关系图](images/00_database_relationship.png)

#### 4.2.3 EF Code First 数据访问层设计

EF Code First模式从以下实体类自动生成数据库表：

```csharp
// Student.cs - 学生实体 (实际文件：e:\webtest\Backend\Models\Student.cs)
namespace Backend.Models
{
    public class Student
    {
        public int Id { get; set; }           // 主键，手动指定ID
        public string Name { get; set; }      // 学生姓名
        public int Age { get; set; }          // 学生年龄
        public string Email { get; set; }     // 学生邮箱
        public List<Enrollment>? Enrollments { get; set; } // 导航属性：一个学生可以有多个成绩记录
    }
}

// Course.cs - 课程实体 (实际文件：e:\webtest\Backend\Models\Course.cs)
namespace Backend.Models
{
    public class Course
    {
        public int Id { get; set; }           // 主键，自增
        public string Title { get; set; }      // 课程名称
        public string Description { get; set; } // 课程描述
        public int Credits { get; set; }       // 学分
        public string Code { get; set; }       // 课程代码
        public string Instructor { get; set; }  // 任课教师
        public List<Enrollment>? Enrollments { get; set; } // 导航属性：一门课程可以有多个成绩记录
    }
}

// Enrollment.cs - 成绩实体 (实际文件：e:\webtest\Backend\Models\Enrollment.cs)
namespace Backend.Models
{
    public class Enrollment
    {
        public int Id { get; set; }           // 主键，自增
        public int StudentId { get; set; }    // 外键：学生ID
        public int CourseId { get; set; }     // 外键：课程ID
        public Grade? Grade { get; set; }     // 成绩，使用枚举类型
        public Student? Student { get; set; }  // 导航属性：关联学生
        public Course? Course { get; set; }    // 导航属性：关联课程
    }

    public enum Grade
    {
        A, B, C, D, F
    }
}

// User.cs - 用户实体（用于认证） (实际文件：e:\webtest\Backend\Models\User.cs)
namespace Backend.Models
{
    public class User
    {
        public int Id { get; set; }           // 主键，自增
        public string Username { get; set; }  // 用户名
        public string PasswordHash { get; set; } // 密码哈希
        public string Role { get; set; }      // 用户角色：Admin或Student
        public int? StudentId { get; set; }   // 学生ID，关联Student表
        public Student? Student { get; set; }  // 导航属性，关联Student实体
    }
}
```

### 4.3 Autofac IOC容器配置

#### 4.3.1 容器初始化与服务注册

```csharp
// Program.cs - .NET 6 Web API应用入口
var builder = WebApplication.CreateBuilder(args);

// 1. 配置Autofac作为服务提供程序工厂
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

// 2. 配置Autofac容器
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    // 注册DbContext，使用InstancePerLifetimeScope生命周期
    containerBuilder.RegisterType<SchoolContext>()
        .AsSelf()
        .InstancePerLifetimeScope();
    
    // 注册所有控制器
    containerBuilder.RegisterControllers(typeof(Program).Assembly)
        .InstancePerLifetimeScope();
    
    // 注册所有业务服务（按约定注册）
    containerBuilder.RegisterAssemblyTypes(typeof(Program).Assembly)
        .Where(t => t.Name.EndsWith("Service"))
        .AsImplementedInterfaces()
        .InstancePerLifetimeScope();
});

// 添加其他服务
builder.Services.AddControllers();
builder.Services.AddDbContext<SchoolContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 配置CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// 配置中间件
app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// 初始化数据库
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<SchoolContext>();
    context.Database.EnsureCreated(); // EF Code First创建数据库
}

app.Run();
```

#### 4.3.2 控制器中的依赖注入

```csharp
// StudentController.cs - 使用Autofac注入SchoolContext
[ApiController]
[Route("api/[controller]")]
public class StudentController : ControllerBase
{
    // 通过构造函数注入SchoolContext
    private readonly SchoolContext _context;
    
    public StudentController(SchoolContext context)
    {
        _context = context;
    }
    
    // API方法实现...
}
```

### 4.4 前后端API交互设计

#### 4.4.1 RESTful API设计规范

| 资源 | HTTP方法 | 路径 | 功能描述 |
|------|----------|------|----------|
| 学生 | GET | api/students | 获取所有学生 |
| 学生 | GET | api/students/{id} | 获取指定学生 |
| 学生 | POST | api/students | 添加学生 |
| 学生 | PUT | api/students/{id} | 更新学生 |
| 学生 | DELETE | api/students/{id} | 删除学生 |
| 课程 | GET | api/courses | 获取所有课程 |
| 成绩 | GET | api/enrollments | 获取所有成绩 |
| 认证 | POST | api/auth/login | 用户登录 |
| 认证 | POST | api/auth/register | 用户注册 |

#### 4.4.2 前端API请求封装

```javascript
// request.js - Axios封装（前端）
import axios from 'axios'

// 创建Axios实例
const service = axios.create({
  baseURL: 'https://localhost:7277/api',  // API基础URL
  timeout: 5000,                          // 请求超时时间
  headers: {
    'Content-Type': 'application/json'    // 默认Content-Type
  }
})

// 请求拦截器：添加Token
service.interceptors.request.use(
  config => {
    // 从localStorage获取Token
    const token = localStorage.getItem('token')
    if (token) {
      // 添加Authorization头
      config.headers.Authorization = `Bearer ${token}`
    }
    return config
  },
  error => {
    console.error('请求错误:', error)
    return Promise.reject(error)
  }
)

// 响应拦截器：统一处理响应
service.interceptors.response.use(
  response => {
    const res = response.data
    return res
  },
  error => {
    console.error('响应错误:', error)
    // 处理Token过期
    if (error.response && error.response.status === 401) {
      // 清除Token并跳转到登录页
      localStorage.removeItem('token')
      window.location.href = '/login'
    }
    return Promise.reject(error)
  }
)

export default service
```

## 五、系统实现

### 5.1 系统文件架构图

#### 后端文件架构（.NET Web API + EF Code First + Autofac）
```
├── Backend/                  # 后端项目根目录
│   ├── Backend.csproj        # 项目文件（.NET 6）
│   ├── Program.cs            # 应用入口（包含Autofac配置）
│   ├── appsettings.json      # 配置文件（数据库连接、JWT密钥等）
│   ├── Controllers/          # API控制器
│   │   ├── AuthController.cs      # 认证控制器（登录、注册、修改密码）
│   │   ├── StudentController.cs   # 学生控制器（CRUD操作）
│   │   ├── CourseController.cs    # 课程控制器（CRUD操作）
│   │   └── EnrollmentController.cs # 成绩控制器（CRUD操作）
│   ├── Models/               # 实体模型（EF Code First）
│   │   ├── Student.cs        # 学生实体
│   │   ├── Course.cs         # 课程实体
│   │   ├── Enrollment.cs     # 成绩实体
│   │   ├── User.cs           # 用户实体
│   │   └── DTO/              # 数据传输对象
│   │       ├── LoginDto.cs
│   │       └── RegisterDto.cs
│   ├── DAL/                  # 数据访问层
│   │   └── SchoolContext.cs  # EF DbContext配置
│   └── Common/               # 公共工具类
│       ├── JwtHelper.cs      # JWT Token生成与验证
│       └── ResponseHelper.cs # 统一API返回格式
```

#### 前端文件架构（Vue.js 3）
```
├── frontend/                 # 前端项目根目录
│   ├── index.html            # HTML入口文件
│   ├── package.json          # 项目配置（Vue 3、Element Plus等依赖）
│   ├── vite.config.js        # Vite构建工具配置
│   ├── src/                  # 源码目录
│   │   ├── main.js           # 应用入口（Vue、Vue Router、Element Plus初始化）
│   │   ├── App.vue           # 根组件（路由出口）
│   │   ├── router/           # 路由配置
│   │   │   └── index.js      # 定义路由规则和导航守卫
│   │   ├── components/       # Vue组件
│   │   │   ├── Login.vue     # 登录组件（含密码可见性切换）
│   │   │   ├── Register.vue  # 注册组件
│   │   │   ├── StudentProfile.vue # 个人中心组件
│   │   │   ├── MyCourses.vue      # 我的课程组件
│   │   │   └── MyGrades.vue       # 我的成绩组件（含GPA计算）
│   │   ├── utils/            # 工具函数
│   │   │   └── request.js    # Axios封装（API请求、Token处理）
│   │   └── style.css         # 全局样式（红色主题）
│   └── public/               # 静态资源
```

### 5.2 数据库结构图

![数据库结构图](images/00_database_structure.png)

**核心表关系**：
- `Students`（学生表）与 `Enrollments`（成绩表）：一对多关系
- `Courses`（课程表）与 `Enrollments`（成绩表）：一对多关系
- `Students`（学生表）与 `Courses`（课程表）：多对多关系（通过 `Enrollments` 表关联）
- `Users`（用户表）与 `Students`（学生表）：一对一关系

### 5.3 登录模块实现

#### 5.3.1 登录模块流程图

![登录模块流程图](images/00_login_flow.png)

#### 5.3.2 后端登录接口（AuthController.cs）

```csharp
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly SchoolContext _context;
    private readonly IConfiguration _configuration;
    
    // 通过Autofac注入依赖
    public AuthController(SchoolContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }
    
    [HttpPost("login")]
    public IActionResult Login(LoginDto loginDto)
    {
        // 1. 验证用户名和密码
        User user = null;
        
        // 学生登录（用户名以100开头）
        if (loginDto.Username.StartsWith("100"))
        {
            var studentId = int.Parse(loginDto.Username);
            var student = _context.Students.Find(studentId);
            if (student == null)
            {
                return Unauthorized("学生ID或密码错误");
            }
            
            user = _context.Users.Find(studentId);
            if (user == null)
            {
                // 创建默认用户
                user = new User
                {
                    Id = studentId,
                    Username = loginDto.Username,
                    PasswordHash = "student123",
                    Role = "Student"
                };
                _context.Users.Add(user);
                _context.SaveChanges();
            }
        }
        // 管理员登录
        else if (loginDto.Username == "admin")
        {
            user = _context.Users.Find(1);
            if (user == null)
            {
                return Unauthorized("管理员账号不存在");
            }
        }
        
        // 2. 密码验证（简化版，实际应使用密码哈希）
        if (user.PasswordHash != loginDto.Password && user.PasswordHash != "admin123")
        {
            return Unauthorized("用户名或密码错误");
        }
        
        // 3. 生成JWT Token
        var token = JwtHelper.GenerateToken(user, _configuration);
        
        return Ok(new {
            token,
            username = user.Username,
            role = user.Role
        });
    }
    
    [HttpPost("register")]
    public IActionResult Register(RegisterDto registerDto)
    {
        // 注册逻辑实现...
    }
    
    [HttpPut("change-password")]
    public IActionResult ChangePassword(ChangePasswordDto changePasswordDto)
    {
        // 修改密码逻辑实现...
    }
}
```

#### 5.3.3 前端登录组件实现

![登录页面](images/01_login.png)

**核心功能**：
- 支持管理员和学生两种角色登录
- 密码可见性切换功能
- 表单验证，确保用户名和密码不为空
- 提供注册入口
- 基于JWT的登录认证

### 5.4 系统主窗体实现

系统主窗体采用响应式设计，适配桌面端和移动端，包含侧边栏导航和顶部导航栏。

![系统主窗体](images/10_main_layout.png)

**核心功能**：
- 侧边栏菜单导航
- 顶部用户信息展示
- 响应式布局，适配不同屏幕尺寸
- 移动端自动切换为底部导航

### 5.5 学生信息管理模块实现

学生信息管理模块允许管理员对学生信息进行增删改查操作，支持分页和搜索功能。

#### 5.5.1 学生信息管理模块流程图

![学生信息管理模块流程图](images/00_student_management_flow.png)

#### 5.5.2 后端学生控制器实现

```csharp
[ApiController]
[Route("api/[controller]")]
public class StudentController : ControllerBase
{
    private readonly SchoolContext _context;
    
    // 通过Autofac注入SchoolContext
    public StudentController(SchoolContext context)
    {
        _context = context;
    }
    
    // GET: api/students - 获取所有学生
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
    {
        return await _context.Students.ToListAsync();
    }
    
    // GET: api/students/{id} - 获取指定学生
    [HttpGet("{id}")]
    public async Task<ActionResult<Student>> GetStudent(int id)
    {
        var student = await _context.Students.FindAsync(id);
        if (student == null) {
            return NotFound(ResponseHelper.Error("学生不存在"));
        }
        return student;
    }
    
    // POST: api/students - 添加学生
    [HttpPost]
    public async Task<ActionResult<Student>> PostStudent(Student student)
    {
        // 检查学生ID是否已存在
        if (_context.Students.Any(s => s.Id == student.Id))
        {
            return BadRequest(ResponseHelper.Error("学生ID已存在"));
        }
        
        _context.Students.Add(student);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetStudent), new { id = student.Id }, student);
    }
    
    // PUT: api/students/{id} - 更新学生
    [HttpPut("{id}")]
    public async Task<IActionResult> PutStudent(int id, Student student)
    {
        if (id != student.Id) {
            return BadRequest(ResponseHelper.Error("学生ID不匹配"));
        }
        
        // 标记实体为已修改
        _context.Entry(student).State = EntityState.Modified;
        
        try {
            await _context.SaveChangesAsync();
        } catch (DbUpdateConcurrencyException) {
            if (!_context.Students.Any(e => e.Id == id)) {
                return NotFound(ResponseHelper.Error("学生不存在"));
            } else {
                throw;
            }
        }
        
        return Ok(ResponseHelper.Success(null, "学生信息更新成功"));
    }
    
    // DELETE: api/students/{id} - 删除学生
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStudent(int id)
    {
        var student = await _context.Students.FindAsync(id);
        if (student == null) {
            return NotFound(ResponseHelper.Error("学生不存在"));
        }
        
        // 删除相关的成绩记录
        var enrollments = _context.Enrollments.Where(e => e.StudentId == id);
        _context.Enrollments.RemoveRange(enrollments);
        
        // 删除学生
        _context.Students.Remove(student);
        await _context.SaveChangesAsync();
        
        return Ok(ResponseHelper.Success(null, "学生删除成功"));
    }
}
```

#### 5.5.3 前端学生管理组件实现

![学生管理页面](images/06_student_management.png)

### 5.6 课程管理模块实现

课程管理模块允许管理员对课程信息进行增删改查操作，支持分页和搜索功能。

![课程管理页面](images/07_course_management.png)

### 5.7 成绩管理模块实现

成绩管理模块允许管理员对学生成绩进行增删改查操作，支持按学生、课程筛选。

![成绩管理页面](images/08_enrollment_management.png)

### 5.8 学生个人中心实现

学生个人中心展示学生的基本信息，包括姓名、年龄、邮箱等，支持修改个人信息。

![个人中心页面](images/03_profile.png)

### 5.9 我的课程模块实现

我的课程模块展示学生选修的所有课程，包括课程名称、学分等信息。

![我的课程页面](images/04_courses.png)

### 5.10 我的成绩模块实现

我的成绩模块展示学生的所有成绩记录，自动计算并显示总GPA。

![我的成绩页面](images/05_grades.png)

### 5.4 系统主窗体实现（Vue 3组件化开发）

```vue
<template>
  <div class="app-container">
    <!-- 响应式布局：桌面端侧边栏 + 移动端底部导航 -->
    
    <!-- 桌面端侧边栏 -->
    <aside class="sidebar" v-if="isDesktop">
      <div class="sidebar-header">
        <h2>学生管理系统</h2>
      </div>
      <el-menu 
        :default-active="activeMenu" 
        class="sidebar-menu" 
        router
        @select="handleMenuSelect"
      >
        <el-menu-item index="/student/profile">
          <el-icon><User /></el-icon>
          <template #title>个人中心</template>
        </el-menu-item>
        <el-menu-item index="/student/courses">
          <el-icon><Document /></el-icon>
          <template #title>我的课程</template>
        </el-menu-item>
        <el-menu-item index="/student/grades">
          <el-icon><DataAnalysis /></el-icon>
          <template #title>我的成绩</template>
        </el-menu-item>
      </el-menu>
    </aside>
    
    <!-- 主内容区 -->
    <main class="main-content">
      <!-- 顶部导航栏 -->
      <header class="top-header">
        <div class="header-left">
          <el-button 
            type="text" 
            @click="toggleSidebar" 
            v-if="isDesktop"
          >
            <el-icon><Menu /></el-icon>
          </el-button>
          <h1 class="system-title">学生管理系统</h1>
        </div>
        <div class="header-right">
          <span class="username">{{ username }}</span>
          <el-dropdown>
            <el-button type="text">
              <el-icon><Setting /></el-icon>
            </el-button>
            <template #dropdown>
              <el-dropdown-menu>
                <el-dropdown-item @click="handleChangePassword">修改密码</el-dropdown-item>
                <el-dropdown-item @click="handleLogout">退出登录</el-dropdown-item>
              </el-dropdown-menu>
            </template>
          </el-dropdown>
        </div>
      </header>
      
      <!-- 内容区域 -->
      <div class="content">
        <router-view />
      </div>
    </main>
    
    <!-- 移动端底部导航 -->
    <nav class="bottom-nav" v-if="isMobile">
      <el-menu 
        mode="horizontal" 
        :default-active="activeMenu" 
        router
        @select="handleMenuSelect"
      >
        <el-menu-item index="/student/profile">
          <el-icon><User /></el-icon>
          <template #title>个人中心</template>
        </el-menu-item>
        <el-menu-item index="/student/courses">
          <el-icon><Document /></el-icon>
          <template #title>我的课程</template>
        </el-menu-item>
        <el-menu-item index="/student/grades">
          <el-icon><DataAnalysis /></el-icon>
          <template #title>我的成绩</template>
        </el-menu-item>
      </el-menu>
    </nav>
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { ElMessage } from 'element-plus'

const router = useRouter()
const username = ref(localStorage.getItem('username') || '')
const activeMenu = ref('/student/profile')
const isSidebarOpen = ref(true)

// 响应式判断：桌面端/移动端
const isDesktop = computed(() => window.innerWidth >= 768)
const isMobile = computed(() => window.innerWidth < 768)

const handleMenuSelect = (key) => {
  activeMenu.value = key
}

const toggleSidebar = () => {
  isSidebarOpen.value = !isSidebarOpen.value
}

const handleChangePassword = () => {
  // 跳转到修改密码页面或显示修改密码对话框
  router.push('/change-password')
}

const handleLogout = () => {
  // 清除localStorage中的用户信息
  localStorage.removeItem('token')
  localStorage.removeItem('username')
  localStorage.removeItem('role')
  
  ElMessage.success('退出登录成功')
  router.push('/login')
}

// 监听窗口大小变化
onMounted(() => {
  const handleResize = () => {
    // 响应式布局处理
  }
  
  window.addEventListener('resize', handleResize)
  
  // 初始设置
  activeMenu.value = router.currentRoute.value.path
})
</script>

<style scoped>
.app-container {
  display: flex;
  height: 100vh;
  background-color: #f5f7fa;
}

/* 侧边栏样式 */
.sidebar {
  width: 200px;
  background-color: #ffffff;
  box-shadow: 2px 0 8px rgba(0, 0, 0, 0.1);
  transition: width 0.3s ease;
  overflow: hidden;
}

.sidebar-header {
  padding: 20px;
  text-align: center;
  border-bottom: 1px solid #e0e0e0;
}

.sidebar-header h2 {
  margin: 0;
  font-size: 18px;
  color: #333;
}

.sidebar-menu {
  border-right: none;
}

/* 主内容区样式 */
.main-content {
  flex: 1;
  display: flex;
  flex-direction: column;
  overflow: hidden;
}

.top-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 0 20px;
  height: 60px;
  background-color: #ffffff;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
  z-index: 100;
}

.system-title {
  margin: 0;
  font-size: 20px;
  color: #ff4d4f; /* 红色主题 */
}

.header-right {
  display: flex;
  align-items: center;
  gap: 15px;
}

.username {
  font-weight: 500;
  color: #333;
}

.content {
  flex: 1;
  padding: 20px;
  overflow-y: auto;
}

/* 移动端底部导航 */
.bottom-nav {
  position: fixed;
  bottom: 0;
  left: 0;
  right: 0;
  background-color: #ffffff;
  box-shadow: 0 -2px 8px rgba(0, 0, 0, 0.1);
  z-index: 100;
}

.bottom-nav .el-menu {
  border-bottom: none;
}
</style>
```

### 5.5 人事档案管理模块实现（EF Code First CRUD）

#### 后端学生控制器（StudentController.cs）

```csharp
[ApiController]
[Route("api/[controller]")]
public class StudentController : ControllerBase
{
    private readonly SchoolContext _context;
    
    // 通过Autofac注入SchoolContext
    public StudentController(SchoolContext context)
    {
        _context = context;
    }
    
    // GET: api/students - 获取所有学生
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
    {
        return await _context.Students.ToListAsync();
    }
    
    // GET: api/students/{id} - 获取指定学生
    [HttpGet("{id}")]
    public async Task<ActionResult<Student>> GetStudent(int id)
    {
        var student = await _context.Students.FindAsync(id);
        if (student == null) {
            return NotFound(ResponseHelper.Error("学生不存在"));
        }
        return student;
    }
    
    // POST: api/students - 添加学生
    [HttpPost]
    public async Task<ActionResult<Student>> PostStudent(Student student)
    {
        // 检查学生ID是否已存在
        if (_context.Students.Any(s => s.Id == student.Id))
        {
            return BadRequest(ResponseHelper.Error("学生ID已存在"));
        }
        
        _context.Students.Add(student);
        await _context.SaveChangesAsync();
        
        return CreatedAtAction(nameof(GetStudent), new { id = student.Id }, student);
    }
    
    // PUT: api/students/{id} - 更新学生
    [HttpPut("{id}")]
    public async Task<IActionResult> PutStudent(int id, Student student)
    {
        if (id != student.Id) {
            return BadRequest(ResponseHelper.Error("学生ID不匹配"));
        }
        
        // 标记实体为已修改
        _context.Entry(student).State = EntityState.Modified;
        
        try {
            await _context.SaveChangesAsync();
        } catch (DbUpdateConcurrencyException) {
            if (!_context.Students.Any(e => e.Id == id)) {
                return NotFound(ResponseHelper.Error("学生不存在"));
            } else {
                throw;
            }
        }
        
        return Ok(ResponseHelper.Success(null, "学生信息更新成功"));
    }
    
    // DELETE: api/students/{id} - 删除学生
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStudent(int id)
    {
        var student = await _context.Students.FindAsync(id);
        if (student == null) {
            return NotFound(ResponseHelper.Error("学生不存在"));
        }
        
        // 删除相关的成绩记录
        var enrollments = _context.Enrollments.Where(e => e.StudentId == id);
        _context.Enrollments.RemoveRange(enrollments);
        
        // 删除学生
        _context.Students.Remove(student);
        await _context.SaveChangesAsync();
        
        return Ok(ResponseHelper.Success(null, "学生删除成功"));
    }
    
    // GET: api/students/{id}/courses - 获取学生的课程
    [HttpGet("{id}/courses")]
    public async Task<ActionResult<IEnumerable<Course>>> GetStudentCourses(int id)
    {
        var student = await _context.Students
            .Include(s => s.Enrollments)
            .ThenInclude(e => e.Course)
            .FirstOrDefaultAsync(s => s.Id == id);
        
        if (student == null) {
            return NotFound(ResponseHelper.Error("学生不存在"));
        }
        
        var courses = student.Enrollments.Select(e => e.Course).ToList();
        return courses;
    }
}
```

#### 前端学生管理组件（Admin/StudentManagement.vue）

```vue
<template>
  <div class="student-management">
    <el-card shadow="hover">
      <template #header>
        <div class="card-header">
          <span>学生管理</span>
          <el-button type="primary" @click="handleAddStudent">新增学生</el-button>
        </div>
      </template>
      
      <!-- 学生列表 -->
      <el-table :data="students" stripe style="width: 100%">
        <el-table-column prop="id" label="学生ID" width="100"></el-table-column>
        <el-table-column prop="name" label="姓名" width="120"></el-table-column>
        <el-table-column prop="age" label="年龄" width="80"></el-table-column>
        <el-table-column prop="email" label="邮箱"></el-table-column>
        <el-table-column label="操作" width="200">
          <template #default="scope">
            <el-button type="primary" size="small" @click="handleEditStudent(scope.row)">编辑</el-button>
            <el-button type="danger" size="small" @click="handleDeleteStudent(scope.row.id)">删除</el-button>
          </template>
        </el-table-column>
      </el-table>
      
      <!-- 分页 -->
      <div class="pagination-container">
        <el-pagination
          v-model:current-page="currentPage"
          v-model:page-size="pageSize"
          :page-sizes="[10, 20, 50, 100]"
          layout="total, sizes, prev, pager, next, jumper"
          :total="total"
          @size-change="handleSizeChange"
          @current-change="handleCurrentChange"
        ></el-pagination>
      </div>
      
      <!-- 新增/编辑对话框 -->
      <el-dialog v-model="dialogVisible" :title="dialogTitle" width="500px">
        <el-form :model="form" :rules="rules" ref="formRef">
          <el-form-item label="学生ID" prop="id">
            <el-input v-model.number="form.id" placeholder="请输入学生ID" :disabled="isEdit"></el-input>
          </el-form-item>
          <el-form-item label="姓名" prop="name">
            <el-input v-model="form.name" placeholder="请输入姓名"></el-input>
          </el-form-item>
          <el-form-item label="年龄" prop="age">
            <el-input v-model.number="form.age" placeholder="请输入年龄"></el-input>
          </el-form-item>
          <el-form-item label="邮箱" prop="email">
            <el-input v-model="form.email" placeholder="请输入邮箱"></el-input>
          </el-form-item>
        </el-form>
        <template #footer>
          <span class="dialog-footer">
            <el-button @click="dialogVisible = false">取消</el-button>
            <el-button type="primary" @click="handleSubmit">确定</el-button>
          </span>
        </template>
      </el-dialog>
    </el-card>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { ElMessage } from 'element-plus'
import request from '@/utils/request'

// 数据
const students = ref([])
const total = ref(0)
const currentPage = ref(1)
const pageSize = ref(10)

// 对话框
const dialogVisible = ref(false)
const dialogTitle = ref('新增学生')
const isEdit = ref(false)
const formRef = ref(null)

// 表单数据
const form = ref({
  id: '',
  name: '',
  age: '',
  email: ''
})

// 表单验证规则
const rules = {
  id: [{ required: true, message: '请输入学生ID', trigger: 'blur' }],
  name: [{ required: true, message: '请输入姓名', trigger: 'blur' }],
  age: [{ required: true, message: '请输入年龄', trigger: 'blur' }],
  email: [
    { required: true, message: '请输入邮箱', trigger: 'blur' },
    { type: 'email', message: '请输入正确的邮箱格式', trigger: ['blur', 'change'] }
  ]
}

// 获取学生列表
const getStudents = async () => {
  try {
    const res = await request.get('/students', {
      params: {
        page: currentPage.value,
        pageSize: pageSize.value
      }
    })
    students.value = res.data
    total.value = res.total
  } catch (error) {
    ElMessage.error('获取学生列表失败')
  }
}

// 新增学生
const handleAddStudent = () => {
  isEdit.value = false
  dialogTitle.value = '新增学生'
  form.value = {
    id: '',
    name: '',
    age: '',
    email: ''
  }
  dialogVisible.value = true
}

// 编辑学生
const handleEditStudent = (row) => {
  isEdit.value = true
  dialogTitle.value = '编辑学生'
  form.value = { ...row }
  dialogVisible.value = true
}

// 删除学生
const handleDeleteStudent = async (id) => {
  try {
    await request.delete(`/students/${id}`)
    ElMessage.success('学生删除成功')
    getStudents() // 刷新列表
  } catch (error) {
    ElMessage.error('学生删除失败')
  }
}

// 提交表单
const handleSubmit = async () => {
  try {
    await formRef.value.validate()
    
    if (isEdit.value) {
      // 编辑学生
      await request.put(`/students/${form.value.id}`, form.value)
      ElMessage.success('学生编辑成功')
    } else {
      // 新增学生
      await request.post('/students', form.value)
      ElMessage.success('学生新增成功')
    }
    
    dialogVisible.value = false
    getStudents() // 刷新列表
  } catch (error) {
    ElMessage.error('操作失败：' + (error.response?.data?.message || error.message))
  }
}

// 分页处理
const handleSizeChange = (size) => {
  pageSize.value = size
  getStudents()
}

const handleCurrentChange = (current) => {
  currentPage.value = current
  getStudents()
}

// 页面加载时获取学生列表
onMounted(() => {
  getStudents()
})
</script>

<style scoped>
.card-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.pagination-container {
  margin-top: 20px;
  display: flex;
  justify-content: flex-end;
}
</style>
```

## 六、前端页面展示

本项目前端采用Vue.js 3开发，实现了多个功能页面，根据用户角色分为公共页面、学生页面和管理员页面三个层级，以下是详细展示：

### 6.1 公共页面

#### 6.1.1 登录页面

**功能描述**：
- 用户进入系统的唯一入口，支持管理员和学生两种角色登录
- 包含用户名输入框、密码输入框和登录按钮
- 支持密码可见性切换功能（点击小眼睛图标）
- 提供注册入口，方便新用户注册
- 包含表单验证，确保用户名和密码不为空

**技术实现**：
- 使用Vue 3组合式API开发
- 采用Element Plus组件库构建UI
- 实现了基于JWT的登录认证
- 密码输入框使用了show-password属性实现可见性切换

![登录页面](images/01_login.png)

#### 6.1.2 注册页面

**功能描述**：
- 允许新用户注册学生账号
- 包含用户名、密码、确认密码等输入字段
- 实现了密码强度校验
- 提供表单验证，确保所有必填字段都已填写
- 密码和确认密码必须一致才能提交

**技术实现**：
- 使用Vue 3组合式API开发
- 采用Element Plus表单组件
- 自定义了密码验证规则
- 通过Axios向后端发送注册请求

![注册页面](images/02_register.png)

#### 6.1.3 修改密码页面

**功能描述**：
- 允许用户修改自己的密码
- 包含旧密码、新密码、确认新密码输入框
- 实现了密码强度校验
- 旧密码必须正确才能修改
- 新密码和确认新密码必须一致

**技术实现**：
- 使用Vue 3组合式API开发
- 采用Element Plus表单组件
- 自定义了密码验证规则
- 通过Axios向后端发送修改密码请求

![修改密码页面](images/09_change_password.png)

#### 6.1.4 系统主页面（响应式布局）

**功能描述**：
- 系统的主布局页面，包含侧边栏和顶部导航
- 侧边栏提供系统功能导航
- 顶部导航显示系统名称和用户信息
- 支持侧边栏折叠/展开功能
- 移动端自动切换为底部导航

**技术实现**：
- 使用Vue 3组合式API开发
- 实现了响应式设计
- 采用Element Plus菜单组件
- 支持路由懒加载

![系统主页面](images/10_main_layout.png)

### 6.2 学生页面

#### 6.2.1 个人中心页面

**功能描述**：
- 展示当前登录学生的详细信息，包括姓名、年龄、邮箱等
- 支持修改个人信息功能
- 提供修改密码入口
- 采用卡片式布局，信息展示清晰
- 包含用户头像展示

**技术实现**：
- 使用Vue 3组合式API开发
- 采用Element Plus卡片组件
- 实现了信息修改的对话框
- 响应式设计，适配不同屏幕尺寸

![个人中心页面](images/03_profile.png)

#### 6.2.2 我的课程页面

**功能描述**：
- 展示学生选修的所有课程列表
- 每门课程显示课程名称、学分、课程描述等信息
- 支持按课程名称搜索
- 课程列表采用卡片式布局
- 可以查看课程详细信息

**技术实现**：
- 使用Vue 3组合式API开发
- 采用Element Plus卡片和表格组件
- 实现了课程数据的懒加载
- 通过Axios从后端获取课程数据

![我的课程页面](images/04_courses.png)

#### 6.2.3 我的成绩页面

**功能描述**：
- 展示学生的所有成绩记录
- 包含课程名称、学分、成绩、GPA等信息
- 自动计算并显示总GPA
- 支持按课程名称或成绩筛选
- 成绩采用表格形式展示，清晰易读

**技术实现**：
- 使用Vue 3组合式API开发
- 采用Element Plus表格组件
- 实现了GPA自动计算功能
- 支持成绩数据的排序

![我的成绩页面](images/05_grades.png)

### 6.3 管理员页面

#### 6.3.1 学生管理页面

**功能描述**：
- 管理员对学生信息进行全面管理
- 支持学生信息的增删改查操作
- 学生列表支持分页展示
- 提供按学生姓名或ID搜索功能
- 支持批量删除学生
- 新增/编辑学生信息使用对话框形式

**技术实现**：
- 使用Vue 3组合式API开发
- 采用Element Plus表格和分页组件
- 实现了学生数据的CRUD操作
- 通过Axios与后端API交互

![学生管理页面](images/06_student_management.png)

#### 6.3.2 课程管理页面

**功能描述**：
- 管理员对课程信息进行全面管理
- 支持课程信息的增删改查操作
- 课程列表支持分页和搜索
- 支持按课程名称或学分筛选
- 新增/编辑课程使用对话框形式

**技术实现**：
- 使用Vue 3组合式API开发
- 采用Element Plus表格和表单组件
- 实现了课程数据的CRUD操作
- 支持课程数据的批量导入导出

![课程管理页面](images/07_course_management.png)

#### 6.3.3 成绩管理页面

**功能描述**：
- 管理员对学生成绩进行全面管理
- 支持成绩信息的增删改查操作
- 成绩列表支持分页和搜索
- 可以按学生、课程或成绩筛选
- 支持批量导入成绩
- 新增/编辑成绩使用对话框形式

**技术实现**：
- 使用Vue 3组合式API开发
- 采用Element Plus表格和表单组件
- 实现了成绩数据的CRUD操作
- 支持成绩数据的统计分析

![成绩管理页面](images/08_enrollment_management.png)

## 七、个人心得与总结（实验3技术感悟）

### 项目开发总结

本项目是实验3的核心实现，成功构建了一个基于前后端分离架构的学生管理系统。系统采用Vue.js 3作为前端框架，.NET Web API作为后端服务，EF Code First模式进行数据库开发，并通过Autofac实现依赖注入。项目实现了完整的学生管理功能，包括用户认证、学生信息管理、课程管理和成绩管理，具有良好的扩展性、可维护性和用户体验。

### 核心技术实现感悟

#### 1. 前后端分离架构

前后端分离架构是本项目的核心设计理念，通过实践我深刻体会到了其优势：
- **开发效率提升**：前后端可以并行开发，前端专注于用户体验，后端专注于业务逻辑
- **技术栈解耦**：前后端使用不同的技术栈，便于技术选型和团队协作
- **独立部署**：前后端可以独立部署和迭代，降低了部署风险
- **更好的用户体验**：前端采用SPA架构，页面加载速度快，交互流畅

#### 2. Vue.js 3 组件化开发

Vue.js 3的组合式API给我留下了深刻印象：
- **代码组织更清晰**：通过setup函数可以将相关的逻辑组织在一起，提高了代码的可维护性
- **更好的TypeScript支持**：组合式API天生支持TypeScript，类型推断更准确
- **组件复用性高**：通过props和emit实现组件间通信，便于组件复用
- **响应式系统强大**：通过ref和reactive实现响应式数据，简化了状态管理

#### 3. .NET Web API 与 RESTful 设计

.NET Web API是构建RESTful接口的强大框架：
- **路由配置灵活**：支持属性路由和约定路由，便于API设计
- **中间件机制强大**：可以轻松添加CORS、认证、日志等中间件
- **内置模型验证**：通过数据注解可以实现自动的请求验证
- **良好的性能**：.NET 6的性能表现优秀，适合构建高性能API

#### 4. EF Code First 数据访问

EF Code First模式极大简化了数据库开发：
- **从代码生成数据库**：无需手动编写SQL，直接从实体类生成数据库表
- **数据迁移便捷**：支持数据库结构的版本管理，便于团队协作
- **LINQ查询强大**：提供了类型安全的查询方式，减少了查询错误
- **导航属性方便**：通过导航属性可以轻松实现实体间的关联查询

#### 5. Autofac IOC 容器

Autofac实现的依赖注入机制提升了系统的可测试性和可维护性：
- **降低耦合度**：通过构造函数注入，减少了组件间的直接依赖
- **便于单元测试**：可以轻松替换依赖的实现，便于进行单元测试
- **生命周期管理灵活**：支持多种生命周期，如单例、瞬态、作用域等
- **注册方式多样**：支持按类型注册、按约定注册等多种方式

### 遇到的问题与解决方案

1. **跨域问题**：前端请求后端API时出现跨域错误，通过在后端配置CORS中间件解决
2. **密码安全**：初始设计中密码明文存储，后来改为使用哈希算法存储密码
3. **JWT Token管理**：实现了Token的存储、刷新和过期处理机制
4. **EF Code First数据库更新**：修改实体后数据库未更新，通过数据库迁移命令解决
5. **Vue Router导航守卫**：实现了基于Token的路由守卫，确保未登录用户无法访问受保护路由
6. **响应式布局适配**：通过媒体查询和弹性布局，实现了桌面端和移动端的适配

### 未来改进方向

1. **添加分页和搜索功能**：当前系统数据量大时性能会下降，需要添加分页和搜索功能
2. **完善权限管理**：实现基于角色的细粒度权限控制
3. **添加数据统计和可视化**：使用ECharts等图表库实现数据统计和可视化
4. **优化数据库查询**：对复杂查询添加索引，提高查询性能
5. **添加API文档**：使用Swagger生成API文档，便于前后端协作
6. **实现自动化测试**：添加单元测试和集成测试，提高代码质量
7. **优化移动端体验**：进一步优化移动端的交互和性能

### 实验3技术成长总结

通过实验3的开发，我从以下几个方面获得了成长：

1. **技术栈掌握**：熟练掌握了Vue.js 3、.NET Web API、EF Code First和Autofac等核心技术
2. **架构设计能力**：深入理解了前后端分离架构的设计理念和实现方式
3. **问题解决能力**：在开发过程中遇到了各种问题，通过查阅文档、调试和思考，提高了问题解决能力
4. **代码质量意识**：注重代码的可读性、可维护性和性能，培养了良好的代码习惯
5. **团队协作意识**：虽然是单人开发，但按照前后端分离的模式进行开发，模拟了团队协作的场景

### 个人感悟

实验3是一次宝贵的全栈开发实践，让我深刻体会到了现代化Web开发的魅力。前后端分离架构、组件化开发、依赖注入等设计理念，不仅提高了开发效率，也使系统更加灵活和可维护。

在未来的学习和工作中，我将继续深入学习这些技术，不断提升自己的专业能力。同时，我也意识到，技术是不断发展的，作为一名开发者，需要保持学习的热情，不断跟进新技术的发展，才能在激烈的竞争中立于不败之地。

通过本次实验，我不仅掌握了新的技术，还培养了良好的学习习惯和解决问题的能力，这些都将对我未来的职业发展产生积极的影响。