using Microsoft.AspNetCore.Authentication;

namespace AjaxExample.Models.DTO
{
#nullable disable
    public class GeoJsonDTO
    {
        public string type { get; set; }
        public MetaData metadata { get; set; }
        public List<Features> features { get; set; }
    }

    public class MetaData
    {
        public string url { get; set; }
        public string title { get; set; }
        public int status { get; set; }
        public int count { get; set; }
    }

    public class Features
    {
        public Properties properties { get; set; }
        // public Geometry geometry {get; set;}     // not pulling this one out at the moment
    }

    public class Properties
    {
        public double mag { get; set; }
        public string place { get; set; }
    }
}
