namespace ProjetoBMA.DTOs
{
    public class TimeEntryQueryParameters
    {
        public string? EmployeeId { get; set; }
        public string? Type { get; set; } // Entrada, Saída, Intervalo
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }

        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;

        /// <summary>
        /// ex: timestamp_desc, timestamp_asc, employeeName_asc
        /// </summary>
        public string? Sort { get; set; }
    }
}
