using Dormy.WebService.Api.Core.Entities;
using Dormy.WebService.Api.Infrastructure.Postgres.IRepositories;

namespace Dormy.WebService.Api.Infrastructure.Postgres.Repositories
{
    public class InvoiceItemRepository : BaseRepository<InvoiceItemEntity>, IInvoiceItemRepository
    {
        public InvoiceItemRepository(ApplicationContext context) : base(context)
        {

        }
    }
}
