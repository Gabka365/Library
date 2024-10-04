using Library.Data.Models;
using Library.Data.Repositories;
using Library.Models.Books;
using Library.Services.AuthStuff;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
using Library.Services;
using Library.Models.User;

namespace Library.Controllers
{
    public class UserController : Controller
    {
        private AuthService _authService;
        private BooksRepository _booksRepository;
        private UserRepository _userRepository;
        private Services.PathHelper _pathHelper;

        public UserController(AuthService authService, BooksRepository booksRepository, UserRepository userRepository, Services.PathHelper pathHelper) 
        {
            _authService = authService;
            _booksRepository = booksRepository;
            _userRepository = userRepository;
            _pathHelper = pathHelper;
        }


        public IActionResult UserBooks()
        {
            var user = _authService.GetUser();

            var books = user.Books;

            var booksViewModel = books.Select(BuildBookViewModel).ToList();

            var viewModel = new UserBooksViewModel 
            { 
                Books = booksViewModel 
            };

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Add(int bookId)
        {
            var user = _authService.GetUser();
            var book = _booksRepository.Get(bookId);

            book.Count--;
            _booksRepository.Update(book);


            user.Books.Add(book);

            _userRepository.Update(user);

            return RedirectToAction("ReadBooks","Books");
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
                Count = book.Count,
            };
    }
}
