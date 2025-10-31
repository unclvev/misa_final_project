<template>
  <header class="header">
    <div class="logo">
      <slot name="logo">
        <img src="/favicon.ico" alt="CRM Logo" width="36" /> CRM
      </slot>
    </div>
    <nav class="main-nav">
      <slot name="menu">
        <ul>
          <li><a href="#">Bàn làm việc</a></li>
          <li><a href="#">Khách hàng</a></li>
          <li><a href="#">Cơ hội</a></li>
          <li><a href="#">Báo giá</a></li>
          <li><a href="#">Đơn hàng</a></li>
          <li><a href="#">Hàng hoá</a></li>
          <li><a href="#">Báo cáo</a></li>
        </ul>
      </slot>
    </nav>
  </header>
</template>

<script setup>
</script>

<style scoped>
.header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  background: #fff;
  border-bottom: 1px solid #ececec;
  padding: 0 2rem;
  height: 60px;
}
.logo {
  font-size: 1.3rem;
  font-weight: 700;
  display: flex;
  align-items: center;
}
.main-nav ul {
  list-style: none;
  display: flex;
  gap: 1rem;
  margin: 0;
  padding: 0;
}
.main-nav a {
  text-decoration: none;
  color: #252a3e;
  font-weight: 500;
  transition: color 0.2s;
}
.main-nav a:hover {
  color: #1996ff;
}
</style>
