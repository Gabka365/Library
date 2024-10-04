using Library.Data.Models;

namespace Library.Models.Books
{
    public class ReadBooksViewModel
    {
        public List<BookViewModel> Books { get; set; }
        public bool IsAdmin { get; set; }
    }
}
