using VoiceRecordAPI.Validations;

namespace VoiceRecordAPI.DTOs
{
    public class RoleDtoAdd
    {
        [FirstLetterUpperCase]
        public string RoleName { get; set; }
    }
}