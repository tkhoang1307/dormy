using Dormy.WebService.Api.Core.Entities;
using Dormy.WebService.Api.Infrastructure.Postgres.IRepositories;

namespace Dormy.WebService.Api.Infrastructure.Postgres.Repositories
{
    public class HealthInsuranceRepository : BaseRepository<HealthInsuranceEntity>, IHealthInsuranceRepository
    {
        public HealthInsuranceRepository(ApplicationContext context) : base(context)
        {

        }
    }
}
