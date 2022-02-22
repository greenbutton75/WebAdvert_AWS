using AdvertAPI.Models;
using AdvertAPI.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace AdvertAPI.Controllers
{
    [ApiController]
    [Route("adverts/v1")]
    [Produces("application/json")]
    public class AdvertAPIController : ControllerBase
    {

        private readonly ILogger<AdvertAPIController> _logger;
        private readonly IAdvertStorageService _storage;

        public AdvertAPIController(ILogger<AdvertAPIController> logger, IAdvertStorageService storage)
        {
            _logger = logger;
            _storage = storage;
        }

        [HttpGet]
        [Route("{id}", Name = "Get")]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Get(string id)
        {
            _logger.LogInformation("Get");
            try
            {
                var advert = await _storage.GetByIdAsync(id);
                return new JsonResult(advert);
            }
            catch (KeyNotFoundException)
            {
                return new NotFoundResult();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                return new StatusCodeResult(500);
            }
        }


        [HttpGet]
        [Route("all")]
        [ProducesResponseType(200)]
        [EnableCors("AllOrigin")]
        public async Task<IActionResult> All()
        {
            _logger.LogInformation("All");
            return new JsonResult(await _storage.GetAllAsync());
        }
        
        [HttpPost]
        [Route("Create")]
        [ProducesResponseType(404)]
        [ProducesResponseType(201)]
        public async Task<IActionResult> Create(AdvertModel model)
        {
            _logger.LogInformation("Create");
            string recordId;
            try
            {
                recordId = await _storage.AddAsync(model);
            }
            catch (KeyNotFoundException)
            {
                return new NotFoundResult();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                return StatusCode(500, exception.Message);
            }

            return CreatedAtRoute(nameof(Get), new { id = recordId }, model);
        }

        [HttpPut]
        [Route("Confirm")]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Confirm(ConfirmAdvertModel model)
        {
            _logger.LogInformation("Confirm");
            try
            {
                await _storage.ConfirmAsync(model);
                //await RaiseAdvertConfirmedMessage(model);
            }
            catch (KeyNotFoundException)
            {
                return new NotFoundResult();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                return StatusCode(500, exception.Message);
            }

            return new OkResult();
        }
    }
}