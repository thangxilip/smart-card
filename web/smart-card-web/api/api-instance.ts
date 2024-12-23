import { Api } from "@/api/service-proxy";
import { LOCALSTORAGE_CONSTANTS } from "@/Utils/constants";

const apiClient = new Api({
  timeout: 1000 * 60 * 10,
  baseURL: "https://localhost:7052",
  withCredentials: true,
});

apiClient.instance.interceptors.request.use(
  (config) => {
    const accessToken = localStorage.getItem(
      LOCALSTORAGE_CONSTANTS.ACCESS_TOKEN,
    );

    if (accessToken) {
      config.headers.Authorization = `Bearer ${accessToken}`;
    }

    return config;
  },
  (error) => {
    return Promise.reject(error);
  },
);

export default apiClient;
