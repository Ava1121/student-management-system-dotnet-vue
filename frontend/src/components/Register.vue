<template>
  <div class="register-container">
    <div class="register-card">
      <div class="logo-section">
        <div class="logo-icon">
          <span class="logo-text">🏫</span>
        </div>
      </div>
      
      <div class="title-section">
        <h1 class="main-title">学生管理系统</h1>
        <h2 class="sub-title">学生注册</h2>
      </div>
      
      <form @submit.prevent="handleRegister" class="register-form">
        <div class="form-group">
          <div class="input-wrapper">
            <span class="input-icon">👤</span>
            <input 
              type="text" 
              id="student-id" 
              v-model="form.studentId" 
              required
              placeholder="请输入学生ID"
              class="input-field"
            >
          </div>
        </div>
        
        <div class="form-group">
          <div class="input-wrapper">
            <span class="input-icon">📝</span>
            <input 
              type="text" 
              id="student-name" 
              v-model="form.name" 
              required
              placeholder="请输入姓名"
              class="input-field"
            >
          </div>
        </div>
        
        <div class="form-group">
          <div class="input-wrapper">
            <span class="input-icon">📧</span>
            <input 
              type="email" 
              id="student-email" 
              v-model="form.email" 
              required
              placeholder="请输入邮箱"
              class="input-field"
            >
          </div>
        </div>
        
        <div class="form-group">
          <div class="input-wrapper">
            <span class="input-icon">🔢</span>
            <input 
              type="number" 
              id="student-age" 
              v-model="form.age" 
              required
              placeholder="请输入年龄"
              class="input-field"
              min="1"
              max="100"
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
              placeholder="请设置密码"
              class="input-field"
              minlength="6"
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
        
        <div class="form-actions">
          <button type="submit" class="btn btn-primary" :disabled="isLoading">
            <span v-if="isLoading" class="loading-spinner"></span>
            <span class="btn-text">{{ isLoading ? '注册中...' : '注册' }}</span>
          </button>
          <button type="button" class="btn btn-secondary" @click="handleLoginRedirect">
            <span class="btn-text">已有账号？登录</span>
          </button>
        </div>
        
        <div v-if="error" class="error-message">
          <span class="error-icon">⚠️</span>
          {{ error }}
        </div>
        
        <div v-if="success" class="success-message">
          <span class="success-icon">✅</span>
          {{ success }}
        </div>
        
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
  name: 'Register',
  data() {
    return {
      showPassword: false,
      form: {
        studentId: '',
        name: '',
        email: '',
        age: '',
        password: ''
      },
      isLoading: false,
      error: '',
      success: ''
    };
  },
  methods: {
    togglePasswordVisibility() {
      this.showPassword = !this.showPassword;
    },
    async handleRegister() {
      try {
        this.isLoading = true;
        this.error = '';
        this.success = '';
        
        // 表单验证
        if (this.form.password.length < 6) {
          this.error = '密码长度不能少于6位';
          this.isLoading = false;
          return;
        }
        
        // 调用后端API注册
        const response = await axios.post('http://localhost:5005/api/Auth/register', {
          studentId: this.form.studentId,
          name: this.form.name,
          email: this.form.email,
          age: parseInt(this.form.age),
          password: this.form.password
        });
        
        this.success = '注册成功！3秒后自动跳转到登录页';
        
        // 3秒后跳转到登录页
        setTimeout(() => {
          this.$router.push('/login');
        }, 3000);
        
      } catch (err) {
        this.error = err.response?.data || '注册失败，请检查输入信息';
        console.error('注册失败:', err);
      } finally {
        this.isLoading = false;
      }
    },
    handleLoginRedirect() {
      this.$router.push('/login');
    }
  }
};
</script>

<style scoped>
.register-container {
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

.register-card {
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

.register-card::before {
  content: '';
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  height: 6px;
  background: linear-gradient(90deg, #dc2626 0%, #ef4444 100%);
}

.register-card:hover {
  transform: translateY(-3px);
  box-shadow: 0 20px 55px rgba(0, 0, 0, 0.25);
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

/* 表单样式 */
.register-form {
  width: 100%;
}

.form-group {
  margin-bottom: 20px;
  width: 100%;
}

.input-wrapper {
  position: relative;
  display: flex;
  align-items: center;
  width: 100%;
}

/* 密码输入框容器 */
.password-wrapper {
  position: relative;
}

.input-icon {
  position: absolute;
  left: 16px;
  font-size: 18px;
  color: #9ca3af;
  transition: all 0.3s ease;
}

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

/* 为带密码切换按钮的输入框添加右侧内边距 */
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

/* 表单操作区 */
.form-actions {
  margin-top: 30px;
  margin-bottom: 20px;
  width: 100%;
  display: flex;
  flex-direction: column;
  gap: 15px;
}

/* 按钮样式 */
.btn {
  width: 100%;
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
}

.btn-primary {
  background: linear-gradient(135deg, #dc2626 0%, #ef4444 100%);
  color: white;
  box-shadow: 0 4px 15px rgba(220, 38, 38, 0.4);
}

.btn-primary:hover:not(:disabled) {
  transform: translateY(-2px);
  box-shadow: 0 6px 20px rgba(220, 38, 38, 0.5);
}

.btn-primary:disabled {
  background: linear-gradient(135deg, #fecaca 0%, #fee2e2 100%);
  cursor: not-allowed;
  box-shadow: none;
  transform: none;
  opacity: 0.8;
}

.btn-secondary {
  background: #f3f4f6;
  color: #374151;
  border: 2px solid #e5e7eb;
}

.btn-secondary:hover {
  background: #e5e7eb;
  transform: translateY(-2px);
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
}

.btn-secondary:focus {
  outline: none;
  box-shadow: 0 0 0 3px rgba(239, 68, 68, 0.1);
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

/* 成功信息 */
.success-message {
  margin-bottom: 20px;
  color: #059669;
  text-align: center;
  font-weight: 600;
  font-size: 13px;
  background-color: #f0fdf4;
  padding: 12px 16px;
  border-radius: 10px;
  border: 1px solid #bbf7d0;
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 8px;
}

.success-icon {
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
  .register-card {
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
