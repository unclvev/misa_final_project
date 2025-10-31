<script setup>
import { computed } from 'vue'

const props = defineProps({
  items: { type: Array, default: () => [] },
  loading: { type: Boolean, default: false },
  modelValue: { type: Array, default: () => [] },
})

const emit = defineEmits([
  'update:modelValue',
  'edit',
  'delete'
])

const allChecked = computed(() => {
  if (!props.items.length) return false
  return props.items.every(r => props.modelValue.includes(r.id))
})

function toggleAll(e) {
  const checked = e.target.checked
  const next = checked ? props.items.map(r => r.id) : []
  emit('update:modelValue', next)
}

function toggleOne(id, e) {
  const checked = e.target.checked
  const set = new Set(props.modelValue)
  if (checked) set.add(id); else set.delete(id)
  emit('update:modelValue', Array.from(set))
}

function formatDate(d) {
  if (!d) return ''
  const dt = new Date(d)
  if (isNaN(dt)) return d
  const dd = String(dt.getDate()).padStart(2, '0')
  const mm = String(dt.getMonth() + 1).padStart(2, '0')
  const yy = dt.getFullYear()
  return `${dd}/${mm}/${yy}`
}
</script>

<template>
  <div class="table-container flex-1 overflow-y-auto">
    <table class="w-full">
      <thead class="sticky">
        <tr>
          <th class="text-left">
            <input type="checkbox" :checked="allChecked" @change="toggleAll" />
          </th>
          <th class="text-left">Loại khách hàng</th>
          <th class="text-left">Mã khách hàng</th>
          <th class="text-left">Tên khách hàng</th>
          <th class="text-left">Mã số thuế</th>
          <th class="text-left">Địa chỉ (Giao hàng)</th>
          <th class="text-left">Điện thoại</th>
          <th class="text-left">Ngày mua gần nhất</th>
          <!-- <th class="text-left">Email</th> -->
          <th class="text-left">Thao tác</th>
        </tr>
      </thead>

      <tbody v-if="!loading && items.length">
        <tr v-for="c in items" :key="c.id">
          <td>
            <input type="checkbox" :checked="modelValue.includes(c.id)" @change="e => toggleOne(c.id, e)" />
          </td>
          <td>{{ c.type || '—' }}</td>
          <td>{{ c.code || '—' }}</td>
          <td>{{ c.name || '—' }}</td>
          <td>{{ c.tax || '—' }}</td>
          <td>{{ c.address || '—' }}</td>
          <td>{{ c.phone || '—' }}</td>
          <td>{{ formatDate(c.recentDate) }}</td>
          <!-- <td>{{ c.email }}</td> -->
          <td>
            <button class="btn-action" @click="$emit('edit', c)">Sửa</button>
            <button class="btn-action" @click="$emit('delete', c)">Xóa</button>
          </td>
        </tr>
      </tbody>

      <tbody v-else-if="loading">
        <tr>
          <td colspan="9" class="empty">Đang tải…</td>
        </tr>
      </tbody>

      <tbody v-else>
        <tr>
          <td colspan="9" class="empty">Không có dữ liệu</td>
        </tr>
      </tbody>
    </table>
  </div>
  
</template>

<style scoped>
.table-container {
  border: 1px solid #e5e7eb;
  border-radius: 0;
  overflow: hidden;
  margin: 0;
}

table {
  width: 100%;
  border-collapse: collapse;
}

thead {
  background: #f9fafb;
}

th {
  padding: .75rem 1rem;
  text-align: left;
  font-weight: 600;
  color: #374151;
  font-size: .875rem;
  border-bottom: 1px solid #e5e7eb;
}

td {
  padding: .75rem 1rem;
  border-bottom: 1px solid #f3f4f6;
  font-size: .875rem;
  color: #374151;
}

tbody tr:hover {
  background: #f9fafb;
}

.btn-action {
  background: #3b82f6;
  color: #fff;
  border: none;
  padding: .25rem .5rem;
  border-radius: 4px;
  font-size: .75rem;
  margin-right: .25rem;
  cursor: pointer;
  transition: background .2s;
}

.btn-action:hover {
  background: #2563eb;
}

.sticky {
  position: sticky;
  top: 0;
  z-index: 10;
}

.empty {
  text-align: center;
  color: #6b7280;
}

.w-full {
  width: 100%;
}
</style>


