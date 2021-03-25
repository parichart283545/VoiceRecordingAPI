using System.Collections.Generic;
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
    public class CallTypeService : ServiceBase, ICallTypeService
    {
        private readonly AppDBContext _dBContext;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContext;
        private readonly ILogger<CallTypeService> _log;
        public CallTypeService(AppDBContext dBContext,
                            IMapper mapper,
                            IHttpContextAccessor httpContext,
                            ILogger<CallTypeService> log) : base(dBContext, mapper, httpContext)
        {
            _httpContext = httpContext;
            _dBContext = dBContext;
            _mapper = mapper;
            _log = log;
        }
        public async Task<ServiceResponse<List<GetCallTypes>>> GetCallTypeList()
        {
            //Get data from model
            List<CallType> typesDb = await _dBContext.CallTypes.AsNoTracking().ToListAsync();
            //Mapping
            List<GetCallTypes> typeDtos = _mapper.Map<List<GetCallTypes>>(typesDb);
            //Return result with ResponseResultl format
            return ResponseResult.Success(typeDtos);
        }
    }
}