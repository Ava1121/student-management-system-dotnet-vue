<template>
  <div class="app">
    <template v-if="isAuthenticated">
      <!-- 主内容区域 -->
      <main class="main-content">
        <!-- 系统标题 -->
        <div class="system-header">
          <h1 class="system-title">学生管理系统</h1>
        </div>
        
        <!-- 页面内容 -->
        <div class="content-wrapper">
          <router-view />
        </div>
      </main>
      
      <!-- 底部导航栏 -->
      <footer class="bottom-nav">
        <!-- 导航菜单 -->
        <nav class="nav-menu">
          <!-- 管理员导航 -->
            <template v-if="userRole === 'Admin'">
              <router-link to="/" class="nav-link">
                <span class="nav-icon">🏠</span>
                <span class="nav-text">首页</span>
              </router-link>
              <router-link to="/students" class="nav-link">
                <span class="nav-icon">👥</span>
                <span class="nav-text">学生管理</span>
              </router-link>
              <router-link to="/courses" class="nav-link">
                <span class="nav-icon">📚</span>
                <span class="nav-text">课程管理</span>
              </router-link>
              <router-link to="/enrollments" class="nav-link">
                <span class="nav-icon">📊</span>
                <span class="nav-text">成绩管理</span>
              </router-link>
              <router-link to="/logs" class="nav-link">
                <span class="nav-icon">📋</span>
                <span class="nav-text">日志管理</span>
              </router-link>
            </template>
          
          <!-- 学生导航 -->
          <template v-else-if="userRole === 'Student'">
            <router-link to="/profile" class="nav-link">
              <span class="nav-icon">👨‍🎓</span>
              <span class="nav-text">个人中心</span>
            </router-link>
            <router-link to="/my-courses" class="nav-link">
              <span class="nav-icon">📚</span>
              <span class="nav-text">我的课程</span>
            </router-link>
            <router-link to="/my-grades" class="nav-link">
              <span class="nav-icon">📊</span>
              <span class="nav-text">我的成绩</span>
            </router-link>
          </template>
        </nav>
        
        <!-- 用户信息和设置 -->
        <div class="user-settings">
          <!-- 用户信息 -->
          <div class="user-info">
            <div class="username">{{ username }}</div>
            <div class="user-role">{{ userRole === 'Admin' ? '管理员' : '学生' }}</div>
          </div>
          
          <!-- 设置选项 -->
          <div class="settings-links">
            <router-link to="/profile" class="setting-link" v-if="userRole === 'Student'">
              <span class="setting-icon">🔍</span>
            </router-link>
            <div class="setting-link" @click="handleSystemInfo" v-else>
              <span class="setting-icon">📋</span>
            </div>
            <div class="setting-link" @click="handleChangePassword">
              <span class="setting-icon">🔐</span>
            </div>
            <button class="setting-link logout-btn" @click="handleLogout">
              <span class="setting-icon">👋</span>
            </button>
          </div>
        </div>
      </footer>
    </template>
    <template v-else>
      <!-- 未登录状态 -->
      <main class="app-main">
        <div class="content-wrapper">
          <router-view />
        </div>
      </main>
    </template>
  </div>
</template>

<script> 
export default { 
  name: 'App', 
  data() {
    return {
      isAuthenticated: false,
      username: '',
      // 添加响应式角色变量
      currentRole: localStorage.getItem('role')
    };
  },
  computed: {
    userRole() {
      const role = this.currentRole;
      console.log('Current user role:', role);
      return role;
    }
  },
  mounted() {
    this.checkAuthStatus();
    // 监听路由变化，更新认证状态
    this.$router.afterEach(() => {
      this.checkAuthStatus();
    });
  },
  methods: {
    checkAuthStatus() {
      this.isAuthenticated = localStorage.getItem('token') !== null;
      this.username = localStorage.getItem('username') || '';
      // 手动更新响应式角色变量，触发重新渲染
      this.currentRole = localStorage.getItem('role');
    },
    handleLogout() {
      if (confirm('确定要退出登录吗？')) {
        // 清除本地存储的认证信息
        localStorage.removeItem('token');
        localStorage.removeItem('username');
        localStorage.removeItem('role');
        // 跳转到登录页
        this.$router.push('/login');
      }
    },
    handleChangePassword() {
      // 跳转到修改密码页面
      this.$router.push('/change-password');
    },
    handleSystemInfo() {
      // 显示系统信息
      alert('学生管理系统\n版本: 1.0.0\n开发者: 张艳艳\n© 2026 学生管理系统');
    }
  }
}
</script>

<style>
* {
  margin: 0;
  padding: 0;
  box-sizing: border-box;
}
/* 全局样式 */
body {
  font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, Oxygen, Ubuntu, Cantarell, 'Open Sans', 'Helvetica Neue', sans-serif;
  background: transparent;
  min-height: 100vh;
  color: #333;
  line-height: 1.6;
  margin: 0;
  padding: 0;
}

.app {
  width: 100%;
  min-height: 100vh;
  display: flex;
  flex-direction: column;
  overflow: hidden;
}

/* 主内容区域 */
.main-content {
  flex: 1;
  min-height: calc(100vh - 80px);
  overflow-y: auto;
  background: transparent;
  padding: 30px;
  margin-bottom: 80px;
}

/* 系统标题 */
.system-header {
  text-align: center;
  margin-bottom: 30px;
}

.system-title {
  font-size: 32px;
  font-weight: 800;
  color: #ffffff;
  margin: 0;
  text-shadow: 2px 2px 4px rgba(0, 0, 0, 0.2);
}

/* 页面内容 */
.content-wrapper {
  padding: 0;
  max-width: 1400px;
  width: 100%;
  margin: 0 auto;
  overflow-y: auto;
  background: transparent;
}

/* 底部导航栏 */
.bottom-nav {
  background: linear-gradient(90deg, #dc2626 0%, #ef4444 100%);
  color: white;
  padding: 15px 30px;
  box-shadow: 0 -2px 10px rgba(0, 0, 0, 0.1);
  z-index: 1000;
  display: flex;
  justify-content: space-between;
  align-items: center;
  gap: 20px;
  position: fixed;
  bottom: 0;
  left: 0;
  right: 0;
  width: 100%;
}

/* 导航菜单 */
.nav-menu {
  display: flex;
  gap: 15px;
  align-items: center;
  flex-wrap: wrap;
}

.nav-link {
  display: flex;
  align-items: center;
  gap: 8px;
  color: #ffffff;
  text-decoration: none;
  padding: 10px 18px;
  border-radius: 8px;
  transition: all 0.3s ease;
  font-weight: 600;
  font-size: 14px;
  background: rgba(255, 255, 255, 0.1);
  border: 2px solid transparent;
}

.nav-link:hover {
  background: rgba(255, 255, 255, 0.2);
  border-color: #ffffff;
  transform: translateY(-2px);
  box-shadow: 0 4px 15px rgba(0, 0, 0, 0.1);
}

.nav-link.router-link-active {
  background: rgba(255, 255, 255, 0.25);
  border-color: #ffffff;
  box-shadow: 0 4px 15px rgba(0, 0, 0, 0.15);
}

.nav-icon {
  font-size: 16px;
  width: 20px;
  text-align: center;
}

.nav-text {
  font-size: 13px;
  text-transform: uppercase;
  letter-spacing: 0.5px;
}

/* 用户信息和设置 */
.user-settings {
  display: flex;
  align-items: center;
  gap: 20px;
}

/* 用户信息 */
.user-info {
  display: flex;
  flex-direction: column;
  align-items: flex-end;
  gap: 2px;
}

.user-info .username {
  font-weight: 600;
  font-size: 14px;
  color: #ffffff;
}

.user-info .user-role {
  font-size: 12px;
  color: rgba(255, 255, 255, 0.9);
  background: rgba(255, 255, 255, 0.15);
  padding: 3px 10px;
  border-radius: 12px;
  text-transform: uppercase;
  letter-spacing: 0.5px;
  font-weight: 500;
}

/* 设置选项 */
.settings-links {
  display: flex;
  gap: 10px;
  align-items: center;
}

.setting-link {
  display: flex;
  align-items: center;
  justify-content: center;
  color: rgba(255, 255, 255, 0.9);
  text-decoration: none;
  padding: 10px;
  border-radius: 8px;
  transition: all 0.3s ease;
  font-size: 14px;
  cursor: pointer;
  background: rgba(255, 255, 255, 0.1);
  border: 2px solid transparent;
  width: 40px;
  height: 40px;
  text-align: center;
}

.setting-link:hover {
  background: rgba(255, 255, 255, 0.2);
  border-color: #ffffff;
  transform: translateY(-2px);
  box-shadow: 0 4px 15px rgba(0, 0, 0, 0.1);
}

.setting-link.logout-btn:hover {
  background: rgba(255, 255, 255, 0.3);
  color: #fef2f2;
}

.setting-icon {
  font-size: 16px;
  width: 16px;
  text-align: center;
}

/* 未登录状态 */
.app-main {
  width: 100%;
  min-height: 100vh;
  display: flex;
  justify-content: center;
  align-items: center;
  padding: 20px;
  background: transparent;
}

/* 滚动条样式 */
.main-content::-webkit-scrollbar {
  width: 8px;
}

.main-content::-webkit-scrollbar-track {
  background: rgba(0, 0, 0, 0.05);
}

.main-content::-webkit-scrollbar-thumb {
  background: rgba(0, 0, 0, 0.2);
  border-radius: 4px;
}

.main-content::-webkit-scrollbar-thumb:hover {
  background: rgba(0, 0, 0, 0.3);
}

/* 响应式设计 */
@media (max-width: 1200px) {
  .main-content {
    padding: 20px;
  }
  
  .bottom-nav {
    padding: 15px 20px;
  }
  
  .nav-menu {
    gap: 10px;
  }
  
  .nav-link {
    padding: 8px 15px;
    font-size: 13px;
  }
  
  .nav-text {
    font-size: 12px;
  }
}

@media (max-width: 768px) {
  .main-content {
    padding: 15px;
  }
  
  .system-title {
    font-size: 24px;
  }
  
  .bottom-nav {
    flex-direction: column;
    gap: 15px;
    padding: 15px;
    align-items: stretch;
  }
  
  .nav-menu {
    justify-content: center;
    gap: 8px;
  }
  
  .nav-link {
    padding: 8px 12px;
    font-size: 13px;
  }
  
  .nav-icon {
    font-size: 14px;
  }
  
  .nav-text {
    font-size: 12px;
  }
  
  .user-settings {
    flex-direction: row;
    justify-content: space-between;
    align-items: center;
  }
  
  .user-info {
    flex-direction: row;
    gap: 10px;
    align-items: center;
  }
  
  .user-info .username {
    font-size: 13px;
  }
  
  .user-info .user-role {
    font-size: 11px;
    padding: 3px 8px;
  }
  
  .settings-links {
    gap: 8px;
  }
  
  .setting-link {
    width: 36px;
    height: 36px;
    padding: 8px;
  }
  
  .setting-icon {
    font-size: 14px;
  }
}

@media (max-width: 480px) {
  .main-content {
    padding: 12px;
  }
  
  .system-title {
    font-size: 20px;
  }
  
  .bottom-nav {
    padding: 12px;
  }
  
  .nav-menu {
    flex-wrap: wrap;
    gap: 6px;
  }
  
  .nav-link {
    padding: 6px 10px;
    font-size: 11px;
    gap: 6px;
  }
  
  .nav-text {
    display: none;
  }
  
  .nav-icon {
    font-size: 16px;
  }
  
  .user-settings {
    gap: 15px;
  }
  
  .user-info {
    gap: 8px;
  }
  
  .user-info .username {
    font-size: 12px;
  }
  
  .user-info .user-role {
    font-size: 10px;
    padding: 2px 6px;
  }
  
  .settings-links {
    gap: 6px;
  }
  
  .setting-link {
    width: 32px;
    height: 32px;
    padding: 6px;
  }
  
  .setting-icon {
    font-size: 13px;
  }
}
</style>