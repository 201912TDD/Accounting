using System;
using System.Collections.Generic;
using System.Text;

namespace Accounting
{
    public class Budget
    {
        public string  YearMonth { get; set; }
        public decimal Amount { get; set; }

        public int DaysInBudget()
        {
            var firstDayOfBudget = DateTime.ParseExact(YearMonth + "01", "yyyyMMdd", null);

            return DateTime.DaysInMonth(firstDayOfBudget.Year, firstDayOfBudget.Month);
        }
    }
}
