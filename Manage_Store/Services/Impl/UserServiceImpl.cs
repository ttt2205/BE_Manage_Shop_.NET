
using Manage_Store.Data;

namespace Manage_Store.Services.Impl
{
    public class UserServiceImpl : IUserService
    {
        private readonly AppDbContext _context;
        public UserServiceImpl(AppDbContext context)
        {
            _context = context;
        }

        public Task<string> createNewUser()
        {
            throw new NotImplementedException();
        }
    }
}
