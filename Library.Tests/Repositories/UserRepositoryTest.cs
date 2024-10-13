using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Moq;
using Library.Data;
using Library.Data.Repositories;
using Library.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Tests.Repositories
{
    public class UserRepositoryTest
    {
        [Test]
        [TestCase(2)]
        public void Get_Success(int id)
        {
            // Prepare
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            var options = new DbContextOptionsBuilder<LibraryDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .UseInternalServiceProvider(serviceProvider)
                .Options;

            using (var dbContext = new LibraryDbContext(options))
            {
                dbContext.Database.EnsureCreated();

                var users = AddUsers(dbContext);

                var userRepository = new UserRepository(dbContext);

                // Act
                var result = userRepository.Get(id);

                // Assert
                var user = users.SingleOrDefault(x => x.Id == id);
                Assert.That(result, Is.EqualTo(user));
            }
        }


        [Test]
        [TestCase(2)]
        public void Get_Fail(int id)
        {
            // Prepare
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            var options = new DbContextOptionsBuilder<LibraryDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .UseInternalServiceProvider(serviceProvider)
                .Options;

            using (var dbContext = new LibraryDbContext(options))
            {
                dbContext.Database.EnsureCreated();

                var userRepository = new UserRepository(dbContext);

                // Act
                var result = userRepository.Get(id);

                // Assert
                Assert.That(result, Is.EqualTo(null));
            }
        }

        [Test]
        public void CreateUser_Success()
        {
            // Prepare
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()  
                .BuildServiceProvider();

            var options = new DbContextOptionsBuilder<LibraryDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .UseInternalServiceProvider(serviceProvider)  
                .Options;

            using (var dbContext = new LibraryDbContext(options))
            {
                dbContext.Database.EnsureCreated();

                var user = new User
                {
                    Id = 2,
                    Name = "Test_3",
                    HashedPassword = "123",
                    RefreshToken = null,
                    UserRole = Data.Enums.UserRole.User,
                    TokenCreated = null,
                    TokenExpires = null,
                };

                var userRepository = new UserRepository(dbContext);

                // Act
                var result = userRepository.Create(user);

                // Assert
                Assert.That(result, Is.EqualTo(user));
            }
        }


        private DbSet<User> AddUsers(LibraryDbContext databaseContext)
        {    
            if (databaseContext.Users.Count() == 0)
            {
                for (int i = 1; i <= 10; i++)
                {
                    databaseContext.Users.Add(
                    new User
                    {
                        Id = i,
                        Name = $"Test{i}",
                        HashedPassword = "HashedPassword" + i.ToString(),
                        RefreshToken = null,
                        UserRole = Data.Enums.UserRole.User,
                        TokenCreated = null,
                        TokenExpires = null,
                        BookInstances = null
                    });
                    databaseContext.SaveChanges();
                }
            }

            return databaseContext.Users;
        }
    }
}
