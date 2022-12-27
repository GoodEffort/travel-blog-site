<script setup lang="ts">
import { ref } from 'vue';
import { usePhotosStore } from '@/stores/photos';

const store = usePhotosStore();

const tripName = ref("");

function generatePhotos() {
    if (tripName.value !== "") {
        store.generatePhotos(tripName.value);
    }
}
</script>

<template>
    <div>
        <input class="form-control" type="text" placeholder="Trip Name" v-model="tripName" />
        <button class="btn btn-primary" @click="generatePhotos">Generate Photos</button>
    </div>
</template>