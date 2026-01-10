<template>
  <div class="log-viewer">
    <div class="log-content">
      <!-- 日志文件列表 -->
      <div class="log-files-panel">
        <div class="panel-header">
          <h3>日志文件列表</h3>
        </div>
        <div class="panel-body">
          <div v-if="logFiles.length === 0" class="empty-state">
            <div class="empty-icon"></div>
            <div class="empty-text">暂无日志文件</div>
          </div>
          <div v-else class="log-files-list">
            <div 
              v-for="file in logFiles" 
              :key="file.filename" 
              class="log-file-item"
              :class="{ active: selectedLogFile && selectedLogFile.filename === file.filename }"
              @click="selectLogFile(file)">
              <div class="file-info">
                <div class="file-name">{{ file.filename }}</div>
                <div class="file-date">{{ formatDate(file.lastWriteTime) }}</div>
              </div>
            </div>
          </div>
        </div>
      </div>
      
      <!-- 日志内容 -->
      <div class="log-content-panel">
        <div class="panel-header">
          <h3>📝 日志内容</h3>
          <div class="selected-file-info" v-if="selectedLogFile">
            <span>{{ selectedLogFile.filename }}</span>
          </div>
        </div>
        <div class="panel-body">
          <div v-if="loading" class="loading-state">
            <div class="loading-spinner"></div>
            <div class="loading-text">加载日志内容中...</div>
          </div>
          <div v-else-if="error" class="error-state">
            <div class="error-icon"></div>
            <div class="error-text">{{ error }}</div>
            <button class="retry-btn" @click="retryFetchContent">重试</button>
          </div>
          <div v-else-if="!selectedLogFile" class="empty-state">
            <div class="empty-icon"></div>
            <div class="empty-text">请从左侧选择一个日志文件查看内容</div>
          </div>
          <div v-else class="log-content-area">
            <pre class="log-pre">{{ logContent }}</pre>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { ref, onMounted } from 'vue';
import api from '../services/api';

export default {
  name: 'LogViewer',
  setup() {
    const logFiles = ref([]);
    const selectedLogFile = ref(null);
    const logContent = ref('');
    const loading = ref(false);
    const error = ref(null);
    
    // 获取日志文件列表
    const fetchLogFiles = async () => {
      try {
        error.value = null;
        const response = await api.get('/Log');
        logFiles.value = response.data.logs;
      } catch (err) {
        console.error('获取日志文件失败:', err);
        error.value = '获取日志文件失败，请重试';
      }
    };
    
    // 选择日志文件
    const selectLogFile = async (file) => {
      selectedLogFile.value = file;
      await fetchLogContent(file);
    };
    
    // 获取日志内容
    const fetchLogContent = async (file) => {
      loading.value = true;
      error.value = null;
      try {
        const response = await api.get(`/Log/${file.filename}`);
        logContent.value = response.data.content;
      } catch (err) {
        console.error('获取日志内容失败:', err);
        error.value = '获取日志内容失败，请重试';
        logContent.value = '';
      } finally {
        loading.value = false;
      }
    };
    
    // 重试获取日志内容
    const retryFetchContent = async () => {
      if (selectedLogFile.value) {
        await fetchLogContent(selectedLogFile.value);
      }
    };
    
    // 格式化日期
    const formatDate = (date) => {
      return new Date(date).toLocaleString();
    };
    
    // 组件挂载时获取日志文件列表
    onMounted(() => {
      fetchLogFiles();
    });
    
    return {
      logFiles,
      selectedLogFile,
      logContent,
      loading,
      error,
      selectLogFile,
      retryFetchContent,
      formatDate
    };
  }
};
</script>

<style scoped>
.log-viewer {
  width: 100%;
  padding: 0;
}

.log-content {
  display: flex;
  gap: 20px;
  margin-top: 0;
  min-height: 600px;
  padding: 0 20px;
  width: 100%;
}

/* 面板样式 */
.log-files-panel, .log-content-panel {
  background: #ffffff;
  border: 2px solid #e0e0e0;
  border-radius: 12px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
  overflow: hidden;
  transition: all 0.3s ease;
}

.log-files-panel:hover, .log-content-panel:hover {
  box-shadow: 0 4px 12px rgba(220, 38, 38, 0.2);
}

/* 左侧日志文件面板 */
.log-files-panel {
  flex: 1;
  max-width: 350px;
}

/* 右侧日志内容面板 */
.log-content-panel {
  flex: 3;
}

/* 面板头部 */
.panel-header {
  background: linear-gradient(135deg, #dc2626 0%, #ef4444 100%);
  color: #ffffff;
  padding: 15px 20px;
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.panel-header h3 {
  margin: 0;
  font-size: 16px;
  font-weight: 600;
}

.selected-file-info {
  font-size: 14px;
  opacity: 0.9;
  background: rgba(255, 255, 255, 0.2);
  padding: 4px 10px;
  border-radius: 12px;
}

/* 面板内容 */
.panel-body {
  padding: 20px;
  height: 500px;
  overflow-y: auto;
  background: #ffffff;
}

/* 空状态 */
.empty-state {
  text-align: center;
  padding: 60px 20px;
  color: #ef4444;
}

.empty-icon {
  font-size: 48px;
  margin-bottom: 15px;
  opacity: 0.5;
}

.empty-text {
  font-size: 16px;
  font-weight: 500;
}

/* 日志文件列表 */
.log-files-list {
  display: flex;
  flex-direction: column;
  gap: 10px;
}

.log-file-item {
  background: #ffffff;
  border: 2px solid #e0e0e0;
  border-radius: 8px;
  padding: 18px 20px;
  cursor: pointer;
  transition: all 0.3s ease;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
  color: #333333;
}

.log-file-item:hover {
  border-color: #ef4444;
  background: #fef2f2;
  color: #ef4444;
  box-shadow: 0 4px 8px rgba(220, 38, 38, 0.2);
  transform: translateX(5px);
}

.log-file-item.active {
  background: linear-gradient(135deg, #dc2626 0%, #ef4444 100%);
  color: #ffffff;
  border-color: #ef4444;
  box-shadow: 0 4px 12px rgba(220, 38, 38, 0.3);
}

.file-info {
  display: flex;
  flex-direction: column;
  gap: 8px;
}

.file-name {
  font-weight: 600;
  font-size: 16px;
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
}

.file-date {
  font-size: 14px;
  opacity: 0.8;
}

/* 日志内容区域 */
.log-content-area {
  height: 100%;
}

.log-pre {
  background: #000000;
  color: #ffffff;
  padding: 25px;
  border-radius: 8px;
  font-family: 'Fira Code', 'Monaco', 'Consolas', monospace;
  font-size: 14px;
  line-height: 1.6;
  white-space: pre-wrap;
  word-wrap: break-word;
  height: 100%;
  overflow-y: auto;
  box-shadow: inset 0 2px 10px rgba(255, 255, 255, 0.1);
  border: 2px solid #e0e0e0;
}

/* 加载状态 */
.loading-state {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  height: 100%;
  gap: 15px;
  color: #dc2626;
}

.loading-spinner {
  width: 50px;
  height: 50px;
  border: 4px solid rgba(220, 38, 38, 0.1);
  border-top-color: #dc2626;
  border-radius: 50%;
  animation: spin 1s linear infinite;
}

@keyframes spin {
  to { transform: rotate(360deg); }
}

.loading-text {
  font-size: 16px;
  font-weight: 500;
}

/* 错误状态 */
.error-state {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  height: 100%;
  gap: 15px;
  color: #dc2626;
  text-align: center;
}

.error-icon {
  font-size: 48px;
  opacity: 0.8;
}

.error-text {
  font-size: 16px;
  font-weight: 500;
  max-width: 400px;
}

.retry-btn {
  background: linear-gradient(135deg, #dc2626 0%, #ef4444 100%);
  color: #ffffff;
  border: none;
  padding: 12px 24px;
  border-radius: 8px;
  cursor: pointer;
  transition: all 0.3s ease;
  font-weight: 600;
  box-shadow: 0 4px 15px rgba(220, 38, 38, 0.3);
}

.retry-btn:hover {
  background: linear-gradient(135deg, #b91c1c 0%, #dc2626 100%);
  color: #ffffff;
  transform: translateY(-2px);
  box-shadow: 0 6px 20px rgba(220, 38, 38, 0.4);
}

/* 滚动条样式 */
.panel-body::-webkit-scrollbar, .log-pre::-webkit-scrollbar {
  width: 8px;
}

.panel-body::-webkit-scrollbar-track, .log-pre::-webkit-scrollbar-track {
  background: rgba(220, 38, 38, 0.1);
  border-radius: 4px;
}

.panel-body::-webkit-scrollbar-thumb, .log-pre::-webkit-scrollbar-thumb {
  background: #ef4444;
  border-radius: 4px;
}

.panel-body::-webkit-scrollbar-thumb:hover, .log-pre::-webkit-scrollbar-thumb:hover {
  background: #fca5a5;
}
</style>