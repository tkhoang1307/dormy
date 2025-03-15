namespace Dormy.WebService.Api.Core.Utilities
{
    public static class DateTimeHelper
    {
        public static bool AreValidStartDateEndDate(DateTime? startDate, DateTime? endDate, bool allowedEqual = false)
        {
            if (startDate.HasValue && endDate.HasValue)
            {
                return allowedEqual ? DateTime.Compare(endDate.Value, startDate.Value) >= 0 : DateTime.Compare(endDate.Value, startDate.Value) > 0;
            }
            return true;
        }

        public static bool IsEqual(this DateTime? date1, DateTime? date2, bool onlyDate = false)
        {
            var format = onlyDate ? "MM/dd/yyyy" : "MM/dd/yyyy HH:mm:ss";
            return date1.ToStringDateTimeFormat(format) == date2.ToStringDateTimeFormat(format);
        }

        private static string ToStringDateTimeFormat(this DateTime? dateVal, string format)
        {
            return dateVal.HasValue ? dateVal.Value.ToString(format) : null;
        }

        public static bool AreValidStartDateEndDateWithoutTime(DateTime? startDate, DateTime? endDate, bool allowedEqual = false)
        {
            if (startDate.HasValue && endDate.HasValue)
            {
                return allowedEqual ? DateTime.Compare(endDate.Value.Date, startDate.Value.Date) >= 0 : DateTime.Compare(endDate.Value.Date, startDate.Value.Date) > 0;
            }
            return true;
        }

        public static (int, int) CalculateThePreviousMonthYear(int currentMonth, int currentYear)
        {
            if (currentMonth == 1)
            {
                return (12, currentYear - 1);  // If January, previous month is December of the previous year
            }
            else
            {
                return (currentMonth - 1, currentYear);  // Otherwise, just subtract 1 from the current month
            }
        }
    }
}
