using AjaxExample.Models;

namespace AjaxExample.Services
{
    public interface INumbersService
    {
        RandomNumbers GetRandomNumbers(string message, int count, int max);
    }
}

