using System.Collections.Generic;
using System.Threading.Tasks;
using VoiceRecordAPI.DTOs;
using VoiceRecordAPI.Models;

namespace VoiceRecordAPI.Services
{
    public interface IVoiceRecordURLService
    {
        Task<ServiceResponse<GetVoiceRecordURLRequest>> InsertVoiceRecordURL(int voiceRecDetId, string voiceRecDetUrl);
    }
}