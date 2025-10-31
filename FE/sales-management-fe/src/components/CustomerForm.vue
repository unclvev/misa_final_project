<template>
  <form class="customer-form" @submit.prevent="submitForm">
    <!-- Khối lưới lớn chia làm 2 cột -->
    <div class="grid">
      <!-- Cột trái là thông tin tên, loại, mã số thuế... -->
      <div class="col">
        <div class="form-group">
          <label>Tên khách hàng <span class="star">*</span></label>
          <input v-model="localForm.name" required type="text" />
          <div v-if="errors?.name" class="error-text">{{ errors.name }}</div>
        </div>
        <div class="form-group">
          <label>Loại khách hàng</label>
          <input v-model="localForm.type" type="text" placeholder="null" />
        </div>
        <div class="form-group">
          <label>Mã số thuế</label>
          <input v-model="localForm.tax" type="text" />
          <div v-if="errors?.tax" class="error-text">{{ errors.tax }}</div>
        </div>
        <div class="form-group">
          <label>Email</label>
          <input v-model="localForm.email" type="email" />
          <div v-if="errors?.email" class="error-text">{{ errors.email }}</div>
        </div>
        <div class="form-group">
          <label>Ngành nghề</label>
          <input v-model="localForm.industry" type="text" placeholder="null" />
        </div>
        <div class="form-group">
          <label>Loại hình</label>
          <select v-model="localForm.category">
            <option value="">- Không chọn -</option>
            <option value="option1">Option 1</option>
          </select>
        </div>
        <div class="form-group">
          <label>Giới tính</label>
          <select v-model="localForm.gender">
            <option value="">- Không chọn -</option>
            <option value="male">Nam</option>
            <option value="female">Nữ</option>
          </select>
        </div>
        <div class="form-group">
          <label>Kênh liên hệ</label>
          <select v-model="localForm.channel">
            <option value="">- Không chọn -</option>
            <option value="option1">Option 1</option>
          </select>
        </div>
        <div class="form-group">
          <label>Đơn vị chủ quản</label>
          <select v-model="localForm.masterUnit">
            <option value="">- Không chọn -</option>
            <option value="option1">Option 1</option>
          </select>
        </div>
        <div class="form-group">
          <label>Điện thoại khác</label>
          <input v-model="localForm.phoneOther" type="text" />
        </div>
        <div class="form-group">
          <label>Tên thường gọi</label>
          <input v-model="localForm.alias" type="text" />
        </div>
      </div>
      <!-- Cột phải là mã khách hàng, tên viết tắt,... -->
      <div class="col">
        <div class="form-group">
          <label>Mã khách hàng</label>
          <input v-model="localForm.code" type="text" />
        </div>
        <div class="form-group">
          <label>Tên viết tắt</label>
          <input v-model="localForm.shortName" type="text" />
        </div>
        <div class="form-group">
          <label>Lĩnh vực</label>
          <input v-model="localForm.field" type="text" placeholder="null" />
        </div>
        <div class="form-group">
          <label>Điện thoại <span class="star">*</span></label>
          <input v-model="localForm.phone" type="tel" />
          <div v-if="errors?.phone" class="error-text">{{ errors.phone }}</div>
        </div>
        <div class="form-group">
          <label>Nguồn gốc</label>
          <select v-model="localForm.origin">
            <option value="">- Không chọn -</option>
            <option value="option1">Option 1</option>
            <option value="option2">Option 2</option>
          </select>
        </div>
        <div class="form-group">
          <label>Số hộ chiếu</label>
          <input v-model="localForm.passport" type="text" />
        </div>
        <div class="form-group">
          <label>Zalo</label>
          <input v-model="localForm.zalo" type="text" />
        </div>
        <div class="form-group">
          <label>Số CMND/CCCD <span class="info">i</span></label>
          <input v-model="localForm.idCard" type="text" />
        </div>
        <div class="form-group">
          <label>Mã số ĐVQHNS <span class="info">i</span></label>
          <input v-model="localForm.unitCode" type="text" />
        </div>
        <div class="form-group">
          <label>Kế toán - Một dòng</label>
          <input v-model="localForm.account1" type="text" />
        </div>
        <div class="form-group" style="flex: 1 1 100%">
          <label>Kế toán - Nhiều dòng</label>
          <textarea v-model="localForm.accountMany" rows="2" style="resize:vertical;"></textarea>
        </div>
      </div>
    </div>
    <!-- Footer button chỉ hiện nếu showButton -->
    <div class="form-footer" v-if="showButton">
      <button type="submit" class="btn-submit">Lưu</button>
      <button type="button" class="btn-submit-outline" @click="$emit('submit', { ...localForm, addAnother: true })">Lưu và thêm</button>
      <button type="button" class="btn-cancel">Hủy bỏ</button>
    </div>
  </form>
</template>

<script setup>
import { reactive, watch } from 'vue';
const props = defineProps({
  modelValue: { type: Object, default: () => ({}) },
  showButton: { type: Boolean, default: true },
  errors: { type: Object, default: () => ({}) }
});
const emit = defineEmits(['update:modelValue', 'submit']);
const localForm = reactive({ ...props.modelValue });
watch(
  () => props.modelValue,
  (val) => { Object.assign(localForm, val); }
);
watch(localForm, (val) => emit('update:modelValue', { ...val }), { deep: true });
function submitForm() {
  emit('submit', { ...localForm });
}
</script>

<style scoped>
.customer-form {
  width: 100%;
  max-width: 1440px;
  margin: 0 auto;
  background: #fff;
  border-radius: 0;
  box-shadow: none;
  padding: 0 0 0 0;
}
.grid {
  display: flex;
  gap: 30px;
  padding: 0 0 0 36px;
  margin-bottom: 18px;
}
.col {
  flex: 1 1 0;
  min-width: 0;
  display: flex;
  flex-direction: column;
  gap: 13px;
}
.form-group {
  display: flex;
  flex-direction: column;
  gap: 2px;
}
.form-group label {
  font-weight: 500;
  margin-bottom: 2px;
  color: #222d40;
  font-size: 15px;
}
.form-group input,
.form-group select,
.form-group textarea {
  font-size: 15px;
  border: 1px solid #d6dee7;
  border-radius: 5px;
  padding: 8px 10px;
  background: #f7fafd;
  outline: none;
  margin: 0;
}
.form-group textarea {
  min-height: 40px;
  resize: none;
}
.star {
  color: red;
}
.info {
  background: #eef2ff;
  color: #688;
  border-radius: 50%;
  display: inline-block;
  width: 18px;
  height: 18px;
  text-align: center;
  line-height: 15px;
  font-size: 12px;
  font-style: normal;
  margin-left: 2px;
  border: 1px solid #dde;
}
.error-text {
  color: #dc2626;
  font-size: 12px;
  margin-top: 4px;
}
.form-footer {
  display: flex;
  gap: 14px;
  justify-content: flex-end;
  padding: 36px 46px 0 0;
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
