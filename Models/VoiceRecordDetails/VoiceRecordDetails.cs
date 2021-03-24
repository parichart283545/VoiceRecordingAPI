using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VoiceRecordAPI.Models
{
    [Table("VoiceRecordDetails")]
    public class VoiceRecordDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int ExtensionNo { get; set; }
        public string PhoneNumberFrom { get; set; }
        public string PhoneNumberTo { get; set; }
        public DateTime DatetimeFileName { get; set; }
        public DateTime FileCreateDatetime { get; set; }
        public DateTime FileModifyDatetime { get; set; }
        [MaxLength(4096)]
        public string FileName { get; set; }
        [MaxLength(4096)]
        public string FilePath { get; set; }
        [MaxLength(4096)]
        public string FullPath { get; set; }
        [MaxLength(4096)]
        public string URLPath { get; set; }
        public DateTime CreatedDate { get; set; }
        //[ForeignKey("VoiceRecordProviders")]
        public int VoiceRecordProvidersId { get; set; }
        public VoiceRecordProviders VoiceRecordProvider { get; set; }

        //[ForeignKey("CallType")]
        public int CallTypeId { get; set; }
        public CallType CallTypes { get; set; }


    }
}