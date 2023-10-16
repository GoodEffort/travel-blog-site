<script setup lang="ts">
import { usePhotosStore } from '@/stores/photos';
import { storeToRefs } from 'pinia';

const store = usePhotosStore();
const { autoLoad } = storeToRefs(store);
</script>

<template>
    <button class="btn" :class="autoLoad ? 'btn-primary' : 'btn-secondary'" @click="autoLoad = !autoLoad">
        <font-awesome-icon icon="sync-alt" />
    </button>
</template>