import { AxiosError } from "axios";

export default function isAxiosError(error: any): error is AxiosError {
  return typeof error.isAxiosError === "boolean" && error.isAxiosError;
}
