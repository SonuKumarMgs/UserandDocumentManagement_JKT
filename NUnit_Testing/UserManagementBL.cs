using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserandDocumentManagement_JKT.Data;
using UserandDocumentManagement_JKT.Models;
using UserandDocumentManagement_JKT.Sevices.Implementations;

namespace NUnit_Testing
{
    [TestFixture]
    public class UserManagementBL
    {
        private AppDbCotext _context;
        private UserServiceImpl _userService;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbCotext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // new DB per test
                .Options;

            _context = new AppDbCotext(options);
            _context.Database.EnsureCreated();

            // Seed users
            _context.Users.AddRange(
                new User { Id = Guid.NewGuid(), Username = "admin", Role = "Admin", Email = "admin@example.com" },
                new User { Id = Guid.NewGuid(), Username = "user1", Role = "User", Email = "user1@example.com" },
                new User { Id = Guid.NewGuid(), Username = "editor", Role = "Editor", Email = "editor@example.com" }
            );
            _context.SaveChanges();

            _userService = new UserServiceImpl(_context);
        }

        [Test]
        public async Task GetAllUsersAsync_ReturnsNonAdminUsers()
        {
            var users = await _userService.GetAllUsersAsync();

            Assert.IsNotNull(users);
            Assert.IsTrue(users.All(u => u.Role != "Admin"));
            Assert.AreEqual(2, users.Count); // Admin should be filtered out
        }

        [Test]
        public async Task GetUsersByUserIdAsync_ReturnsUser_WhenUserExists()
        {
            var existingUser = _context.Users.First(u => u.Role != "Admin");

            var result = await _userService.GetUsersByUserIdAsync(existingUser.Id);

            Assert.IsNotNull(result);
            Assert.AreEqual(existingUser.Username, result.Username);
        }

        [Test]
        public void GetUsersByUserIdAsync_ThrowsException_WhenUserNotFound()
        {
            var nonExistentId = Guid.NewGuid();

            var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            {
                await _userService.GetUsersByUserIdAsync(nonExistentId);
            });

            Assert.That(ex.Message, Is.EqualTo("User not found."));
        }

        [Test]
        public async Task UpdateUserRoleByIdAsync_UpdatesRoleSuccessfully()
        {
            var user = _context.Users.First(u => u.Role == "User");

            string newRole = "Manager";
            var result = await _userService.UpdateUserRoleByIdAsync(user.Id, newRole);

            var updatedUser = await _context.Users.FindAsync(user.Id);

            Assert.AreEqual("Updated data sucessfully..", result);
            Assert.AreEqual(newRole, updatedUser.Role);
        }

        [Test]
        public void UpdateUserRoleByIdAsync_ThrowsException_WhenUserNotFound()
        {
            var nonExistentId = Guid.NewGuid();

            var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            {
                await _userService.UpdateUserRoleByIdAsync(nonExistentId, "User");
            });

            Assert.That(ex.Message, Is.EqualTo("User not found."));
        }
    }
}
