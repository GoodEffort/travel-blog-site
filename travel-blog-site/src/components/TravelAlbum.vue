<script setup lang="ts">
import { ref, watch } from 'vue';
import { usePhotosStore } from '@/stores/photos';
import { storeToRefs } from 'pinia';
import BootstrapTable from "@/components/BootstrapTable/BootstrapTable.vue"
import type { TableColumn, TableUpdate } from './BootstrapTable/table-types';
import type { Photo } from '@/clients/data-contracts';
import { siteURL } from '@/site-info';
import { useToast } from 'vue-toast-notification';

const store = usePhotosStore();
const toast = useToast();

const props = defineProps<{
  album: string;
}>();

const {
  photos,
  currentPage,
  totalRecordCount,
  currentPageSize,
  autoLoad,
} = storeToRefs(store);

const loading = ref(false);

// doesn't need to be reactive
const columns: TableColumn[] = [
  {
    key: "photoDescription",
    name: "Description",
    sortable: true,
    filterable: true,
  },
  {
    key: "photo",
  }
];
const originalPhotoPageSize = 25;

async function getPhotos() {
  try {
    loading.value = true;
    await store.getPhotos(props.album, currentPage.value, currentPageSize.value);
  } catch (error) {
    console.error(error);
    if (error instanceof Error) {
      toast.error(error.message);
    }
  } finally {
    loading.value = false;
  }
}

function handleUpdate({
  page,
  scrollEnd,
}: TableUpdate) {
  currentPage.value = page - 1;

  if (scrollEnd && autoLoad.value) {
    currentPageSize.value += originalPhotoPageSize;
  }

  getPhotos();
}

watch(
  () => props.album,
  () => {
    getPhotos();
  },
  { immediate: true }
)

watch(
  autoLoad,
  () => {
    if (!autoLoad.value) {
      currentPageSize.value = originalPhotoPageSize;
    }
    
    getPhotos();
  }
)
</script>

<template>
  <main>
    <div class="jumbotron top-margin">
      <BootstrapTable :columns="columns" :rows="photos" :totalRecordCount="totalRecordCount"
        :pageCount="currentPageSize" :loading="loading" photo-mode="photoName"
        :photo-url="`${siteURL}/trips/${props.album}/`" :auto-load="autoLoad" @update="handleUpdate">
        <template #photo-photoName="{ row }: { row: Photo }">
          <div class="image-div">
            <img class="img-thumbnail" :src="`${siteURL}/trips/${props.album}/${row.photoName}`"
              :alt="row.photoDescription" loading="lazy"/>
          </div>
        </template>
      </BootstrapTable>
    </div>
  </main>
</template>

<style scoped>
img.img-thumbnail {
  object-fit: contain;
  height: 300px;
  z-index: 1;
}

img.img-thumbnail:hover {
  position: relative;
  z-index: 10;
  transition: transform .2s;
  transform: scale(1.1);
}

div.image-div {
  display: inline-block;
  width: 25%;
  height: 300px;
  padding-left: .5em;
  padding-right: .5em;
  margin-bottom: 1em;
  text-align: center;
}

div.jumbotron.top-margin {
  margin-top: 5em;
}
</style>