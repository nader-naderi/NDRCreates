using Microsoft.EntityFrameworkCore.Metadata.Internal;

using NDRCreates.Models.Entities;

using System.ComponentModel.DataAnnotations;

namespace NDRCreates.Models.DisplayModels
{
    public class FeedDisplayModel
    {
        public List<UnityPackage> Packages { get; set; }
    }

    public class PackageDisplayModel
    {
        public string Title { get; set; }

        public string ThumbnailUrl { get; set; }

        [Required]
        [StringLength(100)]
        public string UserId { get; set; }

        public BasicUser User { get; set; }

        [Required]
        public DateTime PublicationDate { get; set; }

        public int Duration { get; set; }

        public int Views { get; set; }

        public int Likes { get; set; }

        public int Dislikes { get; set; }
    }
}
