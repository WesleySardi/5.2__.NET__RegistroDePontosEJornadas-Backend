using ProjetoBMA.Domain.Enums;

namespace ProjetoBMA.Utils
{
    public static class TimeEntryHelper
    {
        public static void EnsureValidType(TimeEntryType type)
        {
            if (!Enum.IsDefined(typeof(TimeEntryType), type))
            {
                throw new ArgumentException(
                    $"Tipo inválido. Valores permitidos: {string.Join(", ", Enum.GetNames(typeof(TimeEntryType)))}"
                );
            }
        }
    }
}
