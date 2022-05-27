import axios, { AxiosRequestConfig, CancelTokenSource } from "axios";

const BASE_URL = "http://localhost:5000";

const defaults: Partial<AxiosRequestConfig> = {
  baseURL: BASE_URL,
};

const instance = axios.create(defaults);

const api = <TResponse>(
  method: "get" | "post" | "patch" | "delete" | "put",
  path: string,
  variables: any = {},
  config: Partial<AxiosRequestConfig> = {}
): [Promise<TResponse>, CancelTokenSource] => {
  const source = axios.CancelToken.source();
  const request = instance
    .request<TResponse>({
      method: method,
      url: path,
      params: method === "get" ? variables : null,
      data: method !== "get" ? variables : null,
      ...config,
      cancelToken: source.token,
    })
    .then((res) => res.data);

  return [request, source];
};

export default {
  get: <TResponse>(
    path: string,
    params: any = {},
    config: Partial<AxiosRequestConfig> = {}
  ) => api<TResponse>("get", path, params, config),
  post: <TResponse>(
    path: string,
    data: any = {},
    config: Partial<AxiosRequestConfig> = {}
  ) => api<TResponse>("post", path, data, config),
  patch: <TResponse>(
    path: string,
    data: any = {},
    config: Partial<AxiosRequestConfig> = {}
  ) => api<TResponse>("patch", path, data, config),
  put: <TResponse>(
    path: string,
    data: any = {},
    config: Partial<AxiosRequestConfig> = {}
  ) => api<TResponse>("put", path, data, config),
  delete: <TResponse>(path: string, config: Partial<AxiosRequestConfig> = {}) =>
    api<TResponse>("delete", path, {}, config),
};
