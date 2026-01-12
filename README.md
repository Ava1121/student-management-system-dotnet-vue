# 教育云平台 - 全栈Web应用

基于现代化前后端分离架构开发的企业级教育云平台，面向高校教务管理场景，提供学生信息管理、课程管理、成绩管理、用户认证等核心功能。

## 🚀 技术栈

### 后端
- **框架**：.NET Web API 9.0
- **ORM**：Entity Framework Core (Code First)
- **IOC容器**：Autofac
- **认证授权**：JWT (JSON Web Token)
- **日志**：Serilog
- **数据库**：SQL Server

### 前端
- **框架**：Vue.js 3
- **构建工具**：Vite
- **路由**：Vue Router
- **HTTP客户端**：Axios

## ✨ 核心功能

### 学生管理
- 学生信息CRUD操作
- 学生档案管理
- 学生成绩查询

### 课程管理
- 课程信息管理
- 课程列表展示
- 课程详情查看

### 成绩管理
- 成绩录入与修改
- 成绩查询与统计
- 成绩报表生成

### 用户认证
- 注册与登录
- 密码修改
- 基于角色的权限控制

### 系统管理
- 操作日志查看
- 系统状态监控

## 🏗️ 系统架构

### 架构设计
```
┌─────────────────────────────────────────────────────────────────┐
│                        前端应用 (Vue.js)                         │
├─────────────────────────────────────────────────────────────────┤
│  组件层  │  路由层  │  API服务层  │  状态管理  │  工具函数        │
└─────────────────────────────────────────────────────────────────┘
                                │
                                ▼
┌─────────────────────────────────────────────────────────────────┐
│                     API网关/反向代理                            │
└─────────────────────────────────────────────────────────────────┘
                                │
                                ▼
┌─────────────────────────────────────────────────────────────────┐
│                        后端应用 (.NET Web API)                   │
├─────────────────────────────────────────────────────────────────┤
│  Controllers │  Services  │  Repositories  │  Models  │  Utils    │
└─────────────────────────────────────────────────────────────────┘
                                │
                                ▼
┌─────────────────────────────────────────────────────────────────┐
│                          数据库 (SQL Server)                     │
└─────────────────────────────────────────────────────────────────┘
```

### 分层设计
- **表示层**：负责处理HTTP请求和响应，实现API接口
- **业务逻辑层**：实现核心业务逻辑，处理业务规则和流程
- **数据访问层**：负责与数据库交互，实现数据持久化
- **模型层**：定义数据结构和实体关系
- **工具层**：提供通用工具和辅助功能

## 📦 项目结构

```
├── Backend/                    # 后端应用
│   ├── Controllers/            # API控制器
│   ├── Data/                   # 数据库上下文和配置
│   ├── Models/                 # 数据模型
│   ├── Services/               # 业务逻辑服务
│   ├── Properties/             # 项目属性
│   ├── appsettings.json        # 配置文件
│   └── Program.cs              # 应用入口
├── frontend/                   # 前端应用
│   ├── src/                    # 源代码
│   │   ├── components/         # Vue组件
│   │   ├── router/             # 路由配置
│   │   ├── services/           # API服务
│   │   ├── App.vue             # 根组件
│   │   └── main.js             # 应用入口
│   ├── index.html              # HTML模板
│   ├── package.json            # 依赖配置
│   └── vite.config.js          # Vite配置
├── README.md                   # 项目说明文档
└── SYSTEM_DESIGN.md            # 系统设计文档
```

## 🛠️ 快速开始

### 环境要求
- **.NET SDK**：9.0或更高版本
- **Node.js**：16.x或更高版本
- **SQL Server**：2019或更高版本

### 后端部署
1. **配置数据库连接**
   ```json
   // Backend/appsettings.json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost;Database=SchoolDB;Trusted_Connection=True;MultipleActiveResultSets=true"
     }
   }
   ```

2. **执行数据库迁移**
   ```bash
   cd Backend
   dotnet ef database update
   ```

3. **启动后端服务**
   ```bash
   dotnet run
   ```
   后端服务将在 `https://localhost:5001` 启动

### 前端部署
1. **安装依赖**
   ```bash
   cd frontend
   npm install
   ```

2. **启动开发服务器**
   ```bash
   npm run dev
   ```
   前端应用将在 `http://localhost:5173` 启动

### 生产部署
1. **构建前端**
   ```bash
   cd frontend
   npm run build
   ```

2. **构建后端**
   ```bash
   cd Backend
   dotnet publish -c Release -o ./publish
   ```

## 🎯 核心特性

### 1. 现代化架构设计
- 前后端分离，松耦合设计
- 基于RESTful API的通信协议
- 支持跨域资源共享 (CORS)

### 2. 安全性
- JWT认证机制，无状态会话管理
- 基于角色的权限控制 (RBAC)
- 密码哈希存储
- 请求参数验证

### 3. 可扩展性
- 模块化设计，便于功能扩展
- 依赖注入，降低组件耦合度
- 支持多种数据库（SQL Server、MySQL等）

### 4. 高性能
- 异步编程模型，提升并发处理能力
- 数据库索引优化
- 缓存机制支持

### 5. 可维护性
- 清晰的代码结构和命名规范
- 完整的日志记录
- 统一的错误处理

## 📋 开发规范

### 后端开发规范
- 遵循RESTful API设计原则
- 使用异步方法处理IO操作
- 实现统一的API响应格式
- 编写单元测试和集成测试

### 前端开发规范
- 组件化开发，复用性优先
- 遵循Vue 3 Composition API最佳实践
- 实现响应式设计，适配多种设备
- 使用TypeScript增强类型安全性（可选）

## 🤝 贡献指南

1. Fork 项目
2. 创建功能分支 (`git checkout -b feature/AmazingFeature`)
3. 提交更改 (`git commit -m 'Add some AmazingFeature'`)
4. 推送到分支 (`git push origin feature/AmazingFeature`)
5. 打开 Pull Request

## 📄 许可证

本项目采用 MIT 许可证 - 查看 [LICENSE](LICENSE) 文件了解详情

## 📞 联系方式

如有问题或建议，欢迎通过以下方式联系：
- GitHub Issues：[提交Issue](https://github.com/Ava1121/student-management-system-dotnet-vue/issues)
- 邮箱：your-email@example.com

---

**项目地址**：[https://github.com/Ava1121/student-management-system-dotnet-vue](https://github.com/Ava1121/student-management-system-dotnet-vue)

**更新日期**：2026-01-12