<script setup lang="ts">
import { RouterLink, useRoute } from 'vue-router';
import { defineAsyncComponent, computed, watch } from 'vue';
import { usePhotosStore } from '@/stores/photos';
import NightModeToggle from '@/components/NightModeToggle.vue';
import AutoLoadToggle from './AutoLoadToggle.vue';

const { clearPhotos } = usePhotosStore();

const CountryFlag = defineAsyncComponent(() => import('vue-country-flag-next'));

const route = useRoute();
const selected = computed(() => route.name || route.path.split('/').reverse()[0]);

watch(route, () => {
  clearPhotos();
});

</script>

<template>
  <nav class="navbar navbar-expand-lg fixed-top">
    <a class="navbar-brand" href="#">Luke Hovarter</a>
    <ul class="navbar-nav">
      <li class="nav-item" :class="{ active: selected === 'Home' }">
        <router-link to="/">
          <font-awesome-icon icon="home" />
          &nbsp;
          <span class="label">Home</span>
        </router-link>
      </li>
      <li class="nav-item" :class="{ active: selected === 'Iceland' }">
        <router-link to="/trip/Iceland">
          <country-flag :country="'IS'" :size="'small'" :rounded="true" />
          &nbsp;
          <span class="label">Iceland</span>
        </router-link>
      </li>
      <li class="nav-item" :class="{ active: selected === 'Scotland' }">
        <router-link to="/trip/Scotland">
          <country-flag :country="'gb-sct'" :size="'small'" :rounded="true" />
          &nbsp;
          <span class="label">Scotland</span>
        </router-link>
      </li>
      <li class="nav-item" :class="{ active: selected === 'England' }">
        <router-link to="/trip/England">
          <country-flag :country="'gb-eng'" :size="'small'" :rounded="true" />
          &nbsp;
          <span class="label">England</span>
        </router-link>
      </li>
    </ul>
    <div>
      <auto-load-toggle id="autoload" />
      <night-mode-toggle />
    </div>
  </nav>
</template>

<style scoped>
a:link {
  text-decoration: none;
}

.sidebar {
  box-shadow: 0 2px 5px 0 #0000000d, 0 2px 10px 0 #0000000d;
}

.sidebar .active {
  box-shadow: 0 2px 5px 0 #00000029, 0 2px 10px 0 #0000001f;
}

.sidebar-sticky {
  position: relative;
  top: 0;
  height: calc(100vh - 48px);
  padding-top: 0.5rem;
  overflow-x: hidden;
  overflow-y: auto;
}

nav.navbar {
  justify-content: space-between;
  padding-left: 1em;
  padding-right: 1em;
}

nav.navbar li.nav-item:last-child {
  margin-left: 1em;
}

nav.navbar li.nav-item:not(:last-child)::after {
  margin-left: 1em;
  content: '|';
  display: inline;
  width: 0;
  height: 2px;
  color: white;
  transition: width .3s;
}

nav.navbar li.nav-item.active > a > span {
  color: hsl(25, 70%, 45%);
  border-bottom: hsl(25, 70%, 45%) 2px solid;
}

nav.navbar li.nav-item {
  width: 7em;
}

nav.navbar a.navbar-brand {
  margin-right: 0px;
}

#autoload {
  margin-right: 0.5em;
}

nav.navbar {
  background-color: white;
  border-bottom: 1px solid black;
}

html.dark nav.navbar {
  background-color: #000000;
  border-bottom: 1px solid #5f5f5f;
}
</style>