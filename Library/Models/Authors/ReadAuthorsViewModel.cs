﻿using Library.Data.Models;

namespace Library.Models.Authors
{
    public class ReadAuthorsViewModel
    {
        public List<AuthorViewModel> authors;
        public bool IsAdmin { get; set; }
    }
}
