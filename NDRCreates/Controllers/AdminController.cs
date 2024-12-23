using Microsoft.AspNetCore.Mvc;

namespace NDRCreates.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ManageContents()
        {
            return RedirectToAction("Index", "Home");
        }

        public IActionResult UploadFiles()
        {
            return View();
        }

        [HttpPost]
        public IActionResult UploadFiles(IFormFile files)
        {
            // Handle file upload
            return View(files);
        }

        public IActionResult ApproveContent()
        {
            // Display content approval page
            return View();
        }

        [HttpPost]
        public IActionResult ApproveContent(int contentId)
        {
            // Handle content approval
            return View();
        }

        // View activities
        public IActionResult ViewActivities()
        {
            // Display activities page
            return View();
        }
    }
}
