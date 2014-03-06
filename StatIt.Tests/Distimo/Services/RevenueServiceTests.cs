using Moq;
using StatIt.Engine.Distimo.Services;
using StatIt.Engine.Distimo.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace StatIt.Tests.Distimo.Services
{
    public class RevenueServiceTests
    {
        Mock<IDistimoService> MockDistimoService;

        public RevenueServiceTests()
        {
            MockDistimoService = new Mock<IDistimoService>();
        }

        [Fact]
        public void Test_Revenues_Date_Strings_Formmatted_Correctly()
        {
            // Arrange
            var revenuesService = new RevenuesService(MockDistimoService.Object);

            // Act
            var testStartDate = DateTime.Now;
            var testEndDate = DateTime.Now;
            var queryString = revenuesService.BuildRevenuesQueryString(testStartDate, testEndDate);

            // Assert
            var startIndex = queryString.IndexOf("from=");
            var startDate = queryString.Substring(startIndex, 15);

            var toIndex = queryString.IndexOf("to=");
            var endDate = queryString.Substring(toIndex, 13);

            Assert.Equal(startDate, "from=" + testStartDate.ToString("yyyy-MM-dd"));
            Assert.Equal(endDate, "to=" + testEndDate.ToString("yyyy-MM-dd"));
        }

        [Fact]
        public void Test_Specified_Data_Extracted_Correctly()
        {
            // Arrange 
            
            // setup some sample json
            MockDistimoService.Setup(x => x.GetDistimoData(SupportedDistimoApis.Revenues, "mockQueryString")).Returns(@"{""lines"":[{""data"":{""application_id"":""515117"",""application"":""Winx Fairy School (Kindle Tablet Edition)"",""revenuetype_id"":""total"",""revenuetype"":""total"",""appstore_id"":""10"",""appstore"":""Amazon Appstore"",""country_id"":-1,""country"":""All Countries"",""metric_id"":""download"",""metric"":""One-Off Revenue"",""currency_id"":2,""currency"":2},""pointStart"":1390176000000,""points"":[1651,1214,1261,938,1582,859,117]},{""data"":{""application_id"":""515866"",""application"":""Sing It, Laurie!"",""revenuetype_id"":""total"",""revenuetype"":""total"",""appstore_id"":""1"",""appstore"":""Apple App Store"",""country_id"":-1,""country"":""All Countries"",""metric_id"":""download"",""metric"":""One-Off Revenue"",""currency_id"":2,""currency"":2},""pointStart"":1390176000000,""points"":[56.81,47.52,95.68,71.76,37.78,29.7,14.85]},{""data"":{""application_id"":""517699"",""application"":""Winx Fairy School"",""revenuetype_id"":""total"",""revenuetype"":""total"",""appstore_id"":""1"",""appstore"":""Apple App Store"",""country_id"":-1,""country"":""All Countries"",""metric_id"":""download"",""metric"":""One-Off Revenue"",""currency_id"":2,""currency"":2},""pointStart"":1390176000000,""points"":[4510,4208,3804,3898,4071,3320,923]}]}");
            var revenuesService = new RevenuesService(MockDistimoService.Object);

            // Act 
            var dataItems = revenuesService.ExtractAppRevenueData("Winx Fairy School", "mockQueryString");

            // Assert
            Assert.Equal(dataItems.Count, 2);
        }
    }
}
