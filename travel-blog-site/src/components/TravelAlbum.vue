<script setup lang="ts">
defineProps<{
  album: string;
}>();
</script>

<template>
  <main>
    {{ album }}
  </main>
</template>