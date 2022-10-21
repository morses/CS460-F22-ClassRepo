using AjaxExample.Models;

namespace AjaxExample.Services
{
    public class NumbersService : INumbersService
    {
        public RandomNumbers GetRandomNumbers(string message, int count, int max)
        {
            if (count < 1 || max < 0)
            {
                throw new ArgumentOutOfRangeException("Parameter count must be greater than 0, or max must not be negative");
            }
            
            Random generator = new Random();
            return new RandomNumbers
            {
                Message = message,
                Count = count,
                Max = max,
                Domain = Enumerable.Range(1, count),
                Range = Enumerable.Range(1, count).Select(x => generator.Next(max))
            };
            
        }
    }
}
