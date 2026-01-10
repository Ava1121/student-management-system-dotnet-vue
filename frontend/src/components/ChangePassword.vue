<template>
  <div class="change-password-container">
    <div class="change-password-card">
      <h2 class="card-title">修改密码</h2>
      <form @submit.prevent="handleChangePassword" class="change-password-form">
        <div class="form-group">
          <label for="oldPassword" class="form-label">旧密码</label>
          <div class="password-input-wrapper">
            <input
              :type="showOldPassword ? 'text' : 'password'"
              id="oldPassword"
              v-model="formData.oldPassword"
              placeholder="请输入旧密码"
              class="form-input"
              required
            />
            <button 
              type="button" 
              class="password-toggle" 
              @click="toggleOldPassword"
              title="{{ showOldPassword ? '隐藏密码' : '显示密码' }}"
            >
              <span class="toggle-icon">{{ showOldPassword ? '👁️' : '👁️‍🗨️' }}</span>
            </button>
          </div>
        </div>
        <div class="form-group">
          <label for="newPassword" class="form-label">新密码</label>
          <div class="password-input-wrapper">
            <input
              :type="showNewPassword ? 'text' : 'password'"
              id="newPassword"
              v-model="formData.newPassword"
              placeholder="请输入新密码"
              class="form-input"
              required
              minlength="6"
            />
            <button 
              type="button" 
              class="password-toggle" 
              @click="toggleNewPassword"
              title="{{ showNewPassword ? '隐藏密码' : '显示密码' }}"
            >
              <span class="toggle-icon">{{ showNewPassword ? '👁️' : '👁️‍🗨️' }}</span>
            </button>
          </div>
        </div>
        <div class="form-group">
          <label for="confirmPassword" class="form-label">确认新密码</label>
          <div class="password-input-wrapper">
            <input
              :type="showConfirmPassword ? 'text' : 'password'"
              id="confirmPassword"
              v-model="formData.confirmPassword"
              placeholder="请再次输入新密码"
              class="form-input"
              required
              minlength="6"
            />
            <button 
              type="button" 
              class="password-toggle" 
              @click="toggleConfirmPassword"
              title="{{ showConfirmPassword ? '隐藏密码' : '显示密码' }}"
            >
              <span class="toggle-icon">{{ showConfirmPassword ? '👁️' : '👁️‍🗨️' }}</span>
            </button>
          </div>
        </div>
        <div v-if="error" class="error-message">
          {{ error }}
        </div>
        <div v-if="success" class="success-message">
          {{ success }}
        </div>
        <div class="form-actions">
          <button type="submit" class="submit-btn" :disabled="isSubmitting">
            {{ isSubmitting ? '提交中...' : '确认修改' }}
          </button>
          <button type="button" class="cancel-btn" @click="handleCancel">
            取消
          </button>
        </div>
      </form>
    </div>
  </div>
</template>

<script>
import axios from 'axios';

export default {
  name: 'ChangePassword',
  data() {
    return {
      formData: {
        oldPassword: '',
        newPassword: '',
        confirmPassword: ''
      },
      // 密码可见性状态
      showOldPassword: false,
      showNewPassword: false,
      showConfirmPassword: false,
      error: '',
      success: '',
      isSubmitting: false
    };
  },
  methods: {
    // 切换旧密码可见性
    toggleOldPassword() {
      this.showOldPassword = !this.showOldPassword;
    },
    
    // 切换新密码可见性
    toggleNewPassword() {
      this.showNewPassword = !this.showNewPassword;
    },
    
    // 切换确认密码可见性
    toggleConfirmPassword() {
      this.showConfirmPassword = !this.showConfirmPassword;
    },
    
    async handleChangePassword() {
      // 表单验证
      if (this.formData.newPassword !== this.formData.confirmPassword) {
        this.error = '两次输入的新密码不一致';
        this.success = '';
        return;
      }
      
      if (this.formData.newPassword.length < 6) {
        this.error = '新密码长度不能少于6位';
        this.success = '';
        return;
      }
      
      this.isSubmitting = true;
      this.error = '';
      this.success = '';
      
      try {
        // 获取当前用户信息
        const username = localStorage.getItem('role') === 'Admin' ? 'admin' : localStorage.getItem('studentId') || '';
        
        // 调用后端API修改密码
        const response = await axios.post('http://localhost:5005/api/Auth/change-password', {
          username: username,
          oldPassword: this.formData.oldPassword,
          newPassword: this.formData.newPassword
        });
        
        // 处理成功响应
        this.success = response.data.message || '密码修改成功！';
        this.formData = {
          oldPassword: '',
          newPassword: '',
          confirmPassword: ''
        };
        
        // 3秒后自动跳转
        setTimeout(() => {
          this.$router.go(-1);
        }, 3000);
      } catch (err) {
        this.error = err.response?.data || '密码修改失败，请检查旧密码是否正确';
        console.error('修改密码失败:', err);
      } finally {
        this.isSubmitting = false;
      }
    },
    handleCancel() {
      this.$router.go(-1);
    }
  }
};
</script>

<style scoped>
.change-password-container {
  display: flex;
  justify-content: center;
  align-items: center;
  min-height: 100vh;
  padding: 20px;
  background: transparent;
}

.change-password-card {
  background: rgba(255, 255, 255, 0.95);
  border-radius: 12px;
  box-shadow: 0 8px 32px rgba(0, 0, 0, 0.1);
  padding: 30px;
  width: 100%;
  max-width: 450px;
  color: #333;
}

.card-title {
  font-size: 24px;
  font-weight: 700;
  color: #dc2626;
  margin-bottom: 25px;
  text-align: center;
}

.change-password-form {
  display: flex;
  flex-direction: column;
  gap: 20px;
}

.form-group {
  display: flex;
  flex-direction: column;
  gap: 8px;
}

.form-label {
  font-weight: 600;
  font-size: 14px;
  color: #1f2937;
}

.password-input-wrapper {
  position: relative;
  display: flex;
  align-items: center;
}

.form-input {
  width: 100%;
  padding: 12px 16px 12px 16px;
  border: 2px solid #e5e7eb;
  border-radius: 8px;
  font-size: 16px;
  font-family: inherit;
  transition: all 0.2s ease;
  color: #374151;
  background: #ffffff;
}

/* 为带密码切换按钮的输入框添加右侧内边距 */
.password-input-wrapper .form-input {
  padding-right: 48px;
}

.form-input:focus {
  outline: none;
  border-color: #dc2626;
  box-shadow: 0 0 0 3px rgba(220, 38, 38, 0.1);
}

.form-input::placeholder {
  color: #9ca3af;
}

/* 密码切换按钮 */
.password-toggle {
  position: absolute;
  right: 8px;
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
  color: #dc2626;
}

.password-toggle:focus {
  outline: none;
  box-shadow: 0 0 0 3px rgba(220, 38, 38, 0.1);
}

.toggle-icon {
  font-size: 18px;
  line-height: 1;
}

.error-message {
  background: #fee2e2;
  color: #dc2626;
  padding: 12px 16px;
  border-radius: 8px;
  font-size: 14px;
  font-weight: 500;
  text-align: center;
}

.success-message {
  background: #d1fae5;
  color: #065f46;
  padding: 12px 16px;
  border-radius: 8px;
  font-size: 14px;
  font-weight: 500;
  text-align: center;
}

.form-actions {
  display: flex;
  gap: 12px;
  margin-top: 10px;
}

.submit-btn,
.cancel-btn {
  flex: 1;
  padding: 12px 20px;
  border: none;
  border-radius: 8px;
  font-size: 16px;
  font-weight: 600;
  cursor: pointer;
  transition: all 0.2s ease;
  font-family: inherit;
}

.submit-btn {
  background: #dc2626;
  color: white;
}

.submit-btn:hover:not(:disabled) {
  background: #b91c1c;
  transform: translateY(-1px);
  box-shadow: 0 4px 12px rgba(220, 38, 38, 0.3);
}

.submit-btn:disabled {
  background: #fecaca;
  color: #991b1b;
  cursor: not-allowed;
  transform: none;
}

.cancel-btn {
  background: #f3f4f6;
  color: #374151;
  border: 2px solid #e5e7eb;
}

.cancel-btn:hover {
  background: #e5e7eb;
  transform: translateY(-1px);
}

@media (max-width: 480px) {
  .change-password-card {
    padding: 24px;
  }
  
  .card-title {
    font-size: 20px;
    margin-bottom: 20px;
  }
  
  .form-actions {
    flex-direction: column;
  }
  
  .submit-btn,
  .cancel-btn {
    padding: 10px 16px;
    font-size: 14px;
  }
}
</style>
