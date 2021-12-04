using Dapper;
using PruebaApiREST.Configurations;
using PruebaApiREST.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PruebaApiREST.DAL
{
    public class ActivitiesProvider : IActivitiesProvider
    {
        private PostgreSQLConfiguration _connectionString;

        public ActivitiesProvider(PostgreSQLConfiguration connectionString)
        {
            _connectionString = connectionString;
        }
        protected Npgsql.NpgsqlConnection dbConnection()
        {
            return new Npgsql.NpgsqlConnection(_connectionString.ConnectionString);
        }

        public async Task<IEnumerable<Activity>> GetAllActivitiesAsync()
        {
            var db = dbConnection();
            var sql = $@"SELECT * FROM public.""Activity""";
            var result = await db.QueryAsync<Activity>(sql, null);
            return result;
        }

        public async Task<bool> DeleteActivityAsync(int id)
        {
            var db = dbConnection();
            var sql = $@"
                            DELETE FROM public.""Activity"" 
                            WHERE id = @Id
            ";
            var result = await db.ExecuteAsync(sql, new
            {
                Id = id
            });
            return result > 0;
        }

        public async Task<Activity> GetActivityAsync(int id)
        {
            var db = dbConnection();
            var sqlQuery = $@"
                            SELECT * FROM public.""Activity"" 
                            WHERE id = @Id";
            var result = await db.QueryFirstOrDefaultAsync<Activity>(sqlQuery, new { Id = id });
            return result;
        }

        public async Task<IEnumerable<Activity>> GetActivitiesList()
        {
            var db = dbConnection();
            var currentTime = DateTime.Now;
            var rule1 = currentTime.AddDays(-3);
            var rule2 = currentTime.AddDays(7);

            var sql = $@"SELECT * FROM public.""Activity""";
            var result = await db.QueryAsync<Activity>(sql, null);

            var activitiesList = new List<Activity>();

            foreach (var item in result)
            {
                if (item.Schedule >= rule1 && item.Schedule <= rule2)
                {
                    if (item.Status == "Activo" && item.Schedule >= currentTime)
                    {
                        item.Condition = "Pendiente a Realizar";
                    }

                    if (item.Status == "Activo" && item.Schedule < currentTime)
                    {
                        item.Condition = "Atrasada";
                    }

                    if (item.Status == "Realizada")
                    {
                        item.Condition = "Finalizada";
                    }

                    activitiesList.Add(item);
                }
            }
            return activitiesList;
        }

        public async Task<bool> InsertActivityAsync(Activity activity)
        {
            var db = dbConnection();
            var getProperty = $@"
                            SELECT * FROM public.""Property"" 
                            WHERE id = @Id";
            var property = await db.QueryFirstOrDefaultAsync<Property>(getProperty, new { Id = activity.PropertyId });

            //No se pueden crear actividades si una Propiedad está desactivada.
            if (property != null && property.Id != 0 && property.DisabledAt == null)
            {
                var sqlActivity = $@"
                            SELECT * FROM public.""Activity"" 
                            WHERE propertyId = @Id AND schedule BETWEEN @Schedule1 AND @Schedule2";
                
                var activityRecovered = await db.QueryFirstOrDefaultAsync<Activity>(sqlActivity, new { Id = property.Id, Schedule1 = activity.Schedule, Schedule2 = activity.Schedule.AddMinutes(60) });

                //No se puede crear actividades en la misma FechaHora para la misma propiedad tomando en cuenta que cada actividad dura maximo una hora.
                if (activityRecovered == null)
                {
                    var sql = $@"
                                INSERT INTO public.""Activity"" 
                                (propertyId,schedule,title,createdat,status)
                                VALUES (@PropertyId,@Schedule,@Title,@CreatedAt,@Status)
                               ";
                    var result = await db.ExecuteAsync(sql, new
                    {
                        PropertyId = activity.PropertyId,
                        Schedule = activity.Schedule,
                        Title = activity.Title,
                        CreatedAt = DateTime.Now,
                        Status = "Activo"
                    });
                    return result > 0;
                }
            }
            return false;
        }

        public async Task<bool> RescheduleActivityAsync(int id, Activity activity)
        {
            var db = dbConnection();
            var sqlThisActivity = $@"
                            SELECT * FROM public.""Activity"" 
                            WHERE id = @Id";

            var thisActivityRecovered = await db.QueryFirstOrDefaultAsync<Activity>(sqlThisActivity, new { Id = id });
            if (thisActivityRecovered == null)
            {
                return false;
            }

            var getProperty = $@"
                            SELECT * FROM public.""Property"" 
                            WHERE id = @Id";
            var property = await db.QueryFirstOrDefaultAsync<Property>(getProperty, new { Id = thisActivityRecovered.PropertyId });

            //No se pueden crear actividades si una Propiedad está desactivada.
            if (property != null && property.Id != 0 && property.DisabledAt == null)
            {
                var sqlActivity = $@"
                            SELECT * FROM public.""Activity"" 
                            WHERE propertyId = @Id AND schedule BETWEEN @Schedule1 AND @Schedule2";

                var dto = new { Id = property.Id, Schedule1 = activity.Schedule, Schedule2 = activity.Schedule.AddMinutes(60) };

                var activityRecovered = await db.QueryFirstOrDefaultAsync<Activity>(sqlActivity, dto );

                //No se puede actualizar actividades en la misma FechaHora para la misma propiedad tomando en cuenta que cada actividad dura maximo una hora.
                if (activityRecovered == null || activityRecovered.Id == id)
                {
                    if (thisActivityRecovered.Status != "Cancelada")
                    {
                        var sqlQuery = $@"
                                UPDATE public.""Activity"" SET
                                Schedule = @Schedule, UpdatedAt = @UpdatedAt
                                WHERE id = @Id
                            ";
                        var result = await db.ExecuteAsync(sqlQuery, new
                        {
                            Schedule = activity.Schedule,
                            UpdatedAt = DateTime.Now,
                            Id = id,
                        });
                        return result > 0;
                    }
                }
            }
            return false;
        }

        public async Task<bool> CancelActivityAsync(int id, Activity activity)
        {
            var db = dbConnection();
            var sqlQuery = $@"
                                UPDATE public.""Activity"" SET
                                status = @Status, UpdatedAt = @UpdatedAt
                                WHERE id = @Id
                            ";
            var result = await db.ExecuteAsync(sqlQuery, new
            {
                UpdatedAt = DateTime.Now,
                Id = id,
                Status = "Cancelada"
            });
            return result > 0;
        }

        public async Task<bool> DoneActivity(int id)
        {
            var db = dbConnection();
            var sqlQuery = $@"
                                UPDATE public.""Activity"" SET
                                status = @Status, UpdatedAt = @UpdatedAt
                                WHERE id = @Id
                            ";
            var result = await db.ExecuteAsync(sqlQuery, new
            {
                UpdatedAt = DateTime.Now,
                Id = id,
                Status = "Realizada"
            });
            return result > 0;
        }

        public async Task<IEnumerable<Activity>> GetActivitiesByDateAndStatusAsync(Search search)
        {
            var date2 = new DateTime();
            if (search.Date2.HasValue)
            {
                date2 = DateTime.Parse($"{search.Date2.Value.Year}-{search.Date2.Value.Month}-{search.Date2.Value.Day} 23:59:59");
            }

            var db = dbConnection();
            var whereStatus = "";
            var whereDate = "";
            var where = "";

            if (!string.IsNullOrEmpty(search.Status))
            { 
                whereStatus = "status = @Status";
            }

            if (search.Date1 != null && search.Date2 != null)
            {
                whereDate = "schedule BETWEEN @Date1 AND @Date2";
            }

            if (!string.IsNullOrEmpty(whereStatus))
            {
                where = $"WHERE {whereStatus}";
            }

            if (!string.IsNullOrEmpty(whereStatus) && !string.IsNullOrEmpty(whereDate))
            {
                where = $"{where} AND {whereDate}";
            }
            else if(!string.IsNullOrEmpty(whereDate))
            {
                where = $"WHERE {whereDate}";
            }



            var sql = $@"SELECT * FROM public.""Activity"" {where}";

            var dto = new {
                                Date1 = search.Date1,
                                Date2 = date2,
                                Status = search.Status
                            };

            var result = await db.QueryAsync<Activity>(sql, dto);
            return result;
        }
    }
}
