import Http from './http';

export const getCustomersPaged = (page = 1, pageSize = 20) =>
  Http.get('/Customer/GetCustomersPaged', { params: { page, pageSize } })
    .then((res) => res.data);

export const searchCustomers = (keyword = '', page = 1, pageSize = 20) =>
  Http.get('/Customer/SearchCustomers', { params: { keyword, page, pageSize } })
    .then((res) => res.data);

export const getAllCustomers = () =>
  Http.get('/Customer/GetAllCustomer').then((res) => res.data);

export const addCustomer = (payload) =>
  Http.post('/Customer/AddCustomer', payload).then((res) => res.data);

export const updateCustomer = (id, payload) =>
  Http.put(`/Customer/UpdateCustomer/${id}`, payload).then((res) => res.data);

export const deleteCustomer = (id) =>
  Http.delete(`/Customer/DeleteCustomer/${id}`).then((res) => res.data);


