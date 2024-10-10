using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Data.Models
{
    public class Book : BaseModel
    {
        public string ISBN { get; set; }
        public string Name { get; set; }
        public string Genre { get; set; }
        public string Description { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public DateTime? ExpectedReturnDate  { get; set; }
        public virtual Author BookAuthor { get; set; }
        public virtual List<BookInstance> Instances { get; set; }
        public virtual User? User { get; set; }
    }
}
