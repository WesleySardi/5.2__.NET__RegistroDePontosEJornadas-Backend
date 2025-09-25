using ProjetoBMA.Domain.Enums;

namespace ProjetoBMA.Utils
{
    public static class TimeEntryHelper
    {
        public static void EnsureValidType(string type)
        {
            var validNames = Enum.GetNames(typeof(TimeEntryType));

            if (!validNames.Contains(type, StringComparer.OrdinalIgnoreCase))
            {
                throw new ArgumentException(
                    $"Tipo inválido. Valores permitidos: {string.Join(", ", validNames)}"
                );
            }
        }
    }
}
