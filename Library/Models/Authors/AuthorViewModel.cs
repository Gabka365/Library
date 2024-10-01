using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Library.Models.Authors
{
    public class AuthorViewModel
    {
        public int? Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Motherland { get; set; }

        [BindProperty, DataType(DataType.Date)]
        public DateTime DateTime { get; set; }

    }
}
