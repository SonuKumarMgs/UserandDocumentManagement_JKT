using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
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

namespace NUnit_Testing
{
    [TestFixture]
    public class AuthServiceBL
    {
        private SignupServiceImpl _signupService;
        private AppDbCotext _context;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbCotext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            _context = new AppDbCotext(options);
            _context.Database.EnsureDeleted();  // Ensure clean DB for every test

            _signupService = new SignupServiceImpl(_context);
        }

        [Test]
        public async Task UserSignup_WhenUsernameExists_ReturnsUsernameExistsMessage()
        {
            // Arrange
            _context.Users.Add(new User { Username = "existinguser", Password = "pass", Email = "test@example.com" });
            await _context.SaveChangesAsync();

            var newUser = new UserDto { Username = "existinguser", Password = "newpass", Email = "new@example.com" };

            // Act
            var result = await _signupService.UserSingup(newUser);

            // Assert
            Assert.AreEqual("Username already exists", result);
        }

        [Test]
        public async Task UserSignup_WithNewUsername_ReturnsSuccessMessage()
        {
            // Arrange
            var newUser = new UserDto { Username = "newuser", Password = "password123", Email = "new@example.com" };

            // Act
            var result = await _signupService.UserSingup(newUser);

            // Assert
            Assert.AreEqual("Registered successfully", result);

            // Also check if user was added
            var userInDb = _context.Users.FirstOrDefault(u => u.Username == "newuser");
            Assert.IsNotNull(userInDb);
            Assert.AreEqual("new@example.com", userInDb.Email);
        }
    }
}
