using Dormy.WebService.Api.Infrastructure.Postgres;
using Dormy.WebService.Api.Infrastructure.Postgres.IRepositories;

namespace Dormy.WebService.Api.Startup
{
    public interface IUnitOfWork
    {
        public IAdminRepository AdminRepository { get; }

        Task SaveChangeAsync();
    }
}
