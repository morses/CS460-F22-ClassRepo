using System;

namespace AsyncDemo.Models
{
    public class Earthquake
    {
        public double Magnitude { get; set; }
        public string Location { get; set; } = String.Empty;
        public long ETime { get; set; }
    }
}