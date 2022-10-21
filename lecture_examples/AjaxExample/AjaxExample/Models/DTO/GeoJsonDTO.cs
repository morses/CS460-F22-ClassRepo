using Microsoft.AspNetCore.Authentication;

namespace AjaxExample.Models.DTO
{
#nullable disable
    public class GeoJsonDTO
    {
        public string Type { get; set; }
        public MetaData Metadata { get; set; }
        public List<Features> Features { get; set; }
    }

    public class MetaData
    {
        public string Url { get; set; }
        public string Title { get; set; }
        public int Status { get; set; }
        public int Count { get; set; }
    }

    public class Features
    {
        public Properties Properties { get; set; }
        // public Geometry Geometry {get; set;}     // not pulling this one out at the moment
    }

    public class Properties
    {
        public int Mag { get; set; }
        public string Place { get; set; }
    }
}
