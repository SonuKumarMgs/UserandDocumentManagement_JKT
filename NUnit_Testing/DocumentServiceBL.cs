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
using UserandDocumentManagement_JKT.Sevices.Implementations;

namespace NUnit_Testing
{
    [TestFixture]
    public class DocumentServiceBL
    {
        private Mock<AppDbCotext> _dbContextMock;
        private Mock<IWebHostEnvironment> _envMock;
        private DocumentServiceImpl _documentService;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbCotext>()
                          .UseInMemoryDatabase(databaseName: "TestDb")
                          .Options;

            _dbContextMock = new Mock<AppDbCotext>(options);
            _envMock = new Mock<IWebHostEnvironment>();

            _envMock.Setup(e => e.ContentRootPath).Returns(Directory.GetCurrentDirectory());

            var dbContext = new AppDbCotext(options);
            _documentService = new DocumentServiceImpl(dbContext, _envMock.Object);
        }

        [Test]
        public async Task UploadDocumentAsync_ValidFile_ReturnsDocument()
        {
            // Arrange
            var ownerId = Guid.NewGuid();
            var fileMock = new Mock<IFormFile>();
            var content = "Fake file content";
            var fileName = "test.txt";
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(content);
            writer.Flush();
            ms.Position = 0;

            fileMock.Setup(f => f.OpenReadStream()).Returns(ms);
            fileMock.Setup(f => f.FileName).Returns(fileName);
            fileMock.Setup(f => f.Length).Returns(ms.Length);
            fileMock.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), default))
                    .Returns<Stream, System.Threading.CancellationToken>((stream, token) => ms.CopyToAsync(stream));

            // Act
            var result = await _documentService.UploadDocumentAsync(fileMock.Object, ownerId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(fileName, result.FileName);
            Assert.AreEqual(ownerId, result.OwnerId);
            Assert.IsTrue(File.Exists(Path.Combine(_envMock.Object.ContentRootPath, result.FilePath)));
        }
    }
}
