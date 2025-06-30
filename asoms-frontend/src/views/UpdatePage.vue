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
        <p>Welcome,</p>
        <h2>Moataz Abdo</h2>
      </div>
    </div>

    <div class="top-nav">
      <router-link to="/home" class="top-nav-item">Home</router-link>
      <router-link to="/update" class="top-nav-item active">Update</router-link>
      <router-link to="/check" class="top-nav-item">Check</router-link>
    </div>

    <div class="search-section">
      <div class="search-bar">
        <font-awesome-icon icon="search" class="search-icon" />
        <input
          type="text"
          placeholder="What are you looking for?"
          class="search-input"
        />
      </div>

      <div class="category-dropdown">
        <label for="category" class="input-label">Category</label>
        <select id="category" class="input-field">
          <option value="">All Categories</option>
          <option value="tyres">Tyres</option>
          <option value="oil">Oil</option>
          <option value="batteries">Batteries</option>
          <option value="accessories">Accessories</option>
        </select>
      </div>
    </div>

    <div class="product-list">
      <div
        class="product-card"
        v-for="(product, index) in products"
        :key="index"
        @click="openQuantityModal(product)"
      >
        <img :src="product.image" :alt="product.brand" class="product-image" />
        <div class="product-info">
          <p class="product-id">Product #...</p>
          <p class="product-price">{{ product.price }}</p>
          <button class="insert-button">Insert &gt;&gt;&gt;</button>
        </div>
      </div>
    </div>

    <div v-if="showSuccessMessage" class="success-message">
      <div class="success-icon">✓</div>
      <p>Quantity Inserted Successfully</p>
    </div>

    <div v-if="showQuantityModal" class="quantity-modal">
      <div class="quantity-modal-content">
        <h3>Insert Quantity</h3>
        <div class="quantity-selector">
          <button @click="decreaseQuantity" class="quantity-btn">-</button>
          <span class="quantity-value">{{ quantity }}</span>
          <button @click="increaseQuantity" class="quantity-btn">+</button>
        </div>
        <button @click="addToInventory" class="btn-primary">
          Add to Inventory
        </button>
      </div>
    </div>

    <!-- Bottom Navigation -->
    <BottomNavigation />
  </div>
</template>

<script setup>
import { ref } from "vue";
import BottomNavigation from "../components/BottomNavigation.vue";

const showQuantityModal = ref(false);
const showSuccessMessage = ref(false);
const selectedProduct = ref(null);
const quantity = ref(1);

const products = [
  {
    id: 1,
    brand: "Shell Helix",
    price: "RM300",
    image: "https://via.placeholder.com/80",
  },
  {
    id: 2,
    brand: "Mobil",
    price: "RM50",
    image: "https://via.placeholder.com/80",
  },
  {
    id: 3,
    brand: "Castrol Edge",
    price: "RM50",
    image: "https://via.placeholder.com/80",
  },
  {
    id: 4,
    brand: "Titan",
    price: "RM50",
    image: "https://via.placeholder.com/80",
  },
];

const openQuantityModal = (product) => {
  selectedProduct.value = product;
  showQuantityModal.value = true;
  quantity.value = 1;
};

const increaseQuantity = () => {
  quantity.value++;
};

const decreaseQuantity = () => {
  if (quantity.value > 1) {
    quantity.value--;
  }
};

const addToInventory = () => {
  showQuantityModal.value = false;
  showSuccessMessage.value = true;

  // Hide success message after 3 seconds
  setTimeout(() => {
    showSuccessMessage.value = false;
  }, 3000);
};
</script>

<style scoped>
.product-list {
  margin-bottom: 20px;
}

.product-card {
  display: flex;
  background-color: var(--card-bg);
  border-radius: 10px;
  padding: 15px;
  margin-bottom: 15px;
  cursor: pointer;
}

.product-id {
  font-size: 14px;
  margin-bottom: 5px;
}

.insert-button {
  background: none;
  border: none;
  color: var(--primary-color);
  font-weight: 600;
  padding: 0;
  cursor: pointer;
  font-size: 14px;
  margin-top: 5px;
}

.quantity-modal {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background-color: rgba(0, 0, 0, 0.7);
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 1000;
}

.quantity-modal-content {
  background-color: var(--background-color);
  border-radius: 10px;
  padding: 20px;
  width: 90%;
  max-width: 350px;
}

.quantity-modal-content h3 {
  font-size: 20px;
  font-weight: 600;
  margin-bottom: 20px;
  text-align: center;
}

.quantity-selector {
  display: flex;
  align-items: center;
  justify-content: center;
  margin-bottom: 20px;
}

.quantity-btn {
  width: 40px;
  height: 40px;
  background-color: var(--card-bg);
  border: none;
  border-radius: 8px;
  font-size: 20px;
  color: white;
  display: flex;
  align-items: center;
  justify-content: center;
  cursor: pointer;
}

.quantity-value {
  width: 50px;
  text-align: center;
  font-size: 18px;
}

.success-message {
  position: fixed;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
  background-color: var(--background-color);
  border-radius: 10px;
  padding: 20px;
  display: flex;
  flex-direction: column;
  align-items: center;
  z-index: 1000;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.2);
}

.success-icon {
  width: 50px;
  height: 50px;
  background-color: rgba(0, 255, 0, 0.2);
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  color: #00ff00;
  font-size: 24px;
  margin-bottom: 10px;
}

.success-message p {
  font-size: 16px;
  color: #00ff00;
}
</style>
