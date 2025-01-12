﻿using Dormy.WebService.Api.Core.Entities;
using Dormy.WebService.Api.Infrastructure.Postgres.IRepositories;

namespace Dormy.WebService.Api.Infrastructure.Postgres.Repositories
{
    public class GuardianRepository : BaseRepository<GuardianEntity>, IGuardianRepository
    {
        public GuardianRepository(ApplicationContext context) : base(context)
        {

        }
    }
}