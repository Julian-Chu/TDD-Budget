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

            return budgets.FirstOrDefault()?.Amount ?? 0;
        }

        private static bool IsInvalidDateRange(DateTime startDate, DateTime endDate)
        {
            return endDate < startDate;
        }
    }
}