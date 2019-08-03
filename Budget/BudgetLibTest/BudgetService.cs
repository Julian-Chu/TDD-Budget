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
            if (IsInvalidDateRange(startDate, endDate))
            {
                return 0;
            }

            Dictionary<string, int> days = GetDaysInMonth(startDate, endDate);

            var budgets = _budgetRepo.GetAll() ?? new List<Budget>();

            var budgetModels = budgets.Select(x => new BudgetTemp()
            {
                BudgetDate = DateTime.ParseExact(x.YearMonth, "yyyyMM", null),
                Amount = x.Amount,
            });
            double sum = 0;
            foreach (var kv in days)
            {
                var b = budgetModels.FirstOrDefault(x => x.BudgetDate.ToString("yyyyMM") == kv.Key);
                var dailyAmout = b?.DailyAmount ?? 0;
                sum += (kv.Value) * (dailyAmout);
            }

            return sum;
        }

        private static bool IsInvalidDateRange(DateTime startDate, DateTime endDate)
        {
            return endDate < startDate;
        }

        public  Dictionary<string, int> GetDaysInMonth(DateTime startDate, DateTime endDate)
        {
            Dictionary<string, int> lookup = new Dictionary<string, int>();
            if (startDate.Year == endDate.Year && startDate.Month == endDate.Month)
            {
                lookup.Add(startDate.ToString("yyyyMM"), (endDate - startDate).Days + 1);
                return lookup;
            }

            var start = new DateTime(startDate.Year, startDate.Month, 1);
            while (start <= endDate)
            {
                var days = DateTime.DaysInMonth(start.Year, start.Month);
                if (start.Month == startDate.Month && start.Year == startDate.Year)
                {
                    days = days - startDate.Day + 1;
                }
                if (start.Month == endDate.Month && start.Year == endDate.Year)
                {
                    days = endDate.Day;
                }

                lookup[start.ToString("yyyyMM")] = days;

                start = start.AddMonths(1);
            }

            return lookup;
        }
    }

    public class BudgetTemp
    {
        public DateTime BudgetDate { get; set; }
        public int Amount { get; set; }

        public double DailyAmount
        {
            get
            {
                return Amount / DateTime.DaysInMonth(BudgetDate.Year, BudgetDate.Month);
            }
        }
    }
}