using ExcelDataReader;
using Microsoft.AspNetCore.Mvc;
using NBE_Project.Models;


namespace NBE_Project.Controllers
{
    public class ExcelFileController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ExcelFileController(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public IActionResult Index(List<Vender> vendors = null)
        {
            // Initialize vendors if it's null
            vendors = vendors ?? new List<Vender>();

            return View(vendors);
        }

        [HttpPost]
        public IActionResult Index(IFormFile file, [FromServices] Microsoft.AspNetCore.Hosting.IWebHostEnvironment hostingEnvironment)
        {
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("File", "Please upload a valid Excel file.");
                return View(new List<Vender>());
            }

            // Ensure file is Excel type
            var extension = Path.GetExtension(file.FileName);
            if (extension != ".xls" && extension != ".xlsx")
            {
                ModelState.AddModelError("File", "Please upload a valid Excel file (.xls or .xlsx).");
                return View(new List<Vender>());
            }

            // Check file size (limit to 25 MB)
            if (file.Length > 25 * 1024 * 1024)
            {
                ModelState.AddModelError("", "File size exceeds 10 MB.");
                return View(new List<Vender>());
            }

            // Sanitize and create unique file name
            string fileName = Path.GetFileNameWithoutExtension(file.FileName)
                .Replace(" ", string.Empty)
                .Replace("..", string.Empty)
                + "_" + Guid.NewGuid().ToString() + extension;



            // Ensure that the 'files' directory exists
            string uploadPath = Path.Combine(hostingEnvironment.WebRootPath, "files");
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            string filePath = Path.Combine(uploadPath, file.FileName);

            try
            {
                using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                    fileStream.Flush();
                }

                var vendors = GetVendors(filePath);
                return View(vendors);
            }
            catch (IOException ex)
            {
                ModelState.AddModelError("File", $"Error reading file: {ex.Message}");
                return View(new List<Vender>());
            }
        }

        private List<Vender> GetVendors(string fileName)
        {
            var vendors = new List<Vender>();

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "files", fileName);
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            try
            {
                using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        while (reader.Read())
                        {
                            // Skip empty rows
                            if (reader.GetValue(0) != null)
                            {
                                vendors.Add(new Vender()
                                {
                                    Name = reader.GetValue(0)?.ToString(),
                                    Code = reader.GetValue(1)?.ToString(),
                                    IncidentType = reader.GetValue(2)?.ToString(),
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log exception here (not implemented for simplicity)
                throw new IOException($"Error reading vendor data: {ex.Message}");
            }

            return vendors;
        }
    }
}
