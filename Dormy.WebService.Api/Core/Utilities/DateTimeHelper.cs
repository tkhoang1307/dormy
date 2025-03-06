namespace Dormy.WebService.Api.Core.Utilities
{
    public class DateTimeHelper
    {
        public (int, int) CalculateThePreviousMonthYear(int currentMonth, int currentYear)
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
