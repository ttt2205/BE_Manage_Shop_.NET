// // DynamicFilterHelper.cs
// using System.Linq.Expressions;
// namespace Manage_Store.Models.Dtos
// {
//     public static class DynamicFilterHelper
//     {
//         public static async Task<ResPagination<List<T>>> GetPagedResultAsync<T>(
//             IQueryable<T> query,
//             int page = 1,
//             int pageSize = 10,
//             List<Expression<Func<T, bool>>>? filters = null)
//         {
//             if (filters != null)
//             {
//                 foreach (var filter in filters)
//                 {
//                     query = query.Where(filter);
//                 }
//             }
//             // Tổng số item
//             var totalItems = await query.CountAsync();
//             // Phân trang
//             var data = await query.Skip((page - 1) * pageSize)
//                                   .Take(pageSize)
//                                   .ToListAsync();

//             var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

//             return new ResPagination<List<T>>
//             {
//                 result = data,
//                 meta = new ResPagination<List<T>>.Meta
//                 {
//                     currentPage = page,
//                     pageSize = pageSize,
//                     totalItems = totalItems,
//                     totalPage = totalPages
//                 }
//             };
//         }
//     }
// }
