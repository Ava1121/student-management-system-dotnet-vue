<template>
  <div class="student-profile">
    <!-- 顶部渐变背景 -->
    <div class="profile-header-gradient"></div>
    
    <div class="content-wrapper">
      <!-- 基本信息卡片 -->
      <div class="profile-header-card">
        <div class="profile-avatar">
          <div class="avatar">
            <span>{{ student?.name?.charAt(0) || '?' }}</span>
          </div>
        </div>
        <div class="profile-basic-info">
          <h2 class="student-name">{{ student?.name }}</h2>
          <p class="student-id">学生ID: {{ student?.id }}</p>
          <p class="student-email">{{ student?.email }}</p>
        </div>
        <div class="profile-stats">
          <div class="stat-item">
            <div class="stat-icon">📚</div>
            <div class="stat-value">{{ totalCourses }}</div>
            <div class="stat-label">已修课程</div>
          </div>
          <div class="stat-item">
            <div class="stat-icon">📊</div>
            <div class="stat-value">{{ gpa.toFixed(2) }}</div>
            <div class="stat-label">平均GPA</div>
          </div>
          <div class="stat-item">
            <div class="stat-icon">🎓</div>
            <div class="stat-value">{{ totalCredits }}</div>
            <div class="stat-label">总学分</div>
          </div>
        </div>
      </div>
      
      <!-- 详细信息和统计 -->
      <div class="profile-content">
        <!-- 个人详细信息 -->
        <div class="info-section profile-info-section">
          <div class="section-title">
            <div class="title-icon">👤</div>
            <h3>个人信息</h3>
          </div>
          <div class="detail-container">
            <div class="detail-group">
              <div class="detail-field">
                <div class="field-label">
                  <span class="field-icon">📋</span>
                  <span>姓名</span>
                </div>
                <div class="field-value">{{ student?.name }}</div>
              </div>
              <div class="detail-field">
                <div class="field-label">
                  <span class="field-icon">📅</span>
                  <span>年龄</span>
                </div>
                <div class="field-value">{{ student?.age }} 岁</div>
              </div>
              <div class="detail-field">
                <div class="field-label">
                  <span class="field-icon">📧</span>
                  <span>邮箱</span>
                </div>
                <div class="field-value">{{ student?.email }}</div>
              </div>
              <div class="detail-field">
                <div class="field-label">
                  <span class="field-icon">🎓</span>
                  <span>入学年份</span>
                </div>
                <div class="field-value">2023</div>
              </div>
              <div class="detail-field">
                <div class="field-label">
                  <span class="field-icon">📚</span>
                  <span>专业</span>
                </div>
                <div class="field-value">计算机科学与技术</div>
              </div>
              <div class="detail-field">
                <div class="field-label">
                  <span class="field-icon">🏫</span>
                  <span>班级</span>
                </div>
                <div class="field-value">2023级1班</div>
              </div>
            </div>
          </div>
        </div>
        
        <!-- 系统通知 -->
        <div class="info-section notifications-section">
          <div class="section-title">
            <div class="title-icon">🔔</div>
            <h3>系统通知</h3>
          </div>
          <div class="notifications-list">
            <div class="notification-card" @click="handleNotificationClick">
              <div class="notification-icon">📢</div>
              <div class="notification-content">
                <h4 class="notification-title">新学期课程表已发布</h4>
                <p class="notification-time">2026-01-05</p>
              </div>
              <div class="notification-badge new">新</div>
            </div>
            <div class="notification-card">
              <div class="notification-icon">📊</div>
              <div class="notification-content">
                <h4 class="notification-title">期末考试成绩已更新</h4>
                <p class="notification-time">2026-01-04</p>
              </div>
              <div class="notification-badge read">已读</div>
            </div>
          </div>
        </div>
        
        <!-- 课程表模态框 -->
        <div v-if="showCourseSchedule" class="modal-overlay">
          <div class="modal-content">
            <div class="modal-header">
              <h3>2026年春季学期课程表 <span class="new-tag">新学期</span></h3>
              <button class="close-btn" @click="closeCourseSchedule">×</button>
            </div>
            <div class="modal-body">
              <div class="schedule-grid">
                <div class="schedule-item" v-for="course in courses" :key="course.id">
                  <div class="new-course-badge">新课程</div>
                  <h4>{{ course.title }}</h4>
                  <p class="schedule-code">{{ course.code }}</p>
                  <p class="schedule-teacher">{{ course.instructor }}</p>
                  <p class="schedule-credits">{{ course.credits }} 学分</p>
                  <p class="schedule-semester">2026年春季学期</p>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { studentService } from '../services/api';
import { enrollmentService } from '../services/api';
import { courseService } from '../services/api';

export default {
  name: 'StudentProfile',
  data() {
    return {
      student: null,
      grades: [],
      courses: [],
      loading: true,
      error: '',
      gpa: 0,
      totalCourses: 0,
      totalCredits: 0,
      showCourseSchedule: false,
      scheduleLoading: false
    };
  },
  mounted() {
    this.fetchStudentProfile();
  },
  methods: {
    async fetchStudentProfile() {
      try {
        // 获取学生基本信息
        const studentResponse = await studentService.getProfile();
        this.student = studentResponse.data;
        
        // 获取学生成绩信息
        await this.fetchStudentGrades();
        // 获取学生课程信息
        await this.fetchStudentCourses();
      } catch (error) {
        this.error = '获取个人信息失败';
        console.error('获取个人信息失败:', error);
      } finally {
        this.loading = false;
      }
    },
    async fetchStudentGrades() {
      try {
        const gradesResponse = await enrollmentService.getMyGrades();
        this.grades = gradesResponse.data;
        this.calculateGPA();
      } catch (error) {
        console.error('获取成绩信息失败:', error);
        // 成绩获取失败不影响个人信息显示
      }
    },
    async fetchStudentCourses() {
      try {
        const coursesResponse = await courseService.getMyCourses();
        this.courses = coursesResponse.data;
      } catch (error) {
        console.error('获取课程信息失败:', error);
      }
    },
    handleNotificationClick() {
      this.showCourseSchedule = true;
    },
    closeCourseSchedule() {
      this.showCourseSchedule = false;
    },
    calculateGPA() {
      if (this.grades.length === 0) {
        this.gpa = 0;
        this.totalCourses = 0;
        this.totalCredits = 0;
        return;
      }
      
      let totalCredits = 0;
      let totalGPAPoints = 0;
      
      // 定义成绩对应的GPA点数
      const gradeGPAMap = {
        'A': 4.0,
        'B': 3.0,
        'C': 2.0,
        'D': 1.0,
        'F': 0.0,
        null: 0.0
      };
      
      this.grades.forEach(grade => {
        totalCredits += grade.credits;
        
        // 获取成绩对应的GPA点数
        const gpaPoints = gradeGPAMap[grade.grade] || 0.0;
        totalGPAPoints += gpaPoints * grade.credits;
      });
      
      this.gpa = totalGPAPoints / totalCredits;
      this.totalCourses = this.grades.length;
      this.totalCredits = totalCredits;
    }
  }
}
</script>

<style scoped>
/* 页面容器 */
.student-profile {
  width: 100%;
  background: #f8f9fa;
  min-height: 100vh;
  margin: 0;
  padding: 0;
  position: relative;
}

/* 顶部背景 */
.profile-header-gradient {
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  height: 200px;
  background: #ef4444;
  z-index: 1;
}

/* 内容容器 */
.content-wrapper {
  width: 100%;
  max-width: 1200px;
  margin: 0 auto;
  padding: 20px;
  box-sizing: border-box;
  position: relative;
  z-index: 2;
  margin-top: 40px;
}

/* 加载和错误状态 */
.loading, .error {
  text-align: center;
  padding: 60px 20px;
  font-size: 18px;
  font-weight: 500;
  border-radius: 12px;
  max-width: 800px;
  margin: 20px auto;
  box-shadow: 0 4px 15px rgba(0, 0, 0, 0.1);
  background: white;
}

.loading {
  color: #ef4444;
  border: 2px solid rgba(220, 38, 38, 0.3);
  animation: pulse 1.5s ease-in-out infinite;
}

@keyframes pulse {
  0%, 100% { opacity: 1; }
  50% { opacity: 0.7; }
}

.error {
  background: #fef2f2;
  color: #dc2626;
  border: 2px solid rgba(220, 38, 38, 0.3);
}

/* 基本信息卡片 */
.profile-header-card {
  display: flex;
  flex-direction: row;
  align-items: center;
  gap: 30px;
  background: white;
  padding: 30px;
  border-radius: 16px;
  box-shadow: 0 8px 32px rgba(0, 0, 0, 0.1);
  margin-bottom: 30px;
  border: 1px solid rgba(255, 255, 255, 0.2);
  backdrop-filter: blur(10px);
  transition: all 0.3s ease;
}

.profile-header-card:hover {
  transform: translateY(-2px);
  box-shadow: 0 12px 40px rgba(0, 0, 0, 0.15);
}

/* 头像 */
.profile-avatar {
  display: flex;
  align-items: center;
  justify-content: center;
}

.avatar {
  width: 100px;
  height: 100px;
  border-radius: 50%;
  background: #ef4444;
  display: flex;
  align-items: center;
  justify-content: center;
  color: white;
  font-size: 48px;
  font-weight: 700;
  box-shadow: 0 8px 24px rgba(220, 38, 38, 0.3);
  transition: all 0.3s ease;
  border: 3px solid white;
}

.avatar:hover {
  transform: scale(1.05);
  box-shadow: 0 12px 32px rgba(220, 38, 38, 0.4);
}

/* 基本信息 */
.profile-basic-info {
  flex: 1;
}

.student-name {
  margin: 0 0 12px 0;
  color: #1f2937;
  font-size: 32px;
  font-weight: 800;
  line-height: 1.2;
}

.student-id {
  margin: 0 0 8px 0;
  color: #6b7280;
  font-size: 16px;
  font-weight: 600;
}

.student-email {
  margin: 0;
  color: #ef4444;
  font-size: 16px;
  font-weight: 500;
}

/* 统计信息 */
.profile-stats {
  display: flex;
  gap: 24px;
}

.stat-item {
  text-align: center;
  padding: 20px;
  background: #fee2e2;
  border-radius: 12px;
  min-width: 100px;
  border: 1px solid rgba(220, 38, 38, 0.1);
  transition: all 0.3s ease;
}

.stat-item:hover {
  transform: translateY(-2px);
  box-shadow: 0 6px 20px rgba(220, 38, 38, 0.2);
}

.stat-icon {
  font-size: 24px;
  margin-bottom: 8px;
  display: block;
  color: #dc2626;
}

.stat-value {
  font-size: 28px;
  font-weight: 800;
  color: #dc2626;
  margin-bottom: 4px;
  display: block;
}

.stat-label {
  font-size: 12px;
  color: #6b7280;
  font-weight: 600;
  text-transform: uppercase;
  letter-spacing: 0.5px;
  display: block;
}

/* 详细内容区域 */
.profile-content {
  display: grid;
  grid-template-columns: 1fr;
  gap: 24px;
}

/* 信息区块 */
.info-section {
  background: white;
  padding: 28px;
  border-radius: 16px;
  box-shadow: 0 4px 20px rgba(0, 0, 0, 0.08);
  border: 1px solid rgba(220, 38, 38, 0.1);
  transition: all 0.3s ease;
}

.info-section:hover {
  transform: translateY(-2px);
  box-shadow: 0 8px 32px rgba(0, 0, 0, 0.12);
}

/* 个人信息区块 */
.profile-info-section {
  background: #fef2f2;
}

/* 标题样式 */
.section-title {
  display: flex;
  align-items: center;
  gap: 12px;
  margin-bottom: 24px;
  padding-bottom: 12px;
  border-bottom: 2px solid rgba(220, 38, 38, 0.1);
}

.title-icon {
  font-size: 24px;
  color: #dc2626;
  display: flex;
  align-items: center;
  justify-content: center;
}

.section-title h3 {
  color: #1f2937;
  font-size: 22px;
  font-weight: 700;
  margin: 0;
  display: block;
}

/* 详情容器 */
.detail-container {
  width: 100%;
}

.detail-group {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(300px, 1fr));
  gap: 20px;
}

.detail-field {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 16px 20px;
  background: rgba(220, 38, 38, 0.05);
  border-radius: 12px;
  border: 1px solid rgba(220, 38, 38, 0.1);
  transition: all 0.3s ease;
}

.detail-field:hover {
  background: rgba(220, 38, 38, 0.1);
  transform: translateX(4px);
}

.field-label {
  display: flex;
  align-items: center;
  gap: 10px;
  font-weight: 600;
  color: #ef4444;
  font-size: 14px;
}

.field-icon {
  font-size: 18px;
  color: #dc2626;
}

.field-value {
  font-weight: 700;
  color: #374151;
  font-size: 16px;
}

/* 通知列表 */
.notifications-section {
  background: #fef2f2;
}

.notifications-list {
  display: flex;
  flex-direction: column;
  gap: 16px;
}

.notification-card {
  display: flex;
  align-items: center;
  gap: 20px;
  padding: 20px;
  background: white;
  border-radius: 12px;
  box-shadow: 0 2px 12px rgba(0, 0, 0, 0.06);
  border: 1px solid rgba(220, 38, 38, 0.1);
  transition: all 0.3s ease;
  cursor: pointer;
  position: relative;
  overflow: hidden;
}

.notification-card::before {
  content: '';
  position: absolute;
  left: 0;
  top: 0;
  bottom: 0;
  width: 4px;
  background: #ef4444;
}

.notification-card:hover {
  transform: translateY(-2px);
  box-shadow: 0 6px 24px rgba(0, 0, 0, 0.12);
}

.notification-icon {
  font-size: 28px;
  color: #dc2626;
  margin-top: 0;
  flex-shrink: 0;
}

.notification-content {
  flex: 1;
}

.notification-title {
  margin: 0 0 8px 0;
  color: #1f2937;
  font-weight: 700;
  font-size: 18px;
  line-height: 1.3;
}

.notification-time {
  margin: 0;
  color: #9ca3af;
  font-size: 14px;
  font-weight: 500;
}

.notification-badge {
  padding: 6px 16px;
  border-radius: 20px;
  font-size: 12px;
  font-weight: 700;
  text-transform: uppercase;
  letter-spacing: 0.5px;
  flex-shrink: 0;
}

.notification-badge.new {
  background: #ef4444;
  color: white;
  box-shadow: 0 4px 12px rgba(220, 38, 38, 0.3);
}

.notification-badge.read {
  background: #f3f4f6;
  color: #6b7280;
}

/* 模态框样式 */
.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: rgba(0, 0, 0, 0.6);
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 2000;
  padding: 20px;
  backdrop-filter: blur(4px);
  animation: fadeIn 0.3s ease;
}

@keyframes fadeIn {
  from { opacity: 0; }
  to { opacity: 1; }
}

.modal-content {
  background: white;
  border-radius: 16px;
  box-shadow: 0 20px 60px rgba(0, 0, 0, 0.3);
  width: 100%;
  max-width: 800px;
  max-height: 85vh;
  overflow: hidden;
  display: flex;
  flex-direction: column;
  animation: slideUp 0.3s ease;
}

@keyframes slideUp {
  from { transform: translateY(30px); opacity: 0; }
  to { transform: translateY(0); opacity: 1; }
}

.modal-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 24px 32px;
  background: #ef4444;
  color: white;
}

.modal-header h3 {
  margin: 0;
  font-size: 24px;
  font-weight: 700;
  line-height: 1.3;
}

.new-tag {
  background: #ffffff;
  color: #dc2626;
  font-size: 12px;
  font-weight: 600;
  padding: 6px 16px;
  border-radius: 20px;
  margin-left: 12px;
  text-transform: uppercase;
  letter-spacing: 0.5px;
}

.close-btn {
  background: transparent;
  border: none;
  color: white;
  font-size: 32px;
  cursor: pointer;
  padding: 0;
  width: 40px;
  height: 40px;
  display: flex;
  align-items: center;
  justify-content: center;
  border-radius: 50%;
  transition: all 0.3s ease;
  flex-shrink: 0;
}

.close-btn:hover {
  background: rgba(255, 255, 255, 0.2);
  transform: rotate(90deg);
}

.modal-body {
  padding: 32px;
  overflow-y: auto;
  flex: 1;
}

/* 课程表网格 */
.schedule-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(280px, 1fr));
  gap: 24px;
}

.schedule-item {
  background: white;
  padding: 24px;
  border-radius: 12px;
  box-shadow: 0 4px 16px rgba(0, 0, 0, 0.08);
  border: 1px solid rgba(220, 38, 38, 0.1);
  transition: all 0.3s ease;
  position: relative;
  overflow: hidden;
}

.schedule-item::before {
  content: '';
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  height: 4px;
  background: #ef4444;
}

.schedule-item:hover {
  transform: translateY(-4px);
  box-shadow: 0 8px 24px rgba(220, 38, 38, 0.2);
}

.new-course-badge {
  position: absolute;
  top: 12px;
  right: 12px;
  background: #dc2626;
  color: white;
  font-size: 10px;
  font-weight: 700;
  padding: 4px 12px;
  border-radius: 16px;
  text-transform: uppercase;
  letter-spacing: 0.5px;
  box-shadow: 0 4px 12px rgba(220, 38, 38, 0.3);
  z-index: 10;
}

.schedule-item h4 {
  color: #1f2937;
  margin: 0 0 12px 0;
  font-size: 20px;
  font-weight: 700;
  line-height: 1.3;
  z-index: 5;
  position: relative;
}

.schedule-code {
  color: #ef4444;
  font-size: 14px;
  margin: 0 0 10px 0;
  font-weight: 600;
  text-transform: uppercase;
  letter-spacing: 1px;
  background: rgba(220, 38, 38, 0.1);
  display: inline-block;
  padding: 6px 12px;
  border-radius: 16px;
  z-index: 5;
  position: relative;
}

.schedule-teacher {
  color: #374151;
  font-size: 15px;
  margin: 0 0 8px 0;
  font-weight: 600;
  z-index: 5;
  position: relative;
}

.schedule-credits {
  color: #6b7280;
  font-size: 14px;
  margin: 0 0 12px 0;
  font-weight: 600;
  z-index: 5;
  position: relative;
}

.schedule-semester {
  color: #ef4444;
  font-size: 12px;
  margin: 12px 0 0 0;
  font-weight: 600;
  text-transform: uppercase;
  letter-spacing: 0.5px;
  background: rgba(220, 38, 38, 0.1);
  display: inline-block;
  padding: 4px 12px;
  border-radius: 12px;
  z-index: 5;
  position: relative;
}

/* 响应式设计 */
@media (max-width: 768px) {
  .content-wrapper {
    margin-top: 20px;
    padding: 16px;
  }
  
  .profile-header-card {
    flex-direction: column;
    text-align: center;
    padding: 24px;
  }
  
  .student-name {
    font-size: 28px;
  }
  
  .profile-stats {
    width: 100%;
    justify-content: space-around;
    gap: 16px;
  }
  
  .stat-item {
    min-width: 80px;
    padding: 16px;
  }
  
  .stat-value {
    font-size: 24px;
  }
  
  .detail-group {
    grid-template-columns: 1fr;
  }
  
  .detail-field {
    flex-direction: column;
    align-items: flex-start;
    gap: 8px;
    text-align: left;
  }
  
  .notification-card {
    flex-direction: column;
    align-items: flex-start;
    gap: 16px;
  }
  
  .notification-content {
    width: 100%;
  }
  
  .schedule-grid {
    grid-template-columns: 1fr;
  }
  
  .modal-content {
    margin: 16px;
    max-height: 90vh;
  }
  
  .modal-header,
  .modal-body {
    padding: 20px;
  }
  
  .modal-header h3 {
    font-size: 20px;
  }
  
  .new-tag {
    font-size: 10px;
    padding: 4px 10px;
  }
}

@media (max-width: 480px) {
  .profile-header-gradient {
    height: 150px;
  }
  
  .content-wrapper {
    margin-top: 16px;
    padding: 12px;
  }
  
  .profile-header-card {
    padding: 20px;
  }
  
  .student-name {
    font-size: 24px;
  }
  
  .info-section {
    padding: 20px;
  }
  
  .section-title h3 {
    font-size: 18px;
  }
  
  .notification-card {
    padding: 16px;
  }
  
  .notification-title {
    font-size: 16px;
  }
  
  .schedule-item {
    padding: 20px;
  }
  
  .schedule-item h4 {
    font-size: 18px;
  }
}
</style>