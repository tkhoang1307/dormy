using Dormy.WebService.Api.Core.Entities;
using Dormy.WebService.Api.Infrastructure.Postgres.IRepositories;

namespace Dormy.WebService.Api.Infrastructure.Postgres.Repositories
{
    public class InvoiceUserRepository : BaseRepository<InvoiceUserEntity>, IInvoiceUserRepository
    {
        public InvoiceUserRepository(ApplicationContext context) : base(context)
        {

        }
    }
}
