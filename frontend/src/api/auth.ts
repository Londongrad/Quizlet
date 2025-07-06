import { api } from "./axios";

export async function login(username: string, password: string): Promise<string> {

  try {
    const res = await api.post("/auth/login", { username, password });
    return res.data.token;

  } catch (error: any) {

    if (error.response && typeof error.response.data === "string") {
      throw new Error(error.response.data);
    }

    if (error.response?.data?.message) {
      throw new Error(error.response.data.message);
    }

    throw new Error("Login failed");
  }
}

export async function register(email: string, username: string, password: string): Promise<string> {

  try {
    const res = await api.post("/auth/register", { email, username, password });
    return res.data.token;

  } catch (error: any) {

    if (error.response && typeof error.response.data === "string") {
      throw new Error(error.response.data);
    }

    if (error.response?.data?.message) {
      throw new Error(error.response.data.message);
    }

    throw new Error("Registration failed");
  }
}