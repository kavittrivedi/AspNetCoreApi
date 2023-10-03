using CityInfo.Api.Models;
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
        [HttpGet("{pointOfInterestId}", Name = "GetPointofInterest")]
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

        [HttpPost]
        public ActionResult<PointOfInterestDto> CreatePointOfInterest(
            int cityId,
            [FromBody] PointOfInterestForCreationDto pointOfInterest) // pointOfInterest is complete data type // [FromBody] is not compulsary, it is assumed that content will come from body.
        {
            // Not needed because of [ApiController]
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest();
            //}



            var city = CitiesDataStore.Current.cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }

            // demo purposes - to be improved
            var maxPointOfInterestId = CitiesDataStore.Current.cities.SelectMany(
                                        c => c.PointsOfInterest).Max(p => p.Id);

            var finalPointOfInterest = new PointOfInterestDto()
            {
                Id = ++maxPointOfInterestId,
                Name = pointOfInterest.Name,
                Description = pointOfInterest.Description,
            };

            city.PointsOfInterest.Add(finalPointOfInterest);

            return CreatedAtRoute("GetPointofInterest",
                                  new
                                  {
                                      CityId = cityId,
                                      pointOfInterestId = finalPointOfInterest.Id,
                                  },
                                  finalPointOfInterest
                                );

        }

        [HttpPut("{pointOfInteresetId}")] // For full update we use HttpPut attribute.
        public ActionResult UpdatePointOfInterest(int cityId, int pointOfInteresetId,
          [FromBody] PointOfInterestUpdateDto pointOfInterest)
        {
            var city = CitiesDataStore.Current.cities.FirstOrDefault(c => c.Id == cityId);

            if (city == null)
            {
                return NotFound();
            }

            var pointOfInterestFromDataStore = city.PointsOfInterest.FirstOrDefault(c => c.Id == pointOfInteresetId);
            
            if (pointOfInterest == null)
            {
                return NotFound();
            }

            pointOfInterestFromDataStore.Name = pointOfInterest.Name;
            pointOfInterestFromDataStore.Description = pointOfInterest.Description;

            return NoContent();
        }
    }
}
