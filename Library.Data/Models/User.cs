using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Data.Models
{
    public class User : BaseModel
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public virtual List<Book> Books { get; set; }
    }
}
