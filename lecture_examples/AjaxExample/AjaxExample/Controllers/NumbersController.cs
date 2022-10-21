using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using AjaxExample.Services;
using AjaxExample.Models;
using System.Net;

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

        [HttpGet("list")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<RandomNumbers> RandomNumbers(int count = 100)
        {
            RandomNumbers nums;
            try
            {
                nums = _numbersService.GetRandomNumbers("Random Numbers API", count, 1000);
            }
            catch(ArgumentOutOfRangeException e)
            {
                return BadRequest(new {message = e.Message});
            }
            return nums;
        }
    }
}
