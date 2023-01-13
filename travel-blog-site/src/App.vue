<script setup lang="ts">
import { RouterView } from 'vue-router'
import { siteTitle } from './site-info';
import Navbar from './components/Navbar.vue';

document.title = siteTitle;
</script>

<template>
  <div class="row" style="margin-left: 0px; margin-right: 0px;">
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
