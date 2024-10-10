using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Data.Models
{
    public class BookInstance : BaseModel
    {
        public virtual Book Book { get; set; }  
        public DateTime? DeliveryDate { get; set; }  
        public DateTime? ExpectedReturnDate { get; set; }  
        public virtual User? User { get; set; }  
    }
}
