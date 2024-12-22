using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

using NDRCreates.Data;
using NDRCreates.Models.Entities;

namespace NDRCreates.Repositories
{
    public interface IRepository
    {
    }

    public interface IUnityPackageRepository
    {
        Task<IEnumerable<UnityPackage>> GetAllPackages();
        Task<UnityPackage> GetPackageById(int id);
        Task<UnityPackage> AddPackage(UnityPackage package);
        Task<UnityPackage> UpdatePackage(int id, UnityPackage package);
        Task<bool> DeletePackage(int id);
        Task<IEnumerable<UnityPackage>> GetAllPackagesByUserId(string userId);
    }

    public class UnityPackageRepository(ApplicationDbContext _db) : IUnityPackageRepository
    {
        public async Task<UnityPackage> AddPackage(UnityPackage package)
        {
            var addedPackage = await _db.UnityPackage.AddAsync(package);
            await _db.SaveChangesAsync();
            return addedPackage.Entity;
        }

        public async Task<bool> DeletePackage(int id)
        {
            // Retrieve the existing Package from the database
            var existingPackage = await GetPackageById(id);

            if (existingPackage == null)
                return false; // Package not found, return false

            // Remove the Package from the database
            _db.UnityPackage.Remove(existingPackage);

            // Save the changes to the database
            await _db.SaveChangesAsync();

            return true; // Deletion successful
        }

        public async Task<IEnumerable<UnityPackage>> GetAllPackages() => await _db.UnityPackage.Include(u => u.User).ToListAsync();

        public async Task<IEnumerable<UnityPackage>> GetAllPackagesByUserId(string userId)
            => await _db.UnityPackage
                .Include(u => u.User)
                    .Where(v => v.UserId == userId)
                        .ToListAsync();


        public async Task<UnityPackage> GetPackageById(int id)
        {
            var package = await _db.UnityPackage.Include(u => u.User).FirstOrDefaultAsync(v => v.Id == id)
                ?? throw new NotFoundException("Package not Found");
            
            if (package.User == null)
                throw new NotFoundException("User not found for the Package.");

            return package;
        }

        public async Task<UnityPackage> UpdatePackage(int id, UnityPackage package)
        {
            // Retrieve the existing Package from the database
            var existingPackage = await GetPackageById(id);

            // Update the existing Package's properties with the new values
            existingPackage.Title = package.Title;
            existingPackage.Description = package.Description;

            // Save the changes to the database
            await _db.SaveChangesAsync();

            return existingPackage;
        }
    }
}
