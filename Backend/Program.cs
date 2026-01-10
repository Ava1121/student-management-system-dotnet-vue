using Backend.Data;
using Backend.Models;
using Backend.Models.Auth;
using Backend.Services;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

// 配置Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File("logs/app-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

// 使用Serilog作为日志提供程序
builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddDbContext<SchoolContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 配置Autofac作为服务提供程序
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    // 注册自定义服务
    containerBuilder.RegisterType<JwtService>().AsSelf().InstancePerLifetimeScope();
    
    // 注册JWT设置
    var jwtSettings = new JwtSettings();
    containerBuilder.RegisterInstance(jwtSettings).AsSelf().SingleInstance();
    
    // 数据库上下文由EF Core自动注册，这里不需要重复注册
});

// Add JWT Authentication
// 重新创建JWT设置用于配置认证
var jwtSettingsForAuth = new JwtSettings();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettingsForAuth.Issuer,
        ValidAudience = jwtSettingsForAuth.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettingsForAuth.SecretKey)),
        // Map JWT claims to User.Identity
        NameClaimType = System.Security.Claims.ClaimTypes.Name, // 使用sub作为用户名
        RoleClaimType = System.Security.Claims.ClaimTypes.Role
    };
});

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers().AddJsonOptions(options =>
{
    // 配置JSON序列化器使用驼峰命名法，与前端保持一致
    options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
});

var app = builder.Build();

// Apply database migrations using EF Core
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<SchoolContext>();
    
    // 只创建数据库（如果不存在），不删除现有数据库
    // 这样可以保留之前注册的学生数据
    dbContext.Database.EnsureCreated();
    
    // 初始化大三课程
    if (dbContext.Courses.Count() <= 4) // 只在课程数量较少时执行初始化，避免重复添加
    {
        // 创建8个大三课程
        var juniorCourses = new List<Course>
        {
            new Course { Title = "高级操作系统", Description = "操作系统高级原理与设计，包括进程管理、内存管理、文件系统、分布式系统等", Credits = 4, Code = "CS402", Instructor = "吴教授" },
            new Course { Title = "分布式系统", Description = "分布式系统原理与实践，包括分布式一致性、分布式存储、分布式计算等", Credits = 4, Code = "CS403", Instructor = "郑教授" },
            new Course { Title = "大数据处理", Description = "大数据处理技术与应用，包括Hadoop、Spark、MapReduce等", Credits = 3, Code = "CS404", Instructor = "王教授" },
            new Course { Title = "机器学习进阶", Description = "机器学习高级算法与应用，包括深度学习、强化学习、自然语言处理等", Credits = 4, Code = "CS405", Instructor = "李教授" },
            new Course { Title = "网络安全", Description = "网络安全原理与实践，包括密码学、入侵检测、防火墙、安全协议等", Credits = 3, Code = "CS406", Instructor = "赵教授" },
            new Course { Title = "云计算", Description = "云计算原理与应用，包括IaaS、PaaS、SaaS、容器技术等", Credits = 3, Code = "CS407", Instructor = "周教授" },
            new Course { Title = "计算机图形学", Description = "计算机图形学原理与应用，包括3D建模、渲染、动画等", Credits = 4, Code = "CS408", Instructor = "孙教授" },
            new Course { Title = "软件工程实践", Description = "软件工程实践项目，包括需求分析、设计、编码、测试、部署等", Credits = 3, Code = "CS409", Instructor = "钱教授" }
        };
        
        foreach (var course in juniorCourses)
        {
            if (!dbContext.Courses.Any(c => c.Code == course.Code))
            {
                dbContext.Courses.Add(course);
            }
        }
        
        await dbContext.SaveChangesAsync();
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// 先应用CORS策略，再进行HTTPS重定向
app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

// 确保在应用程序关闭时关闭日志
Log.CloseAndFlush();
