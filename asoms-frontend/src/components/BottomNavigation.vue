<template>
  <div class="bottom-nav">
    <router-link
      to="/home"
      class="bottom-nav-item"
      :class="{ active: currentRoute === '/home' }"
    >
      <font-awesome-icon icon="home" class="bottom-nav-icon" />
      <span>Home</span>
    </router-link>
    <router-link
      to="/shop"
      class="bottom-nav-item"
      :class="{ active: currentRoute === '/shop' }"
    >
      <font-awesome-icon icon="shopping-cart" class="bottom-nav-icon" />
      <span>Shop</span>
    </router-link>
    <router-link
      to="/checkout"
      class="bottom-nav-item"
      :class="{ active: currentRoute === '/checkout' }"
    >
      <div class="cart-icon-container">
        <font-awesome-icon icon="shopping-bag" class="bottom-nav-icon" />
        <div v-if="cartItemCount > 0" class="cart-badge">
          {{ displayCount }}
        </div>
      </div>
      <span>Cart</span>
    </router-link>
    <router-link
      to="/history"
      class="bottom-nav-item"
      :class="{ active: currentRoute === '/history' }"
    >
      <font-awesome-icon icon="history" class="bottom-nav-icon" />
      <span>History</span>
    </router-link>
    <router-link
      to="/profile"
      class="bottom-nav-item"
      :class="{ active: currentRoute === '/profile' }"
    >
      <font-awesome-icon icon="user" class="bottom-nav-icon" />
      <span>Profile</span>
    </router-link>
  </div>
</template>

<script setup>
import { computed } from "vue";
import { useRoute } from "vue-router";
import { useCartStore } from "../stores/useCartStore";

const route = useRoute();
const cartStore = useCartStore();

const currentRoute = computed(() => route.path);

const cartItemCount = computed(() => {
  return cartStore.items.length;
});

const displayCount = computed(() => {
  return cartItemCount.value > 99 ? "99+" : cartItemCount.value;
});
</script>

<style scoped>
.bottom-nav {
  position: fixed;
  left: 50%;
  bottom: 0;
  transform: translateX(-50%);
  width: 100%;
  max-width: 430px;
  background-color: var(--card-bg);
  display: flex;
  justify-content: space-around;
  padding: 10px 0;
  box-shadow: 0 -2px 10px rgba(0, 0, 0, 0.1);
  z-index: 100;
  box-sizing: border-box;
}

.bottom-nav-item {
  display: flex;
  flex-direction: column;
  align-items: center;
  text-decoration: none;
  color: #cccccc;
  font-size: 12px;
  padding: 5px 0;
  width: 20%;
  transition: color 0.2s ease;
}

.bottom-nav-item.active {
  color: var(--primary-color);
}

.bottom-nav-icon {
  font-size: 20px;
  margin-bottom: 5px;
}

.cart-icon-container {
  position: relative;
}

.cart-badge {
  position: absolute;
  top: -8px;
  right: -8px;
  background-color: var(--primary-color);
  color: white;
  font-size: 10px;
  font-weight: bold;
  width: 18px;
  height: 18px;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
}
</style>
