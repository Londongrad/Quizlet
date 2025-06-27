// src/api/axios.ts
import axios from "axios";

export const api = axios.create({
  baseURL: "https://localhost:7191/api",
});

// Добавляем токен из localStorage
api.interceptors.request.use((config) => {
  const token = localStorage.getItem("token");
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});
