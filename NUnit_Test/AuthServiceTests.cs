using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Org.BouncyCastle.Crypto.Generators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserandDocumentManagement_JKT.Data;
using UserandDocumentManagement_JKT.DTOs;
using UserandDocumentManagement_JKT.Sevices.Interfaces;

namespace NUnit_Test
{
    [TestFixture]
    public class AuthServiceTests
    {
        private ISignupService signupService;
        private AppDbCotext _context;

        [SetUp]
        public void Setup()
        {
          
           
        }

        [Test]
        public async Task AuthenticateAsync_ValidUser_ReturnsUser()
        {
           
        }
       
    }

}
