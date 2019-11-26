using log4net;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace GeoIpApi
{

    public class JsonResult : IHttpActionResult
    {
        public readonly HttpRequestMessage request;

        ILog logger = LogManager.GetLogger("Json Result");
        readonly object data;
        HttpStatusCode statusCode;
        string message;
        string error;
        bool formatJson;

        public JsonResult(HttpStatusCode statusCode = HttpStatusCode.OK, object data = null, string message = null, string error = null, bool formatJson = true)
        {
            this.data = data;
            this.statusCode = statusCode;
            this.message = message;
            this.error = error;
            this.formatJson = formatJson;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var startTime = DateTime.Now.Ticks;

            string jsonData = JsonConvert.SerializeObject(data, formatJson ? Formatting.Indented : Formatting.None);
            var response = new HttpResponseMessage
            {
                Content = new StringContent(jsonData, Encoding.UTF8, "text/json"),
                StatusCode = statusCode
            };
            logger.Debug(string.Format("ExecuteAsync took: {0:0}mu", new TimeSpan(DateTime.Now.Ticks - startTime).Ticks / (TimeSpan.TicksPerMillisecond / 1000)));
            return Task.FromResult(response);
        }

        #region ShorthandResults
        public static JsonResult NotFound(string message = null)
        {
            return new JsonResult(HttpStatusCode.NotFound, message, "The requested resource does not exist");
        }
        public static JsonResult BadRequest(string message = null)
        {
            return new JsonResult(HttpStatusCode.BadRequest, message);
        }

        public static JsonResult Success(object data = null, string message = null)
        {
            return new JsonResult(HttpStatusCode.OK, data, message);
        }

        public static JsonResult Error(object data = null, string message = null)
        {
            return new JsonResult(HttpStatusCode.InternalServerError, data, message);
        }
        #endregion
    }
}

