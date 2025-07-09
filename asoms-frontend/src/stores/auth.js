// src/stores/auth.js
import { defineStore } from "pinia"
import axios from "axios"

const API_BASE = "https://asoms-production.up.railway.app"

// Helper to safely parse JSON
function safeJSONParse(value, fallback = null) {
  try {
    return JSON.parse(value)
  } catch (_) {
    return fallback
  }
}

export const useAuthStore = defineStore("auth", {
  state: () => ({
    user: null,
    token: null,
  }),

  getters: {
    isLoggedIn: (state) => !!state.user && !!state.token,
  },

  actions: {
    login(user, token) {
      console.log("User logging in:", user.id || user.userId)

      this.user = user
      this.token = token
      localStorage.setItem("user", JSON.stringify(user))
      localStorage.setItem("token", token)

      // Initialize user-specific cart after login
      this.initializeUserCart(user)
    },

    setUser(user) {
      this.login(user, this.token)
    },

    setToken(token) {
      this.login(this.user, token)
    },

    logout() {
      console.log("User logging out:", this.user?.id || this.user?.userId)

      // Clear cart from memory but keep it saved for this user
      this.clearUserCartFromMemory()

      this.user = null
      this.token = null
      localStorage.removeItem("user")
      localStorage.removeItem("token")
      // Don't remove cart_current_user here - keep it for potential re-login
    },

    restore() {
      const rawUser = localStorage.getItem("user")
      const rawToken = localStorage.getItem("token")
      const parsedUser = rawUser ? safeJSONParse(rawUser) : null

      if (parsedUser && rawToken) {
        console.log("Restoring user session:", parsedUser.id || parsedUser.userId)

        this.user = parsedUser
        this.token = rawToken

        // Initialize cart for restored user
        this.initializeUserCart(parsedUser)
      } else {
        localStorage.removeItem("user")
        localStorage.removeItem("token")
      }
    },

    // Cart integration methods
    initializeUserCart(user) {
      // Dynamically import cart store to avoid circular dependency
      import("./useCartStore")
        .then(({ useCartStore }) => {
          const cartStore = useCartStore()
          cartStore.initializeUserCart(user.id || user.userId)
        })
        .catch((err) => {
          console.warn("Could not initialize cart store:", err)
        })
    },

    clearUserCartFromMemory() {
      // Dynamically import cart store to avoid circular dependency
      import("./useCartStore")
        .then(({ useCartStore }) => {
          const cartStore = useCartStore()
          cartStore.clearUserCartFromMemory()
        })
        .catch((err) => {
          console.warn("Could not clear cart store:", err)
        })
    },

    async fetchUser() {
      try {
        const res = await axios.get(`${API_BASE}/api/Users/profile`, {
          headers: {
            Authorization: `Bearer ${this.token}`,
          },
        })
        this.setUser(res.data)
        return res.data
      } catch (err) {
        console.error("Failed to fetch user profile:", err)
        throw err
      }
    },

    async uploadProfilePicture(file) {
      try {
        if (!this.user?.id) {
          throw new Error("User ID not found")
        }
        const formData = new FormData()
        formData.append("file", file)
        const response = await axios.post(`${API_BASE}/api/Users/${this.user.id}/upload-profile-picture`, formData, {
          headers: {
            "Content-Type": "multipart/form-data",
            Authorization: `Bearer ${this.token}`,
          },
        })
        if (response.data && response.data.url) {
          this.user.profilePictureUrl = response.data.url
          localStorage.setItem("user", JSON.stringify(this.user))
        }
        return response.data
      } catch (err) {
        console.error("Failed to upload profile picture:", err)
        throw err
      }
    },

    async updateUserProfile(updatedFields) {
      try {
        if (!this.user?.id) {
          throw new Error("User ID not found")
        }
        const response = await axios.put(`${API_BASE}/api/Users/${this.user.id}`, updatedFields, {
          headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${this.token}`,
          },
        })
        this.user = {
          ...this.user,
          ...updatedFields,
        }
        localStorage.setItem("user", JSON.stringify(this.user))
        return response.data
      } catch (err) {
        console.error("Failed to update user profile:", err)
        throw err
      }
    },

    async updateUserName(newName) {
      try {
        const response = await axios.put(
          `${API_BASE}/api/Users/${this.user.id}`,
          {
            fullName: newName,
            email: this.user.email,
            contactNumber: this.user.contactNumber || "",
            address: this.user.address || "",
          },
          {
            headers: {
              Authorization: `Bearer ${this.token}`,
              "Content-Type": "application/json",
            },
          },
        )
        if (this.user) {
          this.user.fullName = newName
          localStorage.setItem("user", JSON.stringify(this.user))
        }
        return response.data
      } catch (error) {
        console.error("Error updating user name:", error)
        throw error
      }
    },

    updateUserProfileLocal(updatedFields) {
      this.user = {
        ...this.user,
        ...updatedFields,
      }
      localStorage.setItem("user", JSON.stringify(this.user))
    },

    updateProfilePictureLocal(url) {
      if (this.user) {
        this.user.profilePictureUrl = url
        localStorage.setItem("user", JSON.stringify(this.user))
      }
    },

    updateUserNameLocal(name) {
      if (this.user) {
        this.user.fullName = name
        localStorage.setItem("user", JSON.stringify(this.user))
      }
    },

    async fetchUserOrders() {
      try {
        if (!this.user?.id) {
          throw new Error("User ID not found")
        }
        const response = await axios.get(`${API_BASE}/api/Orders/user/${this.user.id}`, {
          headers: {
            Authorization: `Bearer ${this.token}`,
          },
        })
        return response.data
      } catch (err) {
        console.error("Failed to fetch user orders:", err)
        throw err
      }
    },

    async updateUserSettings(settings) {
      try {
        if (!this.user?.id) {
          throw new Error("User ID not found")
        }
        const response = await axios.put(`${API_BASE}/api/Users/${this.user.id}/settings`, settings, {
          headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${this.token}`,
          },
        })
        return response.data
      } catch (err) {
        console.warn("Settings endpoint not available, storing locally:", err)
        localStorage.setItem("userSettings", JSON.stringify(settings))
        return settings
      }
    },
  },
})
