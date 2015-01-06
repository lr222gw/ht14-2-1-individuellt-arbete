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

        public void Updatelocation(Location location)
        {
            throw new NotImplementedException();
        }

        public void Deletelocation(Location location)
        {
            throw new NotImplementedException();
        }

        public Location getlocationByGeoId(int id)
        {
            throw new NotImplementedException();
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
                _context.prevSearchCache.Add(new prevSearchCache() { SearchValue = searchToCheck.ToLower().Trim(), validDate = DateTime.Today + new TimeSpan(5, 0, 0, 0) });
                return true;
            }
            else if (result.First().validDate < DateTime.Today)
            {//Finns i databas, men en ny sökning görs ändå för att elliminera chansen att ett nytt ställe kommit fram på kartan...(och missas här..)

                var toChange = result.First();
                _context.prevSearchCache.Remove(toChange);
                save();
                toChange.validDate = DateTime.Today + new TimeSpan(5, 0, 0, 0);
                _context.prevSearchCache.Add(toChange);
                return true;
            }

            return false;
        }

        public void deleteSearch(string searchToDelete)
        {
            throw new NotImplementedException();
        }
    }
}
