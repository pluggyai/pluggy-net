using Newtonsoft.Json;

namespace Pluggy.SDK.Model
{
    /// <summary>Geographic coordinates in decimal degrees, WGS84 reference system.</summary>
    public class GeographicCoordinates
    {
        /// <summary>Between -90 and 90.</summary>
        [JsonProperty("latitude")]
        public double? Latitude { get; set; }

        /// <summary>Between -180 and 180.</summary>
        [JsonProperty("longitude")]
        public double? Longitude { get; set; }
    }
}
