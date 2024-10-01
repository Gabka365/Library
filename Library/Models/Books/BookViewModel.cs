using Library.Data.Models;

namespace Library.Models.Books
{
    public class BookViewModel
    {
        public int? Id { get; set; }
        public string ISBN { get; set; }
        public string Name { get; set; }
        public string Genre { get; set; }
        public string Description { get; set; }
        public bool? HasCover { get; set; }
        public Author? BookAuthor { get; set; }
        public IFormFile? Cover { get; set; }
    }
}
