using System.ComponentModel.DataAnnotations;

namespace Ngay6_KTPMK22B.Models
{
    public class Genre
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string? Name { get; set; }

        // Navigation property for one-to-many relationship
        public ICollection<Movie> Movies { get; set; } = new List<Movie>();
    }
}
