namespace ShareMyCalendar.Authentication.Responses
{
    public class ApiResponse
    {
        public string ErrorCode { get; set; }
        public object Error { get; set; }
        public object Value { get; set; }

        public static ApiResponse Failure(object x)
        {
            return new ApiResponse
            {
                ErrorCode = x.GetType().Name,
                Error = x
            };
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
