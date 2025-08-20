using Newtonsoft.Json;
using System.Collections.Generic;

namespace QShirt.Infrastructure.SmsGate.Models
{
    public class SmsModel
    {
        public SmsModel()
        {
            Messages = new HashSet<Message>();
        }

        [JsonProperty(PropertyName = "messages")]
        public ICollection<Message> Messages { get; set; }

        public class Message
        {
            [JsonProperty(PropertyName = "source_address")]
            public string SourceAddress;

            [JsonProperty(PropertyName = "destination_address")]
            public string DestinationAddress;

            [JsonProperty(PropertyName = "content")]
            public string Content;
        }
    }
}
