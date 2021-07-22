using System;
using System.ComponentModel.DataAnnotations;

namespace VoiceRecordAPI.DTOs
{
    public class RequestParams
    {
        // [Required]
        [StringLength(13)]
        public string DialNumber { get; set; }
        // [Required]
        [StringLength(13)]
        public string DestinationNumber { get; set; }
        // [Required]
        [Range(0, 4)]
        public int? CallTypeId { get; set; }
        // [Required]
        public DateTime? RinggingDatetime { get; set; }
        // [Required]
        public DateTime? ReceivedDatetime { get; set; }
        // [Required]
        //public DateTime? EndDatetime { get; set; }
        // [Required]
        // [StringLength(255)]
        //public string ExtensionId { get; set; }
        // [Required]
        //[MaxLength(2)]
        [Range(0, 10)]
        public int? SystemId { get; set; }
    }
}