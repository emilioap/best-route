using BestRoute.Domain.Interfaces;
using BestRoute.Domain.Models;
using BestRoute.Domain.Models.Requests;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BestRoute.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly ISearchService _service;
        public SearchController(ISearchService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult> GetBestRoute([FromQuery] BestRouteRequest action)
        {
            var errors = new List<ValidationResult>();
            Validator.TryValidateObject(action, new ValidationContext(action, null, null), errors, true);

            if (errors.Count > 0) return BadRequest(errors.First());

            var result = await _service.GetBestRoute(action);

            return Ok(result);
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult> CreateRoute(BestRouteCreateRequest action)
        {
            var errors = new List<ValidationResult>();
            Validator.TryValidateObject(action, new ValidationContext(action, null, null), errors, true);

            if (errors.Count > 0) return BadRequest(errors.First());

            var result = await _service.CreateRoute(action);

            return result.Item1 == true
                ? Ok(result.Item2)
                : StatusCode(500);
        }
    }
}
