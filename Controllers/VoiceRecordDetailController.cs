using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NAudio.Lame;
using NAudio.Wave;
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

        // [HttpGet("voicerecordfilebyid")]
        // public async Task<IActionResult> GetVoiceRecordFileById(string Id)
        // { 

        // }

        [HttpGet("voicerecordfilebyidTest")]
        private async Task<IActionResult> GetVoiceRecordFileByIdTest(string Id)
        {
            string contentType = "audio/wav";
            // //Create byte array
            // byte[] file;// = readFile("http://www-mmsp.ece.mcgill.ca/Documents/AudioFormats/WAVE/Samples/AFsp/M1F1-Alaw-AFsp.wav");
            var streamAudio = await _client.GetStreamAsync("http://www.externalharddrive.com/waves/animal/bird.wav");
            var file = File(streamAudio, contentType);

            // return new FilePathResult("http://www.externalharddrive.com/waves/animal/bird.wav", "audio/wav");
            string html = @" 
            <title>My report</title>
            <style type='text/css'>
            button{
                color: green;
            }
            </style>
            <h1> Header </h1>
            <p>Hello There <button>click me</button></p>
            <p style='color:blue;'>I am blue</p>
            ";

            //return file;
            return Content(html, "text/html", Encoding.UTF8);
            //return View();
        }

        [HttpGet("voicerecordfilebyguid")]
        public async Task<IActionResult> GetVoiceRecordFileByGuid(string guid)
        {
            if (string.IsNullOrEmpty(guid)) { return NotFound(); }
            string contentType = "audio/wav";

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
                var fileStream = File(System.IO.File.OpenRead(result.Data), contentType);
                return fileStream;
                // var sysys = System.IO.File.ReadAllBytes("D:\\SoundFile3CX\\1001\\[Witcha Yawichai]_1001-0898923246_20210330085027(78).wav");
                // var target = new WaveFormat(8000, 16, 1);
                // using (var outPutStream = new MemoryStream())
                // using (var waveStream = new WaveFileReader(new MemoryStream(sysys)))
                // using (var conversionStream = new WaveFormatConversionStream(target, waveStream))
                // using (var writer = new LameMP3FileWriter(outPutStream, conversionStream.WaveFormat, 32, null))
                // {
                //     conversionStream.CopyTo(writer);
                //     //return outPutStream.ToArray();
                //     var fileStream = File(outPutStream.ToArray(), contentType);
                //     return fileStream;
                // }



            }
        }

        [HttpGet("voicerecordfilebyid")]
        private async Task<IActionResult> GetVoiceRecordFileById(string Id)
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
            else
            {
                var fileStream = File(System.IO.File.OpenRead(result.Data), contentType);
                return fileStream;
            }

        }

        [HttpGet("voicerecordlst")]
        private async Task<IActionResult> GetVoiceRecordLst(string ExtensionId, int? CallType, DateTime ReceivedStartDatetime, DateTime ReceivedEndDatetime)
        {
            var result = await _voiceRecordDetailService.GetVoiceRecordURLParam(ExtensionId, CallType, ReceivedStartDatetime, ReceivedEndDatetime);
            return Ok(result);
        }

        [HttpGet("voicerecordbyreceived")]
        private async Task<IActionResult> GetVoiceRecordByReceived(string ExtensionId, int? CallType, DateTime ReceivedDatetime)
        {
            var result = await _voiceRecordDetailService.GetVoiceRecordURLByReceived(ExtensionId, CallType, ReceivedDatetime);
            return Ok(result);
        }

        [HttpGet("voicerecordfile")]
        private async Task<IActionResult> GetVoiceRecordFile([FromQuery] RequestParams filter)
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
            // string MyBatchFile = @"D:\My Test Code\ConsoleApp2\ConsoleApp2\bin\Debug\ConsoleApp2.exe";
            // string _sourcePath = @"D:\SoundFile3CX\1001\[Nukul Dangsompappume]_1005-0982854709_20210429141027(34).wav";
            // string _tempTargetPath = @"D:\SoundFile3CX\Convert\test.mp3";

            // var process = new Process
            // {
            //     StartInfo = {
            //           Arguments = string.Format("\"{0}\" \"{1}\"",
            //                                     _sourcePath,
            //                                     _tempTargetPath)
            //                     }
            // };
            // process.StartInfo.FileName = MyBatchFile;
            // bool b = process.Start();

            var result = await _voiceRecordDetailService.GetVoiceRecordURLWithFilter(filter);
            return Ok(result);
        }

        [HttpGet("voicerecordfile")]
        public async Task<IActionResult> GetVoiceRecordFileDownload([FromQuery] RequestParams filter)
        {
            string contentType = "application/octet-stream";
            var result = await _voiceRecordDetailService.GetVoiceRecordFileWithFilter(filter);
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
                    return File(streamAudio, contentType, Path.GetFileName(result.Data));
                }
                else { return NotFound(); }
            }
            else
            {
                var fileStream = File(System.IO.File.OpenRead(result.Data), contentType, Path.GetFileName(result.Data));
                return fileStream;
            }

        }


    }
}