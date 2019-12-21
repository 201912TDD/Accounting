using System;
using System.Collections.Generic;
using System.Text;

namespace Accounting
{
    public class Budget
    {
        public string YearMonth { get; set; }
        public decimal Amount { get; set; }

        public decimal OverlappingBudget(Period period)
        {
            return DailyAmount() * period.OverlappingDays(CreatePeriod());
        }

        private Period CreatePeriod()
        {
            return new Period(FirstDay(), LastDay());
        }

        private decimal DailyAmount()
        {
            return Amount / DaysInBudget();
        }

        private int DaysInBudget()
        {
            return DateTime.DaysInMonth(FirstDay().Year, FirstDay().Month);
        }

        private DateTime FirstDay()
        {
            return DateTime.ParseExact(YearMonth + "01", "yyyyMMdd", null);
        }

        private DateTime LastDay()
        {
            var lastDay = DateTime.DaysInMonth(FirstDay().Year, FirstDay().Month);
            return DateTime.ParseExact(YearMonth + lastDay, "yyyyMMdd", null);
        }
    }
}