﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using System.Security.Claims;
using UserandDocumentManagement_JKT.Data;
using UserandDocumentManagement_JKT.Models;
using UserandDocumentManagement_JKT.Sevices.Interfaces;

namespace UserandDocumentManagement_JKT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class DocumentManagementController : ControllerBase
    {
        private readonly AppDbCotext _context;        
        private readonly IDocumentService _documentService;
        private readonly IIngestionService _ingestionService;
        public DocumentManagementController(AppDbCotext appDbCotext, IDocumentService documentService,
            IIngestionService ingestionService)
        {
            _context = appDbCotext;
            _documentService = documentService;
            _ingestionService = ingestionService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadDocument(IFormFile file)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized("User ID missing in token.");

            var doc = await _documentService.UploadDocumentAsync(file, Guid.Parse(userId));

            var ingestionResult = await _ingestionService.TriggerIngestionAsync(doc.Id);

            if (!ingestionResult)
                return StatusCode(500, "Document uploaded, but ingestion trigger failed.");

            return Ok(new { doc.Id, doc.FileName, doc.OwnerId });
        }
        [HttpGet("DownloadDocument/{id}")]
        public async Task<IActionResult> DownloadDocument(Guid id)
        {
            var document = await _documentService.DownloadDocumentAsync(id);
            return Ok(new { document }); ;
        }

        [HttpDelete("DeleteDocument/{id}")]
        public async Task<IActionResult> DeleteDocument(Guid id)
        {
            var result = await _documentService.DeleteDocumentAsync(id);
            return result ? NoContent() : NotFound();
        }
        [HttpGet("GetSingleDocumentsByUserId/{id}")]
        public async Task<IActionResult> GetSingleDocumentsByUserIdAsync(Guid id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized("User ID missing in token.");
            var doc = await _documentService.GetSingleDocumentsByUserIdAsync(Guid.Parse(userId));
            if (doc == null) return NotFound("Document not found");

            var dto = new UploadDocument
            {
                Id = doc.Id,
                FileName = doc.FileName,
                FilePath = doc.FilePath,
                UploadedAt = doc.UploadedAt,
                OwnerId = doc.OwnerId 
                
            };

            return Ok(dto);
        }

        [HttpGet("GetAllDocumentsByUserId")]
        public async Task<IActionResult> GetAllDocumentsByUserIdAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized("User ID missing in token.");
            var doc = await _documentService.GetAllDocumentsByUserIdAsync(Guid.Parse(userId));
            return Ok(doc);
        }

    }
}
