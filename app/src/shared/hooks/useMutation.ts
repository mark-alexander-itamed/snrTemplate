import { AxiosRequestConfig, CancelTokenSource } from "axios";
import React from "react";
import api from "../utils/api";

interface MutationState<TResponse> {
  data: TResponse | null;
  error: object | null;
  inProgress: boolean;
}

interface RequestParams {
  path?: string;
}

interface MutationActions<TResponse> {
  request: (data: any, params?: RequestParams) => Promise<TResponse>;
  clear: () => void;
}

const useMutation = <TResponse = any>(
  method: "post" | "put" | "patch" | "delete",
  path: string,
  config: AxiosRequestConfig = {}
): [MutationState<TResponse>, MutationActions<TResponse>] => {
  const [state, setState] = React.useState<MutationState<TResponse>>({
    data: null,
    error: null,
    inProgress: false,
  });

  const request = (data: any, params: RequestParams = {}) => {
    setState((o) => ({ ...o, inProgress: true }));

    // Delete has a slightly different call signature.
    if (method === "delete") {
      return requestDelete(params);
    }

    const [promise] = api[method]<TResponse>(params.path ?? path, data, config);

    promise
      .then((res) => {
        setState(() => ({
          data: res,
          error: null,
          inProgress: false,
        }));
      })
      .catch((error) => {
        setState(() => ({
          data: null,
          error,
          inProgress: false,
        }));
      });

    return promise;
  };

  const requestDelete = ({ path: requestPath }: RequestParams = {}) => {
    const [promise] = api.delete<TResponse>(requestPath ?? path, config);

    promise
      .then((res) => {
        setState(() => ({
          data: res,
          error: null,
          inProgress: false,
        }));
      })
      .catch((error) => {
        setState(() => ({
          data: null,
          error,
          inProgress: false,
        }));
      });

    return promise;
  };

  const clear = () => {
    setState(() => ({ data: null, error: null, inProgress: false }));
  };

  return [state, { request, clear }];
};

export default useMutation;
