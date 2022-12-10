<script setup lang="ts">
import { RouterLink } from 'vue-router';
</script>

<template>
  <header>
    <div>
      <RouterLink to="/">Home</RouterLink>
      <RouterLink to="/trip/Iceland">Iceland</RouterLink>
      <RouterLink to="/trip/Scotland">Scotland</RouterLink>
      <RouterLink to="/trip/England">England</RouterLink>
    </div>
  </header>
</template>