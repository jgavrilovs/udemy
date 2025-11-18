import axios from "axios";
import { store } from "../stores/store";
import { toast } from "react-toastify";

const sleep = (delay: number) => {
  return new Promise((resolve) => {
    setTimeout(resolve, delay);
  });
};

const agent = axios.create({
  baseURL: import.meta.env.VITE_API_URL,
});

agent.interceptors.request.use((config) => {
  store.uiStore.isBusy();
  return config;
});

agent.interceptors.response.use(
  async (response) => {
    await sleep(1000);
    store.uiStore.isIdle();
    return response;
  },
  async (error) => {
    await sleep(1000);
    store.uiStore.isIdle();

    const { status } = error.response;
    switch (status) {
      case 400:
        toast.error("bad request");
        break;

      case 401:
        toast.error("unathorized");
        break;

      case 404:
        toast.error("not found");
        break;

      case 500:
        toast.error("server error");
        break;

      default:
        break;
    }

    return Promise.reject(error);
  }
);

export default agent;
