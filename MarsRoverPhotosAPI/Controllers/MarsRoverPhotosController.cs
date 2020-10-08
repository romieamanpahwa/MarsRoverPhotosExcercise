using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace MarsRoverPhotosAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MarsRoverPhotosController : ControllerBase
    {
        readonly IConfiguration _Configuration;
        readonly string _DatesFilePath;
        readonly string _ExternalAPIURL;
        readonly string _DownloadPhotosDirectory;
        const string dateFormatExpected = "yyyy-MM-dd";       

        public MarsRoverPhotosController(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _Configuration = configuration;

            _ExternalAPIURL = _Configuration["MarsRoverPhotosQueryByEarthDateAPI_URL"];

            //Used to test v1 version of Web Methods: Accessing which the photos are stored locally 
            _DatesFilePath = Path.Combine(environment.WebRootPath, _Configuration["DatesFilePath"]);
            _DownloadPhotosDirectory = Path.Combine(environment.WebRootPath, _Configuration["DownloadPhotosDirectoryPath"]);           
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

        [Route("v1/photos/{date}")]
        //Retrieves Photos and Stores Locally
        public async Task<IActionResult> GetPhotosAsync(string date)
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

            //3. Download and Store all the photos locally
            int photosFoundAndDownloaded = await downloadPhotosAsync(apiResponse, formattedDate);
            return Ok(photosFoundAndDownloaded);
        }

        private async Task<int> downloadPhotosAsync(string photosJSONString, string photosCapturedOn)
        {
            var expectJsonStructure = new { photos = new[] { new { id = string.Empty, img_src = string.Empty } } };
            var photosJSON = JsonConvert.DeserializeAnonymousType(photosJSONString, expectJsonStructure);
            if (photosJSON.photos == null)
                return 0;

            int photosFound = photosJSON.photos.Length;
            if (photosFound > 0)
            {
                using (var httpClient = new HttpClient())
                {
                    //Create a sub directory
                    Directory.CreateDirectory(_DownloadPhotosDirectory + "\\" + photosCapturedOn);

                    foreach (var photo in photosJSON.photos)
                    {
                        var imageFileExtension = Path.GetExtension(photo.img_src);
                        var response = await httpClient.GetAsync($"{photo.img_src}");
                        response.EnsureSuccessStatusCode();
                        using var ms = await response.Content.ReadAsStreamAsync();


                        using var fs = System.IO.File.Create($@"{_DownloadPhotosDirectory}\{photosCapturedOn}\{photo.id}{imageFileExtension}");
                        ms.Seek(0, SeekOrigin.Begin);
                        ms.CopyTo(fs);
                    }
                }
            }

            return photosFound;
        }

        /// <summary>
        ///  Gets all the photos captured by Mars Rover on the dates listed in dates.txt(See Configuration for its FilePath)
        /// </summary>
        /// <returns></returns>        
        [Route("v1/photos")]
        public async Task<IActionResult> GetAsync()
        {
            StringBuilder sbBadRequest = new StringBuilder();
                               
            System.IO.StreamReader file =
                new System.IO.StreamReader(_DatesFilePath);
            string lineFromTxtFile = string.Empty;

            //ACCEPTANCE CRITERIA #2: ... by reading dates one-by-one from the file
            while (!string.IsNullOrEmpty(lineFromTxtFile = file.ReadLine()))
            {                
                await GetPhotosAsync(lineFromTxtFile);              
            }                     

            return Ok();
        }

    }
}



