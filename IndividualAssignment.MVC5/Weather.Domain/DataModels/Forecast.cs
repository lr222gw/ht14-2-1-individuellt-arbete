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
    
    public partial class Forecast
    {
        public int ForecastID { get; set; }
        public int LocationID { get; set; }
        public System.DateTime DateNTime { get; set; }
        public string Temperature { get; set; }
        public string PictureName { get; set; }
    
        public virtual Location Location { get; set; }
    }
}
