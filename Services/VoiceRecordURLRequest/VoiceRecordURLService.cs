using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VoiceRecordAPI.Data;
using VoiceRecordAPI.DTOs;
using VoiceRecordAPI.Models;

namespace VoiceRecordAPI.Services
{
    public class VoiceRecordURLService : ServiceBase, IVoiceRecordURLService
    {
        private readonly AppDBContext _dBContext;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContext;
        private readonly ILogger<VoiceRecordURLService> _log;
        public VoiceRecordURLService(AppDBContext dBContext,
                                    IMapper mapper,
                                    IHttpContextAccessor httpContext,
                                    ILogger<VoiceRecordURLService> log) : base(dBContext, mapper, httpContext)
        {
            _httpContext = httpContext;
            _dBContext = dBContext;
            _mapper = mapper;
            _log = log;
        }
        public async Task<ServiceResponse<GetVoiceRecordURLRequest>> InsertVoiceRecordURL(int voiceRecDetId, string voiceRecDetUrl)
        {
            //Get timeout from configuration
            var configLst = await _dBContext.VoiceRecordConfigurations.Where(x => x.ParameterName == "URLTimeout" || x.ParameterName == "WebDomainFormat").ToListAsync();
            Guid guid = Guid.NewGuid();
            string newURL = configLst.Where(x => x.ParameterName == "WebDomainFormat").Select(x => x.ValueString).Single().Replace("{GUID}", guid.ToString());
            DateTime createDT = DateTime.Now;
            //Insert raw URL
            try
            {
                VoiceRecordURLRequest voiceURL = new VoiceRecordURLRequest();
                voiceURL.Id = guid;
                voiceURL.VoiceRecordDetailId = voiceRecDetId;
                voiceURL.VoiceRecordDetailURL = voiceRecDetUrl;
                voiceURL.ResponseURL = newURL;
                voiceURL.CreatedDatetime = createDT;
                voiceURL.ExpireDatetime = GetExpireDatetime(createDT,
                                                            configLst.Where(x => x.ParameterName == "URLTimeout").Select(x => x.ValueString).Single(),
                                                            Convert.ToInt32(configLst.Where(x => x.ParameterName == "URLTimeout").Select(x => x.ValueNumber).Single()));
                //add to datacontext
                _dBContext.VoiceRecordURLRequests.Add(voiceURL);
                //Save
                await _dBContext.SaveChangesAsync();
                //Mapping model model and dto
                var dto = _mapper.Map<GetVoiceRecordURLRequest>(voiceURL);
                return ResponseResult.Success(dto);
            }
            catch (System.Exception ex)
            {
                //Write log
                _log.LogError($"Add VoiceRecordURL is error detail: {ex.Message}");
                //Return 
                return ResponseResult.Failure<GetVoiceRecordURLRequest>($"Add VoiceRecordURL error detail: {ex.Message}");
            }
        }

        public DateTime GetExpireDatetime(DateTime createDT, string typeString, int value)
        {
            DateTime nowDT = new DateTime();
            if (typeString.Equals("minute".ToLowerInvariant()))
            { nowDT = nowDT.AddMinutes(value); }
            else if (typeString.Equals("hour".ToLowerInvariant()))
            { nowDT = nowDT.AddHours(value); }
            else if (typeString.Equals("day".ToLowerInvariant()))
            { nowDT = nowDT.AddDays(value); }
            else if (typeString.Equals("month".ToLowerInvariant()))
            { nowDT = nowDT.AddMonths(value); }
            else if (typeString.Equals("year".ToLowerInvariant()))
            { nowDT = nowDT.AddYears(value); }
            else
            { //default 7 days
                nowDT = nowDT.AddDays(7);
            }
            return nowDT;
        }



    }
}