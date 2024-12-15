import { Api } from "@/api/service-proxy";

const apiClient = new Api({
  timeout: 1000 * 60 * 10,
  baseURL: "https://localhost:7052",
  withCredentials: true,
});

export default apiClient;
