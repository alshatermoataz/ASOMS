<template>
  <div class="page-container">
    <!-- Enhanced User Welcome Section -->
    <div class="user-welcome-bar">
      <div class="user-info">
        <div class="user-avatar">
          <img :src="imageBase(profilePicture)" alt="User Avatar" />
          <div class="online-indicator"></div>
        </div>
        <div class="welcome-text">
          <p class="greeting">{{ greeting }}</p>
          <h2 class="user-name">{{ userName }}</h2>
          <p class="subtitle">Ready to shop today?</p>
        </div>
      </div>
      <div class="header-actions">
        <!-- <button class="notification-btn" aria-label="Notifications">
          <font-awesome-icon icon="bell" />
          <span class="notification-badge">3</span>
        </button> -->
        <button @click="logout" class="logout-icon-btn" aria-label="Logout">
          <font-awesome-icon icon="power-off" />
        </button>
      </div>
    </div>

    <!-- Enhanced Navigation -->
    <div class="top-nav">
      <router-link to="/home" class="top-nav-item active">
        <font-awesome-icon icon="home" />
        <span>Home</span>
      </router-link>
      <router-link to="/shop" class="top-nav-item">
        <font-awesome-icon icon="shopping-cart" />
        <span>Shop</span>
      </router-link>
      <router-link to="/history" class="top-nav-item">
        <font-awesome-icon icon="history" />
        <span>History</span>
      </router-link>
    </div>

    <!-- Quick Stats Section -->
    <!-- <div class="stats-section">
      <div class="stat-card">
        <div class="stat-icon orders">
          <font-awesome-icon icon="shopping-bag" />
        </div>
        <div class="stat-info">
          <span class="stat-number">12</span>
          <span class="stat-label">Orders</span>
        </div>
      </div>
      <div class="stat-card">
        <div class="stat-icon savings">
          <font-awesome-icon icon="piggy-bank" />
        </div>
        <div class="stat-info">
          <span class="stat-number">RM 450</span>
          <span class="stat-label">Saved</span>
        </div>
      </div>
      <div class="stat-card">
        <div class="stat-icon points">
          <font-awesome-icon icon="star" />
        </div>
        <div class="stat-info">
          <span class="stat-number">850</span>
          <span class="stat-label">Points</span>
        </div>
      </div>
    </div> -->

    <!-- Enhanced Hot Items Section -->
    <div class="hot-items-section">
      <div class="section-header">
        <h3 class="section-title">
          <font-awesome-icon icon="fire" class="fire-icon" />
          HOT ITEMS
        </h3>
        <router-link to="/shop" class="see-all-btn">
          See All
          <font-awesome-icon icon="arrow-right" />
        </router-link>
      </div>

      <div class="hot-items-container">
        <div class="hot-items-grid" v-if="hotProducts.length > 0">
          <div
            class="hot-item-card"
            v-for="product in hotProducts"
            :key="product.id"
            @click="goToProductDetail(product.id)"
          >
            <div class="hot-item-badge">
              <font-awesome-icon icon="fire" />
              HOT
            </div>
            <div class="hot-item-image">
              <img :src="imageBase(product.image)" :alt="product.name" />
            </div>
            <div class="hot-item-info">
              <h4 class="product-name">{{ product.name }}</h4>
              <p class="product-category">{{ product.category }}</p>
              <div class="price-section">
                <span class="current-price">RM {{ product.price }}</span>
                <span class="original-price"
                  >RM {{ (product.price * 1.2).toFixed(2) }}</span
                >
              </div>
              <div class="availability-section">
                <div
                  class="availability-indicator"
                  :class="getAvailabilityClass(product.availableQuantity)"
                >
                  <font-awesome-icon
                    :icon="getAvailabilityIcon(product.availableQuantity)"
                  />
                </div>
                <span class="availability-text"
                  >{{ product.availableQuantity }} in stock</span
                >
              </div>
              <button class="quick-view-btn">
                <font-awesome-icon icon="eye" />
                Quick View
              </button>
            </div>
          </div>
        </div>

        <div v-else class="loading-hot-items">
          <div class="loading-spinner"></div>
          <p>Loading hot items...</p>
        </div>
      </div>
    </div>

    <!-- Enhanced Categories Section -->
    <div class="categories-section">
      <div class="section-header">
        <h3 class="section-title">
          <font-awesome-icon icon="th-large" />
          Shop by Category
        </h3>
      </div>

      <div class="category-grid">
        <div
          class="category-card"
          v-for="category in categories"
          :key="category.name"
          @click="goToShopWithCategory(category.name)"
        >
          <div class="category-icon-container" :class="category.colorClass">
            <font-awesome-icon :icon="category.icon" class="category-icon" />
          </div>
          <p class="category-name">{{ category.name }}</p>
          <!-- <span class="category-count">{{ category.count }} items</span> -->
          <div class="category-hover-effect">
            <font-awesome-icon icon="arrow-right" />
          </div>
        </div>
      </div>
    </div>

    <!-- Featured Deals Section -->
    <!-- <div class="deals-section">
      <div class="section-header">
        <h3 class="section-title">
          <font-awesome-icon icon="tags" />
          Special Deals
        </h3>
      </div>

      <div class="deals-banner">
        <div class="deal-content">
          <div class="deal-badge">LIMITED TIME</div>
          <h4>Up to 30% OFF</h4>
          <p>On selected automotive parts</p>
          <button class="shop-now-btn" @click="goToShop">
            Shop Now
            <font-awesome-icon icon="shopping-cart" />
          </button>
        </div>
        <div class="deal-illustration">
          <font-awesome-icon icon="car" class="car-icon" />
        </div>
      </div>
    </div> -->

    <!-- Bottom Navigation -->
    <BottomNavigation />
  </div>
</template>

<script setup>
import { ref, onMounted } from "vue";
import axios from "axios";
import { useAuthStore } from "../stores/auth";
import { useRouter } from "vue-router";
import BottomNavigation from "../components/BottomNavigation.vue";

const auth = useAuthStore();
const router = useRouter();
const userName = auth.user?.fullName?.trim() || "Guest";
const profilePicture =
  auth.user?.profilePictureUrl || "https://i.ibb.co/nPRRbJ5/ghost-icon.png";
const API_BASE = "https://asoms-production.up.railway.app";

const hotProducts = ref([]);
const greeting = ref("");

// Enhanced categories with icons and colors
const categories = ref([
  {
    name: "Tyres",
    icon: "circle",
    colorClass: "tyres-color",
    count: 45,
  },
  {
    name: "Oil",
    icon: "tint",
    colorClass: "oil-color",
    count: 23,
  },
  {
    name: "Filters",
    icon: "filter",
    colorClass: "filters-color",
    count: 18,
  },
  {
    name: "Batteries",
    icon: "battery-full",
    colorClass: "batteries-color",
    count: 12,
  },
  {
    name: "Parts",
    icon: "cog",
    colorClass: "parts-color",
    count: 67,
  },
  {
    name: "Accessories",
    icon: "wrench",
    colorClass: "accessories-color",
    count: 34,
  },
]);

const fetchHotProducts = async () => {
  try {
    const response = await axios.get(
      `${API_BASE}/api/analytics/top-products?period=30d`
    );
    // Handle the response data structure from analytics API
    const topProductsData = response.data;

    // If the response is an array of products directly
    if (Array.isArray(topProductsData)) {
      hotProducts.value = topProductsData.slice(0, 4); // Limit to 4 items for homepage
    }
    // If the response has a data property containing the products
    else if (topProductsData.data && Array.isArray(topProductsData.data)) {
      hotProducts.value = topProductsData.data.slice(0, 4);
    }
    // If the response has a products property
    else if (
      topProductsData.products &&
      Array.isArray(topProductsData.products)
    ) {
      hotProducts.value = topProductsData.products.slice(0, 4);
    }
    // If the response has a topProducts property
    else if (
      topProductsData.topProducts &&
      Array.isArray(topProductsData.topProducts)
    ) {
      hotProducts.value = topProductsData.topProducts.slice(0, 4);
    } else {
      console.warn("Unexpected API response structure:", topProductsData);
      hotProducts.value = [];
    }
  } catch (err) {
    console.error("Failed to fetch hot products:", err);
    // Fallback to regular products API if analytics fails
    try {
      const fallbackResponse = await axios.get(
        `${API_BASE}/api/product?pageSize=4`
      );
      hotProducts.value =
        fallbackResponse.data.items || fallbackResponse.data || [];
    } catch (fallbackErr) {
      console.error("Fallback API also failed:", fallbackErr);
      hotProducts.value = [];
    }
  }
};

const imageBase = (url) =>
  url?.startsWith("http") ? url : `${API_BASE}${url}`;

const setGreeting = () => {
  const hour = new Date().getHours();
  if (hour < 12) greeting.value = "Good Morning";
  else if (hour < 17) greeting.value = "Good Afternoon";
  else greeting.value = "Good Evening";
};

const getAvailabilityClass = (quantity) => {
  if (quantity > 10) return "high-stock";
  if (quantity > 5) return "medium-stock";
  return "low-stock";
};

const getAvailabilityIcon = (quantity) => {
  if (quantity > 10) return "check-circle";
  if (quantity > 5) return "exclamation-circle";
  return "times-circle";
};

const goToProductDetail = (productId) => {
  router.push(`/product/${productId}`);
};

const goToShopWithCategory = (categoryName) => {
  router.push({
    path: "/shop",
    query: { category: categoryName },
  });
};

const goToShop = () => {
  router.push("/shop");
};

const logout = () => {
  auth.logout();
  router.push("/login");
};

const userNameRef = ref(userName);
const profilePictureRef = ref(profilePicture);

onMounted(() => {
  fetchHotProducts();
  setGreeting();
});
</script>

<style scoped>
.page-container {
  padding: 20px;
  padding-bottom: 100px;
}

/* Enhanced User Welcome Section */
.user-welcome-bar {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 25px;
  padding: 20px;
  background: linear-gradient(
    135deg,
    var(--card-bg) 0%,
    rgba(255, 75, 38, 0.1) 100%
  );
  border-radius: 20px;
  border: 1px solid rgba(255, 255, 255, 0.1);
}

.user-info {
  display: flex;
  align-items: center;
  flex: 1;
}

.user-avatar {
  position: relative;
  width: 60px;
  height: 60px;
  border-radius: 50%;
  overflow: hidden;
  background-color: #222;
  margin-right: 15px;
  border: 3px solid var(--primary-color);
}

.user-avatar img {
  width: 100%;
  height: 100%;
  object-fit: cover;
}

.online-indicator {
  position: absolute;
  bottom: 2px;
  right: 2px;
  width: 16px;
  height: 16px;
  background-color: #22c55e;
  border-radius: 50%;
  border: 2px solid var(--background-color);
}

.welcome-text .greeting {
  font-size: 14px;
  color: #cccccc;
  margin: 0;
}

.welcome-text .user-name {
  font-size: 20px;
  font-weight: 700;
  margin: 2px 0;
  color: white;
}

.welcome-text .subtitle {
  font-size: 12px;
  color: var(--primary-color);
  margin: 0;
}

.header-actions {
  display: flex;
  gap: 10px;
  align-items: center;
}

.notification-btn {
  position: relative;
  width: 40px;
  height: 40px;
  border: 1px solid rgba(255, 255, 255, 0.2);
  border-radius: 50%;
  background-color: rgba(255, 255, 255, 0.1);
  color: white;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 16px;
  cursor: pointer;
  transition: all 0.3s ease;
}

.notification-badge {
  position: absolute;
  top: -5px;
  right: -5px;
  background-color: var(--primary-color);
  color: white;
  border-radius: 50%;
  width: 18px;
  height: 18px;
  font-size: 10px;
  display: flex;
  align-items: center;
  justify-content: center;
  font-weight: bold;
}

.logout-icon-btn {
  width: 40px;
  height: 40px;
  border: 1px solid rgba(255, 255, 255, 0.2);
  border-radius: 50%;
  background-color: rgba(255, 255, 255, 0.1);
  color: white;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 16px;
  cursor: pointer;
  transition: all 0.3s ease;
}

.logout-icon-btn:hover,
.notification-btn:hover {
  background-color: var(--primary-color);
  transform: translateY(-2px);
}

/* Enhanced Navigation */
.top-nav {
  display: flex;
  background-color: var(--card-bg);
  border-radius: 15px;
  padding: 8px;
  margin-bottom: 25px;
  border: 1px solid rgba(255, 255, 255, 0.1);
}

.top-nav-item {
  flex: 1;
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 5px;
  padding: 12px 8px;
  border-radius: 10px;
  text-decoration: none;
  color: #cccccc;
  transition: all 0.3s ease;
  font-size: 12px;
  font-weight: 500;
}

.top-nav-item.active {
  background-color: var(--primary-color);
  color: white;
  transform: translateY(-2px);
  box-shadow: 0 4px 12px rgba(255, 75, 38, 0.3);
}

.top-nav-item:hover:not(.active) {
  color: var(--primary-color);
  background-color: rgba(255, 75, 38, 0.1);
}

/* Stats Section */
.stats-section {
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  gap: 15px;
  margin-bottom: 30px;
}

.stat-card {
  background-color: var(--card-bg);
  border-radius: 15px;
  padding: 20px 15px;
  display: flex;
  align-items: center;
  gap: 12px;
  border: 1px solid rgba(255, 255, 255, 0.1);
  transition: transform 0.3s ease;
}

.stat-card:hover {
  transform: translateY(-3px);
}

.stat-icon {
  width: 40px;
  height: 40px;
  border-radius: 10px;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 18px;
}

.stat-icon.orders {
  background-color: rgba(34, 197, 94, 0.2);
  color: #22c55e;
}

.stat-icon.savings {
  background-color: rgba(59, 130, 246, 0.2);
  color: #3b82f6;
}

.stat-icon.points {
  background-color: rgba(245, 158, 11, 0.2);
  color: #f59e0b;
}

.stat-info {
  display: flex;
  flex-direction: column;
}

.stat-number {
  font-size: 16px;
  font-weight: 700;
  color: white;
}

.stat-label {
  font-size: 11px;
  color: #cccccc;
}

/* Enhanced Hot Items Section */
.section-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 20px;
}

.section-title {
  display: flex;
  align-items: center;
  gap: 10px;
  font-size: 20px;
  font-weight: 700;
  color: white;
}

.fire-icon {
  color: var(--primary-color);
  animation: flicker 2s infinite alternate;
}

@keyframes flicker {
  0% {
    opacity: 1;
  }
  50% {
    opacity: 0.8;
  }
  100% {
    opacity: 1;
  }
}

.see-all-btn {
  display: flex;
  align-items: center;
  gap: 5px;
  color: var(--primary-color);
  text-decoration: none;
  font-size: 14px;
  font-weight: 600;
  transition: all 0.3s ease;
}

.see-all-btn:hover {
  transform: translateX(3px);
}

.hot-items-container {
  margin-bottom: 30px;
}

.hot-items-grid {
  display: grid;
  grid-template-columns: repeat(2, 1fr);
  gap: 20px;
}

.hot-item-card {
  position: relative;
  background-color: var(--card-bg);
  border-radius: 20px;
  padding: 20px;
  border: 1px solid rgba(255, 255, 255, 0.1);
  transition: all 0.3s ease;
  cursor: pointer;
  overflow: hidden;
}

.hot-item-card:hover {
  transform: translateY(-5px);
  box-shadow: 0 10px 30px rgba(255, 75, 38, 0.2);
}

.hot-item-badge {
  position: absolute;
  top: 15px;
  right: 15px;
  background: linear-gradient(45deg, var(--primary-color), #ff6b47);
  color: white;
  padding: 5px 10px;
  border-radius: 20px;
  font-size: 10px;
  font-weight: bold;
  display: flex;
  align-items: center;
  gap: 3px;
}

.hot-item-image {
  width: 100%;
  height: 120px;
  margin-bottom: 15px;
  display: flex;
  justify-content: center;
  align-items: center;
  background-color: rgba(255, 255, 255, 0.05);
  border-radius: 15px;
}

.hot-item-image img {
  max-width: 80%;
  max-height: 80%;
  object-fit: contain;
}

.product-name {
  font-size: 16px;
  font-weight: 600;
  margin-bottom: 5px;
  color: white;
}

.product-category {
  font-size: 12px;
  color: #cccccc;
  margin-bottom: 10px;
}

.price-section {
  display: flex;
  align-items: center;
  gap: 8px;
  margin-bottom: 10px;
}

.current-price {
  color: var(--primary-color);
  font-weight: 700;
  font-size: 18px;
}

.original-price {
  color: #999;
  font-size: 14px;
  text-decoration: line-through;
}

.availability-section {
  display: flex;
  align-items: center;
  gap: 8px;
  margin-bottom: 15px;
}

.availability-indicator {
  width: 20px;
  height: 20px;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 10px;
}

.availability-indicator.high-stock {
  background-color: rgba(34, 197, 94, 0.2);
  color: #22c55e;
}

.availability-indicator.medium-stock {
  background-color: rgba(245, 158, 11, 0.2);
  color: #f59e0b;
}

.availability-indicator.low-stock {
  background-color: rgba(239, 68, 68, 0.2);
  color: #ef4444;
}

.availability-text {
  font-size: 12px;
  color: #cccccc;
}

.quick-view-btn {
  width: 100%;
  background: linear-gradient(45deg, var(--primary-color), #ff6b47);
  color: white;
  border: none;
  padding: 10px;
  border-radius: 10px;
  font-weight: 600;
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 8px;
  transition: all 0.3s ease;
}

.quick-view-btn:hover {
  transform: translateY(-2px);
  box-shadow: 0 5px 15px rgba(255, 75, 38, 0.4);
}

/* Loading State */
.loading-hot-items {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  padding: 40px;
  color: #cccccc;
}

.loading-spinner {
  width: 40px;
  height: 40px;
  border: 3px solid rgba(255, 75, 38, 0.3);
  border-top: 3px solid var(--primary-color);
  border-radius: 50%;
  animation: spin 1s linear infinite;
  margin-bottom: 15px;
}

@keyframes spin {
  0% {
    transform: rotate(0deg);
  }
  100% {
    transform: rotate(360deg);
  }
}

/* Enhanced Categories Section */
.categories-section {
  margin-bottom: 30px;
}

.category-grid {
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  gap: 20px;
}

.category-card {
  position: relative;
  display: flex;
  flex-direction: column;
  align-items: center;
  text-align: center;
  background-color: var(--card-bg);
  border-radius: 20px;
  padding: 25px 15px;
  border: 1px solid rgba(255, 255, 255, 0.1);
  cursor: pointer;
  transition: all 0.3s ease;
  overflow: hidden;
}

.category-card:hover {
  transform: translateY(-5px);
  box-shadow: 0 10px 30px rgba(0, 0, 0, 0.3);
}

.category-card:hover .category-hover-effect {
  opacity: 1;
  transform: translateX(0);
}

.category-icon-container {
  width: 60px;
  height: 60px;
  border-radius: 15px;
  display: flex;
  align-items: center;
  justify-content: center;
  margin-bottom: 15px;
  transition: transform 0.3s ease;
}

.category-card:hover .category-icon-container {
  transform: scale(1.1);
}

.category-icon {
  font-size: 24px;
  color: white;
}

.tyres-color {
  background: linear-gradient(45deg, #6366f1, #8b5cf6);
}

.oil-color {
  background: linear-gradient(45deg, #f59e0b, #f97316);
}

.filters-color {
  background: linear-gradient(45deg, #06b6d4, #0891b2);
}

.batteries-color {
  background: linear-gradient(45deg, #22c55e, #16a34a);
}

.parts-color {
  background: linear-gradient(45deg, #ef4444, #dc2626);
}

.accessories-color {
  background: linear-gradient(45deg, #8b5cf6, #a855f7);
}

.category-name {
  font-size: 16px;
  font-weight: 600;
  margin-bottom: 5px;
  color: white;
}

.category-count {
  font-size: 12px;
  color: #cccccc;
}

.category-hover-effect {
  position: absolute;
  top: 15px;
  right: 15px;
  opacity: 0;
  transform: translateX(10px);
  transition: all 0.3s ease;
  color: var(--primary-color);
}

/* Deals Section */
.deals-section {
  margin-bottom: 30px;
}

.deals-banner {
  background: linear-gradient(135deg, var(--primary-color) 0%, #ff6b47 100%);
  border-radius: 20px;
  padding: 25px;
  display: flex;
  justify-content: space-between;
  align-items: center;
  color: white;
  position: relative;
  overflow: hidden;
}

.deals-banner::before {
  content: "";
  position: absolute;
  top: -50%;
  right: -50%;
  width: 200%;
  height: 200%;
  background: radial-gradient(
    circle,
    rgba(255, 255, 255, 0.1) 0%,
    transparent 70%
  );
  animation: pulse 3s ease-in-out infinite;
}

@keyframes pulse {
  0%,
  100% {
    transform: scale(1);
    opacity: 0.5;
  }
  50% {
    transform: scale(1.1);
    opacity: 0.8;
  }
}

.deal-content {
  flex: 1;
  z-index: 1;
}

.deal-badge {
  background-color: rgba(255, 255, 255, 0.2);
  padding: 5px 12px;
  border-radius: 20px;
  font-size: 10px;
  font-weight: bold;
  display: inline-block;
  margin-bottom: 10px;
}

.deal-content h4 {
  font-size: 24px;
  font-weight: 700;
  margin-bottom: 5px;
}

.deal-content p {
  font-size: 14px;
  margin-bottom: 15px;
  opacity: 0.9;
}

.shop-now-btn {
  background-color: white;
  color: var(--primary-color);
  border: none;
  padding: 12px 20px;
  border-radius: 25px;
  font-weight: 600;
  display: flex;
  align-items: center;
  gap: 8px;
  transition: all 0.3s ease;
  cursor: pointer;
}

.shop-now-btn:hover {
  transform: translateY(-2px);
  box-shadow: 0 5px 15px rgba(0, 0, 0, 0.2);
}

.deal-illustration {
  font-size: 60px;
  opacity: 0.3;
  z-index: 1;
}

.car-icon {
  animation: float 3s ease-in-out infinite;
}

@keyframes float {
  0%,
  100% {
    transform: translateY(0px);
  }
  50% {
    transform: translateY(-10px);
  }
}

/* Responsive Design */
@media (max-width: 480px) {
  .hot-items-grid {
    grid-template-columns: 1fr;
  }

  .category-grid {
    grid-template-columns: repeat(2, 1fr);
  }

  .stats-section {
    grid-template-columns: 1fr;
  }

  .deals-banner {
    flex-direction: column;
    text-align: center;
    gap: 20px;
  }
}
</style>
