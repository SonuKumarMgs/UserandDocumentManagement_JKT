using UserandDocumentManagement_JKT.Sevices.Interfaces;

namespace UserandDocumentManagement_JKT.Sevices.Implementations
{
    public class IngestionServiceImpl : IIngestionService
    {
        private readonly HttpClient _httpClient;

        public IngestionServiceImpl(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<bool> TriggerIngestionAsync(Guid documentId)
        {
            var response = await _httpClient.PostAsync(
            $"http://localhost:8080/api/springboot/ingestion/{documentId}", null); // Change to actual Spring Boot URL

            return response.IsSuccessStatusCode;
        }
    }
}
