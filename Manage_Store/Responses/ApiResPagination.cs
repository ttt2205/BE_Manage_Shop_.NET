namespace Manage_Store.Responses
{
    public class ApiResPagination<T>
    {
        public bool Success { get; set; }
        public int Status { get; set; }
        public string? Message { get; set; }
        public T? Result { get; set; }

        public Meta Meta { get; set; } = new Meta();

        // Hàm static khởi tạo builder
        public static ApiResPaginationBuilder<T> Builder()
        {
            return new ApiResPaginationBuilder<T>();
        }

        // Class Builder bên trong
        public class ApiResPaginationBuilder<TBuilder>
        {
            private readonly ApiResPagination<TBuilder> _response = new ApiResPagination<TBuilder>();

            public ApiResPaginationBuilder<TBuilder> WithSuccess(bool success)
            {
                _response.Success = success;
                return this;
            }

            public ApiResPaginationBuilder<TBuilder> WithStatus(int status)
            {
                _response.Status = status;
                return this;
            }

            public ApiResPaginationBuilder<TBuilder> WithMessage(string? message)
            {
                _response.Message = message;
                return this;
            }

            public ApiResPaginationBuilder<TBuilder> WithResult(TBuilder? Result)
            {
                _response.Result = Result;
                return this;
            }

            public ApiResPaginationBuilder<TBuilder> WithMeta(Meta meta)
            {
                _response.Meta = meta;
                return this;
            }

            public ApiResPagination<TBuilder> Build()
            {
                return _response;
            }
        }
    }

    public class Meta
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }

        public int TotalPage { get; set; }

        public int TotalItems { get; set; }
    }
}
