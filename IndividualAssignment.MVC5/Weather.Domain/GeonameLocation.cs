using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weather.Domain
{
    [MetadataType(typeof(Location_Metadata))]
    public partial class Location
    {
        public Location(string _name, string _Longitude, string _Latitude, string _Country){ //, string _Region
            Name = _name;
            Longitude = _Longitude;
            Latitude = _Latitude;
            Country = _Country;
            //Region = _Region;

        }

        public class Location_Metadata
        {
            public string Name { get; set; }
            public string Longitude { get; set; }
            public string Latitude { get; set; }
            public string Country { get; set; }
            //public string Region { get; set; }
        }
    }
}
