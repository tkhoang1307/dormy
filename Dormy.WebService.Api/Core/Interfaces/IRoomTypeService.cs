﻿using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;

namespace Dormy.WebService.Api.Core.Interfaces
{
    public interface IRoomTypeService
    {
        Task<ApiResponse> CreateRoomType(RoomTypeRequestModel model);
        Task<ApiResponse> GetRoomTypes();
    }
}
