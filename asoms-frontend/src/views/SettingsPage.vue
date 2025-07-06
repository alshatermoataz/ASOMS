<template>
  <div class="admin-dashboard-desktop-layout">
    <!-- Sidebar Navigation -->
    <aside :class="['sidebar-nav-panel', { collapsed: isSidebarCollapsed }]">
      <div class="sidebar-header">
        <div v-if="!isSidebarCollapsed" class="sidebar-logo-container">
          <i class="fas fa-shield-alt sidebar-logo-icon"></i>
          <span class="sidebar-logo-text">ASOMS Admin</span>
        </div>
        <button class="sidebar-toggle-btn" @click="toggleSidebar">
          <i
            :class="
              isSidebarCollapsed
                ? 'fas fa-chevron-right'
                : 'fas fa-chevron-left'
            "
          ></i>
        </button>
      </div>
      <nav class="sidebar-menu">
        <ul>
          <li>
            <router-link to="/admin/analytics" class="nav-item"
              ><i class="fas fa-tachometer-alt fa-fw"></i
              ><span v-if="!isSidebarCollapsed">Dashboard</span></router-link
            >
          </li>
          <li>
            <router-link to="/admin/orders" class="nav-item"
              ><i class="fas fa-shopping-cart fa-fw"></i
              ><span v-if="!isSidebarCollapsed">Orders</span></router-link
            >
          </li>
          <li>
            <router-link to="/admin/customers" class="nav-item"
              ><i class="fas fa-users fa-fw"></i
              ><span v-if="!isSidebarCollapsed">Customers</span></router-link
            >
          </li>
          <li>
            <router-link to="/admin/products" class="nav-item"
              ><i class="fas fa-box-open fa-fw"></i
              ><span v-if="!isSidebarCollapsed">Products</span></router-link
            >
          </li>
          <!-- <li>
            <router-link to="/admin/analytics" class="nav-item"
              ><i class="fas fa-chart-line fa-fw"></i
              ><span v-if="!isSidebarCollapsed">Analytics</span></router-link
            >
          </li> -->
          <li>
            <router-link to="/admin/settings" class="nav-item active"
              ><i class="fas fa-cog fa-fw"></i
              ><span v-if="!isSidebarCollapsed">Settings</span></router-link
            >
          </li>
        </ul>
      </nav>
    </aside>

    <!-- Main Panel -->
    <div class="main-panel-content">
      <!-- Top Navigation -->
      <nav class="top-navbar-main">
        <div class="navbar-content-main">
          <div class="navbar-left-main">
            <div class="navbar-brand-section">
              <i class="fas fa-cog navbar-brand-icon"></i>
              <span class="navbar-brand-text">System Settings</span>
            </div>
          </div>
          <div class="navbar-right-main">
            <!-- <button class="notification-btn">
              <i class="fas fa-bell"></i>
              <span class="notification-badge">1</span>
            </button> -->
            <div class="user-profile-section">
              <img
                src="../assets/placeholder.svg?width=40&height=40"
                alt="Admin User"
                class="user-avatar"
              />
              <div class="user-details">
                <span class="user-name">Admin User</span>
                <span class="user-role">Administrator</span>
              </div>
            </div>
          </div>
        </div>
      </nav>

      <!-- Main Content -->
      <main class="content-area-wrapper">
        <!-- Settings Navigation -->
        <section class="settings-navigation-section">
          <div class="settings-nav-container">
            <div class="settings-nav-header">
              <h2 class="settings-nav-title">
                <i class="fas fa-sliders-h"></i>
                Settings
              </h2>
            </div>
            <div class="settings-nav-tabs">
              <button
                :class="['settings-tab', { active: activeTab === 'general' }]"
                @click="activeTab = 'general'"
              >
                <i class="fas fa-cogs"></i>
                <span>General</span>
              </button>
              <button
                :class="['settings-tab', { active: activeTab === 'admins' }]"
                @click="activeTab = 'admins'"
              >
                <i class="fas fa-user-shield"></i>
                <span>Admin Management</span>
              </button>
            </div>
          </div>
        </section>

        <!-- Settings Content -->
        <section class="settings-content-section">
          <!-- General Settings -->
          <div v-if="activeTab === 'general'" class="settings-panel">
            <div class="panel-header">
              <h3 class="panel-title">
                <i class="fas fa-cogs"></i>
                General Settings
              </h3>
              <p class="panel-description">
                Configure basic system settings and preferences
              </p>
            </div>

            <div class="settings-grid">
              <!-- Company Information -->
              <div class="settings-card">
                <div class="card-header">
                  <h4 class="card-title">
                    <i class="fas fa-building"></i>
                    Company Information
                  </h4>
                </div>
                <div class="card-content">
                  <div class="form-grid">
                    <div class="form-group">
                      <label class="form-label">Company Name</label>
                      <input
                        v-model="settings.general.companyName"
                        type="text"
                        class="form-input"
                        placeholder="Enter company name"
                      />
                    </div>
                    <div class="form-group">
                      <label class="form-label">Business Email</label>
                      <input
                        v-model="settings.general.businessEmail"
                        type="email"
                        class="form-input"
                        placeholder="Enter business email"
                      />
                    </div>
                    <div class="form-group">
                      <label class="form-label">Phone Number</label>
                      <input
                        v-model="settings.general.phoneNumber"
                        type="tel"
                        class="form-input"
                        placeholder="Enter phone number"
                      />
                    </div>
                    <div class="form-group">
                      <label class="form-label">Website URL</label>
                      <input
                        v-model="settings.general.websiteUrl"
                        type="url"
                        class="form-input"
                        placeholder="Enter website URL"
                      />
                    </div>
                    <div class="form-group full-width">
                      <label class="form-label">Business Address</label>
                      <textarea
                        v-model="settings.general.businessAddress"
                        class="form-textarea"
                        rows="3"
                        placeholder="Enter complete business address"
                      ></textarea>
                    </div>
                  </div>
                </div>
              </div>

              <!-- System Preferences -->
              <!-- <div class="settings-card">
                <div class="card-header">
                  <h4 class="card-title">
                    <i class="fas fa-sliders-h"></i>
                    System Preferences
                  </h4>
                </div>
                <div class="card-content">
                  <div class="preferences-list">
                    <div class="preference-item">
                      <div class="preference-info">
                        <div class="preference-name">Default Currency</div>
                        <div class="preference-description">
                          Set the default currency for all transactions
                        </div>
                      </div>
                      <select
                        v-model="settings.general.defaultCurrency"
                        class="preference-select"
                      >
                        <option value="USD">USD - US Dollar</option>
                        <option value="EUR">EUR - Euro</option>
                        <option value="GBP">GBP - British Pound</option>
                        <option value="MYR">MYR - Malaysian Ringgit</option>
                      </select>
                    </div>

                    <div class="preference-item">
                      <div class="preference-info">
                        <div class="preference-name">Time Zone</div>
                        <div class="preference-description">
                          Set the system time zone
                        </div>
                      </div>
                      <select
                        v-model="settings.general.timeZone"
                        class="preference-select"
                      >
                        <option value="UTC">
                          UTC - Coordinated Universal Time
                        </option>
                        <option value="Asia/Kuala_Lumpur">
                          Asia/Kuala_Lumpur - Malaysia Time
                        </option>
                        <option value="EST">EST - Eastern Standard Time</option>
                        <option value="PST">PST - Pacific Standard Time</option>
                      </select>
                    </div>

                    <div class="preference-item">
                      <div class="preference-info">
                        <div class="preference-name">Date Format</div>
                        <div class="preference-description">
                          Choose how dates are displayed
                        </div>
                      </div>
                      <select
                        v-model="settings.general.dateFormat"
                        class="preference-select"
                      >
                        <option value="MM/DD/YYYY">MM/DD/YYYY</option>
                        <option value="DD/MM/YYYY">DD/MM/YYYY</option>
                        <option value="YYYY-MM-DD">YYYY-MM-DD</option>
                      </select>
                    </div>
                  </div>
                </div>
              </div> -->
            </div>

            <!-- Save Button for General Settings -->
            <div class="settings-actions">
              <button
                class="save-btn"
                @click="saveGeneralSettings"
                :disabled="isSaving"
              >
                <i class="fas fa-spinner fa-spin" v-if="isSaving"></i>
                <i class="fas fa-save" v-else></i>
                {{ isSaving ? "Saving..." : "Save Changes" }}
              </button>
            </div>
          </div>

          <!-- Admin Management -->
          <div v-if="activeTab === 'admins'" class="settings-panel">
            <div class="panel-header">
              <h3 class="panel-title">
                <i class="fas fa-user-shield"></i>
                Admin Management
              </h3>
              <p class="panel-description">
                Manage administrator accounts and permissions
              </p>
            </div>

            <!-- Admin Controls -->
            <div class="admin-controls">
              <div class="controls-left">
                <div class="search-container">
                  <i class="fas fa-search search-icon"></i>
                  <input
                    v-model="adminFilters.search"
                    type="text"
                    class="search-input"
                    placeholder="Search admins..."
                    @input="debouncedAdminSearch"
                  />
                </div>
              </div>
              <div class="controls-right">
                <button class="add-admin-btn" @click="openAddAdminModal">
                  <i class="fas fa-plus"></i>
                  Add Admin
                </button>
              </div>
            </div>

            <!-- Admins Table -->
            <div class="admins-table-container">
              <div v-if="isLoadingAdmins" class="loading-state">
                <div class="loading-spinner"></div>
                <p>Loading administrators...</p>
              </div>

              <div v-else-if="admins.length > 0" class="table-wrapper">
                <table class="admins-table">
                  <thead>
                    <tr>
                      <th>Profile</th>
                      <th>Name</th>
                      <th>Email</th>
                      <th>Role</th>
                      <!-- <th>Status</th> -->
                      <th>Created</th>
                      <th>Actions</th>
                    </tr>
                  </thead>
                  <tbody>
                    <tr
                      v-for="admin in admins"
                      :key="admin.id"
                      class="admin-row"
                    >
                      <td class="profile-cell">
                        <div class="profile-avatar">
                          <img
                            v-if="admin.profilePictureUrl"
                            :src="imageBase(admin.profilePictureUrl)"
                            :alt="admin.fullName"
                            class="avatar-img"
                          />
                          <i v-else class="fas fa-user-shield"></i>
                        </div>
                      </td>
                      <td class="name-cell">
                        <div class="admin-name">{{ admin.fullName }}</div>
                      </td>
                      <td class="email-cell">
                        <div class="admin-email">{{ admin.email }}</div>
                      </td>
                      <td class="role-cell">
                        <span class="role-badge">{{ admin.role }}</span>
                      </td>
                      <!-- <td class="status-cell">
                        <span
                          :class="[
                            'status-badge',
                            admin.isActive ? 'active' : 'inactive',
                          ]"
                        >
                          {{ admin.isActive ? "Active" : "Inactive" }}
                        </span>
                      </td> -->
                      <td class="date-cell">
                        <div class="created-date">
                          {{ formatDate(admin.createdAt) }}
                        </div>
                      </td>
                      <td class="actions-cell">
                        <div class="action-buttons">
                          <button
                            class="action-btn edit-btn"
                            @click="editAdmin(admin)"
                            title="Edit Admin"
                          >
                            <i class="fas fa-edit"></i>
                          </button>
                          <button
                            class="action-btn delete-btn"
                            @click="deleteAdmin(admin)"
                            title="Delete Admin"
                            :disabled="admin.id === currentUserId"
                          >
                            <i class="fas fa-trash"></i>
                          </button>
                        </div>
                      </td>
                    </tr>
                  </tbody>
                </table>
              </div>

              <div v-else class="empty-state">
                <div class="empty-icon">
                  <i class="fas fa-user-shield"></i>
                </div>
                <h3>No Administrators Found</h3>
                <p>No administrators match your search criteria.</p>
                <button class="add-admin-btn" @click="openAddAdminModal">
                  <i class="fas fa-plus"></i>
                  Add First Admin
                </button>
              </div>
            </div>
          </div>
        </section>
      </main>
    </div>

    <!-- Add/Edit Admin Modal -->
    <div
      v-if="showAdminModal"
      class="modal-overlay"
      @click.self="closeAdminModal"
    >
      <div class="modal-container admin-modal">
        <div class="modal-header">
          <h3 class="modal-title">
            <i class="fas fa-user-shield"></i>
            {{ editingAdmin ? "Edit Administrator" : "Add New Administrator" }}
          </h3>
          <button class="modal-close" @click="closeAdminModal">
            <i class="fas fa-times"></i>
          </button>
        </div>
        <div class="modal-body">
          <form @submit.prevent="saveAdmin" class="admin-form">
            <!-- Profile Picture Upload -->
            <div class="form-group profile-upload">
              <label class="form-label">Profile Picture</label>
              <div class="image-upload-container">
                <div class="image-preview">
                  <img
                    v-if="
                      imageBase(adminForm.image) || adminForm.profilePictureUrl
                    "
                    :src="
                      imageBase(adminForm.image) || adminForm.profilePictureUrl
                    "
                    alt="Profile Preview"
                    class="preview-img"
                  />
                  <div v-else class="preview-placeholder">
                    <i class="fas fa-user-shield"></i>
                    <span>No Image</span>
                  </div>
                </div>
                <div class="upload-controls">
                  <input
                    type="file"
                    ref="imageInput"
                    @change="handleImageUpload"
                    accept="image/*"
                    class="file-input"
                  />
                  <button
                    type="button"
                    class="upload-btn"
                    @click="$refs.imageInput.click()"
                  >
                    <i class="fas fa-camera"></i>
                    Choose Image
                  </button>
                  <button
                    v-if="adminForm.image || adminForm.profilePictureUrl"
                    type="button"
                    class="remove-btn"
                    @click="removeImage"
                  >
                    <i class="fas fa-trash"></i>
                    Remove
                  </button>
                </div>
              </div>
            </div>

            <div class="form-row">
              <div class="form-group">
                <label class="form-label">Full Name *</label>
                <input
                  v-model="adminForm.fullName"
                  type="text"
                  class="form-input"
                  required
                />
              </div>
              <div class="form-group">
                <label class="form-label">Email *</label>
                <input
                  v-model="adminForm.email"
                  type="email"
                  class="form-input"
                  required
                />
              </div>
            </div>

            <div v-if="!editingAdmin" class="form-group">
              <label class="form-label">Password *</label>
              <input
                v-model="adminForm.password"
                type="password"
                class="form-input"
                required
              />
            </div>

            <div class="form-row">
              <div class="form-group">
                <label class="form-label">Contact Number</label>
                <input
                  v-model="adminForm.contactNumber"
                  type="tel"
                  class="form-input"
                />
              </div>
              <!-- <div class="form-group">
                <label class="form-label">Status</label>
                <select v-model="adminForm.isActive" class="form-select">
                  <option :value="true">Active</option>
                  <option :value="false">Inactive</option>
                </select>
              </div> -->
            </div>

            <div class="modal-actions">
              <button
                type="button"
                class="cancel-btn"
                @click="closeAdminModal"
                :disabled="isSavingAdmin"
              >
                Cancel
              </button>
              <button type="submit" class="save-btn" :disabled="isSavingAdmin">
                <i class="fas fa-spinner fa-spin" v-if="isSavingAdmin"></i>
                <i class="fas fa-save" v-else></i>
                {{
                  isSavingAdmin
                    ? "Saving..."
                    : editingAdmin
                    ? "Update"
                    : "Create"
                }}
              </button>
            </div>
          </form>
        </div>
      </div>
    </div>

    <!-- Toast Messages -->
    <div class="toast-container">
      <div
        v-for="(message, index) in messages"
        :key="index"
        :class="['toast-message', `toast-${message.type}`]"
      >
        <div class="toast-icon">
          <i
            :class="`fas ${
              message.type === 'success'
                ? 'fa-check-circle'
                : 'fa-exclamation-triangle'
            }`"
          ></i>
        </div>
        <div class="toast-content">
          <div class="toast-title">
            {{ message.type === "success" ? "Success" : "Error" }}
          </div>
          <div class="toast-text">{{ message.text }}</div>
        </div>
        <button class="toast-close" @click="removeMessage(index)">
          <i class="fas fa-times"></i>
        </button>
      </div>
    </div>
  </div>
</template>

<script>
import { ref, reactive, onMounted } from "vue";
import axios from "axios";

export default {
  name: "SettingsPage",
  setup() {
    const isSidebarCollapsed = ref(false);
    const activeTab = ref("general");
    const isSaving = ref(false);
    const isSavingAdmin = ref(false);
    const isLoadingAdmins = ref(false);
    const messages = ref([]);
    const currentUserId = ref("current-user-id"); // You should get this from your auth system

    // General Settings
    const settings = reactive({
      general: {
        companyName: "ASOMS Car Products",
        businessEmail: "admin@asoms.com",
        phoneNumber: "+60 12-345-6789",
        websiteUrl: "https://asoms.com",
        businessAddress: "123 Main Street, Kuala Lumpur, Malaysia",
        defaultCurrency: "MYR",
        timeZone: "Asia/Kuala_Lumpur",
        dateFormat: "DD/MM/YYYY",
      },
    });

    // Admin Management
    const admins = ref([]);
    const showAdminModal = ref(false);
    const editingAdmin = ref(null);
    const adminFilters = ref({
      search: "",
    });

    const adminForm = ref({
      fullName: "",
      email: "",
      password: "",
      contactNumber: "",
      isActive: true,
      profilePictureUrl: null,
      image: null,
    });

    const API_BASE_URL = "https://asoms-production.up.railway.app";

    // Methods
    const toggleSidebar = () => {
      isSidebarCollapsed.value = !isSidebarCollapsed.value;
    };

    const saveGeneralSettings = async () => {
      isSaving.value = true;
      try {
        // Here you would make an API call to save general settings
        await new Promise((resolve) => setTimeout(resolve, 1000)); // Simulate API call
        showMessage("General settings saved successfully!", "success");
      } catch (error) {
        showMessage("Failed to save general settings.", "error");
      } finally {
        isSaving.value = false;
      }
    };

    const loadAdmins = async () => {
      isLoadingAdmins.value = true;
      try {
        const response = await axios.get(`${API_BASE_URL}/api/Users`, {
          params: {
            role: "Admin",
            search: adminFilters.value.search || undefined,
          },
        });
        admins.value = response.data.data || [];
      } catch (error) {
        console.error("Error loading admins:", error);
        showMessage("Error loading administrators.", "error");
        admins.value = [];
      } finally {
        isLoadingAdmins.value = false;
      }
    };

    const openAddAdminModal = () => {
      editingAdmin.value = null;
      adminForm.value = {
        fullName: "",
        email: "",
        password: "",
        contactNumber: "",
        isActive: true,
        profilePictureUrl: null,
        image: null,
      };
      showAdminModal.value = true;
    };

    const editAdmin = (admin) => {
      editingAdmin.value = admin;
      adminForm.value = {
        fullName: admin.fullName,
        email: admin.email,
        password: "", // Don't populate password for editing
        contactNumber: admin.contactNumber || "",
        isActive: admin.isActive,
        profilePictureUrl: admin.profilePictureUrl,
        image: null,
      };
      showAdminModal.value = true;
    };

    const closeAdminModal = () => {
      showAdminModal.value = false;
      setTimeout(() => {
        editingAdmin.value = null;
        adminForm.value = {
          fullName: "",
          email: "",
          password: "",
          contactNumber: "",
          isActive: true,
          profilePictureUrl: null,
          image: null,
        };
      }, 300);
    };

    const handleImageUpload = (event) => {
      const file = event.target.files[0];
      if (file) {
        adminForm.value.image = file;
      }
    };

    const imageBase = (url) => {
      if (!url) return null;
      return url.startsWith("http") ? url : `${API_BASE_URL}${url}`;
    };

    const removeImage = () => {
      adminForm.value.image = null;
      adminForm.value.profilePictureUrl = null;
    };

    const saveAdmin = async () => {
      isSavingAdmin.value = true;
      try {
        if (editingAdmin.value) {
          // Update existing admin
          await axios.put(
            `${API_BASE_URL}/api/Users/${editingAdmin.value.id}`,
            {
              fullName: adminForm.value.fullName,
              email: adminForm.value.email,
              contactNumber: adminForm.value.contactNumber,
              profilePictureUrl: adminForm.value.profilePictureUrl,
            }
          );

          // Upload new image if provided
          if (adminForm.value.image) {
            const formData = new FormData();
            formData.append("file", adminForm.value.image);

            await axios.post(
              `${API_BASE_URL}/api/Users/${editingAdmin.value.id}/upload-profile-picture`,
              formData,
              {
                headers: {
                  "Content-Type": "multipart/form-data",
                },
              }
            );
          }

          showMessage("Administrator updated successfully!", "success");
        } else {
          // Create new admin
          const response = await axios.post(`${API_BASE_URL}/api/Users`, {
            email: adminForm.value.email,
            password: adminForm.value.password,
            fullName: adminForm.value.fullName,
            contactNumber: adminForm.value.contactNumber,
            role: "Admin",
            profilePictureUrl: null,
          });

          // Upload image if provided
          if (adminForm.value.image && response.data.userId) {
            const formData = new FormData();
            formData.append("file", adminForm.value.image);

            await axios.post(
              `${API_BASE_URL}/Users/${response.data.userId}/api/upload-profile-picture`,
              formData,
              {
                headers: {
                  "Content-Type": "multipart/form-data",
                },
              }
            );
          }

          showMessage("Administrator created successfully!", "success");
        }

        await loadAdmins();
        closeAdminModal();
      } catch (error) {
        console.error("Error saving admin:", error);
        showMessage("Error saving administrator.", "error");
      } finally {
        isSavingAdmin.value = false;
      }
    };

    const deleteAdmin = async (admin) => {
      if (admin.id === currentUserId.value) {
        showMessage("You cannot delete your own account.", "error");
        return;
      }

      if (confirm(`Are you sure you want to delete ${admin.fullName}?`)) {
        try {
          await axios.delete(`${API_BASE_URL}/api/User/${admin.id}`);
          showMessage("Administrator deleted successfully!", "success");
          await loadAdmins();
        } catch (error) {
          console.error("Error deleting admin:", error);
          showMessage("Error deleting administrator.", "error");
        }
      }
    };

    const formatDate = (dateString) => {
      if (!dateString) return "N/A";
      return new Date(dateString).toLocaleDateString("en-US", {
        year: "numeric",
        month: "short",
        day: "numeric",
      });
    };

    let searchTimeout = null;
    const debouncedAdminSearch = () => {
      clearTimeout(searchTimeout);
      searchTimeout = setTimeout(() => {
        loadAdmins();
      }, 500);
    };

    const showMessage = (text, type = "info") => {
      const id = Date.now();
      messages.value.push({ id, text, type });
      setTimeout(() => {
        messages.value = messages.value.filter((m) => m.id !== id);
      }, 5000);
    };

    const removeMessage = (index) => {
      messages.value.splice(index, 1);
    };

    onMounted(() => {
      loadAdmins();
    });

    return {
      isSidebarCollapsed,
      activeTab,
      isSaving,
      isSavingAdmin,
      isLoadingAdmins,
      messages,
      currentUserId,
      settings,
      admins,
      showAdminModal,
      editingAdmin,
      adminFilters,
      adminForm,
      toggleSidebar,
      saveGeneralSettings,
      loadAdmins,
      openAddAdminModal,
      editAdmin,
      closeAdminModal,
      handleImageUpload,
      imageBase,
      removeImage,
      saveAdmin,
      deleteAdmin,
      formatDate,
      debouncedAdminSearch,
      showMessage,
      removeMessage,
    };
  },
};
</script>

<style scoped>
/* Base Layout */
.admin-dashboard-desktop-layout {
  display: flex;
  width: 100vw;
  height: 100vh;
  overflow: hidden;
  background: linear-gradient(135deg, #f5f7fa 0%, #c3cfe2 100%);
  font-family: "Segoe UI", Tahoma, Geneva, Verdana, sans-serif;
}

/* Sidebar Navigation */
.sidebar-nav-panel {
  width: 16rem;
  background: linear-gradient(180deg, #2c3e50 0%, #34495e 100%);
  color: #ecf0f1;
  display: flex;
  flex-direction: column;
  transition: width 0.3s ease-in-out;
  flex-shrink: 0;
  box-shadow: 2px 0 10px rgba(0, 0, 0, 0.1);
}

.sidebar-nav-panel.collapsed {
  width: 5rem;
}

.sidebar-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 1.125rem 1.25rem;
  border-bottom: 1px solid #4a627a;
  height: 4rem;
}

.sidebar-logo-container {
  display: flex;
  align-items: center;
  gap: 0.75rem;
  font-size: 1.3rem;
  font-weight: 600;
  color: #ffffff;
}

.sidebar-logo-icon {
  font-size: 1.6rem;
  color: #3498db;
}

.sidebar-toggle-btn {
  background: transparent;
  border: none;
  color: #bdc3c7;
  font-size: 1.3rem;
  cursor: pointer;
  padding: 0.5rem;
  border-radius: 0.25rem;
  transition: color 0.2s ease, background-color 0.2s ease;
}

.sidebar-toggle-btn:hover {
  color: #ffffff;
  background-color: rgba(255, 255, 255, 0.1);
}

.sidebar-menu {
  flex-grow: 1;
  overflow-y: auto;
  padding-top: 0.9375rem;
}

.sidebar-menu ul {
  list-style: none;
  padding: 0;
  margin: 0;
}

.sidebar-menu .nav-item {
  display: flex;
  align-items: center;
  padding: 0.875rem 1.25rem;
  color: #bdc3c7;
  text-decoration: none;
  transition: background 0.2s ease, color 0.2s ease;
  white-space: nowrap;
  overflow: hidden;
  font-size: 0.95rem;
}

.sidebar-menu .nav-item i.fa-fw {
  width: 1.25em;
  text-align: center;
  margin-right: 0.9375rem;
  font-size: 1.1rem;
}

.sidebar-nav-panel.collapsed .nav-item i.fa-fw {
  margin-right: 0;
}

.sidebar-menu .nav-item:hover,
.sidebar-menu .nav-item.active {
  background: rgba(255, 255, 255, 0.1);
  color: #ffffff;
  border-left: 3px solid #3498db;
  padding-left: 1.0625rem;
}

.sidebar-nav-panel.collapsed .nav-item.active {
  padding-left: 1.25rem;
}

.sidebar-nav-panel.collapsed .nav-item span {
  display: none;
}

/* Main Panel */
.main-panel-content {
  flex-grow: 1;
  display: flex;
  flex-direction: column;
  overflow: hidden;
  width: calc(100vw - 16rem);
}

.sidebar-nav-panel.collapsed + .main-panel-content {
  width: calc(100vw - 5rem);
}

/* Top Navigation */
.top-navbar-main {
  height: 4rem;
  background-color: #ffffff;
  border-bottom: 1px solid #e0e0e0;
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 0 1.5625rem;
  flex-shrink: 0;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.05);
}

.navbar-content-main {
  display: flex;
  align-items: center;
  justify-content: space-between;
  width: 100%;
}

.navbar-brand-section {
  display: flex;
  align-items: center;
  gap: 0.625rem;
  font-size: 1.3rem;
  font-weight: 600;
  color: #2c3e50;
}

.navbar-brand-icon {
  color: #3498db;
  font-size: 1.4rem;
}

.navbar-right-main {
  display: flex;
  align-items: center;
  gap: 1.25rem;
}

.notification-btn {
  background: transparent;
  border: none;
  font-size: 1.3rem;
  color: #555;
  position: relative;
  cursor: pointer;
  padding: 0.5rem;
  border-radius: 0.375rem;
  transition: background-color 0.2s ease;
}

.notification-btn:hover {
  background-color: #f0f2f5;
}

.notification-badge {
  position: absolute;
  top: -0.3125rem;
  right: -0.5rem;
  background-color: #e74c3c;
  color: white;
  font-size: 0.7rem;
  padding: 0.125rem 0.3125rem;
  border-radius: 50%;
  min-width: 1.125rem;
  height: 1.125rem;
  display: flex;
  align-items: center;
  justify-content: center;
}

.user-profile-section {
  display: flex;
  align-items: center;
  gap: 0.625rem;
  cursor: pointer;
  padding: 0.5rem 0.75rem;
  border-radius: 0.5rem;
  transition: background-color 0.2s ease;
}

.user-profile-section:hover {
  background-color: #f0f2f5;
}

.user-avatar {
  width: 2.5rem;
  height: 2.5rem;
  border-radius: 50%;
  object-fit: cover;
}

.user-details {
  display: flex;
  flex-direction: column;
}

.user-name {
  font-weight: 600;
  color: #2c3e50;
  font-size: 0.9rem;
}

.user-role {
  font-size: 0.8rem;
  color: #7f8c8d;
}

/* Content Area */
.content-area-wrapper {
  flex-grow: 1;
  overflow-y: auto;
  padding: 1.5625rem;
  display: flex;
  flex-direction: column;
  gap: 1.5625rem;
}

/* Settings Navigation */
.settings-navigation-section {
  background: white;
  border-radius: 1rem;
  padding: 1.5625rem;
  box-shadow: 0 0.25rem 1.25rem rgba(0, 0, 0, 0.08);
}

.settings-nav-container {
  display: flex;
  flex-direction: column;
  gap: 1.25rem;
}

.settings-nav-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.settings-nav-title {
  font-size: 1.3rem;
  font-weight: 700;
  color: #2c3e50;
  margin: 0;
  display: flex;
  align-items: center;
  gap: 0.75rem;
}

.settings-nav-tabs {
  display: flex;
  gap: 0.5rem;
}

.settings-tab {
  background: #f8f9fa;
  border: none;
  border-radius: 0.75rem;
  padding: 0.75rem 1.25rem;
  cursor: pointer;
  transition: all 0.3s ease;
  display: flex;
  align-items: center;
  gap: 0.5rem;
  font-weight: 600;
  color: #7f8c8d;
  white-space: nowrap;
}

.settings-tab:hover {
  background: #e9ecef;
  color: #2c3e50;
}

.settings-tab.active {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
  transform: translateY(-2px);
  box-shadow: 0 4px 12px rgba(102, 126, 234, 0.3);
}

/* Settings Content */
.settings-content-section {
  flex-grow: 1;
}

.settings-panel {
  background: white;
  border-radius: 1rem;
  padding: 1.5625rem;
  box-shadow: 0 0.25rem 1.25rem rgba(0, 0, 0, 0.08);
}

.panel-header {
  margin-bottom: 1.875rem;
  padding-bottom: 1.25rem;
  border-bottom: 2px solid #f1f3f4;
}

.panel-title {
  font-size: 1.5rem;
  font-weight: 700;
  color: #2c3e50;
  margin: 0 0 0.5rem 0;
  display: flex;
  align-items: center;
  gap: 0.75rem;
}

.panel-description {
  color: #7f8c8d;
  font-size: 1rem;
  margin: 0;
  line-height: 1.5;
}

/* Settings Grid */
.settings-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(22rem, 1fr));
  gap: 1.5625rem;
  margin-bottom: 2rem;
}

.settings-card {
  background: #f8f9fa;
  border-radius: 0.75rem;
  padding: 1.25rem;
  border: 1px solid #e9ecef;
  transition: all 0.3s ease;
}

.settings-card:hover {
  transform: translateY(-2px);
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
}

.card-header {
  margin-bottom: 1.25rem;
  padding-bottom: 0.75rem;
  border-bottom: 1px solid #e9ecef;
}

.card-title {
  font-size: 1.1rem;
  font-weight: 600;
  color: #2c3e50;
  margin: 0;
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.card-content {
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

/* Form Elements */
.form-grid {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 1rem;
}

.form-group {
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
}

.form-group.full-width {
  grid-column: span 2;
}

.form-row {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 1rem;
}

.form-label {
  font-weight: 600;
  color: #2c3e50;
  font-size: 0.9rem;
}

.form-input,
.form-textarea,
.form-select {
  padding: 0.75rem;
  border: 2px solid #e9ecef;
  border-radius: 0.5rem;
  font-size: 0.9rem;
  transition: all 0.3s ease;
  background: white;
}

.form-input:focus,
.form-textarea:focus,
.form-select:focus {
  outline: none;
  border-color: #667eea;
  box-shadow: 0 0 0 3px rgba(102, 126, 234, 0.1);
}

.form-textarea {
  resize: vertical;
  font-family: inherit;
}

/* Preferences */
.preferences-list {
  display: flex;
  flex-direction: column;
  gap: 1.25rem;
}

.preference-item {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 1rem;
  background: white;
  border-radius: 0.5rem;
  border: 1px solid #e9ecef;
}

.preference-info {
  flex: 1;
}

.preference-name {
  font-weight: 600;
  color: #2c3e50;
  font-size: 0.95rem;
  margin-bottom: 0.25rem;
}

.preference-description {
  font-size: 0.85rem;
  color: #7f8c8d;
}

.preference-select {
  padding: 0.5rem 0.75rem;
  border: 1px solid #e9ecef;
  border-radius: 0.375rem;
  font-size: 0.9rem;
  background: white;
  min-width: 10rem;
}

/* Settings Actions */
.settings-actions {
  display: flex;
  justify-content: flex-end;
  padding-top: 1.5rem;
  border-top: 2px solid #f1f3f4;
}

/* Admin Controls */
.admin-controls {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 1.5rem;
  padding: 1rem;
  background: #f8f9fa;
  border-radius: 0.75rem;
}

.controls-left {
  display: flex;
  align-items: center;
  gap: 1rem;
}

.search-container {
  position: relative;
}

.search-icon {
  position: absolute;
  left: 0.75rem;
  top: 50%;
  transform: translateY(-50%);
  color: #7f8c8d;
  font-size: 0.9rem;
}

.search-input {
  width: 20rem;
  padding: 0.5rem 0.75rem 0.5rem 2.25rem;
  border: 2px solid #e9ecef;
  border-radius: 0.5rem;
  font-size: 0.9rem;
  transition: all 0.3s ease;
  background: white;
}

.search-input:focus {
  outline: none;
  border-color: #667eea;
  box-shadow: 0 0 0 3px rgba(102, 126, 234, 0.1);
}

.add-admin-btn {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
  border: none;
  border-radius: 0.5rem;
  padding: 0.75rem 1.25rem;
  cursor: pointer;
  transition: all 0.3s ease;
  font-weight: 600;
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.add-admin-btn:hover {
  transform: translateY(-2px);
  box-shadow: 0 4px 12px rgba(102, 126, 234, 0.4);
}

/* Admins Table */
.admins-table-container {
  background: white;
  border-radius: 0.75rem;
  overflow: hidden;
  box-shadow: 0 0.25rem 1.25rem rgba(0, 0, 0, 0.08);
}

.table-wrapper {
  overflow-x: auto;
}

.admins-table {
  width: 100%;
  border-collapse: collapse;
}

.admins-table th {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
  padding: 1rem;
  text-align: left;
  font-weight: 600;
  font-size: 0.9rem;
}

.admins-table td {
  padding: 1rem;
  border-bottom: 1px solid #f1f3f4;
  vertical-align: middle;
}

.admin-row:hover {
  background: #f8f9fa;
}

.profile-cell {
  width: 4rem;
}

.profile-avatar {
  width: 3rem;
  height: 3rem;
  border-radius: 50%;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  display: flex;
  align-items: center;
  justify-content: center;
  color: white;
  font-size: 1.2rem;
  overflow: hidden;
}

.avatar-img {
  width: 100%;
  height: 100%;
  object-fit: cover;
}

.admin-name {
  font-weight: 600;
  color: #2c3e50;
}

.admin-email {
  color: #7f8c8d;
  font-size: 0.9rem;
}

.role-badge {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
  padding: 0.25rem 0.75rem;
  border-radius: 1rem;
  font-size: 0.8rem;
  font-weight: 600;
}

.status-badge {
  padding: 0.25rem 0.75rem;
  border-radius: 1rem;
  font-size: 0.8rem;
  font-weight: 600;
}

.status-badge.active {
  background: #e8f5e8;
  color: #27ae60;
}

.status-badge.inactive {
  background: #ffe6e6;
  color: #e74c3c;
}

.created-date {
  color: #7f8c8d;
  font-size: 0.9rem;
}

.action-buttons {
  display: flex;
  gap: 0.5rem;
}

.action-btn {
  width: 2rem;
  height: 2rem;
  border: none;
  border-radius: 0.375rem;
  cursor: pointer;
  transition: all 0.3s ease;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 0.8rem;
}

.edit-btn {
  background: #f39c12;
  color: white;
}

.delete-btn {
  background: #e74c3c;
  color: white;
}

.action-btn:hover:not(:disabled) {
  transform: translateY(-1px);
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.15);
}

.action-btn:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}

/* Loading State */
.loading-state {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  padding: 3rem;
  color: #7f8c8d;
}

.loading-spinner {
  width: 2rem;
  height: 2rem;
  border: 3px solid #f3f3f3;
  border-top: 3px solid #667eea;
  border-radius: 50%;
  animation: spin 1s linear infinite;
  margin-bottom: 1rem;
}

@keyframes spin {
  0% {
    transform: rotate(0deg);
  }
  100% {
    transform: rotate(360deg);
  }
}

/* Empty State */
.empty-state {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  padding: 3rem;
  text-align: center;
}

.empty-icon {
  font-size: 3rem;
  color: #bdc3c7;
  margin-bottom: 1rem;
}

.empty-state h3 {
  color: #2c3e50;
  margin-bottom: 0.5rem;
}

.empty-state p {
  color: #7f8c8d;
  margin-bottom: 1.5rem;
}

/* Modal */
.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: rgba(0, 0, 0, 0.6);
  backdrop-filter: blur(0.5rem);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 1000;
  padding: 1.25rem;
}

.modal-container {
  background: white;
  border-radius: 1rem;
  width: 100%;
  max-width: 35rem;
  max-height: 90vh;
  overflow-y: auto;
  box-shadow: 0 1.5625rem 5rem rgba(0, 0, 0, 0.3);
}

.modal-header {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
  padding: 1.5rem;
  border-radius: 1rem 1rem 0 0;
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.modal-title {
  margin: 0;
  font-size: 1.3rem;
  font-weight: 700;
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.modal-close {
  background: rgba(255, 255, 255, 0.2);
  border: none;
  color: white;
  width: 2rem;
  height: 2rem;
  border-radius: 50%;
  cursor: pointer;
  transition: all 0.3s ease;
  display: flex;
  align-items: center;
  justify-content: center;
}

.modal-close:hover {
  background: rgba(255, 255, 255, 0.3);
  transform: rotate(90deg);
}

.modal-body {
  padding: 1.5rem;
}

.admin-form {
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

/* Profile Upload */
.profile-upload {
  margin-bottom: 1rem;
}

.image-upload-container {
  display: flex;
  align-items: center;
  gap: 1rem;
}

.image-preview {
  width: 5rem;
  height: 5rem;
  border-radius: 50%;
  overflow: hidden;
  border: 2px solid #e9ecef;
  display: flex;
  align-items: center;
  justify-content: center;
  background: #f8f9fa;
}

.preview-img {
  width: 100%;
  height: 100%;
  object-fit: cover;
}

.preview-placeholder {
  display: flex;
  flex-direction: column;
  align-items: center;
  color: #7f8c8d;
  font-size: 0.8rem;
}

.preview-placeholder i {
  font-size: 1.5rem;
  margin-bottom: 0.25rem;
}

.upload-controls {
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
}

.file-input {
  display: none;
}

.upload-btn,
.remove-btn {
  padding: 0.5rem 1rem;
  border: none;
  border-radius: 0.375rem;
  cursor: pointer;
  transition: all 0.3s ease;
  font-weight: 600;
  display: flex;
  align-items: center;
  gap: 0.5rem;
  font-size: 0.85rem;
}

.upload-btn {
  background: #3498db;
  color: white;
}

.remove-btn {
  background: #e74c3c;
  color: white;
}

.upload-btn:hover,
.remove-btn:hover {
  transform: translateY(-1px);
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.15);
}

/* Modal Actions */
.modal-actions {
  display: flex;
  justify-content: flex-end;
  gap: 1rem;
  margin-top: 1.5rem;
  padding-top: 1rem;
  border-top: 1px solid #e9ecef;
}

.cancel-btn,
.save-btn {
  padding: 0.75rem 1.25rem;
  border: none;
  border-radius: 0.5rem;
  cursor: pointer;
  transition: all 0.3s ease;
  font-weight: 600;
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.cancel-btn {
  background: #95a5a6;
  color: white;
}

.save-btn {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
}

.cancel-btn:hover:not(:disabled),
.save-btn:hover:not(:disabled) {
  transform: translateY(-2px);
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.2);
}

.cancel-btn:disabled,
.save-btn:disabled {
  opacity: 0.6;
  cursor: not-allowed;
  transform: none;
}

/* Toast Messages */
.toast-container {
  position: fixed;
  top: 5rem;
  right: 1.5rem;
  z-index: 1001;
  display: flex;
  flex-direction: column;
  gap: 0.75rem;
  max-width: 20rem;
}

.toast-message {
  background: white;
  border-radius: 0.5rem;
  padding: 1rem;
  box-shadow: 0 0.5rem 1.5rem rgba(0, 0, 0, 0.15);
  border-left: 4px solid #2ecc71;
  display: flex;
  align-items: flex-start;
  gap: 0.75rem;
  animation: slideInRight 0.4s ease;
}

.toast-success {
  border-left-color: #2ecc71;
}

.toast-error {
  border-left-color: #e74c3c;
}

.toast-icon {
  width: 1.25rem;
  height: 1.25rem;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  color: white;
  font-size: 0.8rem;
  flex-shrink: 0;
}

.toast-success .toast-icon {
  background: #2ecc71;
}

.toast-error .toast-icon {
  background: #e74c3c;
}

.toast-content {
  flex: 1;
}

.toast-title {
  font-weight: 600;
  color: #2c3e50;
  margin-bottom: 0.25rem;
  font-size: 0.9rem;
}

.toast-text {
  color: #7f8c8d;
  font-size: 0.85rem;
  line-height: 1.4;
}

.toast-close {
  background: none;
  border: none;
  color: #bdc3c7;
  cursor: pointer;
  padding: 0.25rem;
  border-radius: 0.25rem;
  transition: all 0.2s ease;
  flex-shrink: 0;
}

.toast-close:hover {
  color: #7f8c8d;
  background: #f8f9fa;
}

@keyframes slideInRight {
  from {
    transform: translateX(100%);
    opacity: 0;
  }
  to {
    transform: translateX(0);
    opacity: 1;
  }
}

/* Responsive Design */
@media (max-width: 1024px) {
  .content-area-wrapper {
    padding: 1rem;
  }

  .settings-grid {
    grid-template-columns: 1fr;
  }

  .form-grid,
  .form-row {
    grid-template-columns: 1fr;
  }

  .form-group.full-width {
    grid-column: span 1;
  }
}

@media (max-width: 768px) {
  .admin-dashboard-desktop-layout {
    flex-direction: column;
  }

  .sidebar-nav-panel {
    width: 100%;
    height: auto;
  }

  .main-panel-content {
    width: 100%;
  }

  .content-area-wrapper {
    padding: 0.75rem;
    gap: 1rem;
  }

  .admin-controls {
    flex-direction: column;
    gap: 1rem;
    align-items: stretch;
  }

  .search-input {
    width: 100%;
  }

  .preference-item {
    flex-direction: column;
    align-items: flex-start;
    gap: 0.75rem;
  }

  .image-upload-container {
    flex-direction: column;
    align-items: center;
    text-align: center;
  }

  .modal-actions {
    flex-direction: column;
  }

  .toast-container {
    right: 0.75rem;
    left: 0.75rem;
    max-width: none;
  }
}

@media (max-width: 480px) {
  .settings-nav-tabs {
    flex-direction: column;
  }

  .settings-tab {
    width: 100%;
    justify-content: center;
  }

  .admins-table {
    font-size: 0.85rem;
  }

  .admins-table th,
  .admins-table td {
    padding: 0.75rem 0.5rem;
  }
}
</style>
