<template>
  <div class="page-container forgot-password-page">
    <!-- Background Elements -->
    <div class="background-elements">
      <div class="bg-circle circle-1"></div>
      <div class="bg-circle circle-2"></div>
      <div class="bg-gradient"></div>
    </div>

    <div class="forgot-password-container">
      <!-- Header Section -->
      <div class="header-section">
        <router-link to="/login" class="back-button">
          <font-awesome-icon icon="arrow-left" />
        </router-link>

        <div class="logo-section">
          <div class="logo-small">
            <span>AS</span>
            <div class="logo-pulse"></div>
          </div>
        </div>
      </div>

      <!-- Content Section -->
      <div class="content-section">
        <!-- Step 1: Email Input -->
        <div
          v-if="currentStep === 'email'"
          class="step-container"
          key="email-step"
        >
          <div class="icon-container">
            <div class="main-icon">
              <font-awesome-icon icon="key" />
            </div>
          </div>

          <div class="text-section">
            <h1 class="main-title">Forgot Password?</h1>
            <p class="subtitle">
              Don't worry! Enter your email address and we'll send you a reset
              link.
            </p>
          </div>

          <form class="form-container" @submit.prevent="sendResetEmail">
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
                  placeholder="Enter your email address"
                  required
                  :class="{ error: error && !email }"
                />
                <div class="input-focus-line"></div>
              </div>
            </div>

            <div v-if="error" class="error-message">
              <font-awesome-icon icon="exclamation-circle" />
              <span>{{ error }}</span>
            </div>

            <button
              type="submit"
              class="btn-primary"
              :disabled="loading || !email"
            >
              <span v-if="!loading" class="btn-content">
                <span>Send Reset Link</span>
                <font-awesome-icon icon="paper-plane" />
              </span>
              <span v-else class="btn-loading">
                <font-awesome-icon icon="spinner" class="fa-spin" />
                Sending...
              </span>
            </button>
          </form>
        </div>

        <!-- Step 2: Email Sent Confirmation -->
        <div
          v-if="currentStep === 'sent'"
          class="step-container"
          key="sent-step"
        >
          <div class="icon-container success">
            <div class="main-icon">
              <font-awesome-icon icon="envelope-open" />
            </div>
            <div class="success-checkmark">
              <font-awesome-icon icon="check" />
            </div>
          </div>

          <div class="text-section">
            <h1 class="main-title">Check Your Email</h1>
            <p class="subtitle">
              We've sent a password reset link to<br />
              <strong>{{ email }}</strong>
            </p>
          </div>

          <div class="info-cards">
            <div class="info-card">
              <font-awesome-icon icon="clock" class="card-icon" />
              <div class="card-content">
                <h4>Link expires in 15 minutes</h4>
                <p>Make sure to reset your password soon</p>
              </div>
            </div>

            <div class="info-card">
              <font-awesome-icon icon="spam" class="card-icon" />
              <div class="card-content">
                <h4>Check your spam folder</h4>
                <p>Sometimes emails end up there</p>
              </div>
            </div>
          </div>

          <div class="action-buttons">
            <button @click="openEmailApp" class="btn-primary">
              <font-awesome-icon icon="external-link-alt" />
              Open Email App
            </button>

            <button
              @click="resendEmail"
              class="btn-secondary"
              :disabled="resendCooldown > 0"
            >
              <span v-if="resendCooldown === 0">
                <font-awesome-icon icon="redo" />
                Resend Email
              </span>
              <span v-else> Resend in {{ resendCooldown }}s </span>
            </button>
          </div>
        </div>

        <!-- Step 3: Reset Password Form -->
        <div
          v-if="currentStep === 'reset'"
          class="step-container"
          key="reset-step"
        >
          <div class="icon-container">
            <div class="main-icon">
              <font-awesome-icon icon="lock" />
            </div>
          </div>

          <div class="text-section">
            <h1 class="main-title">Reset Password</h1>
            <p class="subtitle">Enter your new password below</p>
          </div>

          <form class="form-container" @submit.prevent="resetPassword">
            <div class="form-group">
              <label for="newPassword" class="input-label">
                <font-awesome-icon icon="lock" class="label-icon" />
                New Password
              </label>
              <div class="input-wrapper">
                <input
                  :type="showPassword ? 'text' : 'password'"
                  id="newPassword"
                  class="input-field"
                  v-model="newPassword"
                  placeholder="Enter new password"
                  required
                  :class="{ error: error && !newPassword }"
                />
                <button
                  type="button"
                  class="password-toggle"
                  @click="showPassword = !showPassword"
                >
                  <font-awesome-icon
                    :icon="showPassword ? 'eye-slash' : 'eye'"
                  />
                </button>
                <div class="input-focus-line"></div>
              </div>
              <div class="password-strength">
                <div class="strength-bar">
                  <div
                    class="strength-fill"
                    :class="passwordStrength.class"
                    :style="{ width: passwordStrength.width + '%' }"
                  ></div>
                </div>
                <span class="strength-text">{{ passwordStrength.text }}</span>
              </div>
            </div>

            <div class="form-group">
              <label for="confirmPassword" class="input-label">
                <font-awesome-icon icon="lock" class="label-icon" />
                Confirm Password
              </label>
              <div class="input-wrapper">
                <input
                  type="password"
                  id="confirmPassword"
                  class="input-field"
                  v-model="confirmPassword"
                  placeholder="Confirm new password"
                  required
                  :class="{ error: error && !confirmPassword }"
                />
                <div class="input-focus-line"></div>
              </div>
            </div>

            <div v-if="error" class="error-message">
              <font-awesome-icon icon="exclamation-circle" />
              <span>{{ error }}</span>
            </div>

            <button
              type="submit"
              class="btn-primary"
              :disabled="loading || !canResetPassword"
            >
              <span v-if="!loading" class="btn-content">
                <span>Reset Password</span>
                <font-awesome-icon icon="check" />
              </span>
              <span v-else class="btn-loading">
                <font-awesome-icon icon="spinner" class="fa-spin" />
                Resetting...
              </span>
            </button>
          </form>
        </div>

        <!-- Step 4: Success -->
        <div
          v-if="currentStep === 'success'"
          class="step-container"
          key="success-step"
        >
          <div class="icon-container success">
            <div class="main-icon">
              <font-awesome-icon icon="check-circle" />
            </div>
            <div class="success-animation"></div>
          </div>

          <div class="text-section">
            <h1 class="main-title">Password Reset!</h1>
            <p class="subtitle">
              Your password has been successfully reset.<br />
              You can now sign in with your new password.
            </p>
          </div>

          <div class="action-buttons">
            <router-link to="/login" class="btn-primary full-width">
              <font-awesome-icon icon="sign-in-alt" />
              Sign In Now
            </router-link>
          </div>
        </div>
      </div>

      <!-- Footer -->
      <div class="footer-section">
        <p>
          Remember your password?
          <router-link to="/login" class="login-link">Sign in here</router-link>
        </p>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from "vue";
import { useRoute, useRouter } from "vue-router";
import axios from "axios";

const route = useRoute();
const router = useRouter();

// Form data
const email = ref("");
const newPassword = ref("");
const confirmPassword = ref("");
const error = ref("");
const loading = ref(false);
const showPassword = ref(false);

// State management
const currentStep = ref("email"); // email, sent, reset, success
const resetToken = ref("");
const resendCooldown = ref(0);

// Computed properties
const passwordStrength = computed(() => {
  const pwd = newPassword.value;
  if (!pwd) return { width: 0, class: "", text: "" };

  let score = 0;
  if (pwd.length >= 8) score++;
  if (/[A-Z]/.test(pwd)) score++;
  if (/[a-z]/.test(pwd)) score++;
  if (/[0-9]/.test(pwd)) score++;
  if (/[^A-Za-z0-9]/.test(pwd)) score++;

  if (score < 2) return { width: 20, class: "weak", text: "Weak" };
  if (score < 4) return { width: 60, class: "medium", text: "Medium" };
  return { width: 100, class: "strong", text: "Strong" };
});

const canResetPassword = computed(() => {
  return (
    newPassword.value.trim() &&
    confirmPassword.value.trim() &&
    newPassword.value === confirmPassword.value &&
    passwordStrength.value.class !== "weak"
  );
});

// Methods
const sendResetEmail = async () => {
  error.value = "";
  loading.value = true;

  try {
    await axios.post(
      "https://asoms-production.up.railway.app/api/auth/forgot-password",
      {
        email: email.value,
      }
    );

    currentStep.value = "sent";
    startResendCooldown();
  } catch (err) {
    error.value =
      err.response?.data?.message ||
      "Failed to send reset email. Please try again.";
  } finally {
    loading.value = false;
  }
};

const resendEmail = async () => {
  if (resendCooldown.value > 0) return;

  error.value = "";
  loading.value = true;

  try {
    await axios.post(
      "https://asoms-production.up.railway.app/api/auth/forgot-password",
      {
        email: email.value,
      }
    );

    startResendCooldown();
  } catch (err) {
    error.value =
      err.response?.data?.message ||
      "Failed to resend email. Please try again.";
  } finally {
    loading.value = false;
  }
};

const resetPassword = async () => {
  error.value = "";

  if (newPassword.value !== confirmPassword.value) {
    error.value = "Passwords do not match.";
    return;
  }

  if (passwordStrength.value.class === "weak") {
    error.value = "Please choose a stronger password.";
    return;
  }

  loading.value = true;

  try {
    await axios.post(
      "https://asoms-production.up.railway.app/api/auth/reset-password",
      {
        token: resetToken.value,
        newPassword: newPassword.value,
      }
    );

    currentStep.value = "success";
  } catch (err) {
    error.value =
      err.response?.data?.message ||
      "Failed to reset password. Please try again.";
  } finally {
    loading.value = false;
  }
};

const openEmailApp = () => {
  // Try to open default email app
  window.location.href = "mailto:";
};

const startResendCooldown = () => {
  resendCooldown.value = 60;
  const interval = setInterval(() => {
    resendCooldown.value--;
    if (resendCooldown.value <= 0) {
      clearInterval(interval);
    }
  }, 1000);
};

// Check for reset token in URL
const token = route.query.token;
if (token) {
  resetToken.value = token;
  currentStep.value = "reset";
}
</script>

<style scoped>
.forgot-password-page {
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
.forgot-password-container {
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

/* Content Section */
.content-section {
  flex: 1;
  display: flex;
  flex-direction: column;
  justify-content: center;
}

.step-container {
  animation: fadeInUp 0.6s ease-out;
  text-align: center;
}

@keyframes fadeInUp {
  from {
    opacity: 0;
    transform: translateY(30px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

/* Icon Container */
.icon-container {
  margin-bottom: 30px;
  position: relative;
  display: flex;
  justify-content: center;
}

.main-icon {
  width: 80px;
  height: 80px;
  background: linear-gradient(135deg, var(--primary-color) 0%, #ff6b47 100%);
  border-radius: 20px;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 32px;
  color: white;
  box-shadow: 0 15px 35px rgba(255, 75, 38, 0.3);
  animation: iconFloat 3s ease-in-out infinite;
}

@keyframes iconFloat {
  0%,
  100% {
    transform: translateY(0px);
  }
  50% {
    transform: translateY(-5px);
  }
}

.icon-container.success .main-icon {
  background: linear-gradient(135deg, #22c55e 0%, #16a34a 100%);
  box-shadow: 0 15px 35px rgba(34, 197, 94, 0.3);
}

.success-checkmark {
  position: absolute;
  top: -5px;
  right: -5px;
  width: 30px;
  height: 30px;
  background: #22c55e;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  color: white;
  font-size: 14px;
  animation: checkmarkPop 0.5s ease-out 0.3s both;
}

@keyframes checkmarkPop {
  0% {
    transform: scale(0);
  }
  80% {
    transform: scale(1.2);
  }
  100% {
    transform: scale(1);
  }
}

.success-animation {
  position: absolute;
  top: -10px;
  left: -10px;
  right: -10px;
  bottom: -10px;
  border: 2px solid #22c55e;
  border-radius: 25px;
  animation: successPulse 2s ease-in-out infinite;
}

@keyframes successPulse {
  0%,
  100% {
    opacity: 0.3;
    transform: scale(1);
  }
  50% {
    opacity: 0.6;
    transform: scale(1.05);
  }
}

/* Text Section */
.text-section {
  margin-bottom: 40px;
}

.main-title {
  font-size: 32px;
  font-weight: 800;
  color: white;
  margin-bottom: 12px;
  line-height: 1.2;
}

.subtitle {
  color: #888;
  font-size: 16px;
  line-height: 1.6;
  max-width: 320px;
  margin: 0 auto;
}

/* Form Container */
.form-container {
  margin-bottom: 30px;
}

.form-group {
  margin-bottom: 24px;
  text-align: left;
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

/* Password Strength */
.password-strength {
  margin-top: 8px;
  display: flex;
  align-items: center;
  gap: 12px;
}

.strength-bar {
  flex: 1;
  height: 4px;
  background: rgba(255, 255, 255, 0.1);
  border-radius: 2px;
  overflow: hidden;
}

.strength-fill {
  height: 100%;
  border-radius: 2px;
  transition: all 0.3s ease;
}

.strength-fill.weak {
  background: #ff4444;
}
.strength-fill.medium {
  background: #ffaa00;
}
.strength-fill.strong {
  background: #00aa44;
}

.strength-text {
  font-size: 12px;
  font-weight: 500;
  color: #888;
}

/* Info Cards */
.info-cards {
  display: flex;
  flex-direction: column;
  gap: 16px;
  margin-bottom: 30px;
}

.info-card {
  display: flex;
  align-items: center;
  gap: 16px;
  padding: 16px;
  background: rgba(255, 255, 255, 0.05);
  border: 1px solid rgba(255, 255, 255, 0.1);
  border-radius: 12px;
  text-align: left;
  backdrop-filter: blur(10px);
}

.card-icon {
  font-size: 20px;
  color: var(--primary-color);
  flex-shrink: 0;
}

.card-content h4 {
  font-size: 14px;
  font-weight: 600;
  color: white;
  margin-bottom: 4px;
}

.card-content p {
  font-size: 12px;
  color: #888;
  margin: 0;
}

/* Buttons */
.action-buttons {
  display: flex;
  flex-direction: column;
  gap: 12px;
}

.btn-primary,
.btn-secondary {
  width: 100%;
  padding: 16px 20px;
  border-radius: 12px;
  font-size: 16px;
  font-weight: 600;
  cursor: pointer;
  transition: all 0.3s ease;
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 8px;
  border: none;
  text-decoration: none;
}

.btn-primary {
  background: linear-gradient(135deg, var(--primary-color) 0%, #ff6b47 100%);
  color: white;
  box-shadow: 0 8px 25px rgba(255, 75, 38, 0.3);
}

.btn-primary:hover:not(:disabled) {
  transform: translateY(-2px);
  box-shadow: 0 12px 35px rgba(255, 75, 38, 0.4);
}

.btn-primary:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}

.btn-secondary {
  background: rgba(255, 255, 255, 0.1);
  color: white;
  border: 2px solid rgba(255, 255, 255, 0.2);
}

.btn-secondary:hover:not(:disabled) {
  background: rgba(255, 255, 255, 0.15);
  transform: translateY(-1px);
}

.btn-secondary:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}

.btn-content,
.btn-loading {
  display: flex;
  align-items: center;
  gap: 8px;
}

.full-width {
  width: 100%;
}

/* Error Message */
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
  margin-bottom: 20px;
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

/* Footer */
.footer-section {
  text-align: center;
  padding-bottom: 20px;
  margin-top: auto;
}

.footer-section p {
  color: #888;
  font-size: 14px;
}

.login-link {
  color: var(--primary-color);
  text-decoration: none;
  font-weight: 600;
  transition: opacity 0.3s ease;
}

.login-link:hover {
  opacity: 0.8;
}

/* Responsive Design */
@media (max-width: 480px) {
  .forgot-password-container {
    padding: 15px;
  }

  .main-title {
    font-size: 28px;
  }

  .main-icon {
    width: 70px;
    height: 70px;
    font-size: 28px;
  }

  .info-cards {
    gap: 12px;
  }

  .info-card {
    padding: 12px;
  }
}
</style>
