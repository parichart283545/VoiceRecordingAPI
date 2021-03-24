using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VoiceRecordAPI.Models
{
    [Table("CallType")]
    public class CallType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MaxLength(255)]
        public string Detail { get; set; }
        public bool IsActive { get; set; }
        [MaxLength(4096)]
        public string Remark { get; set; }
    }
}