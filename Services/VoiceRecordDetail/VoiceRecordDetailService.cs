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

            var VoiceDto = _mapper.Map<GetVoiceRecordDetail>(queryable);

            string fileString = (VoiceDto.URLPath == null || VoiceDto.URLPath == string.Empty) ? VoiceDto.FullPath : VoiceDto.URLPath;
            //Testing
            if (!Directory.Exists(VoiceDto.FilePath))
                Directory.CreateDirectory(VoiceDto.FilePath);
            if (!File.Exists(VoiceDto.FullPath))
                File.Copy("D:\\Record\\[หญิง 0969426936]_1009-0853652569_202103191022(12345).wav", VoiceDto.FullPath);

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
            if (EndDT != default(DateTime))
            {
                queryable = queryable.Where(x => x.CreatedDate >= StartDT && x.CreatedDate <= EndDT);
            }
            //************************************End Filter*************************************//

            var dto = await queryable.AsNoTracking().ToListAsync();

            var VoiceDto = _mapper.Map<List<GetVoiceRecordDetail>>(dto);

            return ResponseResult.Success(VoiceDto);
        }

        public Task<ServiceResponse<string>> GetVoiceRecordURL(int? ExtensionNo, int? CallType, DateTime StartDT, DateTime EndDT)
        {
            throw new NotImplementedException();
        }
    }
}