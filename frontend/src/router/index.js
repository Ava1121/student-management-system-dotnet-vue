import { createRouter, createWebHistory } from 'vue-router';
import StudentManagement from '../components/StudentManagement.vue';
import CourseManagement from '../components/CourseManagement.vue';
import EnrollmentManagement from '../components/EnrollmentManagement.vue';
import StudentProfile from '../components/StudentProfile.vue';
import Login from '../components/Login.vue';
import Dashboard from '../components/Dashboard.vue';

const routes = [
  {
    path: '/login',
    name: 'Login',
    component: Login,
    meta: { requiresAuth: false }
  },
  {
    path: '/register',
    name: 'Register',
    component: () => import('../components/Register.vue'),
    meta: { requiresAuth: false }
  },
  // 公共认证路由
  {
    path: '/change-password',
    name: 'ChangePassword',
    component: () => import('../components/ChangePassword.vue'),
    meta: { requiresAuth: true }
  },
  // 管理员路由
  {
    path: '/',
    name: 'Dashboard',
    component: Dashboard,
    meta: { requiresAuth: true, roles: ['Admin'] }
  },
  {
    path: '/students',
    name: 'StudentManagement',
    component: StudentManagement,
    meta: { requiresAuth: true, roles: ['Admin'] }
  },
  {
    path: '/courses',
    name: 'CourseManagement',
    component: CourseManagement,
    meta: { requiresAuth: true, roles: ['Admin'] }
  },
  {
    path: '/enrollments',
    name: 'EnrollmentManagement',
    component: EnrollmentManagement,
    meta: { requiresAuth: true, roles: ['Admin'] }
  },
  {
    path: '/logs',
    name: 'LogViewer',
    component: () => import('../components/LogViewer.vue'),
    meta: { requiresAuth: true, roles: ['Admin'] }
  },
  // 学生路由
  {
    path: '/profile',
    name: 'StudentProfile',
    component: StudentProfile,
    meta: { requiresAuth: true, roles: ['Student'] }
  },
  {
    path: '/my-courses',
    name: 'MyCourses',
    component: () => import('../components/MyCourses.vue'),
    meta: { requiresAuth: true, roles: ['Student'] }
  },
  {
    path: '/my-grades',
    name: 'MyGrades',
    component: () => import('../components/MyGrades.vue'),
    meta: { requiresAuth: true, roles: ['Student'] }
  }
];

const router = createRouter({
  history: createWebHistory(),
  routes
});

// 路由守卫
router.beforeEach((to, from, next) => {
  const requiresAuth = to.matched.some(record => record.meta.requiresAuth);
  const isAuthenticated = localStorage.getItem('token') !== null;
  const userRole = localStorage.getItem('role');

  if (requiresAuth && !isAuthenticated) {
    // 需要认证但未登录，重定向到登录页
    next('/login');
  } else if (!requiresAuth && isAuthenticated && to.path === '/login') {
    // 已登录但访问登录页，根据角色重定向到对应首页
    if (userRole === 'Admin') {
      next('/');
    } else {
      next('/profile');
    }
  } else if (requiresAuth && isAuthenticated) {
    // 已认证，检查角色权限
    const allowedRoles = to.matched.some(record => record.meta.roles);
    if (allowedRoles) {
      // 检查用户是否有该路由所需的角色
      const hasRole = to.matched.some(record => {
        return record.meta.roles && record.meta.roles.includes(userRole);
      });
      
      if (!hasRole) {
        // 没有权限，根据角色重定向到对应首页
        if (userRole === 'Admin') {
          next('/');
        } else {
          next('/profile');
        }
      } else {
        next();
      }
    } else {
      next();
    }
  } else {
    // 其他情况，正常跳转
    next();
  }
});

export default router;