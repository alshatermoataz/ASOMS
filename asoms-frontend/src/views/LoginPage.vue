<template>
  <div class="page-container">
    <div class="status-bar">
      <span>9:41</span>
      <div>
        <span>Π</span>
      </div>
    </div>
    
    <div class="login-container">
      <div class="logo-small">
        <span>B</span>
      </div>
      
      <h1 class="welcome-text">Welcome<br>Back!</h1>
      
      <form class="login-form" @submit.prevent="handleLogin">
        <div class="form-group">
          <label for="email" class="input-label">Email</label>
          <input 
            type="email" 
            id="email" 
            class="input-field" 
            v-model="email" 
            required
          />
        </div>
        
        <div class="form-group">
          <label for="password" class="input-label">Password</label>
          <input 
            type="password" 
            id="password" 
            class="input-field" 
            v-model="password" 
            required
          />
        </div>

        <p v-if="error" class="text-red-500 text-sm">{{ error }}</p>
        
        <button type="submit" class="btn-primary">
          Sign in
          <font-awesome-icon icon="arrow-right" class="ml-2" />
        </button>
      </form>
      
      <router-link to="/" class="back-link">Back</router-link>
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '../stores/auth'
import axios from 'axios'

const email = ref('')
const password = ref('')
const error = ref('')
const router = useRouter()
const auth = useAuthStore()

const handleLogin = async () => {
  error.value = ''
  try {
    const response = await axios.post('https://localhost:7004/api/auth/login', {
      email: email.value,
      password: password.value
    })

    const { token, user } = response.data
    auth.login(user, token)
    router.push('/home')
  } catch (err) {
    error.value = err.response?.data?.message || 'Login failed. Please check your credentials.'
  }
}
</script>

<style scoped>
.login-container {
  padding: 20px;
  display: flex;
  flex-direction: column;
  height: calc(100vh - 40px);
}

.logo-small {
  width: 50px;
  height: 50px;
  background-color: var(--primary-color);
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  font-weight: bold;
  font-size: 20px;
  margin-bottom: 30px;
  margin-top: 20px;
}

.welcome-text {
  font-size: 32px;
  font-weight: 700;
  margin-bottom: 40px;
  line-height: 1.2;
}

.login-form {
  display: flex;
  flex-direction: column;
  gap: 20px;
  margin-bottom: 30px;
}

.form-group {
  margin-bottom: 15px;
}

.ml-2 {
  margin-left: 8px;
}
</style>
