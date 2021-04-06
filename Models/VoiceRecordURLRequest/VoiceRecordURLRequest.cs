using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VoiceRecordAPI.Models
{
    [Table("VoiceRecordURLRequest")]
    public class VoiceRecordURLRequest
    {
        [Key]
        public Guid Id { get; set; }
        public VoiceRecordDetails voiceRecordDetail { get; set; }
        public int VoiceRecordDetailId { get; set; }
        [MaxLength(4096)]
        public string VoiceRecordDetailURL { get; set; }
        [MaxLength(4096)]
        public string ResponseURL { get; set; }
        public DateTime CreatedDatetime { get; set; }
        public DateTime ExpireDatetime { get; set; }

    }
}