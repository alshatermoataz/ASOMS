<template>
  <div class="page-container">
    <div class="status-bar">
      <span>9:41</span>
      <div>
        <span>Π</span>
      </div>
    </div>

    <div class="user-welcome">
      <div class="user-avatar"></div>
      <div class="welcome-text">
        <p>Welcome Back,</p>
        <h2>{{ user?.fullName || "User" }}</h2>
      </div>
    </div>

    <div class="top-nav">
      <router-link to="/home" class="top-nav-item">Home</router-link>
      <router-link to="/shop" class="top-nav-item active">Shop</router-link>
      <router-link to="/history" class="top-nav-item">History</router-link>
    </div>

    <div class="search-section">
      <div class="search-bar">
        <font-awesome-icon icon="search" class="search-icon" />
        <input
          type="text"
          v-model="searchQuery"
          placeholder="Insert Order number"
          class="search-input"
        />
      </div>

      <div class="category-dropdown">
        <select
          id="category"
          v-model="selectedCategory"
          class="input-field category-select"
        >
          <option value="">Category</option>
          <option value="Tyres">Tyres</option>
          <option value="Oil">Oil</option>
          <option value="Accessories">Accessories</option>
          <option value="Tyres">Tyres</option>
          <option value="Parts">Car Parts</option>
          <option value="Batteries">Batteries</option>
          <!-- <option value="Batteries">Batteries</option> -->
        </select>
      </div>
    </div>

    <div class="product-list">
      <div
        class="product-card"
        v-for="product in filteredProducts"
        :key="product.id"
      >
        <img
          :src="fullImageUrl(product.imageUrl) || placeholderImage"
          :alt="product.name"
          class="product-image"
        />
        <div class="product-info">
          <h4 class="product-name">{{ product.name }}</h4>
          <p class="product-price">RM{{ product.price }}</p>
          <p
            class="product-status"
            :class="
              product.status === 'Active' ? 'status-active' : 'status-inactive'
            "
          >
            {{ product.status }}
          </p>
          <router-link :to="`/product/${product.id}`" class="view-button"
            >View <font-awesome-icon icon="arrow-right"
          /></router-link>
        </div>
      </div>
    </div>

    <router-link :to="`/checkout`" class="btn-checkout">Checkout</router-link>

    <!-- Bottom Navigation -->
    <BottomNavigation />
  </div>
</template>

<script setup>
import { ref, onMounted, computed } from "vue";
import axios from "axios";
import { useAuthStore } from "../stores/auth";
import BottomNavigation from "../components/BottomNavigation.vue";

const searchQuery = ref("");
const selectedCategory = ref("");
const products = ref([]);
const placeholderImage = "https://via.placeholder.com/150";
const auth = useAuthStore();
const user = auth.user;

console.log("Products: ", products);
const fetchProducts = async () => {
  try {
    const res = await axios.get(
      "https://asoms-production.up.railway.app/api/product?pageSize=100"
    );
    const productData = res.data.items || res.data || [];
    products.value = productData.sort(
      (a, b) => new Date(b.createdAt) - new Date(a.createdAt)
    );
  } catch (err) {
    console.error("Error fetching products:", err);
  }
};

const fullImageUrl = (path) =>
  path?.startsWith("http")
    ? path
    : `https://asoms-production.up.railway.app${path}`;

const filteredProducts = computed(() => {
  return products.value.filter((p) => {
    const matchCategory =
      selectedCategory.value === "" ||
      p.category.toLowerCase() === selectedCategory.value.toLowerCase();
    const matchSearch = p.name
      .toLowerCase()
      .includes(searchQuery.value.toLowerCase());
    return matchCategory && matchSearch;
  });
});

const goToCheckout = () => {
  router.push("/checkout");
};

onMounted(fetchProducts);
</script>

<style scoped>
@import "../assets/main.css";

.search-section {
  margin-bottom: 20px;
  padding: 0 20px;
}

.search-bar {
  position: relative;
  margin-bottom: 15px;
}

.search-icon {
  position: absolute;
  left: 15px;
  top: 50%;
  transform: translateY(-50%);
  color: #5c5c5c;
}

.search-input {
  background-color: #fff;
  border: none;
  border-radius: 10px;
  padding: 12px 15px 12px 40px;
  color: #000;
  width: 100%;
}

.category-select {
  background-color: #fcd9b5;
  color: #000;
  border-radius: 20px;
  padding: 12px;
  font-weight: bold;
  width: 100%;
}

.product-list {
  padding: 20px;
  margin-bottom: 20px;
}

.product-card {
  display: flex;
  align-items: center;
  background-color: var(--card-bg);
  border-radius: 12px;
  padding: 10px;
  margin-bottom: 15px;
}

.product-image {
  width: 50px;
  height: 50px;
  object-fit: contain;
  margin-right: 15px;
  border-radius: 50%;
  background-color: white;
  padding: 5px;
}

.product-info {
  flex-grow: 1;
}

.product-name {
  font-size: 14px;
  font-weight: 600;
}

.product-price {
  color: orange;
  font-weight: bold;
  font-size: 14px;
  margin: 4px 0;
}

.view-button {
  background-color: #fff;
  color: #000;
  padding: 4px 8px;
  border-radius: 5px;
  font-size: 12px;
  font-weight: bold;
}

.btn-checkout {
  background-color: limegreen;
  color: black;
  font-weight: bold;
  border: none;
  padding: 12px;
  border-radius: 6px;
  width: 90%;
  margin: 0 auto 90px auto;
  display: block;
}

.product-status {
  font-size: 10px;
  font-weight: bold;
  margin: 2px 0;
}
.status-active {
  color: #22c55e;
}
.status-inactive {
  color: #ef4444;
}
</style>
