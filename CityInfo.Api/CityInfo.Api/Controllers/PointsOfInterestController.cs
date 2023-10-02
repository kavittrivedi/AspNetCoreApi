using CityInfo.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.Api.Controllers
{
    [Route("api/cities/{cityId}/pointsofinterest")]
    [ApiController]
    public class PointsOfInterestController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<PointOfInterestDto>> GetPointsOfInterest(int cityId)
        {
            var city = CitiesDataStore.Current.cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }

            return Ok(city.PointsOfInterest);
        }

        /// <summary>
        /// This end point is use to get point of interest based on city and point of interest id.
        /// </summary>
        /// <param name="cityId">The city id.</param>
        /// <param name="pointOfInterestId">The point of interest id.</param>
        /// <returns>Respective point of interest or proper error code.</returns>
        [HttpGet("{pointOfInterestId}")]
        public ActionResult<PointOfInterestDto> GetPointOfInterest(
            int cityId, int pointOfInterestId)
        {
            var city = CitiesDataStore.Current.cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            { 
                return NotFound();
            }

            // Find point of interest
            var pointOfInterest = city.PointsOfInterest
            .FirstOrDefault(c => c.Id == pointOfInterestId);
            if (pointOfInterest == null)
            {
                return NotFound();
            }

            return Ok(pointOfInterest);
        }
    }
}
