import { AxiosRequestConfig } from "axios";
import React from "react";
import api from "../utils/api";

interface QueryState<TResponse> {
  data: TResponse | null;
  error: object | null;
  loading: boolean;
}

interface RequestParams {
  path?: string;
  params?: any;
}

type ResetUpdateFunc = () => void;

interface QueryActions<TResponse> {
  request: (params?: RequestParams) => Promise<TResponse>;
  clear: () => void;
  optimisticUpdate: (to: TResponse) => ResetUpdateFunc;
}

export function useQuery<TResponse = any>(
  path: string,
  params: any = {},
  config: AxiosRequestConfig = {}
): [QueryState<TResponse>, QueryActions<TResponse>] {
  const [state, setState] = React.useState<QueryState<TResponse>>({
    data: null,
    error: null,
    loading: false,
  });

  const request = (params: RequestParams = {}) => {
    setState((o) => ({ ...o, loading: true }));
    const [data] = api.get<TResponse>(
      params.path ?? path,
      params.params ?? params,
      config
    );

    data
      .then((response) => {
        setState(() => ({
          data: response,
          error: null,
          loading: false,
        }));
      })
      .catch((error) => {
        console.error(error);
        setState(() => ({
          data: null,
          error: error,
          loading: false,
        }));
      });

    return data;
  };

  const clear = () => {
    setState(() => ({ data: null, error: null, loading: false }));
  };

  const optimisticUpdate = (to: TResponse) => {
    const currentValue = state.data;

    setState((o) => ({ ...o, data: to }));

    return () => {
      setState((o) => ({ ...o, data: currentValue }));
    };
  };

  return [state, { request, clear, optimisticUpdate }];
}
