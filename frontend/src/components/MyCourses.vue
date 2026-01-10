<template>
  <div class="my-courses">
    <div class="content-container">
      <div v-if="loading" class="loading">
        加载中...
      </div>
      <div v-else-if="error" class="error">
        {{ error }}
      </div>
      <div v-else-if="courses.length === 0" class="no-data">
        暂无课程信息
      </div>
      <div v-else class="courses-list">
        <div class="course-card" v-for="course in courses" :key="course.id">
            <h3>{{ course.title }}</h3>
            <p class="course-code">{{ course.code }}</p>
            <p class="course-description">{{ course.description }}</p>
            <div class="course-info">
              <span class="info-item"><strong>学分:</strong> {{ course.credits }}</span>
              <span class="info-item"><strong>教师:</strong> {{ course.instructor }}</span>
            </div>
          </div>
      </div>
    </div>
  </div>
</template>

<script>
import { courseService } from '../services/api';

export default {
  name: 'MyCourses',
  data() {
    return {
      courses: [],
      loading: true,
      error: ''
    };
  },
  mounted() {
    this.fetchMyCourses();
  },
  methods: {
    async fetchMyCourses() {
      try {
        const response = await courseService.getMyCourses();
        this.courses = response.data;
      } catch (error) {
        this.error = '获取课程信息失败';
        console.error('获取课程信息失败:', error);
      } finally {
        this.loading = false;
      }
    }
  }
};
</script>

<style scoped>
/* 页面容器 */
.my-courses {
  width: 100%;
  padding: 0;
  background: #f8f9fa;
  min-height: 100%;
}

/* 内容容器 */
.content-container {
  width: 100%;
  margin: 0;
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

/* 课程列表 */
.courses-list {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
  gap: 20px;
  margin: 0;
  width: 100%;
}

/* 课程卡片 */
.course-card {
  padding: 25px;
  background: white;
  border-radius: 8px;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
  transition: all 0.3s ease;
  border: 1px solid rgba(0, 0, 0, 0.08);
  overflow: hidden;
  position: relative;
}

.course-card:hover {
  transform: translateY(-3px);
  box-shadow: 0 8px 20px rgba(220, 38, 38, 0.15);
  border-color: rgba(220, 38, 38, 0.2);
}

.course-card h3 {
  color: #dc2626;
  margin-bottom: 12px;
  font-size: 22px;
  font-weight: 700;
  transition: all 0.3s ease;
  line-height: 1.3;
}

.course-card:hover h3 {
  color: #ef4444;
}

.course-code {
  color: #ef4444;
  font-size: 13px;
  margin-bottom: 18px;
  font-weight: 600;
  text-transform: uppercase;
  letter-spacing: 1px;
  background: rgba(220, 38, 38, 0.1);
  display: inline-block;
  padding: 6px 12px;
  border-radius: 16px;
  transition: all 0.3s ease;
}

.course-card:hover .course-code {
  background: rgba(220, 38, 38, 0.15);
  color: #dc2626;
}

.course-description {
  color: #6b7280;
  margin-bottom: 20px;
  line-height: 1.6;
  min-height: 70px;
  font-size: 14px;
  transition: all 0.3s ease;
}

.course-card:hover .course-description {
  color: #4b5563;
}

.course-info {
  display: flex;
  flex-direction: column;
  gap: 10px;
  padding-top: 18px;
  border-top: 1px solid rgba(0, 0, 0, 0.08);
}

.info-item {
  color: #4b5563;
  font-size: 14px;
  font-weight: 500;
  display: flex;
  justify-content: space-between;
  align-items: center;
  transition: all 0.3s ease;
}

.course-card:hover .info-item {
  color: #2d3748;
}

.info-item strong {
  font-weight: 600;
  color: #ef4444;
  min-width: 50px;
  text-align: left;
}

.course-card:hover .info-item strong {
  color: #dc2626;
}
</style>