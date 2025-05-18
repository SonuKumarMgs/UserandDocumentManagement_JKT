using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using Org.BouncyCastle.Crypto.Macs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserandDocumentManagement_JKT.Data;
using UserandDocumentManagement_JKT.DTOs;
using UserandDocumentManagement_JKT.Models;
using UserandDocumentManagement_JKT.Sevices.Implementations;
using UserandDocumentManagement_JKT.Sevices.Interfaces;

namespace NUnit_Test
{
    [TestFixture]
    public class UserManagementBL
    {
        private IUserService _userService;
        private AppDbCotext _context;
        private List<User> usersProfile;
        private UserServiceImpl userServiceImpl;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbCotext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;

            _context = new AppDbCotext(options);
            _context.Users.Add(new User
            {
                Id = Guid.NewGuid(),
                Username = "rakeshkumar",
                Password = "hashedpassword",
                Email = "rkmgs@gmail.com",
                Role = "editor"
            });
            _context.SaveChanges();

            _userService = new UserServiceImpl(_context);

            // _context = new Mock<AppDbCotext>();
            //_userService = new Mock<IUserService>();
            //usersProfile = new List<User>();
            //usersProfile.Add(new User() {  Username = "rakeshkumar", Email = "rkmgs@gmail.com", Password = "AQAAAAIAAYagAAAAEHgd4Bc/ro1Qj0Wm3z0ON60Q5cHctpeQ9beKFXw9FVhSM09alZHGKapDBqVy8Fp6Pg==", Role= "editor" });
            var userDbSet = GetQueryableMockDbSet(usersProfile);

            //_context.Setup(c => c.Users).Returns(userDbSet.Object);
        }

        [Test]
        public async Task GetAllValidUser_ReturnsUser()
        {
           // userServiceImpl = new UserServiceImpl(_context.);
            var result = await userServiceImpl.GetAllUsersAsync();
            Assert.IsNotNull(result);
        }
        [Test]
        public async Task GetUsersByUserId_ReturnsUser()
        {
            var userId = Guid.Parse("0196caac-9063-73e5-aa4d-d500033361da");
           // userServiceImpl = new UserServiceImpl(_context.Object);
            var result = await userServiceImpl.GetUsersByUserIdAsync(userId);
            Assert.IsNotNull(result);
        }
        [Test]
        public async Task UpdateUserRoleById_ReturnsUser()
        {
            var userId = Guid.Parse("0196caac-9063-73e5-aa4d-d500033361da");
            string newRole = "viewer";
           // userServiceImpl = new UserServiceImpl(_context.Object);
            var result = await userServiceImpl.UpdateUserRoleByIdAsync(userId, newRole);
            Assert.IsNotNull(result);
        }

        public static Mock<DbSet<T>> GetQueryableMockDbSet<T>(List<T> sourceList) where T : class
        {
            var queryable = sourceList.AsQueryable();
            var dbSet = new Mock<DbSet<T>>();

            dbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            dbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            dbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            dbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());

            dbSet.Setup(d => d.Add(It.IsAny<T>())).Callback<T>(sourceList.Add);

            return dbSet;
        }

    }
}
