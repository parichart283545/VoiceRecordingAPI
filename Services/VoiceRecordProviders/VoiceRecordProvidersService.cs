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
    public class VoiceRecordProvidersService : ServiceBase, IVoiceRecordProvidersService
    {
        private readonly AppDBContext _dBContext;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContext;
        private readonly ILogger<VoiceRecordProvidersService> _log;
        public VoiceRecordProvidersService(AppDBContext dBContext,
                                    IMapper mapper,
                                    IHttpContextAccessor httpContext,
                                    ILogger<VoiceRecordProvidersService> log) : base(dBContext, mapper, httpContext)
        {
            _httpContext = httpContext;
            _dBContext = dBContext;
            _mapper = mapper;
            _log = log;
        }
        public async Task<ServiceResponse<List<GetVoiceRecordProviders>>> GetVoiceRecordProviderList()
        {
            //Get data from model
            List<VoiceRecordProviders> typesDb = await _dBContext.VoiceRecordProviders.AsNoTracking().ToListAsync();
            //Mapping
            List<GetVoiceRecordProviders> typeDtos = _mapper.Map<List<GetVoiceRecordProviders>>(typesDb);
            //Return result with ResponseResultl format
            return ResponseResult.Success(typeDtos);
        }



    }
}