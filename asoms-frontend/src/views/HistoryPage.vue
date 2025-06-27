<template>
  <div class="page-container">
    <div class="status-bar">
      <span>9:41</span>
      <div>
        <span>Π</span>
      </div>
    </div>

    <!-- Header -->
    <div class="header-nav">
      <div class="back-button" @click="goBack">
        <font-awesome-icon icon="arrow-left" class="back-icon" />
      </div>
      <div class="header-title">Order History</div>
    </div>

    <!-- Loading State -->
    <div v-if="loading" class="loading-container">
      <div class="loading-spinner"></div>
      <p>Loading order history...</p>
    </div>

    <!-- Error State -->
    <div v-else-if="error" class="error-container">
      <font-awesome-icon icon="exclamation-circle" class="error-icon" />
      <p>{{ errorMessage }}</p>
      <button @click="fetchOrders" class="btn-primary retry-btn">Retry</button>
    </div>

    <!-- Empty Orders State -->
    <div v-else-if="!orders.length" class="empty-orders-container">
      <div class="empty-orders-icon">
        <font-awesome-icon icon="receipt" />
      </div>
      <h3>No orders yet</h3>
      <p>You haven't placed any orders yet</p>
      <button @click="goToShop" class="btn-primary">Start Shopping</button>
    </div>

    <!-- Orders List -->
    <div v-else class="orders-content">
      <h3 class="section-title">Your Orders</h3>

      <div class="orders-list">
        <div
          v-for="order in orders"
          :key="order.id"
          class="order-card"
          @click="toggleOrderDetails(order.id)"
        >
          <div class="order-header">
            <div class="order-icon">
              <font-awesome-icon icon="shopping-bag" />
            </div>
            <div class="order-info">
              <div class="order-id">Order #{{ formatOrderId(order.id) }}</div>
              <div class="order-date">{{ formatDate(order.createdAt) }}</div>
            </div>
            <div class="order-status" :class="getStatusClass(order.status)">
              {{ order.status }}
            </div>
          </div>

          <div class="order-summary">
            <div class="order-payment">
              <span class="label">Payment:</span>
              <span class="value">{{ order.paymentMethod }}</span>
            </div>
            <div class="order-total">
              <span class="label">Total:</span>
              <span class="value">RM {{ formatPrice(order.totalAmount) }}</span>
            </div>
          </div>

          <!-- New section for notes and pickup time -->
          <div class="order-additional-info">
            <div v-if="order.notesToBuyer" class="order-notes">
              <span class="label">Notes From Seller:</span>
              <span class="value">{{ order.notesToBuyer }}</span>
            </div>
            <div v-if="order.pickupTime" class="order-pickup">
              <span class="label">Pickup Time:</span>
              <span class="value">{{
                formatPickupTime(order.pickupTime)
              }}</span>
            </div>
          </div>

          <!-- Order Details (Expandable) -->
          <div v-if="expandedOrderId === order.id" class="order-details">
            <div class="order-details-header">
              <h4>Order Items</h4>
            </div>

            <div class="order-items">
              <div
                v-for="(item, index) in order.items"
                :key="index"
                class="order-item"
              >
                <div class="item-image">
                  <img
                    :src="`https://asoms-production.up.railway.app${item.productImage}`"
                    :alt="item.productName"
                    @error="handleImageError($event)"
                  />
                </div>
                <div class="item-details">
                  <h4>{{ item.productName }}</h4>
                  <div class="item-price">
                    <p>
                      RM {{ formatPrice(item.price) }} × {{ item.quantity }}
                    </p>
                  </div>
                </div>
                <div class="item-total">
                  <p>RM {{ formatPrice(item.total) }}</p>
                </div>
              </div>
            </div>

            <!-- Additional order details -->
            <div class="order-details-additional">
              <div v-if="order.notesToBuyer" class="detail-row">
                <span class="detail-label">Notes to Buyer:</span>
                <span class="detail-value">{{ order.notesToBuyer }}</span>
              </div>
              <div v-if="order.pickupTime" class="detail-row">
                <span class="detail-label">Pickup Time:</span>
                <span class="detail-value">{{
                  formatPickupTime(order.pickupTime)
                }}</span>
              </div>
            </div>

            <div class="order-total-summary">
              <div class="total-row">
                <span>Total</span>
                <span>RM {{ formatPrice(order.totalAmount) }}</span>
              </div>
            </div>
          </div>

          <div class="order-footer">
            <font-awesome-icon
              :icon="
                expandedOrderId === order.id ? 'chevron-up' : 'chevron-down'
              "
              class="expand-icon"
            />
          </div>
        </div>
      </div>
    </div>

    <!-- Toast Notification -->
    <div v-if="showToast" class="toast" :class="{ show: showToast }">
      <div class="toast-icon">
        <font-awesome-icon :icon="toastIcon" />
      </div>
      <div class="toast-message">{{ toastMessage }}</div>
    </div>

    <!-- Bottom Navigation -->
    <div class="bottom-nav">
      <router-link to="/home" class="bottom-nav-item">
        <font-awesome-icon icon="home" class="bottom-nav-icon" />
        <span>Home</span>
      </router-link>
      <router-link to="/shop" class="bottom-nav-item active">
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
import { useRouter } from "vue-router";
import { useAuthStore } from "../stores/auth";
import axios from "axios";
import { FontAwesomeIcon } from "@fortawesome/vue-fontawesome";
import BottomNavigation from "../components/BottomNavigation.vue";

const router = useRouter();
const auth = useAuthStore();

// State variables
const loading = ref(true);
const error = ref(false);
const errorMessage = ref("");
const orders = ref([]);
const expandedOrderId = ref(null);
const showToast = ref(false);
const toastMessage = ref("");
const toastIcon = ref("info-circle");

// Fetch orders from API
const fetchOrders = async () => {
  loading.value = true;
  error.value = false;

  try {
    // Fetch orders from API
    const response = await axios.get(
      `https://asoms-production.up.railway.app/api/orders/user/${auth.user?.id}`,
      {
        headers: {
          Authorization: `Bearer ${auth.token}`,
          "Content-Type": "application/json",
        },
      }
    );

    orders.value = response.data;
    loading.value = false;
  } catch (err) {
    console.error("Failed to fetch orders:", err);
    error.value = true;
    errorMessage.value = "Failed to load your order history. Please try again.";
    loading.value = false;
  }
};

const formatOrderId = (id) => {
  return id ? String(id).substring(0, 8).toUpperCase() : "N/A";
};

// Toggle order details expansion
const toggleOrderDetails = (orderId) => {
  if (expandedOrderId.value === orderId) {
    expandedOrderId.value = null;
  } else {
    expandedOrderId.value = orderId;
  }
};

// Format date to readable format
const formatDate = (dateString) => {
  const options = { year: "numeric", month: "short", day: "numeric" };
  return new Date(dateString).toLocaleDateString(undefined, options);
};

// Format pickup time to readable format
const formatPickupTime = (timeString) => {
  const options = {
    year: "numeric",
    month: "short",
    day: "numeric",
    hour: "2-digit",
    minute: "2-digit",
  };
  return new Date(timeString).toLocaleDateString(undefined, options);
};

// Format price to 2 decimal places
const formatPrice = (price) => {
  return parseFloat(price).toFixed(2);
};

// Get CSS class based on order status
const getStatusClass = (status) => {
  switch (status.toLowerCase()) {
    case "confirmed":
      return "status-confirmed";
    case "pending approval":
      return "status-processing";
    case "completed":
      return "status-completed";
    case "cancelled":
      return "status-cancelled";
    default:
      return "";
  }
};

// Handle image loading errors
const handleImageError = (event) => {
  event.target.src = "/images/product-placeholder.png";
};

// Show toast message
const showToastMessage = (message, icon = "info-circle") => {
  toastMessage.value = message;
  toastIcon.value = icon;
  showToast.value = true;

  // Hide toast after 3 seconds
  setTimeout(() => {
    showToast.value = false;
  }, 3000);
};

// Navigation
const goBack = () => {
  router.back();
};

const goToShop = () => {
  router.push("/shop");
};

console.log("Auth: ", auth.user);

console.log("logged in: ", auth.isLoggedIn);

// Lifecycle hooks
import { watch } from "vue";

watch(
  () => auth.user,
  (newUser) => {
    if (newUser && auth.isLoggedIn) {
      fetchOrders();
    } else {
      error.value = true;
      errorMessage.value = "Please log in to view your order history.";
      loading.value = false;
    }
  },
  { immediate: true } // ← runs once right away
);
</script>

<style scoped>
/* General Styles */
.page-container {
  position: relative;
  padding-bottom: 80px;
}

/* Header with Back Button */
.header-nav {
  display: flex;
  align-items: center;
  padding: 15px;
  margin-top: 30px;
  position: relative;
}

.back-button {
  display: flex;
  align-items: center;
  justify-content: center;
  background-color: rgba(255, 255, 255, 0.15);
  width: 40px;
  height: 40px;
  border-radius: 50%;
  font-size: 18px;
  cursor: pointer;
  transition: all 0.2s ease;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.2);
}

.back-icon {
  color: white;
}

.back-button:hover {
  background-color: rgba(255, 75, 38, 0.6);
  transform: translateY(-2px);
}

.back-button:active {
  transform: translateY(0);
}

.header-title {
  position: absolute;
  left: 50%;
  transform: translateX(-50%);
  font-size: 18px;
  font-weight: 600;
}

/* Loading and Error States */
.loading-container,
.error-container,
.empty-orders-container {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  height: 50vh;
  padding: 0 20px;
  text-align: center;
}

.loading-spinner {
  width: 30px;
  height: 30px;
  border: 3px solid rgba(255, 255, 255, 0.3);
  border-radius: 50%;
  border-top: 3px solid white;
  animation: spin 1s linear infinite;
  margin-bottom: 20px;
}

@keyframes spin {
  0% {
    transform: rotate(0deg);
  }
  100% {
    transform: rotate(360deg);
  }
}

.error-icon,
.empty-orders-icon {
  font-size: 50px;
  color: #ff4b26;
  margin-bottom: 20px;
}

.empty-orders-icon {
  color: #cccccc;
}

.retry-btn {
  margin-top: 20px;
  max-width: 200px;
}

/* Orders Content */
.orders-content {
  padding: 0 15px;
  margin-top: 10px;
  margin-bottom: 20px;
}

.section-title {
  font-size: 18px;
  font-weight: 600;
  margin-bottom: 15px;
}

/* Order Cards */
.orders-list {
  display: flex;
  flex-direction: column;
  gap: 15px;
}

.order-card {
  background-color: var(--card-bg);
  border-radius: 10px;
  overflow: hidden;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
  cursor: pointer;
  transition: transform 0.2s ease, box-shadow 0.2s ease;
}

.order-card:hover {
  transform: translateY(-2px);
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.2);
}

.order-header {
  display: flex;
  align-items: center;
  padding: 15px;
  border-bottom: 1px solid rgba(255, 255, 255, 0.1);
}

.order-icon {
  width: 40px;
  height: 40px;
  background-color: rgba(255, 75, 38, 0.2);
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 18px;
  color: var(--primary-color);
  margin-right: 15px;
}

.order-info {
  flex: 1;
}

.order-id {
  font-size: 16px;
  font-weight: 600;
  margin-bottom: 4px;
}

.order-date {
  font-size: 14px;
  color: #cccccc;
}

.order-status {
  padding: 6px 12px;
  border-radius: 20px;
  font-size: 12px;
  font-weight: 600;
  text-transform: uppercase;
}

.status-confirmed {
  background-color: rgba(33, 150, 243, 0.2);
  color: #2196f3;
}

.status-processing {
  background-color: rgba(255, 152, 0, 0.2);
  color: #ff9800;
}

.status-completed {
  background-color: rgba(76, 175, 80, 0.2);
  color: #4caf50;
}

.status-cancelled {
  background-color: rgba(244, 67, 54, 0.2);
  color: #f44336;
}

.order-summary {
  display: flex;
  justify-content: space-between;
  padding: 15px;
  border-bottom: 1px solid rgba(255, 255, 255, 0.1);
}

.order-payment,
.order-total {
  display: flex;
  flex-direction: column;
}

.label {
  font-size: 12px;
  color: #cccccc;
  margin-bottom: 4px;
}

.value {
  font-size: 14px;
  font-weight: 500;
}

/* New styles for additional order info */
.order-additional-info {
  padding: 15px;
  border-bottom: 1px solid rgba(255, 255, 255, 0.1);
  display: flex;
  flex-direction: column;
  gap: 10px;
}

.order-notes,
.order-pickup {
  display: flex;
  flex-direction: column;
}

.order-notes .value {
  color: #e0e0e0;
  font-style: italic;
}

.order-pickup .value {
  color: var(--primary-color);
  font-weight: 600;
}

.order-footer {
  display: flex;
  justify-content: center;
  padding: 10px;
}

.expand-icon {
  color: #cccccc;
  transition: transform 0.3s ease;
}

/* Order Details */
.order-details {
  padding: 15px;
  border-top: 1px solid rgba(255, 255, 255, 0.1);
  background-color: rgba(0, 0, 0, 0.2);
}

.order-details-header {
  margin-bottom: 15px;
}

.order-details-header h4 {
  font-size: 16px;
  font-weight: 600;
  margin: 0;
}

.order-items {
  display: flex;
  flex-direction: column;
  gap: 10px;
}

.order-item {
  display: flex;
  padding: 10px;
  background-color: rgba(255, 255, 255, 0.05);
  border-radius: 8px;
}

.item-image {
  width: 50px;
  height: 50px;
  background-color: rgba(255, 255, 255, 0.1);
  border-radius: 6px;
  overflow: hidden;
  margin-right: 15px;
  display: flex;
  align-items: center;
  justify-content: center;
}

.item-image img {
  max-width: 100%;
  max-height: 100%;
  object-fit: contain;
}

.item-details {
  flex: 1;
}

.item-details h4 {
  font-size: 14px;
  font-weight: 500;
  margin: 0 0 5px 0;
}

.item-price {
  font-size: 12px;
  color: #cccccc;
}

.item-total {
  display: flex;
  align-items: center;
  font-size: 14px;
  font-weight: 600;
  color: var(--primary-color);
}

/* New styles for additional details in expanded view */
.order-details-additional {
  margin-top: 15px;
  padding-top: 15px;
  border-top: 1px solid rgba(255, 255, 255, 0.1);
  display: flex;
  flex-direction: column;
  gap: 10px;
}

.detail-row {
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
}

.detail-label {
  font-size: 14px;
  color: #cccccc;
  font-weight: 500;
  min-width: 120px;
}

.detail-value {
  font-size: 14px;
  color: #ffffff;
  text-align: right;
  flex: 1;
  margin-left: 10px;
}

.order-total-summary {
  margin-top: 15px;
  padding-top: 15px;
  border-top: 1px solid rgba(255, 255, 255, 0.1);
}

.total-row {
  display: flex;
  justify-content: space-between;
  font-size: 16px;
  font-weight: 600;
  color: var(--primary-color);
}

/* Toast Notification */
.toast {
  position: fixed;
  bottom: 100px;
  left: 50%;
  transform: translateX(-50%) translateY(100px);
  background-color: #333333;
  color: white;
  padding: 15px 20px;
  border-radius: 10px;
  display: flex;
  align-items: center;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.2);
  transition: transform 0.3s ease;
  z-index: 1000;
  min-width: 250px;
}

.toast.show {
  transform: translateX(-50%) translateY(0);
}

.toast-icon {
  margin-right: 10px;
  font-size: 20px;
  color: var(--primary-color);
}

.toast-message {
  font-size: 14px;
}

/* Add this to your existing styles */
select.form-input option {
  color: black;
  background-color: white;
}
</style>
