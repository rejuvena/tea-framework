using System.Collections.Generic;
using Newtonsoft.Json;

namespace TeaFramework
{
    public class TeaMetadata
    {
        [JsonProperty("displayNames")] public Dictionary<string, string> DisplayNames { get; } = new();
    }
}