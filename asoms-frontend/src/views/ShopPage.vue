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
          <h2 class="user-name">{{ user?.fullName || "User" }}</h2>
          <p class="subtitle">Find what you need</p>
        </div>
      </div>
      <div class="header-actions">
        <!-- <button class="cart-btn" aria-label="Shopping Cart">
          <font-awesome-icon icon="shopping-cart" />
          <span class="cart-badge">{{ cartStore.totalItems }}</span>
        </button> -->
        <button
          class="filter-toggle-btn"
          @click="toggleFilters"
          aria-label="Toggle Filters"
        >
          <font-awesome-icon icon="sliders-h" />
        </button>
      </div>
    </div>

    <!-- Enhanced Navigation -->
    <div class="top-nav">
      <router-link to="/home" class="top-nav-item">
        <font-awesome-icon icon="home" />
        <span>Home</span>
      </router-link>
      <router-link to="/shop" class="top-nav-item active">
        <font-awesome-icon icon="shopping-cart" />
        <span>Shop</span>
      </router-link>
      <router-link to="/history" class="top-nav-item">
        <font-awesome-icon icon="history" />
        <span>History</span>
      </router-link>
    </div>

    <!-- Enhanced Search Section -->
    <div class="search-section">
      <div class="search-container">
        <div class="search-bar">
          <font-awesome-icon icon="search" class="search-icon" />
          <input
            type="text"
            v-model="searchQuery"
            placeholder="Search products, brands, or parts..."
            class="search-input"
            @input="debouncedSearch"
          />
          <button
            v-if="searchQuery"
            @click="clearSearch"
            class="clear-search-btn"
          >
            <font-awesome-icon icon="times" />
          </button>
        </div>

        <div
          class="search-suggestions"
          v-if="showSuggestions && searchSuggestions.length > 0"
        >
          <div
            v-for="suggestion in searchSuggestions"
            :key="suggestion"
            class="suggestion-item"
            @click="selectSuggestion(suggestion)"
          >
            <font-awesome-icon icon="search" />
            <span>{{ suggestion }}</span>
          </div>
        </div>
      </div>

      <!-- Enhanced Filter Section -->
      <div
        class="filters-container"
        :class="{ 'filters-expanded': showFilters }"
      >
        <div class="filter-row">
          <div class="filter-group">
            <label class="filter-label">Category</label>
            <div class="category-filter-grid">
              <button
                v-for="category in categories"
                :key="category.name"
                :class="[
                  'category-filter-btn',
                  { active: selectedCategory === category.name },
                ]"
                @click="selectCategory(category.name)"
              >
                <div class="category-filter-icon" :class="category.colorClass">
                  <font-awesome-icon :icon="category.icon" />
                </div>
                <span>{{ category.name }}</span>
              </button>
            </div>
          </div>
        </div>

        <div class="filter-row">
          <div class="filter-group">
            <label class="filter-label">Price Range</label>
            <div class="price-filter-container">
              <div class="price-input-group">
                <input
                  type="number"
                  v-model="priceRange.min"
                  placeholder="Min"
                  class="price-input"
                  @input="applyFilters"
                />
                <span class="price-separator">-</span>
                <input
                  type="number"
                  v-model="priceRange.max"
                  placeholder="Max"
                  class="price-input"
                  @input="applyFilters"
                />
              </div>
              <div class="price-range-display">
                RM {{ priceRange.min || 0 }} - RM {{ priceRange.max || 1000 }}
              </div>
            </div>
          </div>

          <div class="filter-group">
            <label class="filter-label">Sort By</label>
            <select v-model="sortBy" @change="applyFilters" class="sort-select">
              <option value="newest">Newest First</option>
              <option value="price-low">Price: Low to High</option>
              <option value="price-high">Price: High to Low</option>
              <option value="popular">Most Popular</option>
            </select>
          </div>
        </div>

        <div class="filter-actions">
          <button @click="clearAllFilters" class="clear-filters-btn">
            <font-awesome-icon icon="refresh" />
            Clear All
          </button>
          <button @click="applyFilters" class="apply-filters-btn">
            <font-awesome-icon icon="check" />
            Apply Filters
          </button>
        </div>
      </div>
    </div>

    <!-- Results Summary -->
    <div class="results-summary">
      <div class="results-info">
        <h3 class="results-title">
          <font-awesome-icon icon="box-open" />
          {{ filteredProducts.length }} Products Found
        </h3>
        <p class="results-subtitle" v-if="selectedCategory">
          in {{ selectedCategory }} category
        </p>
      </div>

      <div class="view-toggle">
        <button
          :class="['view-btn', { active: viewMode === 'grid' }]"
          @click="viewMode = 'grid'"
        >
          <font-awesome-icon icon="th" />
        </button>
        <button
          :class="['view-btn', { active: viewMode === 'list' }]"
          @click="viewMode = 'list'"
        >
          <font-awesome-icon icon="list" />
        </button>
      </div>
    </div>

    <!-- Loading State -->
    <div v-if="loading" class="loading-container">
      <div class="loading-spinner"></div>
      <p class="loading-text">Loading products...</p>
    </div>

    <!-- Enhanced Product List -->
    <div v-else-if="filteredProducts.length > 0" class="products-container">
      <!-- Grid View -->
      <div v-if="viewMode === 'grid'" class="product-grid">
        <div
          class="product-card"
          v-for="product in paginatedProducts"
          :key="product.id"
          @click="goToProduct(product.id)"
        >
          <div class="product-image-container">
            <img
              :src="fullImageUrl(product.imageUrl) || placeholderImage"
              :alt="product.name"
              class="product-image"
            />
            <div class="product-badges">
              <span v-if="product.isNew" class="badge new-badge">
                <font-awesome-icon icon="star" />
                NEW
              </span>
              <span v-if="product.discount" class="badge discount-badge">
                -{{ product.discount }}%
              </span>
            </div>
            <div class="product-actions">
              <button
                class="action-btn quick-view-btn"
                @click.stop="quickView(product)"
              >
                <font-awesome-icon icon="eye" />
              </button>
            </div>
          </div>

          <div class="product-info">
            <div class="product-category">{{ product.category }}</div>
            <h4 class="product-name">{{ product.name }}</h4>

            <div class="price-container">
              <span class="current-price">RM {{ product.price }}</span>
              <span v-if="product.originalPrice" class="original-price">
                RM {{ product.originalPrice }}
              </span>
            </div>

            <div class="product-status-container">
              <div
                class="availability-indicator"
                :class="getAvailabilityClass(product.quantity)"
              >
                <font-awesome-icon
                  :icon="getAvailabilityIcon(product.quantity)"
                />
              </div>
              <span class="availability-text">
                {{ getAvailabilityText(product.quantity) }}
              </span>
            </div>

            <button
              class="add-to-cart-btn"
              @click.stop="addToCart(product)"
              :disabled="product.quantity <= 0"
            >
              <font-awesome-icon icon="shopping-cart" />
              Add to Cart
            </button>
          </div>
        </div>
      </div>

      <!-- List View -->
      <div v-else class="product-list">
        <div
          class="product-list-item"
          v-for="product in paginatedProducts"
          :key="product.id"
          @click="goToProduct(product.id)"
        >
          <div class="list-item-image">
            <img
              :src="fullImageUrl(product.imageUrl) || placeholderImage"
              :alt="product.name"
              class="product-image"
            />
          </div>

          <div class="list-item-content">
            <div class="list-item-header">
              <div class="product-category">{{ product.category }}</div>
              <div class="product-badges">
                <span v-if="product.isNew" class="badge new-badge">NEW</span>
              </div>
            </div>

            <h4 class="product-name">{{ product.name }}</h4>

            <div class="list-item-footer">
              <div class="price-container">
                <span class="current-price">RM {{ product.price }}</span>
                <span v-if="product.originalPrice" class="original-price">
                  RM {{ product.originalPrice }}
                </span>
              </div>

              <div class="list-item-actions">
                <button
                  class="add-to-cart-btn"
                  @click.stop="addToCart(product)"
                  :disabled="product.quantity <= 0"
                >
                  <font-awesome-icon icon="shopping-cart" />
                  Add to Cart
                </button>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- Empty State -->
    <div v-else class="empty-state">
      <div class="empty-state-icon">
        <font-awesome-icon icon="search" />
      </div>
      <h3 class="empty-state-title">No Products Found</h3>
      <p class="empty-state-description">
        Try adjusting your search terms or filters to find what you're looking
        for.
      </p>
      <button @click="clearAllFilters" class="empty-state-btn">
        <font-awesome-icon icon="refresh" />
        Clear All Filters
      </button>
    </div>

    <!-- Quick View Modal -->
    <div
      v-if="showQuickView"
      class="modal-overlay"
      @click.self="closeQuickView"
    >
      <div class="quick-view-modal">
        <div class="modal-header">
          <h3>Quick View</h3>
          <button @click="closeQuickView" class="close-btn">
            <font-awesome-icon icon="times" />
          </button>
        </div>
        <div class="modal-content" v-if="selectedProduct">
          <div class="modal-image">
            <img
              :src="fullImageUrl(selectedProduct.imageUrl)"
              :alt="selectedProduct.name"
            />
          </div>
          <div class="modal-info">
            <div class="product-category">{{ selectedProduct.category }}</div>
            <h4>{{ selectedProduct.name }}</h4>
            <div class="price-container">
              <span class="current-price">RM {{ selectedProduct.price }}</span>
            </div>
            <p class="product-description">
              {{ selectedProduct.description || "No description available." }}
            </p>
            <div class="modal-actions">
              <button
                @click="goToProduct(selectedProduct.id)"
                class="view-details-btn"
              >
                View Details
              </button>
              <button
                @click="addToCart(selectedProduct)"
                class="add-to-cart-btn"
                :disabled="selectedProduct.quantity <= 0"
              >
                <font-awesome-icon icon="shopping-cart" />
                Add to Cart
              </button>
            </div>
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

    <!-- Add pagination controls after the products container -->
    <div v-if="totalPages > 1" class="pagination-container">
      <div class="pagination-info">
        <span class="pagination-text">
          Showing {{ (currentPage - 1) * itemsPerPage + 1 }}-{{
            Math.min(currentPage * itemsPerPage, filteredProducts.length)
          }}
          of {{ filteredProducts.length }} products
        </span>
      </div>

      <div class="pagination-controls">
        <button
          @click="prevPage"
          :disabled="currentPage === 1"
          class="pagination-btn prev-btn"
        >
          <font-awesome-icon icon="chevron-left" />
          Previous
        </button>

        <div class="page-numbers">
          <button
            v-for="page in getVisiblePages"
            :key="page"
            @click="goToPage(page)"
            :class="['page-btn', { active: page === currentPage }]"
          >
            {{ page }}
          </button>
        </div>

        <button
          @click="nextPage"
          :disabled="currentPage === totalPages"
          class="pagination-btn next-btn"
        >
          Next
          <font-awesome-icon icon="chevron-right" />
        </button>
      </div>
    </div>

    <!-- Bottom Navigation -->
    <BottomNavigation />
  </div>
</template>

<script setup>
import { ref, onMounted, computed, watch } from "vue";
import { useRoute, useRouter } from "vue-router";
import axios from "axios";
import { useAuthStore } from "../stores/auth";
import { useCartStore } from "../stores/useCartStore";
import BottomNavigation from "../components/BottomNavigation.vue";

const route = useRoute();
const router = useRouter();
const auth = useAuthStore();
const cartStore = useCartStore();
const user = auth.user;

// Reactive data
const searchQuery = ref("");
const selectedCategory = ref("");
const products = ref([]);
const loading = ref(false);
const viewMode = ref("grid");
const showFilters = ref(false);
const showQuickView = ref(false);
const selectedProduct = ref(null);
const showSuggestions = ref(false);
const greeting = ref("");

// Toast notification
const showToast = ref(false);
const toastMessage = ref("");
const toastIcon = ref("check-circle");

// Enhanced user data
const profilePicture =
  auth.user?.profilePictureUrl || "https://i.ibb.co/nPRRbJ5/ghost-icon.png";

// Filter states
const priceRange = ref({ min: "", max: "" });
const sortBy = ref("newest");

// Search suggestions
const searchSuggestions = ref([
  "Engine Oil",
  "Brake Pads",
  "Air Filter",
  "Spark Plugs",
  "Tyres",
  "Battery",
]);

const placeholderImage = "https://via.placeholder.com/300x200?text=No+Image";

// Categories with icons (matching HomePage)
const categories = ref([
  { name: "Tyres", icon: "circle", colorClass: "tyres-color" },
  { name: "Oil", icon: "tint", colorClass: "oil-color" },
  { name: "Accessories", icon: "wrench", colorClass: "accessories-color" },
  { name: "Parts", icon: "cog", colorClass: "parts-color" },
  { name: "Batteries", icon: "battery-full", colorClass: "batteries-color" },
  { name: "Filters", icon: "filter", colorClass: "filters-color" },
]);

// Add these reactive variables
const currentPage = ref(1);
const itemsPerPage = ref(14);

// Methods
const fetchProducts = async () => {
  loading.value = true;
  try {
    const res = await axios.get(
      "https://asoms-production.up.railway.app/api/product?pageSize=100"
    );
    const productData = res.data.items || res.data || [];

    // Directly use product data without injecting random discount or isNew
    products.value = productData.sort(
      (a, b) => new Date(b.createdAt) - new Date(a.createdAt)
    );
  } catch (err) {
    console.error("Error fetching products:", err);
  } finally {
    loading.value = false;
  }
};

const fullImageUrl = (path) =>
  path?.startsWith("http")
    ? path
    : `https://asoms-production.up.railway.app${path}`;

const imageBase = (url) =>
  url?.startsWith("http")
    ? url
    : `https://asoms-production.up.railway.app${url}`;

const filteredProducts = computed(() => {
  let filtered = products.value.filter((p) => {
    const matchCategory =
      selectedCategory.value === "" ||
      p.category.toLowerCase() === selectedCategory.value.toLowerCase();
    const matchSearch = p.name
      .toLowerCase()
      .includes(searchQuery.value.toLowerCase());
    const matchPrice =
      (!priceRange.value.min || p.price >= priceRange.value.min) &&
      (!priceRange.value.max || p.price <= priceRange.value.max);

    return matchCategory && matchSearch && matchPrice;
  });

  // Apply sorting
  switch (sortBy.value) {
    case "price-low":
      filtered.sort((a, b) => a.price - b.price);
      break;
    case "price-high":
      filtered.sort((a, b) => b.price - a.price);
      break;
    case "popular":
      filtered.sort((a, b) => (b.reviews || 0) - (a.reviews || 0));
      break;
    default: // newest
      filtered.sort((a, b) => new Date(b.createdAt) - new Date(a.createdAt));
  }

  return filtered;
});

// Add pagination computed property
const paginatedProducts = computed(() => {
  const start = (currentPage.value - 1) * itemsPerPage.value;
  const end = start + itemsPerPage.value;
  return filteredProducts.value.slice(start, end);
});

const totalPages = computed(() => {
  return Math.ceil(filteredProducts.value.length / itemsPerPage.value);
});

const getVisiblePages = computed(() => {
  const pages = [];
  const maxVisible = 5;
  let start = Math.max(1, currentPage.value - Math.floor(maxVisible / 2));
  let end = Math.min(totalPages.value, start + maxVisible - 1);

  if (end - start + 1 < maxVisible) {
    start = Math.max(1, end - maxVisible + 1);
  }

  for (let i = start; i <= end; i++) {
    pages.push(i);
  }

  return pages;
});

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

const getAvailabilityText = (quantity) => {
  if (quantity > 10) return `${quantity} in stock`;
  if (quantity > 5) return `Only ${quantity} left`;
  return quantity > 0 ? `Last ${quantity} items` : "Out of stock";
};

const toggleFilters = () => {
  showFilters.value = !showFilters.value;
};

const selectCategory = (category) => {
  selectedCategory.value = selectedCategory.value === category ? "" : category;
  resetPagination();
  applyFilters();
};

const clearSearch = () => {
  searchQuery.value = "";
  showSuggestions.value = false;
};

const selectSuggestion = (suggestion) => {
  searchQuery.value = suggestion;
  showSuggestions.value = false;
  applyFilters();
};

const clearAllFilters = () => {
  searchQuery.value = "";
  selectedCategory.value = "";
  priceRange.value = { min: "", max: "" };
  sortBy.value = "newest";
  showSuggestions.value = false;
  resetPagination();
};

const applyFilters = () => {
  showSuggestions.value = false;
  resetPagination();
};

const goToProduct = (productId) => {
  router.push(`/product/${productId}`);
};

// Add to cart functionality (from ProductDetailPage)
const addToCart = (product) => {
  if (product.quantity <= 0) return;

  // Create the cart item (same structure as ProductDetailPage)
  const cartItem = {
    id: product.id,
    name: product.name,
    price: product.price,
    imageUrl: product.imageUrl,
    quantity: 1, // Default quantity for shop page
    size: null, // No size selection in shop page
  };

  // Add to cart using the cart store
  cartStore.addToCart(cartItem);

  // Show success toast
  showToastMessage("Added to cart successfully", "shopping-cart");
};

const quickView = (product) => {
  selectedProduct.value = product;
  showQuickView.value = true;
};

const closeQuickView = () => {
  showQuickView.value = false;
  selectedProduct.value = null;
};

// Toast notification function
const showToastMessage = (message, icon = "check-circle") => {
  toastMessage.value = message;
  toastIcon.value = icon;
  showToast.value = true;

  // Hide toast after 3 seconds
  setTimeout(() => {
    showToast.value = false;
  }, 3000);
};

// Debounced search
let searchTimeout = null;
const debouncedSearch = () => {
  clearTimeout(searchTimeout);
  showSuggestions.value = searchQuery.value.length > 0;
  searchTimeout = setTimeout(() => {
    applyFilters();
  }, 300);
};

// Watch for route changes (category from HomePage)
watch(
  () => route.query.category,
  (newCategory) => {
    if (newCategory) {
      selectedCategory.value = newCategory;
    }
  },
  { immediate: true }
);

const setGreeting = () => {
  const hour = new Date().getHours();
  if (hour < 12) greeting.value = "Good Morning";
  else if (hour < 17) greeting.value = "Good Afternoon";
  else greeting.value = "Good Evening";
};

// Add pagination methods
const goToPage = (page) => {
  if (page >= 1 && page <= totalPages.value) {
    currentPage.value = page;
    // Scroll to top when changing pages
    window.scrollTo({ top: 0, behavior: "smooth" });
  }
};

const nextPage = () => {
  if (currentPage.value < totalPages.value) {
    goToPage(currentPage.value + 1);
  }
};

const prevPage = () => {
  if (currentPage.value > 1) {
    goToPage(currentPage.value - 1);
  }
};

// Reset to page 1 when filters change
const resetPagination = () => {
  currentPage.value = 1;
};

onMounted(() => {
  setGreeting();
  fetchProducts();
});
</script>

<style scoped>
.page-container {
  padding: 20px;
  padding-bottom: 100px;
}

/* Enhanced User Welcome Section (matching HomePage) */
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

.cart-btn,
.filter-toggle-btn {
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

.cart-btn:hover,
.filter-toggle-btn:hover {
  background-color: var(--primary-color);
  transform: translateY(-2px);
}

.cart-badge {
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

/* Enhanced Navigation (matching HomePage) */
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

/* Enhanced Search Section */
.search-section {
  margin-bottom: 25px;
}

.search-container {
  position: relative;
  margin-bottom: 20px;
}

.search-bar {
  position: relative;
  background-color: var(--card-bg);
  border-radius: 15px;
  border: 1px solid rgba(255, 255, 255, 0.1);
  overflow: hidden;
}

.search-icon {
  position: absolute;
  left: 20px;
  top: 50%;
  transform: translateY(-50%);
  color: #cccccc;
  font-size: 16px;
  z-index: 2;
}

.search-input {
  background: transparent;
  border: none;
  border-radius: 15px;
  padding: 18px 60px 18px 50px;
  color: white;
  width: 100%;
  font-size: 16px;
}

.search-input::placeholder {
  color: #888;
}

.clear-search-btn {
  position: absolute;
  right: 15px;
  top: 50%;
  transform: translateY(-50%);
  background: none;
  border: none;
  color: #cccccc;
  cursor: pointer;
  padding: 5px;
  border-radius: 50%;
  transition: all 0.3s ease;
}

.clear-search-btn:hover {
  color: var(--primary-color);
  background-color: rgba(255, 75, 38, 0.1);
}

.search-suggestions {
  position: absolute;
  top: 100%;
  left: 0;
  right: 0;
  background-color: var(--card-bg);
  border-radius: 15px;
  border: 1px solid rgba(255, 255, 255, 0.1);
  margin-top: 5px;
  z-index: 10;
  overflow: hidden;
}

.suggestion-item {
  display: flex;
  align-items: center;
  gap: 12px;
  padding: 15px 20px;
  cursor: pointer;
  transition: background-color 0.3s ease;
  color: #cccccc;
}

.suggestion-item:hover {
  background-color: rgba(255, 75, 38, 0.1);
  color: var(--primary-color);
}

/* Enhanced Filters */
.filters-container {
  background-color: var(--card-bg);
  border-radius: 20px;
  border: 1px solid rgba(255, 255, 255, 0.1);
  overflow: hidden;
  max-height: 0;
  transition: all 0.3s ease;
}

.filters-container.filters-expanded {
  max-height: 500px;
  padding: 25px;
}

.filter-row {
  margin-bottom: 25px;
}

.filter-row:last-child {
  margin-bottom: 0;
}

.filter-group {
  margin-bottom: 20px;
}

.filter-label {
  display: block;
  margin-bottom: 12px;
  font-size: 16px;
  font-weight: 600;
  color: white;
}

.category-filter-grid {
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  gap: 12px;
}

.category-filter-btn {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 8px;
  padding: 15px 10px;
  background-color: rgba(255, 255, 255, 0.05);
  border: 1px solid rgba(255, 255, 255, 0.1);
  border-radius: 15px;
  color: #cccccc;
  cursor: pointer;
  transition: all 0.3s ease;
  font-size: 12px;
}

.category-filter-btn.active {
  background-color: var(--primary-color);
  color: white;
  transform: translateY(-2px);
}

.category-filter-btn:hover:not(.active) {
  background-color: rgba(255, 75, 38, 0.1);
  color: var(--primary-color);
}

.category-filter-icon {
  width: 35px;
  height: 35px;
  border-radius: 10px;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 16px;
}

/* Category colors (matching HomePage) */
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

.price-filter-container {
  display: flex;
  flex-direction: column;
  gap: 12px;
}

.price-input-group {
  display: flex;
  align-items: center;
  gap: 12px;
}

.price-input {
  flex: 1;
  background-color: rgba(255, 255, 255, 0.05);
  border: 1px solid rgba(255, 255, 255, 0.1);
  border-radius: 10px;
  padding: 12px 15px;
  color: white;
  font-size: 14px;
}

.price-separator {
  color: #cccccc;
  font-weight: bold;
}

.price-range-display {
  font-size: 14px;
  color: var(--primary-color);
  font-weight: 600;
}

.sort-select {
  width: 100%;
  background-color: rgba(255, 255, 255, 0.05);
  border: 1px solid rgba(255, 255, 255, 0.1);
  border-radius: 10px;
  padding: 12px 15px;
  color: white;
  font-size: 14px;

  /* Add these */
  appearance: none; /* Remove default arrow and style */
  -webkit-appearance: none;
  -moz-appearance: none;

  background-image: url('data:image/svg+xml;charset=US-ASCII,<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 4 5"><path fill="white" d="M2 0L0 2h4L2 0zm0 5l2-2H0l2 2z"/></svg>');
  background-repeat: no-repeat;
  background-position: right 1rem center;
  background-size: 0.65em auto;
}

.sort-select option {
  background-color: #ffffff; /* or #222 or your dropdown background */
  color: rgb(0, 0, 0);
}

.filter-actions {
  display: flex;
  gap: 12px;
  margin-top: 20px;
}

.clear-filters-btn,
.apply-filters-btn {
  flex: 1;
  padding: 12px 20px;
  border-radius: 10px;
  font-weight: 600;
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 8px;
  cursor: pointer;
  transition: all 0.3s ease;
}

.clear-filters-btn {
  background-color: rgba(255, 255, 255, 0.1);
  border: 1px solid rgba(255, 255, 255, 0.2);
  color: #cccccc;
}

.apply-filters-btn {
  background-color: var(--primary-color);
  border: 1px solid var(--primary-color);
  color: white;
}

.clear-filters-btn:hover,
.apply-filters-btn:hover {
  transform: translateY(-2px);
}

/* Results Summary */
.results-summary {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 25px;
  padding: 20px;
  background-color: var(--card-bg);
  border-radius: 15px;
  border: 1px solid rgba(255, 255, 255, 0.1);
}

.results-title {
  display: flex;
  align-items: center;
  gap: 10px;
  font-size: 18px;
  font-weight: 700;
  color: white;
  margin: 0;
}

.results-subtitle {
  font-size: 14px;
  color: var(--primary-color);
  margin: 5px 0 0 0;
}

.view-toggle {
  display: flex;
  gap: 5px;
  background-color: rgba(255, 255, 255, 0.05);
  border-radius: 10px;
  padding: 5px;
}

.view-btn {
  padding: 8px 12px;
  background: none;
  border: none;
  border-radius: 8px;
  color: #cccccc;
  cursor: pointer;
  transition: all 0.3s ease;
}

.view-btn.active {
  background-color: var(--primary-color);
  color: white;
}

/* Loading State */
.loading-container {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  padding: 60px 20px;
  color: #cccccc;
}

.loading-spinner {
  width: 50px;
  height: 50px;
  border: 4px solid rgba(255, 75, 38, 0.3);
  border-top: 4px solid var(--primary-color);
  border-radius: 50%;
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

.loading-text {
  font-size: 16px;
  font-weight: 500;
}

/* Enhanced Product Grid - Fixed to 2 products per row */
.product-grid {
  display: grid;
  grid-template-columns: repeat(2, 1fr);
  gap: 15px;
}

.product-card {
  background-color: var(--card-bg);
  border-radius: 15px;
  border: 1px solid rgba(255, 255, 255, 0.1);
  overflow: hidden;
  cursor: pointer;
  transition: all 0.3s ease;
  display: flex;
  flex-direction: column;
  height: 320px; /* Reduced height */
  width: 100%; /* Ensure full width */
}

.product-image-container {
  position: relative;
  height: 120px; /* Reduced height */
  width: 100%;
  background-color: rgba(255, 255, 255, 0.05);
  display: flex;
  align-items: center;
  justify-content: center;
  flex-shrink: 0;
  overflow: hidden;
}

.product-image {
  width: 80px; /* Smaller image */
  height: 80px;
  object-fit: contain;
  object-position: center;
}

.product-info {
  padding: 12px; /* Reduced padding */
  display: flex;
  flex-direction: column;
  flex: 1;
  gap: 6px; /* Reduced gap */
  overflow: hidden; /* Prevent overflow */
}

.product-name {
  font-size: 13px; /* Smaller font */
  font-weight: 600;
  color: white;
  line-height: 1.2;
  margin: 0;
  display: -webkit-box;
  -webkit-line-clamp: 2;
  -webkit-box-orient: vertical;
  overflow: hidden;
  height: 32px; /* Fixed height for 2 lines */
  word-wrap: break-word; /* Break long words */
}

.current-price {
  font-size: 14px; /* Smaller font */
  font-weight: 700;
  color: var(--primary-color);
}

.availability-text {
  font-size: 9px; /* Smaller font */
  color: #cccccc;
  white-space: nowrap; /* Prevent wrapping */
  overflow: hidden;
  text-overflow: ellipsis;
}

.add-to-cart-btn {
  width: 100%;
  background: linear-gradient(45deg, var(--primary-color), #ff6b47);
  color: white;
  border: none;
  padding: 8px; /* Reduced padding */
  border-radius: 8px;
  font-weight: 600;
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 4px; /* Reduced gap */
  cursor: pointer;
  transition: all 0.3s ease;
  font-size: 11px; /* Smaller font */
  margin-top: auto;
  flex-shrink: 0; /* Prevent shrinking */
}

/* List View */
.product-list {
  display: flex;
  flex-direction: column;
  gap: 15px;
}

.product-list-item {
  display: flex;
  background-color: var(--card-bg);
  border-radius: 15px;
  border: 1px solid rgba(255, 255, 255, 0.1);
  padding: 20px;
  cursor: pointer;
  transition: all 0.3s ease;
}

.product-list-item:hover {
  transform: translateY(-2px);
  box-shadow: 0 5px 20px rgba(255, 75, 38, 0.1);
}

.list-item-image {
  width: 80px;
  height: 80px;
  background-color: rgba(255, 255, 255, 0.05);
  border-radius: 12px;
  display: flex;
  align-items: center;
  justify-content: center;
  margin-right: 20px;
  flex-shrink: 0;
  overflow: hidden;
}

.list-item-image .product-image {
  width: 60px; /* Fixed width for list view */
  height: 60px; /* Fixed height for list view */
  object-fit: contain;
}

.list-item-content {
  flex: 1;
  display: flex;
  flex-direction: column;
}

.list-item-header {
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
  margin-bottom: 8px;
}

.list-item-footer {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-top: auto;
}

.list-item-actions {
  display: flex;
  gap: 10px;
  align-items: center;
}

.list-item-actions .add-to-cart-btn {
  width: auto;
  padding: 8px 16px;
  font-size: 14px;
}

/* Empty State */
.empty-state {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  padding: 60px 20px;
  text-align: center;
}

.empty-state-icon {
  font-size: 60px;
  color: #666;
  margin-bottom: 20px;
}

.empty-state-title {
  font-size: 24px;
  font-weight: 700;
  color: white;
  margin-bottom: 10px;
}

.empty-state-description {
  font-size: 16px;
  color: #cccccc;
  margin-bottom: 30px;
  line-height: 1.5;
}

.empty-state-btn {
  background-color: var(--primary-color);
  color: white;
  border: none;
  padding: 12px 24px;
  border-radius: 25px;
  font-weight: 600;
  display: flex;
  align-items: center;
  gap: 8px;
  cursor: pointer;
  transition: all 0.3s ease;
}

.empty-state-btn:hover {
  transform: translateY(-2px);
  box-shadow: 0 5px 15px rgba(255, 75, 38, 0.4);
}

/* Quick View Modal */
.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background-color: rgba(0, 0, 0, 0.8);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 1000;
  padding: 20px;
}

.quick-view-modal {
  background-color: var(--card-bg);
  border-radius: 20px;
  border: 1px solid rgba(255, 255, 255, 0.1);
  max-width: 500px;
  width: 100%;
  max-height: 80vh;
  overflow-y: auto;
}

.modal-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 20px;
  border-bottom: 1px solid rgba(255, 255, 255, 0.1);
}

.modal-header h3 {
  font-size: 20px;
  font-weight: 700;
  color: white;
  margin: 0;
}

.close-btn {
  width: 35px;
  height: 35px;
  border-radius: 50%;
  border: none;
  background-color: rgba(255, 255, 255, 0.1);
  color: #cccccc;
  display: flex;
  align-items: center;
  justify-content: center;
  cursor: pointer;
  transition: all 0.3s ease;
}

.close-btn:hover {
  background-color: var(--primary-color);
  color: white;
}

.modal-content {
  padding: 20px;
}

.modal-image {
  width: 100%;
  height: 200px;
  background-color: rgba(255, 255, 255, 0.05);
  border-radius: 15px;
  display: flex;
  align-items: center;
  justify-content: center;
  margin-bottom: 20px;
}

.modal-image img {
  max-width: 80%;
  max-height: 80%;
  object-fit: contain;
}

.modal-info h4 {
  font-size: 20px;
  font-weight: 700;
  color: white;
  margin-bottom: 15px;
}

.product-description {
  font-size: 14px;
  color: #cccccc;
  line-height: 1.6;
  margin-bottom: 25px;
}

.modal-actions {
  display: flex;
  gap: 12px;
}

.view-details-btn {
  flex: 1;
  background-color: rgba(255, 255, 255, 0.1);
  border: 1px solid rgba(255, 255, 255, 0.2);
  color: white;
  padding: 12px 20px;
  border-radius: 12px;
  font-weight: 600;
  cursor: pointer;
  transition: all 0.3s ease;
}

.view-details-btn:hover {
  background-color: rgba(255, 255, 255, 0.2);
}

.modal-actions .add-to-cart-btn {
  flex: 1;
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

/* Pagination Styles */
.pagination-container {
  margin-top: 30px;
  padding: 20px;
  background-color: var(--card-bg);
  border-radius: 15px;
  border: 1px solid rgba(255, 255, 255, 0.1);
}

.pagination-info {
  text-align: center;
  margin-bottom: 20px;
}

.pagination-text {
  font-size: 14px;
  color: #cccccc;
}

.pagination-controls {
  display: flex;
  justify-content: center;
  align-items: center;
  gap: 15px;
  flex-wrap: wrap;
}

.pagination-btn {
  display: flex;
  align-items: center;
  gap: 8px;
  padding: 10px 16px;
  background-color: rgba(255, 255, 255, 0.1);
  border: 1px solid rgba(255, 255, 255, 0.2);
  border-radius: 10px;
  color: white;
  font-size: 14px;
  font-weight: 500;
  cursor: pointer;
  transition: all 0.3s ease;
}

.pagination-btn:hover:not(:disabled) {
  background-color: var(--primary-color);
  transform: translateY(-2px);
}

.pagination-btn:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}

.page-numbers {
  display: flex;
  gap: 8px;
}

.page-btn {
  width: 40px;
  height: 40px;
  border-radius: 10px;
  border: 1px solid rgba(255, 255, 255, 0.2);
  background-color: rgba(255, 255, 255, 0.1);
  color: white;
  font-size: 14px;
  font-weight: 500;
  cursor: pointer;
  transition: all 0.3s ease;
  display: flex;
  align-items: center;
  justify-content: center;
}

.page-btn:hover {
  background-color: rgba(255, 75, 38, 0.3);
}

.page-btn.active {
  background-color: var(--primary-color);
  border-color: var(--primary-color);
  transform: translateY(-2px);
}

/* Responsive pagination */
@media (max-width: 480px) {
  .pagination-controls {
    flex-direction: column;
    gap: 15px;
  }

  .page-numbers {
    order: 2;
  }

  .pagination-btn {
    order: 1;
  }
}

/* Responsive Design - Improved */
@media (max-width: 480px) {
  .product-grid {
    grid-template-columns: repeat(2, 1fr);
    gap: 12px;
  }

  .product-card {
    height: 320px;
  }

  .product-image-container {
    height: 120px;
  }

  .product-image {
    width: 80px;
    height: 80px;
  }

  .product-info {
    padding: 12px;
  }

  .product-name {
    font-size: 13px;
    height: 32px;
  }

  .current-price {
    font-size: 14px;
  }
}

@media (min-width: 481px) and (max-width: 768px) {
  .product-grid {
    grid-template-columns: repeat(2, 1fr);
    gap: 18px;
  }
}

@media (min-width: 769px) {
  .product-grid {
    grid-template-columns: repeat(3, 1fr);
    gap: 20px;
  }

  .product-card {
    height: 360px;
  }
}
</style>
