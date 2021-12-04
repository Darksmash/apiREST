using PruebaApiREST.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PruebaApiREST.DAL
{
    public interface IActivitiesProvider
    {
        Task<IEnumerable<Activity>> GetAllActivitiesAsync();
        Task<IEnumerable<Activity>> GetActivitiesByDateAndStatusAsync(Search search);
        Task<IEnumerable<Activity>> GetActivitiesList();
        Task<Activity> GetActivityAsync(int id);
        Task<bool> InsertActivityAsync(Activity activity);
        Task<bool> RescheduleActivityAsync(int id, Activity activity);
        Task<bool> CancelActivityAsync(int id, Activity activity);
        Task<bool> DeleteActivityAsync(int id);
        Task<bool> DoneActivity(int id);
    }
}
