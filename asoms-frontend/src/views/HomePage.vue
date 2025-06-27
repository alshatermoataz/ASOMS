<template>
  <div class="page-container">
    <div class="status-bar">
      <span>9:41</span>
      <div>
        <span>Π</span>
      </div>
    </div>

    <div class="user-welcome-bar">
      <div class="user-info">
        <div class="user-avatar">
          <img :src="imageBase(profilePicture)" alt="User Avatar" />
        </div>
        <div class="welcome-text">
          <p>Welcome Back,</p>
          <h2>{{ userName }}</h2>
        </div>
      </div>
      <button @click="logout" class="logout-icon-btn" aria-label="Logout">
        <font-awesome-icon icon="power-off" />
      </button>
    </div>

    <div class="top-nav">
      <router-link to="/home" class="top-nav-item active">Home</router-link>
      <router-link to="/shop" class="top-nav-item">Shop</router-link>
      <router-link to="/history" class="top-nav-item">History</router-link>
    </div>

    <div class="hot-items-section">
      <h3 class="section-title">HOT ITEMS</h3>
      <div class="hot-items-grid">
        <div
          class="hot-item-card"
          v-for="product in hotProducts"
          :key="product.id"
        >
          <div class="hot-item-image">
            <img :src="imageBase(product.imageUrl)" :alt="product.name" />
          </div>
          <div class="hot-item-info">
            <h4>{{ product.name }}</h4>
            <p>{{ product.size }}</p>
            <p class="hot-item-price">${{ product.price }}</p>
            <p class="availability">Availability: {{ product.quantity }}pcs</p>
          </div>
        </div>
      </div>
    </div>

    <div class="other-products-section">
      <h3 class="section-title">Other products</h3>
      <div class="category-grid">
        <div class="category-card">
          <div class="category-image">
            <img :src="TyresImage" alt="Tyres" />
          </div>
          <p>Tyres</p>
        </div>
        <div class="category-card">
          <div class="category-image">
            <img :src="oilImage" alt="Oil" />
          </div>
          <p>Oil</p>
        </div>
        <div class="category-card">
          <div class="category-image">
            <img :src="FilterImage" alt="Filters" />
          </div>
          <p>Filters</p>
        </div>
      </div>
    </div>

    <div class="bottom-nav">
      <router-link to="/home" class="bottom-nav-item active">
        <font-awesome-icon icon="home" class="bottom-nav-icon" />
        <span>Home</span>
      </router-link>
      <router-link to="/shop" class="bottom-nav-item">
        <font-awesome-icon icon="shopping-cart" class="bottom-nav-icon" />
        <span>Shop</span>
      </router-link>
      <router-link to="/history" class="bottom-nav-item">
        <font-awesome-icon icon="history" class="bottom-nav-icon" />
        <span>History</span>
      </router-link>
      <router-link to="/profile" class="bottom-nav-item">
        <font-awesome-icon icon="user" class="bottom-nav-icon" />
        <span>Profile</span>
      </router-link>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from "vue";
import axios from "axios";
import { useAuthStore } from "../stores/auth";
import { useRouter } from "vue-router";

const auth = useAuthStore();
const router = useRouter();
const userName = auth.user?.fullName?.trim() || "Guest";
const profilePicture =
  auth.user?.profilePictureUrl || "https://i.ibb.co/nPRRbJ5/ghost-icon.png";
const API_BASE = "https://asoms-production.up.railway.app";

const hotProducts = ref([]);

console.log("User: ", auth.user);

const fetchHotProducts = async () => {
  try {
    const response = await axios.get(`${API_BASE}/api/product?pageSize=2`);
    hotProducts.value = response.data.items || response.data || [];
  } catch (err) {
    console.error("Failed to fetch hot products:", err);
  }
};

const imageBase = (url) =>
  url?.startsWith("http") ? url : `${API_BASE}${url}`;

onMounted(fetchHotProducts);

const logout = () => {
  auth.logout();
  router.push("/login");
};

import oilImage from "../assets/Oil.jpg";
import TyresImage from "../assets/Tyres.jpg";
import FilterImage from "../assets/Filter.jpg";
</script>

<style scoped>
.logout-icon-btn {
  width: 32px;
  height: 32px;
  border: 1px solid #ccc;
  border-radius: 50%;
  background-color: #2b2b2b;
  color: white;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 14px;
  cursor: pointer;
  transition: background-color 0.2s ease;
}

.logout-icon-btn:hover {
  background-color: #f87171;
  color: black;
}

.user-welcome-bar {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin: 20px 0;
}

.user-info {
  display: flex;
  align-items: center;
}

.user-avatar {
  width: 50px;
  height: 50px;
  border-radius: 50%;
  overflow: hidden;
  background-color: #222;
  margin-right: 12px;
}

.user-avatar img {
  width: 100%;
  height: 100%;
  object-fit: cover;
}

.logout-btn {
  background: none;
  border: none;
  color: white;
  font-size: 18px;
  padding: 8px;
  cursor: pointer;
}
.logout-btn:hover {
  color: #f87171;
}

.welcome-text p {
  font-size: 13px;
  color: #cccccc;
  margin: 0;
}

.welcome-text h2 {
  font-size: 18px;
  font-weight: 600;
  margin: 0;
}

.user-welcome {
  display: flex;
  align-items: center;
  margin-bottom: 20px;
}

.user-avatar {
  width: 50px;
  height: 50px;
  border-radius: 50%;
  background-color: #3c3c3c;
  margin-right: 15px;
}

.welcome-text p {
  font-size: 14px;
  color: #cccccc;
}

.welcome-text h2 {
  font-size: 18px;
  font-weight: 600;
}

.hot-items-grid {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 15px;
  margin-bottom: 30px;
}

.hot-item-card {
  background-color: var(--card-bg);
  border-radius: 10px;
  padding: 15px;
  height: 100%;
}

.hot-item-image {
  width: 100%;
  height: 120px;
  margin-bottom: 10px;
  display: flex;
  justify-content: center;
  align-items: center;
}

.hot-item-image img {
  max-width: 100%;
  max-height: 100%;
  object-fit: contain;
}

.hot-item-info h4 {
  font-size: 16px;
  font-weight: 600;
  margin-bottom: 5px;
}

.hot-item-info p {
  font-size: 14px;
  color: #cccccc;
  margin-bottom: 5px;
}

.hot-item-price {
  color: var(--primary-color) !important;
  font-weight: 600 !important;
  font-size: 16px !important;
}

.availability {
  font-size: 12px !important;
  color: #999999 !important;
}

.category-grid {
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  gap: 15px;
  margin-bottom: 80px;
}

.category-card {
  display: flex;
  flex-direction: column;
  align-items: center;
  text-align: center;
}

.category-image {
  width: 80px;
  height: 80px;
  background-color: var(--card-bg);
  border-radius: 10px;
  display: flex;
  justify-content: center;
  align-items: center;
  margin-bottom: 10px;
}

.category-image img {
  max-width: 60%;
  max-height: 60%;
  object-fit: contain;
}

.category-card p {
  font-size: 14px;
}
</style>
