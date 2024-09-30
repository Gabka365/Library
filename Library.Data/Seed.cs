using Library.Data.Models;
using Library.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
