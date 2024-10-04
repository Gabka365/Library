using Microsoft.AspNetCore.Mvc;
using Library.Data.Repositories;
using Library.Data.Models;
using Library.Models.Authors;
using Library.Models.Common;
using System.Security.Claims;

namespace Library.Controllers
{
    public class AuthorsController : Controller
    {
        private AuthorsRepository _authorsRepository;

        public AuthorsController(AuthorsRepository authorsRepository) 
        { 
            _authorsRepository = authorsRepository; 
        }

        public IActionResult ReadAuthors()
        {
            var authorsViewModel = _authorsRepository
                .GetAll()
                .Select(BuildAuthorViewModel)
                .ToList();

            var viewModel = new ReadAuthorsViewModel
            {
                authors = authorsViewModel
            };

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult CreateAuthor()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateAuthor(AuthorViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var author = new Author
            {
                FirstName = viewModel.FirstName,
                LastName = viewModel.LastName,
                Birthday = viewModel.DateTime,
                Motherland = viewModel.Motherland
            };

            _authorsRepository.Create(author);

            return RedirectToAction("Authors", "Home");
        }


        [HttpGet]
        public IActionResult UpdateAuthor(int id)
        {
            var author = _authorsRepository.Get(id);

            var viewModel = new AuthorViewModel
            {
                Id = id,
                FirstName = author.FirstName,
                LastName = author.LastName,
                DateTime = author.Birthday,
                Motherland = author.Motherland  
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult UpdateAuthor(AuthorViewModel viewModel)
        {

            var author = new Author
            {
                Id = (int)viewModel.Id,
                FirstName = viewModel.FirstName,
                LastName = viewModel.LastName,
                Birthday = viewModel.DateTime,
                Motherland = viewModel.Motherland
            };

            _authorsRepository.Update(author);

            return RedirectToAction("ReadAuthors");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            _authorsRepository.Delete(id);
            return RedirectToAction("ReadAuthors");
        }

        [HttpGet]
        public IActionResult SearchByIdAuthor()
        {
            var viewModel = new SearchByIdViewModel();
            return View(viewModel); 
        }

        [HttpPost]
        public IActionResult SearchByIdAuthor(SearchByIdViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            int Id = Convert.ToInt32(viewModel.Id);

            var author = _authorsRepository.Get(Id);

            viewModel.Author = author;

            return View(viewModel);
        }

        private AuthorViewModel BuildAuthorViewModel(Author author)
            => new AuthorViewModel
            {
                Id = author.Id,
                FirstName = author.FirstName,
                LastName = author.LastName,
                DateTime = author.Birthday,
                Motherland = author.Motherland
            };
    }
}
