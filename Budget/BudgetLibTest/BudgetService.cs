using System;
using System.Collections.Generic;
using System.Linq;

namespace BudgetLibTest
{
    public class BudgetService
    {
        private readonly IBudgetRepo _budgetRepo;

        public BudgetService(IBudgetRepo budgetRepo)
        {
            _budgetRepo = budgetRepo;
        }

        public double Query(DateTime startDate, DateTime endDate)
        {
            if ( IsInvalidDateRange(startDate, endDate))
            {
                return 0;
            }

            Dictionary<string, int> days = GetDaysInMonth(startDate, endDate);
            return _budgetRepo.GetAll().Sum(b => b.DailyAmount * days[b.YearMonth]);
        }

        private static bool IsInvalidDateRange(DateTime startDate, DateTime endDate)
        {
            return endDate < startDate;
        }

        public Dictionary<string, int> GetDaysInMonth(DateTime startDate, DateTime endDate)
        {
            Dictionary<string, int> lookup = new Dictionary<string, int>();

            var currentMonthFirstDay = new DateTime(startDate.Year, startDate.Month, 1);
            while (currentMonthFirstDay <= endDate)
            {
                var currentMonthLastDay = currentMonthFirstDay.AddMonths(1).AddDays(-1);
                var start = startDate > currentMonthFirstDay ? startDate : currentMonthFirstDay;
                var end = endDate < currentMonthLastDay ? endDate : currentMonthLastDay;
                var days = (end - start).Days + 1;

                lookup[currentMonthFirstDay.ToString("yyyyMM")] = days;

                currentMonthFirstDay = currentMonthFirstDay.AddMonths(1);
            }

            return lookup;
        }
    }
}