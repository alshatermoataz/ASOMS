import { defineStore } from 'pinia'
import axios from 'axios'

export const useProductStore = defineStore('products', {
  state: () => ({
    products: [],
    orders: [],
    loading: false,
    error: null,
    lastFetched: null
  }),
  
  getters: {
    getProductById: (state) => (id) => {
      return state.products.find(product => product.id === parseInt(id) || product.id === id)
    },
    
    getProductsByCategory: (state) => (category) => {
      if (!category) return state.products
      return state.products.filter(product => product.category === category)
    },
    
    getOrderById: (state) => (id) => {
      return state.orders.find(order => order.id === id)
    },
    
    isProductInStock: (state) => (id) => {
      const product = state.products.find(p => p.id === parseInt(id) || p.id === id)
      return product ? product.quantity > 0 : false // Changed from stock to quantity
    },
    
    getAvailableQuantity: (state) => (id) => { // Changed from getAvailableStock to getAvailableQuantity
      const product = state.products.find(p => p.id === parseInt(id) || p.id === id)
      return product ? product.quantity : 0 // Changed from stock to quantity
    },
    
    // Check if data needs refreshing (older than 5 minutes)
    needsRefresh: (state) => {
      if (!state.lastFetched) return true
      const fiveMinutesAgo = new Date(Date.now() - 5 * 60 * 1000)
      return state.lastFetched < fiveMinutesAgo
    }
  },
  
  actions: {
    // Fetch products from the API
    async fetchProducts(forceRefresh = false) {
      // Skip if already loading
      if (this.loading) return this.products
      
      // Skip if data is fresh and no force refresh
      if (!forceRefresh && !this.needsRefresh && this.products.length > 0) {
        return this.products
      }
      
      this.loading = true
      this.error = null
      
      try {
        
        let response
        try {
          response = await axios.get('https://asoms-production.up.railway.app/api/product')
        } catch (err) {
        }
        
        // Process the data to ensure price is a number
        this.products = response.data.map(product => ({
          ...product,
          id: parseInt(product.id) || product.id, // Ensure ID is a number if possible
          price: typeof product.price === 'string' 
            ? parseFloat(product.price.replace(/[^\d.]/g, '')) 
            : product.price,
          quantity: product.quantity !== undefined ? parseInt(product.quantity) : 10 // Changed from stock to quantity
        }))
        
        console.log('Fetched products:', this.products)
        
        this.lastFetched = new Date()
        this.loading = false
        return this.products
      } catch (error) {
        console.error('Error fetching products:', error)
        this.error = error.message || 'Failed to fetch products'
        this.loading = false
        
        // If API fails, return existing data
        return this.products
      }
    },
    
    // Fetch a single product by ID
    async fetchProductById(id) {
      this.loading = true
      this.error = null
      
      try {
        // Try both endpoints
        let response
        try {
          response = await axios.get(`https://asoms-production.up.railway.app/api/product/${id}`)
        } catch (err) {

        }
        
        // Process the data to ensure price is a number
        const product = {
          ...response.data,
          id: parseInt(response.data.id) || response.data.id,
          price: typeof response.data.price === 'string' 
            ? parseFloat(response.data.price.replace(/[^\d.]/g, '')) 
            : response.data.price,
          quantity: response.data.quantity !== undefined ? parseInt(response.data.quantity) : 10 // Changed from stock to quantity
        }
        
        // Update the product in the store if it exists, otherwise add it
        const index = this.products.findIndex(p => p.id === product.id)
        if (index !== -1) {
          this.products[index] = product
        } else {
          this.products.push(product)
        }
        
        this.loading = false
        return product
      } catch (error) {
        console.error(`Error fetching product ${id}:`, error)
        this.error = error.message || `Failed to fetch product ${id}`
        this.loading = false
        
        // Return the product from the store if it exists
        return this.getProductById(id)
      }
    },
    
    // Update product quantity (can be positive or negative quantity adjustment)
    async updateProductQuantity(productId, quantityChange) { // Changed from updateProductStock to updateProductQuantity
      const product = this.getProductById(productId)
      if (!product) return false
      
      try {
        // In a real app, you would make an API call here
        // For now, we'll just update the local state
        const newQuantity = product.quantity + quantityChange // Changed from stock to quantity
        
        // Ensure quantity doesn't go below 0
        const updatedQuantity = newQuantity < 0 ? 0 : newQuantity // Changed from stock to quantity
        
        // Update the product in the store
        const index = this.products.findIndex(p => p.id === product.id)
        if (index !== -1) {
          this.products[index] = { 
            ...this.products[index], 
            quantity: updatedQuantity // Changed from stock to quantity
          }
        }
        
        return true
      } catch (error) {
        console.error(`Error updating product quantity for ${productId}:`, error) // Changed from stock to quantity
        this.error = error.message || `Failed to update product quantity` // Changed from stock to quantity
        return false
      }
    },
    
    // Check if there's enough quantity for a product
    hasEnoughQuantity(productId, requestedQuantity) { // Changed from hasEnoughStock to hasEnoughQuantity
      const product = this.getProductById(productId)
      return product ? product.quantity >= requestedQuantity : false // Changed from stock to quantity
    },
    
    // Fetch orders from the API
    async fetchOrders() {
      this.loading = true
      this.error = null
      
      try {
        // In a real app, you would make an API call here
        // For now, we'll just simulate a delay
        await new Promise(resolve => setTimeout(resolve, 500))
        
        // Since we don't have a real API for orders yet, we'll use mock data
        this.orders = [
          {
            id: 1,
            productId: 1,
            price: 300,
            status: 'Paid',
            date: '2023-05-15',
            paymentMethod: 'Cash'
          },
          {
            id: 2,
            productId: 2,
            price: 50,
            status: 'Unpaid',
            date: '2023-05-20',
            paymentMethod: 'Cash'
          },
          {
            id: 3,
            productId: 3,
            price: 50,
            status: 'Unpaid',
            date: '2023-05-25',
            paymentMethod: 'Credit'
          },
          {
            id: 4,
            productId: 4,
            price: 50,
            status: 'Paid',
            date: '2023-06-01',
            paymentMethod: 'Cash'
          }
        ]
        
        this.loading = false
        return this.orders
      } catch (error) {
        console.error('Error fetching orders:', error)
        this.error = error.message || 'Failed to fetch orders'
        this.loading = false
        return []
      }
    },
    
    // Create a new order
    async createOrder(orderData) {
      try {
        // In a real app, you would make an API call here
        // For now, we'll just simulate a delay and create the order locally
        await new Promise(resolve => setTimeout(resolve, 500))
        
        const newOrder = {
          id: this.orders.length + 1,
          ...orderData,
          status: 'Pending',
          date: new Date().toISOString().split('T')[0]
        }
        
        this.orders.push(newOrder)
        
        // Update quantity for each product in the order
        if (orderData.items && Array.isArray(orderData.items)) {
          for (const item of orderData.items) {
            await this.updateProductQuantity(item.id, -item.quantity) // Changed from updateProductStock to updateProductQuantity
          }
        }
        
        return newOrder
      } catch (error) {
        console.error('Error creating order:', error)
        this.error = error.message || 'Failed to create order'
        throw error
      }
    },
    
    // Update order status
    async updateOrderStatus(orderId, status) {
      try {
        // In a real app, you would make an API call here
        // For now, we'll just update the local state
        const order = this.orders.find(o => o.id === orderId)
        if (order) {
          order.status = status
          return true
        }
        return false
      } catch (error) {
        console.error(`Error updating order status for ${orderId}:`, error)
        this.error = error.message || `Failed to update order status`
        return false
      }
    },
    
    // Add a new product
    async addProduct(productData) {
      try {
        // In a real app, you would make an API call here
        // For now, we'll just simulate a delay and add the product locally
        await new Promise(resolve => setTimeout(resolve, 500))
        
        const newProduct = {
          id: this.products.length + 1,
          ...productData,
          quantity: productData.quantity || 0, // Changed from stock to quantity
          price: typeof productData.price === 'string' 
            ? parseFloat(productData.price.replace(/[^\d.]/g, '')) 
            : productData.price
        }
        
        this.products.push(newProduct)
        return newProduct
      } catch (error) {
        console.error('Error adding product:', error)
        this.error = error.message || 'Failed to add product'
        throw error
      }
    },
    
    // Update a product
    async updateProduct(productId, productData) {
      try {
        // In a real app, you would make an API call here
        // For now, we'll just update the local state
        const index = this.products.findIndex(p => p.id === productId)
        if (index !== -1) {
          // Process price if it's a string
          if (typeof productData.price === 'string') {
            productData.price = parseFloat(productData.price.replace(/[^\d.]/g, ''))
          }
          
          this.products[index] = { ...this.products[index], ...productData }
          return this.products[index]
        }
        return null
      } catch (error) {
        console.error(`Error updating product ${productId}:`, error)
        this.error = error.message || `Failed to update product`
        return null
      }
    }
  }
})