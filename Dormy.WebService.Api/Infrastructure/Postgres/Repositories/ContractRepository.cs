using Dormy.WebService.Api.Core.Entities;
using Dormy.WebService.Api.Infrastructure.Postgres.IRepositories;

namespace Dormy.WebService.Api.Infrastructure.Postgres.Repositories
{
    public class ContractRepository : BaseRepository<ContractEntity>, IContractRepository
    {
        public ContractRepository(ApplicationContext context) : base(context)
        {

        }
    }
}
