using System;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using GeoIpApi.Data;
using GeoIpApi.Models;
using GeoIpApi.Models.Dtos;
using Newtonsoft.Json.Linq;

namespace GeoIpApi.Controllers
{
    [RoutePrefix("geoinfos"),FromUri]
    public class GeoInfosController : ApiController
    {
        private AppDbContext db = new AppDbContext();

        [HttpGet, Route("")]
        public IQueryable<GeoInfoDto> GetGeoInfos()
        {
            return db.GeoInfos.Include(gi => gi.Location.Languages).ToList().Select(Mapper.Map<GeoInfo,GeoInfoDto>).AsQueryable();            
        }

        [HttpGet, Route("{id}")]
        public async Task<IHttpActionResult> GetGeoInfo(int id)
        {
            GeoInfo geoInfo = await db.GeoInfos.Include(gi => gi.Location.Languages).Where(gi => gi.Id == id).SingleOrDefaultAsync();
            if (geoInfo == null)
            {
                return JsonResult.NotFound(string.Format("GeoInfo with id:{0} could not be found!", id));
            }

            var geoInfoDto = Mapper.Map<GeoInfo, GeoInfoDto>(geoInfo);

            return JsonResult.Success(geoInfoDto);
        }

        [HttpPost, Route("")]
        public async Task<IHttpActionResult> CreateGeoInfo([FromBody] string ip)
        {
            if (!ModelState.IsValid)
            {
                return JsonResult.BadRequest(ModelState.ToString());
            }
            if (!ValidateIPv4(ip))
                return JsonResult.BadRequest("IP address is in a wrong format!");
            var exists = await GeoInfoExists(ip);
            if(exists)
                return JsonResult.BadRequest("IP address already exists in the DB!");
            
            GeoInfo geoInfo = await FillWithDetails(ip);
            db.GeoInfos.Add(geoInfo);
            await db.SaveChangesAsync();
            return JsonResult.Success(geoInfo);
        }

        // DELETE: api/GeoInfos/5
        [HttpDelete,Route("{id}")]
        public async Task<IHttpActionResult> DeleteGeoInfo(int id)
        {
            GeoInfo geoInfo = await db.GeoInfos.FindAsync(id);
            if (geoInfo == null)
            {
                return JsonResult.NotFound(string.Format("GeoInfo with id:{0} could not be found!",id));
            }

            db.GeoInfos.Remove(geoInfo);
            await db.SaveChangesAsync();

            return JsonResult.Success(string.Format("GeoInfo with id:{0} was removed successfully!",id));
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
            var resss = await response.Content.ReadAsStringAsync();
            JObject jsonBody = JObject.Parse(await response.Content.ReadAsStringAsync());
            GeoInfo geoInfo = jsonBody.ToObject<GeoInfo>();
            return geoInfo;
        }
    }
}