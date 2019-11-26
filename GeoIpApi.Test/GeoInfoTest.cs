using System;
using System.Threading.Tasks;
using GeoIpApi.Controllers;
using GeoIpApi.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeoIpApi.Test
{
    [TestClass]
    public class GeoInfoTest
    {

        [TestMethod]
        public async Task GetAllGeoInfos()
        {
            var controller = new GeoInfosController();
            var result = await controller.GetGeoInfos();
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task GetGeoInfoByIdAsyc()
        {
            var controller = new GeoInfosController();
            var result = await controller.GetGeoInfo(1);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task CreateGeoInfo()
        {
            var controller = new GeoInfosController();
            var result = await controller.CreateGeoInfo("123.123.42.32");
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void DeleteGeoInfoById()
        {

        }


        
    }
}
