namespace Dormy.WebService.Api.Models.ResponseModels
{
    public class InitialRegistrationDataResponseModel
    {
        public List<EnumResponseModel>? GenderEnums { get; set; } = [];

        public List<EnumResponseModel>? RelationshipEnums { get; set; } = [];

        public List<InitialDataWorkplaceResponseModel>? ListWorkplaces { get; set; } = [];
    }

    public class InitialDataWorkplaceResponseModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Abbrevation { get; set; } = string.Empty;
    }
}
