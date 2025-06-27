<template>
  <div class="page-container">
    <div class="status-bar">
      <span>9:41</span>
      <div>
        <span>Π</span>
      </div>
    </div>

    <!-- Profile Header -->
    <div class="profile-header">
      <div class="profile-cover"></div>

      <div class="profile-avatar-container">
        <div class="profile-avatar" @click="triggerFileInput">
          <img
            v-if="profileImage"
            :src="imageBase(profileImage)"
            alt="Profile"
          />
          <div v-else class="profile-avatar-placeholder">
            {{ userInitials }}
          </div>
          <div class="edit-avatar-icon">
            <font-awesome-icon icon="camera" />
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
        <h2 v-if="!isEditingName" @click="startEditName">{{ editableName }}</h2>
        <div v-else class="edit-name-container">
          <input
            type="text"
            v-model="editableName"
            class="edit-name-input"
            ref="nameInput"
            @blur="saveName"
            @keyup.enter="saveName"
          />
        </div>
        <p class="profile-email">{{ auth.user?.email || "No email" }}</p>
      </div>
    </div>

    <!-- Profile Stats -->
    <div class="profile-stats">
      <div class="stat-item">
        <span class="stat-value">{{ orderStats.total }}</span>
        <span class="stat-label">Orders</span>
      </div>
      <div class="stat-item">
        <span class="stat-value">{{ orderStats.completed }}</span>
        <span class="stat-label">Completed</span>
      </div>
      <div class="stat-item">
        <span class="stat-value">{{ orderStats.pending }}</span>
        <span class="stat-label">Pending</span>
      </div>
    </div>

    <!-- Profile Sections -->
    <div class="profile-sections">
      <!-- Account Section -->
      <div class="profile-section">
        <h3 class="section-title">Account</h3>

        <div class="profile-menu-item" @click="toggleSection('personalInfo')">
          <div class="menu-item-icon">
            <font-awesome-icon icon="user" />
          </div>
          <div class="menu-item-text">Personal Information</div>
          <div class="menu-item-arrow">
            <font-awesome-icon
              :icon="
                expandedSections.personalInfo ? 'chevron-up' : 'chevron-down'
              "
            />
          </div>
        </div>

        <div v-if="expandedSections.personalInfo" class="profile-submenu">
          <div class="profile-form-group">
            <label>Full Name</label>
            <input
              type="text"
              v-model="personalInfo.fullName"
              class="profile-input"
            />
          </div>
          <div class="profile-form-group">
            <label>Phone Number</label>
            <input
              type="tel"
              v-model="personalInfo.phone"
              class="profile-input"
            />
          </div>
          <button
            class="btn-primary save-btn"
            @click="savePersonalInfo"
            :disabled="isUpdating"
          >
            {{ isUpdating ? "Saving..." : "Save Changes" }}
          </button>
        </div>

        <div class="profile-menu-item" @click="toggleSection('paymentMethods')">
          <div class="menu-item-icon">
            <font-awesome-icon icon="credit-card" />
          </div>
          <div class="menu-item-text">Payment Methods</div>
          <div class="menu-item-arrow">
            <font-awesome-icon
              :icon="
                expandedSections.paymentMethods ? 'chevron-up' : 'chevron-down'
              "
            />
          </div>
        </div>

        <div v-if="expandedSections.paymentMethods" class="profile-submenu">
          <div
            v-for="(method, index) in paymentMethods"
            :key="index"
            class="payment-method-item"
          >
            <div class="payment-method-icon">
              <font-awesome-icon
                :icon="method.type === 'credit' ? 'credit-card' : 'money-bill'"
              />
            </div>
            <div class="payment-method-details">
              <div class="payment-method-name">{{ method.name }}</div>
              <div class="payment-method-info">{{ method.info }}</div>
            </div>
            <div class="payment-method-default" v-if="method.isDefault">
              Default
            </div>
          </div>

          <button class="btn-secondary add-payment-btn">
            <font-awesome-icon icon="plus" /> Add Payment Method
          </button>
        </div>
      </div>

      <!-- Settings Section -->
      <div class="profile-section">
        <h3 class="section-title">Settings</h3>

        <div class="profile-menu-item">
          <div class="menu-item-icon">
            <font-awesome-icon icon="bell" />
          </div>
          <div class="menu-item-text">Notifications</div>
          <div class="toggle-switch">
            <input
              type="checkbox"
              id="notifications"
              v-model="settings.notifications"
            />
            <label for="notifications"></label>
          </div>
        </div>

        <div class="profile-menu-item">
          <div class="menu-item-icon">
            <font-awesome-icon icon="moon" />
          </div>
          <div class="menu-item-text">Dark Mode</div>
          <div class="toggle-switch">
            <input type="checkbox" id="darkMode" v-model="settings.darkMode" />
            <label for="darkMode"></label>
          </div>
        </div>

        <div class="profile-menu-item">
          <div class="menu-item-icon">
            <font-awesome-icon icon="language" />
          </div>
          <div class="menu-item-text">Language</div>
          <div class="menu-item-value">English</div>
        </div>
      </div>

      <!-- Support Section -->
      <div class="profile-section">
        <h3 class="section-title">Support</h3>

        <div class="profile-menu-item">
          <div class="menu-item-icon">
            <font-awesome-icon icon="question-circle" />
          </div>
          <div class="menu-item-text">Help Center</div>
          <div class="menu-item-arrow">
            <font-awesome-icon icon="chevron-right" />
          </div>
        </div>

        <div class="profile-menu-item">
          <div class="menu-item-icon">
            <font-awesome-icon icon="headset" />
          </div>
          <div class="menu-item-text">Contact Support</div>
          <div class="menu-item-arrow">
            <font-awesome-icon icon="chevron-right" />
          </div>
        </div>

        <div class="profile-menu-item">
          <div class="menu-item-icon">
            <font-awesome-icon icon="file-alt" />
          </div>
          <div class="menu-item-text">Terms & Conditions</div>
          <div class="menu-item-arrow">
            <font-awesome-icon icon="chevron-right" />
          </div>
        </div>
      </div>

      <!-- Logout Button -->
      <button class="btn-logout" @click="handleLogout" :disabled="isLoggingOut">
        <font-awesome-icon icon="sign-out-alt" />
        {{ isLoggingOut ? "Logging out..." : "Logout" }}
      </button>
    </div>

    <!-- Success Toast -->
    <div v-if="showToast" class="toast-message" :class="{ show: showToast }">
      <div class="toast-icon">
        <font-awesome-icon
          :icon="
            toastType === 'success' ? 'check-circle' : 'exclamation-circle'
          "
        />
      </div>
      <div class="toast-text">{{ toastMessage }}</div>
    </div>

    <!-- Bottom Navigation -->
    <div class="bottom-nav">
      <router-link to="/home" class="bottom-nav-item">
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
      <router-link to="/profile" class="bottom-nav-item active">
        <font-awesome-icon icon="user" class="bottom-nav-icon" />
        <span>Profile</span>
      </router-link>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, onMounted, nextTick, watch } from "vue";
import { useRouter } from "vue-router";
import { useAuthStore } from "../stores/auth";
import { FontAwesomeIcon } from "@fortawesome/vue-fontawesome";

const auth = useAuthStore();
const router = useRouter();
const fileInput = ref(null);
const nameInput = ref(null);
const defaultAvatar = "/images/default-avatar.png";
const API_BASE = "https://asoms-production.up.railway.app";

// Loading states
const isUpdating = ref(false);
const isLoggingOut = ref(false);

console.log("Auth first: ", auth.user);
// Profile image with fallback
const profileImage = computed(() => {
  return auth.user?.profilePictureUrl || defaultAvatar;
});

console.log("Profile Image: ", profileImage);

// Editable name
const isEditingName = ref(false);
const editableName = ref("");

// Order statistics (could be moved to a separate store later)
const orderStats = ref({
  total: 12,
  completed: 8,
  pending: 4,
});

// Expanded sections
const expandedSections = ref({
  personalInfo: false,
  paymentMethods: false,
});

// Personal information - reactive to store changes
const personalInfo = ref({
  fullName: "",
  phone: "",
});

// Payment methods
const paymentMethods = ref([
  {
    type: "Invoice",
    name: "Invoice",
    info: "Take now, Pay later",
    isDefault: false,
  },
  {
    type: "cash",
    name: "Cash on Pick-up",
    info: "Pay when you receive your order",
    isDefault: true,
  },
]);

// Settings
const settings = ref({
  notifications: true,
  darkMode: true,
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

// Watch for auth.user changes to update local reactive data
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
    try {
      // Create a preview immediately for better UX
      const reader = new FileReader();
      reader.onload = (e) => {
        // We can't directly update the computed, but we can show a preview
        // The actual update will happen through the store
      };
      reader.readAsDataURL(file);

      // Upload to server through store
      const formData = new FormData();
      formData.append("profilePicture", file);

      // You would need to add this method to your auth store
      await auth.uploadProfilePicture(formData);

      showToastMessage("Profile picture updated successfully", "success");
    } catch (error) {
      console.error("Error uploading profile picture:", error);
      showToastMessage("Failed to update profile picture", "error");
    }
  }
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

    // Update through store
    await auth.updateUserProfile({
      fullName: personalInfo.value.fullName,
      contactNumber: personalInfo.value.phone,
      address: personalInfo.value.address,
    });

    // Update editable name
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
    // Still redirect to login even if the API call fails
    auth.logout();
    router.push("/login");
  } finally {
    isLoggingOut.value = false;
  }
};

// Initialize data on component mount
onMounted(async () => {
  // Restore auth state from localStorage if needed
  auth.restore();

  // Fetch latest user data if logged in
  /*if (auth.isLoggedIn) {
    try {
      await auth.fetchUser()
    } catch (error) {
      console.error('Failed to fetch user data:', error)
    }
  } else {
    // Redirect to login if not authenticated
    router.push('/login')
  }*/
});
</script>

<style scoped>
/* The CSS remains the same as in your original component */
.profile-header {
  position: relative;
  margin-bottom: 60px;
}

.profile-cover {
  height: 150px;
  background: linear-gradient(to right, #1a1a1a, #333333);
  border-radius: 10px;
  margin-bottom: 50px;
}

.profile-avatar-container {
  position: absolute;
  left: 50%;
  top: 120px;
  transform: translateX(-50%);
}

.profile-avatar {
  width: 100px;
  height: 100px;
  border-radius: 50%;
  background-color: var(--card-bg);
  border: 4px solid var(--background-color);
  overflow: hidden;
  display: flex;
  align-items: center;
  justify-content: center;
  position: relative;
  cursor: pointer;
}

.profile-avatar img {
  width: 100%;
  height: 100%;
  object-fit: cover;
}

.profile-avatar-placeholder {
  font-size: 36px;
  font-weight: 600;
  color: var(--primary-color);
}

.edit-avatar-icon {
  position: absolute;
  bottom: 0;
  right: 0;
  background-color: var(--primary-color);
  width: 30px;
  height: 30px;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  color: white;
  font-size: 14px;
}

.profile-name-container {
  text-align: center;
  margin-top: 10px;
}

.profile-name-container h2 {
  font-size: 24px;
  font-weight: 600;
  margin-bottom: 5px;
  cursor: pointer;
}

.profile-email {
  font-size: 14px;
  color: #cccccc;
}

.edit-name-container {
  margin-bottom: 5px;
}

.edit-name-input {
  background-color: var(--input-bg);
  border: none;
  border-radius: 8px;
  padding: 8px 15px;
  color: white;
  width: 100%;
  text-align: center;
  font-size: 20px;
  font-weight: 600;
}

.profile-stats {
  display: flex;
  justify-content: space-around;
  background-color: var(--card-bg);
  border-radius: 10px;
  padding: 15px;
  margin-bottom: 20px;
}

.stat-item {
  display: flex;
  flex-direction: column;
  align-items: center;
}

.stat-value {
  font-size: 24px;
  font-weight: 600;
  color: var(--primary-color);
}

.stat-label {
  font-size: 14px;
  color: #cccccc;
}

.profile-sections {
  margin-bottom: 80px;
}

.profile-section {
  margin-bottom: 20px;
}

.section-title {
  font-size: 18px;
  font-weight: 600;
  margin-bottom: 15px;
  color: #ffffff;
}

.profile-menu-item {
  display: flex;
  align-items: center;
  background-color: var(--card-bg);
  border-radius: 10px;
  padding: 15px;
  margin-bottom: 10px;
  cursor: pointer;
}

.menu-item-icon {
  width: 40px;
  height: 40px;
  background-color: rgba(255, 75, 38, 0.1);
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  color: var(--primary-color);
  margin-right: 15px;
}

.menu-item-text {
  flex: 1;
  font-size: 16px;
}

.menu-item-arrow,
.menu-item-value {
  color: #cccccc;
}

.profile-submenu {
  background-color: var(--card-bg);
  border-radius: 10px;
  padding: 15px;
  margin-top: -10px;
  margin-bottom: 10px;
}

.profile-form-group {
  margin-bottom: 15px;
}

.profile-form-group label {
  display: block;
  margin-bottom: 8px;
  font-size: 14px;
  color: #cccccc;
}

.profile-input,
.profile-textarea {
  background-color: var(--input-bg);
  border: none;
  border-radius: 8px;
  padding: 12px 15px;
  color: white;
  width: 100%;
}

.profile-textarea {
  min-height: 80px;
  resize: vertical;
}

.save-btn {
  margin-top: 10px;
}

.save-btn:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}

.payment-method-item {
  display: flex;
  align-items: center;
  padding: 10px 0;
  border-bottom: 1px solid var(--input-bg);
}

.payment-method-item:last-child {
  border-bottom: none;
}

.payment-method-icon {
  width: 40px;
  height: 40px;
  background-color: rgba(255, 255, 255, 0.1);
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  margin-right: 15px;
}

.payment-method-details {
  flex: 1;
}

.payment-method-name {
  font-size: 16px;
  margin-bottom: 5px;
}

.payment-method-info {
  font-size: 14px;
  color: #cccccc;
}

.payment-method-default {
  font-size: 12px;
  color: var(--primary-color);
  background-color: rgba(255, 75, 38, 0.1);
  padding: 4px 8px;
  border-radius: 4px;
}

.add-payment-btn {
  margin-top: 15px;
  width: 100%;
}

.toggle-switch {
  position: relative;
  display: inline-block;
  width: 50px;
  height: 24px;
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
  background-color: var(--input-bg);
  transition: 0.4s;
  border-radius: 34px;
}

.toggle-switch label:before {
  position: absolute;
  content: "";
  height: 16px;
  width: 16px;
  left: 4px;
  bottom: 4px;
  background-color: white;
  transition: 0.4s;
  border-radius: 50%;
}

.toggle-switch input:checked + label {
  background-color: var(--primary-color);
}

.toggle-switch input:checked + label:before {
  transform: translateX(26px);
}

.btn-logout {
  background-color: transparent;
  border: 1px solid #ff4b26;
  color: #ff4b26;
  padding: 12px 20px;
  border-radius: 8px;
  font-weight: 600;
  width: 100%;
  cursor: pointer;
  font-size: 16px;
  margin-top: 20px;
  transition: all 0.3s ease;
}

.btn-logout:hover {
  background-color: rgba(255, 75, 38, 0.1);
}

.btn-logout:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}

.toast-message {
  position: fixed;
  bottom: 100px;
  left: 50%;
  transform: translateX(-50%) translateY(100px);
  background-color: #333333;
  color: white;
  padding: 15px 20px;
  border-radius: 8px;
  display: flex;
  align-items: center;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.2);
  transition: transform 0.3s ease;
  z-index: 1000;
}

.toast-message.show {
  transform: translateX(-50%) translateY(0);
}

.toast-icon {
  margin-right: 10px;
  font-size: 20px;
}

.toast-icon .fa-check-circle {
  color: #00ff00;
}

.toast-icon .fa-exclamation-circle {
  color: #ff4b26;
}

.toast-text {
  font-size: 14px;
}
</style>
