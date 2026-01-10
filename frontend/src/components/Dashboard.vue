<template>
  <div class="dashboard-container">
    <h1 class="dashboard-title">管理员控制面板</h1>

    <!-- 数据统计卡片 -->
    <div class="stats-grid">
      <div class="stat-card">
        <div class="stat-icon students"><i class="fa fa-users"></i></div>
        <div class="stat-content">
          <h3 class="stat-number">{{ studentCount }}</h3>
          <p class="stat-label">学生总数</p>
        </div>
      </div>

      <div class="stat-card">
        <div class="stat-icon courses"><i class="fa fa-book"></i></div>
        <div class="stat-content">
          <h3 class="stat-number">{{ courseCount }}</h3>
          <p class="stat-label">课程总数</p>
        </div>
      </div>

      <div class="stat-card">
        <div class="stat-icon enrollments"><i class="fa fa-clipboard-list"></i></div>      
        <div class="stat-content">
          <h3 class="stat-number">{{ enrollmentCount }}</h3>
          <p class="stat-label">选课总数</p>
        </div>
      </div>

      <div class="stat-card">
        <div class="stat-icon users"><i class="fa fa-user-cog"></i></div>
        <div class="stat-content">
          <h3 class="stat-number">{{ userCount }}</h3>
          <p class="stat-label">用户总数</p>
        </div>
      </div>
    </div>

    <!-- 主要内容区域 -->
    <div class="dashboard-content">
      <!-- 最近操作日志 -->
      <div class="log-section">
        <h2 class="section-title">最近操作日志</h2>
        <div class="log-table-container">
          <table class="log-table">
            <thead>
              <tr>
                <th>时间</th>
                <th>操作</th>
                <th>用户</th>
                <th>详情</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="log in recentLogs" :key="log.id">
                <td>{{ formatDate(log.timestamp) }}</td>
                <td>{{ log.action }}</td>
                <td>{{ log.user }}</td>
                <td>{{ log.details }}</td>
              </tr>
              <tr v-if="recentLogs.length === 0">
                <td colspan="4" class="no-data">暂无日志记录</td>
              </tr>
            </tbody>
          </table>
        </div>
        <div class="view-all-btn">
          <button @click="$router.push('/logs')" class="btn btn-primary">查看全部日志</button>
        </div>
      </div>

      <!-- 快速功能按钮 -->
      <div class="quick-actions">
        <h2 class="section-title">快速操作</h2>
        <div class="action-buttons">
          <button @click="$router.push('/students')" class="btn btn-primary">
            <i class="fa fa-users"></i> 学生管理
          </button>
          <button @click="$router.push('/courses')" class="btn btn-primary">
            <i class="fa fa-book"></i> 课程管理
          </button>
          <button @click="$router.push('/enrollments')" class="btn btn-primary">
            <i class="fa fa-clipboard-list"></i> 成绩管理
          </button>
          <button @click="$router.push('/logs')" class="btn btn-primary">
            <i class="fa fa-history"></i> 日志管理
          </button>
        </div>
      </div>
      
      <!-- 仪表盘分析 -->
      <div class="dashboard-analysis">
        <h2 class="section-title">数据分析</h2>
        <div class="analysis-grid">
          <!-- 学生年龄分布 -->
          <div class="analysis-card">
            <h3 class="analysis-title">学生年龄分布</h3>
            <div class="age-distribution">
              <div class="age-group" v-for="group in ageDistribution" :key="group.age">
                <div class="age-label">{{ group.age }}岁</div>
                <div class="age-bar-container">
                  <div class="age-bar" :style="{ width: group.percentage + '%' }"></div>
                </div>
                <div class="age-count">{{ group.count }}</div>
              </div>
            </div>
          </div>
          
          <!-- 课程学分分布 -->
          <div class="analysis-card">
            <h3 class="analysis-title">课程学分分布</h3>
            <div class="credits-distribution">
              <div class="credit-item" v-for="item in creditsDistribution" :key="item.credits">
                <div class="credit-label">{{ item.credits }}学分</div>
                <div class="credit-circle" :style="{ '--percentage': item.percentage + '%' }"></div>
                <div class="credit-count">{{ item.count }}门课程</div>
              </div>
            </div>
          </div>
          
          <!-- 最近一周新增 -->
          <div class="analysis-card full-width">
            <h3 class="analysis-title">最近一周新增数据</h3>
            <div class="weekly-stats">
              <div class="weekly-item">
                <div class="weekly-label">新增学生</div>
                <div class="weekly-value">{{ weeklyStats.newStudents }}</div>
              </div>
              <div class="weekly-item">
                <div class="weekly-label">新增课程</div>
                <div class="weekly-value">{{ weeklyStats.newCourses }}</div>
              </div>
              <div class="weekly-item">
                <div class="weekly-label">新增选课</div>
                <div class="weekly-value">{{ weeklyStats.newEnrollments }}</div>
              </div>
              <div class="weekly-item">
                <div class="weekly-label">新增用户</div>
                <div class="weekly-value">{{ weeklyStats.newUsers }}</div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: 'Dashboard',
  data() {
    return {
      // 统计数据
      studentCount: 0,
      courseCount: 0,
      enrollmentCount: 0,
      userCount: 0,
      // 最近日志（模拟数据）
      recentLogs: [
        {
          id: 1,
          timestamp: Date.now() - 3600000, // 1小时前
          action: '添加学生',
          user: 'admin',
          details: '添加了学生：张三 (ID: 1001)'
        },
        {
          id: 2,
          timestamp: Date.now() - 7200000, // 2小时前
          action: '添加课程',
          user: 'admin',
          details: '添加了课程：高等数学 (ID: 2001)'
        },
        {
          id: 3,
          timestamp: Date.now() - 10800000, // 3小时前
          action: '成绩录入',
          user: 'admin',
          details: '为学生张三录入了高等数学成绩：A'
        },
        {
          id: 4,
          timestamp: Date.now() - 14400000, // 4小时前
          action: '删除学生',
          user: 'admin',
          details: '删除了学生：李四 (ID: 1002)'
        },
        {
          id: 5,
          timestamp: Date.now() - 18000000, // 5小时前
          action: '更新课程',
          user: 'admin',
          details: '更新了课程：大学英语 (ID: 2002)'
        }
      ],
      // 仪表盘分析数据
      ageDistribution: [
        { age: 18, count: 15, percentage: 30 },
        { age: 19, count: 20, percentage: 40 },
        { age: 20, count: 10, percentage: 20 },
        { age: 21, count: 5, percentage: 10 }
      ],
      creditsDistribution: [
        { credits: 2, count: 10, percentage: 40 },
        { credits: 3, count: 8, percentage: 32 },
        { credits: 4, count: 7, percentage: 28 }
      ],
      weeklyStats: {
        newStudents: 5,
        newCourses: 2,
        newEnrollments: 12,
        newUsers: 3
      }
    };
  },
  mounted() {
    this.fetchData();
  },
  methods: {
    async fetchData() {
      try {
        // 这里应该调用API获取真实数据
        // 暂时使用模拟数据
        this.studentCount = 6;
        this.courseCount = 15;
        this.enrollmentCount = 45;
        this.userCount = 8;
      } catch (error) {
        console.error('获取数据失败:', error);
      }
    },
    formatDate(timestamp) {
      // 格式化时间戳为可读日期
      if (!timestamp) return '';
      const date = new Date(timestamp);
      return date.toLocaleString();
    }
  }
};
</script>

<style scoped>
/* 页面容器 */
.dashboard-container {
  width: 100%;
  padding: 20px;
  background: #f8f9fa;
  min-height: calc(100vh - 80px);
}

/* 标题样式 */
.dashboard-title {
  color: #333;
  margin-bottom: 30px;
  font-size: 28px;
  font-weight: 700;
  text-align: center;
  text-transform: uppercase;
  letter-spacing: 1px;
}

/* 统计卡片网格 */
.stats-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
  gap: 20px;
  margin-bottom: 30px;
  padding: 0 20px;
}

/* 统计卡片 */
.stat-card {
  background: #ffffff;
  border-radius: 12px;
  box-shadow: 0 4px 15px rgba(0, 0, 0, 0.1);
  padding: 25px;
  display: flex;
  align-items: center;
  gap: 20px;
  transition: all 0.3s ease;
  border: 2px solid #e0e0e0;
}

.stat-card:hover {
  box-shadow: 0 8px 25px rgba(0, 0, 0, 0.15);
  transform: translateY(-3px);
  border-color: #ef4444;
}

/* 统计图标 */
.stat-icon {
  width: 60px;
  height: 60px;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 24px;
  color: #ffffff;
  background: linear-gradient(135deg, #dc2626 0%, #ef4444 100%);
  box-shadow: 0 4px 12px rgba(220, 38, 38, 0.3);
}

.stat-icon.students {
  background: linear-gradient(135deg, #dc2626 0%, #ef4444 100%);
}

.stat-icon.courses {
  background: linear-gradient(135deg, #dc2626 0%, #ef4444 100%);
}

.stat-icon.enrollments {
  background: linear-gradient(135deg, #dc2626 0%, #ef4444 100%);
}

.stat-icon.users {
  background: linear-gradient(135deg, #dc2626 0%, #ef4444 100%);
}

/* 统计内容 */
.stat-content {
  flex: 1;
}

.stat-number {
  font-size: 32px;
  font-weight: 800;
  color: #dc2626;
  margin: 0;
  line-height: 1;
}

.stat-label {
  font-size: 14px;
  color: #666;
  margin: 8px 0 0 0;
  text-transform: uppercase;
  letter-spacing: 0.5px;
}

/* 主要内容区域 */
.dashboard-content {
  background: #ffffff;
  border-radius: 12px;
  box-shadow: 0 4px 15px rgba(0, 0, 0, 0.1);
  padding: 30px;
  margin: 0 20px;
}

/* 区域标题 */
.section-title {
  color: #333;
  margin-bottom: 25px;
  font-size: 22px;
  font-weight: 700;
  text-transform: uppercase;
  letter-spacing: 0.5px;
  position: relative;
  padding-bottom: 10px;
}

.section-title::after {
  content: '';
  position: absolute;
  bottom: 0;
  left: 0;
  width: 50px;
  height: 4px;
  background: linear-gradient(135deg, #dc2626 0%, #ef4444 100%);
  border-radius: 2px;
}

/* 日志部分 */
.log-section {
  margin-bottom: 30px;
}

.log-table-container {
  overflow-x: auto;
  border-radius: 8px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
  margin-bottom: 20px;
}

.log-table {
  width: 100%;
  border-collapse: collapse;
  background: #ffffff;
}

.log-table th {
  background: linear-gradient(135deg, #dc2626 0%, #ef4444 100%);
  color: #ffffff;
  padding: 15px 20px;
  text-align: left;
  font-weight: 600;
  font-size: 14px;
  text-transform: uppercase;
  letter-spacing: 0.5px;
  border-right: 1px solid rgba(255, 255, 255, 0.2);
}

.log-table th:last-child {
  border-right: none;
}

.log-table td {
  padding: 15px 20px;
  border-bottom: 1px solid #e0e0e0;
  font-size: 14px;
  color: #333;
}

.log-table tr:last-child td {
  border-bottom: none;
}

.log-table tr:hover {
  background: rgba(220, 38, 38, 0.05);
}

.no-data {
  text-align: center;
  color: #666;
  font-style: italic;
}

/* 查看全部按钮 */
.view-all-btn {
  text-align: right;
}

/* 按钮样式 */
.btn {
  padding: 12px 25px;
  border: none;
  border-radius: 8px;
  cursor: pointer;
  transition: all 0.3s ease;
  font-weight: 600;
  font-size: 14px;
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

/* 快速操作 */
.quick-actions {
  margin-bottom: 30px;
}

.action-buttons {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
  gap: 15px;
}

/* 仪表盘分析 */
.dashboard-analysis {
  margin-top: 40px;
}

.analysis-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(400px, 1fr));
  gap: 25px;
}

.analysis-card {
  background: #ffffff;
  border-radius: 12px;
  box-shadow: 0 4px 15px rgba(0, 0, 0, 0.1);
  padding: 25px;
  border: 2px solid #e0e0e0;
  transition: all 0.3s ease;
}

.analysis-card:hover {
  box-shadow: 0 8px 25px rgba(0, 0, 0, 0.15);
  border-color: #ef4444;
}

.analysis-card.full-width {
  grid-column: 1 / -1;
}

.analysis-title {
  font-size: 18px;
  font-weight: 600;
  color: #333;
  margin-bottom: 20px;
  text-transform: uppercase;
  letter-spacing: 0.5px;
}

/* 学生年龄分布 */
.age-distribution {
  display: flex;
  flex-direction: column;
  gap: 15px;
}

.age-group {
  display: flex;
  align-items: center;
  gap: 15px;
}

.age-label {
  width: 40px;
  font-weight: 600;
  color: #333;
}

.age-bar-container {
  flex: 1;
  height: 20px;
  background: #f0f0f0;
  border-radius: 10px;
  overflow: hidden;
}

.age-bar {
  height: 100%;
  background: linear-gradient(135deg, #dc2626 0%, #ef4444 100%);
  border-radius: 10px;
  transition: width 0.5s ease;
}

.age-count {
  width: 30px;
  text-align: right;
  font-weight: 600;
  color: #dc2626;
}

/* 课程学分分布 */
.credits-distribution {
  display: flex;
  justify-content: space-around;
  align-items: center;
}

.credit-item {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 10px;
}

.credit-label {
  font-weight: 600;
  color: #333;
  text-transform: uppercase;
  font-size: 14px;
}

.credit-circle {
  width: 120px;
  height: 120px;
  border-radius: 50%;
  background: conic-gradient(
    #ef4444 var(--percentage),
    #f0f0f0 var(--percentage)
  );
  display: flex;
  align-items: center;
  justify-content: center;
  position: relative;
}

.credit-circle::before {
  content: '';
  width: 90px;
  height: 90px;
  background: #ffffff;
  border-radius: 50%;
  position: absolute;
}

.credit-count {
  font-weight: 600;
  color: #666;
  font-size: 14px;
}

/* 最近一周新增数据 */
.weekly-stats {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
  gap: 20px;
}

.weekly-item {
  background: #f8f9fa;
  padding: 20px;
  border-radius: 8px;
  text-align: center;
  transition: all 0.3s ease;
  border: 2px solid #e0e0e0;
}

.weekly-item:hover {
  background: #fee2e2;
  border-color: #ef4444;
  transform: translateY(-2px);
}

.weekly-label {
  font-size: 14px;
  color: #666;
  margin-bottom: 10px;
  text-transform: uppercase;
  letter-spacing: 0.5px;
}

.weekly-value {
  font-size: 32px;
  font-weight: 800;
  color: #dc2626;
}

/* 响应式设计 */
@media (max-width: 1200px) {
  .analysis-grid {
    grid-template-columns: 1fr;
  }
  
  .analysis-card.full-width {
    grid-column: 1;
  }
}

@media (max-width: 768px) {
  .dashboard-container {
    padding: 10px;
  }
  
  .dashboard-title {
    font-size: 24px;
    margin-bottom: 20px;
  }
  
  .stats-grid {
    grid-template-columns: 1fr;
    padding: 0 10px;
  }
  
  .dashboard-content {
    padding: 20px;
    margin: 0 10px;
  }
  
  .action-buttons {
    grid-template-columns: 1fr;
  }
  
  .credits-distribution {
    flex-direction: column;
    gap: 20px;
  }
  
  .weekly-stats {
    grid-template-columns: 1fr;
  }
}
</style>