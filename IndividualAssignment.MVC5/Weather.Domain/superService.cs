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

        public Location getLocationFromGeoID(string GeoID)
        {
            
            var Location = myDB.getlocationByGeoId(int.Parse(GeoID));
            var Forecasts = getForecastForLocation(Location);

            Location.Forecast = Forecasts.ToArray();


            return Location;
        }
        public IEnumerable<Forecast> getForecastForLocation(Location locationToCheck)
        {
            IEnumerable<Forecast> yrList;
            if (myDB.RecentlyLocationForecast(locationToCheck)) {
            //om jag ska hämta ny eller cachad data...
                //return yrList;

                //FEL

                yrList = myDB.getCachedForecasts(locationToCheck);
                myDB.InsertForecast(locationToCheck, yrList);
                return yrList;
            }
            else
            {
                YrWebservice yr = new YrWebservice();
                yrList = yr.preGetForecastFromLaNLo(locationToCheck.Latitude, locationToCheck.Longitude);
                myDB.InsertForecast(locationToCheck, yrList);
                return yrList;
            }

                
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



        public Location getImageForLocationForecasts(Location theLocation)
        {
            YrWebservice yr = new YrWebservice();
            return yr.getImageForLocationForecasts(theLocation);
        }
    }
}
