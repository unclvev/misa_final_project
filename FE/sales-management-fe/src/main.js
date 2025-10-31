import { createApp } from 'vue';
import App from './App.vue';
import router from './router/index.js';
// Ant Design Vue: đăng ký plugin và reset CSS toàn cục
import Antd from 'ant-design-vue';
import 'ant-design-vue/dist/reset.css';

const app = createApp(App);
app.use(router);
// Ant Design Vue: kích hoạt các thành phần AntD ở phạm vi toàn cục
app.use(Antd);
app.mount('#app');

