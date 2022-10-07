using System.Runtime.Serialization;

namespace ConfTool.Shared.DTO
{
    [DataContract]
    public class ConferenceResult
    {
        [DataMember(Order = 1)]
        public bool Successfull { get; set; }
        [DataMember(Order = 2)]
        public string Error { get; set; }
        [DataMember(Order = 3)]
        public ConferenceDetail Conference { get; set; }
    }
}
