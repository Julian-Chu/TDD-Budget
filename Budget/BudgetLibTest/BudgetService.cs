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

            var budgets = _budgetRepo.GetAll() ?? new List<Budget>();

            var budgetModels = budgets.Select(x => new BudgetTemp()
            {
                BudgetDate = DateTime.ParseExact(x.YearMonth, "yyyyMM", null),
                Amount = x.Amount,
            });

            if (startDate.Month == endDate.Month && startDate.Year == endDate.Year)
            {
                //int days = DateTime.DaysInMonth(startDate.Year, startDate.Month);
                return ((endDate - startDate).Days + 1) * budgetModels.FirstOrDefault()?.DailyAmount ?? 0;
            }

            return budgets.FirstOrDefault()?.Amount ?? 0;
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