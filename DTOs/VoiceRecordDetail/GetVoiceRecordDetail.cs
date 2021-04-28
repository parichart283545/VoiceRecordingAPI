using System;

namespace VoiceRecordAPI.DTOs
{
    public class GetVoiceRecordDetail
    {
        public int Id { get; set; }
        public string ExtensionNo { get; set; }
        public string PhoneNumberFrom { get; set; }
        public string PhoneNumberTo { get; set; }
        public DateTime DatetimeFileName { get; set; }
        public DateTime FileCreateDatetime { get; set; }
        public DateTime FileModifyDatetime { get; set; }
        public string FileName { get; set; }
        //public string FilePath { get; set; }
        public string FullPath { get; set; }
        public string URLPath { get; set; }
        public DateTime CreatedDate { get; set; }
        public int VoiceRecordProvidersId { get; set; }
        public int CallTypeId { get; set; }

    }
}