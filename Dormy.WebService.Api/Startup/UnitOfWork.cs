using Dormy.WebService.Api.Infrastructure.Postgres;
using Dormy.WebService.Api.Infrastructure.Postgres.IRepositories;
using Dormy.WebService.Api.Infrastructure.Postgres.Repositories;

namespace Dormy.WebService.Api.Startup
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationContext _context;

        public IAdminRepository AdminRepository { get; }

        public UnitOfWork(ApplicationContext context)
        {
            _context = context;
            AdminRepository = new AdminRepository(_context);
        }

        public async Task SaveChangeAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
