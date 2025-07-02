<template>
  <div class="page-container">
    <!-- <div class="status-bar">
      <span>9:41</span>
      <div>
        <span>Π</span>
      </div>
    </div> -->

    <!-- Fixed Back Button -->
    <div class="header-nav">
      <div class="back-button" @click="goBack">
        <font-awesome-icon icon="arrow-left" class="back-icon" />
      </div>
      <div class="header-title">Product Details</div>
    </div>

    <!-- Loading State -->
    <div v-if="loading" class="loading-container">
      <div class="loading-spinner"></div>
      <p>Loading product...</p>
    </div>

    <!-- Error State -->
    <div v-else-if="error" class="error-container">
      <font-awesome-icon icon="exclamation-circle" class="error-icon" />
      <p>{{ errorMessage }}</p>
      <button @click="fetchProduct" class="btn-primary retry-btn">Retry</button>
    </div>

    <!-- Product Detail -->
    <div v-else-if="product" class="product-detail">
      <!-- Product Image Gallery -->
      <div class="product-image-gallery">
        <div class="main-image-container">
          <img
            :src="fullImageUrl"
            :alt="product.name"
            class="product-detail-image"
            @error="handleImageError"
          />
          <div v-if="isOnSale" class="sale-badge">SALE</div>
        </div>

        <!-- Thumbnail Images (if available) -->
        <div v-if="hasMultipleImages" class="image-thumbnails">
          <div
            v-for="(image, index) in productImages"
            :key="index"
            class="thumbnail-container"
            :class="{ active: currentImageIndex === index }"
            @click="selectImage(index)"
          >
            <img
              :src="image"
              :alt="`${product.name} view ${index + 1}`"
              class="thumbnail-image"
            />
          </div>
        </div>
      </div>

      <!-- Product Info -->
      <div class="product-info">
        <!-- Product Header - Removed favorite button -->
        <h2 class="product-detail-name">{{ product.name }}</h2>

        <p class="product-detail-description">{{ product.description }}</p>

        <div class="price-container">
          <p class="product-detail-price">
            RM {{ formatPrice(product.price) }}
          </p>
          <p v-if="isOnSale" class="original-price">
            RM {{ formatPrice(product.originalPrice) }}
          </p>
        </div>

        <!-- Product Specifications -->
        <div class="product-specifications">
          <h3>Specifications</h3>
          <div class="specs-grid">
            <div class="spec-item">
              <span class="spec-label">Brand</span>
              <span class="spec-value">{{ product.brand || "N/A" }}</span>
            </div>
            <div class="spec-item">
              <span class="spec-label">Category</span>
              <span class="spec-value">{{ product.category || "N/A" }}</span>
            </div>
            <div class="spec-item">
              <span class="spec-label">Size</span>
              <span class="spec-value">{{ product.size || "N/A" }}</span>
            </div>
            <div class="spec-item">
              <span class="spec-label">In Stock</span>
              <span class="spec-value" :class="{ 'low-stock': isLowStock }">
                {{ product.quantity }} pcs
              </span>
            </div>
          </div>
        </div>

        <!-- Size Selection (if applicable) -->
        <div v-if="hasSizes" class="size-selection">
          <h3>Select Size</h3>
          <div class="size-options">
            <button
              v-for="size in availableSizes"
              :key="size"
              class="size-option"
              :class="{ active: selectedSize === size }"
              @click="selectSize(size)"
            >
              {{ size }}
            </button>
          </div>
        </div>

        <!-- Improved Quantity Selector -->
        <div class="quantity-section">
          <h3>Quantity</h3>
          <div class="quantity-selector">
            <button
              @click="decreaseQuantity"
              class="quantity-btn minus-btn"
              :disabled="quantity <= 1"
            >
              <span class="quantity-btn-text">−</span>
            </button>
            <span class="quantity-value">{{ quantity }}</span>
            <button
              @click="increaseQuantity"
              class="quantity-btn plus-btn"
              :disabled="quantity >= product.quantity"
            >
              <span class="quantity-btn-text">+</span>
            </button>
          </div>
        </div>

        <!-- Add to Cart Button -->
        <button
          @click="addToCart"
          class="btn-primary add-to-cart-btn"
          :disabled="!canAddToCart"
        >
          <font-awesome-icon icon="shopping-cart" class="btn-icon" />
          Add to cart
        </button>

        <!-- Out of Stock Message -->
        <p v-if="product.quantity <= 0" class="out-of-stock-message">
          <font-awesome-icon icon="exclamation-triangle" /> This product is
          currently out of stock
        </p>
      </div>
    </div>

    <!-- Related Products -->
    <div v-if="product && relatedProducts.length > 0" class="related-products">
      <h3>You might also like</h3>
      <div class="related-products-grid">
        <div
          v-for="relatedProduct in relatedProducts"
          :key="relatedProduct.id"
          class="related-product-card"
          @click="navigateToProduct(relatedProduct.id)"
        >
          <div class="related-product-image">
            <img
              :src="`https://asoms-production.up.railway.app${relatedProduct.imageUrl}`"
              :alt="relatedProduct.name"
            />
          </div>
          <div class="related-product-info">
            <h4>{{ relatedProduct.name }}</h4>
            <p>RM {{ formatPrice(relatedProduct.price) }}</p>
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
    <BottomNavigation />
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from "vue";
import { useRoute, useRouter } from "vue-router";
import { useAuthStore } from "../stores/auth";
import { useCartStore } from "../stores/useCartStore";
import axios from "axios";
import BottomNavigation from "../components/BottomNavigation.vue";

const route = useRoute();
const router = useRouter();
const auth = useAuthStore();
const cartStore = useCartStore();

// State variables
const product = ref(null);
const fullImageUrl = ref("");
const fallbackImageUrl = "/images/product-placeholder.png";
const loading = ref(true);
const error = ref(false);
const errorMessage = ref("");
const quantity = ref(1);
const selectedSize = ref(null);
const currentImageIndex = ref(0);
const relatedProducts = ref([]);
const showToast = ref(false);
const toastMessage = ref("");
const toastIcon = ref("check-circle");

// Computed properties
const userName = computed(() => auth.user?.fullName || "Guest");

const isOnSale = computed(() => {
  return (
    product.value &&
    product.value.originalPrice &&
    product.value.price < product.value.originalPrice
  );
});

const isLowStock = computed(() => {
  return (
    product.value && product.value.quantity <= 5 && product.value.quantity > 0
  );
});

const canAddToCart = computed(() => {
  return (
    product.value &&
    product.value.quantity > 0 &&
    (!hasSizes.value || selectedSize.value)
  );
});
console.log("can? ", canAddToCart);
const hasSizes = computed(() => {
  return product.value && product.value.sizes && product.value.sizes.length > 0;
});

const availableSizes = computed(() => {
  return hasSizes.value ? product.value.sizes : [];
});

const hasMultipleImages = computed(() => {
  return productImages.value.length > 1;
});

const productImages = computed(() => {
  if (!product.value) return [fallbackImageUrl];

  // If product has additional images, use them
  if (
    product.value.additionalImages &&
    product.value.additionalImages.length > 0
  ) {
    return [
      `https://asoms-production.up.railway.app${product.value.imageUrl}`,
      ...product.value.additionalImages.map(
        (img) => `https://asoms-production.up.railway.app${img}`
      ),
    ];
  }

  // Otherwise just return the main image
  return [`https://asoms-production.up.railway.app${product.value.imageUrl}`];
});

// Methods
const fetchProduct = async () => {
  loading.value = true;
  error.value = false;

  try {
    const id = route.params.id;
    console.log("Fetching product ID:", id);

    const response = await axios.get(
      `https://asoms-production.up.railway.app/api/product/${id}`
    );
    product.value = response.data;

    // Set the full image URL
    fullImageUrl.value = `https://asoms-production.up.railway.app${response.data.imageUrl}`;

    // If product has sizes, select the first one by default
    if (hasSizes.value) {
      selectedSize.value = availableSizes.value[0];
    }

    // Fetch related products
    // fetchRelatedProducts()

    loading.value = false;
  } catch (err) {
    console.error("Failed to fetch product:", err);
    error.value = true;
    errorMessage.value = "Failed to load product. Please try again.";
    loading.value = false;
  }
};

// const fetchRelatedProducts = async () => {
//   if (!product.value) return

//   try {
//     // Fetch related products based on category
//     const response = await axios.get(`https://asoms-production.up.railway.app/api/product/category/${product.value.category}`)

//     // Filter out the current product and limit to 4 related products
//     relatedProducts.value = response.data
//       .filter(p => p.id !== product.value.id)
//       .slice(0, 4)
//   } catch (err) {
//     console.error('Failed to fetch related products:', err)
//     // Don't show error for related products, just leave the section empty
//     relatedProducts.value = []
//   }
// }

const handleImageError = () => {
  // If the image fails to load, use a fallback image
  fullImageUrl.value = fallbackImageUrl;
};

const increaseQuantity = () => {
  if (product.value && quantity.value < product.value.quantity) {
    quantity.value++;
  }
};

const decreaseQuantity = () => {
  if (quantity.value > 1) {
    quantity.value--;
  }
};

const selectSize = (size) => {
  selectedSize.value = size;
};

const selectImage = (index) => {
  currentImageIndex.value = index;
  fullImageUrl.value = productImages.value[index];
};

const addToCart = () => {
  if (!canAddToCart.value) return;

  // Create the cart item
  const cartItem = {
    id: product.value.id,
    name: product.value.name,
    price: product.value.price,
    imageUrl: product.value.imageUrl,
    quantity: quantity.value,
    size: selectedSize.value,
  };

  // Add to cart using the cart store
  cartStore.addToCart(cartItem);

  // Show success toast
  // showToastMessage('Added to bag successfully', 'shopping-bag')

  // Reset quantity
  quantity.value = 1;
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

const navigateToProduct = (productId) => {
  router.push(`/product/${productId}`);
};

const goBack = () => {
  router.back();
};

const cartTotal = computed(() => {
  return cartStore.items.reduce(
    (total, item) => total + item.price * item.quantity,
    0
  );
});

const goToCheckout = () => {
  router.push("/checkout");
};

// Lifecycle hooks
onMounted(() => {
  fetchProduct();
});
</script>

<style scoped>
/* General Styles */
.page-container {
  position: relative;
  padding-bottom: 80px;
}

/* Improved Header with Back Button */
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
.error-container {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  height: 50vh;
  padding: 0 20px;
  text-align: center;
}

.loading-spinner {
  width: 50px;
  height: 50px;
  border: 4px solid rgba(255, 75, 38, 0.1);
  border-radius: 50%;
  border-top: 4px solid var(--primary-color);
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

.error-icon {
  font-size: 50px;
  color: #ff4b26;
  margin-bottom: 20px;
}

.retry-btn {
  margin-top: 20px;
  max-width: 200px;
}

/* Product Detail */
.product-detail {
  padding: 0 15px;
  margin-top: 10px;
  margin-bottom: 20px;
}

/* Product Image Gallery */
.product-image-gallery {
  margin-bottom: 20px;
}

.main-image-container {
  position: relative;
  width: 100%;
  height: 300px;
  background-color: var(--card-bg);
  border-radius: 15px;
  display: flex;
  justify-content: center;
  align-items: center;
  overflow: hidden;
  margin-bottom: 10px;
}

.product-detail-image {
  max-width: 100%;
  max-height: 100%;
  object-fit: contain;
  padding: 20px;
}

.sale-badge {
  position: absolute;
  top: 10px;
  right: 10px;
  background-color: var(--primary-color);
  color: white;
  padding: 5px 10px;
  border-radius: 5px;
  font-size: 12px;
  font-weight: bold;
}

.image-thumbnails {
  display: flex;
  gap: 10px;
  overflow-x: auto;
  padding-bottom: 10px;
}

.thumbnail-container {
  width: 60px;
  height: 60px;
  border-radius: 10px;
  overflow: hidden;
  background-color: var(--card-bg);
  cursor: pointer;
  border: 2px solid transparent;
  transition: border-color 0.2s ease;
}

.thumbnail-container.active {
  border-color: var(--primary-color);
}

.thumbnail-image {
  width: 100%;
  height: 100%;
  object-fit: cover;
}

/* Product Info */
.product-info {
  margin-bottom: 30px;
}

.product-detail-name {
  font-size: 24px;
  font-weight: 700;
  margin-bottom: 15px;
}

.product-detail-description {
  font-size: 16px;
  color: #cccccc;
  margin-bottom: 15px;
  line-height: 1.5;
}

.price-container {
  display: flex;
  align-items: center;
  margin-bottom: 20px;
}

.product-detail-price {
  font-size: 24px;
  font-weight: 700;
  color: var(--primary-color);
  margin: 0;
  margin-right: 10px;
}

.original-price {
  font-size: 18px;
  color: #cccccc;
  text-decoration: line-through;
  margin: 0;
}

/* Product Specifications */
.product-specifications {
  background-color: var(--card-bg);
  border-radius: 15px;
  padding: 15px;
  margin-bottom: 20px;
}

.product-specifications h3 {
  font-size: 18px;
  font-weight: 600;
  margin-bottom: 15px;
}

.specs-grid {
  display: grid;
  grid-template-columns: repeat(2, 1fr);
  gap: 15px;
}

.spec-item {
  display: flex;
  flex-direction: column;
}

.spec-label {
  font-size: 14px;
  color: #cccccc;
  margin-bottom: 5px;
}

.spec-value {
  font-size: 16px;
  font-weight: 500;
}

.low-stock {
  color: #ffa500;
}

/* Size Selection */
.size-selection {
  margin-bottom: 20px;
}

.size-selection h3 {
  font-size: 18px;
  font-weight: 600;
  margin-bottom: 15px;
}

.size-options {
  display: flex;
  flex-wrap: wrap;
  gap: 10px;
}

.size-option {
  width: 50px;
  height: 50px;
  display: flex;
  align-items: center;
  justify-content: center;
  background-color: var(--card-bg);
  border: 2px solid transparent;
  border-radius: 10px;
  font-size: 16px;
  font-weight: 500;
  cursor: pointer;
  transition: all 0.2s ease;
}

.size-option.active {
  border-color: var(--primary-color);
  color: var(--primary-color);
}

/* Improved Quantity Section */
.quantity-section {
  margin-bottom: 20px;
}

.quantity-section h3 {
  font-size: 18px;
  font-weight: 600;
  margin-bottom: 15px;
}

.quantity-selector {
  display: flex;
  align-items: center;
  background-color: var(--card-bg);
  border-radius: 10px;
  padding: 5px;
  width: fit-content;
}

/* Improved Quantity Buttons */
.quantity-btn {
  width: 40px;
  height: 40px;
  border: none;
  border-radius: 8px;
  display: flex;
  align-items: center;
  justify-content: center;
  cursor: pointer;
  transition: background-color 0.2s ease;
  position: relative;
}

.quantity-btn-text {
  font-size: 24px;
  font-weight: bold;
  line-height: 1;
  color: white;
}

.minus-btn {
  background-color: rgba(255, 75, 38, 0.2);
}

.plus-btn {
  background-color: rgba(255, 75, 38, 0.2);
}

.quantity-btn:hover:not(:disabled) {
  background-color: rgba(255, 75, 38, 0.4);
}

.quantity-btn:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}

.quantity-value {
  width: 50px;
  text-align: center;
  font-size: 18px;
  font-weight: 500;
}

/* Add to Cart Button */
.add-to-cart-btn {
  width: 100%;
  padding: 15px;
  font-size: 16px;
  font-weight: 600;
  display: flex;
  align-items: center;
  justify-content: center;
  margin-bottom: 10px;
}

.add-to-cart-btn:disabled {
  background-color: #666666;
  cursor: not-allowed;
}

.btn-icon {
  margin-right: 10px;
}

.out-of-stock-message {
  color: #ff4b26;
  text-align: center;
  font-size: 14px;
  margin-top: 10px;
}

/* Related Products */
.related-products {
  padding: 0 15px;
  margin-bottom: 30px;
}

.related-products h3 {
  font-size: 20px;
  font-weight: 600;
  margin-bottom: 15px;
}

.related-products-grid {
  display: grid;
  grid-template-columns: repeat(2, 1fr);
  gap: 15px;
}

.related-product-card {
  background-color: var(--card-bg);
  border-radius: 10px;
  overflow: hidden;
  cursor: pointer;
  transition: transform 0.2s ease;
}

.related-product-card:hover {
  transform: translateY(-5px);
}

.related-product-image {
  height: 120px;
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 10px;
}

.related-product-image img {
  max-width: 100%;
  max-height: 100%;
  object-fit: contain;
}

.related-product-info {
  padding: 10px;
}

.related-product-info h4 {
  font-size: 14px;
  font-weight: 500;
  margin: 0 0 5px 0;
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
}

.related-product-info p {
  font-size: 14px;
  font-weight: 600;
  color: var(--primary-color);
  margin: 0;
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

/* Floating Cart Button */
.floating-cart-button {
  position: fixed;
  bottom: 90px;
  left: 50%;
  transform: translateX(-50%);
  background-color: var(--primary-color);
  color: white;
  display: flex;
  align-items: center;
  padding: 12px 20px;
  border-radius: 30px;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.2);
  cursor: pointer;
  z-index: 90;
  min-width: 200px;
}

.cart-icon {
  position: relative;
  font-size: 20px;
  margin-right: 15px;
}

.cart-badge {
  position: absolute;
  top: -8px;
  right: -8px;
  background-color: white;
  color: var(--primary-color);
  font-size: 10px;
  font-weight: bold;
  width: 18px;
  height: 18px;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
}

.cart-text {
  display: flex;
  flex-direction: column;
}

.cart-total {
  font-weight: 600;
}
</style>
