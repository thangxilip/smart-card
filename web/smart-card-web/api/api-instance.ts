import { Api } from "@/api/service-proxy";
import { LOCALSTORAGE_CONSTANTS } from "@/utils/constants";

const apiClient = new Api({
  timeout: 1000 * 60 * 10,
  baseURL: process.env.NEXT_PUBLIC_BACK_END_API,
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
