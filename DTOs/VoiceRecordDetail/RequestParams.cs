using System;
using System.ComponentModel.DataAnnotations;

namespace VoiceRecordAPI.DTOs
{
    public class RequestParams
    {
        // [Required]
        // [StringLength(10)]
        public string dialNumber { get; set; }
        // [Required]
        // [StringLength(10)]
        public string destinationNumber { get; set; }
        // [Required]
        // [StringLength(6)]
        public string CallTypeId { get; set; }
        // [Required]
        public DateTime ringgingDatetime { get; set; }
        // [Required]
        public DateTime receivedDatetime { get; set; }
        // [Required]
        public DateTime endDatetime { get; set; }
        // [Required]
        // [StringLength(255)]
        public string ExtensionId { get; set; }
        // [Required]
        // [StringLength(6)]
        public string systemId { get; set; }
    }
}