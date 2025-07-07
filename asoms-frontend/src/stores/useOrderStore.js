import { defineStore } from "pinia"
import axios from "axios"
import { getCurrentInstance } from "vue"

const API_URL = "https://asoms-production.up.railway.app/api/orders"
// const API_URL = "https://localhost:7004/api/orders"


export const useOrderStore = defineStore("orders", {
  state: () => ({
    orders: [],
    loading: false,
    error: null,
    lastFetched: null,
    totalItems: 0,
    totalPages: 1,
    currentPage: 1,
    pageSize: 25,
    // Stats
    stats: {
      totalOrders: 0,
      pendingOrders: 0,
      completedOrders: 0,
      totalRevenue: 0,
      averageOrderValue: 0,
    },
    // Available statuses
    availableStatuses: [
      { key: "PendingApproval", value: "Pending Approval" },
      { key: "Confirmed", value: "Confirmed" },
      { key: "Rejected", value: "Rejected" },
      { key: "Completed", value: "Completed" },
    ],
  }),

  getters: {
    getOrderById: (state) => (id) => {
      return state.orders.find((order) => order.id === id)
    },

    needsRefresh: (state) => {
      if (!state.lastFetched) return true
      const fiveMinutesAgo = new Date(Date.now() - 5 * 60 * 1000)
      return state.lastFetched < fiveMinutesAgo
    },

    paginationInfo: (state) => {
      const start = state.totalItems > 0 ? (state.currentPage - 1) * state.pageSize + 1 : 0
      const end = Math.min(state.currentPage * state.pageSize, state.totalItems)
      return { start, end }
    },

    displayedPages: (state) => {
      const pages = []
      const maxPagesToShow = 5
      let startPage = Math.max(1, state.currentPage - Math.floor(maxPagesToShow / 2))
      const endPage = Math.min(state.totalPages, startPage + maxPagesToShow - 1)

      if (endPage - startPage + 1 < maxPagesToShow) {
        startPage = Math.max(1, endPage - maxPagesToShow + 1)
      }

      for (let i = startPage; i <= endPage; i++) {
        pages.push(i)
      }
      return pages
    },
  },

  actions: {
    async fetchOrders(params = {}) {
      if (this.loading) return this.getReturnData()

      // // If no params provided and we have cached data, return it
      // if (Object.keys(params).length === 0 && !this.needsRefresh && this.orders.length > 0) {
      //   return this.getReturnData()
      // }

      this.loading = true
      this.error = null

      try {
        // Build query string from params
        const queryParams = new URLSearchParams()

        if (params.page) {
          queryParams.append("page", params.page)
          this.currentPage = params.page
        }
        if (params.pageSize) {
          queryParams.append("pageSize", params.pageSize)
          this.pageSize = params.pageSize
        }
        if (params.status) queryParams.append("status", params.status)
        if (params.paymentMethod) queryParams.append("paymentMethod", params.paymentMethod)
        if (params.search) queryParams.append("search", params.search)
        if (params.sort) queryParams.append("sort", params.sort)
        if (params.dateRange) queryParams.append("dateRange", params.dateRange)
        if (params.minAmount) queryParams.append("minAmount", params.minAmount)
        if (params.maxAmount) queryParams.append("maxAmount", params.maxAmount)

        const queryString = queryParams.toString()
        const url = `${API_URL}${queryString ? "?" + queryString : ""}`

        const response = await axios.get(url)

        console.log("response is here 1 " , response.data);
        let ordersArr = []
        let totalItems = 0
        let totalPages = 1


        ordersArr = response.data.data
        totalItems = response.data.totalItems || ordersArr.length
        totalPages = response.data.totalPages || 1

        this.orders = ordersArr
        this.totalItems = totalItems
        this.totalPages = totalPages
        this.lastFetched = new Date()

        // Update stats
        this.updateStats()

        return this.getReturnData()
      } catch (error) {
        this.error = error.message || "Failed to fetch orders"
        console.error("Error fetching orders:", error)
        return this.getReturnData()
      } finally {
        this.loading = false
      }
    },

    async updateOrderStatus(orderId, status, notes = "") {
      try {
        const response = await axios.put(`${API_URL}/${orderId}/status`, {
          status,
          notesToBuyer: notes,
        })

        if (response.data.success) {
          // Update the order in the local state
          const orderIndex = this.orders.findIndex((o) => o.id === orderId)
          if (orderIndex !== -1) {
            this.orders[orderIndex].status = status
            this.orders[orderIndex].statusDisplay = this.getStatusDisplay(status)
            this.orders[orderIndex].statusChangedAt = new Date().toISOString()
            this.orders[orderIndex].statusChangedBy = "Admin User"
            if (notes) {
              this.orders[orderIndex].notesToBuyer = notes
            }
          }

          // Update stats
          this.updateStats()

          return { success: true, message: response.data.message }
        }

        return { success: false, message: response.data.message || "Failed to update order status" }
      } catch (error) {
        console.error("Error updating order status:", error)
        return { success: false, message: "Error updating order status. Please try again." }
      }
    },

    async deleteOrder(orderId) {
      try {
        await axios.delete(`${API_URL}/${orderId}`)

        // Remove from local state
        this.orders = this.orders.filter((order) => order.id !== orderId)
        this.totalItems = Math.max(0, this.totalItems - 1)

        // Update stats
        this.updateStats()

        return { success: true, message: "Order deleted successfully" }
      } catch (error) {
        console.error("Error deleting order:", error)
        return { success: false, message: "Failed to delete order. Please try again." }
      }
    },

    async downloadInvoices(orderIds) {
      try {
        const response = await axios.post(`${API_URL}/download-invoices`, orderIds, {
          responseType: "blob",
        })

        const blob = new Blob([response.data], { type: "application/pdf" })
        const url = window.URL.createObjectURL(blob)
        const link = document.createElement("a")

        const filename = orderIds.length === 1 ? `Invoice_${this.formatOrderId(orderIds[0])}.pdf` : "Invoices.pdf"

        link.href = url
        link.setAttribute("download", filename)
        document.body.appendChild(link)
        link.click()
        link.remove()
        window.URL.revokeObjectURL(url)

        return { success: true, message: "Invoice(s) downloaded successfully" }
      } catch (error) {
        console.error("Error downloading invoices:", error)
        return { success: false, message: "Failed to download invoices. Please try again." }
      }
    },

    // Helper methods
    updateStats() {
      this.stats.totalOrders = this.totalItems
      this.stats.pendingOrders = this.orders.filter((o) => o.status === "Pending Approval").length
      
      this.stats.completedOrders = this.orders.filter((o) => o.status === "Completed").length

      const completedOrders = this.orders.filter((o) => o.status === "Completed")
      this.stats.totalRevenue = completedOrders.reduce(
        (sum, order) => sum + Number.parseFloat(order.totalAmount || 0),
        0,
      )

      this.stats.averageOrderValue =
        this.stats.completedOrders > 0 ? this.stats.totalRevenue / this.stats.completedOrders : 0
    },

    getReturnData() {
      return {
        data: this.orders,
        totalItems: this.totalItems,
        totalPages: this.totalPages,
      }
    },

    getStatusDisplay(status) {
      const statusMap = {
        PendingApproval: "Pending Approval",
        Confirmed: "Confirmed",
        Rejected: "Rejected",
        Completed: "Completed",
      }
      return statusMap[status] || status
    },

    formatOrderId(id) {
      return id ? String(id).substring(0, 8).toUpperCase() : "N/A"
    },

    getAvailableStatusesFor(currentStatus) {
      switch (currentStatus) {
        case "PendingApproval":
          return [
            { key: "Confirmed", value: "Confirmed" },
            { key: "Rejected", value: "Rejected" },
          ]
        case "Confirmed":
          return [{ key: "Completed", value: "Completed" }]
        case "Rejected":
          return [{ key: "PendingApproval", value: "Pending Approval" }]
        default:
          return []
      }
    },

    initSignalR() {
      const instance = getCurrentInstance()
      const connection = instance?.appContext.config.globalProperties.$signalR
      if (!connection) {
        console.warn("SignalR connection not found")
        return
      }

      console.log("Registering OrderUpdated handler")
      connection.on("OrderUpdated", async () => {
        console.log("OrderUpdated event received!")
        await this.fetchOrders({ force: true })
      })
    },
  },
})
