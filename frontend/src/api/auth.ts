import { api } from "./axios";

export async function login(username: string, password: string): Promise<string> {
  const res = await api.post("/auth/login", { username, password });
  console.log("Ответ от сервера:", res.data);
  return res.data.token;
}