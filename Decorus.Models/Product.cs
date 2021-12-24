using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decorus.Models
{
    public class Product
    {

        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public string ISBN { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        [Range(1, 10000)]
        public double Price { get; set; }
        [Required]
        [Range(1, 10000)]
        public double ListPrice { get; set; }

        [Required]
        [Range(1, 10000)]
        public double Price50 { get; set; }

        [Required]
        [Range(1, 10000)]
        public double Price100 { get; set; }

        public string ImageUrl { get; set; }

        // Will automatically make CategoryId a foreign key
        [Required]
        public int CategoryId { get; set; }
        // Can explicitly use ForeignKey tag, but not needed. Only if the name is different, it will not automatically map.
        // When Id is appended 
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

        // Will automatically make CoverTypeId a foreign key
        [Required]
        public int CoverTypeId { get; set; }
        public CoverType CoverType { get; set; }

    }
}
