using System.ComponentModel.DataAnnotations;

namespace DecorusWeb.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public int DIsplayOrder { get; set; }
        public DateTime CreatedDayTime { get; set; } = DateTime.Now;
    }
}
