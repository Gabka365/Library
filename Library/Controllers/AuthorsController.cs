using Microsoft.AspNetCore.Mvc;
using Library.Data.Repositories;
using Library.Data.Models;
using Library.Models;

namespace Library.Controllers
{
    public class AuthorsController : Controller
    {
        private AuthorsRepository _authorsRepository;

        public AuthorsController(AuthorsRepository authorsRepository) 
        { 
            _authorsRepository = authorsRepository; 
        }

        public IActionResult Read()
        {
            var authorsRepo = _authorsRepository.GetAll();

            var viewModel = new ReadViewModel
            {
                authors = authorsRepo
            };

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(AuthorViewModel viewModel)
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
        public IActionResult Update(int id)
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
        public IActionResult Update(AuthorViewModel viewModel)
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

            return RedirectToAction("Read");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            _authorsRepository.Delete(id);
            return RedirectToAction("Read");
        }

        [HttpGet]
        public IActionResult SearchById()
        {
            var viewModel = new SearchByIdViewModel();
            return View(viewModel); 
        }

        [HttpPost]
        public IActionResult SearchById(SearchByIdViewModel viewModel)
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
    }
}
