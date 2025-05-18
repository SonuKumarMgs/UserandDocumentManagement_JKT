using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using Org.BouncyCastle.Crypto.Generators;
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
    public class AuthServiceTests
    {
        private Mock<ISignupService> _signupService;
        private Mock<AppDbCotext> _context;
        private List<UserDto> userdto;
        private SignupServiceImpl signupServiceImpl;

        [SetUp]
        public void Setup()
        {
            _signupService = new Mock<ISignupService>();
            userdto = new List<UserDto>();
            userdto.Add(new UserDto() { Username = "TestCase", Email = "test@gmail.com", Password = "Test@12345" });
        }

        [Test]
        public async Task AuthenticateAsync_ValidUser_ReturnsUser()
        {
            var userToReturn = userdto.FirstOrDefault(x => x.Username == "TestCase");
            signupServiceImpl = new SignupServiceImpl(_context.Object);
            var result = await signupServiceImpl.UserSingup(userToReturn);
            Assert.IsNotNull(result);
        }

    }

}
