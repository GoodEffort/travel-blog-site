<script setup lang="ts">
import { RouterView } from 'vue-router'
import Navbar from './components/Navbar.vue';
</script>

<template>
  <div class="col">
    <navbar />
  </div>

  <div class="col">
    <router-view />
  </div>
</template>

<style scoped>

</style>
