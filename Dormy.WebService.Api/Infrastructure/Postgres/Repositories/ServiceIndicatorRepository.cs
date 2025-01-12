using Dormy.WebService.Api.Core.Entities;
using Dormy.WebService.Api.Infrastructure.Postgres.IRepositories;

namespace Dormy.WebService.Api.Infrastructure.Postgres.Repositories
{
    public class ServiceIndicatorRepository : BaseRepository<ServiceIndicatorEntity>, IServiceIndicatorRepository
    {
        public ServiceIndicatorRepository(ApplicationContext context) : base(context)
        {

        }
    }
}
