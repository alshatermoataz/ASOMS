import { createRouter, createWebHistory } from 'vue-router'
import SplashPage from '../views/SplashPage.vue'
import LoginPage from '../views/LoginPage.vue'
import SignupPage from '../views/SignupPage.vue'
import HomePage from '../views/HomePage.vue'
import ShopPage from '../views/ShopPage.vue'
import HistoryPage from '../views/HistoryPage.vue'
import ProductDetailPage from '../views/ProductDetailPage.vue'
import CheckoutPage from '../views/CheckoutPage.vue'
import UpdatePage from '../views/UpdatePage.vue'
import CheckPage from '../views/CheckPage.vue'
import OrdersPage from '../views/OrdersPage.vue'
import CustomersPage from '../views/CustomersPage.vue'
import ProductsPage from '../views/ProductsPage.vue'
import AnalyticsPage from '../views/AnalyticsPage.vue'
import SettingsPage from '../views/SettingsPage.vue'
import { useAuthStore } from '../stores/auth'
import ProfilePage from '../views/ProfilePage.vue'
import ForgotPassword from '../views/ForgotPasswordPage.vue'

const routes = [
  {
    path: '/',
    name: 'Splash',
    component: SplashPage
  },
  {
    path: '/login',
    name: 'Login',
    component: LoginPage
  },
  {
    path: '/signup',
    name: 'Signup',
    component: SignupPage
  },
  {
    path: '/home',
    name: 'Home',
    component: HomePage,
    meta: { requiresAuth: true, role: 'user' }
  },
  {
    path: '/shop',
    name: 'Shop',
    component: ShopPage,
    meta: { requiresAuth: true, role: 'user' }
  },
  {
    path: '/history',
    name: 'History',
    component: HistoryPage,
    meta: { requiresAuth: true, role: 'user' }
  },
  {
    path: '/product/:id',
    name: 'ProductDetail',
    component: ProductDetailPage,
    meta: { requiresAuth: true, role: 'user' }
  },
  {
    path: '/checkout',
    name: 'Checkout',
    component: CheckoutPage,
    meta: { requiresAuth: true, role: 'user' }
  },
  {
    path: '/update',
    name: 'Update',
    component: UpdatePage,
    meta: { requiresAuth: true, role: 'user' }
  },
  {
    path: '/profile',
    name: 'Profile',
    component: ProfilePage,
    meta: { requiresAuth: true, role: 'user' }
  },
  {
    path: '/forgotpassword',
    name: 'forgotpassword',
    component: ForgotPassword
  },
  // Admin routes
  {
    path: '/admin/check',
    name: 'Check',
    component: CheckPage,
    meta: { requiresAuth: true, role: 'admin' }
  },
  {
    path: '/admin/orders',
    name: 'Orders',
    component: OrdersPage,
    meta: { requiresAuth: true, role: 'admin' }
  },
  {
    path: '/admin/customers',
    name: 'Customers',
    component: CustomersPage,
    meta: { requiresAuth: true, role: 'admin' }
  },
  {
    path: '/admin/products',
    name: 'Products',
    component: ProductsPage,
    meta: { requiresAuth: true, role: 'admin' }
  },
  {
    path: '/admin/analytics',
    name: 'analytics',
    component: AnalyticsPage,
    meta: { requiresAuth: true, role: 'admin' }
  },
  {
    path: '/admin/settings',
    name: 'settings',
    component: SettingsPage,
    meta: { requiresAuth: true, role: 'admin' }
  }
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

router.beforeEach((to, from, next) => {
  const auth = useAuthStore()
  
  // Public routes that don't require login
  const publicPages = ['Splash', 'Login', 'Signup', 'forgotpassword']
  const authRequired = !publicPages.includes(to.name)
  
  // Check if authentication is required
  if (authRequired && !auth.token) {
    return next('/login')
  }
  
  // Role-based access control
  if (to.meta.requiresAuth && auth.token) {
    const userRole = auth.user?.role?.toLowerCase()
    const requiredRole = to.meta.role?.toLowerCase()
    
    // If route requires admin role but user is not admin
    if (requiredRole === 'admin' && userRole !== 'admin') {
      // Redirect non-admin users trying to access admin routes
      return next('/home')
    }
    
    // If route requires user role but user is admin, allow access
    // (Admins can access user routes)
    if (requiredRole === 'user' && userRole === 'admin') {
      return next()
    }
  }
  
  next()
})

export default router
