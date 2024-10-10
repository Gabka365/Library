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
        private readonly AuthService _authService;
        private readonly BooksRepository _booksRepository;
        private readonly UserRepository _userRepository;
        private readonly Services.PathHelper _pathHelper;
        private readonly BookInstancesRepository _bookInstancesRepository;

        public UserController(AuthService authService, BooksRepository booksRepository, 
            UserRepository userRepository, Services.PathHelper pathHelper, BookInstancesRepository bookInstancesRepository) 
        {
            _authService = authService;
            _booksRepository = booksRepository;
            _userRepository = userRepository;
            _pathHelper = pathHelper;
            _bookInstancesRepository = bookInstancesRepository;
        }


        public IActionResult UserBooks()
        {
            var user = _authService.GetUser();

            var bookInstances = _userRepository.GetBookInstances(user.Id);

            var bookInstancesViewModel = bookInstances.Select(BuildBookInstanceViewModel).ToList();

            var viewModel = new UserBooksViewModel 
            { 
                BookInstances = bookInstancesViewModel 
            };

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Add(int bookId)
        {
            var user = _authService.GetUser();
            var book = _booksRepository.Get(bookId);

            var availableBookInstance = _bookInstancesRepository
                                            .GetByBook(book)
                                            .FirstOrDefault(instance => instance.User == null && instance.ExpectedReturnDate == null);

            if (availableBookInstance != null)
            {
                availableBookInstance.User = user;
                availableBookInstance.DeliveryDate = DateTime.Now;
                availableBookInstance.ExpectedReturnDate = DateTime.Now.AddDays(30);

                _bookInstancesRepository.Update(availableBookInstance);
                _userRepository.Update(user);  

                return RedirectToAction("ReadBooks", "Books");
            }

            return RedirectToAction("Books", "Home");
        }


        [HttpGet]
        public IActionResult Return(int instanceId)
        {
            var bookInstance = _bookInstancesRepository.GetById(instanceId);

            var user = bookInstance.User;
            user.BookInstances.Remove(bookInstance);

            bookInstance.User = null;   
            bookInstance.DeliveryDate = null;
            bookInstance.ExpectedReturnDate = null;
            
            _bookInstancesRepository.Update(bookInstance);
            _userRepository.Update(user);

            return RedirectToAction("UserBooks");
        }

        private BookViewModel BuildBookInstanceViewModel(BookInstance instance)
            => new BookViewModel
            {
                Id = instance.Book.Id,
                InstanceId = instance.Id,
                Name = instance.Book.Name,
                Description = instance.Book.Description,
                ISBN = instance.Book.ISBN,
                Genre = instance.Book.Genre,
                BookAuthor = instance.Book.BookAuthor,
                HasCover = _pathHelper.IsBookCoverExist(instance.Book.Id),
                DeliveryDate = instance.DeliveryDate,
                ExpectedReturnDate = instance.ExpectedReturnDate,
            };


    }
}
