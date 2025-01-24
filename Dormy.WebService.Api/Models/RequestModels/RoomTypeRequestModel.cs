﻿namespace Dormy.WebService.Api.Models.RequestModels
{
    public class RoomTypeRequestModel
    {
        public string RoomTypeName { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public int Capacity { get; set; }

        public decimal Price { get; set; }

        public List<Guid> RoomServiceIds { get; set; } = [];
    }

    public class RoomTypeUpdateRequestModel
    {
        public Guid Id { get; set; }

        public string RoomTypeName { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public int Capacity { get; set; }

        public decimal Price { get; set; }

        public List<Guid> RoomServiceIds { get; set;} = [];
    }
}
