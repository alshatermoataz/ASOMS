<template>
  <div class="page-container">
    <!-- <div class="status-bar">
      <span>9:41</span>
      <div>
        <span>Π</span>
      </div>
    </div> -->

    <!-- Header with Back Button -->
    <div class="header-nav">
      <div class="back-button" @click="goBack">
        <font-awesome-icon icon="arrow-left" class="back-icon" />
      </div>
      <div class="header-title">Place Order</div>
    </div>

    <!-- Loading State -->
    <div v-if="loading" class="loading-container">
      <div class="loading-spinner"></div>
      <p>Loading checkout...</p>
    </div>

    <!-- Error State -->
    <div v-else-if="error" class="error-container">
      <font-awesome-icon icon="exclamation-circle" class="error-icon" />
      <p>{{ errorMessage }}</p>
      <button @click="initCheckout" class="btn-primary retry-btn">Retry</button>
    </div>

    <!-- Empty Cart State -->
    <div v-else-if="!cartItems.length" class="empty-cart-container">
      <div class="empty-cart-icon">
        <font-awesome-icon icon="shopping-cart" />
      </div>
      <h3>Your cart is empty</h3>
      <p>Add some items to your cart to place an order</p>
      <button @click="goToShop" class="btn-primary">Continue Shopping</button>
    </div>

    <!-- Checkout Content -->
    <div v-else class="checkout-content">
      <!-- Order Items with Quantity Controls -->
      <div class="checkout-section" data-aos="fade-up" data-aos-delay="100">
        <h3 class="section-title">
          <div class="section-icon-wrapper">
            <font-awesome-icon icon="shopping-cart" class="section-icon" />
          </div>
          Order Items ({{ cartItems.length }})
        </h3>
        <div class="order-items">
          <div
            v-for="(item, index) in cartItems"
            :key="index"
            class="order-item"
            :data-aos-delay="150 + index * 50"
          >
            <div class="item-image">
              <img
                :src="`https://asoms-production.up.railway.app${item.imageUrl}`"
                :alt="item.name"
                @error="handleImageError($event)"
              />
            </div>
            <div class="item-details">
              <h4>{{ item.name }}</h4>
              <p v-if="item.size" class="item-size">Size: {{ item.size }}</p>
              <div class="item-price">
                <p>RM {{ formatPrice(item.price) }}</p>
              </div>

              <!-- Quantity Controls -->
              <div class="quantity-controls">
                <button
                  @click="decreaseQuantity(item)"
                  class="quantity-btn minus-btn"
                >
                  <span class="quantity-btn-text">−</span>
                </button>
                <span class="quantity-value">{{ item.quantity }}</span>
                <button
                  @click="increaseQuantity(item)"
                  class="quantity-btn plus-btn"
                  :disabled="item.quantity >= item.availableQuantity"
                  :class="{ disabled: item.quantity >= item.availableQuantity }"
                >
                  <span class="quantity-btn-text">+</span>
                </button>
                <span
                  v-if="item.quantity >= item.availableQuantity"
                  class="stock-limit"
                >
                  Max available
                </span>
              </div>
            </div>
            <div class="item-total">
              <p>RM {{ formatPrice(item.price * item.quantity) }}</p>
            </div>
          </div>
        </div>
      </div>

      <!-- Pickup Information -->
      <div class="checkout-section" data-aos="fade-up" data-aos-delay="200">
        <h3 class="section-title">
          <div class="section-icon-wrapper">
            <font-awesome-icon icon="map-marker-alt" class="section-icon" />
          </div>
          Pickup Information
        </h3>
        <div class="pickup-container">
          <div class="form-group">
            <label for="pickupName">Full Name</label>
            <input
              type="text"
              id="pickupName"
              v-model="pickupInfo.name"
              class="form-input"
              placeholder="Your full name"
            />
          </div>

          <div class="form-group">
            <label for="pickupPhone">Phone Number</label>
            <div class="phone-input-container">
              <div class="country-code">
                <div class="flag">🇾🇪</div>
                <span>+967</span>
              </div>
              <input
                type="tel"
                id="pickupPhone"
                v-model="pickupInfo.phoneNumber"
                class="form-input phone-input"
                placeholder="Your phone number"
              />
            </div>
          </div>

          <div class="form-group">
            <label for="pickupDate">Pickup Date</label>
            <input
              type="date"
              id="pickupDate"
              v-model="pickupInfo.date"
              class="form-input"
              :min="minPickupDate"
            />
          </div>

          <div class="form-group">
            <label for="pickupTime">Pickup Time</label>
            <select
              id="pickupTime"
              v-model="pickupInfo.time"
              class="form-input"
            >
              <option value="">Select a time</option>
              <option v-for="time in pickupTimes" :key="time" :value="time">
                {{ time }}
              </option>
            </select>
          </div>

          <div class="form-group">
            <label for="pickupNotes">Special Instructions (Optional)</label>
            <textarea
              id="pickupNotes"
              v-model="pickupInfo.notes"
              class="form-textarea"
              placeholder="Any special instructions for your order"
              rows="3"
            ></textarea>
          </div>
        </div>
      </div>

      <!-- Payment Method Selection -->
      <div class="checkout-section" data-aos="fade-up" data-aos-delay="300">
        <h3 class="section-title">
          <div class="section-icon-wrapper">
            <font-awesome-icon icon="credit-card" class="section-icon" />
          </div>
          Payment Method
        </h3>
        <div class="payment-container">
          <div class="payment-options">
            <div
              class="payment-option"
              :class="{ selected: paymentMethod === 'cash' }"
              @click="selectPaymentMethod('cash')"
            >
              <div class="payment-icon">
                <font-awesome-icon icon="money-bill-wave" />
              </div>
              <div class="payment-details">
                <h4>Cash on Pickup</h4>
                <p>Pay with cash when you collect your order</p>
              </div>
              <div class="payment-radio">
                <div class="radio-wrapper">
                  <input type="radio" v-model="paymentMethod" value="cash" />
                  <span class="radio-checkmark"></span>
                </div>
              </div>
            </div>

            <div
              class="payment-option"
              :class="{ selected: paymentMethod === 'invoice' }"
              @click="selectPaymentMethod('invoice')"
            >
              <div class="payment-icon">
                <font-awesome-icon icon="file-invoice" />
              </div>
              <div class="payment-details">
                <h4>Invoice</h4>
                <p>Receive an invoice for payment processing</p>
              </div>
              <div class="payment-radio">
                <div class="radio-wrapper">
                  <input type="radio" v-model="paymentMethod" value="invoice" />
                  <span class="radio-checkmark"></span>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>

      <!-- Order Summary -->
      <div class="checkout-section" data-aos="fade-up" data-aos-delay="400">
        <h3 class="section-title">
          <div class="section-icon-wrapper">
            <font-awesome-icon icon="receipt" class="section-icon" />
          </div>
          Order Summary
        </h3>
        <div class="summary-container">
          <div class="summary-row">
            <span>Subtotal</span>
            <span>RM {{ formatPrice(subtotal) }}</span>
          </div>
          <div class="summary-row total">
            <span>Total ({{ paymentMethodLabel }})</span>
            <span>RM {{ formatPrice(total) }}</span>
          </div>
        </div>
      </div>

      <!-- Terms and Conditions -->
      <div class="terms-container" data-aos="fade-up" data-aos-delay="500">
        <div class="checkbox-container">
          <div class="custom-checkbox">
            <input type="checkbox" id="termsCheck" v-model="termsAccepted" />
            <span class="checkmark"></span>
          </div>
          <label for="termsCheck">I agree to the terms and conditions</label>
        </div>
        <p class="terms-text">
          By placing this order, you agree to {{ paymentTermsText }}. Orders
          must be picked up on the selected date and time. Unclaimed orders may
          be cancelled after 24 hours.
        </p>
      </div>

      <!-- Place Order Button -->
      <button
        @click="placeOrder"
        class="btn-primary place-order-btn"
        :disabled="!canPlaceOrder || processingOrder"
        data-aos="fade-up"
        data-aos-delay="600"
      >
        <span v-if="processingOrder" class="processing-content">
          <div class="btn-spinner"></div>
          <span class="processing-text">Processing...</span>
        </span>
        <span v-else class="order-content">
          <font-awesome-icon icon="shopping-bag" class="order-icon" />
          <span>Place Order</span>
        </span>
      </button>
    </div>

    <!-- Confirmation Modal -->
    <div v-if="showConfirmModal" class="modal-overlay">
      <div class="modal-container">
        <div class="modal-header">
          <h3>Remove Item</h3>
          <button @click="cancelRemove" class="close-btn">
            <font-awesome-icon icon="times" />
          </button>
        </div>
        <div class="modal-content">
          <p>Are you sure you want to remove this item from your cart?</p>
          <p>
            <strong>{{ itemToRemove?.name }}</strong>
          </p>
        </div>
        <div class="modal-footer">
          <button @click="cancelRemove" class="btn-text">Cancel</button>
          <button @click="confirmRemove" class="btn-danger">Remove</button>
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
  </div>
</template>

<script setup>
import { ref, computed, onMounted, watch } from "vue";
import { useRouter } from "vue-router";
import { useAuthStore } from "../stores/auth";
import { useCartStore } from "../stores/useCartStore";
import { useProductStore } from "../stores/useProductStore";
import axios from "axios";
import { FontAwesomeIcon } from "@fortawesome/vue-fontawesome";

const router = useRouter();
const auth = useAuthStore();
const cartStore = useCartStore();
const productStore = useProductStore();

// State variables
const loading = ref(true);
const error = ref(false);
const errorMessage = ref("");
const processingOrder = ref(false);
const showToast = ref(false);
const toastMessage = ref("");
const toastIcon = ref("check-circle");
const termsAccepted = ref(false);
const showConfirmModal = ref(false);
const itemToRemove = ref(null);
const productQuantityMap = ref({}); // Cache product quantity info
const paymentMethod = ref("cash"); // Default payment method

// Pickup information
const pickupInfo = ref({
  name: "",
  phoneNumber: "", // Changed from phone to phoneNumber for clarity
  countryCode: "+967", // Added country code
  date: "",
  time: "",
  notes: "",
});

// Computed property for full phone number with country code
const fullPhoneNumber = computed(() => {
  return pickupInfo.value.phoneNumber
    ? `${pickupInfo.value.countryCode}${pickupInfo.value.phoneNumber}`
    : "";
});

// Pickup times (9am to 6pm, hourly)
const pickupTimes = ref([
  "09:00 AM",
  "10:00 AM",
  "11:00 AM",
  "12:00 PM",
  "01:00 PM",
  "02:00 PM",
  "03:00 PM",
  "04:00 PM",
  "05:00 PM",
  "06:00 PM",
]);

// Computed properties
const cartItems = computed(() => {
  return cartStore.items.map((item) => {
    const product = productStore.getProductById(item.id);
    const availableQuantity = product
      ? product.quantity
      : productQuantityMap.value[item.id] || 10;

    return {
      ...item,
      availableQuantity,
    };
  });
});

const subtotal = computed(() => {
  return cartItems.value.reduce(
    (total, item) => total + item.price * item.quantity,
    0
  );
});

// Total is the same as subtotal (no taxes)
const total = computed(() => {
  return subtotal.value;
});

const paymentMethodLabel = computed(() => {
  return paymentMethod.value === "cash" ? "Cash on Pickup" : "Invoice";
});

const paymentTermsText = computed(() => {
  return paymentMethod.value === "cash"
    ? "pay in cash upon pickup"
    : "receive and process the invoice for payment";
});

const canPlaceOrder = computed(() => {
  return (
    pickupInfo.value.name &&
    pickupInfo.value.phoneNumber &&
    pickupInfo.value.date &&
    pickupInfo.value.time &&
    paymentMethod.value &&
    cartItems.value.length > 0 &&
    termsAccepted.value
  );
});

const minPickupDate = computed(() => {
  const today = new Date();
  return today.toISOString().split("T")[0];
});

// Methods
const initCheckout = async () => {
  loading.value = true;
  error.value = false;

  try {
    // Pre-fill pickup info with user data if available
    if (auth.isLoggedIn && auth.user) {
      pickupInfo.value.name = auth.user.fullName || "";

      // Handle phone number - strip country code if present
      if (auth.user.contactNumber) {
        const phoneNumber = auth.user.contactNumber;
        console.log("Phone number ", phoneNumber);
        if (phoneNumber.startsWith("+967")) {
          pickupInfo.value.phoneNumber = phoneNumber.substring(4);
        } else {
          pickupInfo.value.phoneNumber = phoneNumber;
        }
      }
    }

    // Set default pickup date to tomorrow
    const tomorrow = new Date();
    tomorrow.setDate(tomorrow.getDate() + 1);
    pickupInfo.value.date = tomorrow.toISOString().split("T")[0];

    // Make sure product store is loaded to get quantity information
    await productStore.fetchProducts(true); // Force refresh to ensure we have latest quantity

    // Cache quantity information for cart items
    cartStore.items.forEach((item) => {
      const product = productStore.getProductById(item.id);
      if (product) {
        productQuantityMap.value[item.id] = product.quantity;
      }
    });

    loading.value = false;
  } catch (err) {
    console.error("Failed to initialize checkout:", err);
    error.value = true;
    errorMessage.value =
      "Failed to load checkout information. Please try again.";
    loading.value = false;
  }
};

// Payment method selection
const selectPaymentMethod = (method) => {
  paymentMethod.value = method;
  showToastMessage(
    `Payment method set to ${method === "cash" ? "Cash on Pickup" : "Invoice"}`,
    "check-circle"
  );
};

// Cart item quantity controls
const increaseQuantity = (item) => {
  // Get the available quantity directly from the product store
  const availableQuantity = productStore.getAvailableQuantity(item.id);

  if (availableQuantity === 0) {
    showToastMessage("This item is out of stock", "exclamation-circle");
    return;
  }

  if (item.quantity < availableQuantity) {
    cartStore.updateQuantity(item.id, item.quantity + 1, item.size);
    showToastMessage("Quantity updated", "check-circle");
  } else {
    showToastMessage(
      `Only ${availableQuantity} available`,
      "exclamation-circle"
    );
  }
};

const decreaseQuantity = (item) => {
  if (item.quantity > 1) {
    cartStore.updateQuantity(item.id, item.quantity - 1, item.size);
    showToastMessage("Quantity updated", "check-circle");
  } else {
    // If quantity would become 0, show confirmation modal
    itemToRemove.value = item;
    showConfirmModal.value = true;
  }
};

const confirmRemove = () => {
  if (itemToRemove.value) {
    cartStore.removeFromCart(itemToRemove.value.id, itemToRemove.value.size);
    showToastMessage("Item removed from cart", "trash");
    showConfirmModal.value = false;
    itemToRemove.value = null;
  }
};

const cancelRemove = () => {
  showConfirmModal.value = false;
  itemToRemove.value = null;
  // No need to set quantity back to 1 as we're not changing it when showing the modal
};

const placeOrder = async () => {
  if (!canPlaceOrder.value) return;

  processingOrder.value = true;
  console.log("PickUpDate: ", pickupInfo.value.time);
  try {
    // Combine selected date and selected time
    const fullPickupDateTime = `${pickupInfo.value.date} ${pickupInfo.value.time}`;
    // Example: "2024-06-10 01:00 PM"

    const orderDto = {
      userId: auth.user?.id,
      paymentMethod: paymentMethod.value, // e.g., "Cash" or "Invoice"
      notesToSeller: pickupInfo.value.notes || "", // from buyer
      pickupTime: fullPickupDateTime, // ensure it's an ISO string
      items: cartItems.value.map((item) => ({
        productId: item.id,
        quantity: item.quantity,
      })),
    };

    // Send order to the API
    const response = await axios.post(
      "https://asoms-production.up.railway.app/api/orders",
      orderDto,
      {
        headers: {
          Authorization: `Bearer ${auth.token}`,
        },
      }
    );

    const orderId = response.data.id;

    // Clear the cart
    cartStore.clearCart();

    // Success
    showToastMessage("Order placed successfully!", "check-circle");

    setTimeout(() => {
      router.push(`/history`);
    }, 1500);
  } catch (err) {
    console.error("Failed to place order:", err.response?.data || err.message);
    showToastMessage(
      "Failed to place order. Please try again.",
      "times-circle"
    );
  } finally {
    processingOrder.value = false;
  }
};

const showToastMessage = (message, icon = "check-circle") => {
  toastMessage.value = message;
  toastIcon.value = icon;
  showToast.value = true;

  // Hide toast after 3 seconds
  setTimeout(() => {
    showToast.value = false;
  }, 3000);
};

const formatPrice = (price) => {
  return parseFloat(price).toFixed(2);
};

const handleImageError = (event) => {
  event.target.src = "/images/product-placeholder.png";
};

const goBack = () => {
  router.back();
};

const goToShop = () => {
  router.push("/shop");
};

// Watch for auth changes to update pickup info
watch(
  () => auth.user,
  (newUser) => {
    if (newUser) {
      pickupInfo.value.name = newUser.fullName || pickupInfo.value.name;

      // Handle phone number - strip country code if present
      if (newUser.contactNumber) {
        const phoneNumber = newUser.contactNumber;
        if (phoneNumber.startsWith("+967")) {
          pickupInfo.value.phoneNumber = phoneNumber.substring(4);
        } else {
          pickupInfo.value.phoneNumber = phoneNumber;
        }
      }
    }
  },
  { immediate: true }
);

// Lifecycle hooks
onMounted(() => {
  initCheckout();
});
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
  transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
  box-shadow: 0 4px 15px rgba(0, 0, 0, 0.2);
  backdrop-filter: blur(10px);
  border: 1px solid rgba(255, 255, 255, 0.1);
}

.back-icon {
  color: white;
  transition: transform 0.3s ease;
}

.back-button:hover {
  background-color: rgba(255, 75, 38, 0.6);
  transform: translateY(-3px) scale(1.05);
  box-shadow: 0 8px 25px rgba(255, 75, 38, 0.4);
}

.back-button:hover .back-icon {
  transform: translateX(-2px);
}

.back-button:active {
  transform: translateY(-1px) scale(1.02);
}

.header-title {
  position: absolute;
  left: 50%;
  transform: translateX(-50%);
  font-size: 18px;
  font-weight: 600;
  color: white;
  text-shadow: 0 2px 4px rgba(0, 0, 0, 0.3);
}

/* Loading and Error States */
.loading-container,
.error-container,
.empty-cart-container {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  height: 50vh;
  padding: 0 20px;
  text-align: center;
}

.loading-spinner,
.btn-spinner {
  width: 30px;
  height: 30px;
  border: 3px solid rgba(255, 255, 255, 0.3);
  border-radius: 50%;
  border-top: 3px solid var(--primary-color);
  animation: spin 1s linear infinite;
  margin-bottom: 20px;
}

.btn-spinner {
  width: 20px;
  height: 20px;
  margin: 0 10px 0 0;
  display: inline-block;
  vertical-align: middle;
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
.empty-cart-icon {
  font-size: 50px;
  color: #ff4b26;
  margin-bottom: 20px;
}

.empty-cart-icon {
  color: #cccccc;
}

.retry-btn {
  margin-top: 20px;
  max-width: 200px;
}

/* Checkout Content */
.checkout-content {
  padding: 0 15px;
  margin-top: 10px;
  margin-bottom: 20px;
}

/* Enhanced Section Styles */
.checkout-section {
  margin-bottom: 25px;
  background-color: var(--card-bg);
  border-radius: 15px;
  padding: 20px;
  box-shadow: 0 4px 20px rgba(0, 0, 0, 0.1);
  transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
  position: relative;
  overflow: hidden;
}

.checkout-section::before {
  content: "";
  position: absolute;
  top: 0;
  left: -100%;
  width: 100%;
  height: 2px;
  background: linear-gradient(
    90deg,
    transparent,
    var(--primary-color),
    transparent
  );
  transition: left 0.6s ease;
}

.checkout-section:hover::before {
  left: 100%;
}

.checkout-section:hover {
  transform: translateY(-2px);
  box-shadow: 0 8px 30px rgba(0, 0, 0, 0.15);
}

.section-title {
  font-size: 18px;
  font-weight: 600;
  margin-bottom: 15px;
  display: flex;
  align-items: center;
  color: white;
}

.section-icon-wrapper {
  width: 35px;
  height: 35px;
  background: linear-gradient(135deg, var(--primary-color), #ff6b3d);
  border-radius: 10px;
  display: flex;
  align-items: center;
  justify-content: center;
  margin-right: 12px;
  box-shadow: 0 4px 15px rgba(255, 75, 38, 0.3);
  transition: all 0.3s ease;
}

.section-icon {
  color: white;
  font-size: 16px;
}

.section-title:hover .section-icon-wrapper {
  transform: rotate(5deg) scale(1.1);
}

/* Form Styles */
.pickup-container {
  background-color: transparent;
  border-radius: 0;
  padding: 0;
}

.form-group {
  margin-bottom: 20px;
  position: relative;
}

.form-group label {
  display: block;
  font-size: 14px;
  margin-bottom: 8px;
  color: #ddd;
  font-weight: 500;
  transition: color 0.3s ease;
}

.form-input,
.form-textarea {
  width: 100%;
  padding: 15px;
  background-color: rgba(255, 255, 255, 0.1);
  border: 2px solid rgba(255, 255, 255, 0.1);
  border-radius: 10px;
  color: white;
  font-size: 14px;
  transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
  position: relative;
}

.form-input:focus,
.form-textarea:focus {
  outline: none;
  border-color: var(--primary-color);
  box-shadow: 0 0 0 3px rgba(255, 75, 38, 0.1);
  background-color: rgba(255, 255, 255, 0.15);
  transform: translateY(-1px);
}

.form-input:focus + label,
.form-textarea:focus + label {
  color: var(--primary-color);
}

.form-input::placeholder,
.form-textarea::placeholder {
  color: rgba(255, 255, 255, 0.5);
}

/* Phone Input with Country Code */
.phone-input-container {
  display: flex;
  align-items: center;
}

.country-code {
  display: flex;
  align-items: center;
  background-color: rgba(255, 255, 255, 0.1);
  border: 2px solid rgba(255, 255, 255, 0.1);
  border-radius: 10px 0 0 10px;
  padding: 0 15px;
  height: 50px;
  color: white;
  font-size: 14px;
  border-right: none;
  transition: all 0.3s ease;
}

.flag {
  margin-right: 6px;
  font-size: 16px;
}

.phone-input {
  border-radius: 0 10px 10px 0;
  flex: 1;
}

.form-textarea {
  resize: vertical;
  min-height: 80px;
}

/* Payment Method Styles */
.payment-container {
  background-color: transparent;
}

.payment-options {
  display: flex;
  flex-direction: column;
  gap: 15px;
}

.payment-option {
  display: flex;
  align-items: center;
  padding: 20px;
  background-color: rgba(255, 255, 255, 0.05);
  border: 2px solid rgba(255, 255, 255, 0.1);
  border-radius: 12px;
  cursor: pointer;
  transition: all 0.4s cubic-bezier(0.4, 0, 0.2, 1);
  position: relative;
  overflow: hidden;
}

.payment-option::before {
  content: "";
  position: absolute;
  top: 0;
  left: -100%;
  width: 100%;
  height: 100%;
  background: linear-gradient(
    90deg,
    transparent,
    rgba(255, 75, 38, 0.1),
    transparent
  );
  transition: left 0.6s ease;
}

.payment-option:hover::before {
  left: 100%;
}

.payment-option:hover {
  border-color: rgba(255, 75, 38, 0.3);
  transform: translateY(-3px) scale(1.02);
  box-shadow: 0 10px 30px rgba(255, 75, 38, 0.2);
}

.payment-option.selected {
  border-color: var(--primary-color);
  background-color: rgba(255, 75, 38, 0.1);
  box-shadow: 0 6px 25px rgba(255, 75, 38, 0.25);
}

.payment-icon {
  width: 50px;
  height: 50px;
  background: linear-gradient(135deg, var(--primary-color), #ff6b3d);
  border-radius: 12px;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 20px;
  color: white;
  margin-right: 15px;
  box-shadow: 0 4px 15px rgba(255, 75, 38, 0.3);
  transition: all 0.3s ease;
}

.payment-option:hover .payment-icon {
  transform: scale(1.1) rotate(5deg);
}

.payment-details {
  flex: 1;
}

.payment-details h4 {
  font-size: 16px;
  font-weight: 600;
  margin: 0 0 5px 0;
  color: white;
}

.payment-details p {
  font-size: 14px;
  color: #cccccc;
  margin: 0;
  line-height: 1.4;
}

.payment-radio {
  margin-left: 15px;
}

.radio-wrapper {
  position: relative;
  display: inline-block;
}

.radio-wrapper input[type="radio"] {
  opacity: 0;
  /* position: absolute; */
  width: 20px;
  height: 20px;
  cursor: pointer;
}

.radio-checkmark {
  position: absolute;
  top: 0;
  left: 0;
  height: 20px;
  width: 20px;
  background-color: rgba(255, 255, 255, 0.1);
  border: 2px solid rgba(255, 255, 255, 0.3);
  border-radius: 50%;
  transition: all 0.3s ease;
}

.radio-wrapper input[type="radio"]:checked ~ .radio-checkmark {
  background-color: var(--primary-color);
  border-color: var(--primary-color);
}

.radio-checkmark:after {
  content: "";
  position: absolute;
  display: none;
  top: 50%;
  left: 50%;
  width: 6px;
  height: 6px;
  border-radius: 50%;
  background: white;
  transform: translate(-50%, -50%);
}

.radio-wrapper input[type="radio"]:checked ~ .radio-checkmark:after {
  display: block;
}

/* Order Items */
.order-items {
  background-color: rgba(255, 255, 255, 0.02);
  border-radius: 12px;
  padding: 15px;
  border: 1px solid rgba(255, 255, 255, 0.05);
}

.order-item {
  display: flex;
  padding: 15px;
  border-bottom: 1px solid rgba(255, 255, 255, 0.08);
  border-radius: 8px;
  margin-bottom: 10px;
  background: rgba(255, 255, 255, 0.02);
  transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
}

.order-item:hover {
  background: rgba(255, 255, 255, 0.05);
  transform: translateX(8px);
  box-shadow: 0 4px 15px rgba(0, 0, 0, 0.1);
}

.order-item:last-child {
  border-bottom: none;
  margin-bottom: 0;
}

.item-image {
  width: 60px;
  height: 60px;
  background-color: rgba(255, 255, 255, 0.05);
  border-radius: 8px;
  overflow: hidden;
  margin-right: 15px;
  display: flex;
  align-items: center;
  justify-content: center;
  transition: transform 0.3s ease;
}

.order-item:hover .item-image {
  transform: scale(1.05);
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
  font-size: 16px;
  font-weight: 500;
  margin: 0 0 5px 0;
  color: white;
}

.item-size {
  font-size: 14px;
  color: #cccccc;
  margin: 0 0 5px 0;
}

.item-price {
  font-size: 14px;
  font-weight: 500;
  margin: 0 0 10px 0;
  color: #eee;
}

/* Quantity Controls */
.quantity-controls {
  display: flex;
  align-items: center;
  position: relative;
}

.quantity-btn {
  width: 32px;
  height: 32px;
  border: none;
  border-radius: 8px;
  display: flex;
  align-items: center;
  justify-content: center;
  cursor: pointer;
  transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
  background-color: rgba(255, 75, 38, 0.2);
  box-shadow: 0 2px 8px rgba(255, 75, 38, 0.2);
}

.quantity-btn-text {
  font-size: 18px;
  font-weight: bold;
  line-height: 1;
  color: white;
}

.quantity-btn:hover:not(:disabled) {
  background-color: rgba(255, 75, 38, 0.4);
  transform: scale(1.1);
  box-shadow: 0 4px 15px rgba(255, 75, 38, 0.3);
}

.quantity-btn:active:not(:disabled) {
  transform: scale(0.95);
}

.quantity-btn:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}

.quantity-btn.disabled {
  opacity: 0.5;
  cursor: not-allowed;
}

.quantity-value {
  width: 40px;
  text-align: center;
  font-size: 16px;
  font-weight: 500;
  color: white;
  margin: 0 8px;
}

.stock-limit {
  position: absolute;
  bottom: -20px;
  left: 0;
  font-size: 12px;
  color: #ff9800;
}

.item-total {
  display: flex;
  align-items: center;
  font-size: 16px;
  font-weight: 600;
  color: var(--primary-color);
  min-width: 80px;
  justify-content: flex-end;
}

/* Order Summary */
.summary-container {
  background-color: rgba(255, 255, 255, 0.02);
  border-radius: 12px;
  padding: 20px;
  border: 1px solid rgba(255, 255, 255, 0.05);
}

.summary-row {
  display: flex;
  justify-content: space-between;
  padding: 12px 0;
  border-bottom: 1px solid rgba(255, 255, 255, 0.1);
  color: #eee;
  transition: all 0.3s ease;
}

.summary-row:hover {
  color: white;
  transform: translateX(5px);
}

.summary-row:last-child {
  border-bottom: none;
}

.summary-row.total {
  font-size: 20px;
  font-weight: 700;
  color: var(--primary-color);
  padding-top: 15px;
  border-top: 2px solid var(--primary-color);
  margin-top: 10px;
}

/* Terms and Conditions */
.terms-container {
  background-color: var(--card-bg);
  border-radius: 12px;
  padding: 20px;
  margin-bottom: 25px;
  transition: all 0.3s ease;
}

.terms-container:hover {
  transform: translateY(-1px);
  box-shadow: 0 6px 20px rgba(0, 0, 0, 0.1);
}

.checkbox-container {
  display: flex;
  align-items: center;
  margin-bottom: 15px;
}

.custom-checkbox {
  position: relative;
  margin-right: 12px;
}

.custom-checkbox input[type="checkbox"] {
  opacity: 0;
  /* position: absolute; */
  width: 20px;
  height: 20px;
  cursor: pointer;
}

.checkmark {
  position: absolute;
  top: 0;
  left: 0;
  height: 20px;
  width: 20px;
  background-color: rgba(255, 255, 255, 0.1);
  border: 2px solid rgba(255, 255, 255, 0.3);
  border-radius: 4px;
  transition: all 0.3s ease;
}

.custom-checkbox input[type="checkbox"]:checked ~ .checkmark {
  background-color: var(--primary-color);
  border-color: var(--primary-color);
}

.checkmark:after {
  content: "";
  position: absolute;
  display: none;
  left: 6px;
  top: 2px;
  width: 6px;
  height: 10px;
  border: solid white;
  border-width: 0 2px 2px 0;
  transform: rotate(45deg);
}

.custom-checkbox input[type="checkbox"]:checked ~ .checkmark:after {
  display: block;
}

.checkbox-container label {
  font-size: 14px;
  font-weight: 500;
  color: #ddd;
  cursor: pointer;
  transition: color 0.3s ease;
}

.checkbox-container:hover label {
  color: white;
}

.terms-text {
  font-size: 12px;
  color: #cccccc;
  margin: 0;
  line-height: 1.6;
}

/* Place Order Button */
.place-order-btn {
  width: 100%;
  padding: 18px;
  font-size: 16px;
  font-weight: 600;
  margin-top: 20px;
  display: flex;
  align-items: center;
  justify-content: center;
  background: linear-gradient(135deg, var(--primary-color), #ff6b3d);
  border: none;
  border-radius: 12px;
  color: white;
  cursor: pointer;
  transition: all 0.4s cubic-bezier(0.4, 0, 0.2, 1);
  box-shadow: 0 6px 20px rgba(255, 75, 38, 0.3);
  text-transform: uppercase;
  letter-spacing: 1px;
  position: relative;
  overflow: hidden;
}

.place-order-btn::before {
  content: "";
  position: absolute;
  top: 0;
  left: -100%;
  width: 100%;
  height: 100%;
  background: linear-gradient(
    90deg,
    transparent,
    rgba(255, 255, 255, 0.2),
    transparent
  );
  transition: left 0.6s ease;
}

.place-order-btn:hover:not(:disabled)::before {
  left: 100%;
}

.place-order-btn:hover:not(:disabled) {
  transform: translateY(-3px) scale(1.02);
  box-shadow: 0 12px 35px rgba(255, 75, 38, 0.4);
}

.place-order-btn:active:not(:disabled) {
  transform: translateY(-1px) scale(1.01);
}

.place-order-btn:disabled {
  background: linear-gradient(135deg, #666, #555);
  cursor: not-allowed;
  box-shadow: none;
  transform: none;
}

.processing-content,
.order-content {
  display: flex;
  align-items: center;
  justify-content: center;
}

.processing-text {
  margin-left: 8px;
}

.order-icon {
  margin-right: 8px;
  transition: transform 0.3s ease;
}

.place-order-btn:hover:not(:disabled) .order-icon {
  transform: scale(1.2);
}

/* Confirmation Modal */
.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background-color: rgba(0, 0, 0, 0.7);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 1000;
  backdrop-filter: blur(5px);
  animation: fadeIn 0.3s ease;
}

@keyframes fadeIn {
  from {
    opacity: 0;
  }
  to {
    opacity: 1;
  }
}

.modal-container {
  width: 90%;
  max-width: 400px;
  background-color: #1a1a1a;
  border-radius: 15px;
  overflow: hidden;
  border: 1px solid rgba(255, 255, 255, 0.1);
  box-shadow: 0 10px 30px rgba(0, 0, 0, 0.5);
  animation: slideUp 0.3s ease;
}

@keyframes slideUp {
  from {
    opacity: 0;
    transform: translateY(30px) scale(0.9);
  }
  to {
    opacity: 1;
    transform: translateY(0) scale(1);
  }
}

.modal-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 15px;
  border-bottom: 1px solid rgba(255, 255, 255, 0.1);
}

.modal-header h3 {
  margin: 0;
  font-size: 18px;
  font-weight: 600;
  color: white;
}

.close-btn {
  background: none;
  border: none;
  color: white;
  font-size: 18px;
  cursor: pointer;
  transition: all 0.3s ease;
  padding: 5px;
  border-radius: 50%;
}

.close-btn:hover {
  color: var(--primary-color);
  background-color: rgba(255, 75, 38, 0.1);
  transform: rotate(90deg);
}

.modal-content {
  padding: 20px 15px;
}

.modal-content p {
  margin: 0 0 10px 0;
  line-height: 1.5;
  color: #ddd;
}

.modal-footer {
  display: flex;
  justify-content: flex-end;
  gap: 10px;
  padding: 15px;
  border-top: 1px solid rgba(255, 255, 255, 0.1);
}

.btn-text {
  background: none;
  border: none;
  color: var(--primary-color);
  cursor: pointer;
  font-size: 14px;
  padding: 8px 12px;
  transition: all 0.3s ease;
  border-radius: 5px;
}

.btn-text:hover {
  background: rgba(255, 75, 38, 0.1);
}

.btn-danger {
  background: linear-gradient(135deg, #f44336, #d32f2f);
  border: none;
  color: white;
  padding: 10px 15px;
  border-radius: 5px;
  cursor: pointer;
  font-size: 14px;
  transition: all 0.3s ease;
}

.btn-danger:hover {
  background: linear-gradient(135deg, #d32f2f, #b71c1c);
  transform: translateY(-1px);
  box-shadow: 0 4px 15px rgba(244, 67, 54, 0.3);
}

/* Toast Notification */
.toast {
  position: fixed;
  bottom: 100px;
  left: 50%;
  transform: translateX(-50%) translateY(100px);
  background: linear-gradient(135deg, #333333, #444444);
  color: white;
  padding: 15px 20px;
  border-radius: 10px;
  display: flex;
  align-items: center;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.2);
  transition: all 0.4s cubic-bezier(0.4, 0, 0.2, 1);
  z-index: 1000;
  min-width: 250px;
  backdrop-filter: blur(10px);
  border: 1px solid rgba(255, 255, 255, 0.1);
}

.toast.show {
  transform: translateX(-50%) translateY(0);
}

.toast-icon {
  margin-right: 10px;
  font-size: 20px;
  color: var(--primary-color);
  animation: pulse 2s infinite;
}

@keyframes pulse {
  0% {
    transform: scale(1);
  }
  50% {
    transform: scale(1.1);
  }
  100% {
    transform: scale(1);
  }
}

.toast-message {
  font-size: 14px;
}

/* Add this to your existing styles */
select.form-input option {
  color: black;
  background-color: white;
}

/* Responsive Design */
@media (max-width: 768px) {
  .checkout-section {
    margin-bottom: 20px;
    padding: 15px;
  }

  .payment-option {
    padding: 15px;
  }

  .payment-icon {
    width: 40px;
    height: 40px;
    font-size: 18px;
  }

  .place-order-btn {
    padding: 15px;
    font-size: 15px;
  }
}
</style>
