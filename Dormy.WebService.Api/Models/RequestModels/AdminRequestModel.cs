﻿using Dormy.WebService.Api.Models.Enums;

namespace Dormy.WebService.Api.Models.RequestModels
{
    public class AdminRequestModel
    {
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        private DateTime _dateOfBirth;
        public DateTime DateOfBirth
        {
            get => _dateOfBirth;
            set => _dateOfBirth = value.Date;
        }

        public string PhoneNumber { get; set; } = string.Empty;

        public string JobTitle { get; set; } = string.Empty;

        public string Gender { get; set; } = string.Empty;
    }
}
