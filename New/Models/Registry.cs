#nullable enable
using GeneaGrab_JsonToDb.New.Models.Dates;
using GeneaGrab_JsonToDb.Old;
using Microsoft.EntityFrameworkCore;

namespace GeneaGrab_JsonToDb.New.Models
{
    /// <summary>Data on the registry</summary>
    [PrimaryKey(nameof(ProviderId), nameof(Id))]
    public sealed class Registry
    {
        public Registry(string providerId, string id)
        {
            ProviderId = providerId;
            Id = id;
        }

        public string ProviderId { get; init; }

        public string Id { get; init; }

        /// <summary>Call number of the document (if applicable)</summary>
        public string? CallNumber { get; set; }
        public string? URL { get; set; }
        public string? ArkURL { get; set; }
        public IList<RegistryType> Types { get; set; } = new List<RegistryType>();

        public string? Title { get; set; }
        public string? Subtitle { get; set; }
        public string? Author { get; set; }
        public IList<string> Location { get; set; } = new List<string>();
        public string? Notes { get; set; }


        /// <summary>Any additional information that might be needed</summary>
        public object? Extra { get; set; }

        public Date? From { get; set; }
        public Date? To { get; set; }
        public IEnumerable<Frame> Frames { get; set; } = new List<Frame>();
    }
}
