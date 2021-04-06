using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VoiceRecordAPI.DTOs;
using VoiceRecordAPI.Models;

namespace VoiceRecordAPI.Services
{
    public interface IVoiceRecordDetailService
    {
        Task<ServiceResponseWithPagination<List<GetVoiceRecordDetail>>> GetVoiceRecordDetailJson(FilterVoiceRecordDetail filter);
        Task<ServiceResponse<string>> GetVoiceRecordURL(string voiceId);
        Task<ServiceResponse<string>> GetVoiceRecordFileByGuid(string guidStr);
        //int? AgentId,int? CallType, DateTime StartDT,DateTime EndDT
        Task<ServiceResponse<List<GetVoiceRecordDetail>>> GetVoiceRecordURLParam(int? ExtensionNo, int? CallType, DateTime StartDT, DateTime EndDT);
        Task<ServiceResponse<List<GetVoiceRecordDetail>>> GetVoiceRecordURLByReceived(int? ExtensionNo, int? CallType, DateTime DT);
        Task<ServiceResponse<string>> GetVoiceRecordURLWithFilter(RequestParams filter);
    }
}