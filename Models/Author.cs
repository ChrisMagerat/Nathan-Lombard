using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nathan.Models
{
    public class Author
    {
        [Key]
        [Column("AuthorId")]
        public Guid AuthorId { get; set; }

        [StringLength(100)]
        [Unicode(false)]
        public string? AuthorName { get; set; }

        public DateTime ActiveFrom { get; set; }

        public DateTime ActiveTo { get; set; }


        [Column("UserID")]
        [ForeignKey("UserModel")]
        public string? CreatedBy { get; set; }


    }
}
