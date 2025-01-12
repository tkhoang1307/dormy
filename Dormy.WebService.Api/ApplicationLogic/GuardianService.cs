using Dormy.WebService.Api.Core.Interfaces;
using Dormy.WebService.Api.Startup;

namespace Dormy.WebService.Api.ApplicationLogic
{
    public class GuardianService : IGuardianService
    {
        private readonly IUnitOfWork _unitOfWork;

        public GuardianService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
    }
}
