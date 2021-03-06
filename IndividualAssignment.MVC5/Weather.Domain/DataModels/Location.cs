//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Weather.Domain
{
    using System;
    using System.Collections.Generic;
    
    public partial class Location
    {
        public Location()
        {
            this.Forecast = new HashSet<Forecast>();
        }
    
        public int LocationID { get; set; }
        public Nullable<System.DateTime> NextForecastUpdate { get; set; }
        public string Name { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string Country { get; set; }
        public int GeoLocationID { get; set; }
    
        public virtual ICollection<Forecast> Forecast { get; set; }
    }
}
