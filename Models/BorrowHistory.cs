using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TechnoIsland_Bookstore.Models
{
    public class BorrowHistory
    {
        [Key]
        public int BorrowHistoryId { get; set; }
        [Required]

        [Display(Name = "Book")]
        public int Id { get; set; }
        public Book Book { get; set; }
        [Required]
        [Display(Name = "User")]
        public int UserId { get; set; }
        public User User { get; set; }
        [Display(Name = "Borrow Date")]
        public DateTime BorrowDate { get; set; }
        [Display(Name = "Return Date")]
        public DateTime ReturnDate { get; set; }
    }
}
