using Microsoft.AspNetCore.Mvc;
using UglyToad.PdfPig;
using System.Text;

namespace Skill_Extractor_Alpha.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileUploadController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public FileUploadController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            string extractedText = string.Empty;

            if (file.ContentType == "application/pdf")
            {
                // Extract text from PDF
                extractedText = ExtractTextFromPdf(file);
            }
            else if (file.ContentType == "application/msword" ||
                     file.ContentType == "application/vnd.openxmlformats-officedocument.wordprocessingml.document")
            {
                // Extract text from Word document
                //extractedText = ExtractTextFromWord(file);
            }
            else
            {
                return BadRequest("Unsupported file type.");
            }

            // Send the extracted text to another endpoint
            var client = _httpClientFactory.CreateClient();
            var content = new StringContent(extractedText, System.Text.Encoding.UTF8, "text/plain");
            var response = await client.PostAsync("https://your-endpoint/api/process-text", content);

            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, "Failed to send text to the processing endpoint.");

            return Ok("File uploaded and text extracted successfully.");
        }

		private string ExtractTextFromPdf(IFormFile file)
		{
			using (var stream = file.OpenReadStream())
			{
				var text = new StringBuilder();
				using (PdfDocument document = PdfDocument.Open(stream))
				{
					foreach (var page in document.GetPages())
					{
						string pageText = page.Text;
						text.Append(pageText);
					}
				}

				return text.ToString();
			}
		}
	
		//private string ExtractTextFromWord(IFormFile file)
  //      {
  //          using (var stream = file.OpenReadStream())
  //          {
  //              var stream2 = file.OpenReadStream();
  //              var application = new Microsoft.Office.Interop.Word.Application();
  //              var document = application.Documents.Open(ref stream2);
  //              var text = document.Content.Text;
  //              document.Close();
  //              application.Quit();
  //              return text;
  //          }
  //      }
    }
}
