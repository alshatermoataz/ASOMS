<template>
  <div class="page-container">
    <!-- Profile Header -->
    <div class="profile-header">
      <div class="profile-cover">
        <div class="cover-gradient"></div>
        <div class="cover-pattern"></div>
      </div>

      <div class="profile-avatar-container">
        <div class="profile-avatar" @click="triggerFileInput">
          <img
            v-if="profileImage && profileImage !== defaultAvatar"
            :src="imageBase(profileImage)"
            alt="Profile"
            class="avatar-image"
          />
          <div v-else class="profile-avatar-placeholder">
            {{ userInitials }}
          </div>
          <div class="edit-avatar-overlay">
            <div class="edit-avatar-icon">
              <i class="fas fa-camera"></i>
            </div>
          </div>
          <div v-if="isUploadingImage" class="upload-spinner">
            <i class="fas fa-spinner fa-spin"></i>
          </div>
        </div>
        <input
          type="file"
          ref="fileInput"
          style="display: none"
          accept="image/*"
          @change="onFileSelected"
        />
      </div>

      <div class="profile-name-container">
        <h2 v-if="!isEditingName" @click="startEditName" class="profile-name">
          {{ editableName || "Add your name" }}
          <i class="fas fa-edit edit-name-icon"></i>
        </h2>
        <div v-else class="edit-name-container">
          <input
            type="text"
            v-model="editableName"
            class="edit-name-input"
            ref="nameInput"
            @blur="saveName"
            @keyup.enter="saveName"
            placeholder="Enter your name"
          />
        </div>
        <p class="profile-email">{{ auth.user?.email || "No email" }}</p>
        <div class="profile-badges">
          <span class="badge verified">
            <i class="fas fa-check-circle"></i>
            Verified
          </span>
        </div>
      </div>
    </div>

    <!-- Profile Stats -->
    <div class="profile-stats">
      <div class="stat-item" v-for="stat in orderStats" :key="stat.label">
        <div class="stat-icon">
          <i :class="stat.icon"></i>
        </div>
        <div class="stat-content">
          <span class="stat-value">{{ stat.value }}</span>
          <span class="stat-label">{{ stat.label }}</span>
        </div>
      </div>
    </div>

    <!-- Recent Orders Preview -->
    <div class="recent-orders-section" v-if="recentOrders.length > 0">
      <div class="section-header">
        <h3 class="section-title">
          <i class="fas fa-clock"></i>
          Recent Orders
        </h3>
        <router-link to="/history" class="view-all-link">
          View All
          <i class="fas fa-arrow-right"></i>
        </router-link>
      </div>
      <div class="orders-preview">
        <div
          v-for="order in recentOrders.slice(0, 3)"
          :key="order.id"
          class="order-preview-item"
        >
          <div class="order-icon">
            <i class="fas fa-box"></i>
          </div>
          <div class="order-details">
            <div class="order-title">Order #{{ order.orderNumber }}</div>
            <div class="order-date">{{ formatDate(order.createdAt) }}</div>
          </div>
          <div class="order-status" :class="getStatusClass(order.status)">
            {{ order.status }}
          </div>
        </div>
      </div>
    </div>

    <!-- Profile Sections -->
    <div class="profile-sections">
      <!-- Account Section -->
      <div class="profile-section">
        <h3 class="section-title">
          <i class="fas fa-user-cog"></i>
          Account
        </h3>

        <div class="profile-menu-item" @click="toggleSection('personalInfo')">
          <div class="menu-item-icon">
            <i class="fas fa-user"></i>
          </div>
          <div class="menu-item-content">
            <div class="menu-item-text">Personal Information</div>
            <div class="menu-item-subtitle">Update your details</div>
          </div>
          <div class="menu-item-arrow">
            <i
              :class="
                expandedSections.personalInfo
                  ? 'fas fa-chevron-up'
                  : 'fas fa-chevron-down'
              "
            ></i>
          </div>
        </div>

        <div v-if="expandedSections.personalInfo" class="profile-submenu">
          <div class="form-grid">
            <div class="profile-form-group">
              <label>
                <i class="fas fa-user"></i>
                Full Name
              </label>
              <input
                type="text"
                v-model="personalInfo.fullName"
                class="profile-input"
                placeholder="Enter your full name"
              />
            </div>
            <div class="profile-form-group">
              <label>
                <i class="fas fa-phone"></i>
                Phone Number
              </label>
              <input
                type="tel"
                v-model="personalInfo.phone"
                class="profile-input"
                placeholder="Enter your phone number"
              />
            </div>
            <div class="profile-form-group full-width">
              <label>
                <i class="fas fa-map-marker-alt"></i>
                Address
              </label>
              <textarea
                v-model="personalInfo.address"
                class="profile-textarea"
                placeholder="Enter your address"
                rows="3"
              ></textarea>
            </div>
          </div>
          <button
            class="btn-primary save-btn"
            @click="savePersonalInfo"
            :disabled="isUpdating"
          >
            <i class="fas fa-save"></i>
            {{ isUpdating ? "Saving..." : "Save Changes" }}
          </button>
        </div>

        <div class="profile-menu-item" @click="toggleSection('paymentMethods')">
          <div class="menu-item-icon">
            <i class="fas fa-credit-card"></i>
          </div>
          <div class="menu-item-content">
            <div class="menu-item-text">Payment Methods</div>
            <div class="menu-item-subtitle">Manage payment options</div>
          </div>
          <div class="menu-item-arrow">
            <i
              :class="
                expandedSections.paymentMethods
                  ? 'fas fa-chevron-up'
                  : 'fas fa-chevron-down'
              "
            ></i>
          </div>
        </div>

        <div v-if="expandedSections.paymentMethods" class="profile-submenu">
          <div
            v-for="(method, index) in paymentMethods"
            :key="index"
            class="payment-method-item"
          >
            <div class="payment-method-icon">
              <i :class="method.icon"></i>
            </div>
            <div class="payment-method-details">
              <div class="payment-method-name">{{ method.name }}</div>
              <div class="payment-method-info">{{ method.info }}</div>
            </div>
            <div class="payment-method-actions">
              <div v-if="method.isDefault" class="payment-method-default">
                <i class="fas fa-star"></i>
                Default
              </div>
              <button class="btn-icon" @click="removePaymentMethod(index)">
                <i class="fas fa-trash"></i>
              </button>
            </div>
          </div>

          <button class="btn-secondary add-payment-btn">
            <i class="fas fa-plus"></i>
            Add Payment Method
          </button>
        </div>
      </div>

      <!-- Settings Section -->
      <!-- <div class="profile-section">
        <h3 class="section-title">
          <i class="fas fa-cog"></i>
          Settings
        </h3>

        <div class="settings-grid">
          <div class="profile-menu-item">
            <div class="menu-item-icon">
              <i class="fas fa-bell"></i>
            </div>
            <div class="menu-item-content">
              <div class="menu-item-text">Notifications</div>
              <div class="menu-item-subtitle">Push notifications</div>
            </div>
            <div class="toggle-switch">
              <input
                type="checkbox"
                id="notifications"
                v-model="settings.notifications"
                @change="saveSettings"
              />
              <label for="notifications"></label>
            </div>
          </div>

          <div class="profile-menu-item">
            <div class="menu-item-icon">
              <i class="fas fa-moon"></i>
            </div>
            <div class="menu-item-content">
              <div class="menu-item-text">Dark Mode</div>
              <div class="menu-item-subtitle">Theme preference</div>
            </div>
            <div class="toggle-switch">
              <input
                type="checkbox"
                id="darkMode"
                v-model="settings.darkMode"
                @change="saveSettings"
              />
              <label for="darkMode"></label>
            </div>
          </div>

          <div class="profile-menu-item">
            <div class="menu-item-icon">
              <i class="fas fa-language"></i>
            </div>
            <div class="menu-item-content">
              <div class="menu-item-text">Language</div>
              <div class="menu-item-subtitle">App language</div>
            </div>
            <select class="language-select" v-model="settings.language">
              <option value="en">English</option>
              <option value="es">Español</option>
              <option value="fr">Français</option>
            </select>
          </div>
        </div>
      </div> -->

      <!-- Support Section -->
      <!-- <div class="profile-section">
        <h3 class="section-title">
          <i class="fas fa-life-ring"></i>
          Support
        </h3>

        <div class="support-grid">
          <div class="profile-menu-item" @click="navigateTo('/help')">
            <div class="menu-item-icon">
              <i class="fas fa-question-circle"></i>
            </div>
            <div class="menu-item-content">
              <div class="menu-item-text">Help Center</div>
              <div class="menu-item-subtitle">FAQs and guides</div>
            </div>
            <div class="menu-item-arrow">
              <i class="fas fa-chevron-right"></i>
            </div>
          </div>

          <div class="profile-menu-item" @click="navigateTo('/contact')">
            <div class="menu-item-icon">
              <i class="fas fa-headset"></i>
            </div>
            <div class="menu-item-content">
              <div class="menu-item-text">Contact Support</div>
              <div class="menu-item-subtitle">Get help from our team</div>
            </div>
            <div class="menu-item-arrow">
              <i class="fas fa-chevron-right"></i>
            </div>
          </div>

          <div class="profile-menu-item" @click="navigateTo('/terms')">
            <div class="menu-item-icon">
              <i class="fas fa-file-contract"></i>
            </div>
            <div class="menu-item-content">
              <div class="menu-item-text">Terms & Privacy</div>
              <div class="menu-item-subtitle">Legal information</div>
            </div>
            <div class="menu-item-arrow">
              <i class="fas fa-chevron-right"></i>
            </div>
          </div>
        </div>
      </div> -->

      <!-- Logout Button -->
      <button class="btn-logout" @click="handleLogout" :disabled="isLoggingOut">
        <i class="fas fa-sign-out-alt"></i>
        {{ isLoggingOut ? "Logging out..." : "Logout" }}
      </button>
    </div>

    <!-- Success Toast -->
    <div v-if="showToast" class="toast-message" :class="{ show: showToast }">
      <div class="toast-icon">
        <i
          :class="
            toastType === 'success'
              ? 'fas fa-check-circle'
              : 'fas fa-exclamation-circle'
          "
        ></i>
      </div>
      <div class="toast-text">{{ toastMessage }}</div>
    </div>

    <!-- Bottom Navigation -->
    <BottomNavigation />
  </div>
</template>

<script setup>
import { ref, computed, onMounted, nextTick, watch } from "vue";
import { useRouter } from "vue-router";
import { useAuthStore } from "../stores/auth";
import axios from "axios";
import BottomNavigation from "../components/BottomNavigation.vue";

const auth = useAuthStore();
const router = useRouter();
const fileInput = ref(null);
const nameInput = ref(null);
const defaultAvatar = "/images/default-avatar.png";
const API_BASE = "https://asoms-production.up.railway.app";

// Loading states
const isUpdating = ref(false);
const isLoggingOut = ref(false);
const isUploadingImage = ref(false);
const isLoadingOrders = ref(false);

// Profile image with fallback
const profileImage = computed(() => {
  return auth.user?.profilePictureUrl || defaultAvatar;
});

// Editable name
const isEditingName = ref(false);
const editableName = ref("");

// Order statistics and recent orders
const orderStats = ref([
  { label: "Total Orders", value: 0, icon: "fas fa-shopping-bag" },
  { label: "Completed", value: 0, icon: "fas fa-check-circle" },
  { label: "Pending", value: 0, icon: "fas fa-clock" },
  { label: "Rejected", value: 0, icon: "fas fa-times-circle" },
]);

const recentOrders = ref([]);

// Expanded sections
const expandedSections = ref({
  personalInfo: false,
  paymentMethods: false,
});

// Personal information
const personalInfo = ref({
  fullName: "",
  phone: "",
  address: "",
});

// Payment methods with better icons
const paymentMethods = ref([
  {
    type: "invoice",
    name: "Invoice",
    info: "Take now, Pay later",
    icon: "fas fa-file-invoice",
    isDefault: false,
  },
  {
    type: "cash",
    name: "Cash on Pick-up",
    info: "Pay when you receive your order",
    icon: "fas fa-money-bill-wave",
    isDefault: true,
  },
]);

// Settings
const settings = ref({
  notifications: true,
  darkMode: true,
  language: "en",
});

// Toast
const showToast = ref(false);
const toastMessage = ref("");
const toastType = ref("success");

// Computed
const userInitials = computed(() => {
  if (!auth.user?.fullName) return "U";
  const nameParts = auth.user.fullName.split(" ");
  if (nameParts.length >= 2) {
    return `${nameParts[0][0]}${nameParts[1][0]}`;
  }
  return nameParts[0][0];
});

// Watch for auth.user changes
watch(
  () => auth.user,
  (newUser) => {
    if (newUser) {
      editableName.value = newUser.fullName || "";
      personalInfo.value = {
        fullName: newUser.fullName || "",
        phone: newUser.contactNumber || "",
        address: newUser.address || "",
      };
    }
  },
  { immediate: true, deep: true }
);

// Methods
const triggerFileInput = () => {
  fileInput.value.click();
};

const onFileSelected = async (event) => {
  const file = event.target.files[0];
  if (file) {
    // Validate file size (max 5MB)
    if (file.size > 5 * 1024 * 1024) {
      showToastMessage("File size must be less than 5MB", "error");
      return;
    }

    // Validate file type
    if (!file.type.startsWith("image/")) {
      showToastMessage("Please select a valid image file", "error");
      return;
    }

    try {
      isUploadingImage.value = true;

      // Use the store method instead of direct API call
      await auth.uploadProfilePicture(file);

      showToastMessage("Profile picture updated successfully", "success");
    } catch (error) {
      console.error("Error uploading profile picture:", error);
      showToastMessage(
        error.response?.data?.message || "Failed to update profile picture",
        "error"
      );
    } finally {
      isUploadingImage.value = false;
    }
  }
};

const loadOrderStats = async () => {
  try {
    isLoadingOrders.value = true;

    // Use the store method to fetch orders
    const orders = await auth.fetchUserOrders();

    // Calculate statistics
    const stats = {
      total: orders.length,
      completed: orders.filter((order) => order.status === "Completed").length,
      pending: orders.filter((order) => order.status === "Pending Approval")
        .length,
      cancelled: orders.filter((order) => order.status === "Rejected").length,
    };

    // Update order stats
    orderStats.value[0].value = stats.total;
    orderStats.value[1].value = stats.completed;
    orderStats.value[2].value = stats.pending;
    orderStats.value[3].value = stats.cancelled;

    // Set recent orders (sorted by date, most recent first)
    recentOrders.value = orders
      .sort((a, b) => new Date(b.createdAt) - new Date(a.createdAt))
      .slice(0, 5);
  } catch (error) {
    console.error("Error loading order stats:", error);
    // Keep default values if API fails
  } finally {
    isLoadingOrders.value = false;
  }
};

const formatDate = (dateString) => {
  if (!dateString) return "N/A";
  const date = new Date(dateString);
  return date.toLocaleDateString("en-US", {
    month: "short",
    day: "numeric",
    year: "numeric",
  });
};

const getStatusClass = (status) => {
  const statusClasses = {
    Completed: "status-completed",
    Pending: "status-pending",
    Processing: "status-processing",
    Cancelled: "status-cancelled",
    Shipped: "status-shipped",
  };
  return statusClasses[status] || "status-default";
};

const startEditName = () => {
  isEditingName.value = true;
  nextTick(() => {
    nameInput.value?.focus();
  });
};

const saveName = async () => {
  if (!editableName.value.trim()) {
    showToastMessage("Name cannot be empty", "error");
    return;
  }

  try {
    isUpdating.value = true;
    // Use the store method
    await auth.updateUserName(editableName.value);
    isEditingName.value = false;
    showToastMessage("Name updated successfully", "success");
  } catch (error) {
    console.error("Error updating name:", error);
    showToastMessage("Failed to update name", "error");
  } finally {
    isUpdating.value = false;
  }
};

const toggleSection = (section) => {
  expandedSections.value[section] = !expandedSections.value[section];
};

const savePersonalInfo = async () => {
  if (!personalInfo.value.fullName.trim()) {
    showToastMessage("Full name is required", "error");
    return;
  }

  try {
    isUpdating.value = true;

    // Use the store method
    await auth.updateUserProfile({
      email: auth.user?.email,
      fullName: personalInfo.value.fullName,
      contactNumber: personalInfo.value.phone,
      address: personalInfo.value.address,
    });

    editableName.value = personalInfo.value.fullName;
    showToastMessage("Personal information updated successfully", "success");
    expandedSections.value.personalInfo = false;
  } catch (error) {
    console.error("Error updating personal info:", error);
    showToastMessage("Failed to update personal information", "error");
  } finally {
    isUpdating.value = false;
  }
};

const saveSettings = async () => {
  try {
    // Try to save to API first, fallback to localStorage
    await auth.updateUserSettings(settings.value);
    showToastMessage("Settings saved", "success");
  } catch (error) {
    console.error("Error saving settings:", error);
    // Fallback to localStorage
    localStorage.setItem("userSettings", JSON.stringify(settings.value));
    showToastMessage("Settings saved locally", "success");
  }
};
const removePaymentMethod = (index) => {
  if (paymentMethods.value[index].isDefault) {
    showToastMessage("Cannot remove default payment method", "error");
    return;
  }
  paymentMethods.value.splice(index, 1);
  showToastMessage("Payment method removed", "success");
};

const navigateTo = (path) => {
  router.push(path);
};

const imageBase = (url) =>
  url?.startsWith("http") ? url : `${API_BASE}${url}`;

const showToastMessage = (message, type = "success") => {
  toastMessage.value = message;
  toastType.value = type;
  showToast.value = true;
  setTimeout(() => {
    showToast.value = false;
  }, 3000);
};

const handleLogout = async () => {
  try {
    isLoggingOut.value = true;
    await auth.logout();
    router.push("/login");
  } catch (error) {
    console.error("Error logging out:", error);
    auth.logout();
    router.push("/login");
  } finally {
    isLoggingOut.value = false;
  }
};

// Initialize data on component mount
onMounted(async () => {
  auth.restore();

  if (auth.isLoggedIn && auth.user) {
    await loadOrderStats();

    // Load saved settings
    const savedSettings = localStorage.getItem("userSettings");
    if (savedSettings) {
      settings.value = { ...settings.value, ...JSON.parse(savedSettings) };
    }
  }
});
</script>

<style scoped>
:root {
  --primary-color: #ff4b26;
  --secondary-color: #ff6b47;
  --background-color: #0a0a0a;
  --card-bg: #1a1a1a;
  --input-bg: #2a2a2a;
  --text-primary: #ffffff;
  --text-secondary: #cccccc;
  --text-muted: #888888;
  --border-color: #333333;
  --success-color: #00ff88;
  --error-color: #ff4757;
  --warning-color: #ffa502;
}

.page-container {
  min-height: 100vh;
  background: linear-gradient(135deg, var(--background-color) 0%, #1a1a1a 100%);
  color: var(--text-primary);
  padding: 0 20px 100px;
  font-family: -apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, sans-serif;
}

/* Profile Header */
.profile-header {
  position: relative;
  margin-bottom: 40px;
  padding-top: 20px;
}

.profile-cover {
  height: 180px;
  background: linear-gradient(
    135deg,
    var(--primary-color),
    var(--secondary-color)
  );
  border-radius: 20px;
  margin-bottom: 60px;
  position: relative;
  overflow: hidden;
}

.cover-gradient {
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: linear-gradient(
    45deg,
    rgba(255, 75, 38, 0.8),
    rgba(255, 107, 71, 0.6)
  );
}

.cover-pattern {
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background-image: radial-gradient(
      circle at 20% 50%,
      rgba(255, 255, 255, 0.1) 0%,
      transparent 50%
    ),
    radial-gradient(
      circle at 80% 20%,
      rgba(255, 255, 255, 0.1) 0%,
      transparent 50%
    );
}

.profile-avatar-container {
  position: absolute;
  left: 50%;
  top: 140px;
  transform: translateX(-50%);
}

.profile-avatar {
  width: 120px;
  height: 120px;
  border-radius: 50%;
  background: linear-gradient(135deg, var(--card-bg), #2a2a2a);
  border: 4px solid var(--background-color);
  overflow: hidden;
  display: flex;
  align-items: center;
  justify-content: center;
  position: relative;
  cursor: pointer;
  transition: all 0.3s ease;
  box-shadow: 0 8px 32px rgba(0, 0, 0, 0.3);
}

.profile-avatar:hover {
  transform: scale(1.05);
  box-shadow: 0 12px 40px rgba(255, 75, 38, 0.2);
}

.avatar-image {
  width: 100%;
  height: 100%;
  object-fit: cover;
}

.profile-avatar-placeholder {
  font-size: 40px;
  font-weight: 700;
  color: var(--primary-color);
  text-shadow: 0 2px 4px rgba(0, 0, 0, 0.3);
}

.edit-avatar-overlay {
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: rgba(0, 0, 0, 0.5);
  display: flex;
  align-items: center;
  justify-content: center;
  opacity: 0;
  transition: opacity 0.3s ease;
}

.profile-avatar:hover .edit-avatar-overlay {
  opacity: 1;
}

.edit-avatar-icon {
  background: var(--primary-color);
  width: 40px;
  height: 40px;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  color: white;
  font-size: 16px;
}

.upload-spinner {
  position: absolute;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
  background: rgba(0, 0, 0, 0.8);
  width: 60px;
  height: 60px;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  color: var(--primary-color);
  font-size: 20px;
}

.profile-name-container {
  text-align: center;
  margin-top: 20px;
}

.profile-name {
  font-size: 28px;
  font-weight: 700;
  margin-bottom: 8px;
  cursor: pointer;
  transition: color 0.3s ease;
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 10px;
}

.profile-name:hover {
  color: var(--primary-color);
}

.edit-name-icon {
  font-size: 16px;
  opacity: 0.6;
}

.profile-email {
  font-size: 16px;
  color: var(--text-secondary);
  margin-bottom: 15px;
}

.profile-badges {
  display: flex;
  justify-content: center;
  gap: 10px;
}

.badge {
  display: inline-flex;
  align-items: center;
  gap: 5px;
  padding: 6px 12px;
  border-radius: 20px;
  font-size: 12px;
  font-weight: 600;
}

.badge.verified {
  background: rgba(0, 255, 136, 0.1);
  color: var(--success-color);
  border: 1px solid rgba(0, 255, 136, 0.2);
}

.edit-name-container {
  margin-bottom: 8px;
}

.edit-name-input {
  background: var(--input-bg);
  border: 2px solid var(--border-color);
  border-radius: 12px;
  padding: 12px 20px;
  color: var(--text-primary);
  width: 100%;
  max-width: 300px;
  text-align: center;
  font-size: 20px;
  font-weight: 600;
  transition: border-color 0.3s ease;
}

.edit-name-input:focus {
  outline: none;
  border-color: var(--primary-color);
}

/* Profile Stats */
.profile-stats {
  display: grid;
  grid-template-columns: repeat(2, 1fr);
  gap: 15px;
  margin-bottom: 30px;
}

.stat-item {
  background: linear-gradient(135deg, var(--card-bg), #252525);
  border-radius: 16px;
  padding: 20px;
  display: flex;
  align-items: center;
  gap: 15px;
  transition: transform 0.3s ease;
  border: 1px solid var(--border-color);
}

.stat-item:hover {
  transform: translateY(-2px);
}

.stat-icon {
  width: 50px;
  height: 50px;
  background: linear-gradient(
    135deg,
    var(--primary-color),
    var(--secondary-color)
  );
  border-radius: 12px;
  display: flex;
  align-items: center;
  justify-content: center;
  color: white;
  font-size: 20px;
}

.stat-content {
  display: flex;
  flex-direction: column;
}

.stat-value {
  font-size: 24px;
  font-weight: 700;
  color: var(--text-primary);
  line-height: 1;
}

.stat-label {
  font-size: 14px;
  color: var(--text-secondary);
  margin-top: 4px;
}

/* Recent Orders */
.recent-orders-section {
  margin-bottom: 30px;
}

.section-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 15px;
}

.section-title {
  font-size: 20px;
  font-weight: 700;
  color: var(--text-primary);
  display: flex;
  align-items: center;
  gap: 10px;
}

.view-all-link {
  color: var(--primary-color);
  text-decoration: none;
  font-size: 14px;
  font-weight: 600;
  display: flex;
  align-items: center;
  gap: 5px;
  transition: opacity 0.3s ease;
}

.view-all-link:hover {
  opacity: 0.8;
}

.orders-preview {
  background: var(--card-bg);
  border-radius: 16px;
  padding: 20px;
  border: 1px solid var(--border-color);
}

.order-preview-item {
  display: flex;
  align-items: center;
  gap: 15px;
  padding: 15px 0;
  border-bottom: 1px solid var(--border-color);
}

.order-preview-item:last-child {
  border-bottom: none;
}

.order-icon {
  width: 40px;
  height: 40px;
  background: rgba(255, 75, 38, 0.1);
  border-radius: 10px;
  display: flex;
  align-items: center;
  justify-content: center;
  color: var(--primary-color);
}

.order-details {
  flex: 1;
}

.order-title {
  font-weight: 600;
  color: var(--text-primary);
  margin-bottom: 4px;
}

.order-date {
  font-size: 14px;
  color: var(--text-secondary);
}

.order-status {
  padding: 6px 12px;
  border-radius: 20px;
  font-size: 12px;
  font-weight: 600;
}

.status-completed {
  background: rgba(0, 255, 136, 0.1);
  color: var(--success-color);
}

.status-pending {
  background: rgba(255, 165, 2, 0.1);
  color: var(--warning-color);
}

.status-cancelled {
  background: rgba(255, 71, 87, 0.1);
  color: var(--error-color);
}

/* Profile Sections */
.profile-sections {
  margin-bottom: 100px;
}

.profile-section {
  margin-bottom: 30px;
}

.profile-menu-item {
  display: flex;
  align-items: center;
  background: var(--card-bg);
  border-radius: 16px;
  padding: 20px;
  margin-bottom: 12px;
  cursor: pointer;
  transition: all 0.3s ease;
  border: 1px solid var(--border-color);
}

.profile-menu-item:hover {
  background: #252525;
  transform: translateY(-1px);
}

.menu-item-icon {
  width: 50px;
  height: 50px;
  background: linear-gradient(
    135deg,
    rgba(255, 75, 38, 0.1),
    rgba(255, 107, 71, 0.1)
  );
  border-radius: 12px;
  display: flex;
  align-items: center;
  justify-content: center;
  color: var(--primary-color);
  margin-right: 15px;
  font-size: 18px;
}

.menu-item-content {
  flex: 1;
}

.menu-item-text {
  font-size: 16px;
  font-weight: 600;
  color: var(--text-primary);
  margin-bottom: 4px;
}

.menu-item-subtitle {
  font-size: 14px;
  color: var(--text-secondary);
}

.menu-item-arrow {
  color: var(--text-muted);
  font-size: 16px;
}

/* Submenu */
.profile-submenu {
  background: var(--card-bg);
  border-radius: 16px;
  padding: 25px;
  margin-top: -12px;
  margin-bottom: 12px;
  border: 1px solid var(--border-color);
}

.form-grid {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 20px;
  margin-bottom: 25px;
}

.profile-form-group {
  display: flex;
  flex-direction: column;
}

.profile-form-group.full-width {
  grid-column: 1 / -1;
}

.profile-form-group label {
  display: flex;
  align-items: center;
  gap: 8px;
  margin-bottom: 10px;
  font-size: 14px;
  font-weight: 600;
  color: var(--text-secondary);
}

.profile-input,
.profile-textarea {
  background: var(--input-bg);
  border: 2px solid var(--border-color);
  border-radius: 12px;
  padding: 14px 16px;
  color: var(--text-primary);
  font-size: 16px;
  transition: border-color 0.3s ease;
}

.profile-input:focus,
.profile-textarea:focus {
  outline: none;
  border-color: var(--primary-color);
}

.profile-textarea {
  resize: vertical;
  min-height: 80px;
}

/* Payment Methods */
.payment-method-item {
  display: flex;
  align-items: center;
  padding: 15px 0;
  border-bottom: 1px solid var(--border-color);
}

.payment-method-item:last-child {
  border-bottom: none;
}

.payment-method-icon {
  width: 45px;
  height: 45px;
  background: rgba(255, 255, 255, 0.05);
  border-radius: 12px;
  display: flex;
  align-items: center;
  justify-content: center;
  margin-right: 15px;
  color: var(--primary-color);
}

.payment-method-details {
  flex: 1;
}

.payment-method-name {
  font-size: 16px;
  font-weight: 600;
  color: var(--text-primary);
  margin-bottom: 4px;
}

.payment-method-info {
  font-size: 14px;
  color: var(--text-secondary);
}

.payment-method-actions {
  display: flex;
  align-items: center;
  gap: 10px;
}

.payment-method-default {
  display: flex;
  align-items: center;
  gap: 5px;
  font-size: 12px;
  color: var(--primary-color);
  background: rgba(255, 75, 38, 0.1);
  padding: 6px 10px;
  border-radius: 20px;
  font-weight: 600;
}

/* Settings */
.settings-grid,
.support-grid {
  display: flex;
  flex-direction: column;
  gap: 12px;
}

.language-select {
  background: var(--input-bg);
  border: 2px solid var(--border-color);
  border-radius: 8px;
  padding: 8px 12px;
  color: var(--text-primary);
  font-size: 14px;
}

/* Toggle Switch */
.toggle-switch {
  position: relative;
  display: inline-block;
  width: 60px;
  height: 30px;
}

.toggle-switch input {
  opacity: 0;
  width: 0;
  height: 0;
}

.toggle-switch label {
  position: absolute;
  cursor: pointer;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: var(--input-bg);
  transition: 0.3s;
  border-radius: 30px;
  border: 2px solid var(--border-color);
}

.toggle-switch label:before {
  position: absolute;
  content: "";
  height: 22px;
  width: 22px;
  left: 2px;
  bottom: 2px;
  background: var(--text-primary);
  transition: 0.3s;
  border-radius: 50%;
}

.toggle-switch input:checked + label {
  background: var(--primary-color);
  border-color: var(--primary-color);
}

.toggle-switch input:checked + label:before {
  transform: translateX(30px);
}

/* Buttons */
.btn-primary,
.btn-secondary {
  border: none;
  border-radius: 12px;
  padding: 14px 24px;
  font-weight: 600;
  font-size: 16px;
  cursor: pointer;
  transition: all 0.3s ease;
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 8px;
}

.btn-primary {
  background: linear-gradient(
    135deg,
    var(--primary-color),
    var(--secondary-color)
  );
  color: white;
}

.btn-primary:hover {
  transform: translateY(-2px);
  box-shadow: 0 8px 25px rgba(255, 75, 38, 0.3);
}

.btn-primary:disabled {
  opacity: 0.6;
  cursor: not-allowed;
  transform: none;
}

.btn-secondary {
  background: var(--input-bg);
  color: var(--text-primary);
  border: 2px solid var(--border-color);
}

.btn-secondary:hover {
  background: #353535;
  border-color: var(--primary-color);
}

.btn-icon {
  background: transparent;
  border: none;
  color: var(--text-muted);
  cursor: pointer;
  padding: 8px;
  border-radius: 8px;
  transition: all 0.3s ease;
}

.btn-icon:hover {
  background: rgba(255, 75, 38, 0.1);
  color: var(--primary-color);
}

.add-payment-btn {
  margin-top: 20px;
  width: 100%;
}

.save-btn {
  width: 100%;
}

.btn-logout {
  background: transparent;
  border: 2px solid var(--error-color);
  color: var(--error-color);
  padding: 16px 24px;
  border-radius: 12px;
  font-weight: 600;
  width: 100%;
  cursor: pointer;
  font-size: 16px;
  margin-top: 30px;
  transition: all 0.3s ease;
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 10px;
}

.btn-logout:hover {
  background: rgba(255, 71, 87, 0.1);
}

.btn-logout:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}

/* Toast */
.toast-message {
  position: fixed;
  bottom: 120px;
  left: 50%;
  transform: translateX(-50%) translateY(100px);
  background: var(--card-bg);
  color: var(--text-primary);
  padding: 16px 24px;
  border-radius: 12px;
  display: flex;
  align-items: center;
  box-shadow: 0 8px 32px rgba(0, 0, 0, 0.3);
  transition: transform 0.3s ease;
  z-index: 1000;
  border: 1px solid var(--border-color);
  min-width: 280px;
}

.toast-message.show {
  transform: translateX(-50%) translateY(0);
}

.toast-icon {
  margin-right: 12px;
  font-size: 20px;
}

.toast-icon .fa-check-circle {
  color: var(--success-color);
}

.toast-icon .fa-exclamation-circle {
  color: var(--error-color);
}

.toast-text {
  font-size: 14px;
  font-weight: 500;
}

/* Responsive Design */
@media (max-width: 768px) {
  .form-grid {
    grid-template-columns: 1fr;
  }

  .profile-stats {
    grid-template-columns: 1fr 1fr;
  }

  .page-container {
    padding: 0 15px 100px;
  }
}

@media (max-width: 480px) {
  .profile-stats {
    grid-template-columns: 1fr;
  }

  .profile-cover {
    height: 150px;
  }

  .profile-avatar {
    width: 100px;
    height: 100px;
  }

  .profile-name {
    font-size: 24px;
  }
}
</style>
