using Microsoft.AspNetCore.Mvc;

using NDRCreates.Models.DisplayModels;
using NDRCreates.Services.FileUploadService;

namespace NDRCreates.Controllers
{
    public class AdminController(IUnityPackageService _packageService) : Controller
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

        public IActionResult ViewTables()
        {
            return View("Tables");
        }

        public IActionResult ViewCharts()
        {
            return View("Charts");
        }

        public IActionResult EditContents()
        {
            return View("adminEditView");
        }

        public async Task<IActionResult> UnityPackagesEditPageAsync()
        {
            var packages = await _packageService.GetAllPackages();

            FeedDisplayModel model = new()
            {
                Packages = packages.ToList(),
            };

            return View("adminUnityPackgesSectionView", model);
        }

        public IActionResult CoursesEditPage()
        {
            return View("adminCoursesSectionView");
        }

        public IActionResult BlogEditPage()
        {
            return View("adminBlogSectionView");
        }

        public IActionResult PortfolioEditPage()
        {
            return View("adminPortfolioSectionView");
        }
    }
}
