<template>
  <div class="page-container splash-page">
    <!-- Animated Background -->
    <div class="background-animation">
      <div class="floating-shape shape-1"></div>
      <div class="floating-shape shape-2"></div>
      <div class="floating-shape shape-3"></div>
      <div class="floating-shape shape-4"></div>
    </div>

    <!-- Main Content -->
    <div class="content-wrapper">
      <div class="logo-container">
        <div class="logo-wrapper">
          <div class="logo">
            <span class="logo-text">AS</span>
            <div class="tire-icon">
              <div class="tire-inner"></div>
            </div>
          </div>
          <div class="logo-glow"></div>
        </div>

        <h1 class="app-name">
          <span class="letter" style="--delay: 0.1s">A</span>
          <span class="letter" style="--delay: 0.2s">S</span>
          <span class="letter" style="--delay: 0.3s">O</span>
          <span class="letter" style="--delay: 0.4s">M</span>
          <span class="letter" style="--delay: 0.5s">S</span>
        </h1>

        <p class="tagline">
          <span class="tagline-highlight">Choose</span> your Car Products
          <span class="tagline-highlight">at ease</span>
        </p>

        <div class="features-preview">
          <div class="feature-item">
            <font-awesome-icon icon="car" class="feature-icon" />
            <span>Auto Parts</span>
          </div>
          <!-- <div class="feature-item">
            <font-awesome-icon icon="shipping-fast" class="feature-icon" />
            <span>Fast Delivery</span>
          </div> -->
          <div class="feature-item">
            <font-awesome-icon icon="shield-alt" class="feature-icon" />
            <span>Quality Assured</span>
          </div>
        </div>
      </div>

      <div class="auth-section">
        <div class="auth-buttons">
          <router-link to="/login" class="auth-link">
            <button class="btn-primary">
              <span class="btn-text">Get Started</span>
              <div class="btn-icon">
                <font-awesome-icon icon="arrow-right" />
              </div>
            </button>
          </router-link>

          <div class="divider">
            <span>or</span>
          </div>

          <router-link to="/signup" class="auth-link">
            <button class="btn-secondary">
              <span class="btn-text">Create Account</span>
              <div class="btn-icon">
                <font-awesome-icon icon="user-plus" />
              </div>
            </button>
          </router-link>
        </div>

        <!-- <div class="bottom-text">
          <p>Join thousands of satisfied customers</p>
          <div class="trust-indicators">
            <div class="trust-item">
              <font-awesome-icon icon="star" />
              <span>4.8 Rating</span>
            </div>
            <div class="trust-item">
              <font-awesome-icon icon="users" />
              <span>10K+ Users</span>
            </div>
          </div>
        </div> -->
      </div>
    </div>
  </div>
</template>

<script setup>
import { onMounted } from "vue";

onMounted(() => {
  // Add entrance animations
  const elements = document.querySelectorAll(".letter");
  elements.forEach((el, index) => {
    setTimeout(() => {
      el.classList.add("animate-in");
    }, index * 100);
  });
});
</script>

<style scoped>
.splash-page {
  position: relative;
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
  min-height: 100vh;
  overflow: hidden;
  background: linear-gradient(135deg, #0a0a0a 0%, #1a1a1a 50%, #0a0a0a 100%);
}

/* Animated Background */
.background-animation {
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  pointer-events: none;
  overflow: hidden;
}

.floating-shape {
  position: absolute;
  border-radius: 50%;
  background: linear-gradient(
    45deg,
    rgba(255, 75, 38, 0.1),
    rgba(255, 107, 71, 0.05)
  );
  animation: float 6s ease-in-out infinite;
}

.shape-1 {
  width: 100px;
  height: 100px;
  top: 10%;
  left: 10%;
  animation-delay: 0s;
}

.shape-2 {
  width: 150px;
  height: 150px;
  top: 60%;
  right: 10%;
  animation-delay: 2s;
}

.shape-3 {
  width: 80px;
  height: 80px;
  bottom: 20%;
  left: 20%;
  animation-delay: 4s;
}

.shape-4 {
  width: 120px;
  height: 120px;
  top: 30%;
  right: 30%;
  animation-delay: 1s;
}

@keyframes float {
  0%,
  100% {
    transform: translateY(0px) rotate(0deg);
    opacity: 0.3;
  }
  50% {
    transform: translateY(-20px) rotate(180deg);
    opacity: 0.6;
  }
}

/* Main Content */
.content-wrapper {
  display: flex;
  flex-direction: column;
  justify-content: space-between;
  align-items: center;
  height: 100vh;
  width: 100%;
  max-width: 400px;
  padding: 60px 20px 40px;
  z-index: 1;
}

.logo-container {
  display: flex;
  flex-direction: column;
  align-items: center;
  text-align: center;
  flex: 1;
  justify-content: center;
}

.logo-wrapper {
  position: relative;
  margin-bottom: 40px;
}

.logo {
  width: 120px;
  height: 120px;
  background: linear-gradient(135deg, var(--primary-color) 0%, #ff6b47 100%);
  border-radius: 30px;
  display: flex;
  align-items: center;
  justify-content: center;
  position: relative;
  font-weight: 800;
  font-size: 36px;
  color: white;
  box-shadow: 0 20px 40px rgba(255, 75, 38, 0.3);
  animation: logoFloat 3s ease-in-out infinite;
  transform-style: preserve-3d;
}

.logo-text {
  z-index: 2;
  text-shadow: 0 2px 4px rgba(0, 0, 0, 0.3);
}

.tire-icon {
  position: absolute;
  width: 40px;
  height: 40px;
  border: 4px solid white;
  border-radius: 50%;
  bottom: -8px;
  right: -8px;
  background: linear-gradient(45deg, #333, #666);
  display: flex;
  align-items: center;
  justify-content: center;
  animation: tireRotate 4s linear infinite;
}

.tire-inner {
  width: 20px;
  height: 20px;
  border: 2px solid white;
  border-radius: 50%;
  background: #222;
}

.logo-glow {
  position: absolute;
  top: -10px;
  left: -10px;
  right: -10px;
  bottom: -10px;
  background: linear-gradient(
    135deg,
    rgba(255, 75, 38, 0.4),
    rgba(255, 107, 71, 0.2)
  );
  border-radius: 40px;
  filter: blur(20px);
  animation: glowPulse 2s ease-in-out infinite alternate;
  z-index: -1;
}

@keyframes logoFloat {
  0%,
  100% {
    transform: translateY(0px) rotateY(0deg);
  }
  50% {
    transform: translateY(-10px) rotateY(5deg);
  }
}

@keyframes tireRotate {
  from {
    transform: rotate(0deg);
  }
  to {
    transform: rotate(360deg);
  }
}

@keyframes glowPulse {
  0% {
    opacity: 0.5;
    transform: scale(1);
  }
  100% {
    opacity: 0.8;
    transform: scale(1.05);
  }
}

.app-name {
  font-size: 48px;
  font-weight: 800;
  margin-bottom: 20px;
  background: linear-gradient(135deg, #ffffff 0%, #cccccc 100%);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
  background-clip: text;
  letter-spacing: 2px;
}

.letter {
  display: inline-block;
  opacity: 0;
  transform: translateY(30px);
  animation: letterDrop 0.6s ease-out forwards;
  animation-delay: var(--delay);
}

@keyframes letterDrop {
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

.tagline {
  font-size: 18px;
  color: #cccccc;
  margin-bottom: 40px;
  line-height: 1.6;
  max-width: 280px;
}

.tagline-highlight {
  color: var(--primary-color);
  font-weight: 600;
}

.features-preview {
  display: flex;
  gap: 30px;
  margin-top: 20px;
}

.feature-item {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 8px;
  opacity: 0.8;
}

.feature-icon {
  font-size: 24px;
  color: var(--primary-color);
  margin-bottom: 5px;
}

.feature-item span {
  font-size: 12px;
  color: #999;
  font-weight: 500;
}

/* Auth Section */
.auth-section {
  width: 100%;
  display: flex;
  flex-direction: column;
  align-items: center;
}

.auth-buttons {
  width: 100%;
  display: flex;
  flex-direction: column;
  gap: 20px;
  margin-bottom: 30px;
}

.auth-link {
  text-decoration: none;
  width: 100%;
}

.btn-primary,
.btn-secondary {
  width: 100%;
  padding: 18px 24px;
  border-radius: 16px;
  font-weight: 600;
  font-size: 16px;
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 12px;
  transition: all 0.3s ease;
  position: relative;
  overflow: hidden;
  border: none;
  cursor: pointer;
}

.btn-primary {
  background: linear-gradient(135deg, var(--primary-color) 0%, #ff6b47 100%);
  color: white;
  box-shadow: 0 8px 25px rgba(255, 75, 38, 0.3);
}

.btn-primary:hover {
  transform: translateY(-2px);
  box-shadow: 0 12px 35px rgba(255, 75, 38, 0.4);
}

.btn-secondary {
  background: rgba(255, 255, 255, 0.1);
  color: white;
  border: 2px solid rgba(255, 255, 255, 0.2);
  backdrop-filter: blur(10px);
}

.btn-secondary:hover {
  background: rgba(255, 255, 255, 0.15);
  border-color: rgba(255, 255, 255, 0.3);
  transform: translateY(-2px);
}

.btn-text {
  z-index: 2;
}

.btn-icon {
  transition: transform 0.3s ease;
}

.btn-primary:hover .btn-icon,
.btn-secondary:hover .btn-icon {
  transform: translateX(4px);
}

.divider {
  text-align: center;
  position: relative;
  margin: 10px 0;
}

.divider span {
  background: var(--background-color);
  padding: 0 15px;
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

.bottom-text {
  text-align: center;
  color: #888;
}

.bottom-text p {
  font-size: 14px;
  margin-bottom: 15px;
}

.trust-indicators {
  display: flex;
  gap: 20px;
  justify-content: center;
}

.trust-item {
  display: flex;
  align-items: center;
  gap: 6px;
  font-size: 12px;
  color: #999;
}

.trust-item svg {
  color: #ffd700;
  font-size: 14px;
}

/* Responsive Design */
@media (max-width: 480px) {
  .content-wrapper {
    padding: 40px 15px 30px;
  }

  .logo {
    width: 100px;
    height: 100px;
    font-size: 30px;
  }

  .app-name {
    font-size: 40px;
  }

  .features-preview {
    gap: 20px;
  }

  .feature-icon {
    font-size: 20px;
  }
}

/* Loading Animation */
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

.logo-container > * {
  animation: fadeInUp 0.8s ease-out forwards;
}

.auth-section {
  animation: fadeInUp 1s ease-out 0.3s forwards;
  opacity: 0;
}

.auth-section {
  animation-fill-mode: forwards;
}
</style>
