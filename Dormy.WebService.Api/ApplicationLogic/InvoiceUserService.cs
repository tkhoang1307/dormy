using Dormy.WebService.Api.Core.Constants;
using Dormy.WebService.Api.Core.Interfaces;
using Dormy.WebService.Api.Models.ResponseModels;
using Dormy.WebService.Api.Presentation.Mappers;
using Dormy.WebService.Api.Startup;

namespace Dormy.WebService.Api.ApplicationLogic
{
    public class InvoiceUserService : IInvoiceUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContextService _userContextService;
        private readonly InvoiceUserMapper _invoiceUserMapper;

        public InvoiceUserService(IUnitOfWork unitOfWork, IUserContextService userContextService)
        {
            _unitOfWork = unitOfWork;
            _invoiceUserMapper = new InvoiceUserMapper();
            _userContextService = userContextService;
        }

        public Task<ApiResponse> CreateInvoiceUsersBatch(Guid roomId)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse> HardDeleteInvoiceUsersBatchByInvoiceId(Guid invoiceId)
        {
            var invoiceEntity = await _unitOfWork.InvoiceRepository.GetAsync(i => i.Id == invoiceId);
            if (invoiceEntity == null)
            {
                return new ApiResponse().SetNotFound(invoiceId, message: string.Format(ErrorMessages.PropertyDoesNotExist, "Invoice"));
            }

            var invoiceUsers = await _unitOfWork.InvoiceUserRepository.GetAllAsync(x => x.InvoiceId == invoiceId);
            var invoiceUserIds = invoiceUsers.Select(x => x.Id).ToList();
            foreach (var invoiceUserId in invoiceUserIds)
            {
                await _unitOfWork.InvoiceUserRepository.DeleteByIdAsync(invoiceUserId);
            }

            return new ApiResponse().SetOk(invoiceId);
        }
    }
}
