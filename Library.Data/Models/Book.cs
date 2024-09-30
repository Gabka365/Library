using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Data.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string ISBN { get; set; }
        public string Name { get; set; }
        public string Genre { get; set; }
        public string Description { get; set; }
        public DateTime DeliveryDate { get; set; }
        public DateTime ExpectedReturnDate  { get; set; }
        public virtual Author Author { get; set; }
        public virtual List<User> ActualUsers { get; set; }
    }
}
