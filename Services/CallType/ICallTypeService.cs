using System.Collections.Generic;
using System.Threading.Tasks;
using VoiceRecordAPI.DTOs;
using VoiceRecordAPI.Models;

namespace VoiceRecordAPI.Services
{
    public interface ICallTypeService
    {
        Task<ServiceResponse<List<GetCallTypes>>> GetCallTypeList();
    }
}