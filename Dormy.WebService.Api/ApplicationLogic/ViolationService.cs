using Dormy.WebService.Api.Core.Interfaces;
using Dormy.WebService.Api.Startup;

namespace Dormy.WebService.Api.ApplicationLogic
{
    public class ViolationService : IViolationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ViolationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
    }
}
