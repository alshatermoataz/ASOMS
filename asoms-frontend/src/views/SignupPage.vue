<template>
  <div class="page-container signup-page">
    <!-- Background Elements -->
    <div class="background-elements">
      <div class="bg-circle circle-1"></div>
      <div class="bg-circle circle-2"></div>
      <div class="bg-circle circle-3"></div>
      <div class="bg-gradient"></div>
    </div>

    <div class="signup-container">
      <!-- Header Section -->
      <div class="header-section">
        <router-link to="/" class="back-button">
          <font-awesome-icon icon="arrow-left" />
        </router-link>

        <div class="progress-indicator">
          <div class="progress-bar">
            <div
              class="progress-fill"
              :style="{ width: progressWidth + '%' }"
            ></div>
          </div>
          <span class="progress-text">{{ currentStep }}/{{ totalSteps }}</span>
        </div>
      </div>

      <!-- Welcome Section -->
      <div class="welcome-section">
        <h1 class="signup-title">
          <span class="title-line">Create an</span>
          <span class="title-line highlight">Account</span>
        </h1>
        <p class="signup-subtitle">Join thousands of automotive enthusiasts</p>
      </div>

      <!-- Form Section -->
      <div class="form-section">
        <form class="signup-form" @submit.prevent="handleSignup">
          <!-- Step 1: Personal Info -->
          <div v-if="currentStep === 1" class="form-step" key="step1">
            <div class="step-header">
              <h3>Personal Information</h3>
              <p>Tell us about yourself</p>
            </div>

            <div class="form-group">
              <label for="fullName" class="input-label">
                <font-awesome-icon icon="user" class="label-icon" />
                Full Name
              </label>
              <div class="input-wrapper">
                <input
                  type="text"
                  id="fullName"
                  class="input-field"
                  v-model="fullName"
                  placeholder="Enter your full name"
                  required
                  :class="{ error: error && !fullName }"
                />
                <div class="input-focus-line"></div>
              </div>
            </div>

            <div class="form-group">
              <label for="phone" class="input-label">
                <font-awesome-icon icon="phone" class="label-icon" />
                Phone Number
              </label>
              <div class="input-wrapper">
                <input
                  type="tel"
                  id="phone"
                  class="input-field"
                  v-model="phone"
                  placeholder="Enter your phone number"
                  required
                  :class="{ error: error && !phone }"
                />
                <div class="input-focus-line"></div>
              </div>
            </div>
          </div>

          <!-- Step 2: Account Info -->
          <div v-if="currentStep === 2" class="form-step" key="step2">
            <div class="step-header">
              <h3>Account Details</h3>
              <p>Secure your account</p>
            </div>

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
                  placeholder="Create a strong password"
                  required
                  :class="{ error: error && !password }"
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
                  placeholder="Confirm your password"
                  required
                  :class="{ error: error && !confirmPassword }"
                />
                <div class="input-focus-line"></div>
              </div>
            </div>
          </div>

          <div v-if="error" class="error-message">
            <font-awesome-icon icon="exclamation-circle" />
            <span>{{ error }}</span>
          </div>

          <!-- Form Navigation -->
          <div class="form-navigation">
            <button
              v-if="currentStep > 1"
              type="button"
              class="btn-secondary"
              @click="previousStep"
            >
              <font-awesome-icon icon="arrow-left" />
              Previous
            </button>

            <button
              v-if="currentStep < totalSteps"
              type="button"
              class="btn-primary"
              @click="nextStep"
              :disabled="!canProceed"
            >
              Next
              <font-awesome-icon icon="arrow-right" />
            </button>

            <button
              v-if="currentStep === totalSteps"
              type="submit"
              class="btn-primary"
              :disabled="loading || !canProceed"
            >
              <span v-if="!loading" class="btn-content">
                <span>Create Account</span>
                <font-awesome-icon icon="user-plus" />
              </span>
              <span v-else class="btn-loading">
                <font-awesome-icon icon="spinner" class="fa-spin" />
                Creating...
              </span>
            </button>
          </div>
        </form>

        <!-- Terms and Privacy -->
        <div class="terms-section">
          <label class="terms-checkbox">
            <input type="checkbox" v-model="agreeToTerms" />
            <span class="checkmark"></span>
            <span class="terms-text">
              I agree to the
              <a href="#" class="terms-link">Terms of Service</a>
              and
              <a href="#" class="terms-link">Privacy Policy</a>
            </span>
          </label>
        </div>

        <!-- Social Signup -->
        <!-- <div class="social-section">
          <div class="divider">
            <span>or sign up with</span>
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

        <!-- Login Link -->
        <div class="login-section">
          <p>
            Already have an account?
            <router-link to="/login" class="login-link"
              >Sign in here</router-link
            >
          </p>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, watch } from "vue";
import { useRouter } from "vue-router";
import axios from "axios";
import { useAuthStore } from "../stores/auth";

const router = useRouter();
const auth = useAuthStore();

// Form data
const fullName = ref("");
const phone = ref("");
const email = ref("");
const password = ref("");
const confirmPassword = ref("");
const error = ref("");
const loading = ref(false);
const showPassword = ref(false);
const agreeToTerms = ref(false);

// Multi-step form
const currentStep = ref(1);
const totalSteps = ref(2);

// Computed properties
const progressWidth = computed(
  () => (currentStep.value / totalSteps.value) * 100
);

const passwordStrength = computed(() => {
  const pwd = password.value;
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

const canProceed = computed(() => {
  if (currentStep.value === 1) {
    return fullName.value.trim() && phone.value.trim();
  }
  if (currentStep.value === 2) {
    return (
      email.value.trim() &&
      password.value.trim() &&
      confirmPassword.value.trim() &&
      password.value === confirmPassword.value &&
      agreeToTerms.value
    );
  }
  return false;
});

// Methods
const nextStep = () => {
  if (canProceed.value && currentStep.value < totalSteps.value) {
    currentStep.value++;
  }
};

const previousStep = () => {
  if (currentStep.value > 1) {
    currentStep.value--;
  }
};

const handleSignup = async () => {
  error.value = "";

  if (password.value !== confirmPassword.value) {
    error.value = "Passwords do not match.";
    return;
  }

  if (!agreeToTerms.value) {
    error.value = "Please agree to the terms and conditions.";
    return;
  }

  loading.value = true;

  try {
    console.log("Sending signup request...");

    const response = await axios.post(
      "https://asoms-production.up.railway.app/api/auth/register",
      {
        FullName: fullName.value,
        Email: email.value,
        Password: password.value,
        ContactNumber: phone.value,
        Role: "Customer",
      }
    );

    console.log("API Response:", response.data);

    // Handle the actual API response structure
    const { token, userId, email: userEmail, role } = response.data;

    // Create user object from the response data
    const user = {
      id: userId,
      userId: userId, // Some parts of the app might expect userId
      email: userEmail,
      fullName: fullName.value, // Use the form data since API doesn't return it
      contactNumber: phone.value, // Use the form data since API doesn't return it
      role: role,
    };

    console.log("Logging in user:", user);

    auth.login(user, token);

    // Add success animation before redirect
    setTimeout(() => {
      router.push("/home");
    }, 500);
  } catch (err) {
    console.error("Signup error:", err);
    console.error("Error response:", err.response?.data);

    error.value =
      err.response?.data?.message ||
      err.response?.data?.error ||
      "Signup failed. Please check your input.";
  } finally {
    loading.value = false;
  }
};

// Watch for password confirmation
watch([password, confirmPassword], () => {
  if (confirmPassword.value && password.value !== confirmPassword.value) {
    error.value = "Passwords do not match.";
  } else {
    error.value = "";
  }
});
</script>

<style scoped>
.signup-page {
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
    rgba(255, 75, 38, 0.08),
    rgba(255, 107, 71, 0.04)
  );
  animation: float 10s ease-in-out infinite;
}

.circle-1 {
  width: 180px;
  height: 180px;
  top: -40px;
  right: -40px;
  animation-delay: 0s;
}

.circle-2 {
  width: 120px;
  height: 120px;
  bottom: 20%;
  left: -30px;
  animation-delay: 3s;
}

.circle-3 {
  width: 100px;
  height: 100px;
  top: 40%;
  right: -20px;
  animation-delay: 6s;
}

.bg-gradient {
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background: radial-gradient(
    circle at 70% 30%,
    rgba(255, 75, 38, 0.03) 0%,
    transparent 50%
  );
}

@keyframes float {
  0%,
  100% {
    transform: translateY(0px) rotate(0deg);
  }
  50% {
    transform: translateY(-15px) rotate(180deg);
  }
}

/* Main Container */
.signup-container {
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
  margin-bottom: 30px;
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

.progress-indicator {
  display: flex;
  align-items: center;
  gap: 12px;
}

.progress-bar {
  width: 80px;
  height: 4px;
  background: rgba(255, 255, 255, 0.1);
  border-radius: 2px;
  overflow: hidden;
}

.progress-fill {
  height: 100%;
  background: linear-gradient(90deg, var(--primary-color), #ff6b47);
  border-radius: 2px;
  transition: width 0.3s ease;
}

.progress-text {
  font-size: 12px;
  color: #888;
  font-weight: 500;
}

/* Welcome Section */
.welcome-section {
  margin-bottom: 30px;
  text-align: left;
}

.signup-title {
  font-size: 36px;
  font-weight: 800;
  line-height: 1.2;
  margin-bottom: 12px;
  display: flex;
  flex-direction: column;
}

.title-line {
  color: white;
  animation: slideInLeft 0.8s ease-out forwards;
  opacity: 0;
}

.title-line:nth-child(1) {
  animation-delay: 0.2s;
}
.title-line:nth-child(2) {
  animation-delay: 0.4s;
}

.highlight {
  background: linear-gradient(135deg, var(--primary-color) 0%, #ff6b47 100%);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
  background-clip: text;
}

.signup-subtitle {
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

.signup-form {
  margin-bottom: 20px;
}

.form-step {
  animation: slideIn 0.5s ease-out;
}

@keyframes slideIn {
  from {
    opacity: 0;
    transform: translateX(20px);
  }
  to {
    opacity: 1;
    transform: translateX(0);
  }
}

.step-header {
  margin-bottom: 24px;
  text-align: center;
}

.step-header h3 {
  font-size: 20px;
  font-weight: 700;
  color: white;
  margin-bottom: 8px;
}

.step-header p {
  color: #888;
  font-size: 14px;
}

.form-group {
  display: flex;
  flex-direction: column;
  gap: 8px;
  margin-bottom: 20px;
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
}

.strength-text {
  color: #888;
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

/* Form Navigation */
.form-navigation {
  display: flex;
  gap: 12px;
  margin-bottom: 20px;
}

.btn-primary,
.btn-secondary {
  flex: 1;
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

.btn-secondary:hover {
  background: rgba(255, 255, 255, 0.15);
  transform: translateY(-1px);
}

.btn-content,
.btn-loading {
  display: flex;
  align-items: center;
  gap: 8px;
}

/* Terms Section */
.terms-section {
  margin-bottom: 20px;
}

.terms-checkbox {
  display: flex;
  align-items: flex-start;
  gap: 12px;
  font-size: 14px;
  color: #ccc;
  cursor: pointer;
  line-height: 1.5;
}

.terms-checkbox input {
  display: none;
}

.checkmark {
  width: 18px;
  height: 18px;
  border: 2px solid rgba(255, 255, 255, 0.3);
  border-radius: 4px;
  position: relative;
  transition: all 0.3s ease;
  flex-shrink: 0;
  margin-top: 2px;
}

.terms-checkbox input:checked + .checkmark {
  background: var(--primary-color);
  border-color: var(--primary-color);
}

.terms-checkbox input:checked + .checkmark::after {
  content: "✓";
  position: absolute;
  top: -2px;
  left: 2px;
  color: white;
  font-size: 12px;
  font-weight: bold;
}

.terms-link {
  color: var(--primary-color);
  text-decoration: none;
  font-weight: 500;
}

.terms-link:hover {
  opacity: 0.8;
}

/* Social Section */
.social-section {
  margin-bottom: 20px;
}

.divider {
  text-align: center;
  position: relative;
  margin: 20px 0;
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

/* Login Section */
.login-section {
  text-align: center;
  margin-top: auto;
  padding-bottom: 20px;
}

.login-section p {
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
  .signup-container {
    padding: 15px;
  }

  .signup-title {
    font-size: 32px;
  }

  .form-navigation {
    flex-direction: column;
  }

  .social-buttons {
    flex-direction: column;
  }

  .terms-checkbox {
    font-size: 13px;
  }
}

/* Animation for form transitions */
.form-step {
  animation: slideIn 0.5s ease-out;
}

@keyframes slideIn {
  from {
    opacity: 0;
    transform: translateX(20px);
  }
  to {
    opacity: 1;
    transform: translateX(0);
  }
}
</style>
