using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ConfTool.Shared.DTO
{
    [DataContract]
    public class ConferenceRequestModel
    {
        [DataMember(Order = 1)]
        public Guid Id { get; set; }
    }
}
