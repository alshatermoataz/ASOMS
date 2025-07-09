<template>
  <div class="admin-dashboard-desktop-layout">
    <!-- Sidebar Navigation -->
    <aside :class="['sidebar-nav-panel', { collapsed: isSidebarCollapsed }]">
      <div class="sidebar-header">
        <div v-if="!isSidebarCollapsed" class="sidebar-logo-container">
          <i class="fas fa-shield-alt sidebar-logo-icon"></i>
          <span class="sidebar-logo-text">ASOMS Admin</span>
        </div>
        <button
          class="sidebar-toggle-btn"
          @click="toggleSidebar"
          :aria-expanded="!isSidebarCollapsed"
          aria-label="Toggle sidebar"
        >
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
            <a href="#" class="nav-item active"
              ><i class="fas fa-tachometer-alt fa-fw"></i
              ><span v-if="!isSidebarCollapsed">Dashboard</span></a
            >
          </li>
          <li>
            <a href="#" class="nav-item"
              ><i class="fas fa-shopping-cart fa-fw"></i
              ><span v-if="!isSidebarCollapsed">Orders</span></a
            >
          </li>
          <li>
            <a href="#" class="nav-item"
              ><i class="fas fa-users fa-fw"></i
              ><span v-if="!isSidebarCollapsed">Customers</span></a
            >
          </li>
          <li>
            <a href="#" class="nav-item"
              ><i class="fas fa-box-open fa-fw"></i
              ><span v-if="!isSidebarCollapsed">Products</span></a
            >
          </li>
          <li>
            <a href="#" class="nav-item"
              ><i class="fas fa-chart-line fa-fw"></i
              ><span v-if="!isSidebarCollapsed">Analytics</span></a
            >
          </li>
          <li>
            <a href="#" class="nav-item"
              ><i class="fas fa-cog fa-fw"></i
              ><span v-if="!isSidebarCollapsed">Settings</span></a
            >
          </li>
        </ul>
      </nav>
      <div class="sidebar-footer-panel">
        <a href="#" class="nav-item">
          <i class="fas fa-question-circle fa-fw"></i>
          <span v-if="!isSidebarCollapsed">Help</span>
        </a>
        <a href="#" class="nav-item">
          <i class="fas fa-sign-out-alt fa-fw"></i>
          <span v-if="!isSidebarCollapsed">Logout</span>
        </a>
      </div>
    </aside>

    <!-- Main Panel (Top Navbar + Content) -->
    <div class="main-panel-content">
      <!-- Top Navigation Bar -->
      <nav class="top-navbar-main">
        <div class="navbar-content-main">
          <div class="navbar-left-main">
            <div class="navbar-brand-section">
              <i class="fas fa-clipboard-check navbar-brand-icon"></i>
              <span class="navbar-brand-text">Order Management Dashboard</span>
            </div>
          </div>
          <div class="navbar-right-main">
            <!-- <button class="notification-btn" aria-label="Notifications">
              <i class="fas fa-bell"></i>
              <span class="notification-badge">3</span>
            </button> -->
            <!-- <div class="user-profile-section">
              <img src="" alt="Admin User" class="user-avatar" />
              <div class="user-details">
                <span class="user-name">Admin User</span>
                <span class="user-role">Administrator</span>
              </div>
              <i class="fas fa-chevron-down dropdown-arrow"></i>
            </div> -->
          </div>
        </div>
      </nav>

      <!-- Main Content Area -->
      <main class="content-area-wrapper">
        <!-- Stats Dashboard -->
        <section class="stats-dashboard-section">
          <div class="stats-grid-container">
            <div class="stat-card total-orders-card">
              <div class="stat-card-inner">
                <div class="stat-icon-container">
                  <i class="fas fa-shopping-cart"></i>
                </div>
                <div class="stat-content-container">
                  <div class="stat-number">{{ totalOrders }}</div>
                  <div class="stat-label">Total Orders</div>
                  <div class="stat-trend positive">
                    <i class="fas fa-arrow-up"></i>
                    <span>+12% from last month</span>
                  </div>
                </div>
              </div>
            </div>

            <div class="stat-card pending-orders-card">
              <div class="stat-card-inner">
                <div class="stat-icon-container">
                  <i class="fas fa-clock"></i>
                </div>
                <div class="stat-content-container">
                  <div class="stat-number">{{ pendingOrders }}</div>
                  <div class="stat-label">Pending Approval</div>
                  <div class="stat-trend warning">
                    <i class="fas fa-exclamation-triangle"></i>
                    <span>Requires attention</span>
                  </div>
                </div>
              </div>
            </div>

            <div class="stat-card completed-orders-card">
              <div class="stat-card-inner">
                <div class="stat-icon-container">
                  <i class="fas fa-check-circle"></i>
                </div>
                <div class="stat-content-container">
                  <div class="stat-number">{{ completedOrders }}</div>
                  <div class="stat-label">Completed Today</div>
                  <div class="stat-trend positive">
                    <i class="fas fa-arrow-up"></i>
                    <span>+8% from yesterday</span>
                  </div>
                </div>
              </div>
            </div>

            <div class="stat-card revenue-card">
              <div class="stat-card-inner">
                <div class="stat-icon-container">
                  <i class="fas fa-dollar-sign"></i>
                </div>
                <div class="stat-content-container">
                  <div class="stat-number">${{ totalRevenue }}</div>
                  <div class="stat-label">Total Revenue</div>
                  <div class="stat-trend positive">
                    <i class="fas fa-arrow-up"></i>
                    <span>+15% from last week</span>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </section>

        <!-- Filters and Controls -->
        <section class="controls-section-main">
          <div class="filters-panel-container">
            <div class="panel-header-section">
              <h3 class="panel-title">
                <i class="fas fa-filter"></i>
                Filters & Search
              </h3>
              <button class="clear-filters-btn" @click="clearFilters">
                <i class="fas fa-times"></i>
                Clear All
              </button>
            </div>

            <div class="filters-grid-layout">
              <div class="filter-group-container">
                <label class="filter-label-text">Status</label>
                <select
                  v-model="filters.status"
                  class="filter-select-input"
                  @change="loadOrders(1)"
                >
                  <option value="">All Statuses</option>
                  <option
                    v-for="status in availableStatuses"
                    :key="status.key"
                    :value="status.key"
                  >
                    {{ status.label }}
                  </option>
                </select>
              </div>

              <div class="filter-group-container">
                <label class="filter-label-text">Payment Method</label>
                <select
                  v-model="filters.paymentMethod"
                  class="filter-select-input"
                  @change="loadOrders(1)"
                >
                  <option value="">All Methods</option>
                  <option value="Cash">Cash</option>
                  <option value="Invoice">Invoice</option>
                </select>
              </div>

              <div class="filter-group-container">
                <label class="filter-label-text">Search</label>
                <div class="search-input-container">
                  <i class="fas fa-search search-input-icon"></i>
                  <input
                    v-model="filters.search"
                    type="text"
                    class="search-input-field"
                    placeholder="Search customer..."
                    @input="debouncedSearch"
                    @keyup.enter="loadOrders(1)"
                  />
                </div>
              </div>

              <div class="filter-group-container">
                <label class="filter-label-text">Date Range</label>
                <select class="filter-select-input">
                  <option value="">All Time</option>
                  <option value="today">Today</option>
                  <option value="week">This Week</option>
                  <option value="month">This Month</option>
                </select>
              </div>
            </div>
          </div>

          <div class="actions-panel-container">
            <div class="panel-header-section">
              <h3 class="panel-title">
                <i class="fas fa-cogs"></i>
                Quick Actions
              </h3>
            </div>

            <div class="actions-grid-layout">
              <button
                class="action-button refresh-action"
                @click="loadOrders(currentPage)"
              >
                <i
                  class="fas fa-sync-alt"
                  :class="{ 'fa-spin': isLoading }"
                ></i>
                <span>Refresh Data</span>
              </button>

              <button class="action-button export-action">
                <i class="fas fa-download"></i>
                <span>Export Orders</span>
              </button>

              <button class="action-button bulk-action">
                <i class="fas fa-tasks"></i>
                <span>Bulk Actions</span>
              </button>

              <button class="action-button analytics-action">
                <i class="fas fa-chart-bar"></i>
                <span>View Analytics</span>
              </button>
            </div>
          </div>
        </section>

        <!-- Orders Table Section -->
        <section class="orders-table-section">
          <div class="section-header-container">
            <div class="header-left-section">
              <h2 class="section-main-title">
                <i class="fas fa-list-ul"></i>
                Orders Management
              </h2>
              <div class="results-info-text">
                <span v-if="!isLoading">
                  Showing {{ paginationInfo.start }}-{{ paginationInfo.end }} of
                  {{ totalItems }} orders
                </span>
              </div>
            </div>

            <div class="header-right-section">
              <div class="view-toggle-options">
                <button class="view-toggle-btn active">
                  <i class="fas fa-table"></i>
                  Table View
                </button>
                <button class="view-toggle-btn">
                  <i class="fas fa-th-large"></i>
                  Card View
                </button>
              </div>

              <div class="sort-dropdown-container">
                <select class="sort-dropdown-select">
                  <option value="date-desc">Newest First</option>
                  <option value="date-asc">Oldest First</option>
                  <option value="amount-desc">Highest Amount</option>
                  <option value="amount-asc">Lowest Amount</option>
                </select>
              </div>
            </div>
          </div>

          <!-- Loading State -->
          <div v-if="isLoading" class="loading-state-container">
            <div class="loading-content-wrapper">
              <div class="loading-spinner-container">
                <div class="spinner-ring-animation"></div>
              </div>
              <h3 class="loading-title">Loading Orders...</h3>
              <p class="loading-description">
                Please wait while we fetch the latest order data
              </p>
            </div>
          </div>

          <!-- Orders Table -->
          <div v-else-if="orders.length > 0" class="table-wrapper-container">
            <div class="table-scroll-container">
              <table class="orders-data-table">
                <thead class="table-header-section">
                  <tr>
                    <th class="checkbox-column-header">
                      <input type="checkbox" class="header-checkbox" />
                    </th>
                    <th class="sortable-column-header">
                      Order ID
                      <i class="fas fa-sort sort-indicator"></i>
                    </th>
                    <th class="sortable-column-header">
                      Customer
                      <i class="fas fa-sort sort-indicator"></i>
                    </th>
                    <th class="standard-column-header">Payment Method</th>
                    <th class="standard-column-header">Status</th>
                    <th class="sortable-column-header">
                      Amount
                      <i class="fas fa-sort sort-indicator"></i>
                    </th>
                    <th class="sortable-column-header">
                      Date Created
                      <i class="fas fa-sort sort-indicator"></i>
                    </th>
                    <th class="actions-column-header">Actions</th>
                  </tr>
                </thead>
                <tbody class="table-body-section">
                  <tr
                    v-for="order in orders"
                    :key="order.id"
                    class="table-data-row"
                  >
                    <td class="checkbox-column-cell">
                      <input type="checkbox" class="row-checkbox" />
                    </td>
                    <td class="order-id-column-cell">
                      <div class="order-id-display">
                        <strong class="order-number-text"
                          >#{{ formatOrderId(order.id) }}</strong
                        >
                        <span class="order-full-id-text">{{ order.id }}</span>
                      </div>
                    </td>
                    <td class="customer-column-cell">
                      <div class="customer-info-display">
                        <div class="customer-avatar-container">
                          <i class="fas fa-user"></i>
                        </div>
                        <div class="customer-details-container">
                          <div class="customer-name-text">
                            {{ order.userFullName || "N/A" }}
                          </div>
                          <div class="customer-meta-text">
                            Customer ID: {{ order.userId || "N/A" }}
                          </div>
                        </div>
                      </div>
                    </td>
                    <td class="payment-column-cell">
                      <div class="payment-method-container">
                        <span
                          :class="`payment-status-badge payment-${order.paymentMethod.toLowerCase()}`"
                        >
                          <i
                            :class="`fas ${
                              order.paymentMethod === 'Cash'
                                ? 'fa-money-bill-wave'
                                : 'fa-file-invoice'
                            }`"
                          ></i>
                          {{ order.paymentMethod }}
                        </span>
                      </div>
                    </td>
                    <td class="status-column-cell">
                      <span
                        :class="`order-status-badge status-${order.status
                          .toLowerCase()
                          .replace(/\s+/g, '')}`"
                      >
                        <i :class="getStatusIcon(order.status)"></i>
                        {{ order.statusDisplay || order.status }}
                      </span>
                    </td>
                    <td class="amount-column-cell">
                      <div class="amount-display-container">
                        <span class="currency-symbol">RM</span>
                        <span class="amount-value">{{
                          formatCurrency(order.totalAmount)
                        }}</span>
                      </div>
                    </td>
                    <td class="date-column-cell">
                      <div class="date-display-container">
                        <div class="date-primary-text">
                          {{ formatDate(order.createdAt) }}
                        </div>
                        <div class="date-secondary-text">
                          {{ formatTime(order.createdAt) }}
                        </div>
                      </div>
                    </td>
                    <td class="actions-column-cell">
                      <div class="action-buttons-container">
                        <button
                          class="action-button-small view-action"
                          @click="viewOrderDetails(order)"
                          title="View Details"
                        >
                          <i class="fas fa-eye"></i>
                        </button>

                        <button
                          class="action-button-small manage-action"
                          @click="openStatusModal(order)"
                          title="Manage Status"
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
                              @click.prevent="openStatusModal(order)"
                            >
                              <i class="fas fa-edit"></i>
                              Change Status
                            </a>
                            <a href="#" class="dropdown-menu-item">
                              <i class="fas fa-print"></i>
                              Print Invoice
                            </a>
                            <a href="#" class="dropdown-menu-item danger-item">
                              <i class="fas fa-trash"></i>
                              Delete Order
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

          <!-- Empty State -->
          <div v-else class="empty-state-container">
            <div class="empty-state-content">
              <div class="empty-state-icon">
                <i class="fas fa-inbox"></i>
              </div>
              <h3 class="empty-state-title">No Orders Found</h3>
              <p class="empty-state-description">
                No orders match your current search criteria. Try adjusting your
                filters or search terms.
              </p>
              <button class="empty-state-action-btn" @click="clearFilters">
                <i class="fas fa-refresh"></i>
                Clear Filters
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
                  @change="loadOrders(1)"
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
                class="pagination-control-btn first-page-btn"
                @click="loadOrders(1)"
                :disabled="currentPage <= 1"
                title="First Page"
              >
                <i class="fas fa-angle-double-left"></i>
              </button>

              <button
                class="pagination-control-btn prev-page-btn"
                @click="loadOrders(currentPage - 1)"
                :disabled="currentPage <= 1"
                title="Previous Page"
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
                  @click="loadOrders(page)"
                >
                  {{ page }}
                </button>
              </div>

              <button
                class="pagination-control-btn next-page-btn"
                @click="loadOrders(currentPage + 1)"
                :disabled="currentPage >= totalPages"
                title="Next Page"
              >
                Next
                <i class="fas fa-angle-right"></i>
              </button>

              <button
                class="pagination-control-btn last-page-btn"
                @click="loadOrders(totalPages)"
                :disabled="currentPage >= totalPages"
                title="Last Page"
              >
                <i class="fas fa-angle-double-right"></i>
              </button>
            </div>

            <div class="pagination-summary-section">
              <span class="pagination-summary-text"
                >Page {{ currentPage }} of {{ totalPages }}</span
              >
            </div>
          </div>
        </section>
      </main>
    </div>

    <!-- Order Details Modal -->
    <div
      v-if="showModal"
      class="modal-overlay-backdrop"
      @click.self="closeModal"
    >
      <div class="modal-container-wrapper">
        <div class="modal-header-section">
          <h3 class="modal-title-text">
            <i class="fas fa-receipt"></i>
            Order Details - #{{
              selectedOrder ? formatOrderId(selectedOrder.id) : ""
            }}
          </h3>
          <button class="modal-close-button" @click="closeModal">
            <i class="fas fa-times"></i>
          </button>
        </div>
        <div class="modal-body-section">
          <div v-if="selectedOrder" class="order-details-container">
            <div class="details-grid-container">
              <div class="details-section-container">
                <h6 class="details-section-title">Order Information</h6>
                <div class="details-table-container">
                  <div class="detail-row-container">
                    <span class="detail-label-text">Order ID:</span>
                    <span class="detail-value-text"
                      >#{{ formatOrderId(selectedOrder.id) }}</span
                    >
                  </div>
                  <div class="detail-row-container">
                    <span class="detail-label-text">Status:</span>
                    <span class="detail-value-text">
                      <span
                        :class="`order-status-badge status-${selectedOrder.status
                          .toLowerCase()
                          .replace(/\s+/g, '')}`"
                      >
                        {{
                          selectedOrder.statusDisplay || selectedOrder.status
                        }}
                      </span>
                    </span>
                  </div>
                  <div class="detail-row-container">
                    <span class="detail-label-text">Payment Method:</span>
                    <span class="detail-value-text">
                      <span
                        :class="`payment-status-badge payment-${selectedOrder.paymentMethod.toLowerCase()}`"
                      >
                        {{ selectedOrder.paymentMethod }}
                      </span>
                    </span>
                  </div>
                  <div class="detail-row-container">
                    <span class="detail-label-text">Total Amount:</span>
                    <span class="detail-value-text amount-highlight"
                      >${{ formatCurrency(selectedOrder.totalAmount) }}</span
                    >
                  </div>
                  <div class="detail-row-container">
                    <span class="detail-label-text">Created:</span>
                    <span class="detail-value-text">{{
                      formatDate(selectedOrder.createdAt, true)
                    }}</span>
                  </div>
                  <div
                    v-if="selectedOrder.statusChangedAt"
                    class="detail-row-container"
                  >
                    <span class="detail-label-text">Status Changed:</span>
                    <span class="detail-value-text">{{
                      formatDate(selectedOrder.statusChangedAt, true)
                    }}</span>
                  </div>
                  <div
                    v-if="selectedOrder.statusChangedBy"
                    class="detail-row-container"
                  >
                    <span class="detail-label-text">Changed By:</span>
                    <span class="detail-value-text">{{
                      selectedOrder.statusChangedBy
                    }}</span>
                  </div>
                </div>
              </div>

              <div class="details-section-container">
                <h6 class="details-section-title">Customer Information</h6>
                <div class="details-table-container">
                  <div class="detail-row-container">
                    <span class="detail-label-text">Name:</span>
                    <span class="detail-value-text">{{
                      selectedOrder.userFullName || "N/A"
                    }}</span>
                  </div>
                  <div class="detail-row-container">
                    <span class="detail-label-text">Customer ID:</span>
                    <span class="detail-value-text">
                      {{
                        selectedOrder.userId
                          ? formatOrderId(selectedOrder.userId)
                          : "N/A"
                      }}
                    </span>
                  </div>
                </div>
              </div>
            </div>

            <div v-if="selectedOrder.notes" class="notes-section-container">
              <h6 class="details-section-title">Notes</h6>
              <p class="notes-text">{{ selectedOrder.notes }}</p>
            </div>

            <div class="modal-actions-container">
              <button
                class="modal-action-button manage-status-action"
                @click="
                  openStatusModal(selectedOrder);
                  closeModal();
                "
              >
                <i class="fas fa-edit"></i>
                Manage Status
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- Status Management Modal -->
    <div
      v-if="showStatusModal"
      class="modal-overlay-backdrop"
      @click.self="closeStatusModal"
    >
      <div class="modal-container-wrapper status-modal">
        <div class="modal-header-section">
          <h3 class="modal-title-text">
            <i class="fas fa-edit"></i>
            Manage Order Status - #{{
              statusOrder ? formatOrderId(statusOrder.id) : ""
            }}
          </h3>
          <button class="modal-close-button" @click="closeStatusModal">
            <i class="fas fa-times"></i>
          </button>
        </div>
        <div class="modal-body-section">
          <div v-if="statusOrder" class="status-management-container">
            <div class="current-status-section">
              <h6 class="section-subtitle">Current Status</h6>
              <div class="current-status-display">
                <span
                  :class="`order-status-badge status-${statusOrder.status
                    .toLowerCase()
                    .replace(/\s+/g, '')}`"
                >
                  <i :class="getStatusIcon(statusOrder.status)"></i>
                  {{ statusOrder.statusDisplay || statusOrder.status }}
                </span>
                <div class="status-meta">
                  <div
                    v-if="statusOrder.statusChangedAt"
                    class="status-changed-date"
                  >
                    Changed: {{ formatDate(statusOrder.statusChangedAt, true) }}
                  </div>
                  <div
                    v-if="statusOrder.statusChangedBy"
                    class="status-changed-by"
                  >
                    By: {{ statusOrder.statusChangedBy }}
                  </div>
                </div>
              </div>

              <div class="new-status-section">
                <h6 class="section-subtitle">Change Status To</h6>
                <div class="status-options-grid">
                  <button
                    v-for="status in getAvailableStatuses(
                      statusOrder.status
                        .replace(/\s+/g, '')
                        .replace(/^\w/, (c) => c.toUpperCase())
                    )"
                    :key="status.key"
                    :class="[
                      'status-option-btn',
                      `status-${status.key.toLowerCase().replace(/\s+/g, '')}`,
                    ]"
                    @click="selectedNewStatus = status"
                    :disabled="isUpdatingStatus"
                  >
                    <i :class="getStatusIcon(status.key)"></i>
                    <span>{{ status.label }}</span>
                  </button>
                </div>

                <div v-if="selectedNewStatus" class="selected-status-preview">
                  <h6 class="section-subtitle">New Status Preview</h6>
                  <span
                    :class="`order-status-badge status-${selectedNewStatus.key
                      .toLowerCase()
                      .replace(/\s+/g, '')}`"
                  >
                    <i :class="getStatusIcon(selectedNewStatus.key)"></i>
                    {{ selectedNewStatus.label }}
                  </span>
                </div>
              </div>

              <div class="notes-input-section">
                <h6 class="section-subtitle">Notes (Optional)</h6>
                <textarea
                  v-model="statusNotes"
                  class="status-notes-input"
                  placeholder="Add notes about this status change..."
                  rows="3"
                  :disabled="isUpdatingStatus"
                ></textarea>
              </div>

              <div class="status-actions-container">
                <button
                  class="status-action-btn cancel-btn"
                  @click="closeStatusModal"
                  :disabled="isUpdatingStatus"
                >
                  <i class="fas fa-times"></i>
                  Cancel
                </button>
                <button
                  class="status-action-btn update-btn"
                  @click="updateOrderStatus"
                  :disabled="!selectedNewStatus || isUpdatingStatus"
                >
                  <i class="fas fa-spinner fa-spin" v-if="isUpdatingStatus"></i>
                  <i class="fas fa-check" v-else></i>
                  {{ isUpdatingStatus ? "Updating..." : "Update Status" }}
                </button>
              </div>
            </div>
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
  </div>
</template>

<script>
import { ref, computed, onMounted } from "vue";
import axios from "axios";

export default {
  name: "CheckPage",
  setup() {
    const orders = ref([]);
    const selectedOrder = ref(null);
    const statusOrder = ref(null);
    const isLoading = ref(false);
    const currentPage = ref(1);
    const totalPages = ref(1);
    const totalItems = ref(0);
    const pageSize = ref(10);
    const showModal = ref(false);
    const showStatusModal = ref(false);
    const messages = ref([]);
    const pendingOrders = ref("-");
    const totalOrders = ref("-");
    const completedOrders = ref("24");
    const totalRevenue = ref("12,450");
    const availableStatuses = ref([]);
    const selectedNewStatus = ref("");
    const statusNotes = ref("");
    const isUpdatingStatus = ref(false);

    const filters = ref({
      status: "",
      paymentMethod: "",
      search: "",
    });

    const isSidebarCollapsed = ref(false);

    const toggleSidebar = () => {
      isSidebarCollapsed.value = !isSidebarCollapsed.value;
    };

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

    const API_URL = "https://asoms-production.up.railway.app/api/orders";

    const loadOrderStatuses = async () => {
      try {
        const response = await axios.get(`${API_URL}/statuses`);
        availableStatuses.value = response.data;
      } catch (error) {
        console.error("Error loading order statuses:", error);
      }
    };

    const loadOrders = async (page = 1) => {
      isLoading.value = true;
      currentPage.value = page;
      try {
        const params = {
          page: currentPage.value,
          pageSize: pageSize.value,
          status: filters.value.status || undefined,
          paymentMethod: filters.value.paymentMethod || undefined,
          search: filters.value.search || undefined,
        };

        console.log("Params: ", params);

        const response = await axios.get(API_URL, { params });

        orders.value = response.data.data || [];
        totalPages.value = response.data.totalPages || 1;
        totalItems.value = response.data.totalItems || 0;
        totalOrders.value = response.data.totalItems || 0;
        pendingOrders.value =
          orders.value.filter((o) => o.status === "Pending Approval").length ||
          0;
        completedOrders.value =
          orders.value.filter((o) => o.status === "Completed").length || 0;
        totalRevenue.value = orders.value
          .reduce(
            (sum, order) =>
              sum +
              (order.status === "Completed"
                ? parseFloat(order.totalAmount)
                : 0),
            0
          )
          .toFixed(2);
      } catch (error) {
        console.error(
          "Error loading orders:",
          error.response?.data || error.message
        );
        showMessage(
          `Error loading orders: ${error.message}. Check console & API.`,
          "error"
        );
        orders.value = [];
        totalItems.value = 0;
      } finally {
        isLoading.value = false;
      }
    };

    const viewOrderDetails = (order) => {
      selectedOrder.value = order;
      showModal.value = true;
    };

    const closeModal = () => {
      showModal.value = false;
      setTimeout(() => {
        selectedOrder.value = null;
      }, 300);
    };

    const openStatusModal = (order) => {
      statusOrder.value = order;
      selectedNewStatus.value = "";
      statusNotes.value = "";
      showStatusModal.value = true;
    };

    const closeStatusModal = () => {
      showStatusModal.value = false;
      setTimeout(() => {
        statusOrder.value = null;
        selectedNewStatus.value = "";
        statusNotes.value = "";
      }, 300);
    };

    const getAvailableStatuses = (currentStatusKey) => {
      // Make sure we have statuses loaded
      if (!availableStatuses.value || !Array.isArray(availableStatuses.value)) {
        return [];
      }

      // 🔧 FIXED: Updated status transitions to match exact API response format
      const statusTransitions = {
        PendingApproval: ["Confirmed", "Rejected"],
        Confirmed: ["Completed", "Rejected"],
        Rejected: ["PendingApproval"],
        Completed: [],
      };

      const availableTransitions = statusTransitions[currentStatusKey] || [];

      // 🔧 FIXED: Filter using the correct property names from API
      const filtered = availableStatuses.value.filter((status) => {
        return availableTransitions.includes(status.key);
      });

      return filtered;
    };

    const updateOrderStatus = async () => {
      if (!selectedNewStatus.value || !statusOrder.value) return;
      console.log("New Selected: ", selectedNewStatus.value);
      isUpdatingStatus.value = true;
      try {
        const response = await axios.put(
          `${API_URL}/${statusOrder.value.id}/status`,
          {
            status: selectedNewStatus.value.key,
            notesToBuyer: statusNotes.value, // ← UPDATE THIS LINE
          }
        );

        if (response.data.success) {
          showMessage(response.data.message, "success");

          // Update the order in the list
          const orderIndex = orders.value.findIndex(
            (o) => o.id === statusOrder.value.id
          );
          if (orderIndex !== -1) {
            orders.value[orderIndex].status = selectedNewStatus.value.key;
            orders.value[orderIndex].statusDisplay =
              selectedNewStatus.value.display || selectedNewStatus.value.label;
            orders.value[orderIndex].statusChangedAt = new Date().toISOString();
            orders.value[orderIndex].statusChangedBy = "Admin User";
            if (statusNotes.value) {
              orders.value[orderIndex].notesToBuyer = statusNotes.value;
            }
          }

          // Update stats
          pendingOrders.value =
            orders.value.filter((o) => o.status === "PendingApproval").length ||
            0;
          completedOrders.value =
            orders.value.filter((o) => o.status === "Completed").length || 0;

          closeStatusModal();
        } else {
          showMessage(
            response.data.message || "Failed to update order status.",
            "error"
          );
        }
      } catch (error) {
        console.error("Error updating order status:", error);
        showMessage("Error updating order status. Please try again.", "error");
      } finally {
        isUpdatingStatus.value = false;
      }
    };

    const clearFilters = () => {
      filters.value = { status: "", paymentMethod: "", search: "" };
      loadOrders(1);
    };

    const showMessage = (text, type = "info") => {
      const id = Date.now();
      messages.value.push({ id, text, type });
      setTimeout(() => {
        messages.value = messages.value.filter((m) => m.id !== id);
      }, 5000);
    };

    const getStatusIcon = (status) => {
      const icons = {
        Confirmed: "fas fa-check-circle",
        PendingApproval: "fas fa-clock",
        Rejected: "fas fa-times-circle",
        Completed: "fas fa-flag-checkered",
      };
      return icons[status] || "fas fa-circle";
    };

    const formatTime = (dateString) => {
      if (!dateString) return "";
      return new Date(dateString).toLocaleTimeString("en-US", {
        hour: "2-digit",
        minute: "2-digit",
      });
    };

    const removeMessage = (index) => {
      messages.value.splice(index, 1);
    };

    const formatOrderId = (id) => {
      return id ? String(id).substring(0, 8).toUpperCase() : "N/A";
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

    let searchTimeout = null;
    const debouncedSearch = () => {
      clearTimeout(searchTimeout);
      searchTimeout = setTimeout(() => {
        loadOrders(1);
      }, 500);
    };

    onMounted(() => {
      loadOrderStatuses();
      loadOrders(1);
    });

    console.log("Available Statuses: ", getAvailableStatuses());

    return {
      orders,
      selectedOrder,
      statusOrder,
      isLoading,
      currentPage,
      totalPages,
      totalItems,
      pageSize,
      showModal,
      showStatusModal,
      messages,
      filters,
      pendingOrders,
      totalOrders,
      completedOrders,
      totalRevenue,
      paginationInfo,
      displayedPages,
      availableStatuses,
      selectedNewStatus,
      statusNotes,
      isUpdatingStatus,
      loadOrders,
      viewOrderDetails,
      closeModal,
      openStatusModal,
      closeStatusModal,
      getAvailableStatuses,
      updateOrderStatus,
      clearFilters,
      formatOrderId,
      formatCurrency,
      formatDate,
      debouncedSearch,
      getStatusIcon,
      formatTime,
      removeMessage,
      isSidebarCollapsed,
      toggleSidebar,
    };
  },
};
</script>

<style scoped>
/* Previous styles remain the same... */

/* Status Management Modal Styles */
.status-modal {
  max-width: 600px;
}

.status-management-container {
  display: flex;
  flex-direction: column;
  gap: 1.5rem;
}

.current-status-section,
.new-status-section,
.notes-input-section {
  background: #f8f9fa;
  border-radius: 0.75rem;
  padding: 1.25rem;
}

.section-subtitle {
  font-size: 1rem;
  font-weight: 600;
  color: #2c3e50;
  margin-bottom: 1rem;
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.current-status-display {
  display: flex;
  flex-direction: column;
  gap: 0.75rem;
}

.status-meta {
  display: flex;
  flex-direction: column;
  gap: 0.25rem;
  font-size: 0.85rem;
  color: #7f8c8d;
}

.status-options-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(150px, 1fr));
  gap: 1rem;
  margin-bottom: 1rem;
}

.status-option-btn {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 0.5rem;
  padding: 1rem;
  border: 2px solid #e9ecef;
  border-radius: 0.75rem;
  background: white;
  cursor: pointer;
  transition: all 0.3s ease;
  font-weight: 600;
}

.status-option-btn:hover:not(:disabled) {
  transform: translateY(-2px);
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
}

.status-option-btn:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}

.status-option-btn.status-confirmed {
  border-color: #2ecc71;
  color: white;
}

.status-option-btn.status-confirmed:hover:not(:disabled) {
  background: #2ecc71;
  color: white;
}

.status-option-btn.status-rejected {
  border-color: #e74c3c;
  color: white;
}

.status-option-btn.status-rejected:hover:not(:disabled) {
  background: #e74c3c;
  color: white;
}

.status-option-btn.status-completed {
  border-color: #9b59b6;
  color: white;
}

.status-option-btn.status-completed:hover:not(:disabled) {
  background: #9b59b6;
  color: white;
}

.status-option-btn.status-pendingapproval {
  border-color: #f39c12;
  color: white;
}

.status-option-btn.status-pendingapproval:hover:not(:disabled) {
  background: #f39c12;
  color: white;
}

.selected-status-preview {
  padding: 1rem;
  background: white;
  border-radius: 0.5rem;
  border: 2px solid #667eea;
}

.status-notes-input {
  width: 100%;
  padding: 0.75rem;
  border: 2px solid #e9ecef;
  border-radius: 0.5rem;
  font-size: 0.95rem;
  resize: vertical;
  transition: border-color 0.3s ease;
}

.status-notes-input:focus {
  outline: none;
  border-color: #667eea;
  box-shadow: 0 0 0 3px rgba(102, 126, 234, 0.1);
}

.status-actions-container {
  display: flex;
  justify-content: flex-end;
  gap: 1rem;
  padding-top: 1.5rem;
  border-top: 2px solid #f1f3f4;
}

.status-action-btn {
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

.cancel-btn:hover:not(:disabled) {
  background: #7f8c8d;
  transform: translateY(-2px);
}

.update-btn {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
}

.update-btn:hover:not(:disabled) {
  transform: translateY(-2px);
  box-shadow: 0 4px 12px rgba(102, 126, 234, 0.4);
}

.update-btn:disabled {
  opacity: 0.6;
  cursor: not-allowed;
  transform: none;
}

.manage-action {
  background: linear-gradient(135deg, #f39c12 0%, #e67e22 100%);
  color: white;
}

.manage-status-action {
  background: linear-gradient(135deg, #f39c12 0%, #e67e22 100%);
  color: white;
}

.notes-section-container {
  background: #f8f9fa;
  border-radius: 0.75rem;
  padding: 1.25rem;
  margin-top: 1.5rem;
}

.notes-text {
  color: #2c3e50;
  line-height: 1.6;
  margin: 0;
}

/* Additional responsive styles for status modal */
@media (max-width: 768px) {
  .status-options-grid {
    grid-template-columns: 1fr;
  }

  .status-actions-container {
    flex-direction: column;
  }

  .status-modal {
    max-width: 95%;
  }
}

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

.sidebar-logo-text {
  font-weight: 700;
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

.sidebar-footer-panel {
  padding: 0.9375rem 1.25rem;
  border-top: 1px solid #4a627a;
}

.sidebar-footer-panel .nav-item {
  padding: 0.625rem 0;
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

.navbar-brand-text {
  font-weight: 600;
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

.dropdown-arrow {
  color: #7f8c8d;
  font-size: 0.8rem;
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

/* Stats Dashboard Section - Dynamic Grid */
.stats-dashboard-section {
  margin-bottom: 0.3125rem;
}

.stats-grid-container {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(15rem, 1fr));
  gap: 1.5625rem;
  width: 100%;
}

/* Ensure 4 columns on larger screens */
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

.total-orders-card::before {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
}

.pending-orders-card::before {
  background: linear-gradient(135deg, #f39c12 0%, #e67e22 100%);
}

.completed-orders-card::before {
  background: linear-gradient(135deg, #2ecc71 0%, #27ae60 100%);
}

.revenue-card::before {
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

.total-orders-card .stat-icon-container {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
}

.pending-orders-card .stat-icon-container {
  background: linear-gradient(135deg, #f39c12 0%, #e67e22 100%);
}

.completed-orders-card .stat-icon-container {
  background: linear-gradient(135deg, #2ecc71 0%, #27ae60 100%);
}

.revenue-card .stat-icon-container {
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

.stat-trend i {
  font-size: 0.8rem;
}

/* Controls Section - Dynamic Layout */
.controls-section-main {
  display: grid;
  grid-template-columns: 2fr 1fr;
  gap: 1.875rem;
  width: 100%;
}

@media (max-width: 1024px) {
  .controls-section-main {
    grid-template-columns: 1fr;
  }
}

.filters-panel-container,
.actions-panel-container {
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

/* Filters Grid - Dynamic Layout */
.filters-grid-layout {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(12rem, 1fr));
  gap: 1.25rem;
  align-items: end;
}

@media (min-width: 768px) {
  .filters-grid-layout {
    grid-template-columns: repeat(4, 1fr);
  }
}

.filter-group-container {
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
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

/* Actions Grid - Dynamic Layout */
.actions-grid-layout {
  display: grid;
  grid-template-columns: repeat(2, 1fr);
  gap: 0.9375rem;
}

.action-button {
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

.action-button:hover {
  transform: translateY(-0.1875rem);
  box-shadow: 0 0.5rem 1.5625rem rgba(0, 0, 0, 0.2);
}

.refresh-action {
  background: linear-gradient(135deg, #2ecc71 0%, #27ae60 100%);
}

.export-action {
  background: linear-gradient(135deg, #3498db 0%, #2980b9 100%);
}

.bulk-action {
  background: linear-gradient(135deg, #f39c12 0%, #e67e22 100%);
}

.analytics-action {
  background: linear-gradient(135deg, #9b59b6 0%, #8e44ad 100%);
}

.action-button i {
  font-size: 1.2rem;
}

.action-button span {
  font-size: 0.9rem;
}

/* Orders Table Section */
.orders-table-section {
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

.sort-dropdown-container {
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

/* Table Section */
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

.orders-data-table {
  width: 100%;
  border-collapse: collapse;
  background: white;
}

.table-header-section {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
}

.orders-data-table th {
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

.orders-data-table td {
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

.order-id-display {
  display: flex;
  flex-direction: column;
  gap: 0.25rem;
}

.order-number-text {
  font-family: "Courier New", monospace;
  font-weight: 700;
  color: #2c3e50;
  font-size: 1rem;
}

.order-full-id-text {
  font-size: 0.8rem;
  color: #7f8c8d;
  font-family: "Courier New", monospace;
}

.customer-info-display {
  display: flex;
  align-items: center;
  gap: 0.75rem;
}

.customer-avatar-container {
  width: 2.5rem;
  height: 2.5rem;
  border-radius: 50%;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  display: flex;
  align-items: center;
  justify-content: center;
  color: white;
  font-size: 1.1rem;
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

.customer-meta-text {
  font-size: 0.8rem;
  color: #7f8c8d;
}

.payment-status-badge,
.order-status-badge {
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

.status-confirmed {
  background: linear-gradient(135deg, #2ecc71 0%, #27ae60 100%);
  color: white;
}

.status-pendingapproval {
  background: linear-gradient(135deg, #f39c12 0%, #e67e22 100%);
  color: white;
}

.status-rejected {
  background: linear-gradient(135deg, #e74c3c 0%, #c0392b 100%);
  color: white;
}

.status-completed {
  background: linear-gradient(135deg, #9b59b6 0%, #8e44ad 100%);
  color: white;
}

.payment-cash {
  background: linear-gradient(135deg, #2ecc71 0%, #27ae60 100%);
  color: white;
}

.payment-invoice {
  background: linear-gradient(135deg, #f39c12 0%, #e67e22 100%);
  color: white;
}

.amount-display-container {
  display: flex;
  align-items: baseline;
  gap: 0.125rem;
  font-weight: 600;
  color: #2c3e50;
}

.currency-symbol {
  font-size: 0.9rem;
  color: #7f8c8d;
}

.amount-value {
  font-size: 1.1rem;
}

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

.approve-action {
  background: linear-gradient(135deg, #2ecc71 0%, #27ae60 100%);
  color: white;
}

.reject-action {
  background: linear-gradient(135deg, #e74c3c 0%, #c0392b 100%);
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

.danger-item {
  color: #e74c3c;
}

.danger-item:hover {
  background: #ffe6e6;
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

.pagination-summary-section {
  color: #7f8c8d;
  font-size: 0.95rem;
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
}

.amount-highlight {
  font-size: 1.2rem;
  font-weight: 700;
  color: #27ae60;
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

.approve-modal-action {
  background: linear-gradient(135deg, #2ecc71 0%, #27ae60 100%);
  color: white;
}

.reject-modal-action {
  background: linear-gradient(135deg, #e74c3c 0%, #c0392b 100%);
  color: white;
}

.modal-action-button:hover {
  transform: translateY(-0.125rem);
  box-shadow: 0 0.5rem 1.5625rem rgba(0, 0, 0, 0.2);
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

  .navbar-brand-text {
    font-size: 1.2rem;
  }

  .content-area-wrapper {
    padding: 0.9375rem;
    gap: 1.25rem;
  }

  .stats-grid-container {
    grid-template-columns: 1fr;
  }

  .filters-grid-layout {
    grid-template-columns: 1fr;
  }

  .actions-grid-layout {
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
}

@media (max-width: 480px) {
  .orders-data-table {
    font-size: 0.85rem;
  }

  .orders-data-table th,
  .orders-data-table td {
    padding: 0.75rem 0.5rem;
  }
}
</style>
