using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weather.Domain.Repositories
{
    public class dbRepository : IRepository
    {
        
        Entities _context = new Entities();
        public void Insertlocation(Location location)
        {
            _context.Location.Add(location);
        }
        public void Insertlocations(IEnumerable<Location> location)
        {
            //using(_context = new Entities()){

                foreach(var item in location){
                    var alreadyExists = from GeoLocationID
                                        in _context.Location
                                        where GeoLocationID.GeoLocationID == item.GeoLocationID
                                        select GeoLocationID;
                    
                    if (alreadyExists.ToArray().Length <= 1)
                    {
                        _context.Location.Add(item);
                    }
                    
                }
            //}
            //save();
        }


        public Location getlocationByGeoId(int id)
        {
            var result = from Name
                         in _context.Location
                         where Name.GeoLocationID == id
                         select Name;            
            if (result.Any()) {

                var he =  new Location()
                {
                    Country = result.First().Country,
                    GeoLocationID = result.First().GeoLocationID,
                    Latitude = result.First().Latitude,
                    Longitude = result.First().Longitude,
                    LocationID = result.First().LocationID,
                    Name = result.First().Name,
                    NextForecastUpdate = result.First().NextForecastUpdate
                };
                return he;
            }
            return null;
        }
        public IEnumerable<Location> getlocationByGeoName(string name)
        {
            List<Location> listToReturn = new List<Location>();

            var result = from Name
                    in _context.Location
                            where Name.Name.Contains(name)
                            select Name;
                
            foreach (var results in result)
            {

                listToReturn.Add(
                    new Location(
                        results.Name,
                        results.Longitude,
                        results.Latitude,
                        results.Country,
                        results.GeoLocationID
                        )
                    );
                    
            }
                
            return listToReturn.AsEnumerable<Location>();
        }

        public void save()
        {

            try
            {
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                var he = e;
            }
            
            
        }

        #region IDisposable
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion


        public bool RecentlySearched(string searchToCheck)
        {

            //string myResult = "";
            //DateTime myResultDate = new DateTime();
            prevSearchCache modelFromDatabase = new prevSearchCache();
            //using(_context = new Entities()){ // Går inte att bara ha _context, då får jag felet "DbContext has been disposed" på andra ställen i denna klass...
                var result = (from SearchValue
                         in _context.prevSearchCache
                         where SearchValue.SearchValue == searchToCheck.ToLower().Trim()
                         select SearchValue);

                //foreach(var results in result){
                //myResult = result.First().SearchValue;
                //myResultDate = result.First().validDate;
                    //modelFromDatabase = new prevSearchCache() { ID = results.ID, SearchValue = results.SearchValue, validDate = results.validDate };
                //}                
            //}
            
            //save();
            if (!result.Any())
            {//om den inte hittas så är den ny och då ska data hämtas från geoNames...

                //var he = DateTime.Today + new TimeSpan(5, 0, 0, 0);
                _context.prevSearchCache.Add(new prevSearchCache() { SearchValue = searchToCheck.ToLower().Trim(), validDate = DateTime.Today + new TimeSpan(30, 0, 0, 0) });
                return true;
            }
            else if (result.First().validDate < DateTime.Today)
            {//Finns i databas, men en ny sökning görs ändå för att elliminera chansen att ett nytt ställe kommit fram på kartan...(och missas här..)

                var toChange = result.First();
                _context.prevSearchCache.Remove(toChange);
                save();
                toChange.validDate = DateTime.Today + new TimeSpan(30, 0, 0, 0);
                _context.prevSearchCache.Add(toChange);
                return true;
            }

            return false;
        }

        public bool RecentlyLocationForecast(Location locationToCheck)
        {
            var resultDate = (from location
                         in _context.Location
                          where location.GeoLocationID == locationToCheck.GeoLocationID
                          select location.NextForecastUpdate);

            if(resultDate.Any()){
                var date = resultDate.First().Value;
                if(date < DateTime.Today){
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }            


        }

        public void InsertForecast(Location locationToGiveForecast, IEnumerable<Forecast> yrList)
        {
            try
            {//ta bort den gamla och lägg till den nya som innehåller forecasts...
                var toRemove = from location
                               in _context.Location
                               where location.LocationID == locationToGiveForecast.LocationID
                               select location;
                _context.Location.Remove(toRemove.First());
                save();
                locationToGiveForecast.Forecast = yrList.ToArray();
                _context.Location.Add(locationToGiveForecast);                
                save();
            }catch(Exception e){
                var h = e; 
            }
            

        }

        public IEnumerable<Forecast> getCachedForecasts(Location locationToCheck)
        {
            var result = (from forecast
                         in _context.Forecast
                          where forecast.LocationID == locationToCheck.LocationID
                          select forecast);

            List<Forecast> forecastlist = new List<Forecast>();

            if(result.Any()){

                var res = result;
                foreach (var forecast in res)
                {
                    forecastlist.Add(new Forecast(forecast.DateNTime, forecast.Temperature, forecast.PictureName)
                    {                       
                        ForecastID = forecast.ForecastID,
                        Location = forecast.Location,
                        LocationID = forecast.LocationID
                    });
                }
            }
            return forecastlist.AsEnumerable();

        }


    }
}
