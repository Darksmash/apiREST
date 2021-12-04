using PruebaApiREST.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PruebaApiREST.DAL
{
    public interface ISurveysProvider
    {
        Task<bool> CreateSurveyAsync (int ActivityId, Survey survey);
    }
}
