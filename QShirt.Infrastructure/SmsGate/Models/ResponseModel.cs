using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace QShirt.Infrastructure.SmsGate.Models
{
    public class ResponseModel
    {
        [JsonPropertyName("messages")]
        public List<Message> Messages;

        [JsonPropertyName("request")]
        public Request Request;

        [JsonPropertyName("account")]
        public Account Account;
    }

    public class Message
    {
        [JsonPropertyName("object_type")]
        public string ObjectType;

        [JsonPropertyName("id")]
        public string Id;

        [JsonPropertyName("source_address")]
        public string SourceAddress;

        [JsonPropertyName("source_country")]
        public object SourceCountry;

        [JsonPropertyName("destination_address")]
        public string DestinationAddress;

        [JsonPropertyName("destination_country")]
        public string DestinationCountry;

        [JsonPropertyName("direction")]
        public string Direction;

        [JsonPropertyName("body")]
        public string Body;

        [JsonPropertyName("encoding")]
        public int Encoding;

        [JsonPropertyName("delivery_state")]
        public string DeliveryState;

        [JsonPropertyName("created")]
        public DateTime Created;

        [JsonPropertyName("updated")]
        public DateTime Updated;

        [JsonPropertyName("expires")]
        public DateTime Expires;

        [JsonPropertyName("cost_per_fragment")]
        public double CostPerFragment;

        [JsonPropertyName("fragment_count")]
        public int FragmentCount;

        [JsonPropertyName("total_cost")]
        public double TotalCost;

        [JsonPropertyName("custom_args")]
        public object CustomArgs;
    }

    public class Account
    {
        [JsonPropertyName("object_type")]
        public string ObjectType;

        [JsonPropertyName("account_id")]
        public int AccountId;

        [JsonPropertyName("balance")]
        public double Balance;

        [JsonPropertyName("auto_purchase")]
        public bool AutoPurchase;
    }

    public class Request
    {
        [JsonPropertyName("object_type")]
        public string ObjectType;

        [JsonPropertyName("id")]
        public string Id;

        [JsonPropertyName("sandbox")]
        public bool Sandbox;

        [JsonPropertyName("execution_time")]
        public double ExecutionTime;
    }
}
