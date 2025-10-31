<template>
  <!-- Sử dụng a-table của Ant Design Vue -->
  <a-table
    :columns="columns"
    :data-source="displayData"
    :pagination="false"
    row-key="id"
    bordered
    size="middle"
    :scroll="{ x: 'max-content', y: effectiveBodyHeight }"
    :locale="{ emptyText: '' }"
    :row-class-name="getRowClassName"
  >
    <!-- Slot cho cột số điện thoại: thêm icon và hiển thị đẹp -->
    <template #bodyCell="{ column, record }">
      <template v-if="column.dataIndex === 'phone'">
        <a-tooltip :title="record.phone ? record.phone : ''">
          <span v-if="record.phone">
            <a style="color: #1996ff; margin-right: 3px"><icon-phone /></a>{{ record.phone }}
          </span>
          <span v-else>-</span>
        </a-tooltip>
      </template>
      <template v-else-if="column.dataIndex === 'name'">
        <a-tooltip :title="record.name"><span style="max-width:120px;display:inline-block;overflow:hidden;text-overflow:ellipsis;white-space:nowrap">{{ record.name }}</span></a-tooltip>
      </template>
      <template v-else-if="column.dataIndex === 'address'">
        <span>{{ record.address || '-' }}</span>
      </template>
      <template v-else-if="column.dataIndex === 'recentDate'">
        <span>{{ record.recentDate || '-' }}</span>
      </template>
      <template v-else-if="column.dataIndex === 'boughtGoods'">
        <span>{{ record.boughtGoods || '-' }}</span>
      </template>
      <template v-else-if="column.dataIndex === 'boughtNames'">
        <span style="max-width:120px;display:inline-block;overflow:hidden;text-overflow:ellipsis;white-space:nowrap" :title="record.boughtNames">{{ record.boughtNames || '-' }}</span>
      </template>
      <template v-else>
        <span v-if="!record.__placeholder">{{ record[column.dataIndex] || '-' }}</span>
        <span v-else></span>
      </template>
    </template>
  </a-table>
</template>

<script setup>
// Import icon cho cột điện thoại từ ant-design icons-vue
import { PhoneOutlined as IconPhone } from '@ant-design/icons-vue';
import { defineProps, ref, onMounted, onBeforeUnmount, computed } from 'vue';
const props = defineProps({
  dataSource: { type: Array, required: true, default: () => [] },
  pageSize: { type: Number, default: 20 },
  bodyHeight: { type: Number, default: 0 }
});

// Khai báo các cột bảng đúng thứ tự như trong ảnh mẫu
const columns = [
  { title: 'Loại khách hàng', dataIndex: 'type', width: 110 },
  { title: 'Mã khách hàng', dataIndex: 'code', width: 160 },
  { title: 'Tên khách hàng', dataIndex: 'name', width: 280 },
  { title: 'Mã số thuế', dataIndex: 'tax', width: 140 },
  { title: 'Địa chỉ (Giao hàng)', dataIndex: 'address', width: 220 },
  { title: 'Điện thoại', dataIndex: 'phone', width: 160 },
  { title: 'Ngày mua hàng gần nhất', dataIndex: 'recentDate', width: 180 },
  { title: 'Hàng hóa đã mua', dataIndex: 'boughtGoods', width: 210 },
  { title: 'Tên hàng hóa đã mua', dataIndex: 'boughtNames', width: 300 },
];

// Tính chiều cao phần thân bảng để cân đối theo màn hình
const tableBodyHeight = ref(420);
function calculateTableBodyHeight() {
  // Trừ đi chiều cao header, subheader, padding, thanh phân trang, footer (ước lượng)
  const viewportH = window.innerHeight || 800;
  const estimatedOtherHeights = 260; // điều chỉnh nếu cần
  const height = Math.max(280, viewportH - estimatedOtherHeights);
  tableBodyHeight.value = height;
}
onMounted(() => {
  calculateTableBodyHeight();
  window.addEventListener('resize', calculateTableBodyHeight);
});
onBeforeUnmount(() => {
  window.removeEventListener('resize', calculateTableBodyHeight);
});

const effectiveBodyHeight = computed(() => {
  return props.bodyHeight && props.bodyHeight > 0 ? props.bodyHeight : tableBodyHeight.value;
});

// Bổ sung các hàng trống để phần còn lại là khoảng trắng nếu ít dữ liệu
const displayData = computed(() => {
  const data = Array.isArray(props.dataSource) ? props.dataSource : [];
  const missing = Math.max(0, props.pageSize - data.length);
  if (missing === 0) return data;
  const placeholders = Array.from({ length: missing }, (_, idx) => ({
    id: `placeholder-${idx}`,
    __placeholder: true
  }));
  return [...data, ...placeholders];
});

function getRowClassName(record) {
  return record.__placeholder ? 'placeholder-row' : '';
}
</script>

<style scoped>
.customer-table {
  width: 100%;
  border-collapse: collapse;
  margin-top: 8px;
}
.customer-table th, .customer-table td {
  border: 1px solid #ececec;
  padding: 8px 10px;
}
.customer-table th {
  background: #f1f6fb;
  font-weight: 600;
}
.customer-table tr:hover {
  background: #f5fbff;
  cursor: pointer;
}
/* Hàng trống không có hover và nền giữ trắng để tạo cảm giác khoảng trắng */
:deep(.placeholder-row) td {
  background: #fff !important;
}
:deep(.placeholder-row):hover > td {
  background: #fff !important;
}
</style>
