<template>
  <div>
    <Header />
    <!-- SubHeader sử dụng AntD layout cho chính xác -->
    <div class="ant-page-header"
         style="display:flex;align-items:center;justify-content:space-between;background:#fff;padding:16px 24px 0 24px;height:68px;border-bottom:1px solid #ececec">
      <div style="display:flex;align-items:center;gap:8px">
        <span style="font-size:22px;font-weight:600">Khách hàng</span>
      </div>
      <div style="display:flex;align-items:center;gap:8px;">
        <!-- Ô tìm kiếm đặt tại đây -->
        <a-input-search allow-clear placeholder="Tìm kiếm thông minh..." style="width:320px" @search="onSearch"/>
        <a-button type="primary">+ Thêm</a-button>
        <a-button>Nhập từ Excel</a-button>
      </div>
    </div>
    <section class="section">
      <!-- Bảng danh sách khách hàng, có phân trang -->
      <CustomerTable :dataSource="dataPaging" />
      <!-- Footer tổng số và phân trang -->
      <div style="display:flex;justify-content:space-between;align-items:center;padding:10px 0 0 0">
        <span><b>Tổng số:</b> {{ total }} khách hàng</span>
        <a-pagination
          :current="current"
          :total="filteredCustomers.length"
          :pageSize="pageSize"
          show-size-changer
          :pageSizeOptions="['10', '20', '50', '100']"
          @change="onPageChange"
          @showSizeChange="onPageSizeChange"
        />
      </div>
    </section>
    <Footer />
  </div>
</template>

<script setup>
import Header from '../components/Header.vue';
import CustomerTable from '../components/CustomerTable.vue';
import Footer from '../components/Footer.vue';
import { ref, computed } from 'vue';

const search = ref('');
const customers = ref([
  {
    id: 1,
    type: 'NBH01',
    code: 'KH001-testvquan3',
    name: 'Công ty TNHH Hoa Mai',
    tax: '-',
    address: '-',
    phone: '0555555553',
    recentDate: '-',
    boughtGoods: '-',
    boughtNames: '-',
  },
  // { ... },
]);
// Bộ lọc tìm kiếm
const filteredCustomers = computed(() => {
  if (!search.value) return customers.value;
  return customers.value.filter(c =>
    c.name.toLowerCase().includes(search.value.toLowerCase())
    || c.code.toLowerCase().includes(search.value.toLowerCase())
    // ... trường khác nếu cần
  );
});
// Phân trang
const current = ref(1);
const pageSize = ref(20);
const total = computed(() => filteredCustomers.value.length);
const dataPaging = computed(() =>
  filteredCustomers.value.slice((current.value - 1) * pageSize.value, current.value * pageSize.value)
);
function onSearch(val) {
  search.value = val;
  current.value = 1;
}
function onPageChange(page) {
  current.value = page;
}
function onPageSizeChange(currentPage, size) {
  pageSize.value = size;
  current.value = 1;
}
</script>

<style scoped>
.section {
  background: #f6f8fa;
  min-height: 60vh;
  padding: 2rem 2rem 1rem 2rem;
}
</style>
