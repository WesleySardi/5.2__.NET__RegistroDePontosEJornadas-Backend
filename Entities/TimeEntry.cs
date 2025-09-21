using System.ComponentModel.DataAnnotations;

namespace ProjetoBMA.Entities
{
    public class TimeEntry
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string EmployeeId { get; set; } = null!;

        [Required]
        [MaxLength(200)]
        public string EmployeeName { get; set; } = null!;

        [Required]
        public DateTime Timestamp { get; set; }

        [Required]
        [MaxLength(20)]
        public string Type { get; set; } = null!;

        [MaxLength(100)]
        public string? Location { get; set; }

        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
