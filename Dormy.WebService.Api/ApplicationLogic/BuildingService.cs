using Dormy.WebService.Api.Core.Interfaces;
using Dormy.WebService.Api.Startup;

namespace Dormy.WebService.Api.ApplicationLogic
{
    public class BuildingService: IBuildingService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BuildingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
    }
}
