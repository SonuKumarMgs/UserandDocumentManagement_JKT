using System.Reflection.Metadata;

namespace UserandDocumentManagement_JKT.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Role { get; set; } = "viewer"; // Admin, Editor, Viewer
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<UploadDocument> UploadDocuments { get; set; }
    }
}
