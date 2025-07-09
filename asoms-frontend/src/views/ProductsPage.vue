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
            <a href="#" class="nav-item active"
              ><i class="fas fa-box-open fa-fw"></i
              ><span v-if="!isSidebarCollapsed">Products</span></a
            >
          </li>
          <!-- <li>
            <router-link to="/admin/analytics" class="nav-item"
              ><i class="fas fa-chart-line fa-fw"></i
              ><span v-if="!isSidebarCollapsed">Analytics</span></router-link
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
              <i class="fas fa-box-open navbar-brand-icon"></i>
              <span class="navbar-brand-text">Product Management</span>
            </div>
          </div>
          <div class="navbar-right-main">
            <!-- <button class="notification-btn">
              <i class="fas fa-bell"></i>
              <span class="notification-badge">{{ lowStockCount }}</span>
            </button> -->
            <!-- <div class="user-profile-section">
              <img
                src="../assets/placeholder.svg?width=40&height=40"
                alt="Admin User"
                class="user-avatar"
              />
              <div class="user-details">
                <span class="user-name">Admin User</span>
                <span class="user-role">Administrator</span>
              </div>
            </div> -->
          </div>
        </div>
      </nav>

      <!-- Main Content -->
      <main class="content-area-wrapper">
        <!-- Product Stats -->
        <section class="stats-dashboard-section">
          <div class="stats-grid-container">
            <div class="stat-card total-products-stat">
              <div class="stat-card-inner">
                <div class="stat-icon-container">
                  <i class="fas fa-box"></i>
                </div>
                <div class="stat-content-container">
                  <div class="stat-number">{{ totalProducts }}</div>
                  <div class="stat-label">Total Products</div>
                  <div class="stat-trend positive">
                    <i class="fas fa-arrow-up"></i>
                    <!-- <span>+5% this month</span> -->
                  </div>
                </div>
              </div>
            </div>

            <!-- Update the active products stat card -->
            <div class="stat-card active-products-stat">
              <div class="stat-card-inner">
                <div class="stat-icon-container">
                  <i class="fas fa-shopping-cart"></i>
                </div>
                <div class="stat-content-container">
                  <div class="stat-number">{{ activeProducts }}</div>
                  <div class="stat-label">Active Products</div>
                  <div class="stat-trend positive">
                    <i class="fas fa-chart-line"></i>
                    <span>Ordered in 30 days</span>
                  </div>
                </div>
              </div>
            </div>

            <!-- Add a new stat card for inactive products -->
            <div class="stat-card inactive-products-stat">
              <div class="stat-card-inner">
                <div class="stat-icon-container">
                  <i class="fas fa-pause-circle"></i>
                </div>
                <div class="stat-content-container">
                  <div class="stat-number">
                    {{ totalProducts - activeProducts }}
                  </div>
                  <div class="stat-label">Inactive Products</div>
                  <div class="stat-trend attention">
                    <i class="fas fa-exclamation-triangle"></i>
                    <span>No orders in 30 days</span>
                  </div>
                </div>
              </div>
            </div>

            <div class="stat-card low-stock-stat">
              <div class="stat-card-inner">
                <div class="stat-icon-container">
                  <i class="fas fa-exclamation-triangle"></i>
                </div>
                <div class="stat-content-container">
                  <div class="stat-number">{{ lowStockCount }}</div>
                  <div class="stat-label">Low Stock Items</div>
                  <div class="stat-trend warning">
                    <i class="fas fa-exclamation-triangle"></i>
                    <span>Needs attention</span>
                  </div>
                </div>
              </div>
            </div>

            <!-- <div class="stat-card categories-stat">
              <div class="stat-card-inner">
                <div class="stat-icon-container">
                  <i class="fas fa-tags"></i>
                </div>
                <div class="stat-content-container">
                  <div class="stat-number">{{ totalCategories }}</div>
                  <div class="stat-label">Categories</div>
                  <div class="stat-trend positive">
                    <i class="fas fa-arrow-up"></i>
                    <span>+1 new category</span>
                  </div>
                </div>
              </div>
            </div> -->
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
                  Product Filters
                </h3>
                <button class="clear-filters-btn" @click="clearAllFilters">
                  <i class="fas fa-times"></i>
                  Clear All
                </button>
              </div>

              <div class="advanced-filters-grid">
                <div class="filter-group-container">
                  <label class="filter-label-text">Category</label>
                  <select
                    v-model="filters.category"
                    class="filter-select-input"
                    @change="loadProducts(1)"
                  >
                    <option value="">All Categories</option>
                    <option value="Tyres">Tyres</option>
                    <option value="Batteries">Batteries</option>
                    <option value="Oil">Oil & Fluids</option>
                    <option value="Parts">Car Parts</option>
                    <option value="Accessories">Accessories</option>
                  </select>
                </div>

                <!-- Update the status filter -->
                <div class="filter-group-container">
                  <label class="filter-label-text">Status</label>
                  <select
                    v-model="filters.status"
                    class="filter-select-input"
                    @change="loadProducts(1)"
                  >
                    <option value="">All Statuses</option>
                    <option value="Active">Active (Ordered in 30 days)</option>
                    <option value="Inactive">
                      Inactive (No orders in 30 days)
                    </option>
                  </select>
                </div>

                <div class="filter-group-container">
                  <label class="filter-label-text">Stock Level</label>
                  <select
                    v-model="filters.stockLevel"
                    class="filter-select-input"
                    @change="loadProducts(1)"
                  >
                    <option value="">All Levels</option>
                    <option value="low">Low Stock less than 10</option>
                    <option value="medium">Medium Stock (10-50)</option>
                    <option value="high">High Stock (> 50)</option>
                  </select>
                </div>

                <div class="filter-group-container search-group-advanced">
                  <label class="filter-label-text">Search Products</label>
                  <div class="search-input-container">
                    <i class="fas fa-search search-input-icon"></i>
                    <input
                      v-model="filters.search"
                      type="text"
                      class="search-input-field"
                      placeholder="Product name, brand..."
                      @input="debouncedSearch"
                    />
                  </div>
                </div>
              </div>
            </div>

            <!-- Product Actions Panel -->
            <div class="product-actions-panel">
              <div class="panel-header-section">
                <h3 class="panel-title">
                  <i class="fas fa-cogs"></i>
                  Product Actions
                </h3>
                <span class="selected-count"
                  >{{ selectedProducts.length }} selected</span
                >
              </div>

              <div class="product-actions-grid">
                <button
                  class="product-action-btn add-product"
                  @click="openAddProductModal"
                >
                  <i class="fas fa-plus"></i>
                  <span>Add Product</span>
                </button>

                <button
                  class="product-action-btn bulk-upload"
                  @click="openBulkUploadModal"
                >
                  <i class="fas fa-upload"></i>
                  <span>Bulk Upload</span>
                </button>

                <!-- <button
                  class="product-action-btn export-products"
                  @click="exportProducts"
                  :disabled="selectedProducts.length === 0"
                >
                  <i class="fas fa-download"></i>
                  <span>Export Selected</span>
                </button>

                <button
                  class="product-action-btn update-stock"
                  @click="openBulkStockModal"
                  :disabled="selectedProducts.length === 0"
                >
                  <i class="fas fa-boxes"></i>
                  <span>Update Stock</span>
                </button> -->
              </div>
            </div>
          </div>
        </section>

        <!-- Products Table Section -->
        <section class="products-table-section">
          <div class="section-header-container">
            <div class="header-left-section">
              <h2 class="section-main-title">
                <i class="fas fa-list-ul"></i>
                Products List
              </h2>
              <div class="results-info-text">
                <span v-if="!isLoading">
                  Showing {{ paginationInfo.start }}-{{ paginationInfo.end }} of
                  {{ totalItems }} products
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
                  <!-- Update the sort dropdown -->
                  <select
                    v-model="sortBy"
                    @change="loadProducts(1)"
                    class="sort-dropdown-select"
                  >
                    <option value="name-asc">Name A-Z</option>
                    <option value="name-desc">Name Z-A</option>
                    <option value="price-desc">Highest Price</option>
                    <option value="price-asc">Lowest Price</option>
                    <option value="stock-desc">Highest Stock</option>
                    <option value="stock-asc">Lowest Stock</option>
                    <option value="status-desc">Active First</option>
                    <option value="status-asc">Inactive First</option>
                    <option value="lastorder-desc">Recent Orders First</option>
                    <option value="ordercount-desc">Most Ordered</option>
                    <option value="revenue-desc">Highest Revenue</option>
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
              <h3 class="loading-title">Loading Products...</h3>
              <p class="loading-description">Fetching product data</p>
            </div>
          </div>

          <!-- Table View -->
          <div
            v-else-if="products.length > 0 && viewMode === 'table'"
            class="table-wrapper-container"
          >
            <div class="table-scroll-container">
              <table class="products-data-table">
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
                      Product
                      <i class="fas fa-sort sort-indicator"></i>
                    </th>
                    <th class="standard-column-header">Category</th>
                    <th
                      class="sortable-column-header"
                      @click="sortColumn('brand')"
                    >
                      Brand
                      <i class="fas fa-sort sort-indicator"></i>
                    </th>
                    <th
                      class="sortable-column-header"
                      @click="sortColumn('price')"
                    >
                      Price
                      <i class="fas fa-sort sort-indicator"></i>
                    </th>
                    <th
                      class="sortable-column-header"
                      @click="sortColumn('quantity')"
                    >
                      Stock
                      <i class="fas fa-sort sort-indicator"></i>
                    </th>
                    <th class="standard-column-header">Status</th>
                    <th
                      class="sortable-column-header"
                      @click="sortColumn('createdAt')"
                    >
                      Created
                      <i class="fas fa-sort sort-indicator"></i>
                    </th>
                    <th class="actions-column-header">Actions</th>
                  </tr>
                </thead>
                <tbody class="table-body-section">
                  <tr
                    v-for="product in products"
                    :key="product.id"
                    class="table-data-row"
                  >
                    <td class="checkbox-column-cell">
                      <input
                        type="checkbox"
                        v-model="selectedProducts"
                        :value="product.id"
                        class="row-checkbox"
                      />
                    </td>
                    <td class="product-column-cell">
                      <div class="product-info-display">
                        <div class="product-image-container">
                          <img
                            v-if="product.imageUrl"
                            :src="imageBase(product.imageUrl)"
                            :alt="product.name"
                            class="product-image"
                          />
                          <div v-else class="product-image-placeholder">
                            <i class="fas fa-box"></i>
                          </div>
                        </div>
                        <div class="product-details-container">
                          <div class="product-name-text">
                            {{ product.name }}
                          </div>
                          <div class="product-description-text">
                            {{ product.description }}
                          </div>
                        </div>
                      </div>
                    </td>
                    <td class="category-column-cell">
                      <span
                        class="category-badge"
                        :class="`category-${product.category.toLowerCase()}`"
                      >
                        <i :class="getCategoryIcon(product.category)"></i>
                        {{ product.category }}
                      </span>
                    </td>
                    <td class="sku-column-cell">
                      <div class="sku-display">
                        <span class="sku-text">{{
                          product.brand || "N/A"
                        }}</span>
                      </div>
                    </td>
                    <td class="price-column-cell">
                      <div class="price-display-container">
                        <span class="currency-symbol">$</span>
                        <span class="price-value">{{
                          formatCurrency(product.price)
                        }}</span>
                      </div>
                    </td>
                    <td class="stock-column-cell">
                      <div class="stock-display">
                        <span
                          class="stock-number"
                          :class="getStockClass(product.quantity)"
                        >
                          {{ product.quantity }}
                        </span>
                        <span class="stock-label">units</span>
                      </div>
                    </td>
                    <td class="status-column-cell">
                      <span
                        :class="`product-status-badge status-${product.status.toLowerCase()}`"
                      >
                        <i
                          :class="getStatusIcon(product.status.toLowerCase())"
                        ></i>
                        {{ product.status }}
                      </span>
                      <!-- Show additional order info -->
                      <div
                        v-if="
                          product.status === 'Active' && product.lastOrderDate
                        "
                        class="status-details"
                        style="
                          margin-top: 6px !important;
                          display: flex !important;
                          flex-direction: column !important;
                          gap: 2px !important;
                          max-width: 200px !important;
                          width: fit-content !important;
                        "
                      >
                        <small
                          style="
                            background: linear-gradient(
                              135deg,
                              #f8fafc,
                              #e2e8f0
                            ) !important;
                            color: #475569 !important;
                            padding: 3px 8px !important;
                            border-radius: 8px !important;
                            font-size: 0.65rem !important;
                            font-weight: 500 !important;
                            border: 1px solid #e2e8f0 !important;
                            display: inline-block !important;
                            margin-bottom: 2px !important;
                            line-height: 1.2 !important;
                            width: fit-content !important;
                            max-width: 100% !important;
                            white-space: nowrap !important;
                          "
                          >Last order:
                          {{ formatDate(product.lastOrderDate) }}</small
                        >
                      </div>
                      <div
                        v-if="product.orderCountLast30Days > 0"
                        class="status-details"
                      >
                        <small
                          style="
                            background: rgba(59, 130, 246, 0.1) !important;
                            color: #1e40af !important;
                            padding: 3px 8px !important;
                            border-radius: 8px !important;
                            font-size: 0.65rem !important;
                            font-weight: 500 !important;
                            display: inline-block !important;
                            margin-bottom: 2px !important;
                            line-height: 1.2 !important;
                            width: fit-content !important;
                            white-space: nowrap !important;
                          "
                          >{{ product.orderCountLast30Days }} orders (30
                          days)</small
                        >
                      </div>
                    </td>
                    <td class="updated-column-cell">
                      <div class="date-display-container">
                        <div class="date-primary-text">
                          {{ formatDate(product.createdAt) }}
                        </div>
                        <div class="date-secondary-text">
                          {{ formatTime(product.createdAt) }}
                        </div>
                      </div>
                    </td>
                    <td class="actions-column-cell">
                      <div class="action-buttons-container">
                        <button
                          class="action-button-small view-action"
                          @click="viewProductDetails(product)"
                          title="View Details"
                        >
                          <i class="fas fa-eye"></i>
                        </button>

                        <button
                          class="action-button-small edit-action"
                          @click="editProduct(product)"
                          title="Edit Product"
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
                              @click.prevent="duplicateProduct(product)"
                            >
                              <i class="fas fa-copy"></i>
                              Duplicate Product
                            </a>
                            <a
                              href="#"
                              class="dropdown-menu-item"
                              @click.prevent="updateStock(product)"
                            >
                              <i class="fas fa-boxes"></i>
                              Update Stock
                            </a>
                            <a
                              href="#"
                              class="dropdown-menu-item"
                              @click.prevent="toggleProductStatus(product)"
                            >
                              <i
                                :class="
                                  getProductStatus(product) === 'Active'
                                    ? 'fas fa-pause'
                                    : 'fas fa-play'
                                "
                              ></i>
                              {{
                                getProductStatus(product) === "Active"
                                  ? "Deactivate"
                                  : "Activate"
                              }}
                            </a>
                            <div class="dropdown-divider"></div>
                            <a
                              href="#"
                              class="dropdown-menu-item danger-item"
                              @click.prevent="deleteProduct(product)"
                            >
                              <i class="fas fa-trash"></i>
                              Delete Product
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
            v-else-if="products.length > 0 && viewMode === 'cards'"
            class="cards-view-container"
          >
            <div class="products-cards-grid">
              <div
                v-for="product in products"
                :key="product.id"
                class="product-card-item"
              >
                <!-- In the cards view, update the status display -->
                <div class="product-card-header">
                  <div class="product-card-image">
                    <input
                      type="checkbox"
                      v-model="selectedProducts"
                      :value="product.id"
                      class="card-checkbox"
                    />
                    <div class="image-container">
                      <img
                        v-if="product.imageUrl"
                        :src="imageBase(product.imageUrl)"
                        :alt="product.name"
                        class="card-product-image"
                      />
                      <div v-else class="card-image-placeholder">
                        <i class="fas fa-box"></i>
                      </div>
                    </div>
                  </div>
                  <div class="status-container">
                    <span
                      :class="`product-status-badge ${getProductStatus(
                        product
                      )}`"
                    >
                      <i :class="getStatusIcon(getProductStatus(product))"></i>
                      {{ product.status }}
                    </span>
                    <!-- Show order activity info -->
                    <div
                      v-if="product.status === 'Active'"
                      class="order-activity"
                    >
                      <small v-if="product.orderCountLast30Days > 0">
                        {{ product.orderCountLast30Days }} orders
                      </small>
                      <small v-if="product.revenueLast30Days > 0">
                        ${{ formatCurrency(product.revenueLast30Days) }} revenue
                      </small>
                    </div>
                  </div>
                </div>

                <div class="product-card-body">
                  <div class="product-name-card">{{ product.name }}</div>
                  <div class="product-category-card">
                    <span
                      class="category-badge"
                      :class="`category-${product.category.toLowerCase()}`"
                    >
                      {{ product.category }}
                    </span>
                  </div>
                  <div class="product-description-card">
                    {{ product.description }}
                  </div>

                  <div class="product-stats-card">
                    <div class="stat-item">
                      <span class="stat-label">Brand:</span>
                      <span class="stat-value">{{
                        product.brand || "N/A"
                      }}</span>
                    </div>
                    <div class="stat-item">
                      <span class="stat-label">Price:</span>
                      <span class="stat-value price"
                        >${{ formatCurrency(product.price) }}</span
                      >
                    </div>
                    <div class="stat-item">
                      <span class="stat-label">Stock:</span>
                      <span
                        class="stat-value"
                        :class="getStockClass(product.quantity)"
                      >
                        {{ product.quantity }} units
                      </span>
                    </div>
                  </div>
                </div>

                <div class="product-card-footer">
                  <div class="updated-date-card">
                    Created {{ formatDate(product.createdAt) }}
                  </div>
                  <div class="card-actions">
                    <button
                      class="card-action-btn view-btn"
                      @click="viewProductDetails(product)"
                    >
                      <i class="fas fa-eye"></i>
                    </button>
                    <button
                      class="card-action-btn edit-btn"
                      @click="editProduct(product)"
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
                <i class="fas fa-box-open"></i>
              </div>
              <h3 class="empty-state-title">No Products Found</h3>
              <p class="empty-state-description">
                No products match your current filters. Try adjusting your
                search criteria or add a new product.
              </p>
              <button
                class="empty-state-action-btn"
                @click="openAddProductModal"
              >
                <i class="fas fa-plus"></i>
                Add New Product
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
                  @change="loadProducts(1)"
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
                @click="loadProducts(1)"
                :disabled="currentPage <= 1"
              >
                <i class="fas fa-angle-double-left"></i>
              </button>
              <button
                class="pagination-control-btn"
                @click="loadProducts(currentPage - 1)"
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
                  @click="loadProducts(page)"
                >
                  {{ page }}
                </button>
              </div>

              <button
                class="pagination-control-btn"
                @click="loadProducts(currentPage + 1)"
                :disabled="currentPage >= totalPages"
              >
                Next
                <i class="fas fa-angle-right"></i>
              </button>
              <button
                class="pagination-control-btn"
                @click="loadProducts(totalPages)"
                :disabled="currentPage >= totalPages"
              >
                <i class="fas fa-angle-double-right"></i>
              </button>
            </div>
          </div>
        </section>
      </main>
    </div>

    <!-- Product Details Modal -->
    <div
      v-if="showDetailsModal"
      class="modal-overlay-backdrop"
      @click.self="closeDetailsModal"
    >
      <div class="modal-container-wrapper product-details-modal">
        <div class="modal-header-section">
          <h3 class="modal-title-text">
            <i class="fas fa-box"></i>
            Product Details - {{ selectedProduct ? selectedProduct.name : "" }}
          </h3>
          <button class="modal-close-button" @click="closeDetailsModal">
            <i class="fas fa-times"></i>
          </button>
        </div>
        <div class="modal-body-section">
          <div v-if="selectedProduct" class="product-details-container">
            <!-- Product Image Section -->
            <div class="product-image-section">
              <div class="main-image-container">
                <img
                  v-if="selectedProduct.imageUrl"
                  :src="imageBase(selectedProduct.imageUrl)"
                  :alt="selectedProduct.name"
                  class="main-product-image"
                />
                <div v-else class="main-image-placeholder">
                  <i class="fas fa-box"></i>
                </div>
              </div>
              <div class="product-basic-info">
                <h4 class="product-title">{{ selectedProduct.name }}</h4>
                <span
                  :class="`product-status-badge ${getProductStatus(
                    selectedProduct
                  )}`"
                >
                  {{ getProductStatus(selectedProduct) }}
                </span>
                <div class="product-meta">
                  <span class="product-sku">ID: {{ selectedProduct.id }}</span>
                  <span class="product-category">
                    Category: {{ selectedProduct.category }}
                  </span>
                </div>
              </div>
            </div>

            <!-- Product Information Grid -->
            <div class="details-grid-container">
              <div class="details-section-container">
                <h6 class="details-section-title">Product Information</h6>
                <div class="details-table-container">
                  <div class="detail-row-container">
                    <span class="detail-label-text">Name:</span>
                    <span class="detail-value-text">{{
                      selectedProduct.name
                    }}</span>
                  </div>
                  <div class="detail-row-container">
                    <span class="detail-label-text">Description:</span>
                    <span class="detail-value-text">{{
                      selectedProduct.description || "N/A"
                    }}</span>
                  </div>
                  <div class="detail-row-container">
                    <span class="detail-label-text">Category:</span>
                    <span class="detail-value-text">{{
                      selectedProduct.category
                    }}</span>
                  </div>
                  <div class="detail-row-container">
                    <span class="detail-label-text">Brand:</span>
                    <span class="detail-value-text">{{
                      selectedProduct.brand || "N/A"
                    }}</span>
                  </div>
                  <div class="detail-row-container" v-if="selectedProduct.size">
                    <span class="detail-label-text">Size:</span>
                    <span class="detail-value-text">{{
                      selectedProduct.size
                    }}</span>
                  </div>
                </div>
              </div>

              <div class="details-section-container">
                <h6 class="details-section-title">Pricing & Stock</h6>
                <div class="details-table-container">
                  <div class="detail-row-container">
                    <span class="detail-label-text">Price:</span>
                    <span class="detail-value-text price-highlight">
                      ${{ formatCurrency(selectedProduct.price) }}
                    </span>
                  </div>
                  <div class="detail-row-container">
                    <span class="detail-label-text">Stock:</span>
                    <span
                      class="detail-value-text"
                      :class="getStockClass(selectedProduct.quantity)"
                    >
                      {{ selectedProduct.quantity }} units
                    </span>
                  </div>
                  <div class="detail-row-container">
                    <span class="detail-label-text">Created:</span>
                    <span class="detail-value-text">
                      {{ formatDate(selectedProduct.createdAt) }}
                    </span>
                  </div>
                </div>
              </div>

              <div class="details-section-container">
                <h6 class="details-section-title">
                  Order Activity (Last 30 Days)
                </h6>
                <div class="details-table-container">
                  <div class="detail-row-container">
                    <span class="detail-label-text">Status:</span>
                    <span
                      :class="`detail-value-text status-${getProductStatus(
                        selectedProduct
                      )}`"
                    >
                      <i
                        :class="
                          getStatusIcon(getProductStatus(selectedProduct))
                        "
                      ></i>
                      {{ selectedProduct.status }}
                    </span>
                  </div>
                  <div
                    class="detail-row-container"
                    v-if="selectedProduct.lastOrderDate"
                  >
                    <span class="detail-label-text">Last Order:</span>
                    <span class="detail-value-text">
                      {{ formatDate(selectedProduct.lastOrderDate) }}
                    </span>
                  </div>
                  <div class="detail-row-container">
                    <span class="detail-label-text">Orders (30 days):</span>
                    <span class="detail-value-text">
                      {{ selectedProduct.orderCountLast30Days || 0 }} units
                    </span>
                  </div>
                  <div
                    class="detail-row-container"
                    v-if="selectedProduct.revenueLast30Days > 0"
                  >
                    <span class="detail-label-text">Revenue (30 days):</span>
                    <span class="detail-value-text price-highlight">
                      ${{ formatCurrency(selectedProduct.revenueLast30Days) }}
                    </span>
                  </div>
                </div>
              </div>
            </div>

            <!-- Product Actions -->
            <div class="modal-actions-container">
              <button
                class="modal-action-button edit-product-action"
                @click="
                  editProduct(selectedProduct);
                  closeDetailsModal();
                "
              >
                <i class="fas fa-edit"></i>
                Edit Product
              </button>
              <button
                class="modal-action-button stock-action"
                @click="updateStock(selectedProduct)"
              >
                <i class="fas fa-boxes"></i>
                Update Stock
              </button>
              <button
                class="modal-action-button duplicate-action"
                @click="duplicateProduct(selectedProduct)"
              >
                <i class="fas fa-copy"></i>
                Duplicate Product
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- Add/Edit Product Modal -->
    <div
      v-if="showAddEditModal"
      class="modal-overlay-backdrop"
      @click.self="closeAddEditModal"
    >
      <div class="modal-container-wrapper add-edit-modal">
        <div class="modal-header-section">
          <h3 class="modal-title-text">
            <i class="fas fa-plus"></i>
            {{ editingProduct ? "Edit Product" : "Add New Product" }}
          </h3>
          <button class="modal-close-button" @click="closeAddEditModal">
            <i class="fas fa-times"></i>
          </button>
        </div>
        <div class="modal-body-section">
          <form @submit.prevent="saveProduct" class="product-form">
            <div class="form-grid">
              <div class="form-group">
                <label class="form-label">Product Name *</label>
                <input
                  v-model="productForm.name"
                  type="text"
                  class="form-input"
                  required
                />
              </div>

              <div class="form-group">
                <label class="form-label">Brand</label>
                <input
                  v-model="productForm.brand"
                  type="text"
                  class="form-input"
                />
              </div>

              <div class="form-group">
                <label class="form-label">Category *</label>
                <select
                  v-model="productForm.category"
                  class="form-select"
                  required
                >
                  <option value="">Select Category</option>
                  <option value="Tyres">Tyres</option>
                  <option value="Batteries">Batteries</option>
                  <option value="Oil">Oil & Fluids</option>
                  <option value="Parts">Car Parts</option>
                  <option value="Accessories">Accessories</option>
                </select>
              </div>

              <div class="form-group">
                <label class="form-label">Size</label>
                <input
                  v-model="productForm.size"
                  type="text"
                  class="form-input"
                  placeholder="e.g. 175/70/13 or N50"
                />
              </div>

              <div class="form-group full-width">
                <label class="form-label">Description</label>
                <textarea
                  v-model="productForm.description"
                  class="form-textarea"
                  rows="3"
                ></textarea>
              </div>

              <div class="form-group">
                <label class="form-label">Price *</label>
                <input
                  v-model="productForm.price"
                  type="number"
                  step="0.01"
                  class="form-input"
                  required
                />
              </div>

              <div class="form-group">
                <label class="form-label">Stock Quantity *</label>
                <input
                  v-model="productForm.quantity"
                  type="number"
                  class="form-input"
                  required
                />
              </div>

              <div class="form-group">
                <label class="form-label">Product Image</label>
                <input
                  type="file"
                  @change="handleImageUpload"
                  accept="image/*"
                  class="form-input"
                />
              </div>

              <div v-if="productForm.imageUrl" class="form-group">
                <label class="form-label">Current Image</label>
                <div class="current-image-preview">
                  <img
                    :src="imageBase(productForm.imageUrl)"
                    alt="Current product image"
                    style="max-height: 100px; max-width: 100px"
                  />
                </div>
              </div>
            </div>

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
                {{ isSaving ? "Saving..." : "Save Product" }}
              </button>
            </div>
          </form>
        </div>
      </div>
    </div>

    <!-- Bulk Upload Modal -->
    <div
      v-if="showBulkUploadModal"
      class="modal-overlay-backdrop"
      @click.self="closeBulkUploadModal"
    >
      <div class="modal-container-wrapper bulk-upload-modal">
        <div class="modal-header-section">
          <h3 class="modal-title-text">
            <i class="fas fa-upload"></i>
            Bulk Upload Products
          </h3>
          <button class="modal-close-button" @click="closeBulkUploadModal">
            <i class="fas fa-times"></i>
          </button>
        </div>
        <div class="modal-body-section">
          <div class="bulk-upload-container">
            <div class="upload-instructions">
              <h6 class="instructions-title">Upload Instructions</h6>
              <ul class="instructions-list">
                <li>Download the CSV template to see the required format</li>
                <li>
                  Fill in your product data following the template structure
                </li>
                <li>Upload the completed CSV file using the form below</li>
                <li>Review and confirm the import before finalizing</li>
              </ul>
            </div>

            <div class="template-download">
              <button class="template-btn" @click="downloadTemplate">
                <i class="fas fa-download"></i>
                Download CSV Template
              </button>
            </div>

            <div class="file-upload-section">
              <div
                class="upload-area"
                :class="{ 'drag-over': isDragOver }"
                @dragover.prevent="isDragOver = true"
                @dragleave.prevent="isDragOver = false"
                @drop.prevent="handleFileDrop"
              >
                <input
                  type="file"
                  ref="fileInput"
                  @change="handleFileSelect"
                  accept=".csv,.xlsx,.xls"
                  class="file-input"
                />
                <div class="upload-content">
                  <i class="fas fa-cloud-upload-alt upload-icon"></i>
                  <p class="upload-text">
                    Drag and drop your CSV file here, or
                    <button
                      type="button"
                      class="browse-btn"
                      @click="$refs.fileInput.click()"
                    >
                      browse
                    </button>
                  </p>
                  <p class="upload-hint">
                    Supports CSV, Excel files (max 10MB)
                  </p>
                </div>
              </div>

              <div v-if="selectedFile" class="selected-file">
                <div class="file-info">
                  <i class="fas fa-file-csv"></i>
                  <span class="file-name">{{ selectedFile.name }}</span>
                  <span class="file-size"
                    >({{ formatFileSize(selectedFile.size) }})</span
                  >
                </div>
                <button class="remove-file-btn" @click="removeSelectedFile">
                  <i class="fas fa-times"></i>
                </button>
              </div>
            </div>

            <div class="upload-actions">
              <button
                type="button"
                class="upload-btn cancel-btn"
                @click="closeBulkUploadModal"
              >
                <i class="fas fa-times"></i>
                Cancel
              </button>
              <button
                type="button"
                class="upload-btn process-btn"
                @click="processUpload"
                :disabled="!selectedFile || isProcessing"
              >
                <i class="fas fa-spinner fa-spin" v-if="isProcessing"></i>
                <i class="fas fa-upload" v-else></i>
                {{ isProcessing ? "Processing..." : "Upload & Process" }}
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
import axios from "axios";
import debounce from "lodash/debounce";
import { useProductStore } from "../stores/useProductStore";

export default {
  name: "ProductsPage",
  setup() {
    // API base URL
    const apiBaseUrl = "https://asoms-production.up.railway.app";

    // State variables
    const products = ref([]);
    const selectedProduct = ref(null);
    const editingProduct = ref(null);
    const isLoading = ref(false);
    const isSaving = ref(false);
    const isProcessing = ref(false);
    const currentPage = ref(1);
    const totalPages = ref(1);
    const totalItems = ref(0);
    const pageSize = ref(10);
    const viewMode = ref("table");
    const sortBy = ref("name-asc");
    const showDetailsModal = ref(false);
    const showAddEditModal = ref(false);
    const showBulkUploadModal = ref(false);
    const messages = ref([]);
    const selectedProducts = ref([]);
    const selectAll = ref(false);
    const isSidebarCollapsed = ref(false);
    const selectedFile = ref(null);
    const isDragOver = ref(false);

    // Stats
    const totalProducts = ref(0);
    const activeProducts = ref(0);
    const lowStockCount = ref(0);
    const totalCategories = ref(5);

    const filters = ref({
      category: "",
      status: "",
      stockLevel: "",
      search: "",
    });

    const productForm = ref({
      name: "",
      category: "",
      brand: "",
      description: "",
      price: "",
      quantity: "",
      size: "",
      image: null,
      imageUrl: null,
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

    const productStore = useProductStore();

    // Load products from API
    const loadProducts = async (page = 1) => {
      isLoading.value = true;
      currentPage.value = page;

      try {
        // Build query parameters (same as before)
        let queryParams = new URLSearchParams();

        if (filters.value.category) {
          queryParams.append("category", filters.value.category);
        }

        if (filters.value.search) {
          queryParams.append("search", filters.value.search);
        }

        if (filters.value.stockLevel) {
          switch (filters.value.stockLevel) {
            case "low":
              queryParams.append("maxStock", "9");
              break;
            case "medium":
              queryParams.append("minStock", "10");
              queryParams.append("maxStock", "50");
              break;
            case "high":
              queryParams.append("minStock", "51");
              break;
          }
        }

        // Updated status filter - now uses the new logic
        if (filters.value.status) {
          queryParams.append("status", filters.value.status);
        }

        if (sortBy.value) {
          const [field, direction] = sortBy.value.split("-");
          queryParams.append("sortBy", field);
          queryParams.append("sortDirection", direction);
        }

        queryParams.append("page", page);
        queryParams.append("pageSize", pageSize.value);

        // Make API request
        const response = await axios.get(
          `${apiBaseUrl}/api/Product?${queryParams.toString()}`
        );

        // Handle the new response structure with ProductDto
        products.value = response.data.items || response.data || [];
        totalItems.value =
          response.data.totalCount || response.data.length || 0;
        totalPages.value =
          response.data.totalPages ||
          Math.ceil(totalItems.value / pageSize.value);

        // Update stats with new logic
        updateProductStats();
      } catch (error) {
        console.error("Error loading products:", error);
        showMessage("Error loading products. Please try again.", "error");
      } finally {
        isLoading.value = false;
      }
    };

    // Update product statistics
    const updateProductStats = () => {
      totalProducts.value = totalItems.value;

      // Count active products (those with orders in past 30 days)
      activeProducts.value = products.value.filter(
        (p) => p.status === "Active"
      ).length;

      // Low stock count remains the same
      lowStockCount.value = products.value.filter(
        (p) => p.quantity < 10
      ).length;

      // Categories count
      const uniqueCategories = new Set(
        products.value.map((p) => p.category).filter(Boolean)
      );
      totalCategories.value = uniqueCategories.size || 5;
    };

    const toggleSelectAll = () => {
      if (selectAll.value) {
        selectedProducts.value = products.value.map((product) => product.id);
      } else {
        selectedProducts.value = [];
      }
    };

    const viewProductDetails = (product) => {
      selectedProduct.value = product;
      showDetailsModal.value = true;
    };

    const closeDetailsModal = () => {
      showDetailsModal.value = false;
      setTimeout(() => {
        selectedProduct.value = null;
      }, 300);
    };

    const imageBase = (url) => {
      if (!url) return null;
      return url.startsWith("http") ? url : `${apiBaseUrl}${url}`;
    };

    const openAddProductModal = () => {
      editingProduct.value = null;
      productForm.value = {
        name: "",
        category: "",
        brand: "",
        description: "",
        price: "",
        quantity: "",
        size: "",
        image: null,
        imageUrl: null,
      };
      showAddEditModal.value = true;
    };

    const editProduct = (product) => {
      editingProduct.value = product;
      productForm.value = {
        name: product.name,
        category: product.category,
        brand: product.brand || "",
        description: product.description || "",
        price: product.price,
        quantity: product.quantity,
        size: product.size || "",
        image: null,
        imageUrl: product.imageUrl,
      };
      showAddEditModal.value = true;
    };

    const closeAddEditModal = () => {
      showAddEditModal.value = false;
      setTimeout(() => {
        editingProduct.value = null;
        productForm.value = {
          name: "",
          category: "",
          brand: "",
          description: "",
          price: "",
          quantity: "",
          size: "",
          image: null,
          imageUrl: null,
        };
      }, 300);
    };

    const saveProduct = async () => {
      isSaving.value = true;

      try {
        const formData = new FormData();
        formData.append("Name", productForm.value.name);
        formData.append("Description", productForm.value.description || "");
        formData.append("Brand", productForm.value.brand || "");
        formData.append("Category", productForm.value.category);
        formData.append("Size", productForm.value.size || "");
        formData.append("Price", productForm.value.price);
        formData.append("Quantity", productForm.value.quantity);

        if (productForm.value.image) {
          formData.append("Image", productForm.value.image);
        }

        let response;

        if (editingProduct.value) {
          // Update existing product
          if (productForm.value.image) {
            // If there's a new image, use the upload-image endpoint
            await axios.put(
              `${apiBaseUrl}/api/Product/${editingProduct.value.id}`,
              {
                name: productForm.value.name,
                description: productForm.value.description || "",
                brand: productForm.value.brand || "",
                category: productForm.value.category,
                size: productForm.value.size || "",
                price: parseFloat(productForm.value.price),
                quantity: parseInt(productForm.value.quantity),
              }
            );

            // Upload the new image separately
            await axios.post(
              `${apiBaseUrl}/api/Product/${editingProduct.value.id}/upload-image`,
              formData,
              {
                headers: {
                  "Content-Type": "multipart/form-data",
                },
              }
            );
          } else {
            // Just update the product without changing the image
            await axios.put(
              `${apiBaseUrl}/api/Product/${editingProduct.value.id}`,
              {
                name: productForm.value.name,
                description: productForm.value.description || "",
                brand: productForm.value.brand || "",
                category: productForm.value.category,
                size: productForm.value.size || "",
                price: parseFloat(productForm.value.price),
                quantity: parseInt(productForm.value.quantity),
              }
            );
          }

          showMessage("Product updated successfully!", "success");
        } else {
          // Add new product
          response = await axios.post(
            `${apiBaseUrl}/api/Product/upload`,
            formData,
            {
              headers: {
                "Content-Type": "multipart/form-data",
              },
            }
          );

          showMessage("Product added successfully!", "success");
        }

        // Reload products to reflect changes
        await loadProducts(currentPage.value);
        closeAddEditModal();
      } catch (error) {
        console.error("Error saving product:", error);
        showMessage("Error saving product. Please try again.", "error");
      } finally {
        isSaving.value = false;
      }
    };

    const openBulkUploadModal = () => {
      showBulkUploadModal.value = true;
    };

    const closeBulkUploadModal = () => {
      showBulkUploadModal.value = false;
      selectedFile.value = null;
      isDragOver.value = false;
    };

    const handleImageUpload = (event) => {
      const file = event.target.files[0];
      if (file) {
        productForm.value.image = file;
      }
    };

    const handleFileSelect = (event) => {
      const file = event.target.files[0];
      if (file) {
        selectedFile.value = file;
      }
    };

    const handleFileDrop = (event) => {
      isDragOver.value = false;
      const file = event.dataTransfer.files[0];
      if (file) {
        selectedFile.value = file;
      }
    };

    const removeSelectedFile = () => {
      selectedFile.value = null;
    };

    const formatFileSize = (bytes) => {
      if (bytes === 0) return "0 Bytes";
      const k = 1024;
      const sizes = ["Bytes", "KB", "MB", "GB"];
      const i = Math.floor(Math.log(bytes) / Math.log(k));
      return parseFloat((bytes / Math.pow(k, i)).toFixed(2)) + " " + sizes[i];
    };

    const downloadTemplate = () => {
      // Create CSV template
      const csvContent =
        "Name,Category,Brand,Description,Price,Quantity,Size\n" +
        "Sample Product,Tyres,Michelin,Sample description,99.99,100,175/70/13";

      const blob = new Blob([csvContent], { type: "text/csv" });
      const url = window.URL.createObjectURL(blob);
      const a = document.createElement("a");
      a.href = url;
      a.download = "product_template.csv";
      a.click();
      window.URL.revokeObjectURL(url);
    };

    const processUpload = async () => {
      if (!selectedFile.value) return;

      isProcessing.value = true;
      try {
        // Create form data for file upload
        const formData = new FormData();
        formData.append("file", selectedFile.value);

        // Send to the bulk upload endpoint
        const response = await axios.post(
          `${apiBaseUrl}/api/Product/bulk-upload`,
          formData,
          {
            headers: {
              "Content-Type": "multipart/form-data",
            },
          }
        );

        // Handle response
        if (response.data.jobId) {
          // For background processing
          showMessage(
            `File queued for processing. Job ID: ${response.data.jobId}`,
            "info"
          );
          // You could implement polling for job status here
        } else {
          // For immediate processing
          const result = response.data;
          showMessage(
            `Successfully processed ${result.successfulRecords} of ${result.totalRecords} products!`,
            "success"
          );

          if (result.failedRecords > 0) {
            console.error("Failed records:", result.errors);
            showMessage(
              `${result.failedRecords} records failed to import. Check console for details.`,
              "warning"
            );
          }
        }

        closeBulkUploadModal();
        loadProducts(1); // Reload products
      } catch (error) {
        console.error("Error processing upload:", error);
        showMessage("Error processing file. Please try again.", "error");
      } finally {
        isProcessing.value = false;
      }
    };

    const clearAllFilters = () => {
      filters.value = {
        category: "",
        status: "",
        stockLevel: "",
        search: "",
      };
      loadProducts(1);
    };

    const exportProducts = () => {
      if (selectedProducts.value.length === 0) return;

      // Filter the products to only include selected ones
      const productsToExport = products.value.filter((p) =>
        selectedProducts.value.includes(p.id)
      );

      // Convert to CSV
      let csvContent = "Name,Category,Brand,Description,Price,Quantity,Size\n";

      productsToExport.forEach((p) => {
        csvContent += `"${p.name}","${p.category}","${p.brand || ""}","${
          p.description || ""
        }",${p.price},${p.quantity},"${p.size || ""}"\n`;
      });

      // Create and download the file
      const blob = new Blob([csvContent], { type: "text/csv" });
      const url = window.URL.createObjectURL(blob);
      const a = document.createElement("a");
      a.href = url;
      a.download = "exported_products.csv";
      a.click();
      window.URL.revokeObjectURL(url);

      showMessage(
        `Exported ${selectedProducts.value.length} products successfully!`,
        "success"
      );
    };

    const openBulkStockModal = () => {
      if (selectedProducts.value.length === 0) return;
      showMessage("Bulk stock update feature coming soon!", "info");
    };

    const duplicateProduct = async (product) => {
      try {
        // Create a new product based on the existing one
        const newProduct = {
          name: `${product.name} (Copy)`,
          description: product.description,
          brand: product.brand,
          category: product.category,
          size: product.size,
          price: product.price,
          quantity: product.quantity,
        };

        // Send to API
        await axios.post(`${apiBaseUrl}/api/Product`, newProduct);

        showMessage(`Duplicated product ${product.name}!`, "success");
        loadProducts(currentPage.value);
      } catch (error) {
        console.error("Error duplicating product:", error);
        showMessage("Error duplicating product. Please try again.", "error");
      }
    };

    const updateStock = async (product) => {
      // For simplicity, we'll just prompt for a new quantity
      const newQuantity = prompt(
        `Enter new stock quantity for ${product.name}:`,
        product.quantity
      );

      if (newQuantity === null) return; // User cancelled

      const quantity = parseInt(newQuantity);
      if (isNaN(quantity) || quantity < 0) {
        showMessage("Please enter a valid quantity", "error");
        return;
      }

      try {
        // Update the product with new quantity
        await axios.put(`${apiBaseUrl}/api/Product/${product.id}`, {
          ...product,
          quantity: quantity,
        });

        showMessage(
          `Updated stock for ${product.name} to ${quantity}!`,
          "success"
        );
        loadProducts(currentPage.value);
      } catch (error) {
        console.error("Error updating stock:", error);
        showMessage("Error updating stock. Please try again.", "error");
      }
    };

    const toggleProductStatus = async (product) => {
      try {
        // In this implementation, we'll toggle by setting quantity to 0 or 10
        const newQuantity = product.quantity > 0 ? 0 : 10;

        await axios.put(`${apiBaseUrl}/api/Product/${product.id}`, {
          ...product,
          quantity: newQuantity,
        });

        const newStatus = newQuantity > 0 ? "Active" : "Inactive";
        showMessage(
          `${product.name} has been ${newStatus.toLowerCase()}!`,
          "success"
        );

        loadProducts(currentPage.value);
      } catch (error) {
        console.error("Error toggling product status:", error);
        showMessage(
          "Error updating product status. Please try again.",
          "error"
        );
      }
    };

    const deleteProduct = async (product) => {
      if (
        confirm(
          `Are you sure you want to delete ${product.name}? This action cannot be undone.`
        )
      ) {
        try {
          await axios.delete(`${apiBaseUrl}/api/Product/${product.id}`);

          showMessage(`${product.name} has been deleted!`, "success");
          loadProducts(currentPage.value);
        } catch (error) {
          console.error("Error deleting product:", error);
          showMessage("Error deleting product. Please try again.", "error");
        }
      }
    };

    const sortColumn = (column) => {
      // Toggle sort direction if clicking the same column
      if (sortBy.value.startsWith(column)) {
        sortBy.value = sortBy.value.endsWith("-asc")
          ? `${column}-desc`
          : `${column}-asc`;
      } else {
        sortBy.value = `${column}-asc`;
      }

      loadProducts(1);
    };

    const getCategoryIcon = (category) => {
      const icons = {
        Tyres: "fas fa-circle",
        Batteries: "fas fa-battery-full",
        Oil: "fas fa-tint",
        Parts: "fas fa-cog",
        Accessories: "fas fa-wrench",
      };
      return icons[category] || "fas fa-box";
    };

    const getProductStatus = (product) => {
      if (!product) return "";

      // Status now comes directly from the backend as "Active" or "Inactive"
      return product.status?.toLowerCase() || "inactive";
    };

    const getStatusIcon = (status) => {
      const icons = {
        active: "fas fa-check-circle",
        inactive: "fas fa-pause-circle",
      };
      return icons[status] || "fas fa-circle";
    };

    const getStockClass = (stock) => {
      if (stock < 10) return "stock-low";
      if (stock < 50) return "stock-medium";
      return "stock-high";
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

    const formatDate = (dateString) => {
      if (!dateString) return "N/A";
      return new Date(dateString).toLocaleDateString("en-US", {
        year: "numeric",
        month: "short",
        day: "numeric",
      });
    };

    const formatTime = (dateString) => {
      if (!dateString) return "";
      return new Date(dateString).toLocaleTimeString("en-US", {
        hour: "2-digit",
        minute: "2-digit",
      });
    };

    // Debounced search function to prevent too many API calls
    const debouncedSearch = debounce(() => {
      loadProducts(1);
    }, 500);

    // Load products on component mount
    onMounted(() => {
      loadProducts(1);
      productStore.initSignalR();
    });

    return {
      // State
      products,
      selectedProduct,
      editingProduct,
      isLoading,
      isSaving,
      isProcessing,
      currentPage,
      totalPages,
      totalItems,
      pageSize,
      viewMode,
      sortBy,
      showDetailsModal,
      showAddEditModal,
      showBulkUploadModal,
      messages,
      selectedProducts,
      selectAll,
      isSidebarCollapsed,
      selectedFile,
      isDragOver,
      filters,
      productForm,
      paginationInfo,
      displayedPages,
      totalProducts,
      activeProducts,
      lowStockCount,
      totalCategories,

      // Methods
      toggleSidebar,
      loadProducts,
      toggleSelectAll,
      viewProductDetails,
      closeDetailsModal,
      openAddProductModal,
      editProduct,
      closeAddEditModal,
      saveProduct,
      openBulkUploadModal,
      closeBulkUploadModal,
      handleImageUpload,
      handleFileSelect,
      handleFileDrop,
      removeSelectedFile,
      formatFileSize,
      downloadTemplate,
      processUpload,
      clearAllFilters,
      exportProducts,
      openBulkStockModal,
      duplicateProduct,
      updateStock,
      toggleProductStatus,
      deleteProduct,
      sortColumn,
      getCategoryIcon,
      getProductStatus,
      getStatusIcon,
      getStockClass,
      showMessage,
      removeMessage,
      formatCurrency,
      formatDate,
      formatTime,
      debouncedSearch,
      imageBase,
    };
  },
};
</script>

<style scoped>
/* Import all base styles and add product-specific styles */

/* Base Desktop Layout */
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

.total-products-stat::before {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
}

.active-products-stat::before {
  background: linear-gradient(135deg, #2ecc71 0%, #27ae60 100%);
}

.low-stock-stat::before {
  background: linear-gradient(135deg, #f39c12 0%, #e67e22 100%);
}

.inactive-products-stat::before {
  background: linear-gradient(135deg, #6b7280 0%, #dc2626 100%);
}
.categories-stat::before {
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

.total-products-stat .stat-icon-container {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
}

.active-products-stat .stat-icon-container {
  background: linear-gradient(135deg, #2ecc71 0%, #27ae60 100%);
}

.low-stock-stat .stat-icon-container {
  background: linear-gradient(135deg, #f39c12 0%, #e67e22 100%);
}

.inactive-products-stat .stat-icon-container {
  background: linear-gradient(135deg, #6b7280 0%, #dc2626 100%);
}

.categories-stat .stat-icon-container {
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

.stat-trend.attention {
  color: #991515;
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
.product-actions-panel {
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

/* Product Actions Grid */
.product-actions-grid {
  display: grid;
  grid-template-columns: repeat(2, 1fr);
  gap: 0.9375rem;
}

.product-action-btn {
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

.product-action-btn:hover:not(:disabled) {
  transform: translateY(-0.1875rem);
  box-shadow: 0 0.5rem 1.5625rem rgba(0, 0, 0, 0.2);
}

.product-action-btn:disabled {
  opacity: 0.5;
  cursor: not-allowed;
  transform: none;
}

.add-product {
  background: linear-gradient(135deg, #2ecc71 0%, #27ae60 100%);
}

.bulk-upload {
  background: linear-gradient(135deg, #3498db 0%, #2980b9 100%);
}

.export-products {
  background: linear-gradient(135deg, #f39c12 0%, #e67e22 100%);
}

.update-stock {
  background: linear-gradient(135deg, #9b59b6 0%, #8e44ad 100%);
}

.product-action-btn i {
  font-size: 1.2rem;
}

.product-action-btn span {
  font-size: 0.9rem;
}

/* Products Table Section */
.products-table-section {
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

.products-data-table {
  width: 100%;
  border-collapse: collapse;
  background: white;
}

.table-header-section {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
}

.products-data-table th {
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

.products-data-table td {
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

/* Product Info Display */
.product-info-display {
  display: flex;
  align-items: center;
  gap: 0.75rem;
}

.product-image-container {
  width: 3.5rem;
  height: 3.5rem;
  border-radius: 0.5rem;
  overflow: hidden;
  flex-shrink: 0;
}

.product-image {
  width: 100%;
  height: 100%;
  object-fit: cover;
}

.product-image-placeholder {
  width: 100%;
  height: 100%;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  display: flex;
  align-items: center;
  justify-content: center;
  color: white;
  font-size: 1.2rem;
}

.product-details-container {
  display: flex;
  flex-direction: column;
  gap: 0.25rem;
}

.product-name-text {
  font-weight: 600;
  color: #2c3e50;
  font-size: 0.95rem;
}

.product-description-text {
  font-size: 0.8rem;
  color: #7f8c8d;
  max-width: 200px;
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
}

/* Category Badge */
.category-badge {
  display: inline-flex;
  align-items: center;
  gap: 0.375rem;
  padding: 0.375rem 0.75rem;
  border-radius: 1rem;
  font-size: 0.8rem;
  font-weight: 600;
  text-transform: uppercase;
  letter-spacing: 0.03125rem;
}

.category-tyres {
  background: #e8f5e8;
  color: #27ae60;
}

.category-batteries {
  background: #fff3cd;
  color: #e67e22;
}

.category-oil {
  background: #e3f2fd;
  color: #2980b9;
}

.category-parts {
  background: #f3e5f5;
  color: #8e44ad;
}

.category-accessories {
  background: #fce4ec;
  color: #e91e63;
}

/* SKU Display */
.sku-display {
  font-family: "Courier New", monospace;
  font-weight: 600;
  color: #2c3e50;
  font-size: 0.9rem;
}

/* Price Display */
.price-display-container {
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

.price-value {
  font-size: 1.1rem;
}

/* Stock Display */
.stock-display {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 0.125rem;
}

.stock-number {
  font-size: 1.2rem;
  font-weight: 700;
}

.stock-number.stock-low {
  color: #e74c3c;
}

.stock-number.stock-medium {
  color: #f39c12;
}

.stock-number.stock-high {
  color: #27ae60;
}

.stock-label {
  font-size: 0.8rem;
  color: #7f8c8d;
}

/* Product Status Badge */
.product-status-badge {
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

.status-outofstock {
  background: linear-gradient(135deg, #e74c3c 0%, #c0392b 100%);
  color: white;
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

.products-cards-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(20rem, 1fr));
  gap: 1.25rem;
}

.product-card-item {
  background: white;
  border-radius: 0.75rem;
  box-shadow: 0 0.25rem 1.25rem rgba(0, 0, 0, 0.08);
  border: 1px solid #e9ecef;
  transition: all 0.3s ease;
  overflow: hidden;
}

.product-card-item:hover {
  transform: translateY(-0.25rem);
  box-shadow: 0 0.5rem 1.875rem rgba(0, 0, 0, 0.12);
}

.product-card-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 1rem 1.25rem;
  border-bottom: 1px solid #f1f3f4;
  background: #f8f9fa;
}

.product-card-image {
  display: flex;
  align-items: center;
  gap: 0.75rem;
}

.card-checkbox {
  width: 1rem;
  height: 1rem;
  accent-color: #667eea;
}

.image-container {
  width: 3rem;
  height: 3rem;
  border-radius: 0.5rem;
  overflow: hidden;
}

.card-product-image {
  width: 100%;
  height: 100%;
  object-fit: cover;
}

.card-image-placeholder {
  width: 100%;
  height: 100%;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  display: flex;
  align-items: center;
  justify-content: center;
  color: white;
  font-size: 1rem;
}

.product-card-body {
  padding: 1.25rem;
}

.product-name-card {
  font-size: 1.1rem;
  font-weight: 600;
  color: #2c3e50;
  margin-bottom: 0.5rem;
}

.product-category-card {
  margin-bottom: 0.75rem;
}

.product-description-card {
  font-size: 0.9rem;
  color: #7f8c8d;
  margin-bottom: 1rem;
  line-height: 1.4;
}

.product-stats-card {
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

.stat-value.price {
  color: #27ae60;
  font-size: 1.1rem;
}

.product-card-footer {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 1rem 1.25rem;
  border-top: 1px solid #f1f3f4;
  background: #f8f9fa;
}

.updated-date-card {
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

.product-details-modal {
  max-width: 60rem;
}

.add-edit-modal {
  max-width: 45rem;
}

.bulk-upload-modal {
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

/* Product Image Section */
.product-image-section {
  background: #f8f9fa;
  border-radius: 0.75rem;
  padding: 1.5rem;
  margin-bottom: 1.5rem;
  display: flex;
  align-items: center;
  gap: 1.25rem;
}

.main-image-container {
  width: 6rem;
  height: 6rem;
  border-radius: 0.75rem;
  overflow: hidden;
  flex-shrink: 0;
}

.main-product-image {
  width: 100%;
  height: 100%;
  object-fit: cover;
}

.main-image-placeholder {
  width: 100%;
  height: 100%;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  display: flex;
  align-items: center;
  justify-content: center;
  color: white;
  font-size: 1.8rem;
}

.product-basic-info {
  flex: 1;
}

.product-title {
  font-size: 1.5rem;
  font-weight: 700;
  color: #2c3e50;
  margin-bottom: 0.5rem;
}

.product-meta {
  display: flex;
  flex-direction: column;
  gap: 0.25rem;
  margin-top: 0.5rem;
}

.product-sku,
.product-category {
  font-size: 0.9rem;
  color: #7f8c8d;
  font-weight: 500;
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
}

.price-highlight {
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

.edit-product-action {
  background: linear-gradient(135deg, #f39c12 0%, #e67e22 100%);
  color: white;
}

.stock-action {
  background: linear-gradient(135deg, #9b59b6 0%, #8e44ad 100%);
  color: white;
}

.duplicate-action {
  background: linear-gradient(135deg, #3498db 0%, #2980b9 100%);
  color: white;
}

.modal-action-button:hover {
  transform: translateY(-0.125rem);
  box-shadow: 0 0.5rem 1.5625rem rgba(0, 0, 0, 0.2);
}

/* Product Form */
.product-form {
  display: flex;
  flex-direction: column;
  gap: 1.5rem;
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

.form-label {
  font-weight: 600;
  color: #2c3e50;
  font-size: 0.9rem;
}

.form-input,
.form-select,
.form-textarea {
  padding: 0.75rem;
  border: 2px solid #e9ecef;
  border-radius: 0.5rem;
  font-size: 1rem;
  transition: all 0.3s ease;
  background: white;
}

.form-input:focus,
.form-select:focus,
.form-textarea:focus {
  outline: none;
  border-color: #667eea;
  box-shadow: 0 0 0 3px rgba(102, 126, 234, 0.1);
}

.form-textarea {
  resize: vertical;
  font-family: inherit;
}

.form-actions {
  display: flex;
  justify-content: flex-end;
  gap: 1rem;
  padding-top: 1.5rem;
  border-top: 2px solid #f1f3f4;
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

/* Bulk Upload Modal */
.bulk-upload-container {
  display: flex;
  flex-direction: column;
  gap: 1.5rem;
}

.upload-instructions {
  background: #f8f9fa;
  border-radius: 0.75rem;
  padding: 1.25rem;
}

.instructions-title {
  font-size: 1.1rem;
  font-weight: 700;
  color: #2c3e50;
  margin-bottom: 1rem;
}

.instructions-list {
  margin: 0;
  padding-left: 1.25rem;
  color: #7f8c8d;
}

.instructions-list li {
  margin-bottom: 0.5rem;
  line-height: 1.4;
}

.template-download {
  text-align: center;
}

.template-btn {
  background: linear-gradient(135deg, #3498db 0%, #2980b9 100%);
  color: white;
  border: none;
  border-radius: 0.5rem;
  padding: 0.75rem 1.5rem;
  cursor: pointer;
  transition: all 0.3s ease;
  font-weight: 600;
  display: inline-flex;
  align-items: center;
  gap: 0.5rem;
}

.template-btn:hover {
  transform: translateY(-2px);
  box-shadow: 0 4px 12px rgba(52, 152, 219, 0.4);
}

.file-upload-section {
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

.upload-area {
  border: 2px dashed #e9ecef;
  border-radius: 0.75rem;
  padding: 2rem;
  text-align: center;
  transition: all 0.3s ease;
  position: relative;
  background: #f8f9fa;
}

.upload-area.drag-over {
  border-color: #667eea;
  background: rgba(102, 126, 234, 0.05);
}

.file-input {
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  opacity: 0;
  cursor: pointer;
}

.upload-content {
  pointer-events: none;
}

.upload-icon {
  font-size: 3rem;
  color: #bdc3c7;
  margin-bottom: 1rem;
}

.upload-text {
  font-size: 1.1rem;
  color: #2c3e50;
  margin-bottom: 0.5rem;
}

.browse-btn {
  background: none;
  border: none;
  color: #667eea;
  font-weight: 600;
  cursor: pointer;
  text-decoration: underline;
  pointer-events: all;
}

.upload-hint {
  font-size: 0.9rem;
  color: #7f8c8d;
  margin: 0;
}

.selected-file {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 1rem;
  background: white;
  border: 1px solid #e9ecef;
  border-radius: 0.5rem;
}

.file-info {
  display: flex;
  align-items: center;
  gap: 0.75rem;
}

.file-info i {
  font-size: 1.5rem;
  color: #27ae60;
}

.file-name {
  font-weight: 600;
  color: #2c3e50;
}

.file-size {
  color: #7f8c8d;
  font-size: 0.9rem;
}

.remove-file-btn {
  background: #e74c3c;
  color: white;
  border: none;
  border-radius: 50%;
  width: 2rem;
  height: 2rem;
  cursor: pointer;
  transition: all 0.3s ease;
  display: flex;
  align-items: center;
  justify-content: center;
}

.remove-file-btn:hover {
  background: #c0392b;
  transform: scale(1.1);
}

.upload-actions {
  display: flex;
  justify-content: flex-end;
  gap: 1rem;
  padding-top: 1.5rem;
  border-top: 2px solid #f1f3f4;
}

.upload-btn {
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

.upload-btn.cancel-btn {
  background: #95a5a6;
  color: white;
}

.upload-btn.cancel-btn:hover {
  background: #7f8c8d;
  transform: translateY(-2px);
}

.upload-btn.process-btn {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
}

.upload-btn.process-btn:hover:not(:disabled) {
  transform: translateY(-2px);
  box-shadow: 0 4px 12px rgba(102, 126, 234, 0.4);
}

.upload-btn:disabled {
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

  .product-actions-grid {
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

  .products-cards-grid {
    grid-template-columns: 1fr;
  }

  .product-image-section {
    flex-direction: column;
    text-align: center;
  }

  .upload-actions {
    flex-direction: column;
  }

  .status-details {
    margin-top: 6px;
    display: flex;
    flex-direction: column;
    gap: 2px;
  }

  .status-details small {
    background: linear-gradient(135deg, #000000, #000000);
    color: #000000;
    padding: 3px 8px;
    border-radius: 8px;
    font-size: 0.65rem;
    font-weight: 500;
    border: 1px solid #0c0c0c;
  }

  /* Add these styles to your CSS */
  /* .status-details {
    margin-top: 4px;
    font-size: 0.7rem;
    color: #6b7280;
    line-height: 1.2;
  }

  .status-details small {
    display: block;
    background-color: black;
    padding: 2px 6px;
    border-radius: 4px;
    margin-bottom: 2px;
  } */

  .order-activity {
    margin-top: 4px;
  }

  .order-activity small {
    display: block;
    font-size: 0.7rem;
    color: #6b7280;
  }

  .stat-card.inactive-products-stat .stat-icon-container {
    background-color: #fef3c7;
    color: #d97706;
  }

  .product-status-badge.active {
    background-color: #d1fae5;
    color: #065f46;
  }

  .product-status-badge.inactive {
    background-color: #fee2e2;
    color: #991b1b;
  }

  .status-active {
    color: #059669 !important;
    font-weight: 600;
  }

  .status-inactive {
    color: #dc2626 !important;
    font-weight: 600;
  }

  /* Enhanced status badge styles */
  .product-status-badge {
    display: inline-flex;
    align-items: center;
    gap: 6px;
    padding: 6px 12px;
    border-radius: 16px;
    font-size: 0.75rem;
    font-weight: 700;
    text-transform: uppercase;
    letter-spacing: 0.05em;
    box-shadow: 0 1px 2px rgba(0, 0, 0, 0.1);
    transition: all 0.2s ease;
  }

  /* Active status - Green theme */
  .product-status-badge.status-active {
    background: linear-gradient(135deg, #10b981, #059669);
    color: white;
    border: 1px solid #047857;
  }

  .product-status-badge.status-active:hover {
    transform: translateY(-1px);
    box-shadow: 0 2px 4px rgba(16, 185, 129, 0.3);
  }

  /* Inactive status - Red theme */
  .product-status-badge.status-inactive {
    background: linear-gradient(135deg, #ef4444, #dc2626);
    color: white;
    border: 1px solid #b91c1c;
  }

  .product-status-badge.status-inactive:hover {
    transform: translateY(-1px);
    box-shadow: 0 2px 4px rgba(239, 68, 68, 0.3);
  }

  /* Order activity styling for cards */
  .order-activity {
    margin-top: 6px;
    display: flex;
    flex-direction: column;
    gap: 2px;
  }

  .order-activity small {
    background-color: rgba(59, 130, 246, 0.1);
    color: #1e40af;
    padding: 2px 6px;
    border-radius: 6px;
    font-size: 0.65rem;
    font-weight: 500;
  }
}

@media (max-width: 480px) {
  .products-data-table {
    font-size: 0.85rem;
  }

  .products-data-table th,
  .products-data-table td {
    padding: 0.75rem 0.5rem;
  }
}
</style>
