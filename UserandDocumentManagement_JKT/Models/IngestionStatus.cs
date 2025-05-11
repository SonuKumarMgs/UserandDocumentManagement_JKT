using System.Reflection.Metadata;

namespace UserandDocumentManagement_JKT.Models
{
    public class IngestionStatus
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid DocumentId { get; set; }
        public UploadDocument UploadDocuments { get; set; }
        public string Status { get; set; } = "Pending"; // Pending, InProgress, Success, Failed
        public DateTime TriggeredAt { get; set; } = DateTime.UtcNow;
    }
}
