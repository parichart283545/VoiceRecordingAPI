using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VoiceRecordAPI.Data;
using VoiceRecordAPI.DTOs;
using VoiceRecordAPI.Models;
using System.Linq.Dynamic.Core;
using VoiceRecordAPI.Helpers;
using System;
using System.IO;
using System.Text;

namespace VoiceRecordAPI.Services
{
    public class VoiceRecordDetailService : ServiceBase, IVoiceRecordDetailService
    {
        private readonly AppDBContext _dBContext;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContext;
        private readonly ILogger<VoiceRecordDetailService> _log;

        public VoiceRecordDetailService(AppDBContext dBContext,
                            IMapper mapper,
                            IHttpContextAccessor httpContext,
                            ILogger<VoiceRecordDetailService> log) : base(dBContext, mapper, httpContext)
        {
            _httpContext = httpContext;
            _dBContext = dBContext;
            _mapper = mapper;
            _log = log;
        }
        public async Task<ServiceResponseWithPagination<List<GetVoiceRecordDetail>>> GetVoiceRecordDetailJson(FilterVoiceRecordDetail filter)
        {
            var queryable = _dBContext.VoiceRecordDetails.AsNoTracking().AsQueryable();

            //************************************Start Filter*************************************//
            //Filter AgentId
            if (filter.ExtensionNo != null)//null is for get all AgentId
            {
                queryable = queryable.Where(x => x.ExtensionNo == filter.ExtensionNo);
            }
            //Filter CallType
            if (filter.CallingType != null) //null is for get all type
            {
                queryable = queryable.Where(x => x.CallTypeId == filter.CallingType);
            }
            //Filter duration of date
            if (filter.EndDate != default(DateTime))
            {
                queryable = queryable.Where(x => x.CreatedDate >= filter.StartDate && x.CreatedDate <= filter.EndDate);
            }
            //************************************End Filter*************************************//

            //Ordering
            if (!string.IsNullOrWhiteSpace(filter.OrderingField))
            {
                try
                {
                    queryable = queryable.OrderBy($"{filter.OrderingField} {(filter.AscendingOrder ? "ascending" : "descending")}");
                }
                catch
                {
                    return ResponseResultWithPagination.Failure<List<GetVoiceRecordDetail>>($"Could not order by field: {filter.OrderingField}");
                }
            }

            var paginationResult = await _httpContext.HttpContext
                .InsertPaginationParametersInResponse(queryable, filter.RecordsPerPage, filter.Page);

            var dto = await queryable.Paginate(filter).AsNoTracking().ToListAsync();

            var VoiceDto = _mapper.Map<List<GetVoiceRecordDetail>>(dto);

            return ResponseResultWithPagination.Success(VoiceDto, paginationResult);

        }


        public async Task<ServiceResponse<string>> GetVoiceRecordURL(string voiceId)
        {
            voiceId = voiceId.Trim(new char[] { ' ' });
            var queryable = await _dBContext.VoiceRecordDetails.Where(x => x.Id == Convert.ToInt32(voiceId)).AsNoTracking().FirstOrDefaultAsync();
            if (queryable is null) { return ResponseResult.Success<string>(""); }
            //Testing begin
            if (!Directory.Exists(queryable.FilePath))
                Directory.CreateDirectory(queryable.FilePath);
            if (!File.Exists(queryable.FullPath))
                File.Copy("D:\\Record\\[หญิง 0969426936]_1009-0853652569_202103191022(12345).wav", queryable.FullPath);
            //Testing end

            var VoiceDto = _mapper.Map<GetVoiceRecordDetail>(queryable);

            string fileString = (VoiceDto.URLPath == null || VoiceDto.URLPath == string.Empty) ? VoiceDto.FullPath : VoiceDto.URLPath;

            //return ResponseResult.Success(fileString);
            return ResponseResult.Success(VoiceDto.FullPath);//Testing
        }

        public async Task<ServiceResponse<List<GetVoiceRecordDetail>>> GetVoiceRecordURLByReceived(string ExtensionNo, int? CallType, DateTime DT)
        {
            var queryable = _dBContext.VoiceRecordDetails.AsNoTracking().AsQueryable();

            //************************************Start Filter*************************************//
            //Filter AgentId
            if (ExtensionNo != null)//null is for get all AgentId
            {
                queryable = queryable.Where(x => x.ExtensionNo == ExtensionNo);
            }

            //Filter CallType
            if (CallType != null) //null is for get all type
            {
                queryable = queryable.Where(x => x.CallTypeId == CallType);
            }
            //Filter duration of date
            //queryable = queryable.Where(x => (x.FileCreateDatetime.ToString("yyyy/MM/dd HH:mm") == DT.ToString("yyyy/MM/dd HH:mm")));

            //************************************End Filter*************************************//

            var dto = await queryable.AsNoTracking().ToListAsync();
            var dtoDt = dto.Where(x => (x.FileCreateDatetime.ToString("yyyy/MM/dd HH:mm") == DT.ToString("yyyy/MM/dd HH:mm"))).ToList();

            var VoiceDto = _mapper.Map<List<GetVoiceRecordDetail>>(dtoDt);

            return ResponseResult.Success(VoiceDto);
        }

        public async Task<ServiceResponse<List<GetVoiceRecordDetail>>> GetVoiceRecordURLParam(string ExtensionNo, int? CallType, DateTime StartDT, DateTime EndDT)
        {
            var queryable = _dBContext.VoiceRecordDetails.AsNoTracking().AsQueryable();

            //************************************Start Filter*************************************//
            //Filter AgentId
            if (ExtensionNo != null)//null is for get all AgentId
            {
                queryable = queryable.Where(x => x.ExtensionNo == ExtensionNo);
            }

            //Filter CallType
            if (CallType != null) //null is for get all type
            {
                queryable = queryable.Where(x => x.CallTypeId == CallType);
            }
            //Filter duration of date
            queryable = queryable.Where(x => x.FileCreateDatetime >= StartDT && x.FileCreateDatetime <= EndDT);

            //************************************End Filter*************************************//

            var dto = await queryable.AsNoTracking().ToListAsync();

            var VoiceDto = _mapper.Map<List<GetVoiceRecordDetail>>(dto);

            return ResponseResult.Success(VoiceDto);
        }

        // public Task<ServiceResponse<string>> GetVoiceRecordURL(int? ExtensionNo, int? CallType, DateTime StartDT, DateTime EndDT)
        // {
        //     throw new NotImplementedException();
        // }

        public async Task<ServiceResponse<string>> GetVoiceRecordURLWithFilter(RequestParams filter)
        {
            var queryable = _dBContext.VoiceRecordDetails.AsNoTracking().AsQueryable();

            //************************************Start Filter*************************************//
            //DialNumber
            if (!string.IsNullOrEmpty(filter.DialNumber))
            {
                if (!int.TryParse(filter.DialNumber, out int n)) { return ResponseResult.Failure<string>("DialNumber is not numeric"); }
                queryable = queryable.Where(x => x.PhoneNumberFrom == filter.DialNumber);
            }
            //DestinationNumber
            if (!string.IsNullOrEmpty(filter.DestinationNumber))
            {
                if (!int.TryParse(filter.DestinationNumber, out int n1)) { return ResponseResult.Failure<string>("DestinationNumber is not numeric"); }
                queryable = queryable.Where(x => x.PhoneNumberTo == filter.DestinationNumber);
            }
            //ExtensionId
            if (!string.IsNullOrEmpty(filter.ExtensionId))
            {
                if (!int.TryParse(filter.ExtensionId, out int n2)) { return ResponseResult.Failure<string>("ExtensionId is not numeric"); }
                queryable = queryable.Where(x => x.ExtensionNo == filter.ExtensionId);
            }
            //CallType
            if (!(filter.CallTypeId is null))
            {
                queryable = queryable.Where(x => x.CallTypeId == Convert.ToInt32(filter.CallTypeId));
            }
            //ReceivedDatetime & EndDatetime
            //queryable = queryable.Where(x => x.CreatedDate >= ReceivedDatetime && x.CreatedDate <= EndDatetime);

            //ringgingDatetime 
            //receivedDatetime +- no more 5 minutes
            queryable = queryable.Where(x => x.FileCreateDatetime >= filter.ReceivedDatetime.AddMinutes(-1) && x.FileCreateDatetime <= filter.ReceivedDatetime.AddMinutes(1));
            //endDatetime
            queryable = queryable.Where(x => x.FileModifyDatetime >= filter.EndDatetime.AddMinutes(-1) && x.FileModifyDatetime <= filter.EndDatetime.AddMinutes(1));

            //SystemName
            if (!string.IsNullOrEmpty(filter.SystemId))
            {
                queryable = queryable.Where(x => x.VoiceRecordProvidersId == Convert.ToInt32(filter.SystemId));
            }

            //************************************End Filter*************************************//
            var dtolst = await queryable.AsNoTracking().ToListAsync();
            var dto = await queryable.AsNoTracking().FirstOrDefaultAsync();
            if (dto is null) { return ResponseResult.Success(""); }
            //var VoiceDto = _mapper.Map<GetVoiceRecordDetail>(dto);

            //insert request url log
            string newUrl = InsertVoiceRecordURL(dto.Id, dto.FullPath);

            return ResponseResult.Success(newUrl);
        }

        public DateTime GetExpireDatetime(DateTime createDT, string typeString, int value)
        {
            DateTime nowDT = createDT;
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

        public string InsertVoiceRecordURL(int voiceRecDetId, string voiceRecDetUrl)
        {
            //Get timeout from configuration
            var configLst = _dBContext.VoiceRecordConfigurations.Where(x => x.ParameterName == "URLTimeout" || x.ParameterName == "WebDomainFormat").ToList();
            Guid guid = Guid.NewGuid();
            string encodeGuid = EncodingTo(guid.ToString());
            string newURL = configLst.Where(x => x.ParameterName == "WebDomainFormat").Select(x => x.ValueString).Single().Replace("{GUID}", encodeGuid);
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
                _dBContext.SaveChanges();
                //Mapping model model and dto
                var dto = _mapper.Map<GetVoiceRecordURLRequest>(voiceURL);
                return dto.ResponseURL;
            }
            catch (System.Exception ex)
            {
                //Write log
                _log.LogError($"Add VoiceRecordURL is error detail: {ex.Message}");
                //Return 
                return $"Add VoiceRecordURL error detail: {ex.Message}";
            }
        }

        public string EncodingTo(string toEncode)
        {
            byte[] bytes = Encoding.GetEncoding(28591).GetBytes(toEncode);
            string toReturn = System.Convert.ToBase64String(bytes);
            return toReturn;
        }

        public async Task<ServiceResponse<string>> GetVoiceRecordFileByGuid(string guidStr)
        {
            Guid guidTmp = Guid.Parse(guidStr);
            var queryable = await _dBContext.VoiceRecordURLRequests.Where(x => x.Id == guidTmp).AsNoTracking().FirstOrDefaultAsync();
            if (queryable is null) { return ResponseResult.Success<string>(""); }
            if (queryable.ExpireDatetime <= DateTime.Now)
            {

                return ResponseResult.Success("Expired");
            }
            else return ResponseResult.Success(queryable.VoiceRecordDetailURL);//
        }
    }
}