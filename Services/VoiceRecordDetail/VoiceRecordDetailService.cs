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

            return ResponseResult.Success(fileString);
        }

        public async Task<ServiceResponse<List<GetVoiceRecordDetail>>> GetVoiceRecordURLParam(int? ExtensionNo, int? CallType, DateTime StartDT, DateTime EndDT)
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
            if (!string.IsNullOrEmpty(filter.dialNumber))
            {
                if (!int.TryParse(filter.dialNumber, out int n)) { return ResponseResult.Failure<string>("DialNumber is not numeric"); }
                queryable = queryable.Where(x => x.PhoneNumberFrom == filter.dialNumber);
            }
            //DestinationNumber
            if (!string.IsNullOrEmpty(filter.destinationNumber))
            {
                if (!int.TryParse(filter.destinationNumber, out int n1)) { return ResponseResult.Failure<string>("DestinationNumber is not numeric"); }
                queryable = queryable.Where(x => x.PhoneNumberTo == filter.destinationNumber);
            }
            //ExtensionId
            if (!string.IsNullOrEmpty(filter.ExtensionId))
            {
                if (!int.TryParse(filter.ExtensionId, out int n2)) { return ResponseResult.Failure<string>("ExtensionId is not numeric"); }
                queryable = queryable.Where(x => x.ExtensionNo == Convert.ToInt32(filter.ExtensionId));
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
            queryable = queryable.Where(x => x.FileCreateDatetime >= filter.receivedDatetime.AddMinutes(-1) && x.FileCreateDatetime <= filter.receivedDatetime.AddMinutes(1));
            //endDatetime
            queryable = queryable.Where(x => x.FileModifyDatetime >= filter.endDatetime.AddMinutes(-1) && x.FileModifyDatetime <= filter.endDatetime.AddMinutes(1));

            //SystemName
            if (!string.IsNullOrEmpty(filter.systemId))
            {
                queryable = queryable.Where(x => x.VoiceRecordProvidersId == Convert.ToInt32(filter.systemId));
            }

            //************************************End Filter*************************************//
            //var dtolst = await queryable.AsNoTracking().ToListAsync();
            var dto = await queryable.AsNoTracking().FirstOrDefaultAsync();
            if (dto is null) { return ResponseResult.Success(""); }
            var VoiceDto = _mapper.Map<GetVoiceRecordDetail>(dto);
            return ResponseResult.Success(dto.URLPath);
        }
    }
}