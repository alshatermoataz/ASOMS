// src/stores/auth.js

import { defineStore } from 'pinia'
import axios from 'axios'

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
        const res = await axios.get('/api/user/profile', {
          headers: {
            Authorization: `Bearer ${this.token}`
          }
        })
        this.setUser(res.data)
      } catch (err) {
        console.error('Failed to fetch user profile:', err)
      }
    },

    updateUserProfile(updatedFields) {
      this.user = {
        ...this.user,
        ...updatedFields
      }
      localStorage.setItem('user', JSON.stringify(this.user))
    },

    updateProfilePicture(url) {
      if (this.user) {
        this.user.profilePictureUrl = url
        localStorage.setItem('user', JSON.stringify(this.user))
      }
    },

    updateUserName(name) {
      if (this.user) {
        this.user.fullName = name
        localStorage.setItem('user', JSON.stringify(this.user))
      }
    }
  }
})
