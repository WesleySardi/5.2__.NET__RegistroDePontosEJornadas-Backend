using ProjetoBMA.Domain.Enums;

namespace ProjetoBMA.DTOs.ViewModels
{
    public class TimeEntryViewModel
    {
        public Guid Id { get; set; }
        public string EmployeeId { get; set; } = null!;
        public string EmployeeName { get; set; } = null!;
        public DateTime Timestamp { get; set; }
        public TimeEntryType Type { get; set; }
        public string? Location { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
