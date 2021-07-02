using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VoiceRecordAPI.DTOs;
using VoiceRecordAPI.Services;
namespace VoiceRecordAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VoiceRecordDetailController : ControllerBase
    {
        private readonly IVoiceRecordDetailService _voiceRecordDetailService;
        private HttpClient _client;
        public VoiceRecordDetailController(IVoiceRecordDetailService voiceRecordDetailService)
        {
            _voiceRecordDetailService = voiceRecordDetailService;
            _client = new HttpClient();
        }

        [HttpGet("voicerecordfilebyguid")]
        public async Task<IActionResult> GetVoiceRecordFileByGuid(string guid)
        {
            if (string.IsNullOrEmpty(guid)) { return NotFound(); }
            string contentType = "application/octet-stream";

            var result = await _voiceRecordDetailService.GetVoiceRecordFileByGuid(guid);
            if (string.IsNullOrEmpty(result.Data)) { return NotFound(); }
            if (result.Data.Equals("Expired")) { return Ok("Expired"); }
            if (result.Data.Contains("http"))
            {
                //test get url is exist
                bool exist = false;
                try
                {
                    HttpWebRequest request = (HttpWebRequest)System.Net.WebRequest.Create(result.Data);
                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    {
                        exist = response.StatusCode == HttpStatusCode.OK;
                    }
                }
                catch
                {
                    return NotFound();
                }

                if (exist)
                {
                    var streamAudio = await _client.GetStreamAsync(result.Data);
                    return File(streamAudio, contentType);
                }
                else { return NotFound(); }
            }
            else
            {
                FileStreamResult fileStream = null;
                if (System.IO.File.Exists(result.Data))
                {
                    fileStream = File(System.IO.File.OpenRead(result.Data), contentType, Path.GetFileName(result.Data));
                }
                else //find in virtual directory
                {
                    WebClient Client = null;
                    Stream OutputStream = null;
                    try
                    {
                        Client = new WebClient();
                        Client.Credentials = new NetworkCredential("develop", "pn,9y'8Nlv'ihvp");
                        OutputStream = Client.OpenRead(result.Data);
                        if (!OutputStream.CanRead)
                            return NotFound();
                        fileStream = File(OutputStream, contentType, Path.GetFileName(result.Data));
                    }
                    catch
                    {
                        return NotFound();
                    }
                };
                return fileStream;
            }
        }

        [HttpGet("voicerecordurl")]
        public async Task<IActionResult> GetVoiceRecordURL([FromQuery] RequestParams filter)
        {
            return Ok(await _voiceRecordDetailService.GetVoiceRecordURLWithFilter(filter));
        }
    }
}