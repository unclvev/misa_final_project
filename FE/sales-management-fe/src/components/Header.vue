<template>
  <header class="header">
    <div class="left-section">
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
    </div>
    
    <div class="user-avatar">
      <slot name="avatar">
        <img src="https://ui-avatars.com/api/?name=User&background=1996ff&color=fff" alt="User Avatar" />
      </slot>
    </div>
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

.left-section {
  display: flex;
  align-items: center;
  gap: 2rem;
}

.logo {
  font-size: 1.3rem;
  font-weight: 700;
  display: flex;
  align-items: center;
  gap: 0.5rem;
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

.user-avatar {
  display: flex;
  align-items: center;
}

.user-avatar img {
  width: 40px;
  height: 40px;
  border-radius: 50%;
  cursor: pointer;
  transition: transform 0.2s;
}

.user-avatar img:hover {
  transform: scale(1.05);
}
</style>