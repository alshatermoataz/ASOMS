<template>
  <div class="page-container login-page">
    <!-- Background Elements -->
    <div class="background-elements">
      <div class="bg-circle circle-1"></div>
      <div class="bg-circle circle-2"></div>
      <div class="bg-gradient"></div>
    </div>

    <div class="login-container">
      <!-- Header Section -->
      <div class="header-section">
        <router-link to="/" class="back-button">
          <font-awesome-icon icon="arrow-left" />
        </router-link>

        <div class="logo-section">
          <div class="logo-small">
            <span>AS</span>
            <div class="logo-pulse"></div>
          </div>
          <div class="brand-text">ASOMS</div>
        </div>
      </div>

      <!-- Welcome Section -->
      <div class="welcome-section">
        <h1 class="welcome-text">
          <span class="welcome-line">Welcome</span>
          <span class="welcome-line highlight">Back!</span>
        </h1>
        <p class="welcome-subtitle">
          Sign in to continue your automotive journey
        </p>
      </div>

      <!-- Form Section -->
      <div class="form-section">
        <form class="login-form" @submit.prevent="handleLogin">
          <div class="form-group">
            <label for="email" class="input-label">
              <font-awesome-icon icon="envelope" class="label-icon" />
              Email Address
            </label>
            <div class="input-wrapper">
              <input
                type="email"
                id="email"
                class="input-field"
                v-model="email"
                placeholder="Enter your email"
                required
                :class="{ error: error && !email }"
              />
              <div class="input-focus-line"></div>
            </div>
          </div>

          <div class="form-group">
            <label for="password" class="input-label">
              <font-awesome-icon icon="lock" class="label-icon" />
              Password
            </label>
            <div class="input-wrapper">
              <input
                :type="showPassword ? 'text' : 'password'"
                id="password"
                class="input-field"
                v-model="password"
                placeholder="Enter your password"
                required
                :class="{ error: error && !password }"
              />
              <button
                type="button"
                class="password-toggle"
                @click="showPassword = !showPassword"
              >
                <font-awesome-icon :icon="showPassword ? 'eye-slash' : 'eye'" />
              </button>
              <div class="input-focus-line"></div>
            </div>
          </div>

          <div class="form-options">
            <label class="remember-me">
              <input type="checkbox" v-model="rememberMe" />
              <span class="checkmark"></span>
              Remember me
            </label>
            <a href="#" class="forgot-password">Forgot Password?</a>
          </div>

          <div v-if="error" class="error-message">
            <font-awesome-icon icon="exclamation-circle" />
            <span>{{ error }}</span>
          </div>

          <button type="submit" class="btn-primary" :disabled="loading">
            <span v-if="!loading" class="btn-content">
              <span>Sign In</span>
              <font-awesome-icon icon="arrow-right" class="btn-icon" />
            </span>
            <span v-else class="btn-loading">
              <font-awesome-icon icon="spinner" class="fa-spin" />
              Signing in...
            </span>
          </button>
        </form>

        <!-- Social Login -->
        <!-- <div class="social-section">
          <div class="divider">
            <span>or continue with</span>
          </div>

          <div class="social-buttons">
            <button class="social-btn google-btn">
              <font-awesome-icon :icon="['fab', 'google']" />
              Google
            </button>
            <button class="social-btn facebook-btn">
              <font-awesome-icon :icon="['fab', 'facebook-f']" />
              Facebook
            </button>
          </div>
        </div> -->

        <!-- Sign Up Link -->
        <div class="signup-section">
          <p>
            Don't have an account?
            <router-link to="/signup" class="signup-link"
              >Sign up here</router-link
            >
          </p>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref } from "vue";
import { useRouter } from "vue-router";
import { useAuthStore } from "../stores/auth";
import axios from "axios";

const email = ref("");
const password = ref("");
const error = ref("");
const loading = ref(false);
const showPassword = ref(false);
const rememberMe = ref(false);
const router = useRouter();
const auth = useAuthStore();

const handleLogin = async () => {
  error.value = "";
  loading.value = true;

  try {
    const response = await axios.post(
      "https://asoms-production.up.railway.app/api/auth/login",
      {
        email: email.value,
        password: password.value,
      }
    );

    const { token, user } = response.data;
    auth.login(user, token);

    // Add success animation before redirect
    setTimeout(() => {
      router.push("/home");
    }, 500);
  } catch (err) {
    error.value =
      err.response?.data?.message ||
      "Login failed. Please check your credentials.";
  } finally {
    loading.value = false;
  }
};
</script>

<style scoped>
.login-page {
  min-height: 100vh;
  position: relative;
  background: linear-gradient(135deg, #0a0a0a 0%, #1a1a1a 100%);
  overflow: hidden;
}

/* Background Elements */
.background-elements {
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  pointer-events: none;
}

.bg-circle {
  position: absolute;
  border-radius: 50%;
  background: linear-gradient(
    45deg,
    rgba(255, 75, 38, 0.1),
    rgba(255, 107, 71, 0.05)
  );
  animation: float 8s ease-in-out infinite;
}

.circle-1 {
  width: 200px;
  height: 200px;
  top: -50px;
  right: -50px;
  animation-delay: 0s;
}

.circle-2 {
  width: 150px;
  height: 150px;
  bottom: -30px;
  left: -30px;
  animation-delay: 4s;
}

.bg-gradient {
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background: radial-gradient(
    circle at 30% 20%,
    rgba(255, 75, 38, 0.05) 0%,
    transparent 50%
  );
}

@keyframes float {
  0%,
  100% {
    transform: translateY(0px) rotate(0deg);
  }
  50% {
    transform: translateY(-20px) rotate(180deg);
  }
}

/* Main Container */
.login-container {
  position: relative;
  z-index: 1;
  padding: 20px;
  display: flex;
  flex-direction: column;
  min-height: 100vh;
  max-width: 400px;
  margin: 0 auto;
}

/* Header Section */
.header-section {
  display: flex;
  align-items: center;
  justify-content: space-between;
  margin-bottom: 40px;
  padding-top: 20px;
}

.back-button {
  width: 44px;
  height: 44px;
  border-radius: 12px;
  background: rgba(255, 255, 255, 0.1);
  border: 1px solid rgba(255, 255, 255, 0.1);
  display: flex;
  align-items: center;
  justify-content: center;
  color: white;
  text-decoration: none;
  transition: all 0.3s ease;
  backdrop-filter: blur(10px);
}

.back-button:hover {
  background: rgba(255, 255, 255, 0.15);
  transform: translateX(-2px);
}

.logo-section {
  display: flex;
  align-items: center;
  gap: 12px;
}

.logo-small {
  width: 50px;
  height: 50px;
  background: linear-gradient(135deg, var(--primary-color) 0%, #ff6b47 100%);
  border-radius: 15px;
  display: flex;
  align-items: center;
  justify-content: center;
  font-weight: 800;
  font-size: 18px;
  color: white;
  position: relative;
  box-shadow: 0 8px 20px rgba(255, 75, 38, 0.3);
}

.logo-pulse {
  position: absolute;
  top: -2px;
  left: -2px;
  right: -2px;
  bottom: -2px;
  background: linear-gradient(
    135deg,
    rgba(255, 75, 38, 0.4),
    rgba(255, 107, 71, 0.2)
  );
  border-radius: 17px;
  animation: pulse 2s ease-in-out infinite;
  z-index: -1;
}

@keyframes pulse {
  0%,
  100% {
    opacity: 0.5;
    transform: scale(1);
  }
  50% {
    opacity: 0.8;
    transform: scale(1.05);
  }
}

.brand-text {
  font-size: 20px;
  font-weight: 700;
  color: white;
  letter-spacing: 1px;
}

/* Welcome Section */
.welcome-section {
  margin-bottom: 40px;
  text-align: left;
}

.welcome-text {
  font-size: 36px;
  font-weight: 800;
  line-height: 1.2;
  margin-bottom: 12px;
  display: flex;
  flex-direction: column;
}

.welcome-line {
  color: white;
  animation: slideInLeft 0.8s ease-out forwards;
  opacity: 0;
}

.welcome-line:nth-child(1) {
  animation-delay: 0.2s;
}
.welcome-line:nth-child(2) {
  animation-delay: 0.4s;
}

.highlight {
  background: linear-gradient(135deg, var(--primary-color) 0%, #ff6b47 100%);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
  background-clip: text;
}

.welcome-subtitle {
  color: #888;
  font-size: 16px;
  line-height: 1.5;
  animation: fadeInUp 0.8s ease-out 0.6s forwards;
  opacity: 0;
}

@keyframes slideInLeft {
  from {
    opacity: 0;
    transform: translateX(-30px);
  }
  to {
    opacity: 1;
    transform: translateX(0);
  }
}

@keyframes fadeInUp {
  from {
    opacity: 0;
    transform: translateY(20px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

/* Form Section */
.form-section {
  flex: 1;
  display: flex;
  flex-direction: column;
}

.login-form {
  display: flex;
  flex-direction: column;
  gap: 24px;
  margin-bottom: 30px;
}

.form-group {
  display: flex;
  flex-direction: column;
  gap: 8px;
}

.input-label {
  display: flex;
  align-items: center;
  gap: 8px;
  font-size: 14px;
  font-weight: 600;
  color: #ccc;
  margin-bottom: 8px;
}

.label-icon {
  font-size: 16px;
  color: var(--primary-color);
}

.input-wrapper {
  position: relative;
}

.input-field {
  width: 100%;
  padding: 16px 20px;
  background: rgba(255, 255, 255, 0.05);
  border: 2px solid rgba(255, 255, 255, 0.1);
  border-radius: 12px;
  color: white;
  font-size: 16px;
  transition: all 0.3s ease;
  backdrop-filter: blur(10px);
}

.input-field:focus {
  outline: none;
  border-color: var(--primary-color);
  background: rgba(255, 255, 255, 0.08);
  transform: translateY(-1px);
}

.input-field.error {
  border-color: #ff4444;
  background: rgba(255, 68, 68, 0.1);
}

.input-field::placeholder {
  color: #666;
}

.input-focus-line {
  position: absolute;
  bottom: 0;
  left: 0;
  width: 0;
  height: 2px;
  background: linear-gradient(90deg, var(--primary-color), #ff6b47);
  transition: width 0.3s ease;
}

.input-field:focus + .password-toggle + .input-focus-line,
.input-field:focus + .input-focus-line {
  width: 100%;
}

.password-toggle {
  position: absolute;
  right: 16px;
  top: 50%;
  transform: translateY(-50%);
  background: none;
  border: none;
  color: #666;
  cursor: pointer;
  padding: 4px;
  transition: color 0.3s ease;
}

.password-toggle:hover {
  color: var(--primary-color);
}

.form-options {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin: 8px 0;
}

.remember-me {
  display: flex;
  align-items: center;
  gap: 8px;
  font-size: 14px;
  color: #ccc;
  cursor: pointer;
  position: relative;
}

.remember-me input {
  display: none;
}

.checkmark {
  width: 18px;
  height: 18px;
  border: 2px solid rgba(255, 255, 255, 0.3);
  border-radius: 4px;
  position: relative;
  transition: all 0.3s ease;
}

.remember-me input:checked + .checkmark {
  background: var(--primary-color);
  border-color: var(--primary-color);
}

.remember-me input:checked + .checkmark::after {
  content: "✓";
  position: absolute;
  top: -2px;
  left: 2px;
  color: white;
  font-size: 12px;
  font-weight: bold;
}

.forgot-password {
  color: var(--primary-color);
  text-decoration: none;
  font-size: 14px;
  font-weight: 500;
  transition: opacity 0.3s ease;
}

.forgot-password:hover {
  opacity: 0.8;
}

.error-message {
  display: flex;
  align-items: center;
  gap: 8px;
  padding: 12px 16px;
  background: rgba(255, 68, 68, 0.1);
  border: 1px solid rgba(255, 68, 68, 0.3);
  border-radius: 8px;
  color: #ff6b6b;
  font-size: 14px;
  animation: shake 0.5s ease-in-out;
}

@keyframes shake {
  0%,
  100% {
    transform: translateX(0);
  }
  25% {
    transform: translateX(-5px);
  }
  75% {
    transform: translateX(5px);
  }
}

.btn-primary {
  width: 100%;
  padding: 18px 24px;
  background: linear-gradient(135deg, var(--primary-color) 0%, #ff6b47 100%);
  border: none;
  border-radius: 12px;
  color: white;
  font-size: 16px;
  font-weight: 600;
  cursor: pointer;
  transition: all 0.3s ease;
  position: relative;
  overflow: hidden;
  box-shadow: 0 8px 25px rgba(255, 75, 38, 0.3);
}

.btn-primary:hover:not(:disabled) {
  transform: translateY(-2px);
  box-shadow: 0 12px 35px rgba(255, 75, 38, 0.4);
}

.btn-primary:disabled {
  opacity: 0.7;
  cursor: not-allowed;
}

.btn-content,
.btn-loading {
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 8px;
}

.btn-icon {
  transition: transform 0.3s ease;
}

.btn-primary:hover .btn-icon {
  transform: translateX(4px);
}

/* Social Section */
.social-section {
  margin-bottom: 30px;
}

.divider {
  text-align: center;
  position: relative;
  margin: 24px 0;
}

.divider span {
  background: var(--background-color);
  padding: 0 16px;
  color: #666;
  font-size: 14px;
  position: relative;
  z-index: 2;
}

.divider::before {
  content: "";
  position: absolute;
  top: 50%;
  left: 0;
  right: 0;
  height: 1px;
  background: linear-gradient(
    90deg,
    transparent,
    rgba(255, 255, 255, 0.2),
    transparent
  );
}

.social-buttons {
  display: flex;
  gap: 12px;
}

.social-btn {
  flex: 1;
  padding: 14px 20px;
  border: 2px solid rgba(255, 255, 255, 0.1);
  border-radius: 12px;
  background: rgba(255, 255, 255, 0.05);
  color: white;
  font-size: 14px;
  font-weight: 500;
  cursor: pointer;
  transition: all 0.3s ease;
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 8px;
  backdrop-filter: blur(10px);
}

.social-btn:hover {
  background: rgba(255, 255, 255, 0.1);
  border-color: rgba(255, 255, 255, 0.2);
  transform: translateY(-1px);
}

/* Sign Up Section */
.signup-section {
  text-align: center;
  margin-top: auto;
  padding-bottom: 20px;
}

.signup-section p {
  color: #888;
  font-size: 14px;
}

.signup-link {
  color: var(--primary-color);
  text-decoration: none;
  font-weight: 600;
  transition: opacity 0.3s ease;
}

.signup-link:hover {
  opacity: 0.8;
}

/* Responsive Design */
@media (max-width: 480px) {
  .login-container {
    padding: 15px;
  }

  .welcome-text {
    font-size: 32px;
  }

  .social-buttons {
    flex-direction: column;
  }
}
</style>
