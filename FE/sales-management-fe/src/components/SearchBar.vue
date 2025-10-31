<template>
  <form class="search-bar" @submit.prevent="emitSearch">
    <input v-model="searchVal" type="text" placeholder="Tìm kiếm thông minh..." class="search-input" />
    <button class="btn-search" type="submit">Tìm kiếm</button>
  </form>
</template>

<script setup>
import { ref, defineEmits } from 'vue';
const searchVal = ref('');
const emit = defineEmits(['search']);
function emitSearch() {
  emit('search', searchVal.value);
}
</script>

<style scoped>
.search-bar {
  display: flex;
  align-items: center;
  gap: 0.5rem;
}
.search-input {
  padding: 6px 12px;
  border: 1px solid #c2c2d6;
  border-radius: 5px;
  min-width: 220px;
}
.btn-search {
  background: #1996ff;
  color: #fff;
  border: none;
  border-radius: 4px;
  padding: 7px 16px;
  cursor: pointer;
  font-weight: 500;
}
.btn-search:hover {
  background: #1976d2;
}
</style>
