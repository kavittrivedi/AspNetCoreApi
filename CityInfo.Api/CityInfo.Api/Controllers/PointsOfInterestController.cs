using CityInfo.Api.Models;
using CityInfo.Api.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;


namespace CityInfo.Api.Controllers
{
    [Route("api/cities/{cityId}/pointsofinterest")]
    [ApiController]
    public class PointsOfInterestController : ControllerBase
    {
        private readonly ILogger<PointsOfInterestController> _logger;
        private readonly ILocalMailService _localMailService;
        private readonly CitiesDataStore _citiesDataStore;
        public PointsOfInterestController(ILogger<PointsOfInterestController> logger,
            ILocalMailService localMailService, CitiesDataStore citiesDataStore)
        {
            _logger = logger ?? throw new ArgumentException(nameof(logger));
            _localMailService = localMailService ?? throw new ArgumentNullException(nameof(localMailService));
            _citiesDataStore = citiesDataStore ?? throw new ArgumentNullException(nameof(citiesDataStore));
        }

        [HttpGet]
        public ActionResult<IEnumerable<PointOfInterestDto>> GetPointsOfInterest(int cityId)
        {
            var city = _citiesDataStore.cities.FirstOrDefault(c => c.Id == cityId);
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
            try
            {
                //throw new Exception("Exception sample");
                var city = _citiesDataStore.cities.FirstOrDefault(c => c.Id == cityId);
                if (city == null)
                {
                    _logger.LogInformation($"City with id{cityId} wasn't found when accessing points of interest.");
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
            catch (Exception ex)
            {
                _logger.LogCritical(
                    $"Exception while getting points of interest for city with id {cityId}",
                    ex);

                return StatusCode(500, "A problem happened while handling your request.");
            }
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



            var city = _citiesDataStore.cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }

            // demo purposes - to be improved
            var maxPointOfInterestId = _citiesDataStore.cities.SelectMany(
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
            var city = _citiesDataStore.cities.FirstOrDefault(c => c.Id == cityId);

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

        [HttpPatch("{pointofinterestid}")]
        public ActionResult PartiallyUpdatePointOfInterest(
            int cityId, int pointOfInterestId,
            JsonPatchDocument<PointOfInterestUpdateDto> patchDocument)
        {
            var city = _citiesDataStore.cities
                .FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }

            var pointOfInterestFromDataStore = city.PointsOfInterest
                .FirstOrDefault(c => c.Id == pointOfInterestId);
            if (pointOfInterestFromDataStore == null)
            {
                return NotFound();
            }

            var pointOfInterestToPatch =
                new PointOfInterestUpdateDto()
                {
                    Name = pointOfInterestFromDataStore.Name,
                    Description = pointOfInterestFromDataStore.Description
                };

            patchDocument.ApplyTo(pointOfInterestToPatch, ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!TryValidateModel(pointOfInterestToPatch))
            {
                return BadRequest(ModelState);
            }

            pointOfInterestFromDataStore.Name = pointOfInterestToPatch.Name;
            pointOfInterestFromDataStore.Description = pointOfInterestToPatch.Description;

            return NoContent();
        }

        [HttpDelete("{pointOfInterestId}")]
        public ActionResult DeletePointOfInterest(int cityId, int pointOfInterestId)
        {
            var city = _citiesDataStore.cities
            .FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }

            var pointOfInterestFromStore = city.PointsOfInterest
                .FirstOrDefault(c => c.Id == pointOfInterestId);

            if (pointOfInterestFromStore == null)
            {
                return NotFound();
            }

            city.PointsOfInterest.Remove(pointOfInterestFromStore);
            _localMailService.Send("Point of interest deleted.",
                $"Point of interest {pointOfInterestFromStore} with id {pointOfInterestId} was deleted.");
            return NoContent();
        }
    }
}
