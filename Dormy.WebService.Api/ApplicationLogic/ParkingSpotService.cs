using Dormy.WebService.Api.Core.Interfaces;
using Dormy.WebService.Api.Startup;

namespace Dormy.WebService.Api.ApplicationLogic
{
    public class ParkingSpotService : IParkingSpotService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ParkingSpotService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
    }
}
