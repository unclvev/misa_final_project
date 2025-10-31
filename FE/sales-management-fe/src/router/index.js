import { createRouter, createWebHistory } from 'vue-router';
import CustomerList from '../pages/CustomerList.vue';
import CustomerAdd from '../pages/CustomerAdd.vue';

const routes = [
  { path: '/customers', component: CustomerList },
  { path: '/customers/add', component: CustomerAdd },
  { path: '/', redirect: '/customers' },
];
export default createRouter({
  history: createWebHistory(),
  routes,
});
