using System;
using System.ComponentModel.DataAnnotations;

namespace VoiceRecordAPI.DTOs
{
    public class FilterVoiceRecordDetail : PaginationDto
    {
        public int? ExtensionNo { get; set; }// null is all
        [Required]
        public DateTime StartDate { get; set; } //datetime start of create file
        [Required]
        public DateTime EndDate { get; set; } //datetime end of create file
        public int? CallingType { get; set; } // null for all, 1 is Call-In, 2 is Call-Out, 3 is unknow
        public string OrderingField { get; set; } //orderby
        public bool AscendingOrder { get; set; } = true;//false is asc
    }
}