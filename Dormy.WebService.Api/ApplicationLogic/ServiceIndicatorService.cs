using Dormy.WebService.Api.Core.Interfaces;
using Dormy.WebService.Api.Startup;

namespace Dormy.WebService.Api.ApplicationLogic
{
    public class ServiceIndicatorService : IServiceIndicatorService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ServiceIndicatorService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
    }
}
