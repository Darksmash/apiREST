using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PruebaApiREST.DAL;
using PruebaApiREST.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PruebaApiREST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivitiesController : ControllerBase
    {
        private readonly IActivitiesProvider activitiesProvider;

        public ActivitiesController(IActivitiesProvider activitiesProvider)
        {
            this.activitiesProvider = activitiesProvider;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllActivitiesAsync()
        {
            try
            {
                var result = await activitiesProvider.GetAllActivitiesAsync();

                if (result != null && result.Any())
                {

                    return Ok(result);
                }

                return NotFound();
            }
            catch
            {
                return StatusCode(503);
            }
        }

        [HttpGet("List")]
        public async Task<IActionResult> GetActivitiesList()
        {
            try
            {
                var result = await activitiesProvider.GetActivitiesList();

                if (result != null && result.Any())
                {

                    return Ok(result);
                }

                return NotFound();
            }
            catch
            {
                return StatusCode(503);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetActivityAsync(int id)
        {
            try
            {
                var result = await activitiesProvider.GetActivityAsync(id);

                if (result != null)
                {

                    return Ok(result);
                }

                return NotFound();
            }
            catch
            {
                return StatusCode(503);
            }
        }

        [HttpPost("Filter")]
        public async Task<IActionResult> GetActivitiesByDateAndStatusAsync([FromBody]Search search)
        {
            try
            {
                var result = await activitiesProvider.GetActivitiesByDateAndStatusAsync(search);

                if (result != null)
                {

                    return Ok(result);
                }

                return NotFound();
            }
            catch
            {
                return StatusCode(503);
            }
        }

        [HttpPost("Activity")]
        public async Task<IActionResult> InsertPropertyAsync([FromBody] Activity activity)
        {
            if (activity == null)
            {
                return BadRequest();
            }

            try
            {
                var result = await activitiesProvider.InsertActivityAsync(activity);
                if (result)
                {
                    return Ok(result);
                }
                return NotFound();
            }
            catch
            {
                return StatusCode(503);
            }
        
        }

        [HttpPut("RescheduleActivity/{id}")]
        public async Task<IActionResult> RescheduleActivityAsync(int id,[FromBody] Activity activity)
        {
            if (activity == null)
            {
                return BadRequest();
            }

            try
            {
                var result = await activitiesProvider.RescheduleActivityAsync(id,activity);
                if (result)
                {
                    return Ok(result);
                }
                return BadRequest();
            }
            catch
            {
                return StatusCode(503);
            }

        }

        [HttpPut("CancelActivity/{id}")]
        public async Task<IActionResult> CancelActivityAsync(int id, [FromBody] Activity activity)
        {
            if (activity == null)
            {
                return BadRequest();
            }

            try
            {
                var result = await activitiesProvider.CancelActivityAsync(id, activity);
                if (result)
                {
                    return Ok(result);
                }
                return NotFound();
            }
            catch
            {
                return StatusCode(503);
            }

        }

        [HttpPut("DoneActivity/{id}")]
        public async Task<IActionResult> DoneActivity(int id)
        {
            try
            {
                var result = await activitiesProvider.DoneActivity(id);
                if (result)
                {
                    return Ok(result);
                }
                return NotFound();
            }
            catch
            {
                return StatusCode(503);
            }
        }

        [HttpDelete("Activity/{id}")]
        public async Task<IActionResult> DeleteActivityAsync(int id)
        {
            try
            {
                var result = await activitiesProvider.DeleteActivityAsync(id);
                if (result)
                {
                    return Ok(result);
                }
                return NotFound();
            }
            catch
            {
                return StatusCode(503);
            }

        }

    }
}
