using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Accounting
{
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
                var budget = Repo.GetAll().FirstOrDefault(b => b.YearMonth == startDate.ToString("yyyyMM"));
                if (budget != null)
                {
                    var overlappingDays = OverlappingDays(startDate, endDate);
                    return budget.DailyAmount() * overlappingDays;
                }
            }

            var currentDate = new DateTime(startDate.Year, startDate.Month, 1);

            while (currentDate <= endDate)
            {
                if (IsTheSameMonth(startDate, currentDate))
                {
                    var budget = FindBudget(currentDate);
                    if (budget != null)
                    {
                        var overlappingDays = OverlappingDays(startDate, budget.LastDay());
                        totalBudget += budget.DailyAmount() * overlappingDays;
                    }
                }
                else if (IsTheSameMonth(endDate, currentDate))
                {
                    var budget = Repo.GetAll().FirstOrDefault(model => model.YearMonth == endDate.ToString("yyyyMM"));
                    if (budget != null)
                    {
                        var overlappingDays = OverlappingDays(budget.FirstDay(), endDate);
                        totalBudget += budget.DailyAmount() * overlappingDays;
                    }
                }
                else
                {
                    int days = DateTime.DaysInMonth(currentDate.Year, currentDate.Month);
                    var daysInMonth = DateTime.DaysInMonth(currentDate.Year, currentDate.Month);

                    var budget = Repo.GetAll().FirstOrDefault(model => model.YearMonth == currentDate.ToString("yyyyMM"));
                    if (budget != null) totalBudget += (decimal) budget.Amount / daysInMonth * days;
                    else
                    {
                        totalBudget += 0;
                    }
                }

                currentDate = currentDate.AddMonths(1);
            }

            return totalBudget;
        }

        private static bool IsTheSameMonth(DateTime x, DateTime y)
        {
            return x.ToString("yyyyMM") == y.ToString("yyyyMM");
        }

        private static int OverlappingDays(DateTime overlappingStart, DateTime overlappingEnd)
        {
            return overlappingEnd.Subtract(overlappingStart).Days + 1;
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