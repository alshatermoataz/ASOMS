<template>
  <div class="page-container">
    <div class="status-bar">
      <span>9:41</span>
      <div>
        <span>Π</span>
      </div>
    </div>
    
    <div class="signup-container">
      <h1 class="signup-title">Create an<br>account</h1>
      
      <form class="signup-form" @submit.prevent="handleSignup">
        <div class="form-group">
          <label for="fullName" class="input-label">Full name</label>
          <input 
            type="text" 
            id="fullName" 
            class="input-field" 
            v-model="fullName" 
            required
          />
        </div>
        
        <div class="form-group">
          <label for="phone" class="input-label">Phone number</label>
          <input 
            type="tel" 
            id="phone" 
            class="input-field" 
            v-model="phone" 
            required
          />
        </div>
        
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
        
        <div class="form-group">
          <label for="confirmPassword" class="input-label">Confirm Password</label>
          <input 
            type="password" 
            id="confirmPassword" 
            class="input-field" 
            v-model="confirmPassword" 
            required
          />
        </div>
        
        <!-- <div class="form-group">
          <label for="address" class="input-label">Address</label>
          <input 
            type="text" 
            id="address" 
            class="input-field" 
            v-model="address" 
            required
          />
        </div> -->
        
        <p v-if="error" class="text-red-500 text-sm">{{ error }}</p>

        <button type="submit" class="btn-primary">
          Sign Up
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
import axios from 'axios'
import { useAuthStore } from '../stores/auth'

const router = useRouter()
const auth = useAuthStore()

const fullName = ref('')
const phone = ref('')
const email = ref('')
const password = ref('')
const confirmPassword = ref('')
const error = ref('')

const handleSignup = async () => {
  error.value = ''

  if (password.value !== confirmPassword.value) {
    error.value = "Passwords do not match."
    return
  }

  try {
    const response = await axios.post('https://localhost:7004/api/auth/register', {
      FullName: fullName.value,
      Email: email.value,
      Password: password.value,
      ContactNumber: phone.value,
      Role: 'Customer'
    })

    const { token, user } = response.data
    auth.login(user, token)
    router.push('/home')
  } catch (err) {
    error.value = err.response?.data?.message || 'Signup failed. Please check your input.'
  }
}
</script>



<style scoped>
.signup-container {
  padding: 20px;
}

.signup-title {
  font-size: 32px;
  font-weight: 700;
  margin-bottom: 30px;
  line-height: 1.2;
}

.signup-form {
  display: flex;
  flex-direction: column;
  gap: 15px;
  margin-bottom: 30px;
}

.ml-2 {
  margin-left: 8px;
}
</style>