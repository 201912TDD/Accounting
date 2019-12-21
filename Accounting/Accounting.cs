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

            var period = new Period(startDate, endDate);

            return Repo.GetAll().Sum(budget => budget.OverlappingBudget(period));
        }
    }
}