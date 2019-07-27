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

            Dictionary<string, int> days = Help.GetDaysInMonth(startDate, endDate);

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