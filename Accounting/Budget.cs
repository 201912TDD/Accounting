using System;
using System.Collections.Generic;
using System.Text;

namespace Accounting
{
    public class Budget
    {
        public string YearMonth { get; set; }
        public decimal Amount { get; set; }

        public decimal DailyAmount()
        {
            return Amount / DaysInBudget();
        }

        public int DaysInBudget()
        {
            return DateTime.DaysInMonth(FirstDay().Year, FirstDay().Month);
        }

        public DateTime LastDay()
        {
            var lastDay = DateTime.DaysInMonth(FirstDay().Year, FirstDay().Month);
            return DateTime.ParseExact(YearMonth + lastDay, "yyyyMMdd", null);
        }

        public DateTime FirstDay()
        {
            var firstDayOfBudget = DateTime.ParseExact(YearMonth + "01", "yyyyMMdd", null);
            return firstDayOfBudget;
        }
    }
}