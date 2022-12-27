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
const autoLoad = ref(true);
const photoMode = ref<"photoName" | undefined>("photoName");

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
const originalPhotoPageSize = 100;
const originalListPageSize = 10;

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
  scrollEnd,
}: TableUpdate) {
  currentPage.value = page - 1;

  if (scrollEnd && autoLoad.value) {
    currentPageSize.value += photoMode.value === "photoName" ? originalPhotoPageSize : originalListPageSize;
  }

  getPhotos();
}

function togglePhotoMode() {
  if (photoMode.value === "photoName") {
    photoMode.value = undefined;
    currentPageSize.value = 10;

  } else {
    photoMode.value = "photoName";
    currentPageSize.value = 25;
  }
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
      currentPageSize.value = photoMode.value === "photoName" ? originalPhotoPageSize : originalListPageSize;
    }
  }
)
</script>

<template>
  <main>
    <div class="jumbotron">
      <h1 class="display-4">{{ album }}</h1>
      <div class="btn-group">
        <button
          class="btn"
          :class="photoMode == 'photoName' ? 'btn-secondary': 'btn-primary'"
          @click="togglePhotoMode"
        >
          <font-awesome-icon icon="list"/>
        </button>
        <button
          class="btn"
          :class="photoMode == 'photoName' ? 'btn-primary': 'btn-secondary'"
          @click="togglePhotoMode"
        >
          <font-awesome-icon icon="image"/>
        </button>
      </div>
      <div>
        <BootstrapTable :columns="columns" :rows="photos" :totalRecordCount="totalRecordCount"
          :pageCount="currentPageSize" :loading="loading" :photo-mode="photoMode"
          :photo-url="`${siteURL}/trips/${props.album}/`" :auto-load="autoLoad" @update="handleUpdate">
          <template #col-photo="{ row }: { row: Photo }">
            <div>
              <img :src="`${siteURL}/trips/${props.album}/${row.photoName}`" :alt="row.photoDescription"
                style="object-fit: contain; width: 20vw; height: 40vw;" />
            </div>
          </template>
          <template #photo-photoName="{ row }: { row: Photo }">
            <div style="display: inline-block;  width: 20vw;">
              <label style="overflow: hidden;">{{ row.photoDescription }}</label>
              <div>
                <img class="img-thumbnail" :src="`${siteURL}/trips/${props.album}/${row.photoName}`"
                  :alt="row.photoDescription" style="object-fit: contain;" />
              </div>
            </div>
          </template>
        </BootstrapTable>
      </div>
    </div>
  </main>
</template>