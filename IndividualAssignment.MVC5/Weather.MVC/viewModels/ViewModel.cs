using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Weather.Domain;

namespace Weather.MVC.viewModels
{
    public class ViewModel
    {
        [Required]
        [StringLength(50)]        
        public string search {get; set;}
        public IEnumerable<Location> Locations { get; set; }
    }
}