using ProjetoBMA.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace ProjetoBMA.DTOs.Commands
{
    public class UpdateTimeEntryCommand
    {
        [Required(ErrorMessage = "EmployeeId é obrigatório.")]
        [MaxLength(50)]
        public string EmployeeId { get; set; } = null!;

        [Required(ErrorMessage = "EmployeeName é obrigatório.")]
        [MaxLength(200)]
        public string EmployeeName { get; set; } = null!;

        [Required(ErrorMessage = "Timestamp é obrigatório.")]
        public DateTime Timestamp { get; set; }

        [Required(ErrorMessage = "Type é obrigatório.")]
        public TimeEntryType Type { get; set; }

        [MaxLength(100)]
        public string? Location { get; set; }

        public string? Notes { get; set; }
    }
}
