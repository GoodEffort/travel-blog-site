<script setup lang="ts">
import { RouterView } from 'vue-router'
import { siteTitle } from './site-info';
import Navbar from './components/Navbar.vue';

document.title = siteTitle;
</script>

<template>
  <navbar />
  <router-view />
</template>
