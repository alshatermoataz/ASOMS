// src/stores/auth.js
import { defineStore } from 'pinia'
import axios from 'axios'

const API_BASE = 'https://asoms-production.up.railway.app'

export const useAuthStore = defineStore('auth', {
  state: () => ({
    user: null,
    token: null
  }),

  getters: {
    isLoggedIn: (state) => !!state.user && !!state.token
  },

  actions: {
    login(user, token) {
      this.user = user
      this.token = token
      localStorage.setItem('user', JSON.stringify(user))
      localStorage.setItem('token', token)
    },

    setUser(user) {
      this.user = user
      localStorage.setItem('user', JSON.stringify(user))
    },

    setToken(token) {
      this.token = token
      localStorage.setItem('token', token)
    },

    logout() {
      this.user = null
      this.token = null
      localStorage.removeItem('user')
      localStorage.removeItem('token')
    },

    restore() {
      const storedUser = localStorage.getItem('user')
      const storedToken = localStorage.getItem('token')
      if (storedUser && storedToken) {
        this.user = JSON.parse(storedUser)
        this.token = storedToken
      }
    },

    async fetchUser() {
      try {
        const res = await axios.get(`${API_BASE}/api/Users/profile`, {
          headers: {
            Authorization: `Bearer ${this.token}`
          }
        })
        this.setUser(res.data)
        return res.data
      } catch (err) {
        console.error('Failed to fetch user profile:', err)
        throw err
      }
    },

    // Upload profile picture using your API endpoint
    async uploadProfilePicture(file) {
      try {
        if (!this.user?.id) {
          throw new Error('User ID not found')
        }

        const formData = new FormData()
        formData.append('file', file)

        const response = await axios.post(
          `${API_BASE}/api/Users/${this.user.id}/upload-profile-picture`,
          formData,
          {
            headers: {
              'Content-Type': 'multipart/form-data',
              'Authorization': `Bearer ${this.token}`
            }
          }
        )

        // Update user profile picture URL in store
        if (response.data && response.data.url) {
          this.user.profilePictureUrl = response.data.url
          localStorage.setItem('user', JSON.stringify(this.user))
        }

        return response.data
      } catch (err) {
        console.error('Failed to upload profile picture:', err)
        throw err
      }
    },

    // Update user profile data via API
    async updateUserProfile(updatedFields) {
      try {
        if (!this.user?.id) {
          throw new Error('User ID not found')
        }

        const response = await axios.put(
          `${API_BASE}/api/Users/${this.user.id}`,
          updatedFields,
          {
            headers: {
              'Content-Type': 'application/json',
              'Authorization': `Bearer ${this.token}`
            }
          }
        )

        // Update local user data
        this.user = {
          ...this.user,
          ...updatedFields
        }
        localStorage.setItem('user', JSON.stringify(this.user))

        return response.data
      } catch (err) {
        console.error('Failed to update user profile:', err)
        throw err
      }
    },

    // Update user name specifically
    async updateUserName(newName) {
      try {
        const response = await axios.put(
          `${API_BASE}/api/Users/${this.user.id}`,
          {
            fullName: newName,
            email: this.user.email,
            // Include other required fields if needed
            contactNumber: this.user.contactNumber || '',
            address: this.user.address || '',
            // Add any other fields your API requires
          },
          {
            headers: {
              'Authorization': `Bearer ${this.token}`,
              'Content-Type': 'application/json'
            }
          }
        );
    
        // Update the user object with the new name
        if (this.user) {
          this.user.fullName = newName;
          localStorage.setItem('user', JSON.stringify(this.user));
        }
    
        return response.data;
      } catch (error) {
        console.error('Error updating user name:', error);
        throw error;
      }
    },

    // Local-only update methods (for immediate UI updates)
    updateUserProfileLocal(updatedFields) {
      this.user = {
        ...this.user,
        ...updatedFields
      }
      localStorage.setItem('user', JSON.stringify(this.user))
    },

    updateProfilePictureLocal(url) {
      if (this.user) {
        this.user.profilePictureUrl = url
        localStorage.setItem('user', JSON.stringify(this.user))
      }
    },

    updateUserNameLocal(name) {
      if (this.user) {
        this.user.fullName = name
        localStorage.setItem('user', JSON.stringify(this.user))
      }
    },

    // Fetch user orders for statistics
    async fetchUserOrders() {
      try {
        if (!this.user?.id) {
          throw new Error('User ID not found')
        }

        const response = await axios.get(
          `${API_BASE}/api/Orders/user/${this.user.id}`,
          {
            headers: {
              'Authorization': `Bearer ${this.token}`
            }
          }
        )

        return response.data
      } catch (err) {
        console.error('Failed to fetch user orders:', err)
        throw err
      }
    },

    // Update user settings
    async updateUserSettings(settings) {
      try {
        if (!this.user?.id) {
          throw new Error('User ID not found')
        }

        // If you have a settings endpoint, use it
        // Otherwise, you can store settings locally or as part of user profile
        const response = await axios.put(
          `${API_BASE}/api/Users/${this.user.id}/settings`,
          settings,
          {
            headers: {
              'Content-Type': 'application/json',
              'Authorization': `Bearer ${this.token}`
            }
          }
        )

        return response.data
      } catch (err) {
        // If no settings endpoint exists, store locally
        console.warn('Settings endpoint not available, storing locally:', err)
        localStorage.setItem('userSettings', JSON.stringify(settings))
        return settings
      }
    }
  }
})