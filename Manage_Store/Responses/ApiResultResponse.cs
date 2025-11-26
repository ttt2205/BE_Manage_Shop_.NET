namespace Manage_Store.Responses
{
    public class ApiResultResponse<T>
    {
        public bool Success { get; set; }
        public int Status { get; set; }
        public string? Message { get; set; }
        public List<T>? Result { get; set; }

        // Hàm static khởi tạo builder
        public static ApiResultResponseBuilder<T> Builder()
        {
            return new ApiResultResponseBuilder<T>();
        }

        // Class Builder bên trong
        public class ApiResultResponseBuilder<TBuilder>
        {
            private readonly ApiResultResponse<TBuilder> _response = new ApiResultResponse<TBuilder>();

            public ApiResultResponseBuilder<TBuilder> WithSuccess(bool success)
            {
                _response.Success = success;
                return this;
            }

            public ApiResultResponseBuilder<TBuilder> WithStatus(int status)
            {
                _response.Status = status;
                return this;
            }

            public ApiResultResponseBuilder<TBuilder> WithMessage(string? message)
            {
                _response.Message = message;
                return this;
            }

            public ApiResultResponseBuilder<TBuilder> WithResult(List<TBuilder> result)
            {
                _response.Result = result;
                return this;
            }

            public ApiResultResponse<TBuilder> Build()
            {
                return _response;
            }
        }
    }
}
