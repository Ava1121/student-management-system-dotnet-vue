import axios from 'axios';

const api = axios.create({
  baseURL: 'http://localhost:5005/api',
  headers: {
    'Content-Type': 'application/json',
  },
});

// 添加请求拦截器，自动携带token
api.interceptors.request.use(config => {
  const token = localStorage.getItem('token');
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
}, error => {
  return Promise.reject(error);
});

// 添加响应拦截器，处理token过期等问题
api.interceptors.response.use(response => {
  return response;
}, error => {
  if (error.response?.status === 401) {
    // token过期或无效，清除本地存储并重定向到登录页
    localStorage.removeItem('token');
    localStorage.removeItem('username');
    localStorage.removeItem('role');
    window.location.href = '/login';
  }
  return Promise.reject(error);
});

export const studentService = {
  getAll: () => api.get('/Student'),
  getById: (id) => api.get(`/Student/${id}`),
  getProfile: () => api.get('/Student/profile'), // 获取当前登录学生的个人信息
  create: (student) => api.post('/Student', student),
  update: (id, student) => {
    console.log('调用API更新学生:', id, student);
    return api.put(`/Student/${id}`, student);
  },
  delete: (id) => api.delete(`/Student/${id}`),
};

export const courseService = {
  getAll: () => api.get('/Course'),
  getById: (id) => api.get(`/Course/${id}`),
  getMyCourses: () => api.get('/Course/my-courses'), // 获取当前学生的课程
  create: (course) => api.post('/Course', course),
  update: (id, course) => api.put(`/Course/${id}`, course),
  delete: (id) => api.delete(`/Course/${id}`),
};

export const enrollmentService = {
  getAll: () => api.get('/Enrollment'),
  getById: (id) => api.get(`/Enrollment/${id}`),
  getByStudentId: (studentId) => api.get(`/Enrollment/student/${studentId}`),
  getMyGrades: () => api.get('/Enrollment/my-grades'), // 获取当前学生的成绩
  create: (enrollment) => api.post('/Enrollment', enrollment),
  update: (id, enrollment) => api.put(`/Enrollment/${id}`, enrollment),
  delete: (id) => api.delete(`/Enrollment/${id}`),
};

export default api;