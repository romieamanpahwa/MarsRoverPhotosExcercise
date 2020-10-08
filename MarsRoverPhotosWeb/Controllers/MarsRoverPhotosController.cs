using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace MarsRoverPhotosWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MarsRoverPhotosController : ControllerBase
    {
        readonly IConfiguration _Configuration;       
        readonly string _ExternalAPIURL;       
        const string dateFormatExpected = "yyyy-MM-dd";

        public MarsRoverPhotosController(IConfiguration configuration)
        {
            _Configuration = configuration;
            _ExternalAPIURL = _Configuration["MarsRoverPhotosQueryByEarthDateAPI_URL"];
        }

        [Route("photos/{date}")]
        public async Task<IActionResult> GetPhotosByEarthDateAsync(string date)
        {
            try
            {
                //1. Check 'date' for a valid Datetime
                DateTime? validDate = validateDate(date);
                if (validDate == null)
                {
                    return BadRequest(date);
                }

                string formattedDate = validDate.Value.ToString(dateFormatExpected);
                string url = string.Format(_ExternalAPIURL, formattedDate);

                string apiResponse = string.Empty;
                using (var httpClient = new HttpClient())
                {
                    //2. Consume/Invoke external web api: Mars Rover Photos API
                    using (var response = await httpClient.GetAsync(url))
                    {
                        apiResponse = await response.Content.ReadAsStringAsync();
                    }
                }

                if (apiResponse == null)
                {
                    return BadRequest(date);
                }
                var expectJsonStructure = new { photos = new[] { new { id = string.Empty, img_src = string.Empty } } };
                var photosJSON = JsonConvert.DeserializeAnonymousType(apiResponse, expectJsonStructure);

                int photosFound = photosJSON.photos.Length;
                var statusCode = (int)System.Net.HttpStatusCode.OK;
                if (photosFound == 0)
                {
                    statusCode = (int)System.Net.HttpStatusCode.NoContent;
                }

                return new JsonResult(photosJSON.photos) { ContentType = "application/json", StatusCode = statusCode };
            }
            catch (Exception ex)
            {
                //TODO: log 'ex'
                return new JsonResult("Failed") { StatusCode = (int)System.Net.HttpStatusCode.InternalServerError };
            }
        }

        private DateTime? validateDate(string strDate)
        {
            if (DateTime.TryParse(strDate, out var tmpDate))
            {
                return tmpDate;
            }
            else
            {
                return null;
            }
        }

    }
}



