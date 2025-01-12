using Dormy.WebService.Api.Core.Interfaces;
using Dormy.WebService.Api.Startup;

namespace Dormy.WebService.Api.ApplicationLogic
{
    public class ParkingRequestService : IParkingRequestService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ParkingRequestService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
    }
}
