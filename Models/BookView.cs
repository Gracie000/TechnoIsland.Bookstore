using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TechnoIsland_Bookstore.Models
{
    public class BookView
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Publisher { get; set; }
        [Display(Name ="Image Name")]
        public string ImageName { get; set; }
        public bool IsAvailable { get; set; }

    }
}
