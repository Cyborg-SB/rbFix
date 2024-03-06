using QuickFix;
using QuickFix.Fields.Converters;

namespace RbFix.Infrastructure.Helpers
{
    internal static class QuickFixSessionsDicionaryExtensions
    {

        internal static string? GetStringSafe(this Dictionary currentSessionId, string property)
        {
            try
            {
                return currentSessionId.GetString(property);
            }
            catch (Exception)
            {
                return default;
            }

        }

        internal static bool? GetBoolSafe(this Dictionary currentSessionId, string property)
        {

            try
            {
                return currentSessionId.GetBool(property);
            }
            catch (Exception)
            {
                return default;
            }
        }

        internal static ulong? GetUlongSafe(this Dictionary currentSessionId, string property)
        {

            try
            {
                return currentSessionId.GetULong(property);
            }
            catch (Exception)
            {
                return default;
            }

        }

        internal static int? GetIntSsafe(this Dictionary currentSessionId, string property)
        {

            try
            {
                return currentSessionId.GetInt(property);
            }
            catch (Exception)
            {
                return default;
            }

        }

        internal static DayOfWeek? GetDaySafe(this Dictionary currentSessionId, string property)
        {

            try
            {
                return currentSessionId.GetDay(property);
            }

            catch (Exception)
            {
                return default;
            }

        }

        internal static double? GetDobuleSafe(this Dictionary currentSessionId, string property)
        {
            try
            {
                return currentSessionId.GetDouble(property);
            }

            catch (Exception)
            {
                return default;
            }

        }

        internal static TimeStampPrecision? GetTimeStampPrecisionSafe(this Dictionary currentSessionId, string property)
        {
            try
            {
                return currentSessionId.GetTimeStampPrecision(property);
            }

            catch (Exception)
            {
                return default;
            }

        }
    }
}
