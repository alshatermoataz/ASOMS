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
            <router-link to="/admin/analytics" class="nav-item active"
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
            <router-link to="/admin/analytics" class="nav-item active"
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
              <i class="fas fa-chart-line navbar-brand-icon"></i>
              <span class="navbar-brand-text">Analytics Dashboard</span>
            </div>
          </div>
          <div class="navbar-right-main">
            <!-- <button class="notification-btn">
              <i class="fas fa-bell"></i>
              <span class="notification-badge">2</span>
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
        <!-- Analytics Controls -->
        <section class="analytics-controls-section">
          <div class="controls-header">
            <h2 class="controls-title">
              <i class="fas fa-filter"></i>
              Analytics Filters
            </h2>
            <div class="date-range-selector">
              <select
                v-model="selectedPeriod"
                @change="updateAnalytics"
                class="period-select"
              >
                <option value="7d">Last 7 Days</option>
                <option value="30d">Last 30 Days</option>
                <option value="90d">Last 90 Days</option>
                <option value="1y">Last Year</option>
              </select>
              <button
                class="refresh-btn"
                @click="refreshData"
                :disabled="isLoading"
              >
                <i
                  class="fas fa-sync-alt"
                  :class="{ 'fa-spin': isLoading }"
                ></i>
                {{ isLoading ? "Loading..." : "Refresh" }}
              </button>
            </div>
          </div>
        </section>

        <!-- Key Metrics Overview -->
        <section class="key-metrics-section">
          <div class="metrics-grid">
            <div class="metric-card revenue-metric">
              <div class="metric-header">
                <div class="metric-icon">
                  <i class="fas fa-dollar-sign"></i>
                </div>
                <div
                  class="metric-trend"
                  :class="revenueGrowth >= 0 ? 'positive' : 'negative'"
                >
                  <i
                    :class="
                      revenueGrowth >= 0
                        ? 'fas fa-arrow-up'
                        : 'fas fa-arrow-down'
                    "
                  ></i>
                  <span
                    >{{ revenueGrowth >= 0 ? "+" : ""
                    }}{{ revenueGrowth }}%</span
                  >
                </div>
              </div>
              <div class="metric-content">
                <div class="metric-value">
                  ${{ formatCurrency(totalRevenue) }}
                </div>
                <div class="metric-label">Total Revenue</div>
                <div class="metric-subtitle">
                  {{
                    selectedPeriod === "7d"
                      ? "Last 7 days"
                      : selectedPeriod === "30d"
                      ? "Last 30 days"
                      : selectedPeriod === "90d"
                      ? "Last 90 days"
                      : "Last year"
                  }}
                </div>
              </div>
            </div>

            <div class="metric-card orders-metric">
              <div class="metric-header">
                <div class="metric-icon">
                  <i class="fas fa-shopping-cart"></i>
                </div>
                <div
                  class="metric-trend"
                  :class="ordersGrowth >= 0 ? 'positive' : 'negative'"
                >
                  <i
                    :class="
                      ordersGrowth >= 0
                        ? 'fas fa-arrow-up'
                        : 'fas fa-arrow-down'
                    "
                  ></i>
                  <span
                    >{{ ordersGrowth >= 0 ? "+" : "" }}{{ ordersGrowth }}%</span
                  >
                </div>
              </div>
              <div class="metric-content">
                <div class="metric-value">{{ totalOrders }}</div>
                <div class="metric-label">Total Orders</div>
                <div class="metric-subtitle">
                  {{ avgOrderValue }} avg. order value
                </div>
              </div>
            </div>

            <div class="metric-card customers-metric">
              <div class="metric-header">
                <div class="metric-icon">
                  <i class="fas fa-users"></i>
                </div>
                <div
                  class="metric-trend"
                  :class="customerGrowth >= 0 ? 'positive' : 'negative'"
                >
                  <i
                    :class="
                      customerGrowth >= 0
                        ? 'fas fa-arrow-up'
                        : 'fas fa-arrow-down'
                    "
                  ></i>
                  <span
                    >{{ customerGrowth >= 0 ? "+" : ""
                    }}{{ customerGrowth }}%</span
                  >
                </div>
              </div>
              <div class="metric-content">
                <div class="metric-value">{{ totalCustomers }}</div>
                <div class="metric-label">Active Customers</div>
                <div class="metric-subtitle">
                  {{ newCustomers }} new Customers
                  {{
                    selectedPeriod === "7d"
                      ? "Last 7 days"
                      : selectedPeriod === "30d"
                      ? "Last 30 days"
                      : selectedPeriod === "90d"
                      ? "Last 90 days"
                      : "Last year"
                  }}
                </div>
              </div>
            </div>

            <div class="metric-card conversion-metric">
              <div class="metric-header">
                <div class="metric-icon">
                  <i class="fas fa-percentage"></i>
                </div>
                <div class="metric-trend positive">
                  <i class="fas fa-chart-line"></i>
                  <span>{{ conversionRate }}%</span>
                </div>
              </div>
              <div class="metric-content">
                <div class="metric-value">{{ conversionRate }}%</div>
                <div class="metric-label">Conversion Rate</div>
                <div class="metric-subtitle">Customers to Orders</div>
              </div>
            </div>
          </div>
        </section>

        <!-- Charts Section -->
        <section class="charts-section">
          <div class="charts-grid">
            <!-- Revenue Chart -->
            <div class="chart-card revenue-chart-card">
              <div class="chart-header">
                <h3 class="chart-title">
                  <i class="fas fa-chart-line"></i>
                  Revenue Trend
                </h3>
                <div v-if="false" class="chart-controls">
                  <button
                    v-for="period in ['Daily', 'Weekly', 'Monthly']"
                    :key="period"
                    :class="[
                      'chart-period-btn',
                      { active: revenuePeriod === period },
                    ]"
                    @click="revenuePeriod = period"
                  >
                    {{ period }}
                  </button>
                </div>
              </div>
              <div class="chart-container">
                <Line
                  v-if="revenueChartData.labels.length > 0"
                  :data="revenueChartData"
                  :options="revenueChartOptions"
                />
                <div v-else class="empty-chart">
                  <i class="fas fa-chart-line"></i>
                  <p>No revenue data available</p>
                </div>
              </div>
            </div>

            <!-- Orders Chart -->
            <div class="chart-card orders-chart-card">
              <div class="chart-header">
                <h3 class="chart-title">
                  <i class="fas fa-shopping-bag"></i>
                  Orders Overview
                </h3>
              </div>
              <div class="chart-container">
                <Bar
                  v-if="ordersChartData.labels.length > 0"
                  :data="ordersChartData"
                  :options="ordersChartOptions"
                />
                <div v-else class="empty-chart">
                  <i class="fas fa-shopping-cart"></i>
                  <p>No orders data available</p>
                </div>
              </div>
            </div>
          </div>
        </section>

        <!-- Analytics Grid -->
        <section class="analytics-grid-section">
          <div class="analytics-grid">
            <!-- Top Products -->
            <div class="analytics-card top-products-card">
              <div class="card-header">
                <h3 class="card-title">
                  <i class="fas fa-trophy"></i>
                  Top Selling Products
                </h3>
                <button class="view-all-btn">View All</button>
              </div>
              <div class="card-content">
                <div v-if="topProducts.length > 0" class="products-list">
                  <div
                    v-for="(product, index) in topProducts"
                    :key="product.id"
                    class="product-item"
                  >
                    <div class="product-rank">{{ index + 1 }}</div>
                    <div class="product-image">
                      <img
                        v-if="product.image"
                        :src="imageBase(product.image)"
                        :alt="product.name"
                        class="product-img"
                      />
                      <div v-else class="product-placeholder">
                        <i class="fas fa-box"></i>
                      </div>
                    </div>
                    <div class="product-details">
                      <div class="product-name">{{ product.name }}</div>
                      <div class="product-category">{{ product.category }}</div>
                    </div>
                    <div class="product-stats">
                      <div class="product-sales">
                        ${{ formatCurrency(product.sales) }}
                      </div>
                      <div class="product-units">{{ product.units }} units</div>
                    </div>
                  </div>
                </div>
                <div v-else class="empty-state">
                  <div class="empty-icon">
                    <i class="fas fa-box-open"></i>
                  </div>
                  <p>No product data available</p>
                  <small>Data will appear once orders are processed</small>
                </div>
              </div>
            </div>

            <!-- Customer Segments -->
            <div class="analytics-card customer-segments-card">
              <div class="card-header">
                <h3 class="card-title">
                  <i class="fas fa-users-cog"></i>
                  Customer Segments (Active vs Inactive)
                </h3>
              </div>
              <div class="card-content">
                <div v-if="customerSegments.length > 0">
                  <div class="segments-chart-container">
                    <Doughnut
                      v-if="customerSegmentsChartData.labels.length > 0"
                      :data="customerSegmentsChartData"
                      :options="segmentsChartOptions"
                    />
                    <div v-else class="empty-chart">
                      <i class="fas fa-users"></i>
                      <p>No customer data available</p>
                    </div>
                  </div>
                  <div class="segments-legend">
                    <div
                      v-for="segment in customerSegments"
                      :key="segment.name"
                      class="legend-item"
                    >
                      <div
                        class="legend-color"
                        :style="{ backgroundColor: segment.color }"
                      ></div>
                      <div class="legend-details">
                        <div class="legend-name">{{ segment.name }}</div>
                        <div class="legend-value">
                          {{ segment.percentage }}% ({{ segment.count }})
                        </div>
                      </div>
                    </div>
                  </div>
                </div>
                <div v-else class="empty-state">
                  <div class="empty-icon">
                    <i class="fas fa-users"></i>
                  </div>
                  <p>No customer segment data available</p>
                  <small
                    >Customer segments will appear once you have registered
                    customers</small
                  >
                </div>
              </div>
            </div>

            <!-- Sales by Category -->
            <div class="analytics-card category-sales-card">
              <div class="card-header">
                <h3 class="card-title">
                  <i class="fas fa-tags"></i>
                  Sales by Category
                </h3>
              </div>
              <div class="card-content">
                <div v-if="categorySales.length > 0" class="category-list">
                  <div
                    v-for="category in categorySales"
                    :key="category.name"
                    class="category-item"
                  >
                    <div class="category-info">
                      <div
                        class="category-icon"
                        :class="`category-${category.name.toLowerCase()}`"
                      >
                        <i :class="getCategoryIcon(category.name)"></i>
                      </div>
                      <div class="category-details">
                        <div class="category-name">{{ category.name }}</div>
                        <div
                          class="category-growth"
                          :class="
                            category.growth >= 0 ? 'positive' : 'negative'
                          "
                        >
                          <i
                            :class="
                              category.growth >= 0
                                ? 'fas fa-arrow-up'
                                : 'fas fa-arrow-down'
                            "
                          ></i>
                          {{ Math.abs(category.growth) }}%
                        </div>
                      </div>
                    </div>
                    <div class="category-stats">
                      <div class="category-revenue">
                        ${{ formatCurrency(category.revenue) }}
                      </div>
                      <div class="category-percentage">
                        {{ category.percentage }}% of total
                      </div>
                    </div>
                    <div class="category-progress">
                      <div class="progress-bar">
                        <div
                          class="progress-fill"
                          :style="{
                            width: category.percentage + '%',
                            backgroundColor: category.color,
                          }"
                        ></div>
                      </div>
                    </div>
                  </div>
                </div>
                <div v-else class="empty-state">
                  <div class="empty-icon">
                    <i class="fas fa-tags"></i>
                  </div>
                  <p>No category sales data available</p>
                  <small>Sales data will appear once orders are placed</small>
                </div>
              </div>
            </div>

            <!-- Recent Activity -->
            <div v-if="false" class="analytics-card recent-activity-card">
              <div class="card-header">
                <h3 class="card-title">
                  <i class="fas fa-clock"></i>
                  Recent Activity
                </h3>
              </div>
              <div class="card-content">
                <div class="activity-list">
                  <div
                    v-for="activity in recentActivities"
                    :key="activity.id"
                    class="activity-item"
                  >
                    <div class="activity-icon" :class="activity.type">
                      <i :class="getActivityIcon(activity.type)"></i>
                    </div>
                    <div class="activity-details">
                      <div class="activity-text">{{ activity.text }}</div>
                      <div class="activity-time">
                        {{ formatTime(activity.timestamp) }}
                      </div>
                    </div>
                    <div class="activity-value" v-if="activity.value">
                      {{ activity.value }}
                    </div>
                  </div>
                </div>
              </div>
            </div>

            <!-- Performance Metrics -->
            <div v-if="false" class="analytics-card performance-metrics-card">
              <div class="card-header">
                <h3 class="card-title">
                  <i class="fas fa-tachometer-alt"></i>
                  Performance Metrics
                </h3>
              </div>
              <div class="card-content">
                <div class="metrics-list">
                  <div
                    v-for="metric in performanceMetrics"
                    :key="metric.name"
                    class="performance-metric"
                  >
                    <div class="metric-info">
                      <div class="metric-name">{{ metric.name }}</div>
                      <div class="metric-description">
                        {{ metric.description }}
                      </div>
                    </div>
                    <div class="metric-gauge">
                      <div class="gauge-container">
                        <div
                          class="gauge-fill"
                          :style="{
                            width: metric.value + '%',
                            backgroundColor: getGaugeColor(metric.value),
                          }"
                        ></div>
                      </div>
                      <div class="gauge-value">{{ metric.value }}%</div>
                    </div>
                  </div>
                </div>
              </div>
            </div>

            <!-- Geographic Sales -->
            <div v-if="false" class="analytics-card geographic-sales-card">
              <div class="card-header">
                <h3 class="card-title">
                  <i class="fas fa-map-marked-alt"></i>
                  Geographic Sales
                </h3>
              </div>
              <div class="card-content">
                <div class="geographic-list">
                  <div
                    v-for="location in geographicSales"
                    :key="location.name"
                    class="location-item"
                  >
                    <div class="location-info">
                      <div class="location-flag">{{ location.flag }}</div>
                      <div class="location-details">
                        <div class="location-name">{{ location.name }}</div>
                        <div class="location-orders">
                          {{ location.orders }} orders
                        </div>
                      </div>
                    </div>
                    <div class="location-stats">
                      <div class="location-revenue">
                        ${{ formatCurrency(location.revenue) }}
                      </div>
                      <div class="location-percentage">
                        {{ location.percentage }}%
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </section>

        <!-- Export Section -->
        <!-- <section class="export-section">
          <div class="export-card">
            <div class="export-header">
              <h3 class="export-title">
                <i class="fas fa-download"></i>
                Export Analytics Data
              </h3>
              <p class="export-description">
                Download comprehensive analytics reports in various formats
              </p>
            </div>
            <div class="export-actions">
              <button class="export-btn pdf-export" @click="exportData('pdf')">
                <i class="fas fa-file-pdf"></i>
                <span>Export PDF</span>
              </button>
              <button
                class="export-btn excel-export"
                @click="exportData('excel')"
              >
                <i class="fas fa-file-excel"></i>
                <span>Export Excel</span>
              </button>
              <button class="export-btn csv-export" @click="exportData('csv')">
                <i class="fas fa-file-csv"></i>
                <span>Export CSV</span>
              </button>
            </div>
          </div>
        </section> -->
      </main>
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
import { ref, onMounted, nextTick, computed } from "vue";
import axios from "axios";

import {
  Chart as ChartJS,
  CategoryScale,
  LinearScale,
  PointElement,
  LineElement,
  BarElement,
  Title,
  Tooltip,
  Legend,
  ArcElement,
} from "chart.js";
import { Line, Bar, Doughnut } from "vue-chartjs";

ChartJS.register(
  CategoryScale,
  LinearScale,
  PointElement,
  LineElement,
  BarElement,
  Title,
  Tooltip,
  Legend,
  ArcElement
);

const API_BASE = "https://asoms-production.up.railway.app";

export default {
  name: "AnalyticsPage",
  components: {
    Line,
    Bar,
    Doughnut,
  },
  setup() {
    const isSidebarCollapsed = ref(false);
    const isLoading = ref(false);
    const messages = ref([]);
    const selectedPeriod = ref("30d");
    const revenuePeriod = ref("Daily");

    // Analytics Data
    // Replace the existing static data with these reactive refs
    const totalRevenue = ref(0);
    const totalOrders = ref(0);
    const avgOrderValue = ref("$0.00");
    const totalCustomers = ref(0);
    const newCustomers = ref(0);
    const conversionRate = ref(0);
    const topProducts = ref([]);
    const customerSegments = ref([]);
    const categorySales = ref([]);

    // Add these new refs for growth indicators
    const revenueGrowth = ref(0);
    const ordersGrowth = ref(0);
    const customerGrowth = ref(0);

    const recentActivities = ref([]);

    const performanceMetrics = ref([]);

    const geographicSales = ref([]);

    // Chart references
    const revenueChart = ref(null);
    const ordersChart = ref(null);
    const customerSegmentsChart = ref(null);

    const revenueChartData = ref({
      labels: [],
      datasets: [
        {
          label: "Revenue",
          data: [],
          borderColor: "#667eea",
          backgroundColor: "rgba(102, 126, 234, 0.1)",
          tension: 0.4,
        },
      ],
    });

    const ordersChartData = ref({
      labels: [],
      datasets: [
        {
          label: "Orders",
          data: [],
          backgroundColor: "#2ecc71",
        },
      ],
    });

    const customerSegmentsChartData = ref({
      labels: [],
      datasets: [
        {
          data: [],
          backgroundColor: [],
        },
      ],
    });

    const toggleSidebar = () => {
      isSidebarCollapsed.value = !isSidebarCollapsed.value;
    };

    const updateAnalytics = async () => {
      await fetchAnalyticsData();
      await fetchChartData();
    };

    const refreshData = async () => {
      await updateAnalytics();
    };

    const imageBase = (url) => {
      if (!url) return null;
      return url.startsWith("http") ? url : `${API_BASE}/${url}`;
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

    const getActivityIcon = (type) => {
      const icons = {
        order: "fas fa-shopping-cart",
        customer: "fas fa-user-plus",
        product: "fas fa-exclamation-triangle",
      };
      return icons[type] || "fas fa-circle";
    };

    const getGaugeColor = (value) => {
      if (value >= 80) return "#2ecc71";
      if (value >= 60) return "#f39c12";
      return "#e74c3c";
    };

    const exportData = (format) => {
      showMessage(
        `Exporting analytics data as ${format.toUpperCase()}...`,
        "success"
      );
    };

    const formatCurrency = (amount) => {
      return Number.parseFloat(amount || 0).toLocaleString();
    };

    const formatTime = (timestamp) => {
      const now = new Date();
      const diff = now - timestamp;
      const minutes = Math.floor(diff / 60000);
      const hours = Math.floor(minutes / 60);
      const days = Math.floor(hours / 24);

      if (days > 0) return `${days}d ago`;
      if (hours > 0) return `${hours}h ago`;
      return `${minutes}m ago`;
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

    // Mock chart creation (in a real app, you'd use Chart.js or similar)
    const createCharts = () => {
      nextTick(() => {
        // Revenue Chart
        if (revenueChart.value) {
          const ctx = revenueChart.value.getContext("2d");
          ctx.fillStyle = "linear-gradient(135deg, #667eea 0%, #764ba2 100%)";
          ctx.fillRect(
            0,
            0,
            revenueChart.value.width,
            revenueChart.value.height
          );
          ctx.fillStyle = "white";
          ctx.font = "16px Arial";
          ctx.textAlign = "center";
          ctx.fillText(
            "Revenue Chart",
            revenueChart.value.width / 2,
            revenueChart.value.height / 2
          );
        }

        // Orders Chart
        if (ordersChart.value) {
          const ctx = ordersChart.value.getContext("2d");
          ctx.fillStyle = "linear-gradient(135deg, #2ecc71 0%, #27ae60 100%)";
          ctx.fillRect(0, 0, ordersChart.value.width, ordersChart.value.height);
          ctx.fillStyle = "white";
          ctx.font = "16px Arial";
          ctx.textAlign = "center";
          ctx.fillText(
            "Orders Chart",
            ordersChart.value.width / 2,
            ordersChart.value.height / 2
          );
        }

        // Customer Segments Chart (Pie Chart)
        if (customerSegmentsChart.value) {
          const ctx = customerSegmentsChart.value.getContext("2d");
          const centerX = customerSegmentsChart.value.width / 2;
          const centerY = customerSegmentsChart.value.height / 2;
          const radius = Math.min(centerX, centerY) - 10;

          let currentAngle = 0;
          customerSegments.value.forEach((segment) => {
            const sliceAngle = (segment.percentage / 100) * 2 * Math.PI;

            ctx.beginPath();
            ctx.moveTo(centerX, centerY);
            ctx.arc(
              centerX,
              centerY,
              radius,
              currentAngle,
              currentAngle + sliceAngle
            );
            ctx.closePath();
            ctx.fillStyle = segment.color;
            ctx.fill();

            currentAngle += sliceAngle;
          });
        }
      });
    };

    const fetchAnalyticsData = async () => {
      isLoading.value = true;
      try {
        // Fetch dashboard summary
        const summaryResponse = await axios.get(
          `${API_BASE}/api/analytics/dashboard-summary?period=${selectedPeriod.value}`
        );
        const summary = summaryResponse.data;

        totalRevenue.value = summary.totalRevenue;
        revenueGrowth.value = summary.revenueGrowth;
        totalOrders.value = summary.totalOrders;
        ordersGrowth.value = summary.ordersGrowth;
        avgOrderValue.value = `$${summary.avgOrderValue}`;
        totalCustomers.value = summary.totalCustomers;
        customerGrowth.value = summary.customerGrowth;
        newCustomers.value = summary.newCustomers;
        conversionRate.value = summary.conversionRate;

        // Fetch other data in parallel
        const [segmentsResponse, productsResponse, categoryResponse] =
          await Promise.all([
            axios.get(`${API_BASE}/api/analytics/customer-segments`),
            axios.get(
              `${API_BASE}/api/analytics/top-products?period=${selectedPeriod.value}`
            ),
            axios.get(
              `${API_BASE}/api/analytics/category-sales?period=${selectedPeriod.value}`
            ),
          ]);

        customerSegments.value = segmentsResponse.data;
        topProducts.value = productsResponse.data;
        categorySales.value = categoryResponse.data;

        updateCustomerSegmentsChart();
        showMessage("Analytics data updated successfully!", "success");
      } catch (error) {
        console.error("Error fetching analytics:", error);
        showMessage("Error loading analytics data", "error");
      } finally {
        isLoading.value = false;
      }
    };

    const fetchChartData = async () => {
      try {
        const [revenueResponse, ordersResponse] = await Promise.all([
          axios.get(
            `${API_BASE}/api/analytics/revenue-trend?period=${selectedPeriod.value}`
          ),
          axios.get(
            `${API_BASE}/api/analytics/orders-overview?period=${selectedPeriod.value}`
          ),
        ]);

        // Update charts with real data
        updateRevenueChart(revenueResponse.data);
        updateOrdersChart(ordersResponse.data);
      } catch (error) {
        console.error("Error fetching chart data:", error);
      }
    };

    const updateRevenueChart = (data) => {
      revenueChartData.value = {
        labels: data.map((item) => item.date),
        datasets: [
          {
            label: "Revenue ($)",
            data: data.map((item) => item.revenue),
            borderColor: "#667eea",
            backgroundColor: "rgba(102, 126, 234, 0.1)",
            tension: 0.4,
            fill: true,
          },
        ],
      };
    };

    const updateOrdersChart = (data) => {
      if (data.dailyTrend) {
        ordersChartData.value = {
          labels: data.dailyTrend.map((item) => item.date),
          datasets: [
            {
              label: "Orders",
              data: data.dailyTrend.map((item) => item.orders),
              backgroundColor: "#2ecc71",
              borderColor: "#27ae60",
              borderWidth: 1,
            },
          ],
        };
      }
    };

    const updateCustomerSegmentsChart = () => {
      if (customerSegments.value.length > 0) {
        customerSegmentsChartData.value = {
          labels: customerSegments.value.map((segment) => segment.name),
          datasets: [
            {
              data: customerSegments.value.map((segment) => segment.count),
              backgroundColor: customerSegments.value.map(
                (segment) => segment.color
              ),
              borderWidth: 0,
            },
          ],
        };
      }
    };

    const revenueChartOptions = {
      responsive: true,
      maintainAspectRatio: false,
      plugins: {
        legend: {
          display: false,
        },
      },
      scales: {
        y: {
          beginAtZero: true,
          ticks: {
            callback: function (value) {
              return "$" + value.toLocaleString();
            },
          },
        },
      },
    };

    const ordersChartOptions = {
      responsive: true,
      maintainAspectRatio: false,
      plugins: {
        legend: {
          display: false,
        },
      },
      scales: {
        y: {
          beginAtZero: true,
        },
      },
    };

    const segmentsChartOptions = {
      responsive: true,
      maintainAspectRatio: false,
      plugins: {
        legend: {
          display: false,
        },
      },
    };

    onMounted(async () => {
      await fetchAnalyticsData();
      await fetchChartData();
      createCharts();
    });

    return {
      isSidebarCollapsed,
      isLoading,
      messages,
      selectedPeriod,
      revenuePeriod,
      totalRevenue,
      totalOrders,
      avgOrderValue,
      totalCustomers,
      newCustomers,
      conversionRate,
      topProducts,
      customerSegments,
      categorySales,
      recentActivities,
      performanceMetrics,
      geographicSales,
      revenueChart,
      ordersChart,
      customerSegmentsChart,
      revenueGrowth,
      ordersGrowth,
      customerGrowth,
      revenueChartOptions,
      ordersChartOptions,
      segmentsChartOptions,
      revenueChartData,
      ordersChartData,
      customerSegmentsChartData,
      fetchAnalyticsData,
      fetchChartData,
      toggleSidebar,
      updateAnalytics,
      refreshData,
      getCategoryIcon,
      getActivityIcon,
      getGaugeColor,
      exportData,
      formatCurrency,
      formatTime,
      showMessage,
      removeMessage,
      imageBase,
    };
  },
};
</script>

<style scoped>
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

/* Analytics Controls Section */
.analytics-controls-section {
  background: white;
  border-radius: 1rem;
  padding: 1.25rem 1.5625rem;
  box-shadow: 0 0.25rem 1.25rem rgba(0, 0, 0, 0.08);
  border: 1px solid rgba(255, 255, 255, 0.2);
}

.controls-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.controls-title {
  font-size: 1.3rem;
  font-weight: 700;
  color: #2c3e50;
  margin: 0;
  display: flex;
  align-items: center;
  gap: 0.75rem;
}

.date-range-selector {
  display: flex;
  align-items: center;
  gap: 1rem;
}

.period-select {
  padding: 0.5rem 1rem;
  border: 2px solid #e9ecef;
  border-radius: 0.5rem;
  font-size: 0.9rem;
  background: white;
  cursor: pointer;
  transition: all 0.3s ease;
}

.period-select:focus {
  outline: none;
  border-color: #667eea;
  box-shadow: 0 0 0 3px rgba(102, 126, 234, 0.1);
}

.refresh-btn {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
  border: none;
  border-radius: 0.5rem;
  padding: 0.5rem 1rem;
  cursor: pointer;
  transition: all 0.3s ease;
  display: flex;
  align-items: center;
  gap: 0.5rem;
  font-weight: 600;
}

.refresh-btn:hover:not(:disabled) {
  transform: translateY(-2px);
  box-shadow: 0 4px 12px rgba(102, 126, 234, 0.4);
}

.refresh-btn:disabled {
  opacity: 0.6;
  cursor: not-allowed;
  transform: none;
}

/* Key Metrics Section */
.key-metrics-section {
  margin-bottom: 0.3125rem;
}

.metrics-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(18rem, 1fr));
  gap: 1.5625rem;
}

@media (min-width: 1200px) {
  .metrics-grid {
    grid-template-columns: repeat(4, 1fr);
  }
}

.metric-card {
  background: white;
  border-radius: 1rem;
  padding: 1.5625rem;
  box-shadow: 0 0.5rem 1.875rem rgba(0, 0, 0, 0.08);
  border: 1px solid rgba(255, 255, 255, 0.2);
  transition: all 0.3s ease;
  position: relative;
  overflow: hidden;
}

.metric-card::before {
  content: "";
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  height: 0.25rem;
}

.revenue-metric::before {
  background: linear-gradient(135deg, #2ecc71 0%, #27ae60 100%);
}

.orders-metric::before {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
}

.customers-metric::before {
  background: linear-gradient(135deg, #f39c12 0%, #e67e22 100%);
}

.conversion-metric::before {
  background: linear-gradient(135deg, #9b59b6 0%, #8e44ad 100%);
}

.metric-card:hover {
  transform: translateY(-0.3125rem);
  box-shadow: 0 0.9375rem 2.5rem rgba(0, 0, 0, 0.12);
}

.metric-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 1rem;
}

.metric-icon {
  width: 3rem;
  height: 3rem;
  border-radius: 0.75rem;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 1.5rem;
  color: white;
}

.revenue-metric .metric-icon {
  background: linear-gradient(135deg, #2ecc71 0%, #27ae60 100%);
}

.orders-metric .metric-icon {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
}

.customers-metric .metric-icon {
  background: linear-gradient(135deg, #f39c12 0%, #e67e22 100%);
}

.conversion-metric .metric-icon {
  background: linear-gradient(135deg, #9b59b6 0%, #8e44ad 100%);
}

.metric-trend {
  display: flex;
  align-items: center;
  gap: 0.25rem;
  font-size: 0.85rem;
  font-weight: 600;
  padding: 0.25rem 0.5rem;
  border-radius: 0.5rem;
}

.metric-trend.positive {
  background: rgba(46, 204, 113, 0.1);
  color: #27ae60;
}

.metric-trend.negative {
  background: rgba(231, 76, 60, 0.1);
  color: #e74c3c;
}

.metric-content {
  text-align: left;
}

.metric-value {
  font-size: 2.5rem;
  font-weight: 700;
  color: #2c3e50;
  line-height: 1;
  margin-bottom: 0.5rem;
}

.metric-label {
  font-size: 1.1rem;
  color: #2c3e50;
  font-weight: 600;
  margin-bottom: 0.25rem;
}

.metric-subtitle {
  font-size: 0.9rem;
  color: #7f8c8d;
}

/* Charts Section */
.charts-section {
  margin-bottom: 0.3125rem;
}

.charts-grid {
  display: grid;
  grid-template-columns: 2fr 1fr;
  gap: 1.5625rem;
}

.empty-chart {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  height: 100%;
  color: #7f8c8d;
}

.empty-chart i {
  font-size: 3rem;
  color: #bdc3c7;
  margin-bottom: 1rem;
}

.empty-chart p {
  font-size: 1.1rem;
  margin: 0;
}

@media (max-width: 1024px) {
  .charts-grid {
    grid-template-columns: 1fr;
  }
}

.chart-card {
  background: white;
  border-radius: 1rem;
  padding: 1.5625rem;
  box-shadow: 0 0.5rem 1.875rem rgba(0, 0, 0, 0.08);
  border: 1px solid rgba(255, 255, 255, 0.2);
}

.chart-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 1.25rem;
  padding-bottom: 1rem;
  border-bottom: 2px solid #f1f3f4;
}

.chart-title {
  font-size: 1.2rem;
  font-weight: 700;
  color: #2c3e50;
  margin: 0;
  display: flex;
  align-items: center;
  gap: 0.75rem;
}

.chart-controls {
  display: flex;
  gap: 0.5rem;
}

.chart-period-btn {
  background: #f8f9fa;
  border: none;
  padding: 0.375rem 0.75rem;
  border-radius: 0.375rem;
  cursor: pointer;
  transition: all 0.3s ease;
  font-size: 0.85rem;
  font-weight: 600;
  color: #7f8c8d;
}

.chart-period-btn.active {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
}

.chart-container {
  height: 300px;
  position: relative;
}

.chart-canvas {
  width: 100%;
  height: 100%;
  border-radius: 0.5rem;
}

/* Analytics Grid Section */
.analytics-grid-section {
  margin-bottom: 0.3125rem;
}

.analytics-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(22rem, 1fr));
  gap: 1.5625rem;
}

.analytics-card {
  background: white;
  border-radius: 1rem;
  padding: 1.5625rem;
  box-shadow: 0 0.5rem 1.875rem rgba(0, 0, 0, 0.08);
  border: 1px solid rgba(255, 255, 255, 0.2);
  transition: all 0.3s ease;
}

.analytics-card:hover {
  transform: translateY(-0.25rem);
  box-shadow: 0 0.75rem 2.5rem rgba(0, 0, 0, 0.12);
}

.card-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 1.25rem;
  padding-bottom: 1rem;
  border-bottom: 2px solid #f1f3f4;
}

.card-title {
  font-size: 1.1rem;
  font-weight: 700;
  color: #2c3e50;
  margin: 0;
  display: flex;
  align-items: center;
  gap: 0.75rem;
}

.view-all-btn {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
  border: none;
  border-radius: 0.375rem;
  padding: 0.375rem 0.75rem;
  cursor: pointer;
  transition: all 0.3s ease;
  font-size: 0.85rem;
  font-weight: 600;
}

.view-all-btn:hover {
  transform: translateY(-1px);
  box-shadow: 0 2px 8px rgba(102, 126, 234, 0.4);
}

.card-content {
  height: 100%;
}

/* Top Products */
.products-list {
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

.product-item {
  display: flex;
  align-items: center;
  gap: 1rem;
  padding: 0.75rem;
  background: #f8f9fa;
  border-radius: 0.5rem;
  transition: all 0.3s ease;
}

.product-item:hover {
  background: #e9ecef;
  transform: translateX(0.25rem);
}

.product-rank {
  width: 2rem;
  height: 2rem;
  border-radius: 50%;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
  display: flex;
  align-items: center;
  justify-content: center;
  font-weight: 700;
  font-size: 0.9rem;
}

.product-image {
  width: 2.5rem;
  height: 2.5rem;
  border-radius: 0.375rem;
  overflow: hidden;
}

.product-img {
  width: 100%;
  height: 100%;
  object-fit: cover;
}

.product-placeholder {
  width: 100%;
  height: 100%;
  background: linear-gradient(135deg, #bdc3c7 0%, #95a5a6 100%);
  display: flex;
  align-items: center;
  justify-content: center;
  color: white;
  font-size: 1rem;
}

.product-details {
  flex: 1;
}

.product-name {
  font-weight: 600;
  color: #2c3e50;
  font-size: 0.95rem;
  margin-bottom: 0.25rem;
}

.product-category {
  font-size: 0.8rem;
  color: #7f8c8d;
}

.product-stats {
  text-align: right;
}

.product-sales {
  font-weight: 700;
  color: #27ae60;
  font-size: 1rem;
  margin-bottom: 0.25rem;
}

.product-units {
  font-size: 0.8rem;
  color: #7f8c8d;
}

/* Customer Segments */
.segments-chart-container {
  height: 200px;
  margin-bottom: 1rem;
}

.segments-chart {
  width: 100%;
  height: 100%;
}

.segments-legend {
  display: flex;
  flex-direction: column;
  gap: 0.75rem;
}

.legend-item {
  display: flex;
  align-items: center;
  gap: 0.75rem;
}

.legend-color {
  width: 1rem;
  height: 1rem;
  border-radius: 0.25rem;
}

.legend-details {
  flex: 1;
}

.legend-name {
  font-weight: 600;
  color: #2c3e50;
  font-size: 0.9rem;
  margin-bottom: 0.125rem;
}

.legend-value {
  font-size: 0.8rem;
  color: #7f8c8d;
}

/* Category Sales */
.category-list {
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

.category-item {
  display: flex;
  flex-direction: column;
  gap: 0.75rem;
  padding: 1rem;
  background: #f8f9fa;
  border-radius: 0.5rem;
  transition: all 0.3s ease;
}

.category-item:hover {
  background: #e9ecef;
  transform: translateY(-2px);
}

.category-info {
  display: flex;
  align-items: center;
  gap: 0.75rem;
}

.category-icon {
  width: 2.5rem;
  height: 2.5rem;
  border-radius: 0.5rem;
  display: flex;
  align-items: center;
  justify-content: center;
  color: white;
  font-size: 1.1rem;
}

.category-tyres {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
}

.category-batteries {
  background: linear-gradient(135deg, #2ecc71 0%, #27ae60 100%);
}

.category-oil {
  background: linear-gradient(135deg, #f39c12 0%, #e67e22 100%);
}

.category-parts {
  background: linear-gradient(135deg, #9b59b6 0%, #8e44ad 100%);
}

.category-accessories {
  background: linear-gradient(135deg, #e74c3c 0%, #c0392b 100%);
}

.category-details {
  flex: 1;
}

.category-name {
  font-weight: 600;
  color: #2c3e50;
  font-size: 1rem;
  margin-bottom: 0.25rem;
}

.category-growth {
  display: flex;
  align-items: center;
  gap: 0.25rem;
  font-size: 0.85rem;
  font-weight: 600;
}

.category-growth.positive {
  color: #27ae60;
}

.category-growth.negative {
  color: #e74c3c;
}

.category-stats {
  text-align: right;
}

.category-revenue {
  font-weight: 700;
  color: #2c3e50;
  font-size: 1.1rem;
  margin-bottom: 0.25rem;
}

.category-percentage {
  font-size: 0.8rem;
  color: #7f8c8d;
}

.category-progress {
  margin-top: 0.5rem;
}

.progress-bar {
  width: 100%;
  height: 0.5rem;
  background: #e9ecef;
  border-radius: 0.25rem;
  overflow: hidden;
}

.progress-fill {
  height: 100%;
  border-radius: 0.25rem;
  transition: width 0.3s ease;
}

/* Recent Activity */
.activity-list {
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

.activity-item {
  display: flex;
  align-items: center;
  gap: 0.75rem;
  padding: 0.75rem;
  background: #f8f9fa;
  border-radius: 0.5rem;
  transition: all 0.3s ease;
}

.activity-item:hover {
  background: #e9ecef;
  transform: translateX(0.25rem);
}

.activity-icon {
  width: 2.5rem;
  height: 2.5rem;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  color: white;
  font-size: 1rem;
}

.activity-icon.order {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
}

.activity-icon.customer {
  background: linear-gradient(135deg, #2ecc71 0%, #27ae60 100%);
}

.activity-icon.product {
  background: linear-gradient(135deg, #f39c12 0%, #e67e22 100%);
}

.activity-details {
  flex: 1;
}

.activity-text {
  font-weight: 500;
  color: #2c3e50;
  font-size: 0.9rem;
  margin-bottom: 0.25rem;
}

.activity-time {
  font-size: 0.8rem;
  color: #7f8c8d;
}

.activity-value {
  font-weight: 700;
  color: #27ae60;
  font-size: 0.9rem;
}

/* Performance Metrics */
.metrics-list {
  display: flex;
  flex-direction: column;
  gap: 1.25rem;
}

.performance-metric {
  display: flex;
  justify-content: space-between;
  align-items: center;
  gap: 1rem;
}

.metric-info {
  flex: 1;
}

.metric-name {
  font-weight: 600;
  color: #2c3e50;
  font-size: 0.95rem;
  margin-bottom: 0.25rem;
}

.metric-description {
  font-size: 0.8rem;
  color: #7f8c8d;
}

.metric-gauge {
  display: flex;
  align-items: center;
  gap: 0.75rem;
  min-width: 120px;
}

.gauge-container {
  flex: 1;
  height: 0.5rem;
  background: #e9ecef;
  border-radius: 0.25rem;
  overflow: hidden;
}

.gauge-fill {
  height: 100%;
  border-radius: 0.25rem;
  transition: width 0.3s ease;
}

.gauge-value {
  font-weight: 700;
  color: #2c3e50;
  font-size: 0.9rem;
  min-width: 2.5rem;
  text-align: right;
}

/* Geographic Sales */
.geographic-list {
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

.location-item {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 0.75rem;
  background: #f8f9fa;
  border-radius: 0.5rem;
  transition: all 0.3s ease;
}

.location-item:hover {
  background: #e9ecef;
  transform: translateY(-1px);
}

.location-info {
  display: flex;
  align-items: center;
  gap: 0.75rem;
}

.location-flag {
  font-size: 1.5rem;
}

.location-details {
  display: flex;
  flex-direction: column;
  gap: 0.125rem;
}

.location-name {
  font-weight: 600;
  color: #2c3e50;
  font-size: 0.9rem;
}

.location-orders {
  font-size: 0.8rem;
  color: #7f8c8d;
}

.location-stats {
  text-align: right;
}

.location-revenue {
  font-weight: 700;
  color: #2c3e50;
  font-size: 1rem;
  margin-bottom: 0.25rem;
}

.location-percentage {
  font-size: 0.8rem;
  color: #7f8c8d;
}

/* Export Section */
.export-section {
  margin-top: 1rem;
}

.export-card {
  background: white;
  border-radius: 1rem;
  padding: 1.5625rem;
  box-shadow: 0 0.5rem 1.875rem rgba(0, 0, 0, 0.08);
  border: 1px solid rgba(255, 255, 255, 0.2);
}

.export-header {
  text-align: center;
  margin-bottom: 1.5rem;
}

.export-title {
  font-size: 1.3rem;
  font-weight: 700;
  color: #2c3e50;
  margin: 0 0 0.5rem 0;
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 0.75rem;
}

.export-description {
  color: #7f8c8d;
  font-size: 1rem;
  margin: 0;
}

.export-actions {
  display: flex;
  justify-content: center;
  gap: 1rem;
}

@media (max-width: 768px) {
  .export-actions {
    flex-direction: column;
    align-items: center;
  }
}

.export-btn {
  border: none;
  border-radius: 0.5rem;
  padding: 0.75rem 1.5rem;
  cursor: pointer;
  transition: all 0.3s ease;
  font-weight: 600;
  display: flex;
  align-items: center;
  gap: 0.5rem;
  color: white;
}

.pdf-export {
  background: linear-gradient(135deg, #e74c3c 0%, #c0392b 100%);
}

.excel-export {
  background: linear-gradient(135deg, #2ecc71 0%, #27ae60 100%);
}

.csv-export {
  background: linear-gradient(135deg, #f39c12 0%, #e67e22 100%);
}

.export-btn:hover {
  transform: translateY(-2px);
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.2);
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

/* Empty State Styles */
.empty-state {
  text-align: center;
  padding: 2rem 1rem;
  color: #7f8c8d;
}

.empty-icon {
  font-size: 3rem;
  color: #bdc3c7;
  margin-bottom: 1rem;
}

.empty-state p {
  font-size: 1.1rem;
  font-weight: 600;
  margin-bottom: 0.5rem;
  color: #2c3e50;
}

.empty-state small {
  font-size: 0.9rem;
  color: #95a5a6;
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

  .metrics-grid {
    grid-template-columns: repeat(2, 1fr);
  }

  .analytics-grid {
    grid-template-columns: repeat(auto-fit, minmax(20rem, 1fr));
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

  .metrics-grid {
    grid-template-columns: 1fr;
  }

  .charts-grid {
    grid-template-columns: 1fr;
  }

  .analytics-grid {
    grid-template-columns: 1fr;
  }

  .controls-header {
    flex-direction: column;
    gap: 1rem;
    align-items: flex-start;
  }

  .date-range-selector {
    width: 100%;
    justify-content: space-between;
  }

  .period-select {
    flex: 1;
    margin-right: 1rem;
  }
}

@media (max-width: 480px) {
  .metric-value {
    font-size: 2rem;
  }

  .chart-container {
    height: 250px;
  }

  .product-item,
  .category-item,
  .activity-item,
  .location-item {
    flex-direction: column;
    align-items: flex-start;
    gap: 0.5rem;
  }

  .product-stats,
  .category-stats,
  .location-stats {
    text-align: left;
    width: 100%;
  }
}
</style>
