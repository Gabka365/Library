using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Moq;
using Newtonsoft.Json.Bson;
using Library.Controllers;
using System.Runtime.CompilerServices;
using Library.Services.AuthStuff.Interfaces;
using Library.Data.Repositories.Interfaces;
using Library.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Library.Data.Models;
using Library.Data.Enums;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Library.Models.User;


namespace Library.Tests.Controllers
{
    public class UserControllerTest
    {
        private Mock<IAuthService> _authServiceMock;
        private Mock<IBooksRepository> _booksRepositoryMock;
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<IPathHelper> _pathHelperMock;
        private Mock<IBookInstancesRepository> _bookInstancesRepositoryMock;
        private UserController _userController;
        private Book _testBook;
        private List<BookInstance> _bookInstances;
        private User _user;

        [SetUp]
        public void SetUp()
        {
            _authServiceMock = new Mock<IAuthService>();
            _booksRepositoryMock = new Mock<IBooksRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _pathHelperMock = new Mock<IPathHelper>();
            _bookInstancesRepositoryMock = new Mock<IBookInstancesRepository>();

            _userController = new UserController(_authServiceMock.Object, _booksRepositoryMock.Object, 
                _userRepositoryMock.Object, _pathHelperMock.Object, _bookInstancesRepositoryMock.Object);

            _testBook = new Book
            {
                Id = 1,
                ISBN = "test isbn",
                Name = "test name",
                Genre = "test genre",
                BookAuthor = null,
                Description = "test description",
                Instances = null,
                User = null,
            };

            _user = new User { Id = 1, Name = "Test", UserRole = UserRole.User, HashedPassword = "123", Books = { _testBook } };

            _bookInstances = new List<BookInstance>
            {
                new BookInstance
                {
                    Id = 3,
                    DeliveryDate = null,
                    ExpectedReturnDate = null,
                    User = _user,
                    Book = _testBook
                },
                new BookInstance
                {
                    Id = 4,
                    DeliveryDate = null,
                    ExpectedReturnDate = null,
                    User = _user,
                    Book = _testBook,
                }
            };

            _testBook.User = _user;
            _testBook.Instances = _bookInstances;
        }

        [Test]
        [TestCase("2024-10-12", "2024-10-13")]
        public void UserBooks_ReturnsViewWithCorrectViewModel(string startDate, string endDate)
        {
            //Prepare
            _authServiceMock
                .Setup(x => x.GetUser())
                .Returns(_user);

            _userRepositoryMock
                .Setup(x => x.GetBookInstances(_user.Id))
                .Returns(_bookInstances);

            _bookInstances[0].DeliveryDate = DateTime.Parse(startDate);
            _bookInstances[1].ExpectedReturnDate = DateTime.Parse(endDate);

            //Act
            var result = _userController.UserBooks() as ViewResult;

            //Assert
            Assert.That(result, Is.Not.Null);
            var viewModel = result.Model as UserBooksViewModel;
            Assert.That(viewModel, Is.Not.Null);    
            Assert.That(viewModel.BookInstances.Count == 2);
            Assert.That(viewModel.BookInstances[0].DeliveryDate == DateTime.Parse(startDate));
            Assert.That(viewModel.BookInstances[1].ExpectedReturnDate == DateTime.Parse(endDate));
        }


        [Test]
        public void Add_RedirectToHomeIfNoAvailableInstances()
        {
            //Prepare
            _authServiceMock
                .Setup(x => x.GetUser())
                .Returns(_user);

            _booksRepositoryMock
                .Setup(x => x.Get(_testBook.Id))
                .Returns(_testBook);

            _bookInstancesRepositoryMock
                .Setup(x => x.GetByBook(_testBook))
                .Returns(_bookInstances);
            //Act
            var result = _userController.Add(_testBook.Id) as RedirectToActionResult;

            //Assert
            Assert.That("Books", Is.EqualTo(result.ActionName));
            Assert.That("Home", Is.EqualTo(result.ControllerName));
        }


        [Test]
        public void Add_RedirectToBooksIfAvailableInstancesExist()
        {
            //Prepare
            _authServiceMock
                .Setup(x => x.GetUser())
                .Returns(_user);

            _booksRepositoryMock
                .Setup(x => x.Get(_testBook.Id))
                .Returns(_testBook);

            _bookInstancesRepositoryMock
                .Setup(x => x.GetByBook(_testBook))
                .Returns(_bookInstances);

            _bookInstancesRepositoryMock
                .Setup(x => x.Update(It.IsAny<BookInstance>()))
                .Verifiable();

            _userRepositoryMock
                .Setup(x => x.Update(It.IsAny<User>()))
                .Verifiable();

            _bookInstances.Add(new BookInstance 
            {
                Id = 5,
                DeliveryDate = null,
                ExpectedReturnDate = null,
                User = null,
                Book = _testBook
            });


            //Act
            var result = _userController.Add(_testBook.Id) as RedirectToActionResult;


            //Assert
            Assert.That("ReadBooks", Is.EqualTo(result.ActionName));
            Assert.That("Books", Is.EqualTo(result.ControllerName));
        }


        [Test]
        [TestCase(3)]
        public void Return_RemovingExistingUsersInstance(int index)
        {
            //Prepare
            _bookInstancesRepositoryMock
                .Setup(x => x.GetById(index))
                .Returns(_bookInstances.First(x => x.Id == index));

            _bookInstancesRepositoryMock
                .Setup(x => x.Update(It.IsAny<BookInstance>()))
                .Verifiable();

            _userRepositoryMock
                .Setup(x => x.Update(It.IsAny<User>()))
                .Verifiable();

            //Act
            var result = _userController.Return(index);

            //Assert
            var instance = _bookInstances
                .First(x => x.Id == index);

            Assert.That(instance.User, Is.EqualTo(null));
            Assert.That(instance.DeliveryDate, Is.EqualTo(null));
            Assert.That(instance.ExpectedReturnDate, Is.EqualTo(null));
        }
    }
}
