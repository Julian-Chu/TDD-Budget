using System;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;

namespace BudgetLibTest
{
    [TestFixture]
    public class BudgetServiceTest
    {
        private BudgetService _budgetService;
        private IBudgetRepo _stubBudgetRepo;

        [SetUp]
        public void Setup()
        {
            _stubBudgetRepo = Substitute.For<IBudgetRepo>();
            _budgetService = new BudgetService(_stubBudgetRepo);
        }

        [Test]
        public void Invalid_DateRange()
        {
            BudgetShouldBe(new DateTime(2019, 7, 29), new DateTime(2019, 7, 28), 0);
        }

        [Test]
        public void Valid_DateRange()
        {
            BudgetShouldBe(new DateTime(2019, 7, 27), new DateTime(2019, 7, 28), 0);
        }

        [Test]
        public void SingleWholeMonth_HasBudget_()
        {
            _stubBudgetRepo.GetAll()
                .Returns(new List<Budget>
                {
                    new Budget {YearMonth = "201907", Amount = 3100}
                });

            BudgetShouldBe(new DateTime(2019, 7, 1), new DateTime(2019, 7, 31), 3100);
        }

        [Test]
        public void SingleWholeMonth_HasBudget_zero()
        {
            _stubBudgetRepo.GetAll()
                .Returns(new List<Budget>
                {
                    new Budget {YearMonth = "201907", Amount = 0}
                });

            BudgetShouldBe(new DateTime(2019, 7, 1), new DateTime(2019, 7, 31), 0);
        }

        [Test]
        public void SingleWholeMonth_NoBudget_()
        {
            _stubBudgetRepo.GetAll()
                .Returns(new List<Budget>
                {
                });

            BudgetShouldBe(new DateTime(2019, 7, 1), new DateTime(2019, 7, 31), 0);
        }

        [Test]
        public void SingleDay_Budget_100()
        {
            _stubBudgetRepo.GetAll()
                .Returns(new List<Budget>
                {
                    new Budget {YearMonth = "201907", Amount = 3100}
                });

            BudgetShouldBe(new DateTime(2019, 7, 1), new DateTime(2019, 7, 1), 100);
        }

        [Test]
        public void DaysInSameMonth_Budget_500()
        {
            _stubBudgetRepo.GetAll()
                .Returns(new List<Budget>
                {
                    new Budget {YearMonth = "201907", Amount = 3100}
                });

            BudgetShouldBe(new DateTime(2019, 7, 1), new DateTime(2019, 7, 5), 500);
        }

        [Test]
        public void TwoMonth_Budget_300()
        {
            _stubBudgetRepo.GetAll()
                .Returns(new List<Budget>
                {
                    new Budget {YearMonth = "201907", Amount = 3100},
                    new Budget {YearMonth = "201908", Amount = 3100}
            });

            BudgetShouldBe(new DateTime(2019, 7, 1), new DateTime(2019, 8, 2), 3300);
        }

        [Test]
        public void TwoMonth_Budget_6200()
        {
            _stubBudgetRepo.GetAll()
                .Returns(new List<Budget>
                {
                    new Budget {YearMonth = "201907", Amount = 3100},
                    new Budget {YearMonth = "201908", Amount = 3100}
            });

            BudgetShouldBe(new DateTime(2019, 7, 1), new DateTime(2019, 8, 31), 6200);
        }

        [Test]
        public void crossYear()
        {
            _stubBudgetRepo.GetAll()
                .Returns(new List<Budget>
                {
                    new Budget {YearMonth = "201812", Amount = 310},
                    new Budget {YearMonth = "201901", Amount = 3100}
            });

            BudgetShouldBe(new DateTime(2018, 12, 1), new DateTime(2019, 1, 15), 1810);
        }

        private void BudgetShouldBe(DateTime startDate, DateTime endDate, double expected)
        {
            var actual = _budgetService.Query(startDate, endDate);
            Assert.AreEqual(expected, actual);
        }
    }

    public interface IBudgetRepo
    {
        List<Budget> GetAll();
    }

    public class Budget
    {
        public string YearMonth { get; set; }
        public int Amount { get; set; }
    }
}