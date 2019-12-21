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

            var budget = 0m;
            if (startDate.Year == endDate.Year && startDate.Month == endDate.Month)
            {
                var days = endDate.Subtract(startDate).Days + 1;
                return BudgetOfMonth(startDate, days);
            }

            var currentDate = new DateTime(startDate.Year, startDate.Month, 1);

            var i = 0;

            while (true)
            {
                if (currentDate > endDate)
                    break;
                if (i == 0)
                {
                    budget += BudgetOfMonth(startDate,
                                            DateTime.DaysInMonth(startDate.Year, startDate.Month) - startDate.Day + 1);
                }
                else if (currentDate.Year == endDate.Year && currentDate.Month == endDate.Month)
                {
                    budget += BudgetOfMonth(endDate, endDate.Day);
                }
                else
                {
                    budget += BudgetOfMonth(currentDate, DateTime.DaysInMonth(currentDate.Year, currentDate.Month));
                }

                currentDate = currentDate.AddMonths(1);
                i++;
            }

            return budget;
        }

        private decimal BudgetOfMonth(DateTime startDate, int days)
        {
            var daysInMonth = DateTime.DaysInMonth(startDate.Year, startDate.Month);

            var budget = Repo.GetAll().FirstOrDefault(model => model.YearMonth == startDate.ToString("yyyyMM"));
            if (budget != null) return (decimal) budget.Amount / daysInMonth * days;
            return 0;
        }
    }
}