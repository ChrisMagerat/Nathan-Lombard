using System.ComponentModel.DataAnnotations.Schema;

namespace Nathan.ViewModels
{
    public class AuthorViewModel
    {
        public string? name { get; set; }

        public DateTime activeFrom { get; set; }

        public DateTime activeTo { get; set; }

    }
}
