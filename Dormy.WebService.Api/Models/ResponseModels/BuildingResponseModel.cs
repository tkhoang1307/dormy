﻿using Dormy.WebService.Api.Models.Enums;

namespace Dormy.WebService.Api.Models.ResponseModels
{
    public class BuildingResponseModel : BaseResponseModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public int TotalFloors { get; set; }

        public int TotalRooms { get; set; }

        // GenderEnum
        public string GenderRestriction { get; set; } = string.Empty;

        public List<RoomResponseModel> Rooms { get; set; } = [];
    }
}
