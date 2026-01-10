<template>
  <div class="my-grades">
    <div class="content-container">
      <div v-if="loading" class="loading">
        加载中...
      </div>
      <div v-else-if="error" class="error">
        {{ error }}
      </div>
      <div v-else-if="grades.length === 0" class="no-data">
        暂无成绩信息
      </div>
      <div v-else>
        <div class="grades-table-container">
          <table class="grades-table">
            <thead>
              <tr>
                <th><span class="header-icon">📚</span> 课程名称</th>
                <th><span class="header-icon">🔢</span> 课程代码</th>
                <th><span class="header-icon">👨‍🏫</span> 任课教师</th>
                <th><span class="header-icon">💯</span> 学分</th>
                <th><span class="header-icon">📊</span> 成绩</th>
                <th><span class="header-icon">📅</span> 学期</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="grade in grades" :key="grade.id" class="table-tr">
                <td class="table-td">
                  <div class="course-title">{{ grade.courseTitle }}</div>
                  <div class="course-description">{{ grade.description }}</div>
                </td>
                <td class="table-td">{{ grade.courseCode }}</td>
                <td class="table-td">{{ grade.instructor }}</td>
                <td class="table-td">{{ grade.credits }}</td>
                <td class="table-td grade-value">{{ grade.grade }}</td>
                <td class="table-td">{{ grade.semester }}</td>
              </tr>
            </tbody>
          </table>
        </div>
        <div class="gpa-calculation">
          <h3>GPA 计算</h3>
          <div class="gpa-info">
            <span><strong>GPA:</strong> {{ gpa.toFixed(2) }}</span>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { enrollmentService } from '../services/api';

export default {
  name: 'MyGrades',
  data() {
    return {
      grades: [],
      loading: true,
      error: '',
      gpa: 0
    };
  },
  mounted() {
    this.fetchMyGrades();
  },
  methods: {
    async fetchMyGrades() {
      try {
        const response = await enrollmentService.getMyGrades();
        this.grades = response.data;
        this.calculateGPA();
      } catch (error) {
        this.error = '获取成绩信息失败';
        console.error('获取成绩信息失败:', error);
      } finally {
        this.loading = false;
      }
    },
    calculateGPA() {
      if (this.grades.length === 0) {
        this.gpa = 0;
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
        // 确保credits是数字
        const credits = Number(grade.credits) || 0;
        totalCredits += credits;
        
        // 确保grade是字符串
        const gradeString = String(grade.grade).toUpperCase();
        // 获取成绩对应的GPA点数
        const gpaPoints = gradeGPAMap[gradeString] || 0.0;
        totalGPAPoints += gpaPoints * credits;
      });
      
      this.gpa = totalGPAPoints / totalCredits;
    }
  }
};
</script>

<style scoped>
/* 页面容器 */
.my-grades {
  width: 100%;
  padding: 0;
  background: #f8f9fa;
  min-height: 100%;
}

/* 内容容器 */
.content-container {
  width: 100%;
  margin: 0;
  padding: 0;
}

/* 加载和错误状态 */
.loading, .error, .no-data {
  text-align: center;
  padding: 60px 20px;
  font-size: 18px;
  font-weight: 500;
  border-radius: 8px;
  max-width: 100%;
  margin: 0;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
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

.no-data {
  background: white;
  color: #6b7280;
  border: 2px dashed rgba(220, 38, 38, 0.3);
}

/* 成绩表格容器 */
.grades-table-container {
  overflow-x: auto;
  margin: 0 0 20px 0;
  width: 100%;
  border-radius: 8px;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
  background: white;
}

/* 成绩表格 */
.grades-table {
  width: 100%;
  border-collapse: collapse;
  background: white;
  border-radius: 8px;
  overflow: hidden;
}

.grades-table th {
  background: linear-gradient(135deg, #dc2626 0%, #ef4444 100%);
  color: #ffffff;
  padding: 18px 20px;
  text-align: left;
  font-weight: 600;
  font-size: 14px;
  text-transform: uppercase;
  letter-spacing: 0.5px;
  border-right: 1px solid rgba(255, 255, 255, 0.2);

}

.header-icon {
  font-size: 18px;
  vertical-align: middle;
  margin-right: 10px;
}

.grades-table th:last-child {
  border-right: none;
}

.grades-table td {
  padding: 18px 20px;
  border-bottom: 1px solid rgba(0, 0, 0, 0.08);
  border-right: 1px solid rgba(0, 0, 0, 0.08);
  font-size: 15px;
  color: #374151;
  background: white;
  transition: all 0.2s ease;
}

.grades-table td:last-child {
  border-right: none;
}

.grades-table tr:last-child td {
  border-bottom: none;
}

.grades-table tr:hover {
  background: rgba(220, 38, 38, 0.05);
  transform: translateY(-1px);
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.08);
}

/* 课程标题和描述 */
.course-title {
  font-weight: 700;
  color: #dc2626;
  margin-bottom: 5px;
  font-size: 16px;
}

.course-description {
  font-size: 13px;
  color: #6b7280;
  line-height: 1.4;
}

/* 成绩值 */
.grade-value {
  font-weight: 700;
  color: #ef4444;
  font-size: 16px;
  background: rgba(220, 38, 38, 0.1);
  padding: 6px 12px;
  border-radius: 16px;
  display: inline-block;
  min-width: 60px;
  text-align: center;
}

/* GPA 计算区域 */
.gpa-calculation {
  background: linear-gradient(135deg, #fee2e2 0%, #fecaca 100%);
  border-radius: 12px;
  padding: 30px;
  margin: 20px 0 0 0;
  width: 100%;
  box-shadow: 0 6px 20px rgba(239, 68, 68, 0.15);
  border: 1px solid #fecaca;
  color: #dc2626;
  position: relative;
}

.gpa-calculation h3 {
  color: #dc2626;
  margin-bottom: 25px;
  font-size: 24px;
  font-weight: 800;
  display: flex;
  align-items: center;
  gap: 15px;
  position: relative;
  z-index: 1;
}

.gpa-calculation h3::before {
  content: '�';
  font-size: 28px;
  text-shadow: 0 2px 4px rgba(0, 0, 0, 0.2);
}

.gpa-info {
  display: flex;
  justify-content: flex-start;
  gap: 30px;
  flex-wrap: wrap;
  font-size: 20px;
  position: relative;
  z-index: 1;
}

.gpa-info span {
  color: #dc2626;
  font-weight: 700;
  background: rgba(239, 68, 68, 0.08);
  padding: 15px 30px;
  border-radius: 30px;
  display: flex;
  align-items: center;
  gap: 15px;
  transition: all 0.3s ease;
  border: 2px solid rgba(255, 255, 255, 0.3);
  backdrop-filter: blur(10px);
  box-shadow: 0 4px 15px rgba(0, 0, 0, 0.1);
}

.gpa-info span:hover {
  background: rgba(255, 255, 255, 0.25);
  border-color: rgba(255, 255, 255, 0.5);
  transform: translateY(-3px);
  box-shadow: 0 6px 25px rgba(0, 0, 0, 0.2);
}

.gpa-info strong {
  color: #ffd700;
  font-weight: 800;
  font-size: 28px;
  text-shadow: 0 2px 4px rgba(0, 0, 0, 0.2);
}
</style>