using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weather.Domain
{
    public class GeonameLocation
    {
        public GeonameLocation(string _name, string _Longitude, string _Latitude, string _Country){ //, string _Region
            name = _name;
            Longitude = _Longitude;
            Latitude = _Latitude;
            Country = _Country;
            //Region = _Region;

        }
        public string name { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string Country { get; set; }
        //public string Region { get; set; }
    }
}
