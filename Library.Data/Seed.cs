using Library.Data.Models;
using Library.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Library.Data
{
    public class Seed
    {
        public void Fill(IServiceProvider serviceProvider)
        {
            using var service = serviceProvider.CreateScope();

            FillAuthors(service);
            FillBooks(service);
        }

        private void FillAuthors(IServiceScope service)
        {
            var authorsRepository = service.ServiceProvider.GetService<AuthorsRepository>()!;

            if (!authorsRepository.Any())
            {
                var Moem = new Author
                {
                    FirstName = "Somerset",
                    LastName = "Moem",
                    Birthday = new DateTime(1874, 1, 25),
                    Motherland = "England",
                };
                authorsRepository.Create(Moem);

                var Remarque = new Author
                {
                    FirstName = "Erich",
                    LastName = "Remarque",
                    Birthday = new DateTime(1898, 6, 22),
                    Motherland = "Germany"
                };
                authorsRepository.Create(Remarque);

                var Hemingway = new Author
                {
                    FirstName = "Ernest",
                    LastName = "Hemingway",
                    Birthday = new DateTime(1899, 7, 21),
                    Motherland = "USA"
                };
                authorsRepository.Create(Hemingway);
            }
        }

        private void FillBooks(IServiceScope service)
        {
            var booksRepository = service.ServiceProvider.GetService<BooksRepository>()!;
            var authorRepository = service.ServiceProvider.GetService<AuthorsRepository>()!;
            var bookInstancesRepository = service.ServiceProvider.GetService<BookInstancesRepository>()!;

            if (!booksRepository.Any())
            {
                var theBurdenOfHumanPassions = new Book
                {
                    Name = "The burden of human passions",
                    Genre = "Fiction novel",
                    Description = "“A novel of education”, where the author traces the life of the main character Philip Carey from childhood to adolescence, from youth to maturity.",
                    ISBN = "978-5-17-062680-9",
                    BookAuthor = authorRepository.GetByLastName("Moem"),
                };
                var createdMoemBook = booksRepository.Create(theBurdenOfHumanPassions);
                bookInstancesRepository.CreateBookInstances(createdMoemBook, 10, bookInstancesRepository);

                var threeComrades = new Book
                {
                    Name = "Three Comrades",
                    Genre = "Fiction novel",
                    Description = "A story about the friendship of three German soldiers after World War I and their shared experiences of love, loss, and hardship.",
                    ISBN = "978-3-423-01368-1",
                    BookAuthor = authorRepository.GetByLastName("Remarque"),
                };
                var createdRemarqueBook = booksRepository.Create(threeComrades);
                bookInstancesRepository.CreateBookInstances(createdRemarqueBook, 10, bookInstancesRepository);

                var theOldManAndTheSea = new Book
                {
                    Name = "The Old Man and the Sea",
                    Genre = "Fiction novel",
                    Description = "A short novel about an old Cuban fisherman who battles with a giant marlin far out in the Gulf Stream, showcasing themes of struggle, resilience, and dignity.",
                    ISBN = "978-0-684-80122-3",
                    BookAuthor = authorRepository.GetByLastName("Hemingway"),
                };
                var createdHemingwayBook = booksRepository.Create(theOldManAndTheSea);
                bookInstancesRepository.CreateBookInstances(createdHemingwayBook, 10, bookInstancesRepository);
            }
        }
    }
}
