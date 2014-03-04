using StatIt.Engine.Distimo.Services;
using StatIt.Engine.Distimo.Services.Models;
using StatIt.Engine.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Xunit.Extensions;

namespace StatIt.Tests.Distimo.Services
{
    public class DistimoServiceTests
    {
        private Mock<IWebRequestService> MockWebRequestService;
        private Mock<IDistimoAuthService> MockDistimoAuthService;

        public DistimoServiceTests()
        {
            MockWebRequestService = new Mock<IWebRequestService>();
            
            MockDistimoAuthService = new Mock<IDistimoAuthService>();
            MockDistimoAuthService.SetupProperty(property => property.DistimoPrivateKey, "distPrivateKey");
        }

        [Fact]
        public void Is_Revenues_API_Address_As_Expected()
        {
            // Arrange
            var distimoService = new DistimoService(MockWebRequestService.Object, MockDistimoAuthService.Object);
            var distimoBaseAddress = distimoService.DistimoAPIBaseAddress;

            // Act
            var address = distimoService.GetDistimoAPIAddress(SupportedDistimoApis.Revenues);

            // Assert 
            Assert.Equal(address, distimoBaseAddress + "revenues");
        }

        [Fact]
        public void Is_Filter_Asset_Revenues_API_Address_As_Expected()
        {
            // Arrange
            var distimoService = new DistimoService(MockWebRequestService.Object, MockDistimoAuthService.Object);
            var distimoBaseAddress = distimoService.DistimoAPIBaseAddress;

            // Act
            var address = distimoService.GetDistimoAPIAddress(SupportedDistimoApis.FilterAssetRevenues);

            // Assert 
            Assert.Equal(address, distimoBaseAddress + "filters/assets/revenues");
        }

        [Fact]
        public void Is_Assets_API_Address_As_Expected()
        {
            // Arrange
            var distimoService = new DistimoService(MockWebRequestService.Object, MockDistimoAuthService.Object);
            var distimoBaseAddress = distimoService.DistimoAPIBaseAddress;

            // Act
            var address = distimoService.GetDistimoAPIAddress(SupportedDistimoApis.Assets);

            // Assert 
            Assert.Equal(address, distimoBaseAddress + "assets/app");
        }

        [Theory,
        InlineData("from=all&revenue=total&view=line&breakdown=application,appstore"),
        InlineData("from=2014-02-01&to=2014-02-28&revenue=total&view=line&breakdown=application,appstore,date&interval=week")]
        public void Iain_Test(string queryString)
        {
            // Arrange
            var distimoService = new DistimoService(MockWebRequestService.Object, MockDistimoAuthService.Object);

            // Act
            var request = distimoService.CreateDistimoRequest(SupportedDistimoApis.Revenues, queryString);



        }
    }
}
