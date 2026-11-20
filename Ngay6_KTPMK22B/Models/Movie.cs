using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ngay6_KTPMK22B.Models
{
    public class Movie
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 3)]
        public string? Title { get; set; }

        [Required]
        public DateTime ReleaseDate { get; set; }

        [Range(0, 500)]
        public int Duration { get; set; }

        // Foreign key
        [ForeignKey("Genre")]
        [Required(ErrorMessage = "Please select a Genre")]
        public int GenreId { get; set; }

        // Navigation property
        public Genre? Genre { get; set; }
    }
}
