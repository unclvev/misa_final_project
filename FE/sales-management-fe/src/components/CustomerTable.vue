<template>
  <!-- Sử dụng a-table của Ant Design Vue -->
  <a-table
    :columns="columns"
    :data-source="dataSource"
    :pagination="false"
    row-key="id"
    bordered
    size="middle"
    :scroll="{ x: 'max-content' }"
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
        {{ record[column.dataIndex] || '-' }}
      </template>
    </template>
  </a-table>
</template>

<script setup>
// Import icon cho cột điện thoại từ ant-design icons-vue
import { PhoneOutlined as IconPhone } from '@ant-design/icons-vue';
import { defineProps, computed } from 'vue';
const props = defineProps({
  dataSource: { type: Array, required: true, default: () => [] }
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
</style>
