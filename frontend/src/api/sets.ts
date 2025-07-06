import { api } from "./axios";

export interface SetDto {
  id: string;
  title: string;
  description: string;
  createdAt: string;
  wordCount: number;
}

export async function fetchSets(): Promise<SetDto[]> {
  const res = await api.get("/set");
  return res.data;
}

export async function createSet(data: { title: string; description: string }) {
  const response = await api.post("/set", data);
  return response.data;
}

export async function deleteSet(id: string) {
  await api.delete(`/set/${id}`);
}

export async function updateSet(id: string, data: { title: string; description: string }) {
  return await api.put(`/set/${id}`, data);
}
