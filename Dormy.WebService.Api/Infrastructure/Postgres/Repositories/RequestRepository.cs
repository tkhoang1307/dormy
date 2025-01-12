using Dormy.WebService.Api.Core.Entities;
using Dormy.WebService.Api.Infrastructure.Postgres.IRepositories;

namespace Dormy.WebService.Api.Infrastructure.Postgres.Repositories
{
    public class RequestRepository : BaseRepository<RequestEntity>, IRequestRepository
    {
        public RequestRepository(ApplicationContext context) : base(context)
        {

        }
    }
}
