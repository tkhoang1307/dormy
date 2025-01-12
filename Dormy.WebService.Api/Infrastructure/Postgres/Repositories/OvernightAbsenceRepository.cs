﻿using Dormy.WebService.Api.Core.Entities;
using Dormy.WebService.Api.Infrastructure.Postgres.IRepositories;

namespace Dormy.WebService.Api.Infrastructure.Postgres.Repositories
{
    public class OvernightAbsenceRepository : BaseRepository<OvernightAbsenceEntity>, IOvernightAbsenceRepository
    {
        public OvernightAbsenceRepository(ApplicationContext context) : base(context)
        {

        }
    }
}