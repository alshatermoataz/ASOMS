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
            <router-link to="/admin/customers" class="nav-item active"
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
            <a href="#" class="nav-item"
              ><i class="fas fa-chart-line fa-fw"></i
              ><span v-if="!isSidebarCollapsed">Analytics</span></a
            >
          </li> -->
          <li>
            <router-link to="/admin/settings" class="nav-item"
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
              <i class="fas fa-users navbar-brand-icon"></i>
              <span class="navbar-brand-text">Customer Management</span>
            </div>
          </div>
          <div class="navbar-right-main">
            <!-- <button class="notification-btn">
              <i class="fas fa-bell"></i>
              <span class="notification-badge">{{ newCustomersCount }}</span>
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
        <!-- Customer Stats -->
        <section class="stats-dashboard-section">
          <div class="stats-grid-container">
            <div class="stat-card total-customers-stat">
              <div class="stat-card-inner">
                <div class="stat-icon-container">
                  <i class="fas fa-users"></i>
                </div>
                <div class="stat-content-container">
                  <div class="stat-number">{{ totalCustomers }}</div>
                  <div class="stat-label">Total Customers</div>
                  <div class="stat-trend positive">
                    <i class="fas fa-arrow-up"></i>
                    <!-- <span>+8% this month</span> -->
                  </div>
                </div>
              </div>
            </div>

            <div class="stat-card active-customers-stat">
              <div class="stat-card-inner">
                <div class="stat-icon-container">
                  <i class="fas fa-user-check"></i>
                </div>
                <div class="stat-content-container">
                  <div class="stat-number">{{ activeCustomers }}</div>
                  <div class="stat-label">Active Customers</div>
                  <div class="stat-trend positive">
                    <i class="fas fa-arrow-up"></i>
                    <!-- <span>+12% this week</span> -->
                  </div>
                </div>
              </div>
            </div>

            <div class="stat-card new-customers-stat">
              <div class="stat-card-inner">
                <div class="stat-icon-container">
                  <i class="fas fa-user-plus"></i>
                </div>
                <div class="stat-content-container">
                  <div class="stat-number">{{ newCustomersCount }}</div>
                  <div class="stat-label">New This Month</div>
                  <div class="stat-trend positive">
                    <i class="fas fa-arrow-up"></i>
                    <!-- <span>+25% vs last month</span> -->
                  </div>
                </div>
              </div>
            </div>

            <div class="stat-card avg-orders-stat">
              <div class="stat-card-inner">
                <div class="stat-icon-container">
                  <i class="fas fa-shopping-bag"></i>
                </div>
                <div class="stat-content-container">
                  <div class="stat-number">{{ avgOrdersPerCustomer }}</div>
                  <div class="stat-label">Avg Orders/Customer</div>
                  <div class="stat-trend positive">
                    <i class="fas fa-arrow-up"></i>
                    <!-- <span>+3% this quarter</span> -->
                  </div>
                </div>
              </div>
            </div>
          </div>
        </section>

        <!-- Advanced Filters & Actions -->
        <section class="advanced-controls-section">
          <div class="controls-grid-layout">
            <!-- Advanced Filters Panel -->
            <div class="filters-panel-advanced">
              <div class="panel-header-section">
                <h3 class="panel-title">
                  <i class="fas fa-filter"></i>
                  Customer Filters
                </h3>
                <button class="clear-filters-btn" @click="clearAllFilters">
                  <i class="fas fa-times"></i>
                  Clear All
                </button>
              </div>

              <div class="advanced-filters-grid">
                <div class="filter-group-container">
                  <label class="filter-label-text">Order Count Range</label>
                  <div class="amount-range-container">
                    <input
                      v-model="filters.minOrders"
                      type="number"
                      min="0"
                      placeholder="Min"
                      class="filter-select-input"
                      @change="loadCustomers(1)"
                    />
                    <span class="range-separator">-</span>
                    <input
                      v-model="filters.maxOrders"
                      type="number"
                      min="0"
                      placeholder="Max"
                      class="filter-select-input"
                      @change="loadCustomers(1)"
                    />
                  </div>
                </div>

                <div class="filter-group-container">
                  <label class="filter-label-text">Status</label>
                  <select
                    v-model="filters.status"
                    class="filter-select-input"
                    @change="loadCustomers(1)"
                  >
                    <option value="">All</option>
                    <option value="active">Active</option>
                    <option value="inactive">Inactive</option>
                  </select>
                </div>

                <div class="filter-group-container">
                  <label class="filter-label-text">Registration Date</label>
                  <select
                    v-model="filters.registered"
                    class="filter-select-input"
                    @change="loadCustomers(1)"
                  >
                    <option value="">All Time</option>
                    <option value="today">Today</option>
                    <option value="week">This Week</option>
                    <option value="month">This Month</option>
                    <option value="year">This Year</option>
                  </select>
                </div>

                <div class="filter-group-container search-group-advanced">
                  <label class="filter-label-text">Search Customers</label>
                  <div class="search-input-container">
                    <i class="fas fa-search search-input-icon"></i>
                    <input
                      v-model="filters.search"
                      type="text"
                      class="search-input-field"
                      placeholder="Name, email, phone..."
                      @input="debouncedSearch"
                    />
                  </div>
                </div>
              </div>
            </div>

            <!-- Customer Actions Panel -->
            <div class="customer-actions-panel">
              <div class="panel-header-section">
                <h3 class="panel-title">
                  <i class="fas fa-cogs"></i>
                  Customer Actions
                </h3>
                <span class="selected-count"
                  >{{ selectedCustomers.length }} selected</span
                >
              </div>

              <div class="customer-actions-grid">
                <button
                  class="customer-action-btn add-customer"
                  @click="openAddCustomerModal"
                >
                  <i class="fas fa-user-plus"></i>
                  <span>Add Customer</span>
                </button>

                <!-- <button
                  class="customer-action-btn export-customers"
                  @click="exportCustomers"
                  :disabled="selectedCustomers.length === 0"
                >
                  <i class="fas fa-download"></i>
                  <span>Export Selected</span>
                </button>

                <button
                  class="customer-action-btn send-email"
                  @click="sendBulkEmail"
                  :disabled="selectedCustomers.length === 0"
                >
                  <i class="fas fa-envelope"></i>
                  <span>Send Email</span>
                </button>

                <button
                  class="customer-action-btn manage-status"
                  @click="openBulkStatusModal"
                  :disabled="selectedCustomers.length === 0"
                >
                  <i class="fas fa-user-cog"></i>
                  <span>Manage Status</span>
                </button> -->
              </div>
            </div>
          </div>
        </section>

        <!-- Customers Table Section -->
        <section class="customers-table-section">
          <div class="section-header-container">
            <div class="header-left-section">
              <h2 class="section-main-title">
                <i class="fas fa-list-ul"></i>
                Customers List
              </h2>
              <div class="results-info-text">
                <span v-if="!isLoading">
                  Showing {{ paginationInfo.start }}-{{ paginationInfo.end }} of
                  {{ totalItems }} customers
                </span>
              </div>
            </div>

            <div class="header-right-section">
              <div class="view-options-container">
                <div class="view-toggle-options">
                  <button
                    :class="[
                      'view-toggle-btn',
                      { active: viewMode === 'table' },
                    ]"
                    @click="viewMode = 'table'"
                  >
                    <i class="fas fa-table"></i>
                    Table
                  </button>
                  <button
                    :class="[
                      'view-toggle-btn',
                      { active: viewMode === 'cards' },
                    ]"
                    @click="viewMode = 'cards'"
                  >
                    <i class="fas fa-th-large"></i>
                    Cards
                  </button>
                </div>

                <div class="sort-options-container">
                  <select
                    v-model="sortBy"
                    @change="loadCustomers(1)"
                    class="sort-dropdown-select"
                  >
                    <option value="name-asc">Name A-Z</option>
                    <option value="name-desc">Name Z-A</option>
                    <option value="date-desc">Newest First</option>
                    <option value="date-asc">Oldest First</option>
                    <option value="orders-desc">Most Orders</option>
                    <option value="orders-asc">Least Orders</option>
                  </select>
                </div>
              </div>
            </div>
          </div>

          <!-- Loading State -->
          <div v-if="isLoading" class="loading-state-container">
            <div class="loading-content-wrapper">
              <div class="loading-spinner-container">
                <div class="spinner-ring-animation"></div>
              </div>
              <h3 class="loading-title">Loading Customers...</h3>
              <p class="loading-description">Fetching customer data</p>
            </div>
          </div>

          <!-- Table View -->
          <div
            v-else-if="customers.length > 0 && viewMode === 'table'"
            class="table-wrapper-container"
          >
            <div class="table-scroll-container">
              <table class="customers-data-table">
                <thead class="table-header-section">
                  <tr>
                    <th class="checkbox-column-header">
                      <input
                        type="checkbox"
                        v-model="selectAll"
                        @change="toggleSelectAll"
                        class="header-checkbox"
                      />
                    </th>
                    <th
                      class="sortable-column-header"
                      @click="sortColumn('name')"
                    >
                      Customer
                      <i class="fas fa-sort sort-indicator"></i>
                    </th>
                    <th
                      class="sortable-column-header"
                      @click="sortColumn('email')"
                    >
                      Contact Info
                      <i class="fas fa-sort sort-indicator"></i>
                    </th>
                    <th class="standard-column-header">Role</th>
                    <th class="standard-column-header">Status</th>
                    <th
                      class="sortable-column-header"
                      @click="sortColumn('orders')"
                    >
                      Orders
                      <i class="fas fa-sort sort-indicator"></i>
                    </th>
                    <th
                      class="sortable-column-header"
                      @click="sortColumn('lastLogin')"
                    >
                      Last Login
                      <i class="fas fa-sort sort-indicator"></i>
                    </th>
                    <th
                      class="sortable-column-header"
                      @click="sortColumn('registered')"
                    >
                      Registered
                      <i class="fas fa-sort sort-indicator"></i>
                    </th>
                    <th class="actions-column-header">Actions</th>
                  </tr>
                </thead>
                <tbody class="table-body-section">
                  <tr
                    v-for="customer in customers"
                    :key="customer.id"
                    class="table-data-row"
                  >
                    <td class="checkbox-column-cell">
                      <input
                        type="checkbox"
                        v-model="selectedCustomers"
                        :value="customer.id"
                        class="row-checkbox"
                      />
                    </td>
                    <td class="customer-column-cell">
                      <div class="customer-info-display">
                        <div class="customer-avatar-container">
                          <img
                            v-if="customer.profilePictureUrl"
                            :src="imageBase(customer.profilePictureUrl)"
                            :alt="customer.fullName"
                            class="customer-avatar-img"
                            @error="handleImageError"
                          />
                          <div v-else class="customer-avatar-placeholder">
                            {{ getInitials(customer.fullName) }}
                          </div>
                        </div>
                        <div class="customer-details-container">
                          <div class="customer-name-text">
                            {{ customer.fullName }}
                          </div>
                          <div class="customer-id-text">
                            ID: {{ customer.id }}
                          </div>
                        </div>
                      </div>
                    </td>
                    <td class="contact-column-cell">
                      <div class="contact-info-container">
                        <div class="contact-email">
                          <i class="fas fa-envelope"></i>
                          {{ customer.email }}
                          <i
                            v-if="customer.isEmailVerified"
                            class="fas fa-check-circle verified-icon"
                            title="Email Verified"
                          ></i>
                        </div>
                        <div
                          class="contact-phone"
                          v-if="customer.contactNumber"
                        >
                          <i class="fas fa-phone"></i>
                          {{ customer.contactNumber }}
                        </div>
                      </div>
                    </td>
                    <td class="role-column-cell">
                      <span class="role-badge">
                        {{ customer.role }}
                      </span>
                    </td>
                    <td class="status-column-cell">
                      <div class="status-indicators">
                        <span
                          :class="`customer-status-badge status-${customer.status.toLowerCase()}`"
                        >
                          <i :class="getStatusIcon(customer.status)"></i>
                          {{ customer.status }}
                        </span>
                        <div class="security-indicators">
                          <i
                            v-if="customer.enableMFA"
                            class="fas fa-shield-alt mfa-icon"
                            title="MFA Enabled"
                          ></i>
                          <i
                            v-if="customer.isCreatedByRecruiter"
                            class="fas fa-user-tie recruiter-icon"
                            title="Created by Recruiter"
                          ></i>
                        </div>
                      </div>
                    </td>
                    <td class="orders-column-cell">
                      <div class="orders-summary">
                        <span class="orders-count">{{
                          customer.totalOrders
                        }}</span>
                        <span class="orders-label">orders</span>
                      </div>
                    </td>
                    <td class="last-login-column-cell">
                      <div class="last-login-info">
                        <div v-if="customer.lastLogin" class="last-login-date">
                          {{ formatDate(customer.lastLogin) }}
                        </div>
                        <div v-else class="no-login-text">Never</div>
                      </div>
                    </td>
                    <td class="registered-column-cell">
                      <div class="date-display-container">
                        <div class="date-primary-text">
                          {{ formatDate(customer.createdAt) }}
                        </div>
                        <div class="date-secondary-text">
                          {{ formatTime(customer.createdAt) }}
                        </div>
                      </div>
                    </td>
                    <td class="actions-column-cell">
                      <div class="action-buttons-container">
                        <button
                          class="action-button-small view-action"
                          @click="viewCustomerDetails(customer)"
                          title="View Details"
                        >
                          <i class="fas fa-eye"></i>
                        </button>

                        <button
                          class="action-button-small edit-action"
                          @click="editCustomer(customer)"
                          title="Edit Customer"
                        >
                          <i class="fas fa-edit"></i>
                        </button>

                        <div class="dropdown-menu-container">
                          <button
                            class="action-button-small more-actions"
                            title="More Actions"
                          >
                            <i class="fas fa-ellipsis-v"></i>
                          </button>
                          <div class="dropdown-menu-list">
                            <a
                              href="#"
                              class="dropdown-menu-item"
                              @click.prevent="viewCustomerDetails(customer)"
                            >
                              <i class="fas fa-history"></i>
                              Order History
                            </a>
                            <!-- <a
                              href="#"
                              class="dropdown-menu-item"
                              @click.prevent="sendEmail(customer)"
                            >
                              <i class="fas fa-envelope"></i>
                              Send Email
                            </a> -->
                            <!-- <a
                              href="#"
                              class="dropdown-menu-item"
                              @click.prevent="resetPassword(customer)"
                            >
                              <i class="fas fa-key"></i>
                              Reset Password
                            </a> -->
                            <div class="dropdown-divider"></div>
                            <a
                              href="#"
                              class="dropdown-menu-item danger-item"
                              @click.prevent="deleteCustomer(customer)"
                            >
                              <i class="fas fa-trash"></i>
                              Delete Customer
                            </a>
                          </div>
                        </div>
                      </div>
                    </td>
                  </tr>
                </tbody>
              </table>
            </div>
          </div>

          <!-- Cards View -->
          <div
            v-else-if="customers.length > 0 && viewMode === 'cards'"
            class="cards-view-container"
          >
            <div class="customers-cards-grid">
              <div
                v-for="customer in customers"
                :key="customer.id"
                class="customer-card-item"
              >
                <div class="customer-card-header">
                  <div class="customer-card-avatar">
                    <input
                      type="checkbox"
                      v-model="selectedCustomers"
                      :value="customer.id"
                      class="card-checkbox"
                    />
                    <div class="avatar-container">
                      <img
                        v-if="customer.profilePictureUrl"
                        :src="imageBase(customer.profilePictureUrl)"
                        :alt="customer.fullName"
                        class="avatar-img"
                        @error="handleImageError"
                      />
                      <div v-else class="avatar-placeholder">
                        {{ getInitials(customer.fullName) }}
                      </div>
                    </div>
                  </div>
                  <div class="card-status-badges">
                    <span class="role-badge">{{ customer.role }}</span>
                    <span
                      :class="`customer-status-badge status-${customer.status.toLowerCase()}`"
                    >
                      {{ customer.status }}
                    </span>
                  </div>
                </div>

                <div class="customer-card-body">
                  <div class="customer-name-card">{{ customer.fullName }}</div>
                  <div class="customer-contact-card">
                    <div class="contact-item">
                      <i class="fas fa-envelope"></i>
                      {{ customer.email }}
                      <i
                        v-if="customer.isEmailVerified"
                        class="fas fa-check-circle verified-icon"
                        title="Verified"
                      ></i>
                    </div>
                    <div class="contact-item" v-if="customer.contactNumber">
                      <i class="fas fa-phone"></i>
                      {{ customer.contactNumber }}
                    </div>
                  </div>

                  <div class="customer-stats-card">
                    <div class="stat-item">
                      <span class="stat-label">Orders:</span>
                      <span class="stat-value">{{ customer.totalOrders }}</span>
                    </div>
                    <div class="stat-item">
                      <span class="stat-label">Last Login:</span>
                      <span class="stat-value">{{
                        customer.lastLogin
                          ? formatDate(customer.lastLogin)
                          : "Never"
                      }}</span>
                    </div>
                    <div class="stat-item">
                      <span class="stat-label">Security:</span>
                      <div class="security-indicators-card">
                        <i
                          v-if="customer.enableMFA"
                          class="fas fa-shield-alt mfa-icon"
                          title="MFA Enabled"
                        ></i>
                        <i
                          v-if="customer.isCreatedByRecruiter"
                          class="fas fa-user-tie recruiter-icon"
                          title="Created by Recruiter"
                        ></i>
                        <span
                          v-if="
                            !customer.enableMFA &&
                            !customer.isCreatedByRecruiter
                          "
                          class="no-security"
                          >Basic</span
                        >
                      </div>
                    </div>
                  </div>
                </div>

                <div class="customer-card-footer">
                  <div class="registration-date-card">
                    Joined {{ formatDate(customer.createdAt) }}
                  </div>
                  <div class="card-actions">
                    <button
                      class="card-action-btn view-btn"
                      @click="viewCustomerDetails(customer)"
                    >
                      <i class="fas fa-eye"></i>
                    </button>
                    <button
                      class="card-action-btn edit-btn"
                      @click="editCustomer(customer)"
                    >
                      <i class="fas fa-edit"></i>
                    </button>
                    <button class="card-action-btn more-btn">
                      <i class="fas fa-ellipsis-v"></i>
                    </button>
                  </div>
                </div>
              </div>
            </div>
          </div>

          <!-- Empty State -->
          <div v-else class="empty-state-container">
            <div class="empty-state-content">
              <div class="empty-state-icon">
                <i class="fas fa-users"></i>
              </div>
              <h3 class="empty-state-title">No Customers Found</h3>
              <p class="empty-state-description">
                No customers match your current filters. Try adjusting your
                search criteria or add a new customer.
              </p>
              <button
                class="empty-state-action-btn"
                @click="openAddCustomerModal"
              >
                <i class="fas fa-user-plus"></i>
                Add New Customer
              </button>
            </div>
          </div>

          <!-- Pagination -->
          <div v-if="totalPages > 1" class="pagination-section-container">
            <div class="pagination-info-section">
              <div class="page-size-selector-container">
                <label class="page-size-label">Show:</label>
                <select
                  v-model="pageSize"
                  @change="loadCustomers(1)"
                  class="page-size-select-dropdown"
                >
                  <option value="10">10</option>
                  <option value="25">25</option>
                  <option value="50">50</option>
                  <option value="100">100</option>
                </select>
                <span class="page-size-suffix">per page</span>
              </div>
            </div>

            <div class="pagination-controls-container">
              <button
                class="pagination-control-btn"
                @click="loadCustomers(1)"
                :disabled="currentPage <= 1"
              >
                <i class="fas fa-angle-double-left"></i>
              </button>
              <button
                class="pagination-control-btn"
                @click="loadCustomers(currentPage - 1)"
                :disabled="currentPage <= 1"
              >
                <i class="fas fa-angle-left"></i>
                Previous
              </button>

              <div class="page-numbers-container">
                <button
                  v-for="page in displayedPages"
                  :key="page"
                  :class="[
                    'page-number-btn',
                    { 'active-page': page === currentPage },
                  ]"
                  @click="loadCustomers(page)"
                >
                  {{ page }}
                </button>
              </div>

              <button
                class="pagination-control-btn"
                @click="loadCustomers(currentPage + 1)"
                :disabled="currentPage >= totalPages"
              >
                Next
                <i class="fas fa-angle-right"></i>
              </button>
              <button
                class="pagination-control-btn"
                @click="loadCustomers(totalPages)"
                :disabled="currentPage >= totalPages"
              >
                <i class="fas fa-angle-double-right"></i>
              </button>
            </div>
          </div>
        </section>
      </main>
    </div>

    <!-- Customer Details Modal -->
    <div
      v-if="showDetailsModal"
      class="modal-overlay-backdrop"
      @click.self="closeDetailsModal"
    >
      <div class="modal-container-wrapper customer-details-modal">
        <div class="modal-header-section">
          <h3 class="modal-title-text">
            <i class="fas fa-user"></i>
            Customer Details -
            {{ selectedCustomer ? selectedCustomer.fullName : "" }}
          </h3>
          <button class="modal-close-button" @click="closeDetailsModal">
            <i class="fas fa-times"></i>
          </button>
        </div>
        <div class="modal-body-section">
          <div v-if="selectedCustomer" class="customer-details-container">
            <!-- Customer Profile Section -->
            <div class="customer-profile-section">
              <div class="profile-avatar-section">
                <div class="profile-avatar-container">
                  <img
                    v-if="selectedCustomer.profilePictureUrl"
                    :src="imageBase(selectedCustomer.profilePictureUrl)"
                    :alt="selectedCustomer.fullName"
                    class="profile-avatar-img"
                    @error="handleImageError"
                  />
                  <div v-else class="profile-avatar-placeholder">
                    {{ getInitials(selectedCustomer.fullName) }}
                  </div>
                </div>
                <div class="profile-info">
                  <h4 class="profile-name">{{ selectedCustomer.fullName }}</h4>
                  <div class="profile-badges">
                    <span class="role-badge">{{ selectedCustomer.role }}</span>
                    <span
                      :class="`customer-status-badge status-${selectedCustomer.status.toLowerCase()}`"
                    >
                      {{ selectedCustomer.status }}
                    </span>
                  </div>
                  <div class="profile-meta">
                    <span class="customer-id"
                      >ID: {{ selectedCustomer.id }}</span
                    >
                    <span class="registration-date"
                      >Joined
                      {{ formatDate(selectedCustomer.createdAt, true) }}</span
                    >
                  </div>
                </div>
              </div>
            </div>

            <!-- Customer Information Grid -->
            <div class="details-grid-container">
              <!-- Personal Information -->
              <div class="details-section-container">
                <h6 class="details-section-title">Personal Information</h6>
                <div class="details-table-container">
                  <div class="detail-row-container">
                    <span class="detail-label-text">Full Name:</span>
                    <span class="detail-value-text">{{
                      selectedCustomer.fullName
                    }}</span>
                  </div>
                  <div class="detail-row-container">
                    <span class="detail-label-text">Gender:</span>
                    <span class="detail-value-text">{{
                      selectedCustomer.gender || "Not specified"
                    }}</span>
                  </div>
                  <div class="detail-row-container">
                    <span class="detail-label-text">Date of Birth:</span>
                    <span class="detail-value-text">{{
                      selectedCustomer.dateOfBirth
                        ? formatDate(selectedCustomer.dateOfBirth)
                        : "Not provided"
                    }}</span>
                  </div>
                  <div class="detail-row-container">
                    <span class="detail-label-text">Age:</span>
                    <span class="detail-value-text">{{
                      selectedCustomer.dateOfBirth
                        ? calculateAge(selectedCustomer.dateOfBirth)
                        : "N/A"
                    }}</span>
                  </div>
                </div>
              </div>

              <!-- Contact Information -->
              <div class="details-section-container">
                <h6 class="details-section-title">Contact Information</h6>
                <div class="details-table-container">
                  <div class="detail-row-container">
                    <span class="detail-label-text">Email:</span>
                    <div class="detail-value-text">
                      {{ selectedCustomer.email }}
                      <i
                        v-if="selectedCustomer.isEmailVerified"
                        class="fas fa-check-circle verified-icon"
                        title="Email Verified"
                      ></i>
                    </div>
                  </div>
                  <div class="detail-row-container">
                    <span class="detail-label-text">Phone:</span>
                    <span class="detail-value-text">{{
                      selectedCustomer.contactNumber || "Not provided"
                    }}</span>
                  </div>
                  <div class="detail-row-container">
                    <span class="detail-label-text">Address:</span>
                    <span class="detail-value-text">{{
                      selectedCustomer.currentAddressLine1 || "Not provided"
                    }}</span>
                  </div>
                  <div class="detail-row-container">
                    <span class="detail-label-text">Postcode:</span>
                    <span class="detail-value-text">{{
                      selectedCustomer.postcode || "Not provided"
                    }}</span>
                  </div>
                </div>
              </div>

              <!-- Account Information -->
              <div class="details-section-container">
                <h6 class="details-section-title">Account Information</h6>
                <div class="details-table-container">
                  <div class="detail-row-container">
                    <span class="detail-label-text">Role:</span>
                    <span class="detail-value-text">{{
                      selectedCustomer.role
                    }}</span>
                  </div>
                  <div class="detail-row-container">
                    <span class="detail-label-text">Created At:</span>
                    <span class="detail-value-text">{{
                      formatDate(selectedCustomer.createdAt, true)
                    }}</span>
                  </div>
                  <div class="detail-row-container">
                    <span class="detail-label-text">Last Login:</span>
                    <span class="detail-value-text">{{
                      selectedCustomer.lastLogin
                        ? formatDate(selectedCustomer.lastLogin, true)
                        : "Never"
                    }}</span>
                  </div>
                  <!-- <div class="detail-row-container">
                    <span class="detail-label-text">Created by Recruiter:</span>
                    <span class="detail-value-text">
                      <i
                        :class="
                          selectedCustomer.isCreatedByRecruiter
                            ? 'fas fa-check text-green-500'
                            : 'fas fa-times text-red-500'
                        "
                      ></i>
                      {{ selectedCustomer.isCreatedByRecruiter ? "Yes" : "No" }}
                    </span>
                  </div> -->
                </div>
              </div>

              <!-- Security Settings -->
              <!-- <div class="details-section-container">
                <h6 class="details-section-title">Security Settings</h6>
                <div class="details-table-container">
                  <div class="detail-row-container">
                    <span class="detail-label-text">Email Verified:</span>
                    <span class="detail-value-text">
                      <i
                        :class="
                          selectedCustomer.isEmailVerified
                            ? 'fas fa-check text-green-500'
                            : 'fas fa-times text-red-500'
                        "
                      ></i>
                      {{
                        selectedCustomer.isEmailVerified
                          ? "Verified"
                          : "Not Verified"
                      }}
                    </span>
                  </div>
                  <div class="detail-row-container">
                    <span class="detail-label-text">MFA Enabled:</span>
                    <span class="detail-value-text">
                      <i
                        :class="
                          selectedCustomer.enableMFA
                            ? 'fas fa-shield-alt text-green-500'
                            : 'fas fa-shield text-gray-500'
                        "
                      ></i>
                      {{ selectedCustomer.enableMFA ? "Enabled" : "Disabled" }}
                    </span>
                  </div>
                  <div class="detail-row-container">
                    <span class="detail-label-text">Country Location ID:</span>
                    <span class="detail-value-text">{{
                      selectedCustomer.countryLocationId || "Not set"
                    }}</span>
                  </div>
                  <div class="detail-row-container">
                    <span class="detail-label-text"
                      >Nationality Country ID:</span
                    >
                    <span class="detail-value-text">{{
                      selectedCustomer.nationalityCountryId || "Not set"
                    }}</span>
                  </div>
                </div>
              </div> -->
            </div>

            <!-- Recent Orders Section -->
            <div class="recent-orders-section">
              <h6 class="details-section-title">Recent Orders</h6>
              <div
                v-if="
                  selectedCustomer.recentOrders &&
                  selectedCustomer.recentOrders.length > 0
                "
                class="recent-orders-list"
              >
                <div
                  v-for="order in selectedCustomer.recentOrders"
                  :key="order.id"
                  class="recent-order-item"
                >
                  <div class="order-info">
                    <div class="order-id">#{{ order.id }}</div>
                    <div class="order-date">
                      {{ formatDate(order.createdAt) }}
                    </div>
                  </div>
                  <div class="order-amount">
                    ${{ formatCurrency(order.totalAmount) }}
                  </div>
                  <span
                    :class="`order-status-badge status-${order.status
                      .toLowerCase()
                      .replace(/\s+/g, '')}`"
                  >
                    {{ order.status }}
                  </span>
                </div>
              </div>
              <div v-else class="no-orders-message">
                <i class="fas fa-shopping-cart"></i>
                <span>No orders found</span>
              </div>
            </div>

            <!-- Customer Actions -->
            <div class="modal-actions-container">
              <button
                class="modal-action-button edit-customer-action"
                @click="
                  editCustomer(selectedCustomer);
                  closeDetailsModal();
                "
              >
                <i class="fas fa-edit"></i>
                Edit Customer
              </button>
              <!-- <button
                class="modal-action-button view-orders-action"
                @click="viewOrderHistory(selectedCustomer)"
              >
                <i class="fas fa-history"></i>
                View All Orders
              </button> -->
              <!-- <button
                class="modal-action-button email-action"
                @click="sendEmail(selectedCustomer)"
              >
                <i class="fas fa-envelope"></i>
                Send Email
              </button> -->
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- Add/Edit Customer Modal -->
    <div
      v-if="showAddEditModal"
      class="modal-overlay-backdrop"
      @click.self="closeAddEditModal"
    >
      <div class="modal-container-wrapper add-edit-modal">
        <div class="modal-header-section">
          <h3 class="modal-title-text">
            <i class="fas fa-user-plus"></i>
            {{ editingCustomer ? "Edit Customer" : "Add New Customer" }}
          </h3>
          <button class="modal-close-button" @click="closeAddEditModal">
            <i class="fas fa-times"></i>
          </button>
        </div>
        <div class="modal-body-section">
          <form @submit.prevent="saveCustomer" class="customer-form">
            <!-- Personal Information Section -->
            <div class="form-section">
              <h4 class="form-section-title">
                <i class="fas fa-user"></i>
                Personal Information
              </h4>
              <div class="form-grid">
                <div class="form-group">
                  <label class="form-label">Full Name *</label>
                  <input
                    v-model="customerForm.fullName"
                    type="text"
                    class="form-input"
                    required
                  />
                </div>

                <div class="form-group">
                  <label class="form-label">Gender</label>
                  <select v-model="customerForm.gender" class="form-select">
                    <option value="">Select Gender</option>
                    <option value="Male">Male</option>
                    <option value="Female">Female</option>
                    <option value="Other">Other</option>
                    <option value="Prefer not to say">Prefer not to say</option>
                  </select>
                </div>

                <div class="form-group">
                  <label class="form-label">Date of Birth</label>
                  <input
                    v-model="customerForm.dateOfBirth"
                    type="date"
                    class="form-input"
                  />
                </div>

                <div class="form-group">
                  <label class="form-label">Profile Picture</label>
                  <input
                    type="file"
                    @change="handleImageUpload"
                    accept="image/*"
                    class="form-input"
                  />
                </div>

                <div v-if="customerForm.profilePictureUrl" class="form-group">
                  <label class="form-label">Current Profile Picture</label>
                  <div class="current-image-preview">
                    <img
                      :src="imageBase(customerForm.profilePictureUrl)"
                      alt="Current profile picture"
                      style="
                        max-height: 100px;
                        max-width: 100px;
                        border-radius: 50%;
                      "
                    />
                  </div>
                </div>
              </div>
            </div>

            <!-- Contact Information Section -->
            <div class="form-section">
              <h4 class="form-section-title">
                <i class="fas fa-address-book"></i>
                Contact Information
              </h4>
              <div class="form-grid">
                <div class="form-group">
                  <label class="form-label">Email Address *</label>
                  <input
                    v-model="customerForm.email"
                    type="email"
                    class="form-input"
                    required
                  />
                </div>

                <div class="form-group" v-if="!editingCustomer">
                  <label class="form-label">Password *</label>
                  <input
                    v-model="customerForm.password"
                    type="password"
                    class="form-input"
                    :required="!editingCustomer"
                    placeholder="Enter password"
                  />
                </div>

                <div class="form-group full-width">
                  <label class="form-label">Current Address</label>
                  <input
                    v-model="customerForm.currentAddressLine1"
                    type="text"
                    class="form-input"
                    placeholder="Street address, apartment, suite, etc."
                  />
                </div>

                <div class="form-group">
                  <label class="form-label">Contact Number</label>
                  <input
                    v-model="customerForm.contactNumber"
                    type="tel"
                    class="form-input"
                  />
                </div>

                <div class="form-group">
                  <label class="form-label">Postcode</label>
                  <input
                    v-model="customerForm.postcode"
                    type="text"
                    class="form-input"
                  />
                </div>

                <!-- <div class="form-group">
                  <label class="form-label">Country Location ID</label>
                  <input
                    v-model="customerForm.countryLocationId"
                    type="number"
                    class="form-input"
                    placeholder="Country location identifier"
                  />
                </div> -->

                <!-- <div class="form-group">
                  <label class="form-label">Nationality Country ID</label>
                  <input
                    v-model="customerForm.nationalityCountryId"
                    type="number"
                    class="form-input"
                    placeholder="Nationality country identifier"
                  />
                </div> -->
              </div>
            </div>

            <!-- Account Settings Section -->
            <!-- <div class="form-section">
              <h4 class="form-section-title">
                <i class="fas fa-cog"></i>
                Account Settings
              </h4>
              <div class="form-grid">
                <div class="form-group">
                  <label class="form-label">Role</label>
                  <select v-model="customerForm.role" class="form-select">
                    <option value="Customer">Customer</option>
                    <option value="Admin">Admin</option>
                    <option value="Manager">Manager</option>
                  </select>
                </div>

                <div class="form-group" v-if="!editingCustomer">
                  <label class="form-label">Password *</label>
                  <input
                    v-model="customerForm.password"
                    type="password"
                    class="form-input"
                    :required="!editingCustomer"
                    placeholder="Enter password"
                  />
                </div>

                <div class="form-group checkbox-group">
                  <label class="checkbox-label">
                    <input
                      v-model="customerForm.enableMFA"
                      type="checkbox"
                      class="form-checkbox"
                    />
                    <span class="checkbox-text"
                      >Enable Multi-Factor Authentication</span
                    >
                  </label>
                </div>

                <div class="form-group checkbox-group">
                  <label class="checkbox-label">
                    <input
                      v-model="customerForm.isEmailVerified"
                      type="checkbox"
                      class="form-checkbox"
                    />
                    <span class="checkbox-text">Email Verified</span>
                  </label>
                </div>

                <div class="form-group checkbox-group">
                  <label class="checkbox-label">
                    <input
                      v-model="customerForm.isCreatedByRecruiter"
                      type="checkbox"
                      class="form-checkbox"
                    />
                    <span class="checkbox-text">Created by Recruiter</span>
                  </label>
                </div>
              </div>
            </div> -->

            <div class="form-actions">
              <button
                type="button"
                class="form-btn cancel-btn"
                @click="closeAddEditModal"
              >
                <i class="fas fa-times"></i>
                Cancel
              </button>
              <button
                type="submit"
                class="form-btn save-btn"
                :disabled="isSaving"
              >
                <i class="fas fa-spinner fa-spin" v-if="isSaving"></i>
                <i class="fas fa-save" v-else></i>
                {{ isSaving ? "Saving..." : "Save Customer" }}
              </button>
            </div>
          </form>
        </div>
      </div>
    </div>

    <!-- Toast Messages -->
    <div class="toast-messages-container">
      <div
        v-for="(message, index) in messages"
        :key="index"
        :class="['toast-message-item', `toast-${message.type}`]"
      >
        <div class="toast-icon-container">
          <i
            :class="`fas ${
              message.type === 'success'
                ? 'fa-check-circle'
                : 'fa-exclamation-triangle'
            }`"
          ></i>
        </div>
        <div class="toast-content-container">
          <div class="toast-title-text">
            {{ message.type === "success" ? "Success" : "Error" }}
          </div>
          <div class="toast-message-text">{{ message.text }}</div>
        </div>
        <button class="toast-close-button" @click="removeMessage(index)">
          <i class="fas fa-times"></i>
        </button>
      </div>
    </div>
  </div>
</template>

<script>
import { ref, computed, onMounted } from "vue";
import axios from "axios";
import { useProductStore } from "../stores/useProductStore";

export default {
  name: "CustomersPage",
  setup() {
    const customers = ref([]);
    const selectedCustomer = ref(null);
    const editingCustomer = ref(null);
    const isLoading = ref(false);
    const isSaving = ref(false);
    const currentPage = ref(1);
    const totalPages = ref(1);
    const totalItems = ref(0);
    const pageSize = ref(25);
    const viewMode = ref("table");
    const sortBy = ref("name-asc");
    const showDetailsModal = ref(false);
    const showAddEditModal = ref(false);
    const messages = ref([]);
    const selectedCustomers = ref([]);
    const selectAll = ref(false);
    const isSidebarCollapsed = ref(false);

    // Stats
    const totalCustomers = ref(0);
    const activeCustomers = ref(0);
    const newCustomersCount = ref(0);
    const avgOrdersPerCustomer = ref("0");

    const API_BASE = "https://localhost:7004";

    const filters = ref({
      role: "",
      status: "",
      registered: "",
      search: "",
      minOrders: "",
      maxOrders: "",
    });

    const customerForm = ref({
      fullName: "",
      email: "",
      password: "",
      role: "Customer",
      gender: "",
      dateOfBirth: "",
      image: null,
      profilePictureUrl: null,
      contactNumber: "",
      currentAddressLine1: "",
      postcode: "",
      countryLocationId: null,
      nationalityCountryId: null,
      enableMFA: false,
      isCreatedByRecruiter: false,
      isEmailVerified: false,
    });

    const paginationInfo = computed(() => {
      const start =
        totalItems.value > 0 ? (currentPage.value - 1) * pageSize.value + 1 : 0;
      const end = Math.min(
        currentPage.value * pageSize.value,
        totalItems.value
      );
      return { start, end };
    });

    const displayedPages = computed(() => {
      const pages = [];
      const maxPagesToShow = 5;
      let startPage = Math.max(
        1,
        currentPage.value - Math.floor(maxPagesToShow / 2)
      );
      let endPage = Math.min(totalPages.value, startPage + maxPagesToShow - 1);

      if (endPage - startPage + 1 < maxPagesToShow) {
        startPage = Math.max(1, endPage - maxPagesToShow + 1);
      }

      for (let i = startPage; i <= endPage; i++) {
        pages.push(i);
      }

      return pages;
    });

    const toggleSidebar = () => {
      isSidebarCollapsed.value = !isSidebarCollapsed.value;
    };

    // Image base helper function
    const imageBase = (url) => {
      if (!url) return null;
      return url.startsWith("http") ? url : `${API_BASE}${url}`;
    };

    // Handle image error fallback
    const handleImageError = (event) => {
      event.target.style.display = "none";
      event.target.nextElementSibling?.classList.remove("d-none");
    };

    const handleImageUpload = (event) => {
      const file = event.target.files[0];
      if (file) {
        customerForm.value.image = file;
      }
    };

    // Calculate age from date of birth
    const calculateAge = (dateOfBirth) => {
      if (!dateOfBirth) return "N/A";
      const today = new Date();
      const birthDate = new Date(dateOfBirth);
      let age = today.getFullYear() - birthDate.getFullYear();
      const monthDiff = today.getMonth() - birthDate.getMonth();

      if (
        monthDiff < 0 ||
        (monthDiff === 0 && today.getDate() < birthDate.getDate())
      ) {
        age--;
      }

      return age;
    };

    // Determine if a customer is active based on their last login date
    const isCustomerActive = (lastLogin) => {
      if (!lastLogin) return false;

      const loginDate = new Date(lastLogin);
      const thresholdDate = new Date();
      thresholdDate.setDate(thresholdDate.getDate() - 30);

      return loginDate >= thresholdDate;
    };

    const loadCustomers = async (page = 1) => {
      isLoading.value = true;
      currentPage.value = page;

      try {
        // Map filter values to API parameters
        const params = {
          page: currentPage.value,
          pageSize: pageSize.value,
          search: filters.value.search || undefined,
          status: filters.value.status || undefined,
          registered: filters.value.registered || undefined,
          minOrders: filters.value.minOrders || undefined,
          maxOrders: filters.value.maxOrders || undefined,
        };

        // Remove undefined values
        Object.keys(params).forEach((key) => {
          if (params[key] === undefined) {
            delete params[key];
          }
        });

        const response = await axios.get(`${API_BASE}/api/users`, { params });

        // Fetch recent orders for each user in parallel
        const usersWithOrders = await Promise.all(
          response.data.data.map(async (user) => {
            const status = isCustomerActive(user.lastOrderDate)
              ? "Active"
              : "Inactive";

            let recentOrders = [];
            try {
              const recentOrdersResponse = await axios.get(
                `https://asoms-production.up.railway.app/api/orders/user/${user.id}`
              );
              recentOrders = Array.isArray(recentOrdersResponse.data)
                ? recentOrdersResponse.data
                : [];
            } catch (orderErr) {
              console.error(
                `Error fetching orders for user ${user.id}:`,
                orderErr
              );
            }

            return {
              id: user.id,
              fullName: user.fullName,
              email: user.email,
              role: user.role,
              gender: user.gender,
              dateOfBirth: user.dateOfBirth,
              profilePictureUrl: user.profilePictureUrl,
              contactNumber: user.contactNumber,
              currentAddressLine1: user.currentAddressLine1,
              postcode: user.postcode,
              countryLocationId: user.countryLocationId,
              nationalityCountryId: user.nationalityCountryId,
              enableMFA: user.enableMFA,
              isCreatedByRecruiter: user.isCreatedByRecruiter,
              isEmailVerified: user.isEmailVerified,
              createdAt: user.createdAt,
              lastLogin: user.lastLogin,
              status: status,
              totalOrders: user.orderCount || 0,
              recentOrders: recentOrders,
            };
          })
        );

        customers.value = usersWithOrders;
        totalItems.value = response.data.totalUsers;
        totalPages.value = response.data.totalPages;
        totalCustomers.value = response.data.totalUsers;

        // Calculate stats from current page data
        activeCustomers.value = customers.value.filter(
          (c) => c.status === "Active"
        ).length;

        newCustomersCount.value = customers.value.filter((c) => {
          const registeredDate = new Date(c.createdAt);
          const monthAgo = new Date();
          monthAgo.setMonth(monthAgo.getMonth() - 1);
          return registeredDate >= monthAgo;
        }).length;

        const totalOrdersSum = customers.value.reduce(
          (sum, c) => sum + c.totalOrders,
          0
        );

        avgOrdersPerCustomer.value =
          customers.value.length > 0
            ? (totalOrdersSum / customers.value.length).toFixed(1)
            : "0";
      } catch (error) {
        console.error("Error loading customers:", error);
        showMessage("Error loading customers. Please try again.", "error");

        // Reset data on error
        customers.value = [];
        totalItems.value = 0;
        totalPages.value = 1;
        totalCustomers.value = 0;
        activeCustomers.value = 0;
        newCustomersCount.value = 0;
        avgOrdersPerCustomer.value = "0";
      } finally {
        isLoading.value = false;
      }
    };

    const toggleSelectAll = () => {
      if (selectAll.value) {
        selectedCustomers.value = customers.value.map(
          (customer) => customer.id
        );
      } else {
        selectedCustomers.value = [];
      }
    };

    const viewCustomerDetails = (customer) => {
      selectedCustomer.value = customer;
      showDetailsModal.value = true;
    };

    const closeDetailsModal = () => {
      showDetailsModal.value = false;
      setTimeout(() => {
        selectedCustomer.value = null;
      }, 300);
    };

    const openAddCustomerModal = () => {
      editingCustomer.value = null;
      customerForm.value = {
        fullName: "",
        email: "",
        password: "",
        role: "Customer",
        gender: "",
        dateOfBirth: "",
        profilePictureUrl: "",
        contactNumber: "",
        currentAddressLine1: "",
        postcode: "",
        countryLocationId: null,
        nationalityCountryId: null,
        enableMFA: false,
        isCreatedByRecruiter: false,
        isEmailVerified: false,
      };
      showAddEditModal.value = true;
    };

    const editCustomer = (customer) => {
      editingCustomer.value = customer;
      customerForm.value = {
        fullName: customer.fullName,
        email: customer.email,
        password: "", // Don't populate password for editing
        role: customer.role,
        gender: customer.gender || "",
        dateOfBirth: customer.dateOfBirth
          ? customer.dateOfBirth.split("T")[0]
          : "",
        profilePictureUrl: customer.profilePictureUrl || "",
        contactNumber: customer.contactNumber || "",
        currentAddressLine1: customer.currentAddressLine1 || "",
        postcode: customer.postcode || "",
        countryLocationId: customer.countryLocationId,
        nationalityCountryId: customer.nationalityCountryId,
        enableMFA: customer.enableMFA,
        isCreatedByRecruiter: customer.isCreatedByRecruiter,
        isEmailVerified: customer.isEmailVerified,
      };
      showAddEditModal.value = true;
    };

    const closeAddEditModal = () => {
      showAddEditModal.value = false;
      setTimeout(() => {
        editingCustomer.value = null;
        customerForm.value = {
          fullName: "",
          email: "",
          password: "",
          gender: "",
          dateOfBirth: "",
          profilePictureUrl: "",
          contactNumber: "",
          currentAddressLine1: "",
          postcode: "",
        };
      }, 300);
    };

    const saveCustomer = async () => {
      isSaving.value = true;

      try {
        let response;

        if (editingCustomer.value) {
          // UPDATE EXISTING CUSTOMER

          // 1. First update the customer data (without image)
          await axios.put(`${API_BASE}/api/Users/${editingCustomer.value.id}`, {
            fullName: customerForm.value.fullName,
            gender: customerForm.value.gender,
            dateOfBirth: customerForm.value.dateOfBirth,
            email: customerForm.value.email,
            currentAddressLine1: customerForm.value.currentAddressLine1,
            postcode: customerForm.value.postcode,
            contactNumber: customerForm.value.contactNumber,
            profilePictureUrl: customerForm.value.profilePictureUrl, // Keep existing URL
          });

          // 2. If there's a new image, upload it separately
          if (customerForm.value.image) {
            const formData = new FormData();
            formData.append("file", customerForm.value.image);

            const imageResponse = await axios.post(
              `${API_BASE}/api/Users/${editingCustomer.value.id}/upload-profile-picture`,
              formData,
              {
                headers: {
                  "Content-Type": "multipart/form-data",
                },
              }
            );

            // The API returns the new URL, you might want to update your local data
            console.log("New profile picture URL:", imageResponse.data.url);
          }

          showMessage("Customer updated successfully!", "success");
        } else {
          // CREATE NEW CUSTOMER

          // 1. First create the customer (without image)
          response = await axios.post(`${API_BASE}/api/Users`, {
            email: customerForm.value.email,
            password: customerForm.value.password, // You'll need this field
            fullName: customerForm.value.fullName,
            gender: customerForm.value.gender,
            dateOfBirth: customerForm.value.dateOfBirth,
            currentAddressLine1: customerForm.value.currentAddressLine1,
            postcode: customerForm.value.postcode,
            contactNumber: customerForm.value.contactNumber,
            profilePictureUrl: null, // Will be updated after image upload
          });

          // 2. If there's an image, upload it after creating the user
          if (customerForm.value.image && response.data.userId) {
            const formData = new FormData();
            formData.append("file", customerForm.value.image);

            await axios.post(
              `${API_BASE}/api/Users/${response.data.userId}/upload-profile-picture`,
              formData,
              {
                headers: {
                  "Content-Type": "multipart/form-data",
                },
              }
            );
          }

          showMessage("Customer created successfully!", "success");
        }

        // Reload customers and close modal
        await loadCustomers(currentPage.value);
        closeAddEditModal();
      } catch (error) {
        console.error("Error saving customer:", error);
        showMessage("Error saving customer. Please try again.", "error");
      } finally {
        isSaving.value = false;
      }
    };

    const clearAllFilters = () => {
      filters.value = {
        role: "",
        emailVerified: "",
        registered: "",
        search: "",
      };
      loadCustomers(1);
    };

    const exportCustomers = () => {
      if (selectedCustomers.value.length === 0) return;
      showMessage(
        `Exporting ${selectedCustomers.value.length} customers...`,
        "success"
      );
    };

    const sendBulkEmail = () => {
      if (selectedCustomers.value.length === 0) return;
      showMessage(
        `Sending email to ${selectedCustomers.value.length} customers...`,
        "success"
      );
    };

    const openBulkStatusModal = () => {
      if (selectedCustomers.value.length === 0) return;
      showMessage("Bulk status update feature coming soon!", "info");
    };

    const viewOrderHistory = (customer) => {
      showMessage(`Viewing order history for ${customer.fullName}...`, "info");
    };

    const sendEmail = (customer) => {
      showMessage(`Sending email to ${customer.fullName}...`, "success");
    };

    const resetPassword = (customer) => {
      if (confirm(`Reset password for ${customer.fullName}?`)) {
        showMessage(
          `Password reset email sent to ${customer.fullName}!`,
          "success"
        );
      }
    };

    const deleteCustomer = (customer) => {
      if (
        confirm(
          `Are you sure you want to delete ${customer.fullName}? This action cannot be undone.`
        )
      ) {
        const index = customers.value.findIndex((c) => c.id === customer.id);
        if (index !== -1) {
          customers.value.splice(index, 1);
          totalCustomers.value--;
          showMessage(`${customer.fullName} has been deleted!`, "success");
        }
      }
    };

    const sortColumn = (column) => {
      showMessage(`Sorting by ${column}...`, "info");
    };

    const getInitials = (name) => {
      if (!name || typeof name !== "string") {
        return "N/A";
      }
      return name
        .split(" ")
        .map((n) => n[0])
        .join("")
        .toUpperCase();
    };

    const getStatusIcon = (status) => {
      const icons = {
        Active: "fas fa-check-circle",
        Inactive: "fas fa-pause-circle",
      };
      return icons[status] || "fas fa-circle";
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

    const formatCurrency = (amount) => {
      return Number.parseFloat(amount || 0).toFixed(2);
    };

    const formatDate = (dateString, detailed = false) => {
      if (!dateString) return "N/A";
      const options = detailed
        ? {
            year: "numeric",
            month: "long",
            day: "numeric",
            hour: "2-digit",
            minute: "2-digit",
          }
        : { year: "numeric", month: "short", day: "numeric" };
      return new Date(dateString).toLocaleDateString("en-US", options);
    };

    const formatTime = (dateString) => {
      if (!dateString) return "";
      return new Date(dateString).toLocaleTimeString("en-US", {
        hour: "2-digit",
        minute: "2-digit",
      });
    };

    let searchTimeout = null;
    const debouncedSearch = () => {
      clearTimeout(searchTimeout);
      searchTimeout = setTimeout(() => {
        loadCustomers(1);
      }, 500);
    };

    const productStore = useProductStore();

    onMounted(() => {
      loadCustomers(1);
      productStore.initSignalR();
    });

    return {
      customers,
      selectedCustomer,
      editingCustomer,
      isLoading,
      isSaving,
      currentPage,
      totalPages,
      totalItems,
      pageSize,
      viewMode,
      sortBy,
      showDetailsModal,
      showAddEditModal,
      messages,
      selectedCustomers,
      selectAll,
      isSidebarCollapsed,
      totalCustomers,
      activeCustomers,
      newCustomersCount,
      avgOrdersPerCustomer,
      filters,
      customerForm,
      paginationInfo,
      displayedPages,
      toggleSidebar,
      loadCustomers,
      toggleSelectAll,
      viewCustomerDetails,
      closeDetailsModal,
      openAddCustomerModal,
      editCustomer,
      closeAddEditModal,
      saveCustomer,
      clearAllFilters,
      exportCustomers,
      sendBulkEmail,
      openBulkStatusModal,
      viewOrderHistory,
      sendEmail,
      resetPassword,
      deleteCustomer,
      sortColumn,
      getInitials,
      getStatusIcon,
      showMessage,
      removeMessage,
      formatCurrency,
      formatDate,
      formatTime,
      debouncedSearch,
      imageBase,
      handleImageError,
      isCustomerActive,
      calculateAge,
      handleImageUpload,
    };
  },
};
</script>

<style scoped>
/* Import all the base styles from OrdersPage and add customer-specific styles */

/* Base Desktop Layout - Full Viewport */
.admin-dashboard-desktop-layout {
  display: flex;
  width: 100vw;
  height: 100vh;
  overflow: hidden;
  background: linear-gradient(135deg, #f5f7fa 0%, #c3cfe2 100%);
  font-family: "Segoe UI", Tahoma, Geneva, Verdana, sans-serif;
}

/* Sidebar Navigation Panel */
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

/* Main Panel Content */
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

/* Top Navigation Bar */
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

/* Content Area Wrapper */
.content-area-wrapper {
  flex-grow: 1;
  overflow-y: auto;
  padding: 1.5625rem;
  display: flex;
  flex-direction: column;
  gap: 1.5625rem;
}

/* Stats Dashboard Section */
.stats-dashboard-section {
  margin-bottom: 0.3125rem;
}

.stats-grid-container {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(15rem, 1fr));
  gap: 1.5625rem;
  width: 100%;
}

@media (min-width: 1200px) {
  .stats-grid-container {
    grid-template-columns: repeat(4, 1fr);
  }
}

.stat-card {
  background: white;
  border-radius: 1rem;
  padding: 0;
  box-shadow: 0 0.5rem 1.875rem rgba(0, 0, 0, 0.08);
  border: 1px solid rgba(255, 255, 255, 0.2);
  transition: all 0.3s ease;
  position: relative;
  overflow: hidden;
}

.stat-card::before {
  content: "";
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  height: 0.25rem;
}

.total-customers-stat::before {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
}

.active-customers-stat::before {
  background: linear-gradient(135deg, #2ecc71 0%, #27ae60 100%);
}

.new-customers-stat::before {
  background: linear-gradient(135deg, #f39c12 0%, #e67e22 100%);
}

.avg-orders-stat::before {
  background: linear-gradient(135deg, #9b59b6 0%, #8e44ad 100%);
}

.stat-card:hover {
  transform: translateY(-0.3125rem);
  box-shadow: 0 0.9375rem 2.5rem rgba(0, 0, 0, 0.12);
}

.stat-card-inner {
  display: flex;
  align-items: center;
  gap: 1.25rem;
  padding: 1.5625rem;
}

.stat-icon-container {
  width: 3.75rem;
  height: 3.75rem;
  border-radius: 0.75rem;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 1.8rem;
  color: white;
}

.total-customers-stat .stat-icon-container {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
}

.active-customers-stat .stat-icon-container {
  background: linear-gradient(135deg, #2ecc71 0%, #27ae60 100%);
}

.new-customers-stat .stat-icon-container {
  background: linear-gradient(135deg, #f39c12 0%, #e67e22 100%);
}

.avg-orders-stat .stat-icon-container {
  background: linear-gradient(135deg, #9b59b6 0%, #8e44ad 100%);
}

.stat-content-container {
  flex: 1;
}

.stat-number {
  font-size: 2.2rem;
  font-weight: 700;
  color: #2c3e50;
  line-height: 1;
  margin-bottom: 0.3125rem;
}

.stat-label {
  font-size: 1rem;
  color: #7f8c8d;
  font-weight: 500;
  margin-bottom: 0.5rem;
}

.stat-trend {
  display: flex;
  align-items: center;
  gap: 0.3125rem;
  font-size: 0.85rem;
  font-weight: 500;
}

.stat-trend.positive {
  color: #27ae60;
}

.stat-trend.warning {
  color: #f39c12;
}

/* Advanced Controls Section */
.advanced-controls-section {
  margin-bottom: 0.3125rem;
}

.controls-grid-layout {
  display: grid;
  grid-template-columns: 2fr 1fr;
  gap: 1.875rem;
  width: 100%;
}

@media (max-width: 1024px) {
  .controls-grid-layout {
    grid-template-columns: 1fr;
  }
}

.filters-panel-advanced,
.customer-actions-panel {
  background: white;
  border-radius: 1rem;
  padding: 1.5625rem;
  box-shadow: 0 0.5rem 1.875rem rgba(0, 0, 0, 0.08);
  border: 1px solid rgba(255, 255, 255, 0.2);
}

.panel-header-section {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 1.25rem;
  padding-bottom: 0.9375rem;
  border-bottom: 2px solid #f1f3f4;
}

.panel-title {
  font-size: 1.2rem;
  font-weight: 600;
  color: #2c3e50;
  margin: 0;
  display: flex;
  align-items: center;
  gap: 0.625rem;
}

.selected-count {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
  padding: 0.25rem 0.75rem;
  border-radius: 1rem;
  font-size: 0.85rem;
  font-weight: 600;
}

.clear-filters-btn {
  background: linear-gradient(135deg, #95a5a6 0%, #7f8c8d 100%);
  color: white;
  border: none;
  border-radius: 0.5rem;
  padding: 0.5rem 1rem;
  cursor: pointer;
  transition: all 0.3s ease;
  font-size: 0.9rem;
  display: flex;
  align-items: center;
  gap: 0.375rem;
}

.clear-filters-btn:hover {
  transform: translateY(-0.125rem);
  box-shadow: 0 0.3125rem 0.9375rem rgba(149, 165, 166, 0.4);
}

/* Advanced Filters Grid */
.advanced-filters-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(12rem, 1fr));
  gap: 1.25rem;
  align-items: end;
}

@media (min-width: 768px) {
  .advanced-filters-grid {
    grid-template-columns: repeat(3, 1fr);
  }
}

.filter-group-container {
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
}

.search-group-advanced {
  grid-column: span 2;
}

@media (max-width: 768px) {
  .search-group-advanced {
    grid-column: span 1;
  }
}

.filter-label-text {
  font-weight: 600;
  color: #2c3e50;
  font-size: 0.9rem;
  margin-bottom: 0.1875rem;
}

.filter-select-input {
  padding: 0.75rem 0.9375rem;
  border: 2px solid #e9ecef;
  border-radius: 0.625rem;
  font-size: 1rem;
  transition: all 0.3s ease;
  background: white;
}

.filter-select-input:focus {
  outline: none;
  border-color: #667eea;
  box-shadow: 0 0 0 3px rgba(102, 126, 234, 0.1);
}

.search-input-container {
  position: relative;
}

.search-input-icon {
  position: absolute;
  left: 0.9375rem;
  top: 50%;
  transform: translateY(-50%);
  color: #7f8c8d;
  font-size: 1rem;
}

.search-input-field {
  width: 100%;
  padding: 0.75rem 0.9375rem 0.75rem 2.8125rem;
  border: 2px solid #e9ecef;
  border-radius: 0.625rem;
  font-size: 1rem;
  transition: all 0.3s ease;
  background: white;
}

.search-input-field:focus {
  outline: none;
  border-color: #667eea;
  box-shadow: 0 0 0 3px rgba(102, 126, 234, 0.1);
}

/* Customer Actions Grid */
.customer-actions-grid {
  display: grid;
  grid-template-columns: repeat(2, 1fr);
  gap: 0.9375rem;
}

.customer-action-btn {
  border: none;
  border-radius: 0.625rem;
  padding: 0.9375rem 1.25rem;
  cursor: pointer;
  transition: all 0.3s ease;
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 0.5rem;
  font-weight: 600;
  text-align: center;
  color: white;
}

.customer-action-btn:hover:not(:disabled) {
  transform: translateY(-0.1875rem);
  box-shadow: 0 0.5rem 1.5625rem rgba(0, 0, 0, 0.2);
}

.customer-action-btn:disabled {
  opacity: 0.5;
  cursor: not-allowed;
  transform: none;
}

.add-customer {
  background: linear-gradient(135deg, #2ecc71 0%, #27ae60 100%);
}

.export-customers {
  background: linear-gradient(135deg, #3498db 0%, #2980b9 100%);
}

.send-email {
  background: linear-gradient(135deg, #f39c12 0%, #e67e22 100%);
}

.manage-status {
  background: linear-gradient(135deg, #9b59b6 0%, #8e44ad 100%);
}

.customer-action-btn i {
  font-size: 1.2rem;
}

.customer-action-btn span {
  font-size: 0.9rem;
}

/* Customers Table Section */
.customers-table-section {
  background: white;
  border-radius: 1rem;
  padding: 1.875rem;
  box-shadow: 0 0.5rem 1.875rem rgba(0, 0, 0, 0.08);
  border: 1px solid rgba(255, 255, 255, 0.2);
  flex-grow: 1;
  display: flex;
  flex-direction: column;
}

.section-header-container {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 1.5625rem;
  padding-bottom: 1.25rem;
  border-bottom: 2px solid #f1f3f4;
}

@media (max-width: 768px) {
  .section-header-container {
    flex-direction: column;
    gap: 1rem;
    align-items: flex-start;
  }
}

.header-left-section {
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
}

.section-main-title {
  font-size: 1.5rem;
  font-weight: 700;
  color: #2c3e50;
  margin: 0;
  display: flex;
  align-items: center;
  gap: 0.75rem;
}

.results-info-text {
  color: #7f8c8d;
  font-size: 0.95rem;
}

.header-right-section {
  display: flex;
  align-items: center;
  gap: 1.25rem;
}

@media (max-width: 768px) {
  .header-right-section {
    width: 100%;
    justify-content: space-between;
  }
}

.view-options-container {
  display: flex;
  align-items: center;
  gap: 1rem;
}

.view-toggle-options {
  display: flex;
  background: #f8f9fa;
  border-radius: 0.625rem;
  padding: 0.25rem;
}

.view-toggle-btn {
  background: none;
  border: none;
  padding: 0.5rem 1rem;
  border-radius: 0.5rem;
  cursor: pointer;
  transition: all 0.3s ease;
  color: #7f8c8d;
  display: flex;
  align-items: center;
  gap: 0.375rem;
  font-size: 0.9rem;
}

.view-toggle-btn.active {
  background: white;
  color: #667eea;
  box-shadow: 0 0.125rem 0.5rem rgba(0, 0, 0, 0.1);
}

.sort-options-container {
  position: relative;
}

.sort-dropdown-select {
  padding: 0.5rem 0.75rem;
  border: 2px solid #e9ecef;
  border-radius: 0.5rem;
  font-size: 0.9rem;
  background: white;
  cursor: pointer;
}

/* Loading State */
.loading-state-container {
  display: flex;
  justify-content: center;
  align-items: center;
  padding: 5rem 1.25rem;
  flex-grow: 1;
}

.loading-content-wrapper {
  text-align: center;
  max-width: 25rem;
}

.loading-spinner-container {
  margin-bottom: 1.5625rem;
}

.spinner-ring-animation {
  width: 3.75rem;
  height: 3.75rem;
  border: 4px solid #f3f3f3;
  border-top: 4px solid #667eea;
  border-radius: 50%;
  animation: spin 1s linear infinite;
  margin: 0 auto;
}

@keyframes spin {
  0% {
    transform: rotate(0deg);
  }
  100% {
    transform: rotate(360deg);
  }
}

.loading-title {
  color: #2c3e50;
  margin-bottom: 0.625rem;
  font-size: 1.3rem;
}

.loading-description {
  color: #7f8c8d;
  font-size: 1rem;
}

/* Table Styles */
.table-wrapper-container {
  margin-bottom: 1.5625rem;
  flex-grow: 1;
  overflow: hidden;
}

.table-scroll-container {
  border-radius: 0.75rem;
  overflow: auto;
  box-shadow: 0 0.25rem 1.25rem rgba(0, 0, 0, 0.08);
  border: 1px solid #e9ecef;
  height: 100%;
}

.customers-data-table {
  width: 100%;
  border-collapse: collapse;
  background: white;
}

.table-header-section {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
}

.customers-data-table th {
  padding: 1.125rem 1.25rem;
  text-align: left;
  font-weight: 600;
  text-transform: uppercase;
  font-size: 0.85rem;
  letter-spacing: 0.03125rem;
  position: relative;
}

.sortable-column-header {
  cursor: pointer;
  user-select: none;
}

.sortable-column-header:hover {
  background: rgba(255, 255, 255, 0.1);
}

.sort-indicator {
  margin-left: 0.5rem;
  opacity: 0.7;
  font-size: 0.8rem;
}

.customers-data-table td {
  padding: 1.25rem;
  border-bottom: 1px solid #f1f3f4;
  vertical-align: middle;
}

.table-body-section tr {
  transition: all 0.3s ease;
}

.table-data-row:hover {
  background: linear-gradient(135deg, #f8f9fa 0%, #e3f2fd 100%);
}

.checkbox-column-header,
.checkbox-column-cell {
  width: 3.125rem;
  text-align: center;
}

.header-checkbox,
.row-checkbox {
  width: 1.125rem;
  height: 1.125rem;
  cursor: pointer;
  accent-color: #667eea;
}

/* Customer Info Display */
.customer-info-display {
  display: flex;
  align-items: center;
  gap: 0.75rem;
}

.customer-avatar-container {
  width: 2.5rem;
  height: 2.5rem;
  border-radius: 50%;
  overflow: hidden;
  flex-shrink: 0;
}

.customer-avatar-img {
  width: 100%;
  height: 100%;
  object-fit: cover;
}

.customer-avatar-placeholder {
  width: 100%;
  height: 100%;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  display: flex;
  align-items: center;
  justify-content: center;
  color: white;
  font-weight: 600;
  font-size: 0.9rem;
}

.customer-details-container {
  display: flex;
  flex-direction: column;
  gap: 0.125rem;
}

.customer-name-text {
  font-weight: 600;
  color: #2c3e50;
  font-size: 0.95rem;
}

.customer-id-text {
  font-size: 0.8rem;
  color: #7f8c8d;
}

/* Contact Info */
.contact-info-container {
  display: flex;
  flex-direction: column;
  gap: 0.25rem;
}

.contact-email,
.contact-phone {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  font-size: 0.9rem;
  color: #2c3e50;
}

.contact-email i,
.contact-phone i {
  color: #7f8c8d;
  font-size: 0.8rem;
  width: 1rem;
}

.verified-icon {
  color: #27ae60;
  font-size: 0.8rem;
  margin-left: 0.25rem;
}

/* Role Badge */
.role-badge {
  display: inline-flex;
  align-items: center;
  padding: 0.25rem 0.5rem;
  border-radius: 0.75rem;
  font-size: 0.75rem;
  font-weight: 600;
  text-transform: uppercase;
  letter-spacing: 0.03125rem;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
}

/* Customer Status Badge */
.customer-status-badge {
  display: inline-flex;
  align-items: center;
  gap: 0.375rem;
  padding: 0.5rem 0.875rem;
  border-radius: 1.25rem;
  font-size: 0.85rem;
  font-weight: 600;
  text-transform: uppercase;
  letter-spacing: 0.03125rem;
}

.status-active {
  background: linear-gradient(135deg, #2ecc71 0%, #27ae60 100%);
  color: white;
}

.status-inactive {
  background: linear-gradient(135deg, #95a5a6 0%, #7f8c8d 100%);
  color: white;
}

.status-blocked {
  background: linear-gradient(135deg, #e74c3c 0%, #c0392b 100%);
  color: white;
}

/* Status Indicators */
.status-indicators {
  display: flex;
  flex-direction: column;
  gap: 0.25rem;
}

.security-indicators {
  display: flex;
  gap: 0.25rem;
}

.mfa-icon {
  color: #27ae60;
  font-size: 0.8rem;
}

.recruiter-icon {
  color: #3498db;
  font-size: 0.8rem;
}

/* Orders Summary */
.orders-summary {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 0.125rem;
}

.orders-count {
  font-size: 1.2rem;
  font-weight: 700;
  color: #2c3e50;
}

.orders-label {
  font-size: 0.8rem;
  color: #7f8c8d;
}

/* Last Login Info */
.last-login-info {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 0.125rem;
}

.last-login-date {
  font-size: 0.9rem;
  color: #2c3e50;
}

.no-login-text {
  font-size: 0.8rem;
  color: #7f8c8d;
  font-style: italic;
}

/* Date Display */
.date-display-container {
  display: flex;
  flex-direction: column;
  gap: 0.125rem;
}

.date-primary-text {
  font-weight: 500;
  color: #2c3e50;
  font-size: 0.95rem;
}

.date-secondary-text {
  font-size: 0.8rem;
  color: #7f8c8d;
}

/* Action Buttons */
.actions-column-header,
.actions-column-cell {
  width: 11.25rem;
}

.action-buttons-container {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  justify-content: center;
}

.action-button-small {
  width: 2.25rem;
  height: 2.25rem;
  border: none;
  border-radius: 0.5rem;
  cursor: pointer;
  transition: all 0.3s ease;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 0.9rem;
}

.view-action {
  background: linear-gradient(135deg, #3498db 0%, #2980b9 100%);
  color: white;
}

.edit-action {
  background: linear-gradient(135deg, #f39c12 0%, #e67e22 100%);
  color: white;
}

.more-actions {
  background: #f8f9fa;
  color: #7f8c8d;
  border: 1px solid #e9ecef;
}

.action-button-small:hover {
  transform: translateY(-0.125rem);
  box-shadow: 0 0.25rem 0.75rem rgba(0, 0, 0, 0.15);
}

.dropdown-menu-container {
  position: relative;
}

.dropdown-menu-list {
  position: absolute;
  top: 100%;
  right: 0;
  background: white;
  border-radius: 0.5rem;
  box-shadow: 0 0.5rem 1.875rem rgba(0, 0, 0, 0.15);
  border: 1px solid #e9ecef;
  min-width: 10rem;
  z-index: 10;
  display: none;
  padding: 0.5rem 0;
}

.dropdown-menu-container:hover .dropdown-menu-list {
  display: block;
}

.dropdown-menu-item {
  display: flex;
  align-items: center;
  gap: 0.625rem;
  padding: 0.75rem 1rem;
  color: #2c3e50;
  text-decoration: none;
  font-size: 0.9rem;
  transition: background 0.2s ease;
}

.dropdown-menu-item:hover {
  background: #f8f9fa;
}

.dropdown-divider {
  height: 1px;
  background: #e9ecef;
  margin: 0.5rem 0;
}

.danger-item {
  color: #e74c3c;
}

.danger-item:hover {
  background: #ffe6e6;
}

/* Cards View */
.cards-view-container {
  margin-bottom: 1.5625rem;
  flex-grow: 1;
}

.customers-cards-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(22rem, 1fr));
  gap: 1.25rem;
}

.customer-card-item {
  background: white;
  border-radius: 0.75rem;
  box-shadow: 0 0.25rem 1.25rem rgba(0, 0, 0, 0.08);
  border: 1px solid #e9ecef;
  transition: all 0.3s ease;
  overflow: hidden;
}

.customer-card-item:hover {
  transform: translateY(-0.25rem);
  box-shadow: 0 0.5rem 1.875rem rgba(0, 0, 0, 0.12);
}

.customer-card-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 1rem 1.25rem;
  border-bottom: 1px solid #f1f3f4;
  background: #f8f9fa;
}

.customer-card-avatar {
  display: flex;
  align-items: center;
  gap: 0.75rem;
}

.card-checkbox {
  width: 1rem;
  height: 1rem;
  accent-color: #667eea;
}

.avatar-container {
  width: 2.5rem;
  height: 2.5rem;
  border-radius: 50%;
  overflow: hidden;
}

.avatar-img {
  width: 100%;
  height: 100%;
  object-fit: cover;
}

.avatar-placeholder {
  width: 100%;
  height: 100%;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  display: flex;
  align-items: center;
  justify-content: center;
  color: white;
  font-weight: 600;
  font-size: 0.9rem;
}

.card-status-badges {
  display: flex;
  flex-direction: column;
  gap: 0.25rem;
  align-items: flex-end;
}

.customer-card-body {
  padding: 1.25rem;
}

.customer-name-card {
  font-size: 1.1rem;
  font-weight: 600;
  color: #2c3e50;
  margin-bottom: 0.75rem;
}

.customer-contact-card {
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
  margin-bottom: 1rem;
}

.contact-item {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  font-size: 0.9rem;
  color: #2c3e50;
}

.contact-item i {
  color: #7f8c8d;
  font-size: 0.8rem;
  width: 1rem;
}

.customer-stats-card {
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
}

.stat-item {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.stat-label {
  font-size: 0.9rem;
  color: #7f8c8d;
}

.stat-value {
  font-weight: 600;
  color: #2c3e50;
}

.security-indicators-card {
  display: flex;
  gap: 0.25rem;
  align-items: center;
}

.no-security {
  font-size: 0.8rem;
  color: #7f8c8d;
}

.customer-card-footer {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 1rem 1.25rem;
  border-top: 1px solid #f1f3f4;
  background: #f8f9fa;
}

.registration-date-card {
  font-size: 0.9rem;
  color: #7f8c8d;
}

.card-actions {
  display: flex;
  gap: 0.5rem;
}

.card-action-btn {
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

.view-btn {
  background: linear-gradient(135deg, #3498db 0%, #2980b9 100%);
  color: white;
}

.edit-btn {
  background: linear-gradient(135deg, #f39c12 0%, #e67e22 100%);
  color: white;
}

.more-btn {
  background: #f8f9fa;
  color: #7f8c8d;
  border: 1px solid #e9ecef;
}

.card-action-btn:hover {
  transform: translateY(-0.125rem);
  box-shadow: 0 0.25rem 0.75rem rgba(0, 0, 0, 0.15);
}

/* Empty State */
.empty-state-container {
  display: flex;
  justify-content: center;
  align-items: center;
  padding: 5rem 1.25rem;
  flex-grow: 1;
}

.empty-state-content {
  text-align: center;
  max-width: 25rem;
}

.empty-state-icon {
  font-size: 4rem;
  color: #bdc3c7;
  margin-bottom: 1.5625rem;
}

.empty-state-title {
  color: #2c3e50;
  margin-bottom: 0.9375rem;
  font-size: 1.3rem;
}

.empty-state-description {
  color: #7f8c8d;
  margin-bottom: 1.5625rem;
  line-height: 1.6;
}

.empty-state-action-btn {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
  border: none;
  border-radius: 0.625rem;
  padding: 0.75rem 1.5rem;
  cursor: pointer;
  transition: all 0.3s ease;
  display: inline-flex;
  align-items: center;
  gap: 0.5rem;
  font-weight: 600;
}

.empty-state-action-btn:hover {
  transform: translateY(-0.125rem);
  box-shadow: 0 0.5rem 1.5625rem rgba(102, 126, 234, 0.4);
}

/* Pagination */
.pagination-section-container {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-top: 1.5625rem;
  padding-top: 1.25rem;
  border-top: 2px solid #f1f3f4;
}

@media (max-width: 768px) {
  .pagination-section-container {
    flex-direction: column;
    gap: 1rem;
  }
}

.pagination-info-section {
  display: flex;
  align-items: center;
  gap: 0.625rem;
  color: #7f8c8d;
  font-size: 0.95rem;
}

.page-size-selector-container {
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.page-size-label {
  font-weight: 500;
}

.page-size-select-dropdown {
  padding: 0.375rem 0.625rem;
  border: 1px solid #e9ecef;
  border-radius: 0.375rem;
  font-size: 0.9rem;
  background: white;
}

.page-size-suffix {
  color: #7f8c8d;
}

.pagination-controls-container {
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.pagination-control-btn {
  background: white;
  border: 2px solid #e9ecef;
  color: #667eea;
  border-radius: 0.5rem;
  padding: 0.625rem 1rem;
  cursor: pointer;
  transition: all 0.3s ease;
  display: flex;
  align-items: center;
  gap: 0.375rem;
  font-weight: 600;
  font-size: 0.9rem;
}

.pagination-control-btn:hover:not(:disabled) {
  border-color: #667eea;
  background: #667eea;
  color: white;
  transform: translateY(-0.125rem);
}

.pagination-control-btn:disabled {
  opacity: 0.5;
  cursor: not-allowed;
  transform: none;
}

.page-numbers-container {
  display: flex;
  gap: 0.25rem;
}

.page-number-btn {
  width: 2.5rem;
  height: 2.5rem;
  border: 2px solid #e9ecef;
  border-radius: 0.5rem;
  cursor: pointer;
  transition: all 0.3s ease;
  font-weight: 600;
  color: #2c3e50;
  display: flex;
  align-items: center;
  justify-content: center;
  background: white;
}

.page-number-btn:hover {
  border-color: #667eea;
  background: #667eea;
  color: white;
}

.active-page {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  border-color: transparent;
  color: white;
}

/* Modal Styles */
.modal-overlay-backdrop {
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

.modal-container-wrapper {
  background: white;
  border-radius: 1rem;
  width: 100%;
  max-width: 50rem;
  max-height: 90vh;
  overflow-y: auto;
  box-shadow: 0 1.5625rem 5rem rgba(0, 0, 0, 0.3);
}

.customer-details-modal {
  max-width: 60rem;
}

.add-edit-modal {
  max-width: 50rem;
}

.modal-header-section {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
  padding: 1.5625rem 1.875rem;
  border-radius: 1rem 1rem 0 0;
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.modal-title-text {
  margin: 0;
  font-size: 1.4rem;
  font-weight: 700;
  display: flex;
  align-items: center;
  gap: 0.75rem;
}

.modal-close-button {
  background: rgba(255, 255, 255, 0.2);
  border: none;
  color: white;
  width: 2.5rem;
  height: 2.5rem;
  border-radius: 50%;
  cursor: pointer;
  transition: all 0.3s ease;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 1.2rem;
}

.modal-close-button:hover {
  background: rgba(255, 255, 255, 0.3);
  transform: rotate(90deg);
}

.modal-body-section {
  padding: 1.875rem;
}

/* Customer Profile Section */
.customer-profile-section {
  background: #f8f9fa;
  border-radius: 0.75rem;
  padding: 1.5rem;
  margin-bottom: 1.5rem;
}

.profile-avatar-section {
  display: flex;
  align-items: center;
  gap: 1.25rem;
}

.profile-avatar-container {
  width: 4rem;
  height: 4rem;
  border-radius: 50%;
  overflow: hidden;
  flex-shrink: 0;
}

.profile-avatar-img {
  width: 100%;
  height: 100%;
  object-fit: cover;
}

.profile-avatar-placeholder {
  width: 100%;
  height: 100%;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  display: flex;
  align-items: center;
  justify-content: center;
  color: white;
  font-weight: 700;
  font-size: 1.2rem;
}

.profile-info {
  flex: 1;
}

.profile-name {
  font-size: 1.5rem;
  font-weight: 700;
  color: #2c3e50;
  margin-bottom: 0.5rem;
}

.profile-badges {
  display: flex;
  gap: 0.5rem;
  margin-bottom: 0.5rem;
}

.profile-meta {
  display: flex;
  flex-direction: column;
  gap: 0.25rem;
  margin-top: 0.5rem;
}

.customer-id {
  font-size: 0.9rem;
  color: #7f8c8d;
  font-weight: 500;
}

.registration-date {
  font-size: 0.9rem;
  color: #7f8c8d;
}

/* Details Grid */
.details-grid-container {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 1.875rem;
  margin-bottom: 1.5625rem;
}

@media (max-width: 768px) {
  .details-grid-container {
    grid-template-columns: 1fr;
    gap: 1.25rem;
  }
}

.details-section-container {
  background: #f8f9fa;
  border-radius: 0.75rem;
  padding: 1.25rem;
}

.details-section-title {
  font-size: 1.1rem;
  font-weight: 700;
  color: #2c3e50;
  margin-bottom: 1.25rem;
  padding-bottom: 0.625rem;
  border-bottom: 2px solid #e9ecef;
}

.details-table-container {
  display: flex;
  flex-direction: column;
  gap: 0.75rem;
}

.detail-row-container {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 0.5rem 0;
}

.detail-label-text {
  font-weight: 600;
  color: #7f8c8d;
  font-size: 0.9rem;
}

.detail-value-text {
  font-weight: 500;
  color: #2c3e50;
  text-align: right;
  display: flex;
  align-items: center;
  gap: 0.25rem;
}

.text-green-500 {
  color: #27ae60;
}

.text-red-500 {
  color: #e74c3c;
}

.text-gray-500 {
  color: #7f8c8d;
}

/* Recent Orders Section */
.recent-orders-section {
  background: #f8f9fa;
  border-radius: 0.75rem;
  padding: 1.25rem;
  margin-bottom: 1.5625rem;
}

.recent-orders-list {
  display: flex;
  flex-direction: column;
  gap: 0.75rem;
}

.recent-order-item {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 0.75rem;
  background: white;
  border-radius: 0.5rem;
  border: 1px solid #e9ecef;
}

.order-info {
  display: flex;
  flex-direction: column;
  gap: 0.25rem;
}

.order-id {
  font-weight: 600;
  color: #2c3e50;
  font-size: 0.9rem;
}

.order-date {
  font-size: 0.8rem;
  color: #7f8c8d;
}

.order-amount {
  font-weight: 600;
  color: #27ae60;
  font-size: 1rem;
}

.order-status-badge {
  display: inline-flex;
  align-items: center;
  gap: 0.25rem;
  padding: 0.25rem 0.5rem;
  border-radius: 0.75rem;
  font-size: 0.75rem;
  font-weight: 600;
  text-transform: uppercase;
}

.status-completed {
  background: #e8f5e8;
  color: #27ae60;
}

.status-pendingapproval {
  background: #fff3cd;
  color: #e67e22;
}

.status-confirmed {
  background: #e3f2fd;
  color: #2980b9;
}

.no-orders-message {
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 0.5rem;
  padding: 2rem;
  color: #7f8c8d;
  font-style: italic;
}

.modal-actions-container {
  display: flex;
  justify-content: center;
  gap: 0.9375rem;
  padding-top: 1.5625rem;
  border-top: 2px solid #f1f3f4;
}

@media (max-width: 768px) {
  .modal-actions-container {
    flex-direction: column;
  }
}

.modal-action-button {
  padding: 0.75rem 1.5rem;
  border: none;
  border-radius: 0.625rem;
  cursor: pointer;
  transition: all 0.3s ease;
  font-weight: 600;
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.edit-customer-action {
  background: linear-gradient(135deg, #f39c12 0%, #e67e22 100%);
  color: white;
}

.view-orders-action {
  background: linear-gradient(135deg, #9b59b6 0%, #8e44ad 100%);
  color: white;
}

.email-action {
  background: linear-gradient(135deg, #3498db 0%, #2980b9 100%);
  color: white;
}

.modal-action-button:hover {
  transform: translateY(-0.125rem);
  box-shadow: 0 0.5rem 1.5625rem rgba(0, 0, 0, 0.2);
}

/* Customer Form */
.customer-form {
  display: flex;
  flex-direction: column;
  gap: 2rem;
}

.form-section {
  border: 1px solid #e9ecef;
  border-radius: 0.75rem;
  padding: 1.5rem;
  background: #f8f9fa;
}

.form-section-title {
  font-size: 1.2rem;
  font-weight: 700;
  color: #2c3e50;
  margin-bottom: 1.5rem;
  padding-bottom: 0.75rem;
  border-bottom: 2px solid #e9ecef;
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.form-grid {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 1.25rem;
}

@media (max-width: 768px) {
  .form-grid {
    grid-template-columns: 1fr;
  }
}

.form-group {
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
}

.form-group.full-width {
  grid-column: span 2;
}

@media (max-width: 768px) {
  .form-group.full-width {
    grid-column: span 1;
  }
}

.checkbox-group {
  flex-direction: row;
  align-items: center;
}

.form-label {
  font-weight: 600;
  color: #2c3e50;
  font-size: 0.9rem;
}

.checkbox-label {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  cursor: pointer;
  font-weight: 500;
  color: #2c3e50;
}

.form-input,
.form-select {
  padding: 0.75rem;
  border: 2px solid #e9ecef;
  border-radius: 0.5rem;
  font-size: 1rem;
  transition: all 0.3s ease;
  background: white;
}

.form-input:focus,
.form-select:focus {
  outline: none;
  border-color: #667eea;
  box-shadow: 0 0 0 3px rgba(102, 126, 234, 0.1);
}

.form-checkbox {
  width: 1.125rem;
  height: 1.125rem;
  accent-color: #667eea;
}

.checkbox-text {
  font-size: 0.9rem;
}

.form-actions {
  display: flex;
  justify-content: flex-end;
  gap: 1rem;
  padding-top: 1.5rem;
  border-top: 2px solid #f1f3f4;
  background: white;
  margin: 0 -1.875rem -1.875rem;
  padding: 1.5rem 1.875rem;
  border-radius: 0 0 1rem 1rem;
}

@media (max-width: 768px) {
  .form-actions {
    flex-direction: column;
  }
}

.form-btn {
  padding: 0.75rem 1.5rem;
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

.cancel-btn:hover {
  background: #7f8c8d;
  transform: translateY(-2px);
}

.save-btn {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
}

.save-btn:hover:not(:disabled) {
  transform: translateY(-2px);
  box-shadow: 0 4px 12px rgba(102, 126, 234, 0.4);
}

.save-btn:disabled {
  opacity: 0.6;
  cursor: not-allowed;
  transform: none;
}

/* Toast Messages */
.toast-messages-container {
  position: fixed;
  top: 5rem;
  right: 1.875rem;
  z-index: 1001;
  display: flex;
  flex-direction: column;
  gap: 0.9375rem;
  max-width: 25rem;
}

@media (max-width: 768px) {
  .toast-messages-container {
    right: 0.9375rem;
    left: 0.9375rem;
    max-width: none;
  }
}

.toast-message-item {
  background: white;
  border-radius: 0.75rem;
  padding: 1.25rem;
  box-shadow: 0 0.5rem 1.875rem rgba(0, 0, 0, 0.15);
  border-left: 4px solid #2ecc71;
  display: flex;
  align-items: flex-start;
  gap: 0.9375rem;
  animation: slideInRight 0.4s ease;
}

.toast-success {
  border-left-color: #2ecc71;
}

.toast-error {
  border-left-color: #e74c3c;
}

.toast-icon-container {
  width: 1.5rem;
  height: 1.5rem;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  color: white;
  font-size: 0.9rem;
  flex-shrink: 0;
}

.toast-success .toast-icon-container {
  background: #2ecc71;
}

.toast-error .toast-icon-container {
  background: #e74c3c;
}

.toast-content-container {
  flex: 1;
}

.toast-title-text {
  font-weight: 600;
  color: #2c3e50;
  margin-bottom: 0.25rem;
  font-size: 0.95rem;
}

.toast-message-text {
  color: #7f8c8d;
  font-size: 0.9rem;
  line-height: 1.4;
}

.toast-close-button {
  background: none;
  border: none;
  color: #bdc3c7;
  cursor: pointer;
  padding: 0.25rem;
  border-radius: 0.25rem;
  transition: all 0.2s ease;
  flex-shrink: 0;
}

.toast-close-button:hover {
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
    padding: 1.25rem;
  }

  .stats-grid-container {
    grid-template-columns: repeat(2, 1fr);
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
    padding: 0.9375rem;
    gap: 1.25rem;
  }

  .stats-grid-container {
    grid-template-columns: 1fr;
  }

  .advanced-filters-grid {
    grid-template-columns: 1fr;
  }

  .customer-actions-grid {
    grid-template-columns: 1fr;
  }

  .action-buttons-container {
    flex-direction: column;
    gap: 0.25rem;
  }

  .action-button-small {
    width: 2rem;
    height: 2rem;
    font-size: 0.8rem;
  }

  .customers-cards-grid {
    grid-template-columns: 1fr;
  }
}

@media (max-width: 480px) {
  .customers-data-table {
    font-size: 0.85rem;
  }

  .customers-data-table th,
  .customers-data-table td {
    padding: 0.75rem 0.5rem;
  }
}
</style>
