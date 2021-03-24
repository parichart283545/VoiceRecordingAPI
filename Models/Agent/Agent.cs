using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VoiceRecordAPI.Models
{
    [Table("Agent")]
    public class Agent
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(20)]
        public string EmployeeCode { get; set; }
        [MaxLength(20)]
        public string ExtensionNumber { get; set; }
        public bool IsActive { get; set; }
    }
}