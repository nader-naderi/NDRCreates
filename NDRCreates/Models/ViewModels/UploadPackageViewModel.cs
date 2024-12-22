namespace NDRCreates.Models.ViewModels
{
    public class UploadPackageViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }

        // Unity Package File .unitypackage or .zip
        public ICollection<IFormFile> PackageFiles { get; set; }

        // Package Thumbnail .png etc
        public ICollection<IFormFile> ThumbnailFiles { get; set; }

        public string Version { get; set; } = "1.0.0";
    }

    public class EditPackageViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }
    }
}
