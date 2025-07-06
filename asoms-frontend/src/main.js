import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
import { createPinia } from 'pinia'
import { useAuthStore } from './stores/auth'
import '@fortawesome/fontawesome-free/css/all.css'
import './assets/main.css'
import * as signalR from '@microsoft/signalr'

import { library } from '@fortawesome/fontawesome-svg-core'
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome'
import { fas } from '@fortawesome/free-solid-svg-icons' // Import all solid icons

library.add(fas) // Add all solid icons

const pinia = createPinia()
const app = createApp(App)

app.component('font-awesome-icon', FontAwesomeIcon)
app.use(pinia)

const auth = useAuthStore()
auth.restore()

// Create a SignalR connection
const connection = new signalR.HubConnectionBuilder()
  .withUrl('https://asoms-production.up.railway.app/hubs/notifications')
  .withAutomaticReconnect()
  .build()

// Add connection state logging
connection.onreconnecting(error => {
  console.log('SignalR: Reconnecting...', error);
});

connection.onreconnected(connectionId => {
  console.log('SignalR: Reconnected. ConnectionId:', connectionId);
});

connection.onclose(error => {
  console.log('SignalR: Connection closed.', error);
});

// Start the connection
connection.start()
  .then(() => {
    console.log('SignalR: Connected successfully. ConnectionId:', connection.connectionId);
  })
  .catch(err => {
    console.error('SignalR Connection Error:', err);
  });

// Make the connection available globally
app.config.globalProperties.$signalR = connection

app.use(router)
app.mount('#app')