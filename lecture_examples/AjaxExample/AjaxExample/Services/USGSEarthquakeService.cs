using AjaxExample.Models;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using AjaxExample.Models.DTO;
using System.Text.Json;
using Newtonsoft.Json;

namespace AjaxExample.Services
{
    public class USGSEarthquakeService : IEarthquakeService
    {
        public string BaseSource { get; }

        public USGSEarthquakeService()
        {
            BaseSource = "https://earthquake.usgs.gov/earthquakes/feed/v1.0/summary/1.0_{0}.geojson";
        }

        // This version follows the built-in JSON deserialization example:
        //   https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/how-to?pivots=dotnet-6-0#how-to-read-json-as-net-objects-deserialize
        public IEnumerable<Earthquake> GetRecentEarthquakes(EarthquakeTimeRange range)
        {
            // Make request and get back JSON as a string
            string source = string.Format(BaseSource, Earthquake.TimeRanges[range]);
            string jsonResponse = GetJsonStringFromEndpoint(source) ?? "";
            Debug.WriteLine(jsonResponse);

            // Deserialize the string into POCO's (that we wrote)
            GeoJsonDTO? geoJsonDTO = null;
            try
            {
                geoJsonDTO = System.Text.Json.JsonSerializer.Deserialize<GeoJsonDTO>(jsonResponse);
            }
            catch(System.Text.Json.JsonException)
            {
                // Log it, figure out how to handle
                geoJsonDTO = null;
            }
            
            if(geoJsonDTO != null)
            {
                return geoJsonDTO.features.Select(f => new Earthquake(f.properties.mag, f.properties.place));
            }

            return Enumerable.Empty<Earthquake>();
        }

        // This version follows the Newtonsoft.JSON version using LINQ to JSON
        // Need to install Newtonsoft.JSON (Json.NET)
        public IEnumerable<Earthquake> GetRecentEarthquakes2(EarthquakeTimeRange range)
        {
            // Make request and get back JSON as a string
            string source = string.Format(BaseSource, Earthquake.TimeRanges[range]);
            string jsonResponse = GetJsonStringFromEndpoint(source) ?? "";
            Debug.WriteLine(jsonResponse);

            // Deserialize the string and pull out what we want
            JObject? geo = null;
            try
            {
                geo = JObject.Parse(jsonResponse);
            }
            catch(JsonReaderException)
            {
                // jsonResponse is not valid JSON
                geo = null;
            }
            
            if(geo != null)
            {
                // At this point we know that we have valid JSON, but it may not include what we're after
                IEnumerable<Earthquake> output = geo["features"].Select(f => new Earthquake(f["properties"]["mag"].Value<double>(), f["properties"]["place"].Value<string>()));
                return output;
            }
            else
            {
                return Enumerable.Empty<Earthquake>();
            }
        }

        // This is a singleton, we are only supposed to have one per application
        // Better to use IHttpClientFactory with dependency injection but that's too much for this simple example
        // Do note that if another service needs to make HTTP requests they need to use this one, so it would be better
        // to put it someplace else where it can be shared easier
        public static readonly HttpClient _httpClient = new HttpClient();

        public string? GetJsonStringFromEndpoint(string uri)
        {
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri)
            {
                Headers =
                    {
                        { "Accept", "application/json" }
                    }
            };
            HttpResponseMessage response = _httpClient.Send(httpRequestMessage);
            // This is only a minimal version; make sure to cover all your bases here
            if (response.IsSuccessStatusCode)
            {
                // Note there is only an async version of this so to avoid forcing you to use all async I'm waiting for the result manually
                string responseText = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                return responseText;
            }
            else
            {
                // What to do if failure? 401? Should throw and catch specific exceptions that explain what happened
                return null;
            }
        }
    }
}
