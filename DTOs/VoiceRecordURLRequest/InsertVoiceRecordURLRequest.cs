using System;
using VoiceRecordAPI.Models;

namespace VoiceRecordAPI.DTOs
{
    public class InsertVoiceRecordURLRequest
    {
        public Guid Id { get; set; }
        public VoiceRecordDetails voiceRecordDetail { get; set; }
        public int VoiceRecordDetailId { get; set; }
        public string VoiceRecordDetailURL { get; set; }
        public string ResponseURL { get; set; }
        public DateTime CreatedDatetime { get; set; }
        public DateTime ExpireDatetime { get; set; }
    }
}