using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Nathan.Models
{
    public class Book
    {

        [Key]
        [Column("BookId")]
        public Guid BookId { get; set; }

        [StringLength(100)]
        [Unicode(false)]
        public string? BookName { get; set; }

        [StringLength(100)]
        [Unicode(false)]
        public string? Publisher { get; set; }

        public DateTime DatePublished { get; set; }

        public int CopiesSold { get; set; }

        [Column("AuthorID")]
        [ForeignKey("Author")]
        public string? Author { get; set; }

        [Column("UserID")]
        [ForeignKey("UserModel")]
        public string? CreatedBy { get; set; }
    }
}
