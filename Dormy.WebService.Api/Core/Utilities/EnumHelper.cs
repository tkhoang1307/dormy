﻿using Dormy.WebService.Api.Models.ResponseModels;
using System.ComponentModel;

namespace Dormy.WebService.Api.Core.Utilities
{
    public class EnumHelper
    {
        public static List<EnumResponseModel> GetAllEnumDescriptions<TEnum>() where TEnum : Enum
        {
            return Enum.GetValues(typeof(TEnum))
                       .Cast<TEnum>()
                       .Select(enumValue => GetEnumDescriptionResponse(enumValue))
                       .ToList();
        }

        private static EnumResponseModel GetEnumDescriptionResponse<TEnum>(TEnum enumValue) where TEnum : Enum
        {
            var description = GetEnumDescription(enumValue);
            var parts = description.Split(" /*-*/ ", StringSplitOptions.None);

            return new EnumResponseModel
            {
                EnumValue = enumValue.ToString(),
                VietnameseEnumDescription = parts.Length > 1 ? parts[1] : string.Empty,
                EnglishEnumDescription = parts.Length > 0 ? parts[0] : string.Empty
            };
        }

        private static string GetEnumDescription(Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
            return attribute?.Description ?? value.ToString();
        }
    }
}
