using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Nathan.ViewModels
{
    public class BookViewModel
    {
        public string? bookName { get; set; }

        public string? publisher { get; set; }

        public DateTime datePublished { get; set; }

        public int copiesSold { get; set; }
    }
}
