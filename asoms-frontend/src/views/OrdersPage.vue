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
            <router-link to="/admin/orders" class="nav-item active"
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
              <i class="fas fa-shopping-cart navbar-brand-icon"></i>
              <span class="navbar-brand-text">Orders Management</span>
            </div>
          </div>
          <div class="navbar-right-main">
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
        <!-- Orders Stats -->
        <section class="stats-dashboard-section">
          <div class="stats-grid-container">
            <div class="stat-card total-orders-stat">
              <div class="stat-card-inner">
                <div class="stat-icon-container">
                  <i class="fas fa-shopping-cart"></i>
                </div>
                <div class="stat-content-container">
                  <div class="stat-number">
                    {{ orderStore.stats.totalOrders }}
                  </div>
                  <div class="stat-label">Total Orders</div>
                  <div class="stat-trend positive">
                    <i class="fas fa-arrow-up"></i>
                  </div>
                </div>
              </div>
            </div>

            <div class="stat-card pending-orders-stat">
              <div class="stat-card-inner">
                <div class="stat-icon-container">
                  <i class="fas fa-clock"></i>
                </div>
                <div class="stat-content-container">
                  <div class="stat-number">
                    {{ orderStore.stats.pendingOrders }}
                  </div>
                  <div class="stat-label">Pending Orders</div>
                  <div class="stat-trend warning">
                    <i class="fas fa-exclamation-triangle"></i>
                    <span>Needs attention in this Page</span>
                  </div>
                </div>
              </div>
            </div>

            <div class="stat-card revenue-stat">
              <div class="stat-card-inner">
                <div class="stat-icon-container">
                  <i class="fas fa-dollar-sign"></i>
                </div>
                <div class="stat-content-container">
                  <div class="stat-number">
                    ${{ formatCurrency(orderStore.stats.totalRevenue) }}
                  </div>
                  <div class="stat-label">Page Total Revenue</div>
                  <div class="stat-trend positive">
                    <i class="fas fa-arrow-up"></i>
                  </div>
                </div>
              </div>
            </div>

            <div class="stat-card avg-order-stat">
              <div class="stat-card-inner">
                <div class="stat-icon-container">
                  <i class="fas fa-chart-bar"></i>
                </div>
                <div class="stat-content-container">
                  <div class="stat-number">
                    ${{ formatCurrency(orderStore.stats.averageOrderValue) }}
                  </div>
                  <div class="stat-label">Page Avg Order Value</div>
                  <div class="stat-trend positive">
                    <i class="fas fa-arrow-up"></i>
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
                  Advanced Filters
                </h3>
                <button class="clear-filters-btn" @click="clearAllFilters">
                  <i class="fas fa-times"></i>
                  Clear All
                </button>
              </div>

              <div class="advanced-filters-grid">
                <div class="filter-group-container">
                  <label class="filter-label-text">Status</label>
                  <select
                    v-model="filters.status"
                    class="filter-select-input"
                    @change="loadOrders(1)"
                  >
                    <option value="">All Statuses</option>
                    <option value="PendingApproval">Pending Approval</option>
                    <option value="Confirmed">Confirmed</option>
                    <option value="Rejected">Rejected</option>
                    <option value="Completed">Completed</option>
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
                  <label class="filter-label-text">Date Range</label>
                  <select
                    v-model="filters.dateRange"
                    class="filter-select-input"
                    @change="loadOrders(1)"
                  >
                    <option value="">All Time</option>
                    <option value="today">Today</option>
                    <option value="week">This Week</option>
                    <option value="month">This Month</option>
                    <option value="quarter">This Quarter</option>
                  </select>
                </div>

                <div class="filter-group-container">
                  <label class="filter-label-text">Amount Range</label>
                  <div class="amount-range-container">
                    <input
                      v-model="filters.minAmount"
                      type="number"
                      placeholder="Min"
                      class="amount-input"
                      @change="loadOrders(1)"
                    />
                    <span class="range-separator">-</span>
                    <input
                      v-model="filters.maxAmount"
                      type="number"
                      placeholder="Max"
                      class="amount-input"
                      @change="loadOrders(1)"
                    />
                  </div>
                </div>

                <div class="filter-group-container search-group-advanced">
                  <label class="filter-label-text">Search Orders</label>
                  <div class="search-input-container">
                    <i class="fas fa-search search-input-icon"></i>
                    <input
                      v-model="filters.search"
                      type="text"
                      class="search-input-field"
                      placeholder="Customer name..."
                      @input="debouncedSearch"
                    />
                  </div>
                </div>
              </div>
            </div>

            <!-- Bulk Actions Panel -->
            <div class="bulk-actions-panel">
              <div class="panel-header-section">
                <h3 class="panel-title">
                  <i class="fas fa-tasks"></i>
                  Bulk Actions
                </h3>
                <span class="selected-count"
                  >{{ selectedOrders.length }} selected</span
                >
              </div>

              <div class="bulk-actions-grid">
                <button
                  class="bulk-action-btn print-invoices"
                  @click="printInvoices"
                  :disabled="selectedOrders.length === 0"
                >
                  <i class="fas fa-print"></i>
                  <span>Print Invoices</span>
                </button>
              </div>
            </div>
          </div>
        </section>

        <!-- Orders Table Section -->
        <section class="orders-table-section">
          <div class="section-header-container">
            <div class="header-left-section">
              <h2 class="section-main-title">
                <i class="fas fa-list-ul"></i>
                Orders List
              </h2>
              <div class="results-info-text">
                <span v-if="!orderStore.loading">
                  Showing {{ orderStore.paginationInfo.start }}-{{
                    orderStore.paginationInfo.end
                  }}
                  of {{ orderStore.totalItems }} orders
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
                    @change="loadOrders(1)"
                    class="sort-dropdown-select"
                  >
                    <option value="date-desc">Newest First</option>
                    <option value="date-asc">Oldest First</option>
                    <option value="amount-desc">Highest Amount</option>
                    <option value="amount-asc">Lowest Amount</option>
                    <option value="status">By Status</option>
                  </select>
                </div>
              </div>
            </div>
          </div>

          <!-- Loading State -->
          <div v-if="orderStore.loading" class="loading-state-container">
            <div class="loading-content-wrapper">
              <div class="loading-spinner-container">
                <div class="spinner-ring-animation"></div>
              </div>
              <h3 class="loading-title">Loading Orders...</h3>
              <p class="loading-description">Fetching the latest order data</p>
            </div>
          </div>

          <!-- Table View -->
          <div
            v-else-if="orderStore.orders.length > 0 && viewMode === 'table'"
            class="table-wrapper-container"
          >
            <div class="table-scroll-container">
              <table class="orders-data-table">
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
                      @click="sortColumn('orderNumber')"
                    >
                      Order ID
                      <i class="fas fa-sort sort-indicator"></i>
                    </th>
                    <th
                      class="sortable-column-header"
                      @click="sortColumn('customer')"
                    >
                      Customer
                      <i class="fas fa-sort sort-indicator"></i>
                    </th>
                    <th class="standard-column-header">Items</th>
                    <th class="standard-column-header">Payment</th>
                    <th class="standard-column-header">Status</th>
                    <th
                      class="sortable-column-header"
                      @click="sortColumn('amount')"
                    >
                      Amount
                      <i class="fas fa-sort sort-indicator"></i>
                    </th>
                    <th
                      class="sortable-column-header"
                      @click="sortColumn('date')"
                    >
                      Date
                      <i class="fas fa-sort sort-indicator"></i>
                    </th>
                    <th class="actions-column-header">Actions</th>
                  </tr>
                </thead>
                <tbody class="table-body-section">
                  <tr
                    v-for="order in orderStore.orders"
                    :key="order.id"
                    class="table-data-row"
                  >
                    <td class="checkbox-column-cell">
                      <input
                        type="checkbox"
                        v-model="selectedOrders"
                        :value="order.id"
                        class="row-checkbox"
                      />
                    </td>
                    <td class="order-id-column-cell">
                      <div class="order-id-display">
                        <strong class="order-number-text"
                          >#{{ orderStore.formatOrderId(order.id) }}</strong
                        >
                        <span
                          class="order-type-badge"
                          :class="order.paymentMethod.toLowerCase()"
                          >{{ order.paymentMethod }}</span
                        >
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
                            ID: {{ order.userId || "N/A" }}
                          </div>
                        </div>
                      </div>
                    </td>
                    <td class="items-column-cell">
                      <div class="items-summary">
                        <span class="items-count">
                          {{ totalQuantity(order.items) }} items
                        </span>
                        <div class="items-preview">
                          <span
                            v-for="(item, index) in order.topItems"
                            :key="index"
                            class="item-tag"
                          >
                            {{ item }}
                          </span>
                          <span v-if="order.itemsCount > 3" class="more-items">
                            +{{ order.itemsCount - 3 }} more
                          </span>
                        </div>
                      </div>
                    </td>

                    <td class="payment-column-cell">
                      <div class="payment-info-container">
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
                        <div class="payment-meta">
                          <span class="payment-date">{{
                            formatDate(order.createdAt)
                          }}</span>
                        </div>
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
                        <span class="currency-symbol">$</span>
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
                          class="action-button-small edit-action"
                          @click="openStatusModal(order)"
                          title="Edit Status"
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
                              @click.prevent="printInvoice(order)"
                            >
                              <i class="fas fa-print"></i>
                              Print Invoice
                            </a>
                            <div class="dropdown-divider"></div>
                            <a
                              href="#"
                              class="dropdown-menu-item danger-item"
                              @click.prevent="deleteOrder(order)"
                            >
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

          <!-- Cards View -->
          <div
            v-else-if="orderStore.orders.length > 0 && viewMode === 'cards'"
            class="cards-view-container"
          >
            <div class="orders-cards-grid">
              <div
                v-for="order in orderStore.orders"
                :key="order.id"
                class="order-card-item"
              >
                <div class="order-card-header">
                  <div class="order-card-id">
                    <input
                      type="checkbox"
                      v-model="selectedOrders"
                      :value="order.id"
                      class="card-checkbox"
                    />
                    <span class="order-number"
                      >#{{ orderStore.formatOrderId(order.id) }}</span
                    >
                  </div>
                  <span
                    :class="`order-status-badge status-${order.status
                      .toLowerCase()
                      .replace(/\s+/g, '')}`"
                  >
                    {{ order.statusDisplay || order.status }}
                  </span>
                </div>

                <div class="order-card-body">
                  <div class="customer-info-card">
                    <div class="customer-avatar-card">
                      <i class="fas fa-user"></i>
                    </div>
                    <div class="customer-details-card">
                      <div class="customer-name">
                        {{ order.userFullName || "N/A" }}
                      </div>
                      <div class="customer-id">
                        ID: {{ order.userId || "N/A" }}
                      </div>
                    </div>
                  </div>

                  <div class="order-summary-card">
                    <div class="summary-item">
                      <span class="summary-label">Items:</span>
                      <span class="summary-value">{{
                        order.itemsCount || 0
                      }}</span>
                    </div>
                    <div class="summary-item">
                      <span class="summary-label">Payment:</span>
                      <span class="summary-value">{{
                        order.paymentMethod
                      }}</span>
                    </div>
                    <div class="summary-item">
                      <span class="summary-label">Amount:</span>
                      <span class="summary-value amount"
                        >${{ formatCurrency(order.totalAmount) }}</span
                      >
                    </div>
                  </div>
                </div>

                <div class="order-card-footer">
                  <div class="order-date-card">
                    {{ formatDate(order.createdAt) }}
                  </div>
                  <div class="card-actions">
                    <button
                      class="card-action-btn view-btn"
                      @click="viewOrderDetails(order)"
                    >
                      <i class="fas fa-eye"></i>
                    </button>
                    <button
                      class="card-action-btn edit-btn"
                      @click="openStatusModal(order)"
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
                <i class="fas fa-shopping-cart"></i>
              </div>
              <h3 class="empty-state-title">No Orders Found</h3>
              <p class="empty-state-description">
                No orders match your current filters. Try adjusting your search
                criteria or date range.
              </p>
              <button class="empty-state-action-btn" @click="clearAllFilters">
                <i class="fas fa-refresh"></i>
                Clear All Filters
              </button>
            </div>
          </div>

          <!-- Pagination -->
          <div class="pagination-section-container">
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
                class="pagination-control-btn"
                @click="loadOrders(1)"
                :disabled="orderStore.currentPage <= 1"
              >
                <i class="fas fa-angle-double-left"></i>
              </button>
              <button
                class="pagination-control-btn"
                @click="loadOrders(orderStore.currentPage - 1)"
                :disabled="orderStore.currentPage <= 1"
              >
                <i class="fas fa-angle-left"></i>
                Previous
              </button>

              <div class="page-numbers-container">
                <button
                  v-for="page in orderStore.displayedPages"
                  :key="page"
                  :class="[
                    'page-number-btn',
                    { 'active-page': page === orderStore.currentPage },
                  ]"
                  @click="loadOrders(page)"
                >
                  {{ page }}
                </button>
              </div>

              <button
                class="pagination-control-btn"
                @click="loadOrders(orderStore.currentPage + 1)"
                :disabled="orderStore.currentPage >= orderStore.totalPages"
              >
                Next
                <i class="fas fa-angle-right"></i>
              </button>
              <button
                class="pagination-control-btn"
                @click="loadOrders(orderStore.totalPages)"
                :disabled="orderStore.currentPage >= orderStore.totalPages"
              >
                <i class="fas fa-angle-double-right"></i>
              </button>
            </div>
          </div>
        </section>
      </main>
    </div>

    <!-- Order Details Modal -->
    <div
      v-if="showDetailsModal"
      class="modal-overlay-backdrop"
      @click.self="closeDetailsModal"
    >
      <div class="modal-container-wrapper order-details-modal">
        <div class="modal-header-section">
          <h3 class="modal-title-text">
            <i class="fas fa-receipt"></i>
            Order Details - #{{
              selectedOrder ? orderStore.formatOrderId(selectedOrder.id) : ""
            }}
          </h3>
          <button class="modal-close-button" @click="closeDetailsModal">
            <i class="fas fa-times"></i>
          </button>
        </div>
        <div class="modal-body-section">
          <div v-if="selectedOrder" class="order-details-container">
            <!-- Order Information Grid -->
            <div class="details-grid-container">
              <div class="details-section-container">
                <h6 class="details-section-title">Order Information</h6>
                <div class="details-table-container">
                  <div class="detail-row-container">
                    <span class="detail-label-text">Order ID:</span>
                    <span class="detail-value-text"
                      >#{{ orderStore.formatOrderId(selectedOrder.id) }}</span
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
                    <span class="detail-value-text">{{
                      selectedOrder.userId || "N/A"
                    }}</span>
                  </div>
                  <div class="detail-row-container">
                    <span class="detail-label-text">Email:</span>
                    <span class="detail-value-text">{{
                      selectedOrder.customerEmail || "N/A"
                    }}</span>
                  </div>
                  <div class="detail-row-container">
                    <span class="detail-label-text">Phone:</span>
                    <span class="detail-value-text">{{
                      selectedOrder.customerPhone || "N/A"
                    }}</span>
                  </div>
                </div>
              </div>
            </div>

            <!-- Order Items -->
            <div class="order-timeline-section">
              <h6 class="details-section-title">Order Items</h6>
              <div class="timeline-container">
                <div
                  v-for="(item, index) in selectedOrder.items"
                  :key="index"
                  class="timeline-item"
                >
                  <div class="timeline-icon created">
                    <i class="fas fa-box"></i>
                  </div>
                  <div class="timeline-content">
                    <div class="timeline-title">
                      {{ item.product.name }}
                    </div>
                    <div class="timeline-date">
                      Quantity: {{ item.quantity }} &nbsp; | &nbsp; Price: RM
                      {{ parseFloat(item.price).toFixed(2) }} &nbsp; | &nbsp;
                      Subtotal: RM {{ (item.quantity * item.price).toFixed(2) }}
                    </div>
                  </div>
                </div>
              </div>
            </div>

            <!-- Order Actions -->
            <div class="modal-actions-container">
              <button
                class="modal-action-button edit-status-action"
                @click="
                  openStatusModal(selectedOrder);
                  closeDetailsModal();
                "
              >
                <i class="fas fa-edit"></i>
                Edit Status
              </button>
              <button
                class="modal-action-button print-action"
                @click="printInvoice(selectedOrder)"
              >
                <i class="fas fa-print"></i>
                Print Invoice
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
            Update Order Status - #{{
              statusOrder ? orderStore.formatOrderId(statusOrder.id) : ""
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
              <span
                :class="`order-status-badge status-${statusOrder.status
                  .toLowerCase()
                  .replace(/\s+/g, '')}`"
              >
                <i :class="getStatusIcon(statusOrder.status)"></i>
                {{ statusOrder.statusDisplay || statusOrder.status }}
              </span>
            </div>

            <div class="new-status-section">
              <h6 class="section-subtitle">Select New Status</h6>
              <div class="status-options-grid">
                <button
                  v-for="status in availableStatuses"
                  :key="status.key"
                  :class="[
                    'status-option-btn',
                    `status-${status.key.toLowerCase().replace(/\s+/g, '')}`,
                    { selected: selectedNewStatus === status.key },
                  ]"
                  @click="selectedNewStatus = status.key"
                  :disabled="isUpdatingStatus"
                >
                  <i :class="getStatusIcon(status.key)"></i>
                  <span>{{ status.value }}</span>
                </button>
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
</template>

<script>
import { ref, computed, onMounted } from "vue";
import { useOrderStore } from "../stores/useOrderStore";

export default {
  name: "OrdersPage",
  setup() {
    const orderStore = useOrderStore();

    // Local component state (only UI-specific state)
    const selectedOrder = ref(null);
    const statusOrder = ref(null);
    const viewMode = ref("table");
    const sortBy = ref("date-desc");
    const showDetailsModal = ref(false);
    const showStatusModal = ref(false);
    const messages = ref([]);
    const selectedOrders = ref([]);
    const selectAll = ref(false);
    const isSidebarCollapsed = ref(false);
    const pageSize = ref(25);

    // Status management
    const availableStatuses = ref([]);
    const selectedNewStatus = ref("");
    const statusNotes = ref("");
    const isUpdatingStatus = ref(false);

    const filters = ref({
      status: "",
      paymentMethod: "",
      dateRange: "",
      minAmount: "",
      maxAmount: "",
      search: "",
    });

    const toggleSidebar = () => {
      isSidebarCollapsed.value = !isSidebarCollapsed.value;
    };

    const loadOrders = async (page = 1) => {
      try {
        const params = {
          page,
          pageSize: pageSize.value,
          status: filters.value.status || undefined,
          paymentMethod: filters.value.paymentMethod || undefined,
          search: filters.value.search || undefined,
          sort: sortBy.value || undefined,
          dateRange: filters.value.dateRange || undefined,
          minAmount: filters.value.minAmount || undefined,
          maxAmount: filters.value.maxAmount || undefined,
        };

        await orderStore.fetchOrders(params);
      } catch (error) {
        console.error("Error loading orders:", error);
        showMessage("Error loading orders. Please try again.", "error");
      }
    };

    const toggleSelectAll = () => {
      if (selectAll.value) {
        selectedOrders.value = orderStore.orders.map((order) => order.id);
      } else {
        selectedOrders.value = [];
      }
    };

    const viewOrderDetails = (order) => {
      selectedOrder.value = order;
      showDetailsModal.value = true;
    };

    const closeDetailsModal = () => {
      showDetailsModal.value = false;
      setTimeout(() => {
        selectedOrder.value = null;
      }, 300);
    };

    const openStatusModal = (order) => {
      statusOrder.value = order;
      selectedNewStatus.value = "";
      statusNotes.value = "";
      availableStatuses.value = orderStore.getAvailableStatusesFor(
        order.status
      );
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

    const updateOrderStatus = async () => {
      if (!selectedNewStatus.value || !statusOrder.value) return;

      isUpdatingStatus.value = true;
      try {
        const result = await orderStore.updateOrderStatus(
          statusOrder.value.id,
          selectedNewStatus.value,
          statusNotes.value
        );

        if (result.success) {
          showMessage(result.message, "success");
          closeStatusModal();
        } else {
          showMessage(result.message, "error");
        }
      } catch (error) {
        console.error("Error updating order status:", error);
        showMessage("Error updating order status. Please try again.", "error");
      } finally {
        isUpdatingStatus.value = false;
      }
    };

    const clearAllFilters = () => {
      filters.value = {
        status: "",
        paymentMethod: "",
        dateRange: "",
        minAmount: "",
        maxAmount: "",
        search: "",
      };
      loadOrders(1);
    };

    const printInvoices = async () => {
      if (selectedOrders.value.length === 0) return;

      try {
        const result = await orderStore.downloadInvoices(selectedOrders.value);
        if (result.success) {
          showMessage(result.message, "success");
        } else {
          showMessage(result.message, "error");
        }
      } catch (error) {
        console.error("Error downloading invoices:", error);
        showMessage("Failed to download invoices. Please try again.", "error");
      }
    };

    const printInvoice = async (order) => {
      try {
        const result = await orderStore.downloadInvoices([order.id]);
        if (result.success) {
          showMessage(
            `Invoice for order #${orderStore.formatOrderId(
              order.id
            )} downloaded successfully.`,
            "success"
          );
        } else {
          showMessage(
            `Failed to download invoice for order #${orderStore.formatOrderId(
              order.id
            )}.`,
            "error"
          );
        }
      } catch (error) {
        console.error("Error downloading invoice:", error);
        showMessage(
          `Failed to download invoice for order #${orderStore.formatOrderId(
            order.id
          )}.`,
          "error"
        );
      }
    };

    const deleteOrder = async (order) => {
      if (
        confirm(
          `Are you sure you want to delete order #${orderStore.formatOrderId(
            order.id
          )}?`
        )
      ) {
        try {
          const result = await orderStore.deleteOrder(order.id);
          if (result.success) {
            showMessage(result.message, "success");
          } else {
            showMessage(result.message, "error");
          }
        } catch (error) {
          console.error("Failed to delete order:", error);
          showMessage("Failed to delete order. Please try again.", "error");
        }
      }
    };

    const totalQuantity = (items) => {
      if (!Array.isArray(items)) return 0;
      return items.reduce((sum, item) => sum + (item.quantity || 0), 0);
    };

    const sortColumn = (column) => {
      // Implement sorting logic
      showMessage(`Sorting by ${column}...`, "info");
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

    const getStatusIcon = (status) => {
      const icons = {
        Confirmed: "fas fa-check-circle",
        PendingApproval: "fas fa-clock",
        Rejected: "fas fa-times-circle",
        Completed: "fas fa-flag-checkered",
      };
      return icons[status] || "fas fa-circle";
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
        loadOrders(1);
      }, 500);
    };

    onMounted(() => {
      loadOrders(1);
      orderStore.initSignalR();
    });

    return {
      orderStore,
      selectedOrder,
      statusOrder,
      viewMode,
      sortBy,
      showDetailsModal,
      showStatusModal,
      messages,
      selectedOrders,
      selectAll,
      isSidebarCollapsed,
      pageSize,
      availableStatuses,
      selectedNewStatus,
      statusNotes,
      isUpdatingStatus,
      filters,
      toggleSidebar,
      loadOrders,
      toggleSelectAll,
      viewOrderDetails,
      closeDetailsModal,
      openStatusModal,
      closeStatusModal,
      updateOrderStatus,
      clearAllFilters,
      printInvoices,
      printInvoice,
      deleteOrder,
      sortColumn,
      showMessage,
      removeMessage,
      getStatusIcon,
      formatCurrency,
      formatDate,
      formatTime,
      debouncedSearch,
      totalQuantity,
    };
  },
};
</script>

<style scoped>
/* Import all the base styles from CheckPage */
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

.total-orders-stat::before {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
}

.pending-orders-stat::before {
  background: linear-gradient(135deg, #f39c12 0%, #e67e22 100%);
}

.revenue-stat::before {
  background: linear-gradient(135deg, #2ecc71 0%, #27ae60 100%);
}

.avg-order-stat::before {
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

.total-orders-stat .stat-icon-container {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
}

.pending-orders-stat .stat-icon-container {
  background: linear-gradient(135deg, #f39c12 0%, #e67e22 100%);
}

.revenue-stat .stat-icon-container {
  background: linear-gradient(135deg, #2ecc71 0%, #27ae60 100%);
}

.avg-order-stat .stat-icon-container {
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
.bulk-actions-panel {
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

.advanced-filters-grid {
  display: grid;
  grid-template-columns: 165px 166px 0.4fr;
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

.amount-range-container {
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.amount-input {
  flex: 1;
  padding: 0.75rem 0.9375rem;
  border: 2px solid #e9ecef;
  border-radius: 0.625rem;
  font-size: 1rem;
  transition: all 0.3s ease;
  background: white;
}

.amount-input:focus {
  outline: none;
  border-color: #667eea;
  box-shadow: 0 0 0 3px rgba(102, 126, 234, 0.1);
}

.range-separator {
  color: #7f8c8d;
  font-weight: 600;
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

/* Bulk Actions Grid */
.bulk-actions-grid {
  display: grid;
  grid-template-columns: repeat(2, 1fr);
  gap: 0.9375rem;
}

.bulk-action-btn {
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

.bulk-action-btn:hover:not(:disabled) {
  transform: translateY(-0.1875rem);
  box-shadow: 0 0.5rem 1.5625rem rgba(0, 0, 0, 0.2);
}

.bulk-action-btn:disabled {
  opacity: 0.5;
  cursor: not-allowed;
  transform: none;
}

.export-selected {
  background: linear-gradient(135deg, #3498db 0%, #2980b9 100%);
}

.update-status {
  background: linear-gradient(135deg, #f39c12 0%, #e67e22 100%);
}

.print-invoices {
  background: linear-gradient(135deg, #9b59b6 0%, #8e44ad 100%);
}

.archive-orders {
  background: linear-gradient(135deg, #95a5a6 0%, #7f8c8d 100%);
}

.bulk-action-btn i {
  font-size: 1.2rem;
}

.bulk-action-btn span {
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

.order-type-badge {
  font-size: 0.7rem;
  padding: 0.125rem 0.5rem;
  border-radius: 0.75rem;
  font-weight: 600;
  text-transform: uppercase;
}

.order-type-badge.cash {
  background: #e8f5e8;
  color: #27ae60;
}

.order-type-badge.invoice {
  background: #fff3cd;
  color: #e67e22;
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

.items-summary {
  display: flex;
  flex-direction: column;
  gap: 0.25rem;
}

.items-count {
  font-weight: 600;
  color: #2c3e50;
  font-size: 0.9rem;
}

.items-preview {
  display: flex;
  flex-wrap: wrap;
  gap: 0.25rem;
}

.item-tag {
  background: #f8f9fa;
  color: #6c757d;
  padding: 0.125rem 0.375rem;
  border-radius: 0.25rem;
  font-size: 0.7rem;
  font-weight: 500;
}

.more-items {
  color: #7f8c8d;
  font-size: 0.7rem;
  font-style: italic;
}

.payment-info-container {
  display: flex;
  flex-direction: column;
  gap: 0.25rem;
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

.payment-meta {
  font-size: 0.8rem;
  color: #7f8c8d;
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

.orders-cards-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(20rem, 1fr));
  gap: 1.25rem;
}

.order-card-item {
  background: white;
  border-radius: 0.75rem;
  box-shadow: 0 0.25rem 1.25rem rgba(0, 0, 0, 0.08);
  border: 1px solid #e9ecef;
  transition: all 0.3s ease;
  overflow: hidden;
}

.order-card-item:hover {
  transform: translateY(-0.25rem);
  box-shadow: 0 0.5rem 1.875rem rgba(0, 0, 0, 0.12);
}

.order-card-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 1rem 1.25rem;
  border-bottom: 1px solid #f1f3f4;
  background: #f8f9fa;
}

.order-card-id {
  display: flex;
  align-items: center;
  gap: 0.75rem;
}

.card-checkbox {
  width: 1rem;
  height: 1rem;
  accent-color: #667eea;
}

.order-number {
  font-family: "Courier New", monospace;
  font-weight: 700;
  color: #2c3e50;
  font-size: 1rem;
}

.order-card-body {
  padding: 1.25rem;
}

.customer-info-card {
  display: flex;
  align-items: center;
  gap: 0.75rem;
  margin-bottom: 1rem;
}

.customer-avatar-card {
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

.customer-details-card {
  flex: 1;
}

.customer-name {
  font-weight: 600;
  color: #2c3e50;
  margin-bottom: 0.25rem;
}

.customer-id {
  font-size: 0.8rem;
  color: #7f8c8d;
}

.order-summary-card {
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
}

.summary-item {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.summary-label {
  font-size: 0.9rem;
  color: #7f8c8d;
}

.summary-value {
  font-weight: 600;
  color: #2c3e50;
}

.summary-value.amount {
  color: #27ae60;
  font-size: 1.1rem;
}

.order-card-footer {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 1rem 1.25rem;
  border-top: 1px solid #f1f3f4;
  background: #f8f9fa;
}

.order-date-card {
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

.order-details-modal {
  max-width: 60rem;
}

.status-modal {
  max-width: 40rem;
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

/* Order Timeline */
.order-timeline-section {
  background: #f8f9fa;
  border-radius: 0.75rem;
  padding: 1.25rem;
  margin-bottom: 1.5625rem;
}

.timeline-container {
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

.timeline-item {
  display: flex;
  align-items: flex-start;
  gap: 1rem;
}

.timeline-icon {
  width: 2.5rem;
  height: 2.5rem;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  color: white;
  font-size: 1rem;
  flex-shrink: 0;
}

.timeline-icon.created {
  background: linear-gradient(135deg, #2ecc71 0%, #27ae60 100%);
}

.timeline-icon.updated {
  background: linear-gradient(135deg, #f39c12 0%, #e67e22 100%);
}

.timeline-content {
  flex: 1;
  padding-top: 0.25rem;
}

.timeline-title {
  font-weight: 600;
  color: #2c3e50;
  margin-bottom: 0.25rem;
}

.timeline-date {
  font-size: 0.9rem;
  color: #7f8c8d;
  margin-bottom: 0.25rem;
}

.timeline-meta {
  font-size: 0.8rem;
  color: #95a5a6;
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

.edit-status-action {
  background: linear-gradient(135deg, #f39c12 0%, #e67e22 100%);
  color: white;
}

.print-action {
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

/* Status Management Modal */
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

.status-option-btn.selected {
  border-color: #667eea;
  background: #667eea;
  color: white;
}

.status-option-btn.status-confirmed {
  border-color: #2ecc71;
  color: #2ecc71;
}

.status-option-btn.status-confirmed:hover:not(:disabled) {
  background: #2ecc71;
  color: white;
}

.status-option-btn.status-rejected {
  border-color: #e74c3c;
  color: #e74c3c;
}

.status-option-btn.status-rejected:hover:not(:disabled) {
  background: #e74c3c;
  color: white;
}

.status-option-btn.status-completed {
  border-color: #9b59b6;
  color: #9b59b6;
}

.status-option-btn.status-completed:hover:not(:disabled) {
  background: #9b59b6;
  color: white;
}

.status-option-btn.status-pendingapproval {
  border-color: #f39c12;
  color: #f39c12;
}

.status-option-btn.status-pendingapproval:hover:not(:disabled) {
  background: #f39c12;
  color: white;
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

  .bulk-actions-grid {
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

  .orders-cards-grid {
    grid-template-columns: 1fr;
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
