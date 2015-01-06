using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Weather.Domain.Repositories;
using Weather.Domain.Webservices;

namespace Weather.Domain
{
    public class SuperService
    {
        IRepository myDB;
        public SuperService(IRepository repo)
        {
            myDB = repo;
        }
        public SuperService()
            :this(new dbRepository()){                
        }

        public IEnumerable<Location> search(string Search)
        { // Bestämmer var sökningen ska göras, hämta på nytt eller använda från databas. 
            GeonamesWebservice geo = new GeonamesWebservice();
            IEnumerable<Location> result;
            if (myDB.RecentlySearched(Search))
            {                
                result = geo.searchGeonames(Search);
                myDB.Insertlocations(result);
                myDB.save();
            }
            else
            {
                result = myDB.getlocationByGeoName(Search);
            }
            
            
            return result;
        }

        
    }
}
