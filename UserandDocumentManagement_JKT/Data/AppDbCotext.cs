using Microsoft.EntityFrameworkCore;
using UserandDocumentManagement_JKT.Models;

namespace UserandDocumentManagement_JKT.Data
{
    public class AppDbCotext : DbContext
    {
        public AppDbCotext(DbContextOptions<AppDbCotext> options) : base(options) { }

        public  DbSet<User> Users { get; set; }
        public DbSet<UploadDocument> UploadDocuments { get; set; }
        public DbSet<IngestionStatus> IngestionStatuses { get; set; }       
    }
}
