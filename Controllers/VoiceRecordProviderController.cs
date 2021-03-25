using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VoiceRecordAPI.Services;

namespace VoiceRecordAPI.Controllers
{
    [ApiController]
    [Route("api/SystemProvider")]
    public class VoiceRecordProviderController : ControllerBase
    {
        private readonly IVoiceRecordProvidersService _voiceRecordProviderService;
        public VoiceRecordProviderController(IVoiceRecordProvidersService voiceRecordProviderService)
        {
            _voiceRecordProviderService = voiceRecordProviderService;

        }
        [HttpGet("systemproviderlist")]
        public async Task<IActionResult> GetVoiceRecordProvidereList()
        {
            var result = await _voiceRecordProviderService.GetVoiceRecordProviderList();
            return Ok(result);
        }
    }
}