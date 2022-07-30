interface ApiResult<T> {
  requestId: string;
  errorCode: string;
  errorMessage: string;
  value: T;
}

export default ApiResult;