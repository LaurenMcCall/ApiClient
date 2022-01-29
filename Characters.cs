using System.Text.Json.Serialization;
using System;

namespace ApiClient
{
    public class Characters
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("gender")]
        public string Gender { get; set; }

        [JsonPropertyName("culture")]
        public string Culture { get; set; }

        [JsonPropertyName("born")]
        public string Born { get; set; }

        [JsonPropertyName("died")]
        public string Died { get; set; }

        [JsonPropertyName("isAlive")]
        public bool IsAlive { get; set; }
    }
}
