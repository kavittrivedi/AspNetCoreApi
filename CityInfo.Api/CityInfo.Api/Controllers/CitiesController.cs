using Microsoft.AspNetCore.Mvc;

namespace CityInfo.Api.Controllers
{
    [ApiController]
    public class CitiesController: ControllerBase
    {
        [HttpGet("api/cities")]
        public JsonResult GetCities() 
        {
           return new JsonResult(new List<object> { 
                new { id=1, Name= "Vadodara"},
                new { id=1, Name= "Indore"}
            });
        }
    }
}
