<template>
  <div>
    <Header />
    <!-- SubHeader sử dụng AntD layout cho chính xác -->
    <div class="ant-page-header"
         style="display:flex;align-items:center;justify-content:space-between;background:#DDDDDD;padding:16px 24px 0 24px;height:68px;border-bottom:1px solid #ececec">
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
      <div class="table-card">
        <!-- Bảng danh sách khách hàng, có phân trang -->
        <div ref="tableWrap" class="table-wrap">
          <CustomerTable :dataSource="dataPaging" :pageSize="pageSize" :bodyHeight="bodyHeight" />
        </div>
        <!-- Footer phân trang ngay dưới bảng -->
        <div class="table-footer">
          <div class="footer-left">
            <span class="total"><b>Tổng số:</b> {{ total.toLocaleString('vi-VN') }}</span>
            <span class="divider">|</span>
            <span class="debt"><b>Công nợ</b> {{ debtDisplay }}</span>
          </div>
          <div class="footer-right">
            <a-select
              class="page-size"
              :value="pageSize"
              :options="pageSizeOptions"
              @change="onPageSizeSelect"
            />
            <a-pagination
              :current="current"
              :total="filteredCustomers.length"
              :pageSize="pageSize"
              :show-size-changer="false"
              @change="onPageChange"
            />
            <span class="range">{{ rangeStart }} đến {{ rangeEnd }}</span>
          </div>
        </div>
      </div>
    </section>
  </div>
</template>

<script setup>
import Header from '../components/Header.vue';
import CustomerTable from '../components/CustomerTable.vue';
import Footer from '../components/Footer.vue';
import { ref, computed, onMounted, onBeforeUnmount, nextTick } from 'vue';

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

// Hiển thị "Bản ghi trên trang" cho dropdown kích thước trang
const pageSizeOptions = computed(() => [10, 20, 50, 100].map(n => ({ value: n, label: `${n} Bản ghi trên trang` })));
function onPageSizeSelect(val) {
  pageSize.value = val;
  current.value = 1;
  nextTick().then(recalcBodyHeight);
}

// Dải hiển thị "1 đến 100"
const rangeStart = computed(() => (filteredCustomers.value.length === 0 ? 0 : (current.value - 1) * pageSize.value + 1));
const rangeEnd = computed(() => Math.min(filteredCustomers.value.length, current.value * pageSize.value));
const debtDisplay = computed(() => 'đ');

// Tính chiều cao phần thân bảng theo container để footer luôn hiển thị
const tableWrap = ref(null);
const bodyHeight = ref(0);
function recalcBodyHeight() {
  if (!tableWrap.value) return;
  // trừ footer ~56px và đường viền/padding trong card ~16px
  const footerReserve = 64;
  const available = tableWrap.value.clientHeight - footerReserve;
  bodyHeight.value = Math.max(200, available);
}
function onResize() {
  recalcBodyHeight();
}
onMounted(async () => {
  await nextTick();
  recalcBodyHeight();
  window.addEventListener('resize', onResize);
});
onBeforeUnmount(() => {
  window.removeEventListener('resize', onResize);
});
</script>

<style scoped>
.section {
  background: #f6f8fa;
  height: calc(100vh - 128px); /* 60px header + 68px subheader */
  overflow: hidden; /* khoá cuộn ở mức trang */
  padding: 2rem 2rem 0 2rem;
}
.table-card {
  background: #fff;
  border: 1px solid #ececec;
  border-radius: 4px;
  padding: 12px;
  height: 100%;
  display: flex;
  flex-direction: column;
}
.table-wrap {
  flex: 1 1 auto;
  min-height: 0;
  overflow: hidden; /* để a-table tự cuộn nội bộ */
}
.table-footer {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding-top: 8px;
  margin-top: 8px;
  border-top: 1px solid #ececec;
  flex: 0 0 auto;
}
.footer-left {
  display: flex;
  align-items: center;
  gap: 12px;
  color: #4a5568;
}
.footer-left .total b,
.footer-left .debt b { font-weight: 600; }
.footer-left .divider { color: #c0c4cc; }

.footer-right {
  display: flex;
  align-items: center;
  gap: 12px;
}
.footer-right .page-size { width: 180px; }
.footer-right .range { color: #4a5568; }
</style>
