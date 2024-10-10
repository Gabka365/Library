using Library.Data.Models;
using Library.Models.ValidationAttributes;

namespace Library.Models.Common
{
    public class SearchByIdViewModel
    {
        [IdInteger]
        public string Id { get; set; }
        public bool? HasCover { get; set; }
        public Author? Author { get; set; }
        public Book? Book { get; set; }
    }
}
