using Library.Data.Repositories;
using Library.Models.Authors;
using Library.Models.Books;
using Microsoft.AspNetCore.Mvc;
using Library.Models.Common;
using Library.Data.Models;
using Library.Services.Interfaces;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Library.Controllers.ActionFilterAttributes;
using Library.Services.AuthStuff.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Library.Data.Repositories.Interfaces;

namespace Library.Controllers
{
    public class BooksController : Controller
    {
        private readonly IBooksRepository _booksRepository;
        private readonly IAuthorsRepository _authorsRepository;
        private readonly IPathHelper _pathHelper;
        private readonly IAuthService _authService;
        private readonly IBookInstancesRepository _bookInstancesRepository;
        private readonly IMemoryCache _cache;

        public BooksController(IBooksRepository booksRepository, IAuthorsRepository authorsRepository, 
            IPathHelper pathHelper, IAuthService authService, IBookInstancesRepository bookInstancesRepository, IMemoryCache cache)
        {
            _booksRepository = booksRepository;
            _authorsRepository = authorsRepository;
            _pathHelper = pathHelper;
            _authService = authService;
            _bookInstancesRepository = bookInstancesRepository;
            _cache = cache;
        }

        [HttpGet]
        [IsAdmin]
        public IActionResult CreateBook()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateBook(BookViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var author = _authorsRepository.GetByName(viewModel.BookAuthor.FirstName, viewModel.BookAuthor.LastName);
            if (author is null)
            {
                return RedirectToAction("CreateAuthor", "Authors");
            }

            var existingBook = _booksRepository.GetByAuthorAndName(viewModel.Name, author);

            if (existingBook != null)
            {

                for (int i = 0; i < viewModel.Count; i++)
                {
                    var bookInstance = new BookInstance
                    {
                        Book = existingBook
                    };
                    _bookInstancesRepository.Create(bookInstance); 
                }
            }
            else
            {
                var newBook = new Book
                {
                    Name = viewModel.Name,
                    Description = viewModel.Description,
                    ISBN = viewModel.ISBN,
                    Genre = viewModel.Genre,
                    BookAuthor = author,
                };

                var createdBook = _booksRepository.Create(newBook);

                for (int i = 0; i < viewModel.Count; i++)
                {
                    var bookInstance = new BookInstance
                    {
                        Book = createdBook
                    };
                    _bookInstancesRepository.Create(bookInstance);  
                }

                if (viewModel.Cover != null)
                {
                    var path = _pathHelper.GetPathToBookCover(createdBook.Id);

                    if (!_cache.TryGetValue(path, out byte[] cachedImage))
                    {
                        using (var fs = new FileStream(path, FileMode.Create))
                        {
                            viewModel.Cover.CopyTo(fs);
                        }

                        using (var memoryStream = new MemoryStream())
                        {
                            viewModel.Cover.CopyTo(memoryStream);
                            cachedImage = memoryStream.ToArray();
                        }

                        _cache.Set(path, cachedImage, TimeSpan.FromMinutes(30));
                    }
                }
            }

            return RedirectToAction("Books", "Home");
        }

        [HttpGet]
        public IActionResult ReadBooks()
        {
            var booksViewModels = _booksRepository
                .GetAll()
                .Select(BuildBookViewModel)
                .ToList();

            var viewModel = new ReadBooksViewModel
            {
                Books = booksViewModels,
                IsAdmin = _authService.IsAdmin()
            };
            return View(viewModel);
        }

        [HttpGet]
        public IActionResult SearchByIdBook()
        {
            var viewModel = new SearchByIdViewModel();
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult SearchByIdBook(SearchByIdViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            int Id = Convert.ToInt32(viewModel.Id);

            var book = _booksRepository.Get(Id);

            if (book == null)
            {
                return View(viewModel);
            }

            viewModel.Book = book;
            viewModel.HasCover = _pathHelper.IsBookCoverExist(Id);

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult SearchByISBN()
        {
            var viewModel = new SearchByIsbnViewModel();
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult SearchByISBN(SearchByIsbnViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var book = _booksRepository.GetByISBN(viewModel.ISBN);

            if (book == null)
            {
                return View(viewModel);
            }

            viewModel.SearchedBook = book;
            viewModel.HasCover = _pathHelper.IsBookCoverExist(book.Id);

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult SearchByName()
        {
            var viewModel = new SearchByNameViewModel();
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult SearchByName(SearchByNameViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var book = _booksRepository.GetByName(viewModel.Name);

            if (book == null)
            {
                return View(viewModel);
            }

            viewModel.SearchedBook = book;
            viewModel.HasCover = _pathHelper.IsBookCoverExist(book.Id);

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult UpdateBook(int id)
        {
            var book = _booksRepository.Get(id);

            var viewModel = new BookViewModel
            {
                Id = id,
                Name = book.Name,
                Description = book.Description,
                ISBN = book.ISBN,
                Genre = book.Genre,
                BookAuthor = book.BookAuthor,
                Count = _bookInstancesRepository.GetCountOfFreeBooks(book),
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult UpdateBook(BookViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var author = _authorsRepository.GetByName(viewModel.BookAuthor.FirstName, viewModel.BookAuthor.LastName);
            if (author == null)
            {
                return RedirectToAction("CreateAuthor", "Authors");
            }

            var existingBook = _booksRepository.Get((int)viewModel.Id);
            var currentCount = _bookInstancesRepository.GetCountOfFreeBooks(existingBook);


            existingBook.Name = viewModel.Name;
            existingBook.Description = viewModel.Description;
            existingBook.ISBN = viewModel.ISBN;
            existingBook.Genre = viewModel.Genre;
            existingBook.BookAuthor = author;  

            if (viewModel.Count != currentCount)
            {
                _bookInstancesRepository.UpdateBookInstances(existingBook, viewModel.Count, currentCount);
            }

            _booksRepository.Update(existingBook);

            if (viewModel.Cover != null)
            {
                
                var path = _pathHelper?.GetPathToBookCover(existingBook.Id);
                
                System.IO.File.Delete(path);

                using (var fs = new FileStream(path, FileMode.Create))
                {
                    viewModel.Cover.CopyTo(fs);
                }

                if (!_cache.TryGetValue(path, out byte[] cachedImage))
                { 
                    using (var memoryStream = new MemoryStream())
                    {
                        viewModel.Cover.CopyTo(memoryStream);
                        cachedImage = memoryStream.ToArray();
                    }

                    _cache.Set(path, cachedImage, TimeSpan.FromMinutes(30));
                }
            }

            return RedirectToAction("ReadBooks");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            
            _booksRepository.Delete(id);

            var path = _pathHelper?.GetPathToBookCover(id);

            
            System.IO.File.Delete(path);
            _cache.Remove(path);
            

            return RedirectToAction("ReadBooks");
        }


        [HttpGet]
        public IActionResult OpenBookPage(int id)
        {
            var book = _booksRepository.Get(id);

            var viewModel = BuildBookViewModel(book);   

            return View(viewModel);
        }

        private BookViewModel BuildBookViewModel(Book book)
            => new BookViewModel
            {
                Id = book.Id,
                Name = book.Name,
                Description = book.Description,
                ISBN = book.ISBN,
                Genre = book.Genre,
                BookAuthor = book.BookAuthor,
                HasCover = _pathHelper.IsBookCoverExist(book.Id),
                Count = _bookInstancesRepository.GetCountOfFreeBooks(book),
            };
    }
}
