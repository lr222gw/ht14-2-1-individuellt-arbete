using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weather.Domain.Repositories
{
    public interface IRepository : IDisposable
    {
        //List<Location> getAllLocations();
        void Insertlocation(Location location);
        void Insertlocations(IEnumerable<Location> location);
        void Updatelocation(Location location);
        void Deletelocation(Location location);
        Location getlocationByGeoId(int id);
        IEnumerable<Location> getlocationByGeoName(string name);
        bool RecentlySearched(string searchToCheck);
        void deleteSearch(string searchToDelete);
        void save();
    }
}
