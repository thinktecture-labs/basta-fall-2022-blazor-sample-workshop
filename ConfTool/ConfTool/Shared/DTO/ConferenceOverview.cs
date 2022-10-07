using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ConfTool.Shared.DTO
{
    [DataContract]
    public class ConferenceOverview
    {
        [DataMember(Order = 1)]
        public Guid ID { get; set; }
        [DataMember(Order = 2)]
        public string Title { get; set; }
    }
}
