using Dormy.WebService.Api.Core.Entities;
using Dormy.WebService.Api.Infrastructure.Postgres.Repositories;

namespace Dormy.WebService.Api.Infrastructure.Postgres.IRepositories
{
    public interface IInvoiceUserRepository : IBaseRepository<InvoiceUserEntity>
    {
    }
}
