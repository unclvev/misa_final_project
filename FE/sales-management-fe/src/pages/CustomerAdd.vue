<template>
  <div>
    <Header />
    <SubHeader>
      <template #title>
        <h2>{{ isEdit ? 'Sửa Khách Hàng' : 'Thêm Khách Hàng' }}</h2>
      </template>
      <template #actions>
        <button class="btn-cancel" @click="cancel">Hủy bỏ</button>
        <button v-if="!isEdit" class="btn-submit-outline" @click="submit('add')">Lưu và thêm</button>
        <button class="btn-submit" @click="submit('normal')">Lưu</button>
      </template>
    </SubHeader>
    <section class="section">
      <CustomerForm v-model="form" :errors="errors" :showButton="false" />
    </section>
    <Footer />
  </div>
</template>

<script setup>
import Header from '../components/Header.vue';
import SubHeader from '../components/SubHeader.vue';
import CustomerForm from '../components/CustomerForm.vue';
import Footer from '../components/Footer.vue';
import { ref } from 'vue';
import { addCustomer, updateCustomer, getCustomerById } from '../api/customer';
import { useRouter, useRoute } from 'vue-router';
const form = ref({});
const errors = ref({});
const router = useRouter();
const route = useRoute();
const isEdit = ref(!!route.params.id);
const editId = route.params.id;

// Prefill: ưu tiên state; nếu không có, gọi API by id
const stateCustomer = route.state?.customer;
if (isEdit.value) {
  if (stateCustomer) {
    form.value = {
      name: stateCustomer.name,
      email: stateCustomer.email,
      tax: stateCustomer.tax,
      address: stateCustomer.address,
      phone: stateCustomer.phone,
      customerTypeId: stateCustomer.customerTypeId || null,
    };
  } else if (editId) {
    getCustomerById(editId).then((c) => {
      form.value = {
        name: c.fullName ?? c.FullName,
        email: c.email ?? c.Email,
        tax: c.taxCode ?? c.TaxCode,
        address: c.shippingAddress ?? c.ShippingAddress,
        phone: c.phone ?? c.Phone,
        customerTypeId: c.customerTypeId ?? c.CustomerTypeId ?? null,
      };
    }).catch(() => {
      // ignore; giữ form trống nếu lỗi
    });
  }
}
async function submit(type) {
  errors.value = {};
  const payload = {
    FullName: form.value.name || '',
    Email: form.value.email || null,
    TaxCode: form.value.tax || null,
    ShippingAddress: form.value.address || '',
    Phone: form.value.phone || '',
    CustomerTypeId: form.value.customerTypeId || null,
  };
  if (!payload.FullName) errors.value.name = 'Vui lòng nhập tên khách hàng';
  if (!payload.Phone) errors.value.phone = 'Vui lòng nhập số điện thoại';
  if (Object.keys(errors.value).length) return;
  try {
    if (isEdit.value) {
      await updateCustomer(editId, payload);
    } else {
      await addCustomer(payload);
    }
    if (type === 'add') {
      form.value = {};
      alert('Đã lưu. Bạn có thể thêm khách hàng tiếp theo.');
    } else {
      alert(isEdit.value ? 'Cập nhật thành công' : 'Đã lưu thành công');
      router.push({ name: 'CustomerList' });
    }
  } catch (e) {
    console.error(e);
    // Map lỗi validation từ API (400) nếu có
    const data = e?.response?.data;
    if (data && typeof data === 'object') {
      const msg = data?.message || data?.title;
      if (msg) errors.value.general = msg;
      const modelState = data?.errors || data?.Errors;
      if (modelState) {
        if (modelState.FullName?.[0]) errors.value.name = modelState.FullName[0];
        if (modelState.Phone?.[0]) errors.value.phone = modelState.Phone[0];
        if (modelState.TaxCode?.[0]) errors.value.tax = modelState.TaxCode[0];
        if (modelState.Email?.[0]) errors.value.email = modelState.Email[0];
      }
    } else {
      alert('Lưu thất bại');
    }
  }
}
function cancel() {
  router.push({ name: 'CustomerList' });
}
</script>

<style scoped>
.section {
  background: #f6f8fa;
  min-height: 50vh;
  padding: 2rem 0;
}
.btn-submit {
  background: #1996ff;
  color: #fff;
  border: none;
  border-radius: 4px;
  padding: 8px 28px;
  font-weight: 500;
  font-size: 1.1rem;
}
.btn-submit-outline {
  background: #f2f8ff;
  color: #1996ff;
  border: 1.5px solid #1996ff;
  border-radius: 4px;
  padding: 8px 18px;
  font-weight: 500;
  font-size: 1.1rem;
}
.btn-cancel {
  background: #fff;
  color: #363c46;
  border: 1px solid #d2d5db;
  border-radius: 4px;
  padding: 8px 16px;
  font-weight: 500;
  font-size: 1.07rem;
}
</style>
