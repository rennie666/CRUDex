using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUDex.Models.Entities
{
    public class Book
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string? Description { get; set; }
        public int AuthorId { get; set; } //foreign key

        [ForeignKey("AuthorId")]
        public virtual Author BookAuthor { get; set; } //navigation property - pomaga posle da se vizualizirat i dannite na avtora zaedno s knigata mu
    }
}
