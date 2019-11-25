using AutoMapper;
using AutoMapper.QueryableExtensions;
using GeoIpApi.Data;
using GeoIpApi.Models;
using GeoIpApi.Models.Dtos;
using log4net;
using Newtonsoft.Json.Linq;
using System;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace GeoIpApi.Controllers
{
    [RoutePrefix("geoinfos"), FromUri]
    public class GeoInfosController : ApiController
    {
        private AppDbContext db = new AppDbContext();
        private ILog logger = LogManager.GetLogger("GeoInfosController");
        [HttpGet, Route("")]
        public async Task<IHttpActionResult> GetGeoInfos()
        {
            logger.Info("Fetching all GeoInfos from database");
            try
            {
                var geoInfosList = await db.GeoInfos.ProjectTo<GeoInfoDto>().ToListAsync();
                logger.Info(string.Format("Fetched {0} GeoInfos from database", geoInfosList.Count()));
                return JsonResult.Success(geoInfosList);
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                logger.Error(e.StackTrace);
                return JsonResult.Error(string.Format("Could not fetch GeoInfos due to error: {0}", e.Message));
            }
        }

        [HttpGet, Route("{id}")]
        public async Task<IHttpActionResult> GetGeoInfo(int id)
        {
            logger.Info(string.Format("Trying to find GeoInfo with id:{0}", id));
            try
            {
                GeoInfo geoInfo = await db.GeoInfos.Include(gi => gi.Location.Languages).Where(gi => gi.Id == id).SingleOrDefaultAsync();
                if (geoInfo == null)
                {
                    logger.Info(string.Format("GeoInfo with id:{0} could not be found!", id));
                    return JsonResult.NotFound(string.Format("GeoInfo with id:{0} could not be found!", id));
                }
                var geoInfoDto = Mapper.Map<GeoInfo, GeoInfoDto>(geoInfo);
                logger.Info(string.Format("Returning GeoInfo with id:{0}", id));
                return JsonResult.Success(geoInfoDto);
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                logger.Error(e.StackTrace);
                return JsonResult.Error(string.Format("Could not fetch GeoInfo with id:{0} due to error: {1}", id, e.Message));
            }
        }

        [HttpPost, Route("")]
        public async Task<IHttpActionResult> CreateGeoInfo([FromBody] string ip)
        {
            if (!ModelState.IsValid)
            {
                return JsonResult.BadRequest("IP address has to be a string");
            }
            if (!ValidateIPv4(ip))
                return JsonResult.BadRequest("Provided IP address is in a wrong format!");
            try
            {
                var exists = await GeoInfoExists(ip);
                if (exists)
                    return JsonResult.BadRequest("IP address already exists in the DB!");

                GeoInfo geoInfo = new GeoInfo();
                geoInfo = await FillWithDetails(ip);
                db.GeoInfos.Add(geoInfo);
                await db.SaveChangesAsync();
                return JsonResult.Success(Mapper.Map<GeoInfo, GeoInfoDto>(geoInfo));
            }
            catch (Exception e)
            {
                return JsonResult.Error(string.Format("Could not create GeoInfo with ip:{0} due to error: {1}", ip, e.Message));
            }
        }

        // DELETE: api/GeoInfos/5
        [HttpDelete, Route("{id}")]
        public async Task<IHttpActionResult> DeleteGeoInfo(int id)
        {
            try
            {
                GeoInfo geoInfo = await db.GeoInfos.FindAsync(id);
                if (geoInfo == null)
                {
                    return JsonResult.NotFound(string.Format("GeoInfo with id:{0} could not be found!", id));
                }

                db.GeoInfos.Remove(geoInfo);
                await db.SaveChangesAsync();
                return JsonResult.Success(string.Format("GeoInfo with id:{0} was removed successfully!", id));
            }
            catch (Exception e)
            {
                return JsonResult.Error(string.Format("Error: could not delete GeoInfo with id:{0} due to error :{1}", id, e.Message));
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private async Task<bool> GeoInfoExists(string ip)
        {
            return await db.GeoInfos.CountAsync(e => e.Ip == ip) > 0;
        }

        private bool ValidateIPv4(string ipString)
        {
            if (String.IsNullOrWhiteSpace(ipString))
            {
                return false;
            }

            string[] splitValues = ipString.Split('.');
            if (splitValues.Length != 4)
            {
                return false;
            }

            byte tempForParsing;

            return splitValues.All(r => byte.TryParse(r, out tempForParsing));
        }

        private async Task<GeoInfo> FillWithDetails(string ip)
        {
            var apiKey = ConfigurationManager.AppSettings["apiKey"];
            var url = String.Format("http://api.ipstack.com/{0}&access_key={1}", ip, apiKey);
            var response = await new HttpClient().GetAsync(url);
            if (!response.IsSuccessStatusCode)
                throw new Exception("Fetching ip details from external source failed! Please try again later.");
            JObject jsonBody = JObject.Parse(await response.Content.ReadAsStringAsync());
            GeoInfo geoInfo = jsonBody.ToObject<GeoInfo>();
            return geoInfo;
        }
    }
}