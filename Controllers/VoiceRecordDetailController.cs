using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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

        // [HttpGet("voicerecordlist")]
        // public async Task<IActionResult> GetVoiceRecordJson(FilterVoiceRecordDetail filter)
        // {
        //     var result = await _voiceRecordDetailService.GetVoiceRecordDetailJson(filter);
        //     return Ok(result);
        // }

        [HttpGet("voicerecordfilebyid")]
        public async Task<IActionResult> GetVoiceRecordFileById(string Id)
        {
            if (string.IsNullOrEmpty(Id)) { return NoContent(); }
            string contentType = "audio/wav";
            //Testing Begin
            // var streamAudio1 = await _client.GetStreamAsync("https://anthonygiretti.blob.core.windows.net/videos/nature1.mp4");
            // return File(streamAudio1, contentType);
            //Testing End

            var result = await _voiceRecordDetailService.GetVoiceRecordURL(Id);
            if (string.IsNullOrEmpty(result.Data)) { return NoContent(); }
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
                }

                if (exist)
                {
                    var streamAudio = await _client.GetStreamAsync(result.Data);
                    return File(streamAudio, contentType);
                }
                else { return NoContent(); }
            }
            else return File(System.IO.File.OpenRead(result.Data), contentType);
            //**************************************************************************************//
            //return File(System.IO.File.OpenRead("D:\\Record\\[หญิง 0969426936]_1009-0853652569_202103191022(12345).wav"), "audio/wav");
            //string urlBlob = "https://anthonygiretti.blob.core.windows.net/videos/nature1.mp4";
            //return Ok(result);


            //return await _client.GetStreamAsync(result.Data);

        }

        [HttpGet("voicerecordlst")]
        public async Task<IActionResult> GetVoiceRecordLst(int? ExtensionId, int? CallType, DateTime ReceivedStartDatetime, DateTime ReceivedEndDatetime)
        {
            var result = await _voiceRecordDetailService.GetVoiceRecordURLParam(ExtensionId, CallType, ReceivedStartDatetime, ReceivedEndDatetime);
            return Ok(result);
        }

        [HttpGet("voicerecordfile")]
        public async Task<IActionResult> GetVoiceRecordFile([FromQuery] RequestParams filter)
        {
            string contentType = "audio/wav";
            var result = await _voiceRecordDetailService.GetVoiceRecordURLWithFilter(filter);
            if (string.IsNullOrEmpty(result.Data)) { return NoContent(); }
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
                }

                if (exist)
                {
                    var streamAudio = await _client.GetStreamAsync(result.Data);
                    return File(streamAudio, contentType);
                }
                else { return NoContent(); }
            }
            else return File(System.IO.File.OpenRead(result.Data), contentType);
        }

        [HttpGet("voicerecordurl")]
        public async Task<IActionResult> GetVoiceRecordURL([FromQuery] RequestParams filter)
        {
            var result = await _voiceRecordDetailService.GetVoiceRecordURLWithFilter(filter);
            return Ok(result);
        }
    }
}