using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NDRCreates.Models.DisplayModels;
using NDRCreates.Models.Entities;
using NDRCreates.Models.ViewModels;
using NDRCreates.Services.FileUploadService;

namespace NDRCreates.Controllers
{
    public class UnityPackagesController(ILogger<HomeController> _logger, IUnityPackageService _packageService,
        IWebHostEnvironment _hostingEnvironment, IHttpContextAccessor _httpContextAccessor,
            UserManager<BasicUser> _userManager)
        : Controller
    {

        // GET: UnityPackages
        public async Task<IActionResult> Index()
        {
            var packages = await _packageService.GetAllPackages();

            FeedDisplayModel model = new()
            {
                Packages = packages.ToList(),
            };

            return View(model);
        }

        // GET: UnityPackages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var unityPackage = await _packageService.GetPackageById((int)id);

            if (unityPackage == null)
            {
                return NotFound();
            }

            return View(unityPackage);
        }

        // GET: UnityPackages/Create
        public IActionResult Create()
            => View();

        // POST: UnityPackages/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UploadPackageViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Define the server directory paths for Package and thumbnails
            var packagesDirectoryPath = Path.Combine(_hostingEnvironment.WebRootPath, "Uploads/Files/Packages/");
            var thumbnailsDirectoryPath = Path.Combine(_hostingEnvironment.WebRootPath, "Uploads/Files/Thumbnails/");

            // Ensure the directories exist or create them if they don't
            Directory.CreateDirectory(packagesDirectoryPath);
            Directory.CreateDirectory(thumbnailsDirectoryPath);

            // Generate the PackageUrl and ThumbnailUrl based on the saved file names
            var packageFileName = model.PackageFiles.FirstOrDefault()?.FileName;
            var thumbnailFileName = model.ThumbnailFiles.FirstOrDefault()?.FileName;

            foreach (var packageFile in model.PackageFiles)
            {
                // Generate a unique file name for the Package
                var fileName = packageFileName;

                // Save the Package file to the Packages directory
                var packageFilePath = Path.Combine(packagesDirectoryPath, fileName);

                await using var fileStream = System.IO.File.Create(packageFilePath);
                await packageFile.CopyToAsync(fileStream);
            }

            // Process the uploaded thumbnail image files
            foreach (var thumbnailFile in model.ThumbnailFiles)
            {
                // Generate a unique file name for the thumbnail
                var fileName = thumbnailFileName;

                // Save the thumbnail file to the img directory
                var thumbnailFilePath = Path.Combine(thumbnailsDirectoryPath, fileName);

                await using var fileStream = System.IO.File.Create(thumbnailFilePath);
                await thumbnailFile.CopyToAsync(fileStream);
            }

            
            var curUserId = GetUserId();

            // After processing all files, create a new Package object and add it to the database
            UnityPackage newPackage = new()
            {
                Title = model.Title,
                Description = model.Description,
                FilePath = "/Uploads/Files/Packages/" + packageFileName,
                ThumbnailPath = "/Uploads/Files/Thumbnails/" + thumbnailFileName,
                UploadDate = DateTime.Now,
                Version = model.Version,
                UserId = curUserId, // Current Sign in User
            };

            await _packageService.AddPackage(newPackage);

            return RedirectToAction("index", "UnityPackages");
        }

        // GET: UnityPackages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var package = await _packageService.GetPackageById((int)id);

            var model = new EditPackageViewModel
            {
                Id = package.Id,
                Title = package.Title,
                Description = package.Description
            };

            return View(package);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditPackageViewModel model)
        {
            var package = await _packageService.GetPackageById(id);

            if (model.Id != package.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // After processing all files, create a new Package object and add it to the database
            UnityPackage updatedPackage = new()
            {
                Title = model.Title,
                Description = model.Description,
            };

            await _packageService.UpdatePackage(id, updatedPackage);
            return RedirectToAction("Index", "Package");
        }

        // GET: UnityPackages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var unityPackage = await _packageService.GetPackageById((int)id);

            if (unityPackage == null)
            {
                return NotFound();
            }

            return View(unityPackage);
        }

        // POST: UnityPackages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _packageService.DeletePackage((int)id);
            return RedirectToAction(nameof(Index));
        }

        private string GetUserId()
        {
            var principal = _httpContextAccessor.HttpContext.User;
            string userId = _userManager.GetUserId(principal);
            return userId;
        }

        [HttpGet]
        public async Task<IActionResult> DownloadAsync(int id)
        {
            var package = await _packageService.GetPackageById((int)id);

            if (package == null)
            {
                return NotFound("Package not found");
            }
            else
            {
                // Check if the physical file exists
                var filePath = Path.Combine(_hostingEnvironment.WebRootPath, package.FilePath);
                
                if (!System.IO.File.Exists(filePath))
                {
                    return NotFound("File does not exist");
                }

                // Prepare the file for download
                var memoryStream = new MemoryStream();
                using (var stream = new FileStream(filePath, FileMode.Open))
                {
                    stream.CopyTo(memoryStream);
                }

                memoryStream.Position = 0;
                package.DownloadCount++;
                await _packageService.UpdatePackage(package.Id, package);
                return File(memoryStream, "application/octet-stream", Path.GetFileName(package.FilePath));
            }
        }

    }
}
