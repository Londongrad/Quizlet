import { api } from "./axios";

export interface SetDto {
  id: string;
  title: string;
  description: string;
  createdAt: string;
  wordCount: number;
}

export async function fetchSets(): Promise<SetDto[]> {
  const res = await api.get("/sets");
  return res.data;
}