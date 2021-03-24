using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VoiceRecordAPI.Models
{
    [Table("VoiceRecordConfigurations")]
    public class VoiceRecordConfigurations
    {
        [Key]
        public string ParameterName { get; set; }
        public float ValueNumber { get; set; }
        public string ValueString { get; set; }
        public DateTime ValueDatetime { get; set; }
        public bool ValueBoolean { get; set; }
        public string Remark { get; set; }
    }
}