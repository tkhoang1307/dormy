using Dormy.WebService.Api.Core.Entities;
using Dormy.WebService.Api.Infrastructure.Postgres.IRepositories;

namespace Dormy.WebService.Api.Infrastructure.Postgres.Repositories
{
    public class InvoiceRepository : BaseRepository<InvoiceEntity>, IInvoiceRepository
    {
        public InvoiceRepository(ApplicationContext context) : base(context)
        {

        }
    }
}
