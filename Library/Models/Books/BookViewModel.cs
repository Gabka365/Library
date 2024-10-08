﻿using Library.Data.Models;
using Library.Models.ValidationAttributes;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Library.Models.Books
{
    public class BookViewModel
    {
        public int? Id { get; set; }
        public int? InstanceId { get; set; }
        
        [IsbnFormat]
        public string ISBN { get; set; }
        public string Name { get; set; }
        public string Genre { get; set; }
        public string Description { get; set; }
        public bool? HasCover { get; set; }

        [BooksCount]
        public int Count { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public DateTime? ExpectedReturnDate { get; set; }

        [ValidateNever]
        public Author? BookAuthor { get; set; }
        
        [MaxImageSize(300, 400)]
        public IFormFile? Cover { get; set; }
    }
}
