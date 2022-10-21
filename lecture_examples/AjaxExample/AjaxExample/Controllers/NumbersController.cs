using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using AjaxExample.Services;
using AjaxExample.Models;

namespace AjaxExample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NumbersController : ControllerBase
    {
        private readonly INumbersService _numbersService;
        public NumbersController(INumbersService numbersService)
        {
            _numbersService = numbersService;
        }

        [HttpGet]
        public RandomNumbers RandomNumbers(int count = 100)
        {
            RandomNumbers nums = _numbersService.GetRandomNumbers("Random Numbers API", count, 1000);
            return nums;
        }
    }
}
