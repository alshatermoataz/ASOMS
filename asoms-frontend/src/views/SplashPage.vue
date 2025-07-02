<template>
  <div class="page-container splash-page">
    <!-- <div class="status-bar">
        <span>9:41</span>
        <div>
          <span>Π</span>
        </div>
      </div>
       -->
    <div class="logo-container">
      <div class="logo">
        <span>AB</span>
        <div class="tire-icon"></div>
      </div>
      <h1 class="app-name">ASOMS</h1>
      <p class="tagline">Choose your clothes at an ease</p>
    </div>

    <div class="auth-buttons">
      <router-link to="/login">
        <button class="btn-primary">Login</button>
      </router-link>
      <router-link to="/signup" class="mt-15">
        <button class="btn-secondary">Signup</button>
      </router-link>
    </div>
  </div>
</template>

<script setup>
// Splash page logic
</script>

<style scoped>
.splash-page {
  display: flex;
  flex-direction: column;
  justify-content: space-between;
  height: 100vh;
}

.logo-container {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  flex: 1;
}

.logo {
  width: 80px;
  height: 80px;
  background-color: var(--primary-color);
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  position: relative;
  margin-bottom: 20px;
  font-weight: bold;
  font-size: 24px;
}

.tire-icon {
  position: absolute;
  width: 30px;
  height: 30px;
  border: 3px solid white;
  border-radius: 50%;
  bottom: -5px;
  right: -5px;
}

.app-name {
  font-size: 36px;
  font-weight: 700;
  margin-bottom: 10px;
}

.tagline {
  font-size: 16px;
  color: #cccccc;
  margin-bottom: 40px;
}

.auth-buttons {
  padding: 0 20px 40px 20px;
}

.mt-15 {
  margin-top: 15px;
  display: block;
}
</style>
