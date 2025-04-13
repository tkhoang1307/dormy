
using Dormy.WebService.Api.Core.Entities;
using Dormy.WebService.Api.Presentation.Mappers;

namespace Dormy.UnitTests.Tests.VehicleMapperTests
{
    public class VehicleMapperTests
    {
        private readonly VehicleMapper _mapper;

        public VehicleMapperTests()
        {
            _mapper = new VehicleMapper();
        }

        [Fact]
        public void TestVehicleMapper_Success()
        {
            VehicleEntity data = new VehicleEntity
            {
                Id = Guid.Empty,
                VehicleType = "SUV",
                User = new UserEntity
                {
                    FirstName = "Adam",
                    LastName = "John"
                }
            };

            var result = _mapper.MapToVehicleResponseModel(data);

            Assert.NotNull(result);
            Assert.Equal("SUV", result.VehicleType);
            Assert.Equal("Adam John", result.UserFullname);
        }
    }
}
