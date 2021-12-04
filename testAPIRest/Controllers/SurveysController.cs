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
    public class SurveysController : ControllerBase
    {
        private readonly ISurveysProvider surveysProvider;
        public SurveysController(ISurveysProvider surveysProvider)
        {
            this.surveysProvider = surveysProvider;
        }

        [HttpPost("Survey/{ActivityId}")]
        public async Task<IActionResult> InsertPropertyAsync(int ActivityId,[FromBody] Survey survey)
        {
            if (survey == null)
            {
                return BadRequest();
            }
            try
            {
                var result = await surveysProvider.CreateSurveyAsync(ActivityId, survey);
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
