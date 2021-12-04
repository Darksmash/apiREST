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
    public class PropertiesController : ControllerBase
    {
        private readonly IPropertiesProvider propertiesProvider;

        public PropertiesController(IPropertiesProvider propertiesProvider)
        {
            this.propertiesProvider = propertiesProvider;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPropertiesAsync()
        {
            try
            {
                var result = await propertiesProvider.GetAllPropertiesAsync();

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
        public async Task<IActionResult> GetPropertyAsync(int id)
        {
            try
            {
                var result = await propertiesProvider.GetPropertyAsync(id);

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

        [HttpPost("Property")]
        public async Task<IActionResult> InsertPropertyAsync([FromBody] Property property)
        {
            if (property == null)
            {
                return BadRequest();
            }

            try
            {
                var result = await propertiesProvider.InsertPropertyAsync(property);
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

        [HttpPut("Property/{id}")]
        public async Task<IActionResult> UpdatePropertyAsync(int id,[FromBody] Property property)
        {
            if (property == null)
            {
                return BadRequest();
            }

            try
            {
                var result = await propertiesProvider.UpdatePropertyAsync(id,property);
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

        [HttpDelete("Property/{id}")]
        public async Task<IActionResult> DeletePropertyAsync(int id)
        {
            try
            {
                var result = await propertiesProvider.DeletePropertyAsync(id);
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
