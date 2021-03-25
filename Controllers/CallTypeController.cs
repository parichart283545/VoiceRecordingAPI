using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VoiceRecordAPI.Services;

namespace VoiceRecordAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CallTypeController : ControllerBase
    {
        private readonly ICallTypeService _callTypeService;
        public CallTypeController(ICallTypeService callTypeService)
        {
            _callTypeService = callTypeService;

        }

        [HttpGet("calltypelist")]
        public async Task<IActionResult> GetVoiceRecordProvidereList()
        {
            var result = await _callTypeService.GetCallTypeList();
            return Ok(result);
        }
    }
}

