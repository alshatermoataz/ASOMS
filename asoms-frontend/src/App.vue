<template>
  <div class="app-container">
    <router-view />
  </div>
</template>

<script setup>
// App component logic
</script>

<style>
:root {
  --primary-color: #ff4b26;
  --secondary-color: #ffffff;
  --background-color: #1d1d1d;
  --text-color: #ffffff;
  --card-bg: #212121;
  --input-bg: #3c3c3c;
}

* {
  margin: 0;
  padding: 0;
  box-sizing: border-box;
  font-family: "Inter", sans-serif;
}

body {
  background-color: var(--background-color);
  color: var(--text-color);
}

.app-container {
  max-width: 430px;
  min-height: 100vh;
  margin: 0 auto;
  background-color: var(--background-color);
  position: relative;
}

.page-container {
  padding: 20px;
  min-height: 100vh;
  display: flex;
  flex-direction: column;
}

.status-bar {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 10px 20px;
  font-weight: 500;
}

.btn-primary {
  background-color: var(--primary-color);
  color: white;
  border: none;
  padding: 12px 20px;
  border-radius: 8px;
  font-weight: 600;
  width: 100%;
  cursor: pointer;
  font-size: 16px;
}

.btn-secondary {
  background-color: transparent;
  color: white;
  border: 1px solid white;
  padding: 12px 20px;
  border-radius: 8px;
  font-weight: 600;
  width: 100%;
  cursor: pointer;
  font-size: 16px;
}

.input-field {
  background-color: var(--input-bg);
  border: none;
  border-radius: 8px;
  padding: 12px 15px;
  color: white;
  width: 100%;
  margin-bottom: 15px;
}

.input-label {
  margin-bottom: 8px;
  display: block;
  font-size: 14px;
  color: #cccccc;
}

.back-link {
  color: white;
  text-decoration: none;
  display: inline-block;
  margin-top: 20px;
}

.bottom-nav {
  display: flex;
  justify-content: space-around;
  background-color: #0d0d0d;
  padding: 15px 0;
  position: fixed;
  bottom: 0;
  width: 100%;
  max-width: 430px;
}

.bottom-nav-item {
  display: flex;
  flex-direction: column;
  align-items: center;
  color: #5c5c5c;
  text-decoration: none;
  font-size: 12px;
}

.bottom-nav-item.active {
  color: var(--primary-color);
}

.bottom-nav-icon {
  font-size: 20px;
  margin-bottom: 5px;
}

.section-title {
  font-size: 18px;
  font-weight: 600;
  margin: 20px 0 15px 0;
}

.product-card {
  background-color: var(--card-bg);
  border-radius: 10px;
  padding: 15px;
  margin-bottom: 15px;
  display: flex;
  align-items: center;
}

.product-image {
  width: 80px;
  height: 80px;
  object-fit: cover;
  border-radius: 8px;
  margin-right: 15px;
}

.product-info {
  flex: 1;
}

.product-name {
  font-weight: 600;
  margin-bottom: 5px;
}

.product-price {
  color: var(--primary-color);
  font-weight: 600;
  margin-bottom: 10px;
}

.view-button {
  color: var(--primary-color);
  text-decoration: none;
  font-weight: 600;
}

.top-nav {
  display: flex;
  margin: 20px 0;
  border-bottom: 1px solid #3c3c3c;
}

.top-nav-item {
  padding: 10px 20px;
  color: #5c5c5c;
  text-decoration: none;
  font-weight: 500;
}

.top-nav-item.active {
  color: white;
  border-bottom: 2px solid var(--primary-color);
}
</style>
