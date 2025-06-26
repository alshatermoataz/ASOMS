import { defineStore } from 'pinia'
import { ref, computed } from 'vue'

export const useCartStore = defineStore('cart', () => {
  const items = ref([])
  
  // Get the number of unique items in the cart (not total quantity)
  const uniqueItemCount = computed(() => {
    return items.value.length
  })
  
  // Get the total quantity of all items in the cart
  const totalQuantity = computed(() => {
    return items.value.reduce((total, item) => total + item.quantity, 0)
  })
  
  // Get the total price of all items in the cart
  const totalPrice = computed(() => {
    return items.value.reduce((total, item) => total + (item.price * item.quantity), 0)
  })
  
  // Add an item to the cart
  function addToCart(newItem) {
    // Check if the item already exists in the cart
    const existingItemIndex = items.value.findIndex(item => 
      item.id === newItem.id && 
      (newItem.size ? item.size === newItem.size : true)
    )
    
    if (existingItemIndex !== -1) {
      // If item exists, update the quantity
      items.value[existingItemIndex].quantity += newItem.quantity
    } else {
      // If item doesn't exist, add it to the cart
      items.value.push({...newItem})
    }
    
    // Save to localStorage for persistence
    saveCart()
  }
  
  // Remove an item from the cart
  function removeFromCart(itemId, size = null) {
    items.value = items.value.filter(item => 
      !(item.id === itemId && (size ? item.size === size : true))
    )
    
    // Save to localStorage for persistence
    saveCart()
  }
  
  // Update the quantity of an item in the cart
  function updateQuantity(itemId, quantity, size = null) {
    const item = items.value.find(item => 
      item.id === itemId && (size ? item.size === size : true)
    )
    
    if (item) {
      item.quantity = quantity
      
      // Remove item if quantity is 0
      if (quantity <= 0) {
        removeFromCart(itemId, size)
      } else {
        // Save to localStorage for persistence
        saveCart()
      }
    }
  }
  
  // Clear the cart
  function clearCart() {
    items.value = []
    
    // Save to localStorage for persistence
    saveCart()
  }
  
  // Save cart to localStorage
  function saveCart() {
    localStorage.setItem('cart', JSON.stringify(items.value))
  }
  
  // Load cart from localStorage
  function loadCart() {
    const savedCart = localStorage.getItem('cart')
    if (savedCart) {
      items.value = JSON.parse(savedCart)
    }
  }
  
  // Initialize cart from localStorage
  loadCart()
  
  return { 
    items, 
    uniqueItemCount,
    totalQuantity,
    totalPrice,
    addToCart, 
    removeFromCart, 
    updateQuantity, 
    clearCart 
  }
})