using Dapper;
using Newtonsoft.Json;
using PruebaApiREST.Configurations;
using PruebaApiREST.Models;
using System;
using System.Threading.Tasks;

namespace PruebaApiREST.DAL
{
    public class SurveysProvider : ISurveysProvider
    {
        private PostgreSQLConfiguration _connectionString;
        public SurveysProvider(PostgreSQLConfiguration connectionString)
        {
            _connectionString = connectionString;
        }

        protected Npgsql.NpgsqlConnection dbConnection()
        {
            return new Npgsql.NpgsqlConnection(_connectionString.ConnectionString);
        }

        public async Task<bool> CreateSurveyAsync(int ActivityId, Survey survey)
        {
            try
            {
                var db = dbConnection();
                var date = DateTime.Now;
                

                var json = JsonConvert.SerializeObject(survey);

                var sqlQuery = $"INSERT INTO public.\"Survey\" (activityid,answers,createdat) VALUES ('{ActivityId}','{json}','{date.Year}-{date.Month}-{date.Day}')";
                
                var result = await db.ExecuteAsync(sqlQuery, new { });
                return result > 0;

            }
            catch
            {
                return false;
            }
        }
    }
}
