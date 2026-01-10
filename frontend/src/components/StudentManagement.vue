<template>
  <div class="student-management">
    <div class="content-grid">
      <!-- 学生列表 -->
      <div class="list-card">
        <div class="card-header">
          <h2>学生列表</h2>
          <div class="card-actions">
            <button class="btn btn-primary" @click="showAddModal = true">
              <span class="stat-icon">+</span> 添加学生
            </button>
          </div>
          <div class="card-stats">
            <span class="stat-item">
              <span class="stat-icon">👥</span>
              <span class="stat-value">{{ students.length }}</span>
              <span class="stat-label">学生总数</span>
            </span>
          </div>
        </div>
        <div class="card-body">
          <div v-if="students.length === 0" class="empty-state">
            <div class="empty-icon"></div>
            <div class="empty-text">暂无学生数据</div>
            <div class="empty-hint">点击添加学生按钮开始管理</div>
          </div>
          <div v-else class="student-list">
            <div class="table-wrapper">
              <table class="student-table">
                <thead>
                  <tr>
                    <th class="table-th">ID</th>
                    <th class="table-th">姓名</th>
                    <th class="table-th">年龄</th>
                    <th class="table-th">邮箱</th>
                    <th class="table-th table-th-actions">操作</th>
                  </tr>
                </thead>
                <tbody>
                  <tr v-for="student in students" :key="student.id" class="table-tr">
                    <td class="table-td">{{ student.id }}</td>
                    <td class="table-td">{{ student.name }}</td>
                    <td class="table-td">{{ student.age }}</td>
                    <td class="table-td">{{ student.email }}</td>
                    <td class="table-td table-td-actions">
                      <div class="action-buttons">
                        <button 
                          class="btn btn-edit" 
                          @click="editStudent(student)"
                          title="编辑"
                        >
                          编辑
                        </button>
                        <button 
                          class="btn btn-view" 
                          @click="viewStudentCourses(student.id)"
                          title="查看课程"
                        >
                          查看课程
                        </button>
                        <button 
                          class="btn btn-delete" 
                          @click="deleteStudent(student.id)"
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
    
    <!-- 学生课程模态框 -->
    <div v-if="showCoursesModal" class="modal-overlay" @click="showCoursesModal = false">
      <div class="modal-content" @click.stop>
        <div class="modal-header">
          <h2>{{ selectedStudentName }}的课程</h2>
          <button class="modal-close" @click="showCoursesModal = false">×</button>
        </div>
        <div class="modal-body">
          <div v-if="coursesLoading" class="loading-state">
            <div class="loading-spinner"></div>
            <div class="loading-text">加载课程中...</div>
          </div>
          <div v-else-if="studentCourses.length === 0" class="empty-state">
            <div class="empty-icon"></div>
            <div class="empty-text">暂无课程信息</div>
          </div>
          <table v-else class="student-courses-table">
            <thead>
              <tr>
                <th>课程名称</th>
                <th>课程代码</th>
                <th>教师</th>
                <th>学分</th>
                <th>成绩</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="enrollment in studentCourses" :key="enrollment.id">
                <td>{{ enrollment.course?.title || '未知课程' }}</td>
                <td>{{ enrollment.course?.code || '未知代码' }}</td>
                <td>{{ enrollment.course?.instructor || '未知教师' }}</td>
                <td>{{ enrollment.course?.credits || 0 }}</td>
                <td>{{ enrollment.grade !== null ? enrollment.grade : '未评分' }}</td>
              </tr>
            </tbody>
          </table>
        </div>
        <div class="modal-footer">
          <button class="btn btn-primary" @click="showCoursesModal = false">关闭</button>
        </div>
      </div>
    </div>
    
    <!-- 编辑学生模态框 -->
    <div v-if="showEditModal" class="modal-overlay" @click="showEditModal = false">
      <div class="modal-content" @click.stop>
        <div class="modal-header">
          <h2>编辑学生</h2>
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
                    placeholder="请输入学生ID"
                    min="1000"
                    disabled
                  >
                </div>
              </div>
              <div class="form-group">
                <label for="edit-name">姓名</label>
                <div class="input-wrapper">
                  <input 
                    type="text" 
                    id="edit-name" 
                    v-model="editForm.name" 
                    required
                    placeholder="请输入学生姓名"
                  >
                </div>
              </div>
            </div>
            <div class="form-row">
              <div class="form-group">
                <label for="edit-age">年龄</label>
                <div class="input-wrapper">
                  <input 
                    type="number" 
                    id="edit-age" 
                    v-model="editForm.age" 
                    required
                    placeholder="请输入学生年龄"
                    min="1"
                    max="100"
                  >
                </div>
              </div>
              <div class="form-group">
                <label for="edit-email">邮箱</label>
                <div class="input-wrapper">
                  <input 
                    type="email" 
                    id="edit-email" 
                    v-model="editForm.email" 
                    required
                    placeholder="请输入学生邮箱"
                  >
                </div>
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
    
    <!-- 添加学生模态框 -->
    <div v-if="showAddModal" class="modal-overlay" @click="showAddModal = false">
      <div class="modal-content" @click.stop>
        <div class="modal-header">
          <h2>添加学生</h2>
          <button class="modal-close" @click="showAddModal = false">×</button>
        </div>
        <div class="modal-body">
          <form @submit.prevent="handleAddSubmit" class="student-form">
            <div class="form-row">
              <div class="form-group">
                <label for="add-id">ID</label>
                <div class="input-wrapper">
                  <input 
                    type="number" 
                    id="add-id" 
                    v-model="form.id" 
                    required
                    placeholder="请输入学生ID"
                    min="1000"
                  >
                </div>
              </div>
              <div class="form-group">
                <label for="add-name">姓名</label>
                <div class="input-wrapper">
                  <input 
                    type="text" 
                    id="add-name" 
                    v-model="form.name" 
                    required
                    placeholder="请输入学生姓名"
                  >
                </div>
              </div>
            </div>
            <div class="form-row">
              <div class="form-group">
                <label for="add-age">年龄</label>
                <div class="input-wrapper">
                  <input 
                    type="number" 
                    id="add-age" 
                    v-model="form.age" 
                    required
                    placeholder="请输入学生年龄"
                    min="1"
                    max="100"
                  >
                </div>
              </div>
              <div class="form-group">
                <label for="add-email">邮箱</label>
                <div class="input-wrapper">
                  <input 
                    type="email" 
                    id="add-email" 
                    v-model="form.email" 
                    required
                    placeholder="请输入学生邮箱"
                  >
                </div>
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
import { studentService, enrollmentService } from '../services/api';

export default {
  name: 'StudentManagement',
  data() {
    return {
      students: [],
      form: {
        id: null,
        name: '',
        age: '',
        email: ''
      },
      // 添加学生相关
      showAddModal: false,
      // 编辑学生相关
      showEditModal: false,
      editForm: {
        id: null,
        name: '',
        age: '',
        email: ''
      },
      // 查看学生课程相关
      showCoursesModal: false,
      selectedStudentId: null,
      selectedStudentName: '',
      studentCourses: [],
      coursesLoading: false
    };
  },
  mounted() {
    this.fetchStudents();
  },
  methods: {
    async fetchStudents() {
      try {
        const response = await studentService.getAll();
        console.log('获取到的学生数据:', response.data);
        this.students = response.data;
      } catch (error) {
        console.error('获取学生列表失败:', error);
        alert('获取学生列表失败');
      }
    },
    async viewStudentCourses(studentId) {
      try {
        this.selectedStudentId = studentId;
        this.selectedStudentName = this.students.find(s => s.id === studentId)?.name || '未知学生';
        this.coursesLoading = true;
        this.showCoursesModal = true;
        
        // 获取学生的课程信息
        const response = await enrollmentService.getByStudentId(studentId);
        this.studentCourses = response.data;
      } catch (error) {
        console.error('获取学生课程失败:', error);
        alert('获取学生课程失败');
      } finally {
        this.coursesLoading = false;
      }
    },
    async handleAddSubmit() {
      try {
        // 添加学生时包含id字段，允许自定义学生ID
        await studentService.create(this.form);
        alert('学生添加成功');
        this.fetchStudents();
        this.closeAddModal();
      } catch (error) {
        console.error('添加学生失败:', error);
        alert('添加学生失败: ' + (error.response?.data?.detail || error.message || '未知错误'));
      }
    },
    closeAddModal() {
      this.showAddModal = false;
      this.resetForm();
    },
    editStudent(student) {
      console.log('开始编辑学生:', student);
      this.showEditModal = true;
      this.editForm = {
        id: student.id,
        name: student.name,
        age: student.age,
        email: student.email
      };
    },
    async handleEditSubmit() {
      try {
        await studentService.update(this.editForm.id, this.editForm);
        alert('学生更新成功');
        this.fetchStudents();
        this.showEditModal = false;
      } catch (error) {
        console.error('更新学生失败:', error);
        alert('更新学生失败: ' + (error.response?.data?.detail || error.message || '未知错误'));
      }
    },
    async deleteStudent(id) {
      if (confirm('确定要删除这个学生吗?')) {
        try {
          await studentService.delete(id);
          alert('学生删除成功');
          this.fetchStudents();
        } catch (error) {
          console.error('删除失败:', error);
          alert('删除失败');
        }
      }
    },
    resetForm() {
      this.form = {
        id: null,
        name: '',
        age: null,
        email: ''
      };
    }
  }
};
</script>

<style scoped>
/* 页面容器 */
.student-management {
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

.input-wrapper input {
  width: 100%;
  padding: 14px 16px;
  border: 2px solid #e0e0e0;
  border-radius: 8px;
  font-size: 16px;
  transition: all 0.3s ease;
  background-color: #fafafa;
  color: #333;
}

.input-wrapper input:focus {
  outline: none;
  border-color: #ef4444;
  background-color: white;
  box-shadow: 0 0 0 3px rgba(43, 108, 176, 0.1);
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

.btn-view {
  background: #fef2f2;
  color: #dc2626;
  border: 2px solid #fee2e2;
}

.btn-view:hover {
  background: #fee2e2;
  color: #b91c1c;
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

/* 学生课程表格 */
.student-courses-table {
  width: 100%;
  border-collapse: collapse;
  background: #ffffff;
  border-radius: 8px;
  overflow: hidden;
  border: 2px solid #e0e0e0;
}

.student-courses-table th,
.student-courses-table td {
  padding: 20px 25px;
  text-align: left;
  border-bottom: 1px solid #e0e0e0;
  border-right: 1px solid #e0e0e0;
}

.student-courses-table th {
  background: linear-gradient(135deg, #dc2626 0%, #ef4444 100%);
  color: #ffffff;
  font-weight: 700;
  text-transform: uppercase;
  letter-spacing: 0.5px;
  font-size: 14px;
}

.student-courses-table td:last-child,
.student-courses-table th:last-child {
  border-right: none;
}

.student-courses-table tr:hover {
  background: rgba(220, 38, 38, 0.05);
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