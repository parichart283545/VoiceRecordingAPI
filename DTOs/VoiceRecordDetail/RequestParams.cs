using System;
using System.ComponentModel.DataAnnotations;

namespace VoiceRecordAPI.DTOs
{
    public class RequestParams
    {
        // [Required]
        // [StringLength(10)]
        public string DialNumber { get; set; }
        // [Required]
        // [StringLength(10)]
        public string DestinationNumber { get; set; }
        // [Required]
        // [StringLength(6)]
        public string CallTypeId { get; set; }
        // [Required]
        public DateTime RinggingDatetime { get; set; }
        // [Required]
        public DateTime ReceivedDatetime { get; set; }
        // [Required]
        public DateTime EndDatetime { get; set; }
        // [Required]
        // [StringLength(255)]
        public string ExtensionId { get; set; }
        // [Required]
        // [StringLength(6)]
        public string SystemId { get; set; }
    }
}