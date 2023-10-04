using CityInfo.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.Api.Controllers
{
    [ApiController]
    public class CitiesController : ControllerBase
    {
        private readonly CitiesDataStore _citiesDataStore;
        public CitiesController(CitiesDataStore citiesDataStore)
        {
            _citiesDataStore = citiesDataStore ?? throw new ArgumentNullException(nameof(citiesDataStore));
        }
        [HttpGet("api/cities")]
        public ActionResult<IEnumerable<CityDto>> GetCities()
        {
            return Ok(_citiesDataStore.cities);
        }

        [HttpGet("api/GetCity/{id}")]
        public ActionResult<CityDto> GetCity(int id)
        {
            var cityToReturn = new JsonResult(_citiesDataStore.cities.FirstOrDefault(c => c.Id == id));
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
