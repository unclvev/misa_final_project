import axios from 'axios';
import { BASE_API_LOCAL } from './urlbase';

// Axios instance tối giản (không xử lý token)
const Http = axios.create({
  baseURL: BASE_API_LOCAL,
  timeout: 15000,
  withCredentials: false,
});

// Interceptor request (pass-through)
Http.interceptors.request.use(
  (config) => config,
  (error) => Promise.reject(error)
);

// Interceptor response (giữ nguyên response, để caller .data nếu muốn)
Http.interceptors.response.use(
  (response) => response,
  (error) => Promise.reject(error)
);

export default Http;


