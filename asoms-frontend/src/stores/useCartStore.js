import { defineStore } from "pinia"
import { ref, computed } from "vue"
import { useAuthStore } from "./auth"

export const useCartStore = defineStore("cart", () => {
  const auth = useAuthStore()
  const items = ref([])
  const currentUserId = ref(null)

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
    return items.value.reduce((total, item) => total + item.price * item.quantity, 0)
  })

  // Get total items count for badge display
  const totalItems = computed(() => {
    return items.value.reduce((total, item) => total + item.quantity, 0)
  })

  // Add an item to the cart
  function addToCart(newItem) {
    // Check if the item already exists in the cart
    const existingItemIndex = items.value.findIndex(
      (item) => item.id === newItem.id && (newItem.size ? item.size === newItem.size : true),
    )

    if (existingItemIndex !== -1) {
      // If item exists, update the quantity
      items.value[existingItemIndex].quantity += newItem.quantity
    } else {
      // If item doesn't exist, add it to the cart
      items.value.push({ ...newItem })
    }

    // Save to localStorage for persistence
    saveCart()
  }

  // Remove an item from the cart
  function removeFromCart(itemId, size = null) {
    items.value = items.value.filter((item) => !(item.id === itemId && (size ? item.size === size : true)))

    // Save to localStorage for persistence
    saveCart()
  }

  // Update the quantity of an item in the cart
  function updateQuantity(itemId, quantity, size = null) {
    const item = items.value.find((item) => item.id === itemId && (size ? item.size === size : true))

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

  // Clear the cart (only clears current state, doesn't remove from localStorage)
  function clearCart() {
    items.value = []
    // Don't save to localStorage here - this is for temporary clearing
  }

  // Get user-specific cart key
  function getCartKey(userId) {
    return userId ? `cart_${userId}` : "cart_guest"
  }

  // Save cart to localStorage with user-specific key
  function saveCart() {
    const userId = auth.user?.id || auth.user?.userId || currentUserId.value
    const cartKey = getCartKey(userId)

    localStorage.setItem(cartKey, JSON.stringify(items.value))

    // Also save the current user ID for reference
    if (userId) {
      localStorage.setItem("cart_current_user", userId.toString())
    }

    console.log(`Cart saved for user: ${userId}`)
  }

  // Load cart from localStorage for specific user
  function loadCart(userId = null) {
    const targetUserId = userId || auth.user?.id || auth.user?.userId
    const cartKey = getCartKey(targetUserId)

    const savedCart = localStorage.getItem(cartKey)
    if (savedCart) {
      try {
        items.value = JSON.parse(savedCart)
        console.log(`Cart loaded for user: ${targetUserId}`, items.value)
      } catch (error) {
        console.error("Error parsing saved cart:", error)
        items.value = []
      }
    } else {
      items.value = []
      console.log(`No saved cart found for user: ${targetUserId}`)
    }

    // Update current user ID
    currentUserId.value = targetUserId
  }

  // Initialize cart for a specific user (called when user logs in)
  function initializeUserCart(userId) {
    console.log(`Initializing cart for user: ${userId}`)

    // Check if this is a different user
    const previousUserId = localStorage.getItem("cart_current_user")

    if (previousUserId && previousUserId !== userId.toString()) {
      console.log(`Different user detected. Previous: ${previousUserId}, Current: ${userId}`)
      // Clear current cart items
      items.value = []
    }

    // Load cart for the new user
    loadCart(userId)

    // Update current user reference
    localStorage.setItem("cart_current_user", userId.toString())
  }

  // Clear cart from memory but keep in localStorage (called when user logs out)
  function clearUserCartFromMemory() {
    console.log("Clearing cart from memory (logout)")

    // Save current cart before clearing from memory
    if (items.value.length > 0) {
      saveCart()
    }

    // Clear from memory but keep in localStorage
    items.value = []
    currentUserId.value = null
  }

  // Permanently delete cart from localStorage (called when switching users)
  function deleteUserCart(userId) {
    const cartKey = getCartKey(userId)
    localStorage.removeItem(cartKey)
    console.log(`Cart permanently deleted for user: ${userId}`)
  }

  // Check if cart belongs to current user and handle user changes
  function validateCartUser() {
    const currentUser = auth.user?.id || auth.user?.userId
    const savedCurrentUser = localStorage.getItem("cart_current_user")

    console.log(`Validating cart user. Current: ${currentUser}, Saved: ${savedCurrentUser}`)

    // If user has changed, handle the transition
    if (currentUser && savedCurrentUser && currentUser.toString() !== savedCurrentUser) {
      console.log("User changed, switching cart...")
      initializeUserCart(currentUser)
    } else if (currentUser && !savedCurrentUser) {
      // First time login or no previous user
      console.log("First time login, loading user cart...")
      initializeUserCart(currentUser)
    } else if (currentUser && savedCurrentUser && currentUser.toString() === savedCurrentUser) {
      // Same user, load their cart
      console.log("Same user, loading existing cart...")
      loadCart(currentUser)
    }
  }

  // Migrate old cart data (for existing users)
  function migrateOldCart() {
    const oldCart = localStorage.getItem("cart")
    if (oldCart && !localStorage.getItem("cart_migrated")) {
      try {
        const oldCartData = JSON.parse(oldCart)
        if (oldCartData.length > 0) {
          // Save old cart as guest cart
          localStorage.setItem("cart_guest", oldCart)
        }
        // Remove old cart
        localStorage.removeItem("cart")
        // Mark as migrated
        localStorage.setItem("cart_migrated", "true")
        console.log("Old cart data migrated")
      } catch (error) {
        console.error("Error migrating old cart:", error)
      }
    }
  }

  // Initialize cart - check for user and load appropriate cart
  function initialize() {
    console.log("Initializing cart store...")

    // Migrate old cart data first
    migrateOldCart()

    // If user is logged in, validate and load their cart
    if (auth.user) {
      validateCartUser()
    } else {
      // No user logged in, clear current cart
      items.value = []
      currentUserId.value = null
    }
  }

  // Get cart for specific user (useful for debugging)
  function getUserCart(userId) {
    const cartKey = getCartKey(userId)
    const savedCart = localStorage.getItem(cartKey)
    return savedCart ? JSON.parse(savedCart) : []
  }

  // Initialize on store creation
  initialize()

  return {
    items,
    uniqueItemCount,
    totalQuantity,
    totalPrice,
    totalItems,
    addToCart,
    removeFromCart,
    updateQuantity,
    clearCart,
    initializeUserCart,
    clearUserCartFromMemory,
    deleteUserCart,
    validateCartUser,
    loadCart,
    getUserCart,
    saveCart,
  }
})
