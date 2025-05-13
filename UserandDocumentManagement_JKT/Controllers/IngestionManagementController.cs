using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserandDocumentManagement_JKT.Data;

namespace UserandDocumentManagement_JKT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IngestionManagementController : ControllerBase
    {
        private readonly AppDbCotext _context;

        public IngestionManagementController(AppDbCotext context)
        {
            _context = context;
        }

        [HttpGet("status/{documentId}")]
        public async Task<IActionResult> GetStatus(Guid documentId)
        {
            var status = await _context.IngestionStatuses
                .FirstOrDefaultAsync(x => x.DocumentId == documentId);

            if (status == null)
                return NotFound(new { Message = "No ingestion found for document." });

            return Ok(status);
        }

        [HttpDelete("cancel/{documentId}")]
        public async Task<IActionResult> CancelIngestion(Guid documentId)
        {
            var status = await _context.IngestionStatuses
                .FirstOrDefaultAsync(x => x.DocumentId == documentId);

            if (status == null)
                return NotFound(new { Message = "Ingestion not found." });

            status.Status = "Canceled";
            status.TriggeredAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(new { Message = "Ingestion canceled." });
        }

    }
}
