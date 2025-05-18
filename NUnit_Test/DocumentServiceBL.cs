using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserandDocumentManagement_JKT.Data;
using UserandDocumentManagement_JKT.Models;
using UserandDocumentManagement_JKT.Sevices.Implementations;
using UserandDocumentManagement_JKT.Sevices.Interfaces;

namespace NUnit_Test
{
    public class DocumentServiceBL
    {
        private Mock<IDocumentService> _documentService;
        private Mock<AppDbCotext> _context;
        private List<UploadDocument> uploadDocument;
        private DocumentServiceImpl DocumentServiceImpl;
        private  Mock<IWebHostEnvironment> _env;
        [SetUp]
        public void Setup()
        {
            _documentService = new Mock<IDocumentService>();
            uploadDocument = new List<UploadDocument>();           
        }

        [Test]
        public async Task UploadDocument()
        {
            var userId = Guid.Parse("0196caac-9063-73e5-aa4d-d500033361da");
            var fileName = "test.txt";
            var fileContent = "This is a test file.";
            var fileBytes = System.Text.Encoding.UTF8.GetBytes(fileContent);

            var mockFormFile = new Mock<IFormFile>();
            var stream = new MemoryStream(fileBytes);

            mockFormFile.Setup(f => f.FileName).Returns(fileName);
            mockFormFile.Setup(f => f.Length).Returns(fileBytes.Length);
            mockFormFile.Setup(f => f.OpenReadStream()).Returns(stream);
            mockFormFile.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
                        .Returns<Stream, CancellationToken>((target, token) => stream.CopyToAsync(target, token));

            var mockEnv = new Mock<IWebHostEnvironment>();
            var fakeRootPath = Path.Combine(Path.GetTempPath(), "TestRoot");
            mockEnv.Setup(e => e.ContentRootPath).Returns(fakeRootPath);

            var mockSet = new Mock<DbSet<UploadDocument>>();
            var mockContext = new Mock<AppDbCotext>();
            mockContext.Setup(c => c.UploadDocuments).Returns(mockSet.Object);
            mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
                       .ReturnsAsync(1);
            var result = await DocumentServiceImpl.UploadDocumentAsync(mockFormFile.Object, userId);
            Assert.IsNotNull(result);
        }
        [Test]
        public async Task GetAllDocumentsByUserId()
        {
            var userId = Guid.Parse("0196caac-9063-73e5-aa4d-d500033361da");           
            var result = await DocumentServiceImpl.GetAllDocumentsByUserIdAsync(userId);
            Assert.IsNotNull(result);
        }
        [Test]
        public async Task GetSingleDocumentsByUserId()
        {
            var userId = Guid.Parse("0196caac-9063-73e5-aa4d-d500033361da");           
            var result = await DocumentServiceImpl.GetSingleDocumentsByUserIdAsync(userId);
            Assert.IsNotNull(result);
        }
    }
}
