using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using UserandDocumentManagement_JKT.Data;
using UserandDocumentManagement_JKT.Models;
using UserandDocumentManagement_JKT.Sevices.Interfaces;

namespace UserandDocumentManagement_JKT.Sevices.Implementations
{
    public class DocumentServiceImpl : IDocumentService
    {
        private readonly AppDbCotext _context;
        private readonly IWebHostEnvironment _env;

        public DocumentServiceImpl(AppDbCotext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }


        public async Task<UploadDocument> UploadDocumentAsync(IFormFile file, Guid ownerId)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("No file provided");

            var documentsDir = Path.Combine(_env.ContentRootPath, "Documents");

            if (!Directory.Exists(documentsDir))
                Directory.CreateDirectory(documentsDir);

            var uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";
            var fullPath = Path.Combine(documentsDir, uniqueFileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var document = new UploadDocument
            {
                FileName = file.FileName,
                FilePath = Path.Combine("Documents", uniqueFileName),
                OwnerId = ownerId
            };

            _context.UploadDocuments.Add(document);
            await _context.SaveChangesAsync();

            return document;
        }
        public async Task<UploadDocument> DownloadDocumentAsync(Guid documentId)
        {
            var document = await _context.UploadDocuments.FindAsync(documentId);
            if (document == null)
                throw new FileNotFoundException("Document not found.");
            return document;           
        }
        public async Task<List<UploadDocument>> GetAllDocumentsByUserIdAsync(Guid userId)
        {
            return await _context.UploadDocuments
         .Where(d => d.OwnerId == userId)
         .ToListAsync();
        }
        public async Task<UploadDocument> GetSingleDocumentsByUserIdAsync(Guid userId)
        {
            var user=await _context.Users.FindAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }
            UploadDocument data =  await _context.UploadDocuments.Where(d => d.OwnerId == userId).SingleOrDefaultAsync();
            return data;
        }        
        public async Task<bool> DeleteDocumentAsync(Guid documentId)
        {
            var document = await _context.UploadDocuments.FindAsync(documentId);
            if (document == null)
                return false;

            var fullPath = Path.Combine(_env.ContentRootPath, document.FilePath);
            if (File.Exists(fullPath))
                File.Delete(fullPath);

            _context.UploadDocuments.Remove(document);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
