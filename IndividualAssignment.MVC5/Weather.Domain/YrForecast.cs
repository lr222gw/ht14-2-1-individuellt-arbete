using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weather.Domain
{
    [MetadataType(typeof(Forecast_Metadata))]
    public partial class Forecast
    {

        public Forecast(DateTime _DateNTime, string _Temperature, string _Pic)
        {
            DateNTime = _DateNTime;
            Temperature = _Temperature;
            PictureName = _Pic;
        }

        public  string imageUrl { get; set; }
        public class Forecast_Metadata
        {            
            public DateTime DateNTime { get; set; }
            public string Temperature { get; set; }
            public string PictureName { get; set; }
        }
    }
}
