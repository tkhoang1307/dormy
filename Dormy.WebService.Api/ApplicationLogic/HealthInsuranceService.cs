using Dormy.WebService.Api.Core.Interfaces;
using Dormy.WebService.Api.Startup;

namespace Dormy.WebService.Api.ApplicationLogic
{
    public class HealthInsuranceService: IHealthInsuranceService
    {
        private readonly IUnitOfWork _unitOfWork;

        public HealthInsuranceService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
    }
}
