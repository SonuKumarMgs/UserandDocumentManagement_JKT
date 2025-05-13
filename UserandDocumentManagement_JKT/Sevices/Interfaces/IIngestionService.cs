namespace UserandDocumentManagement_JKT.Sevices.Interfaces
{
    public interface IIngestionService
    {
        Task<bool> TriggerIngestionAsync(Guid documentId);
    }
}
