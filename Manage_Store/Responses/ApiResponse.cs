namespace Manage_Store.Responses
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public int Status { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }

        // Hàm static khởi tạo builder
        public static ApiResponseBuilder<T> Builder()
        {
            return new ApiResponseBuilder<T>();
        }

        // Class Builder bên trong
        public class ApiResponseBuilder<TBuilder>
        {
            private readonly ApiResponse<TBuilder> _response = new ApiResponse<TBuilder>();

            public ApiResponseBuilder<TBuilder> WithSuccess(bool success)
            {
                _response.Success = success;
                return this;
            }

            public ApiResponseBuilder<TBuilder> WithStatus(int status)
            {
                _response.Status = status;
                return this;
            }

            public ApiResponseBuilder<TBuilder> WithMessage(string? message)
            {
                _response.Message = message;
                return this;
            }

            public ApiResponseBuilder<TBuilder> WithData(TBuilder? data)
            {
                _response.Data = data;
                return this;
            }

            public ApiResponse<TBuilder> Build()
            {
                return _response;
            }
        }
    }
}
