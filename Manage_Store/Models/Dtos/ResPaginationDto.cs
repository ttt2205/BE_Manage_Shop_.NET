namespace Manage_Store.Models.Dtos
{
    public class ResPagination<T>
    {
        public Meta meta { get; set; }
        public T result { get; set; }
        public class Meta
        {
            public int currentPage { get; set; }
            public int pageSize { get; set; }

            public int totalPage { get; set; }

            public int totalItems { get; set; }
        }
    }
}