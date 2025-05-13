using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserandDocumentManagement_JKT.Models;

namespace UserandDocumentManagement_JKT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IngestionTriggerController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public IngestionTriggerController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpPost("trigger")]
        public async Task<IActionResult> TriggerIngestion([FromBody] IngestionTriggerRequest request)
        {
            var client = _httpClientFactory.CreateClient();

            var springBootUrl = "http://api/springboot/ingestion"; // Replace with actual URL

            var response = await client.PostAsJsonAsync(springBootUrl, request);

            if (response.IsSuccessStatusCode)
                return Ok(new { Message = "Ingestion triggered successfully." });

            return StatusCode((int)response.StatusCode, "Failed to trigger ingestion.");
        }
    }
}
