using System.Collections.Generic;
using System.Threading.Tasks;
using VoiceRecordAPI.DTOs;
using VoiceRecordAPI.Models;

namespace VoiceRecordAPI.Services
{
    public interface IVoiceRecordProvidersService
    {
        Task<ServiceResponse<List<GetVoiceRecordProviders>>> GetVoiceRecordProviderList();
    }
}