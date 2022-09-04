using System.Text.Json.Serialization;
namespace Micropub {
    public record Capabilities {
        public string MicropubEndpoint { get; set; }

        [JsonPropertyName ("media-endpoint")]
        public string MediaEndpoint { get; set; }

        [JsonPropertyName ("q")]
        public string[] Queries { get; set; }

        [JsonPropertyName ("syndicate-to")]
        public SyndicationTarget[] SyndicationTargets { get; set; }

        public record SyndicationTarget {
            [JsonPropertyName ("name")]
            public string Name { get; set; }

            [JsonPropertyName ("uid")]
            public string uid {
                get;
                set;
            }

            [JsonPropertyName ("network")]
            public SyndicationNetwork Network {
                get;
                set;
            }
        }

        public record SyndicationNetwork {
            [JsonPropertyName ("name")]
            public string Name { get; set; }

            [JsonPropertyName ("url")]
            public string Url { get; set; }

            [JsonPropertyName ("photo")]
            public string Photo { get; set; }
        }
    }

}