<template>
  <section class="subheader">
    <div class="title">
      <slot name="title"><h2>Tiêu đề trang</h2></slot>
    </div>
    <div class="actions">
      <slot name="actions"></slot>
    </div>
  </section>
</template>

<script setup></script>

<style scoped>
.subheader {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 1rem 2rem 0.5rem;
  background: #DDDDDD;
  border-bottom: 1px solid #ececec;
  
}
.title h2 {
  margin: 0;
  font-size: 1.5rem;
  font-weight: 600;
}
.actions {
  display: flex;
  gap: 8px;
}
</style>
