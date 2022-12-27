<script setup lang="ts">
import { ref, watch } from 'vue';
import { usePhotosStore } from '@/stores/photos';
import { storeToRefs } from 'pinia';
import BootstrapTable from "@/components/BootstrapTable/BootstrapTable.vue"
import type { TableColumn, TableUpdate } from './BootstrapTable/table-types';
import type { Photo } from '@/clients/data-contracts';
import { siteURL } from '@/site-info';

const store = usePhotosStore();

const props = defineProps<{
  album: string;
}>();

const {
  photos,
  currentPage,
  totalRecordCount,
  currentPageSize
} = storeToRefs(store);

const loading = ref(false);
// doesn't need to be reactive
const columns: TableColumn[] = [
  {
    key: "photoDescription",
    name: "Description",
    sortable: true,
    filterable: true
  },
  {
    key: "photo",
  }
];

async function getPhotos() {
  try {
    loading.value = true;
    await store.getPhotos(props.album, currentPage.value, currentPageSize.value);
  } catch (error) {
    console.error(error);
    if (error instanceof Error) {
      alert(error.message); // make a fancy toast?
    }
  } finally {
    loading.value = false;
  }
}

function handleUpdate({
  page,
}: TableUpdate) {
  currentPage.value = page - 1;
  getPhotos();
}

watch(
  () => props.album,
  () => {
    getPhotos();
  },
  { immediate: true }
)
</script>

<template>
  <main>
    <div class="jumbotron">
      <h1 class="display-4">{{ album }}</h1>
      <BootstrapTable
        :columns="columns"
        :rows="photos"
        :totalRecordCount="totalRecordCount"
        :pageCount="currentPageSize"
        :loading="loading"
        @update="handleUpdate"
      >
        <template #col-photo="{ row }: { row: Photo }">
          <img :src="`${siteURL}/trips/${props.album}/${row.photoName}`" :alt="row.photoDescription" />
        </template>
      </BootstrapTable>
    </div>
  </main>
</template>