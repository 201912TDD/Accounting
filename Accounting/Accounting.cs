using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Accounting
{
    public class Period
    {
        public Period(DateTime startDate, DateTime endDate)
        {
            StartDate = startDate;
            EndDate = endDate;
        }

        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }

        public int OverlappingDays(Period another)
        {
            var overlappingStart = StartDate > another.StartDate ? StartDate : another.StartDate;
            var overlappingEnd = EndDate < another.EndDate ? EndDate : another.EndDate;

            return overlappingEnd.Subtract(overlappingStart).Days + 1;
        }
    }

    class Accounting
    {
        public IBudgetRepo Repo { get; set; }

        public decimal QueryBudget(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
                return 0;

            var totalBudget = 0m;
            if (IsTheSameMonth(startDate, endDate))
            {
                var budget = FindBudget(startDate);
                if (budget != null)
                {
                    var overlappingDays = IntervalDays(startDate, endDate);
                    return budget.DailyAmount() * overlappingDays;
                }
            }

            var currentDate = new DateTime(startDate.Year, startDate.Month, 1);

            var period = new Period(startDate, endDate);
            while (currentDate <= endDate)
            {
                var budget = FindBudget(currentDate);
                if (budget != null)
                {
                    totalBudget += OverlappingBudget(budget, period);
                }

                currentDate = currentDate.AddMonths(1);
            }

            return totalBudget;
        }

        private static int IntervalDays(DateTime overlappingStart, DateTime overlappingEnd)
        {
            return overlappingEnd.Subtract(overlappingStart).Days + 1;
        }

        private static bool IsTheSameMonth(DateTime x, DateTime y)
        {
            return x.ToString("yyyyMM") == y.ToString("yyyyMM");
        }

        private static decimal OverlappingBudget(Budget budget, Period period)
        {
            return budget.DailyAmount() * period.OverlappingDays(budget.CreatePeriod());
        }

        private decimal BudgetOfMonth(DateTime startDate, int days)
        {
            var daysInMonth = DateTime.DaysInMonth(startDate.Year, startDate.Month);

            var budget = Repo.GetAll().FirstOrDefault(model => model.YearMonth == startDate.ToString("yyyyMM"));
            if (budget != null) return (decimal) budget.Amount / daysInMonth * days;
            return 0;
        }

        private Budget FindBudget(DateTime startDate)
        {
            var budget = Repo.GetAll().FirstOrDefault(model => model.YearMonth == startDate.ToString("yyyyMM"));
            return budget;
        }
    }
}