<template>
  <div>
    <Header />
    <SubHeader>
      <template #title>
        <h2>Thêm Khách Hàng</h2>
      </template>
      <template #actions>
        <button class="btn-submit" @click="submit('normal')">Lưu</button>
        <button class="btn-submit-outline" @click="submit('add')">Lưu và thêm</button>
        <button class="btn-cancel" @click="cancel">Hủy bỏ</button>
      </template>
    </SubHeader>
    <section class="section">
      <CustomerForm v-model="form" :showButton="false" />
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
const form = ref({});
function submit(type) {
  if(type==='normal') {
    alert('Submit Lưu: ' + JSON.stringify(form.value));
  } else if(type==='add') {
    alert('Submit Lưu và thêm: ' + JSON.stringify(form.value));
  }
}
function cancel() {
  // Có thể điều hướng về trang list hoặc reset form
  alert('Hủy bỏ');
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
