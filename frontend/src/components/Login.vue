<template>
  <div class="login-container">
    <div class="login-form">
      <!-- Logo 区域 -->
      <div class="logo-section">
        <div class="logo-icon">
          <span class="logo-text">🏫</span>
        </div>
      </div>
      
      <!-- 标题区域 -->
      <div class="title-section">
        <h1 class="main-title">学生管理系统</h1>
        <h2 class="sub-title">{{ loginType === 'admin' ? '管理员登录' : '学生登录' }}</h2>
      </div>
      
      <!-- 登录类型切换 -->
      <div class="login-type-switch">
        <button 
          class="type-btn" 
          :class="{ active: loginType === 'admin' }" 
          @click="loginType = 'admin'"
        >
          <span class="btn-icon">⚙️</span>
          管理员
        </button>
        <button 
          class="type-btn" 
          :class="{ active: loginType === 'student' }" 
          @click="loginType = 'student'"
        >
          <span class="btn-icon">👨‍🎓</span>
          学生
        </button>
      </div>
      
      <!-- 登录表单 -->
      <form @submit.prevent="handleLogin" class="login-form-inner">
        <div class="form-group">
          <div class="input-wrapper">
            <span class="input-icon">{{ loginType === 'admin' ? '👨‍💼' : '🎓' }}</span>
            <input 
              type="text" 
              :id="loginType === 'admin' ? 'admin-username' : 'student-id'"
              v-model="form.username" 
              required
              :placeholder="`请输入${loginType === 'admin' ? '管理员用户名' : '学生ID'}`"
              class="input-field"
            >
          </div>
        </div>
        
        <div class="form-group">
          <div class="input-wrapper password-wrapper">
            <span class="input-icon">🔐</span>
            <input 
              :type="showPassword ? 'text' : 'password'" 
              id="password" 
              v-model="form.password" 
              required
              placeholder="请输入密码"
              class="input-field"
            >
            <button 
              type="button" 
              class="password-toggle" 
              @click="togglePasswordVisibility"
              title="{{ showPassword ? '隐藏密码' : '显示密码' }}"
            >
              <span class="toggle-icon">{{ showPassword ? '👁️' : '👁️‍🗨️' }}</span>
            </button>
          </div>
        </div>
        
        <!-- 验证码 -->
        <div class="form-group">
          <div class="captcha-container">
            <div class="input-wrapper captcha-input">
              <span class="input-icon">✅</span>
              <input 
                type="text" 
                id="captcha" 
                v-model="userInputCaptcha" 
                required
                placeholder="请输入验证码"
                class="input-field"
                maxlength="4"
              >
            </div>
            <div 
              class="captcha-image" 
              @click="generateCaptcha"
              title="点击刷新验证码"
            >
              {{ captchaText }}
            </div>
          </div>
        </div>
        
        <!-- 登录和注册按钮 -->
        <div class="form-actions">
          <button type="button" class="btn btn-secondary" @click="$router.push('/register')">
            <span class="btn-text">注册</span>
          </button>
          <button type="submit" class="btn btn-primary" :disabled="isLoading">
            <span v-if="isLoading" class="loading-spinner"></span>
            <span class="btn-text">{{ isLoading ? '登录中...' : '登录' }}</span>
          </button>
        </div>
        
        <!-- 错误信息 -->
        <div v-if="error" class="error-message">
          <span class="error-icon">⚠️</span>
          {{ error }}
        </div>
        
        <!-- 页脚 -->
        <div class="form-footer">
          <p class="footer-text">© 2025 学生管理系统 | 开发者：张艳艳</p>
        </div>
      </form>
    </div>
  </div>
</template>

<script>
import axios from 'axios';

export default {
  name: 'Login',
  data() {
    return {
      loginType: 'admin', // 'admin' 或 'student'
      showPassword: false, // 密码是否可见
      form: {
        username: '',
        password: ''
      },
      captchaText: '', // 生成的验证码文本
      userInputCaptcha: '', // 用户输入的验证码
      isLoading: false,
      error: ''
    };
  },
  mounted() {
    // 页面加载时生成验证码
    this.generateCaptcha();
  },
  
  methods: {
    // 生成验证码
    generateCaptcha() {
      const chars = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789';
      let result = '';
      for (let i = 0; i < 4; i++) {
        result += chars.charAt(Math.floor(Math.random() * chars.length));
      }
      this.captchaText = result;
      this.userInputCaptcha = '';
    },
    
    // 切换密码可见性
    togglePasswordVisibility() {
      this.showPassword = !this.showPassword;
    },
    
    // 检查验证码
    checkCaptcha() {
      return this.userInputCaptcha.toLowerCase() === this.captchaText.toLowerCase();
    },
    
    async handleLogin() {
      try {
        this.isLoading = true;
        this.error = '';
        
        // 验证验证码
        if (!this.checkCaptcha()) {
          this.error = '验证码错误';
          this.generateCaptcha(); // 重新生成验证码
          this.isLoading = false;
          return;
        }
        
        let response;
        if (this.loginType === 'admin') {
          // 管理员登录
          response = await axios.post('http://localhost:5005/api/Auth/login', this.form);
        } else {
          // 学生登录
          response = await axios.post('http://localhost:5005/api/Auth/student-login', this.form);
        }
        
        // 清除之前的localStorage数据，防止角色混淆
        localStorage.clear();
        
        // 保存认证信息到localStorage
        localStorage.setItem('token', response.data.token);
        localStorage.setItem('username', response.data.username);
        localStorage.setItem('role', response.data.role);
        localStorage.setItem('studentId', response.data.studentId || '');
        
        // 根据角色跳转到不同页面
        if (response.data.role === 'Admin') {
          this.$router.push('/'); // 管理员跳转到学生管理
        } else {
          this.$router.push('/profile'); // 学生跳转到个人中心
        }
      } catch (error) {
        this.error = error.response?.data || '登录失败，请检查用户名和密码';
      } finally {
        this.isLoading = false;
      }
    }
  }
};
</script>

<style scoped>
/* 全局样式 */
* {
  box-sizing: border-box;
  margin: 0;
  padding: 0;
}

/* 确保html和body元素充满整个页面 */
html, body {
  width: 100%;
  height: 100%;
  margin: 0;
  padding: 0;
  overflow: hidden;
}

.login-container {
  display: flex;
  justify-content: center;
  align-items: center;
  min-height: 100vh;
  background: #dc2626;
  padding: 20px;
  margin: 0;
  font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
  width: 100%;
  height: 100vh;
  position: relative;
  overflow: hidden;
}

/* 登录表单容器 */
.login-form {
  background-color: white;
  padding: 35px 30px;
  border-radius: 16px;
  box-shadow: 0 15px 45px rgba(0, 0, 0, 0.2);
  width: 100%;
  max-width: 420px;
  position: relative;
  overflow: hidden;
  transition: transform 0.3s ease, box-shadow 0.3s ease;
}

.login-form:hover {
  transform: translateY(-3px);
  box-shadow: 0 20px 55px rgba(0, 0, 0, 0.25);
}

/* 表单顶部装饰条 */
.login-form::before {
  content: '';
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  height: 6px;
  background: linear-gradient(90deg, #dc2626 0%, #ef4444 100%);
}

/* Logo 区域 */
.logo-section {
  display: flex;
  justify-content: center;
  align-items: center;
  margin-bottom: 25px;
}

.logo-icon {
  width: 70px;
  height: 70px;
  background: linear-gradient(135deg, #dc2626 0%, #ef4444 100%);
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  box-shadow: 0 4px 15px rgba(220, 38, 38, 0.4);
  animation: pulse 2s ease-in-out infinite;
}

@keyframes pulse {
  0%, 100% { transform: scale(1); }
  50% { transform: scale(1.05); }
}

.logo-text {
  font-size: 36px;
}

/* 标题区域 */
.title-section {
  text-align: center;
  margin-bottom: 25px;
}

.main-title {
  font-size: 24px;
  font-weight: 700;
  color: #1f2937;
  margin-bottom: 6px;
}

.sub-title {
  font-size: 18px;
  font-weight: 600;
  color: #dc2626;
}

/* 登录类型切换 */
.login-type-switch {
  display: flex;
  margin-bottom: 25px;
  border: 2px solid white;
  border-radius: 12px;
  overflow: hidden;
  background-color: rgba(255, 255, 255, 0.15);
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
}

.type-btn {
  flex: 1;
  padding: 14px 20px;
  border: none;
  background-color: rgba(255, 255, 255, 0.1);
  cursor: pointer;
  transition: all 0.3s ease;
  font-size: 16px;
  font-weight: 700;
  color: white;
  position: relative;
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 10px;
  outline: none;
  text-shadow: 0 1px 2px rgba(0, 0, 0, 0.2);
}

.btn-icon {
  font-size: 18px;
}

.type-btn.active {
  background: white;
  color: #dc2626;
  box-shadow: 0 4px 15px rgba(0, 0, 0, 0.25);
  text-shadow: none;
  font-weight: 800;
}

.type-btn:hover:not(.active) {
  background-color: rgba(255, 255, 255, 0.3);
  color: white;
  font-weight: 700;
}

/* 登录表单内部 */
.login-form-inner {
  width: 100%;
}

/* 表单组 */
.form-group {
  margin-bottom: 20px;
  width: 100%;
}

/* 输入框容器 */
.input-wrapper {
  position: relative;
  display: flex;
  align-items: center;
  width: 100%;
}

/* 输入框图标 */
.input-icon {
  position: absolute;
  left: 16px;
  font-size: 18px;
  color: #9ca3af;
  transition: all 0.3s ease;
}

/* 密码输入框容器 */
.password-wrapper {
  position: relative;
}

/* 输入框 */
.input-field {
  width: 100%;
  padding: 15px 18px 15px 48px;
  border: 1px solid #e5e7eb;
  border-radius: 10px;
  font-size: 15px;
  font-weight: 500;
  color: #1f2937;
  background-color: white;
  transition: all 0.2s ease;
  outline: none;
}

/* 密码输入框额外内边距，为切换按钮留出空间 */
.password-wrapper .input-field {
  padding-right: 56px;
}

.input-field::placeholder {
  color: #9ca3af;
  font-weight: 400;
}

.input-field:focus {
  border-color: #ef4444;
  box-shadow: 0 0 0 3px rgba(239, 68, 68, 0.1);
}

.input-field:focus + .input-icon {
  color: #ef4444;
}

/* 密码切换按钮 */
.password-toggle {
  position: absolute;
  right: 12px;
  top: 50%;
  transform: translateY(-50%);
  background: transparent;
  border: none;
  cursor: pointer;
  padding: 8px;
  border-radius: 6px;
  transition: all 0.2s ease;
  display: flex;
  align-items: center;
  justify-content: center;
  color: #9ca3af;
  font-size: 18px;
}

.password-toggle:hover {
  background-color: rgba(0, 0, 0, 0.05);
  color: #ef4444;
}

.password-toggle:focus {
  outline: none;
  box-shadow: 0 0 0 3px rgba(239, 68, 68, 0.1);
}

.toggle-icon {
  font-size: 18px;
  line-height: 1;
}

/* 验证码容器 */
.captcha-container {
  display: flex;
  gap: 15px;
  width: 100%;
  align-items: center;
}

/* 验证码输入框 */
.captcha-input {
  flex: 1;
}

/* 验证码图片 */
.captcha-image {
  padding: 15px 20px;
  background: linear-gradient(135deg, #f3f4f6 0%, #e5e7eb 100%);
  border-radius: 10px;
  font-size: 20px;
  font-weight: 800;
  color: #dc2626;
  letter-spacing: 8px;
  text-align: center;
  cursor: pointer;
  transition: all 0.3s ease;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
  border: 2px solid #e5e7eb;
  user-select: none;
  min-width: 120px;
  height: 100%;
  display: flex;
  align-items: center;
  justify-content: center;
}

.captcha-image:hover {
  background: linear-gradient(135deg, #e5e7eb 0%, #d1d5db 100%);
  transform: translateY(-1px);
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
  border-color: #dc2626;
}

.captcha-image:active {
  transform: translateY(0);
}

/* 表单操作区 */
.form-actions {
  margin-top: 30px;
  margin-bottom: 20px;
  width: 100%;
  display: flex;
  gap: 12px;
}

/* 按钮样式 */
.btn {
  flex: 1;
  padding: 15px;
  border: none;
  border-radius: 10px;
  font-size: 16px;
  font-weight: 700;
  cursor: pointer;
  transition: all 0.3s ease;
  text-transform: uppercase;
  letter-spacing: 0.8px;
  position: relative;
  overflow: hidden;
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 8px;
  outline: none;
}

.btn-primary {
  background: linear-gradient(135deg, #dc2626 0%, #ef4444 100%);
  color: white;
  box-shadow: 0 4px 15px rgba(220, 38, 38, 0.4);
}

.btn-primary:hover {
  transform: translateY(-2px);
  box-shadow: 0 6px 20px rgba(220, 38, 38, 0.5);
}

.btn-primary:active {
  transform: translateY(0);
  box-shadow: 0 3px 12px rgba(220, 38, 38, 0.4);
}

.btn-primary:disabled {
  background: linear-gradient(135deg, #fecaca 0%, #fee2e2 100%);
  cursor: not-allowed;
  box-shadow: none;
  transform: none;
  opacity: 0.8;
}

.btn-secondary {
  background: linear-gradient(135deg, #fee2e2 0%, #fecaca 100%);
  color: #dc2626;
  box-shadow: 0 4px 15px rgba(220, 38, 38, 0.2);
  border: 1px solid #fecaca;
}

.btn-secondary:hover {
  transform: translateY(-2px);
  box-shadow: 0 6px 20px rgba(220, 38, 38, 0.3);
  background: linear-gradient(135deg, #fecaca 0%, #fca5a5 100%);
  color: white;
}

.btn-secondary:active {
  transform: translateY(0);
  box-shadow: 0 3px 12px rgba(220, 38, 38, 0.2);
}

/* 加载中动画 */
.loading-spinner {
  width: 20px;
  height: 20px;
  border: 2px solid rgba(255, 255, 255, 0.3);
  border-top-color: white;
  border-radius: 50%;
  animation: spin 1s linear infinite;
}

@keyframes spin {
  to { transform: rotate(360deg); }
}

/* 错误信息 */
.error-message {
  margin-bottom: 20px;
  color: #dc2626;
  text-align: center;
  font-weight: 600;
  font-size: 13px;
  background-color: #fff5f5;
  padding: 12px 16px;
  border-radius: 10px;
  border: 1px solid #fecaca;
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 8px;
  animation: shake 0.5s ease-in-out;
}

@keyframes shake {
  0%, 100% { transform: translateX(0); }
  25% { transform: translateX(-6px); }
  75% { transform: translateX(6px); }
}

.error-icon {
  font-size: 17px;
}

/* 表单底部 */
.form-footer {
  text-align: center;
  margin-top: 25px;
  padding-top: 20px;
  border-top: 1px solid #e5e7eb;
  width: 100%;
}

.footer-text {
  color: #6b7280;
  font-size: 13px;
  font-weight: 500;
}

/* 响应式设计 */
@media (max-width: 480px) {
  .login-form {
    padding: 30px 25px;
    margin: 0 15px;
    max-width: calc(100% - 30px);
  }
  
  .main-title {
    font-size: 22px;
  }
  
  .sub-title {
    font-size: 17px;
  }
  
  .type-btn {
    padding: 12px 16px;
    font-size: 14px;
  }
  
  .input-field {
    padding: 14px 16px 14px 45px;
    font-size: 14px;
  }
  
  .btn {
    padding: 14px;
    font-size: 15px;
  }
}
</style>