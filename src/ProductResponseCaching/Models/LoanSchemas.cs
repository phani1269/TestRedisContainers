using System.ComponentModel.DataAnnotations;

namespace ProductResponseCaching.Models
{
    public class LoanSchemas
    {
        [Key]
        public int Id { get; set; }
        public string? SchemaName { get; set; }
        public string? Description { get; set; }
    }
}
