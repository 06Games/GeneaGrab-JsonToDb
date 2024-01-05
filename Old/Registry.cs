using System.ComponentModel;
using GeneaGrab_JsonToDb.Old.Dates;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GeneaGrab_JsonToDb.Old
{
    /// <summary>Data on the registry</summary>
    public sealed class Registry : IEquatable<Registry>
    {
        public Registry() { }
        public string ProviderID { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] public string[] LocationDetails { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] public string Location { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] public string LocationID { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] public string District { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] public string DistrictID { get; set; }

        public string ID { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] public string CallNumber { get; set; }
        public string URL { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] public string ArkURL { get; set; }
        [JsonProperty(ItemConverterType = typeof(StringEnumConverter))] public IEnumerable<RegistryType> Types { get; set; } = Array.Empty<RegistryType>();

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] public string Title { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] public string Subtitle { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] public string Author { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] public string Notes { get; set; }


        /// <summary>Any additional information that might be needed</summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] public object Extra { get; set; }

        public Date From { get; set; }
        public Date To { get; set; }

        [JsonConverter(typeof(PagesConverter))] public RPage[] Pages { get; set; }


        public bool Equals(Registry other) => ID == other?.ID;
        public override bool Equals(object obj) => Equals(obj as Registry);
        public static bool operator ==(Registry one, Registry two) => one?.ID == two?.ID;
        public static bool operator !=(Registry one, Registry two) => !(one == two);
        public override int GetHashCode() => ID.GetHashCode();
    }
    public class DateFormatConverter : IsoDateTimeConverter
    {
        public DateFormatConverter(string format) => DateTimeFormat = format;
    }
    public class PagesConverter : JsonConverter<RPage[]>
    {
        public override RPage[] ReadJson(JsonReader reader, Type objectType, RPage[] existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            return serializer.Deserialize<Dictionary<int, RPage>>(reader)?.Select(kv =>
            {
                kv.Value.Number = kv.Key;
                return kv.Value;
            }).ToArray();
        }
        public override void WriteJson(JsonWriter writer, RPage[] value, JsonSerializer serializer) => serializer.Serialize(writer, value.ToDictionary(k => k.Number, v => v));
    }

    /// <summary>Data on the page of the registry</summary>
    public class RPage
    {
        [JsonIgnore] public int Number { get; set; }
        public override string ToString() => Number.ToString();
        /// <summary>Ark URL</summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)] public string URL { get; set; }
        /// <summary>Used internaly to download the image</summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)] public string DownloadURL { get; set; }

        public int Zoom { get; set; } = -1;
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore), DefaultValue(-1)] public int MaxZoom { get; set; } = -1;
        /// <summary>Total width of the image</summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)] public int Width { get; set; }
        /// <summary>Total height of the image</summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)] public int Height { get; set; }
        /// <summary>Tiles size (if applicable)</summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] public int? TileSize { get; set; }

        /// <summary>Notes about the page (user can edit this information)</summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] public string Notes { get; set; }
        /// <summary>Any additional information the grabber needs</summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] public object Extra { get; set; }
    }
}
