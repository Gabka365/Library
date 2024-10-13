using Library.Data.Models;
using Library.Models.ValidationAttributes;

namespace Library.Models.Books
{
    public class SearchByNameViewModel
    {
        public string Name { get; set; }
        public Book? SearchedBook { get; set; }
        public bool? HasCover { get; set; }
    }
}
