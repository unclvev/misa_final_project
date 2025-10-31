import Http from './http';

// Ví dụ API: tìm người dùng để mời
export const searchUserToInvite = (query) =>
  Http.get('/Manager/SearchUserToInvite', { params: { search: query } })
    .then((res) => res.data);


