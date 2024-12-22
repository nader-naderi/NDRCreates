using Microsoft.EntityFrameworkCore.Metadata.Internal;

using NDRCreates.Data;
using NDRCreates.Models.Entities;
using NDRCreates.Repositories;

namespace NDRCreates.Services.FileUploadService
{
    public interface IFileUploadService
    {
        Task<string> UploadFileAsync(IFormFile file);

    }

    public class LocalFileStorageService(IHostEnvironment environment, ApplicationDbContext _context) : IFileUploadService
    {
        public readonly IHostEnvironment _environment = environment;

        public async Task<string> UploadFileAsync(IFormFile file)
        {
            var filepath = Path.Combine(_environment.ContentRootPath, "Uploads\\Files", file.FileName);
            using var fileStream = new FileStream(filepath, FileMode.Create);
            await file.CopyToAsync(fileStream);

            return filepath;
        }
    }

    public interface IUnityPackageService
    {
        Task<IEnumerable<UnityPackage>> GetAllPackages();
        Task<IEnumerable<UnityPackage>> GetAllPackagesByUserId(string id);
        Task<UnityPackage> GetPackageById(int id);
        Task<UnityPackage> AddPackage(UnityPackage package);
        Task<UnityPackage> UpdatePackage(int id, UnityPackage package);
        Task DeletePackage(int id);
    }

    public class UnityPackageService(IUnityPackageRepository _packageRepository) : IUnityPackageService
    {
        public async Task<IEnumerable<UnityPackage>> GetAllPackages()
        {
            try
            {
                return await _packageRepository.GetAllPackages();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while fetching packages: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<UnityPackage>> GetAllPackagesByUserId(string userId)
        {
            try
            {
                return await _packageRepository.GetAllPackagesByUserId(userId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while fetching packages: {ex.Message}");
                throw;
            }
        }

        public async Task<UnityPackage> GetPackageById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("package ID must be a positive integer.");
            }

            try
            {
                return await _packageRepository.GetPackageById(id);
            }
            catch (NotFoundException ex)
            {
                Console.WriteLine($"package not found: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while fetching a package: {ex.Message}");
                throw;
            }
        }

        public async Task<UnityPackage> AddPackage(UnityPackage package)
        {
            if (package == null)
            {
                throw new ArgumentNullException(nameof(package), "package object cannot be null.");
            }

            try
            {
                return await _packageRepository.AddPackage(package);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while adding a package: {ex.Message}");
                throw;
            }
        }

        public async Task<UnityPackage> UpdatePackage(int id, UnityPackage package)
        {
            if (id <= 0)
            {
                throw new ArgumentException("package ID must be a positive integer.");
            }

            if (package == null)
            {
                throw new ArgumentNullException(nameof(package), "package object cannot be null.");
            }

            try
            {
                return await _packageRepository.UpdatePackage(id, package);
            }
            catch (NotFoundException ex)
            {
                Console.WriteLine($"package not found: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while updating a package: {ex.Message}");
                throw;
            }
        }

        public async Task DeletePackage(int id)
        {
            try
            {
                await _packageRepository.DeletePackage(id);
            }
            catch (NotFoundException ex)
            {
                Console.WriteLine($"package not found: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while deleting a package: {ex.Message}");
            }
        }
    }
}
