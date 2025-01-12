using Dormy.WebService.Api.Core.Entities;
using Dormy.WebService.Api.Infrastructure.Postgres.IRepositories;

namespace Dormy.WebService.Api.Infrastructure.Postgres.Repositories
{
    public class SettingRepository : BaseRepository<SettingEntity>, ISettingRepository
    {
        public SettingRepository(ApplicationContext context) : base(context)
        {

        }
    }
}
