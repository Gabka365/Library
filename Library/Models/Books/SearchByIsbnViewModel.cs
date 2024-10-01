using Library.Data.Models;

namespace Library.Models.Books
{
    public class SearchByIsbnViewModel
    {
        public string ISBN { get; set; }
        public Book? SearchedBook { get; set; }
    }
}
