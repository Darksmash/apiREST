using PruebaApiREST.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PruebaApiREST.DAL
{
    public interface IPropertiesProvider
    {
        Task<IEnumerable<Property>> GetAllPropertiesAsync();
        Task<Property> GetPropertyAsync(int id);
        Task<bool> InsertPropertyAsync(Property property);
        Task<bool> UpdatePropertyAsync(int id, Property property);
        Task<bool> DeletePropertyAsync(int id);
    }
}
