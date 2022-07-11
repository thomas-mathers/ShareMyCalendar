namespace ShareMyCalendar.Authentication.Responses
{
    public class ApiResponse
    {
        public string ErrorCode { get; set; }
        public object Error { get; set; }
        public object Value { get; set; }

        public static ApiResponse Failure(string errorCode, object x)
        {
            return new ApiResponse
            {
                ErrorCode = errorCode,
                Error = x
            };
        }

        public static ApiResponse Failure(string errorCode)
        {
            return new ApiResponse
            {
                ErrorCode = errorCode
            };
        }

        public static ApiResponse Success()
        {
            return new ApiResponse { };
        }

        public static ApiResponse Success(object x)
        {
            return new ApiResponse
            {
                Value = x
            };
        }
    }
}
