using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weather.Domain
{
    public class YrForecast
    {
        public YrForecast(DateTime _DateNTime, string _Temperature, string _Pic)
        {
            DateNTime = _DateNTime;
            Temperature = _Temperature;
            Pic = _Pic;
        }
        public DateTime DateNTime { get; set; }
        public string Temperature { get; set; }
        public string Pic { get; set; }        
    }
}
