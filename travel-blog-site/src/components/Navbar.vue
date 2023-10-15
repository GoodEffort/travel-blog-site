<script setup lang="ts">
import { RouterLink, useRoute } from 'vue-router';
import { defineAsyncComponent, computed, watch } from 'vue';
import { usePhotosStore } from '@/stores/photos';

const { clearPhotos } = usePhotosStore();

const CountryFlag = defineAsyncComponent(() => import('vue-country-flag-next'));

const route = useRoute();
const selected = computed(() => route.name || route.path.split('/').reverse()[0]);

watch(route, () => {
  clearPhotos();
});

</script>

<template>
  <nav class="d-lg-block sidebar bg-white">
    <div class="position-sticky">
      <div class="list-group list-group-flush">
        <router-link to="/">
          <a href="#" class="list-group-item list-group-item-action py-2 ripple" aria-current="true"
            :class="{ active: selected === 'Home' }">
            <div>
              <font-awesome-icon icon="home" /> Home
            </div>
          </a>
        </router-link>

        <hr />

        <router-link to="/trip/Iceland">
          <a href="#" class="list-group-item list-group-item-action py-2 ripple"
            :class="{ active: selected === 'Iceland' }">
            <div>
              <country-flag :country="'IS'" :size="'small'" :rounded="true" /> Iceland
            </div>
          </a>
        </router-link>

        <router-link to="/trip/Scotland">
          <a href="#" class="list-group-item list-group-item-action py-2 ripple"
            :class="{ active: selected === 'Scotland' }">
            <div>
              <country-flag :country="'gb-sct'" :size="'small'" :rounded="true" /> Scotland
            </div>
          </a>
        </router-link>

        <router-link to="/trip/England">
          <a href="#" class="list-group-item list-group-item-action py-2 ripple"
            :class="{ active: selected === 'England' }">
            <div>
              <country-flag :country="'gb-eng'" :size="'small'" :rounded="true" /> England
            </div>
          </a>
        </router-link>
      </div>
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
</style>