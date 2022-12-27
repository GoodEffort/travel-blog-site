<script setup lang="ts">
import type { TableColumn, TableUpdate } from "./table-types";
import { debounce } from "lodash";
import { computed, ref, watch, useSlots, onBeforeUpdate, onUnmounted } from "vue";

const slots = useSlots();

const table = ref<HTMLTableElement>();
const larrow = ref<HTMLLIElement>();
const rarrow = ref<HTMLLIElement>();
const ellipsis = ref<HTMLLIElement>();
const list = ref<HTMLDivElement>();

const props = withDefaults(
  defineProps<{
    columns: TableColumn[],
    rows: any[],
    rowError?: (row: any) => boolean,
    bordered?: boolean,
    striped?: boolean,
    condensed?: boolean,
    totalRecordCount?: number,
    pageCount?: number,
    loading?: boolean,
    filter?: boolean,
    selectable?: boolean,
    multiselect?: boolean,
    initiallySelected?: any[] | any,
    photoMode?: string,
    photoUrl?: string,
    autoLoad?: boolean,
  }>(),
  {
    bordered: true,
    striped: true,
    condensed: true,
    pageCount: 10,
    loading: false,
    filter: false,
    selectable: false,
    multiselect: false,
    autoLoad: false,
  }
);

const emit = defineEmits<{
  (e: "update", value: TableUpdate ): void,
  (e: "add", value: any ): void,
  (e: "remove", value: any ): void,
  (e: "selected", value: any[] ): void,
}>();

const sortedColumn = ref<TableColumn>();
const desc = ref(false);
const currentPage = ref(0);
const maxWidth = ref(1080);
const arrowWidth = ref(35);
const pageBtnWidth = ref(61);
const ellipsisBtnWidth = ref(41);
const startingIndex = ref(0);
const filterObjects = ref<{ key: string, value: string }[]>([]);
const navWidthSet = ref(false);
const selectedRows = ref<any[]>();
const pageSize = ref(props.pageCount);

const pages = computed(() => {
  if (props.totalRecordCount) {
    return Math.ceil(props.totalRecordCount / pageSize.value);
  } else {
    return 1;
  }
});

const numberOfPagesToShow = computed(() => {
  let workingArea = maxWidth.value - arrowWidth.value * 2;

  if (startingIndex.value > 0) {
    workingArea -= ellipsisBtnWidth.value;
  }

  let pagesToShow = Math.floor(workingArea / pageBtnWidth.value);

  if (startingIndex.value + pagesToShow < pages.value) {
    workingArea -= ellipsisBtnWidth.value;
    pagesToShow = Math.floor(workingArea / pageBtnWidth.value);
  }

  return pagesToShow > pages.value ? pages.value : pagesToShow;
});

if (props.initiallySelected) {
  if (Array.isArray(props.initiallySelected)) {
    selectedRows.value = props.initiallySelected;
  } else {
    selectedRows.value = [props.initiallySelected];
  }
}

function toggleColumn(column: TableColumn) {
  if (!column.sortable && !props.loading) return;

  if (sortedColumn.value != column) {
    // sort by column asc
    sortedColumn.value = column;
    desc.value = false;
  } else if (!desc.value) {
    // sort by column desc
    desc.value = true;
  } else {
    sortedColumn.value = undefined; // clear sort
  }

  emitPageInfo();
};

function previousPage() {
  if (currentPage.value > 0 && !props.loading)
    changePage(currentPage.value - 1);
};

function nextPage() {
  if (currentPage.value < pages.value - 1 && !props.loading) {
    changePage(currentPage.value + 1);
  }
};

function changePage(index: number) {
  if (props.loading) return;

  currentPage.value = index;
  emitPageInfo();

  let workingArea = maxWidth.value;
  workingArea -= 2 * arrowWidth.value;

  if (
    index < startingIndex.value + 1 ||
    index > startingIndex.value + (numberOfPagesToShow.value - 1)
  ) {
    startingIndex.value = index;
  }

  if (startingIndex.value > 0) {
    workingArea -= ellipsisBtnWidth.value;
  }

  let pagesToShow = Math.floor(workingArea / pageBtnWidth.value);

  if (startingIndex.value + pagesToShow > pages.value) {
    startingIndex.value = pages.value - numberOfPagesToShow.value;
  }
};

const emitPageInfo = debounce(function(scrollEnd: boolean = false) {
  const updateVal: TableUpdate = {
    sortField: sortedColumn.value?.key ?? "",
    desc: desc.value,
    page: currentPage.value + 1,
    filters: filterObjects.value,
    scrollEnd,
  };
  emit("update", updateVal);
}, 250);

function validSlot(key: string) {
  return !!slots[`col-${key}`];
};

function validPhotoSlot(key: string) {
  return !!slots[`photo-${key}`];
};

const setMaxWidth = debounce(function () {
  if (table.value) maxWidth.value = table.value.clientWidth * 0.9;
}, 150);

function setNavWidth() {
  if (!navWidthSet.value && larrow.value) {
    arrowWidth.value = larrow.value ? larrow.value.clientWidth : 0;
    navWidthSet.value = true;
  }
};

function promptForNewPage() {
  var index = prompt(`Jump to what page? (1 - ${pages.value})`);
  if (
    index != null &&
    !isNaN(+index - 1) &&
    +index - 1 >= 0 &&
    +index - 1 < pages.value
  ) {
    changePage(+index - 1);
  }
};

const setFilter = debounce(function(value: string, key: string): void {
  let index;

  try {
    index = filterObjects.value.findIndex((e) => e.key == key);
  } catch {
    index = -1;
  }

  if (!value && index >= 0) {
    filterObjects.value.splice(index, 1);
  } else if (value && index >= 0) {
    filterObjects.value[index].value = value;
  } else if (value && index < 0) {
    filterObjects.value.push({ key, value });
  }

  emitPageInfo();
}, 250);

function isSelected(row: any) {
  const selectedIndex: number = selectedRows.value?.indexOf(row) ?? -1;
  return selectedIndex >= 0;
};

function toggleSelection(row: any) {
  if (isSelected(row)) {
    let index = selectedRows.value?.indexOf(row) ?? -1;

    if (index > -1) {
      selectedRows.value?.splice(index, 1);
    }

    emit("remove", row);
  } else if (props.multiselect) {
    selectedRows.value?.push(row);
    emit("add", row);
  } else {
    if (selectedRows.value?.length) {
      emit("remove", selectedRows.value[0]);
    }

    selectedRows.value = [row];
    emit("add", row);
  }
};

// was in create hook before, need to see if this still works
window.addEventListener("resize", setMaxWidth);

window.addEventListener("scroll",  () => {
  if (props.autoLoad) {
    const l = list.value;
    if (l && l.scrollTop + window.innerHeight + window.scrollY >= l.scrollHeight) {
      pageSize.value = pageSize.value + props.pageCount;
      emitPageInfo(true);
    }
  }
});

onBeforeUpdate(() => {
  setMaxWidth();
  setNavWidth();
});

onUnmounted(() => {
  window.removeEventListener("resize", setMaxWidth);
});

watch(
  selectedRows,
  (newVal) => {
    emit("selected", newVal || []);
  },
  { deep: true },
);

watch(
  () => props.pageCount,
  (newVal) => {
    pageSize.value = newVal;
  },
)

watch(
  () => props.autoLoad,
  (newVal) => {
    if (newVal) {
      currentPage.value = 0;
    }
    emitPageInfo();
  },
)
</script>

<template>
  <div ref="list">
    <div v-if="!props.photoMode">
      <table
        ref="table"
        class="table mt-1"
        :class="{
          'table-bordered': bordered,
          'table-striped': selectable ? false : striped,
          'table-condensed': condensed,
          'table-hover': selectable,
        }"
      >
        <thead>
          <tr>
            <th v-if="selectable"></th>
            <th
              v-for="column in columns"
              :key="column.key"
              :style="{
                cursor: column.sortable ? 'pointer' : undefined
              }"
              @click="toggleColumn(column)"
            >
              <span :class="{ 'me-1': column.sortable }">{{ column.name }}</span>
              <font-awesome-icon v-if="column.sortable && column != sortedColumn" icon="sort" />
              <font-awesome-icon v-else-if="column.sortable && !desc" icon="sort-down" />
              <font-awesome-icon v-else-if="column.sortable" icon="sort-up" />
            </th>
          </tr>
        </thead>

        <colgroup>
          <col v-if="selectable" />
          <col v-for="column in columns" :key="column.key" :width="!!column.width ? `${column.width}%` : undefined" />
        </colgroup>

        <tbody v-if="filter">
          <tr>
            <td v-if="selectable"></td>
            <td v-for="column in columns" :key="column.key">
              <input v-if="column.filterable" :key="column.key" class="form-control" type="text"
                @input="
                  ($event) =>
                    column.key ?
                      setFilter(
                        ($event.target as HTMLInputElement).value,
                        column.key
                      ) :
                      undefined
                "
                :readonly="loading" />
            </td>
          </tr>
        </tbody>

        <tbody v-if="loading">
          <tr v-for="index in pageSize" :key="index">
            <td v-if="selectable"></td>
            <td v-for="column in columns" :key="column.key">
              <p class="container placeholder-glow">
                <span class="placeholder col-12"></span>
              </p>
            </td>
          </tr>
        </tbody>
        <tbody v-else>
          <tr
            v-for="row in rows"
            :class="{
              'table-active': isSelected(row),
              'table-danger': rowError === undefined ? false : rowError(row),
            }"
            :key="JSON.stringify(row)"
          >
            <td v-if="selectable" class="fs-4" @click="toggleSelection(row)">
              <font-awesome-icon v-if="isSelected(row)" icon="check-square" style="cursor: pointer" />
              <font-awesome-icon v-else icon="square" style="cursor: pointer" />
            </td>
            <td v-for="column in columns" :key="column.key">
              <p v-if="loading" class="container placeholder-glow">
                <span class="placeholder col-12"></span>
              </p>
              <slot
                v-else-if="validSlot('' + column.key)"
                :name="`col-${column.key}`"
                :value="
                  column.key ? 
                    row[column.key]
                    : undefined
                "
                :row="row"
              />
              <div v-else-if="column.key && row[column.key] instanceof Date">
                {{ row[column.key].toLocaleDateString() }}
              </div>
              <div v-else>{{ column.key && row[column.key] }}</div>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
    <div v-else>
      <div v-for="row in rows" style="display: inline;">
        <slot v-if="validPhotoSlot(props.photoMode)" :name="`photo-${props.photoMode}`" :value="row[props.photoMode]" :row="row" />
        <img v-else-if="props.photoUrl" :src="props.photoUrl + row[props.photoMode]" />
        <div v-else>Must have a photoURL or slot in photomode</div>
      </div>
    </div>
  </div>
  <div class="container" v-if="rows.length === 0 && !loading">
    No Rows Found
  </div>
  <div v-if="pages > 1" class="px-0 text-center">
    <nav aria-label="table navigation" style="width: fit-content" class="m-auto">
      <ul class="pagination">
        <li class="page-item" ref="larrow">
          <a class="page-link" href="#" aria-label="Previous" @click="previousPage" :class="{ disabled: loading }">
            <span aria-hidden="true">&laquo;</span>
          </a>
        </li>
        <li v-if="startingIndex > 0" class="page-item">
          <a class="page-link" href="#" aria-label="Previous" @click="promptForNewPage"
            :class="{ disabled: loading }">
            <span aria-hidden="true">…</span>
          </a>
        </li>
        <li
          v-for="index in numberOfPagesToShow"
          :key="index"
          class="page-item"
          :class="{
            active: currentPage == startingIndex + index - 1,
            disabled: loading,
          }"
          :ref="`pagebtn${index}`"
          @click="changePage(startingIndex + (index - 1))"
        >
          <a class="page-link" href="#">{{ startingIndex + index }}</a>
        </li>
        <li v-if="startingIndex + numberOfPagesToShow < pages" class="page-item" ref="ellipsis">
          <a class="page-link" href="#" aria-label="Previous" @click="promptForNewPage"
            :class="{ disabled: loading }">
            <span aria-hidden="true">…</span>
          </a>
        </li>
        <li class="page-item" ref="rarrow">
          <a class="page-link" href="#" aria-label="Next" @click="nextPage" :class="{ disabled: loading }">
            <span aria-hidden="true">&raquo;</span>
          </a>
        </li>
      </ul>
    </nav>
  </div>
</template>
  
<style scoped>
li.page-item {
  width: 61px;
}

table {
  table-layout: auto;
}
</style>
  