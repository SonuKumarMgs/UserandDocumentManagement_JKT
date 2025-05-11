namespace UserandDocumentManagement_JKT.Models
{
    public class UploadDocument
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string? FileName { get; set; }
        public string? FilePath { get; set; }
        public Guid OwnerId { get; set; }
        public User Owner { get; set; }
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    }
}
