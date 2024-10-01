using Library.Data.Models;

namespace Library.Models.Common
{
    public class SearchByIdViewModel
    {
        public string Id { get; set; }
        public Author? Author { get; set; }
        public Book? Book { get; set; }
    }
}
