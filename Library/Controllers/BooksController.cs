using Library.Data.Repositories;
using Library.Models.Authors;
using Library.Models.Books;
using Microsoft.AspNetCore.Mvc;
using Library.Models.Common;
using Library.Data.Models;

namespace Library.Controllers
{
    public class BooksController : Controller
    {
        private BooksRepository _booksRepository;
        private AuthorsRepository _authorsRepository;


        public BooksController(BooksRepository booksRepository, AuthorsRepository authorsRepository)
        {
            _booksRepository = booksRepository;
            _authorsRepository = authorsRepository;
        }

        [HttpGet]
        public IActionResult CreateBook()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateBook(BookViewModel viewModel)
        {
            if (_authorsRepository.GetByLastName(viewModel.BookAuthor.LastName) is null ||
                _authorsRepository.GetByFirstName(viewModel.BookAuthor.FirstName) is null)
            {
                return RedirectToAction("CreateAuthor", "Authors");
            }

            var book = new Book
            {
                Name = viewModel.Name,
                Description = viewModel.Description,
                ISBN = viewModel.ISBN,
                Genre = viewModel.Genre,
                BookAuthor = _authorsRepository.GetByLastName(viewModel.BookAuthor.LastName)
            };

            _booksRepository.Create(book);

            return RedirectToAction("Books", "Home");
        }

        public IActionResult ReadBooks()
        {
            var booksRepo = _booksRepository.GetAll();

            var viewModel = new ReadBooksViewModel
            {
                Books = booksRepo,
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

            viewModel.Book = book;

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

            viewModel.SearchedBook = book;

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
                BookAuthor = book.BookAuthor
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult UpdateBook(BookViewModel viewModel)
        {
            if(_authorsRepository.GetByLastName(viewModel.BookAuthor.LastName) is null ||
                _authorsRepository.GetByFirstName(viewModel.BookAuthor.FirstName) is null)
            {
                return RedirectToAction("CreateAuthor", "Authors");
            }

            var book = new Book
            {
                Id = (int)viewModel.Id,
                Name = viewModel.Name,
                Description = viewModel.Description,
                ISBN = viewModel.ISBN,
                Genre = viewModel.Genre,
                BookAuthor = viewModel.BookAuthor
            };

           _booksRepository.Update(book);

            return RedirectToAction("ReadBooks");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            _booksRepository.Delete(id);
            return RedirectToAction("ReadBooks");
        }
    }
}
