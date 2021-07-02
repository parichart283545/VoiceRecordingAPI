using System;
using System.Collections.Generic;
using System.IO;
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
        Task<ServiceResponse<List<GetVoiceRecordDetail>>> GetVoiceRecordURLParam(string ExtensionNo, int? CallType, DateTime StartDT, DateTime EndDT);
        Task<ServiceResponse<List<GetVoiceRecordDetail>>> GetVoiceRecordURLByReceived(string ExtensionNo, int? CallType, DateTime DT);
        Task<ServiceResponse<string>> GetVoiceRecordURLWithFilter(RequestParams filter);
        Task<ServiceResponse<string>> GetVoiceRecordFileWithFilter(RequestParams filter);
    }
}