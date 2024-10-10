using Library.Data.Enums;
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
        public string HashedPassword { get; set; }
        public string? RefreshToken { get; set; }
        public UserRole UserRole { get; set; }
        public DateTime? TokenCreated { get; set; }
        public DateTime? TokenExpires { get; set; }
        public virtual List<Book> Books { get; set; } = new List<Book>();
        public virtual List<BookInstance>? BookInstances { get; set; } = new List<BookInstance>();
    }
}
