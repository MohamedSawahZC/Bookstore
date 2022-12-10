using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Models
{
    public class Book
    {
        
        public int Id { get; set; }
        public string Title { get; set; }
        [MaxLength(120)]
        public string Description { get; set; }

        public string ImageUrl { get; set; }
        public Author Author { get; set; }

    }
}
