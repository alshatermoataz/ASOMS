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
    component: HomePage
  },
  {
    path: '/shop',
    name: 'Shop',
    component: ShopPage
  },
  {
    path: '/history',
    name: 'History',
    component: HistoryPage
  },
  {
    path: '/product/:id',
    name: 'ProductDetail',
    component: ProductDetailPage
  },
  {
    path: '/checkout',
    name: 'Checkout',
    component: CheckoutPage
  },
  {
    path: '/update',
    name: 'Update',
    component: UpdatePage
  },
  {
    path: '/admin/check',
    name: 'Check',
    component: CheckPage
  },
  {
    path: '/admin/orders',
    name: 'Orders',
    component: OrdersPage
  },
  {
    path: '/admin/customers',
    name: 'Customers',
    component: CustomersPage
  },
  {
    path: '/admin/products',
    name: 'Products',
    component: ProductsPage
  },
  {
    path: '/admin/analytics',
    name: 'analytics',
    component: AnalyticsPage
  },
  {
    path: '/admin/settings',
    name: 'settings',
    component: SettingsPage
  },
  {
    path: '/profile',
    name: 'Profile',
    component: ProfilePage
  },
  {
    path: '/forgotpassword',
    name: 'forgotpassword',
    component: ForgotPassword
  }
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

router.beforeEach((to, from, next) => {
  const auth = useAuthStore()

  // Public routes that don’t require login
  const publicPages = ['Splash', 'Login', 'Signup']
  const authRequired = !publicPages.includes(to.name)

  if (authRequired && !auth.token) {
    return next('/login')
  }

  next()
})


export default router