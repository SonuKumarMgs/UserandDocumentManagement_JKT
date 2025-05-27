using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;
using UserandDocumentManagement_JKT.Models;

namespace UserandDocumentManagement_JKT.Sevices.Interfaces
{
    public interface IDocumentService
    {
        Task<UploadDocument> UploadDocumentAsync(IFormFile file, Guid ownerId);
        Task<UploadDocument> DownloadDocumentAsync(Guid documentId);
        Task<bool> DeleteDocumentAsync(Guid documentId);
        Task<List<UploadDocument>> GetAllDocumentsByUserIdAsync(Guid userId);
        Task<UploadDocument> GetSingleDocumentsByUserIdAsync(Guid userId);
    }
}
