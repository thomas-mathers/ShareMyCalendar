interface ApiResult<TError, TValue> {
  requestId: string;
  errorCode: string;
  errorMessage: string;
  error: TError;
  value: TValue;
}

export default ApiResult;