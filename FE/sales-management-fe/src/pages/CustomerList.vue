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
        <!-- Ant Design Vue: a-input-search và a-button -->
        <a-input-search allow-clear placeholder="Tìm kiếm thông minh..." style="width:320px" @search="onSearch"/>
        <a-button type="primary" @click="goAdd">+ Thêm</a-button>
        <a-button>Nhập từ Excel</a-button>
      </div>
    </div>
    <section class="section">
      <div class="table-card">
        <!-- Bảng danh sách khách hàng, có phân trang -->
        <div ref="tableWrap" class="table-wrap">
          <SimpleCustomerTable :items="dataPaging" :loading="loading" v-model="selected" />
        </div>
        <!-- Footer phân trang ngay dưới bảng -->
        <div class="table-footer">
          <div class="footer-left">
            <span class="total"><b>Tổng số:</b> {{ total.toLocaleString('vi-VN') }}</span>
            <span class="divider">|</span>
            <span class="debt"><b>Công nợ</b> {{ debtDisplay }}</span>
          </div>
          <div class="footer-right">
            <!-- Ant Design Vue: a-select chọn kích thước trang -->
            <a-select
              class="page-size"
              :value="pageSize"
              :options="pageSizeOptions"
              @change="onPageSizeSelect"
            />
            <!-- Ant Design Vue: a-pagination điều khiển phân trang -->
            <a-pagination
              :current="current"
              :total="total"
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
import SimpleCustomerTable from '../components/SimpleCustomerTable.vue';
import Footer from '../components/Footer.vue';
import { ref, computed, onMounted, onBeforeUnmount, nextTick } from 'vue';
import { getCustomersPaged, searchCustomers } from '../api/customer';
import { useRouter } from 'vue-router';

const search = ref('');
const loading = ref(false);
const selected = ref([]);
const router = useRouter();
const customers = ref([]);
const current = ref(1);
const pageSize = ref(20);
const total = ref(0);
const dataPaging = computed(() => customers.value);
async function fetchData() {
  loading.value = true;
  const page = current.value;
  const size = pageSize.value;
  const keyword = search.value?.trim();
  try {
    const res = await (keyword ? searchCustomers(keyword, page, size) : getCustomersPaged(page, size));
    console.log('Customer API response:', res);
  const list = Array.isArray(res?.data) ? res.data : res?.Data;
  const meta = res?.meta || res?.Meta;
  customers.value = (list || []).map((c) => ({
    id: c.id ?? c.Id,
    type: c.customerType?.typeName ?? c.CustomerType?.TypeName ?? '-',
    code: c.customerCode ?? c.CustomerCode,
    name: c.fullName ?? c.FullName,
    tax: c.taxCode ?? c.TaxCode,
    address: (c.shippingAddress ?? c.ShippingAddress) || '-',
    phone: c.phone ?? c.Phone,
    recentDate: c.lastPurchaseDate ?? c.LastPurchaseDate ?? '-',
    boughtGoods: c.productsSummary ?? c.ProductsSummary ?? '-',
    boughtNames: c.latestProductName ?? c.LatestProductName ?? '-',
  }));
  total.value = meta?.total ?? meta?.Total ?? customers.value.length;
  } catch (err) {
    console.error('Fetch customers failed:', err);
    customers.value = [];
    total.value = 0;
  } finally {
    loading.value = false;
  }
}
async function onSearch(val) {
  search.value = val;
  current.value = 1;
  await fetchData();
}
async function onPageChange(page) {
  current.value = page;
  await fetchData();
}
function goAdd() {
  router.push({ name: 'CustomerAdd' });
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
const rangeStart = computed(() => (total.value === 0 ? 0 : (current.value - 1) * pageSize.value + 1));
const rangeEnd = computed(() => Math.min(total.value, current.value * pageSize.value));
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
  await fetchData();
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
