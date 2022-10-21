
namespace AjaxExample.Models
{
    public class RandomNumbers
    {
        public string? Message { get; set; }
        public int Count { get; set; } = 100;
        public int Max { get; set; } = 1000;
        public IEnumerable<int> Domain { get; set; } = Enumerable.Empty<int>();
        public IEnumerable<int> Range { get; set; } = Enumerable.Empty<int>();

    }
}