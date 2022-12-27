<script setup lang="ts">
import { RouterView } from 'vue-router'
import Navbar from './components/Navbar.vue';
</script>

<template>
  <div class="row">
    <div class="col-2">
      <navbar />
    </div>

    <div class="col">
      <router-view />
    </div>
  </div>
</template>

<style scoped>

</style>
