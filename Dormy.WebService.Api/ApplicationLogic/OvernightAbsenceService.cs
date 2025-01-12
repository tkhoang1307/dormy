using Dormy.WebService.Api.Core.Interfaces;
using Dormy.WebService.Api.Startup;

namespace Dormy.WebService.Api.ApplicationLogic
{
    public class OvernightAbsenceService : IOvernightAbsenceService
    {
        private readonly IUnitOfWork _unitOfWork;

        public OvernightAbsenceService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
    }
}
