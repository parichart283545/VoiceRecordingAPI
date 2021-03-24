using System;
using System.IO;
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

        [HttpGet("voicerecordlist")]
        public async Task<IActionResult> GetVoiceRecordJson(FilterVoiceRecordDetail filter)
        {
            var result = await _voiceRecordDetailService.GetVoiceRecordDetailJson(filter);
            return Ok(result);
        }

        [HttpGet("voicerecordurlbyid")]
        public async Task<IActionResult> GetVoiceRecordURLById(string voiceId)
        {
            string contentType = "audio/wav";
            var result = await _voiceRecordDetailService.GetVoiceRecordURL(voiceId);
            if (result.Data.Contains("http"))
            {
                var streamAudio = await _client.GetStreamAsync(result.Data);
                return File(streamAudio, contentType);
            }
            else return File(System.IO.File.OpenRead(result.Data), contentType);
            //return File(System.IO.File.OpenRead("D:\\Record\\[หญิง 0969426936]_1009-0853652569_202103191022(12345).wav"), "audio/wav");
            //string urlBlob = "https://anthonygiretti.blob.core.windows.net/videos/nature1.mp4";
            //return Ok(result);


            //return await _client.GetStreamAsync(result.Data);
        }

        [HttpGet("voicerecordurl")]
        public async Task<IActionResult> GetVoiceRecordURLParam(int? AgentId, int? CallType, DateTime StartDT, DateTime EndDT)
        {
            var result = await _voiceRecordDetailService.GetVoiceRecordURLParam(AgentId, CallType, StartDT, EndDT);
            return Ok(result);
        }
    }
}