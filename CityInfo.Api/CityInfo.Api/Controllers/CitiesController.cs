using CityInfo.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.Api.Controllers
{
    [ApiController]
    public class CitiesController : ControllerBase
    {
        [HttpGet("api/cities")]
        public ActionResult<IEnumerable<CityDto>> GetCities()
        {
            return Ok(CitiesDataStore.Current.cities);
        }

        [HttpGet("api/GetCity/{id}")]
        public ActionResult<CityDto> GetCity(int id)
        {
            var cityToReturn = new JsonResult(CitiesDataStore.Current.cities.FirstOrDefault(c => c.Id == id));
            if (cityToReturn.Value == null)
            { 
                return NotFound();
            }

            return Ok(cityToReturn);
        }
    }

    //[ApiController]
    //[Route("api/cities")]
    //public class CitiesController1 : ControllerBase
    //{
    //    [HttpGet]
    //    public JsonResult GetCities()
    //    {
    //        return new JsonResult(CitiesDataStore.Current.cities);
    //    }

    //    [HttpGet("{id}")]
    //    public JsonResult GetCity(int id)
    //    {
    //        return new JsonResult(CitiesDataStore.Current.cities.FirstOrDefault(c => c.Id == id));
    //    }
    //}
}
