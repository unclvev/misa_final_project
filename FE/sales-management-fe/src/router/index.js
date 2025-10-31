import { createRouter, createWebHistory } from 'vue-router';
import CustomerList from '../pages/CustomerList.vue';
import CustomerAdd from '../pages/CustomerAdd.vue';

const routes = [
  { path: '/customers', name: 'CustomerList', component: CustomerList },
  { path: '/customers/add', name: 'CustomerAdd', component: CustomerAdd },
  { path: '/', redirect: '/customers' },
];
export default createRouter({
  history: createWebHistory(),
  routes,
});
