using ProjetoBMA.Domain.Enums;

namespace ProjetoBMA.DTOs.Queries
{
    public class TimeEntryQueryParametersQuery
    {
        public string? EmployeeId { get; set; }
        public string? Type { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public string? Sort { get; set; }
    }
}
