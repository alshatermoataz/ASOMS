import { defineStore } from 'pinia'
import axios from 'axios'
import { getCurrentInstance } from 'vue'

export const useOrderStore = defineStore('orders', {
  state: () => ({
    orders: [],
    loading: false,
    error: null,
    lastFetched: null,
    totalItems: 0,
    totalPages: 1
  }),
  getters: {
    getOrderById: (state) => (id) => {
      return state.orders.find(order => order.id === id)
    },
    needsRefresh: (state) => {
      if (!state.lastFetched) return true
      const fiveMinutesAgo = new Date(Date.now() - 5 * 60 * 1000)
      return state.lastFetched < fiveMinutesAgo
    }
  },
  actions: {
    async fetchOrders(params = {}) {
      if (this.loading) return { data: this.orders, totalItems: this.totalItems, totalPages: this.totalPages }
    
      // If no params provided and we have cached data, return it
      if (Object.keys(params).length === 0 && !this.needsRefresh && this.orders.length > 0) {
        return { data: this.orders, totalItems: this.totalItems, totalPages: this.totalPages }
      }
    
      this.loading = true
      this.error = null
    
      try {
        // Build query string from params
        const queryParams = new URLSearchParams()
        
        if (params.page) queryParams.append('page', params.page)
        if (params.pageSize) queryParams.append('pageSize', params.pageSize)
        if (params.status) queryParams.append('status', params.status)
        if (params.paymentMethod) queryParams.append('paymentMethod', params.paymentMethod)
        if (params.search) queryParams.append('search', params.search)
        if (params.sort) queryParams.append('sort', params.sort)
        if (params.dateRange) queryParams.append('dateRange', params.dateRange)
        if (params.minAmount) queryParams.append('minAmount', params.minAmount)
        if (params.maxAmount) queryParams.append('maxAmount', params.maxAmount)
    
        const queryString = queryParams.toString()
        const url = `https://asoms-production.up.railway.app/api/orders${queryString ? '?' + queryString : ''}`
        
        console.log('Fetching from URL:', url) // Debug log
        
        const response = await axios.get(url)
    
        let ordersArr = [];
        let totalItems = 0;
        let totalPages = 1;
    
        if (Array.isArray(response.data)) {
          ordersArr = response.data;
          totalItems = ordersArr.length;
          totalPages = 1;
        } else if (response.data && Array.isArray(response.data.data)) {
          ordersArr = response.data.data;
          totalItems = response.data.totalItems || ordersArr.length;
          totalPages = response.data.totalPages || 1;
        } else if (response.data && Array.isArray(response.data.orders)) {
          ordersArr = response.data.orders;
          totalItems = response.data.totalItems || ordersArr.length;
          totalPages = response.data.totalPages || 1;
        } else {
          ordersArr = [];
          totalItems = 0;
          totalPages = 1;
        }
    
        this.orders = ordersArr;
        this.totalItems = totalItems;
        this.totalPages = totalPages;
        this.lastFetched = new Date()
        this.loading = false
    
        // Return the data in the format your component expects
        return {
          data: { 
            data: ordersArr,
            totalItems: totalItems,
            totalPages: totalPages
          },
          totalItems: totalItems,
          totalPages: totalPages
        }
    
      } catch (error) {
        this.error = error.message || 'Failed to fetch orders'
        this.loading = false
        
        // Return current state even on error
        return {
          data: { 
            data: this.orders,
            totalItems: this.totalItems,
            totalPages: this.totalPages
          },
          totalItems: this.totalItems,
          totalPages: this.totalPages
        }
      }
    },
    initSignalR() {
      const instance = getCurrentInstance()
      const connection = instance?.appContext.config.globalProperties.$signalR
      if (!connection) {
        console.warn('SignalR connection not found')
        return
      }
      console.log('Registering OrderUpdated handler');
      connection.on('OrderUpdated', async () => {
        console.log('OrderUpdated event received!');
        await this.fetchOrders(true);
      })
    }
  }
}) 