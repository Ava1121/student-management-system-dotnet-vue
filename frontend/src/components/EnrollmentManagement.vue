<template>
  <div class="enrollment-management">
    <div class="content-grid">
      <!-- 成绩列表 -->
      <div class="list-card">
        <div class="card-header">
          <h2>成绩列表</h2>
          <div class="card-actions">
            <button class="btn btn-primary" @click="showAddModal = true">
              <span class="stat-icon">+</span> 添加成绩
            </button>
          </div>
          <div class="card-stats">
            <span class="stat-item">
              <span class="stat-icon">📊</span>
              <span class="stat-value">{{ enrollments.length }}</span>
              <span class="stat-label">成绩记录</span>
            </span>
          </div>
        </div>
        <div class="card-body">
          <div v-if="enrollments.length === 0" class="empty-state">
            <div class="empty-icon"></div>
            <div class="empty-text">暂无成绩数据</div>
            <div class="empty-hint">点击添加成绩按钮开始管理</div>
          </div>
          <div v-else class="student-list">
            <div class="table-wrapper">
              <table class="student-table">
                <thead>
                  <tr>
                    <th class="table-th">ID</th>
                    <th class="table-th">学生姓名</th>
                    <th class="table-th">课程名称</th>
                    <th class="table-th">成绩</th>
                    <th class="table-th table-th-actions">操作</th>
                  </tr>
                </thead>
                <tbody>
                  <tr v-for="enrollment in enrollments" :key="enrollment.id" class="table-tr">
                    <td class="table-td">{{ enrollment.id }}</td>
                    <td class="table-td">{{ enrollment.student?.name || '未知学生' }}</td>
                    <td class="table-td">{{ enrollment.course?.title || '未知课程' }}</td>
                    <td class="table-td">{{ getGradeText(enrollment.grade) }}</td>
                    <td class="table-td table-td-actions">
                      <div class="action-buttons">
                        <button 
                          class="btn btn-edit" 
                          @click="editEnrollment(enrollment)"
                          title="编辑"
                        >
                          编辑
                        </button>
                        <button 
                          class="btn btn-delete" 
                          @click="deleteEnrollment(enrollment.id)"
                          title="删除"
                        >
                          删除
                        </button>
                      </div>
                    </td>
                  </tr>
                </tbody>
              </table>
            </div>
          </div>
        </div>
      </div>
    </div>
    
    <!-- 编辑成绩模态框 -->
    <div v-if="showEditModal" class="modal-overlay" @click="showEditModal = false">
      <div class="modal-content" @click.stop>
        <div class="modal-header">
          <h2>编辑成绩</h2>
          <button class="modal-close" @click="showEditModal = false">×</button>
        </div>
        <div class="modal-body">
          <form @submit.prevent="handleEditSubmit" class="student-form">
            <div class="form-row">
              <div class="form-group">
                <label for="edit-id">ID</label>
                <div class="input-wrapper">
                  <input 
                    type="number" 
                    id="edit-id" 
                    v-model="editForm.id" 
                    required
                    placeholder="请输入成绩ID"
                    disabled
                  >
                </div>
              </div>
            </div>
            <div class="form-group">
              <label for="edit-studentId">学生</label>
              <div class="input-wrapper">
                <select 
                  id="edit-studentId" 
                  v-model="editForm.studentId" 
                  required
                >
                  <option value="">选择学生</option>
                  <option v-for="student in students" :key="student.id" :value="student.id">
                    {{ student.name }}
                  </option>
                </select>
              </div>
            </div>
            <div class="form-group">
              <label for="edit-courseId">课程</label>
              <div class="input-wrapper">
                <select 
                  id="edit-courseId" 
                  v-model="editForm.courseId" 
                  required
                >
                  <option value="">选择课程</option>
                  <option v-for="course in courses" :key="course.id" :value="course.id">
                    {{ course.title }}
                  </option>
                </select>
              </div>
            </div>
            <div class="form-group">
              <label for="edit-grade">成绩</label>
              <div class="input-wrapper">
                <select 
                  id="edit-grade" 
                  v-model="editForm.grade"
                >
                  <option value="">未评分</option>
                  <option value="0">A</option>
                  <option value="1">B</option>
                  <option value="2">C</option>
                  <option value="3">D</option>
                  <option value="4">F</option>
                </select>
              </div>
            </div>
            <div class="form-actions" style="margin-top: 25px;">
              <button type="submit" class="btn btn-primary">更新</button>
              <button 
                type="button" 
                class="btn btn-secondary" 
                @click="showEditModal = false"
              >
                取消
              </button>
            </div>
          </form>
        </div>
      </div>
    </div>
    
    <!-- 添加成绩模态框 -->
    <div v-if="showAddModal" class="modal-overlay" @click="showAddModal = false">
      <div class="modal-content" @click.stop>
        <div class="modal-header">
          <h2>添加成绩</h2>
          <button class="modal-close" @click="showAddModal = false">×</button>
        </div>
        <div class="modal-body">
          <form @submit.prevent="handleAddSubmit" class="student-form">
            <div class="form-group">
              <label for="add-studentId">学生</label>
              <div class="input-wrapper">
                <select 
                  id="add-studentId" 
                  v-model="form.studentId" 
                  required
                >
                  <option value="">选择学生</option>
                  <option v-for="student in students" :key="student.id" :value="student.id">
                    {{ student.name }}
                  </option>
                </select>
              </div>
            </div>
            <div class="form-group">
              <label for="add-courseId">课程</label>
              <div class="input-wrapper">
                <select 
                  id="add-courseId" 
                  v-model="form.courseId" 
                  required
                >
                  <option value="">选择课程</option>
                  <option v-for="course in courses" :key="course.id" :value="course.id">
                    {{ course.title }}
                  </option>
                </select>
              </div>
            </div>
            <div class="form-group">
              <label for="add-grade">成绩</label>
              <div class="input-wrapper">
                <select 
                  id="add-grade" 
                  v-model="form.grade"
                >
                  <option value="">未评分</option>
                  <option value="0">A</option>
                  <option value="1">B</option>
                  <option value="2">C</option>
                  <option value="3">D</option>
                  <option value="4">F</option>
                </select>
              </div>
            </div>
            <div class="form-actions" style="margin-top: 25px;">
              <button type="submit" class="btn btn-primary">添加</button>
              <button 
                type="button" 
                class="btn btn-secondary" 
                @click="closeAddModal"
              >
                取消
              </button>
            </div>
          </form>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { enrollmentService, studentService, courseService } from '../services/api';

export default {
  name: 'EnrollmentManagement',
  data() {
    return {
      enrollments: [],
      students: [],
      courses: [],
      form: {
        studentId: '',
        courseId: '',
        grade: ''
      },
      // 添加成绩相关
      showAddModal: false,
      // 编辑成绩相关
      showEditModal: false,
      editForm: {
        id: null,
        studentId: '',
        courseId: '',
        grade: ''
      }
    };
  },
  mounted() {
    this.fetchData();
  },
  methods: {
    async fetchData() {
      try {
        console.log('开始获取成绩管理数据...');
        // 单独获取学生列表，以便更好地调试
        try {
          const studentsRes = await studentService.getAll();
          console.log('获取到的学生数据:', studentsRes.data);
          console.log('学生列表长度:', studentsRes.data.length);
          this.students = studentsRes.data;
          console.log('学生列表赋值后长度:', this.students.length);
        } catch (studentError) {
          console.error('获取学生列表失败:', studentError);
          console.error('学生列表错误详情:', studentError.response?.data || studentError.message);
          console.error('学生列表错误状态:', studentError.response?.status);
        }
        
        // 单独获取课程列表
        try {
          const coursesRes = await courseService.getAll();
          console.log('获取到的课程数据:', coursesRes.data);
          console.log('课程列表长度:', coursesRes.data.length);
          this.courses = coursesRes.data;
        } catch (courseError) {
          console.error('获取课程列表失败:', courseError);
        }
        
        // 单独获取成绩列表
        try {
          const enrollmentsRes = await enrollmentService.getAll();
          console.log('获取到的成绩数据:', enrollmentsRes.data);
          this.enrollments = enrollmentsRes.data;
        } catch (enrollmentError) {
          console.error('获取成绩列表失败:', enrollmentError);
        }
      } catch (error) {
        console.error('获取数据失败:', error);
        alert('获取数据失败');
      }
    },
    async handleAddSubmit() {
      try {
        // 处理成绩字段：如果为空字符串，转换为null；否则转换为整数
        const formData = {
          ...this.form,
          grade: this.form.grade === '' ? null : parseInt(this.form.grade)
        };
        
        console.log('提交的成绩数据:', formData);
        
        await enrollmentService.create(formData);
        alert('成绩添加成功');
        this.fetchData();
        this.closeAddModal();
      } catch (error) {
        console.error('操作失败:', error);
        console.error('错误详情:', error.response?.data || error.message);
        console.error('错误状态:', error.response?.status);
        alert('操作失败');
      }
    },
    closeAddModal() {
      this.showAddModal = false;
      this.resetForm();
    },
    editEnrollment(enrollment) {
      console.log('开始编辑成绩:', enrollment);
      this.showEditModal = true;
      // 处理成绩字段：如果是null，转换为空字符串；否则保持原值
      this.editForm = {
        id: enrollment.id,
        studentId: enrollment.studentId,
        courseId: enrollment.courseId,
        grade: enrollment.grade === null ? '' : enrollment.grade
      };
    },
    async handleEditSubmit() {
      try {
        // 处理成绩字段：如果为空字符串，转换为null；否则转换为整数
        const formData = {
          ...this.editForm,
          grade: this.editForm.grade === '' ? null : parseInt(this.editForm.grade)
        };
        
        console.log('提交的成绩更新数据:', formData);
        
        await enrollmentService.update(this.editForm.id, formData);
        alert('成绩更新成功');
        this.fetchData();
        this.showEditModal = false;
      } catch (error) {
        console.error('操作失败:', error);
        console.error('错误详情:', error.response?.data || error.message);
        console.error('错误状态:', error.response?.status);
        alert('操作失败');
      }
    },
    async deleteEnrollment(id) {
      if (confirm('确定要删除这个成绩吗?')) {
        try {
          await enrollmentService.delete(id);
          alert('成绩删除成功');
          this.fetchData();
        } catch (error) {
          console.error('删除失败:', error);
          alert('删除失败');
        }
      }
    },
    resetForm() {
      this.form = {
        studentId: '',
        courseId: '',
        grade: ''
      };
    },
    getGradeText(grade) {
      const gradeMap = {
        0: 'A',
        1: 'B',
        2: 'C',
        3: 'D',
        4: 'F',
        null: '未评分'
      };
      return gradeMap[grade] || '未评分';
    }
  }
};
</script>

<style scoped>
/* 页面容器 */
.enrollment-management {
  width: 100%;
  padding: 20px;
  background: #f8f9fa;
}

/* 内容网格 */
.content-grid {
  display: grid;
  grid-template-columns: 1fr;
  gap: 20px;
  margin-bottom: 20px;
  width: 100%;
  padding: 0 20px;
}

/* 卡片样式 */
.form-card, .list-card {
  background: #ffffff;
  border-radius: 12px;
  box-shadow: 0 4px 15px rgba(0, 0, 0, 0.1);
  overflow: hidden;
  transition: all 0.3s ease;
  border: 2px solid #e0e0e0;
}

.form-card:hover, .list-card:hover {
  box-shadow: 0 8px 25px rgba(0, 0, 0, 0.15);
  transform: translateY(-3px);
}

/* 卡片头部 */
.card-header {
  background: linear-gradient(135deg, #dc2626 0%, #ef4444 100%);
  color: #ffffff;
  padding: 25px 30px;
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.card-header h2 {
  margin: 0;
  font-size: 22px;
  font-weight: 700;
  color: #ffffff;
  text-transform: uppercase;
  letter-spacing: 0.5px;
}

/* 卡片操作区域 */
.card-actions {
  margin-right: auto;
  margin-left: 20px;
}

/* 卡片统计 */
.card-stats {
  display: flex;
  gap: 15px;
}

.stat-item {
  display: flex;
  align-items: center;
  gap: 8px;
  background: rgba(255, 255, 255, 0.2);
  padding: 10px 20px;
  border-radius: 25px;
  backdrop-filter: blur(10px);
}

.stat-icon {
  font-size: 20px;
}

.stat-value {
  font-weight: 800;
  font-size: 20px;
}

.stat-label {
  font-size: 14px;
  opacity: 0.9;
}

/* 卡片内容 */
.card-body {
  padding: 30px;
  background: #ffffff;
}

/* 表单样式 */
.student-form {
  width: 100%;
}

.form-row {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 25px;
  margin-bottom: 25px;
}

.form-group {
  margin-bottom: 25px;
}

.form-group label {
  display: block;
  margin-bottom: 12px;
  font-weight: 700;
  color: #333;
  font-size: 14px;
  text-transform: uppercase;
  letter-spacing: 0.5px;
}

.input-wrapper {
  position: relative;
}

.input-wrapper input,
.input-wrapper select,
.input-wrapper textarea {
  width: 100%;
  padding: 14px 16px;
  border: 2px solid #e0e0e0;
  border-radius: 8px;
  font-size: 16px;
  transition: all 0.3s ease;
  background-color: #fafafa;
  color: #333;
  resize: vertical;
  min-height: 40px;
  appearance: none;
  cursor: pointer;
}

.input-wrapper input:focus,
.input-wrapper select:focus,
.input-wrapper textarea:focus {
  outline: none;
  border-color: #ef4444;
  background-color: white;
  box-shadow: 0 0 0 3px rgba(220, 38, 38, 0.1);
}

/* 按钮样式 */
.form-actions {
  display: flex;
  gap: 15px;
  margin-top: 35px;
}

.btn {
  padding: 14px 30px;
  border: none;
  border-radius: 8px;
  cursor: pointer;
  transition: all 0.3s ease;
  font-weight: 700;
  font-size: 15px;
  display: inline-flex;
  align-items: center;
  gap: 10px;
  text-transform: uppercase;
  letter-spacing: 0.5px;
  position: relative;
  overflow: hidden;
}

.btn-primary {
  background: linear-gradient(135deg, #dc2626 0%, #ef4444 100%);
  color: white;
  box-shadow: 0 4px 12px rgba(220, 38, 38, 0.3);
}

.btn-primary:hover {
  background: linear-gradient(135deg, #b91c1c 0%, #dc2626 100%);
  transform: translateY(-2px);
  box-shadow: 0 6px 18px rgba(220, 38, 38, 0.5);
}

.btn-secondary {
  background: #f0f0f0;
  color: #666;
}

.btn-secondary:hover {
  background: #e0e0e0;
  color: #333;
  transform: translateY(-2px);
}

/* 表格容器 */
.table-wrapper {
  overflow-x: auto;
  width: 100%;
  border-radius: 8px;
  box-shadow: 0 4px 15px rgba(0, 0, 0, 0.1);
}

/* 表格样式 */
.student-table {
  width: 100%;
  border-collapse: collapse;
  background: #ffffff;
  border-radius: 8px;
  overflow: hidden;
  border: 2px solid #e0e0e0;
}

.table-th {
  background: linear-gradient(135deg, #dc2626 0%, #ef4444 100%);
  color: #ffffff;
  padding: 20px 30px;
  text-align: left;
  font-weight: 700;
  font-size: 14px;
  text-transform: uppercase;
  letter-spacing: 0.5px;
  border-right: 1px solid rgba(255, 255, 255, 0.2);
}

.table-th:last-child {
  border-right: none;
}

.table-td {
  padding: 20px 30px;
  border-bottom: 1px solid #e0e0e0;
  font-size: 16px;
  color: #333;
  border-right: 1px solid #e0e0e0;
  background: #ffffff;
  transition: all 0.3s ease;
}

.table-td:last-child {
  border-right: none;
}

/* 操作列样式 */
.table-th-actions {
  min-width: 250px;
  text-align: center;
}

.table-td-actions {
  min-width: 250px;
  text-align: center;
}

.table-tr {
  transition: all 0.3s ease;
}

.table-tr:hover {
  background: rgba(220, 38, 38, 0.05);
  transform: translateY(-2px);
}

.table-tr:last-child .table-td {
  border-bottom: none;
}

/* 操作按钮组 */
.action-buttons {
  display: flex;
  gap: 12px;
  justify-content: center;
}

.action-buttons .btn {
  padding: 10px 20px;
  font-size: 14px;
  border-radius: 6px;
  min-width: 80px;
  justify-content: center;
}

.btn-edit {
  background: #f0fdf4;
  color: #16a34a;
  border: 2px solid #bbf7d0;
}

.btn-edit:hover {
  background: #dcfce7;
  color: #15803d;
  transform: translateY(-2px);
}

.btn-delete {
  background: #fef2f2;
  color: #dc2626;
  border: 2px solid #fee2e2;
}

.btn-delete:hover {
  background: #fee2e2;
  color: #b91c1c;
  transform: translateY(-2px);
}

/* 空状态 */
.empty-state {
  text-align: center;
  padding: 80px 20px;
  color: #666;
  background: #fafafa;
  border-radius: 8px;
  margin: 20px;
}

.empty-icon {
  font-size: 72px;
  margin-bottom: 20px;
  opacity: 0.4;
  color: #dc2626;
}

.empty-text {
  font-size: 20px;
  font-weight: 600;
  margin-bottom: 10px;
  color: #333;
}

.empty-hint {
  font-size: 16px;
  opacity: 0.7;
}

/* 加载状态 */
.loading-state {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  height: 250px;
  gap: 20px;
  color: #dc2626;
}

.loading-spinner {
  width: 60px;
  height: 60px;
  border: 4px solid rgba(220, 38, 38, 0.1);
  border-top-color: #dc2626;
  border-radius: 50%;
  animation: spin 1s linear infinite;
}

@keyframes spin {
  to { transform: rotate(360deg); }
}

.loading-text {
  font-size: 18px;
  font-weight: 600;
}

/* 模态框样式 */
.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: rgba(0, 0, 0, 0.8);
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 1000;
  backdrop-filter: blur(5px);
}

.modal-content {
  background: #ffffff;
  border-radius: 12px;
  box-shadow: 0 15px 50px rgba(0, 0, 0, 0.3);
  width: 90%;
  max-width: 800px;
  max-height: 80vh;
  overflow: hidden;
  display: flex;
  flex-direction: column;
  animation: modalSlideIn 0.3s ease-out;
  border: 2px solid #e0e0e0;
}

@keyframes modalSlideIn {
  from {
    opacity: 0;
    transform: translateY(-50px) scale(0.9);
  }
  to {
    opacity: 1;
    transform: translateY(0) scale(1);
  }
}

.modal-header {
  background: linear-gradient(135deg, #dc2626 0%, #ef4444 100%);
  color: #ffffff;
  padding: 25px 30px;
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.modal-header h2 {
  margin: 0;
  font-size: 24px;
  font-weight: 700;
  color: #ffffff;
}

.modal-close {
  background: rgba(255, 255, 255, 0.2);
  border: none;
  color: #ffffff;
  font-size: 32px;
  cursor: pointer;
  padding: 0;
  margin: 0;
  transition: all 0.3s ease;
  width: 40px;
  height: 40px;
  display: flex;
  align-items: center;
  justify-content: center;
  border-radius: 50%;
  backdrop-filter: blur(10px);
}

.modal-close:hover {
  background: rgba(255, 255, 255, 0.3);
  transform: rotate(90deg);
}

.modal-body {
  padding: 30px;
  overflow-y: auto;
  flex-grow: 1;
  background: #ffffff;
  color: #333;
}

.modal-footer {
  padding: 25px 30px;
  background: #fafafa;
  text-align: right;
  border-top: 2px solid #e0e0e0;
}

/* 滚动条样式 */
.card-body::-webkit-scrollbar, .modal-body::-webkit-scrollbar {
  width: 10px;
}

.card-body::-webkit-scrollbar-track, .modal-body::-webkit-scrollbar-track {
  background: #f0f0f0;
  border-radius: 5px;
}

.card-body::-webkit-scrollbar-thumb, .modal-body::-webkit-scrollbar-thumb {
  background: #fecaca;
  border-radius: 5px;
}

.card-body::-webkit-scrollbar-thumb:hover, .modal-body::-webkit-scrollbar-thumb:hover {
  background: #fca5a5;
}

/* 响应式设计 */
@media (max-width: 1200px) {
  .content-grid {
    grid-template-columns: 1fr;
    gap: 25px;
  }
  
  .form-card, .list-card {
    max-width: 100%;
  }
}
</style>