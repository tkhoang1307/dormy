using Dormy.WebService.Api.Core.Interfaces;
using Dormy.WebService.Api.Startup;

namespace Dormy.WebService.Api.ApplicationLogic
{
    public class VehicleHistoryService : IVehicleHistoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public VehicleHistoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
    }
}
