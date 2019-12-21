using System;

namespace Accounting
{
    public class Period
    {
        public Period(DateTime startDate, DateTime endDate)
        {
            StartDate = startDate;
            EndDate = endDate;
        }

        public DateTime StartDate { get; }
        public DateTime EndDate { get; }

        public int OverlappingDays(Period another)
        {
            var overlappingStart = StartDate > another.StartDate ? StartDate : another.StartDate;
            var overlappingEnd = EndDate < another.EndDate ? EndDate : another.EndDate;

            if (overlappingStart > overlappingEnd)
            {
                return 0;
            }

            return overlappingEnd.Subtract(overlappingStart).Days + 1;
        }
    }
}