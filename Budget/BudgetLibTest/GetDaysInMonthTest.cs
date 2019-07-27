using System;
using System.Collections.Generic;
using ExpectedObjects;
using NUnit.Framework;

namespace BudgetLibTest
{
    [TestFixture]
    public class GetDaysInMonthTest
    {
        private Help _help;

        [SetUp]
        public void Setup()
        {
            _help = new Help();
        }

        [Test]
        public void WholeMonth()
        {
            var daysInMonth = _help.GetDaysInMonth(new DateTime(2019, 01, 01), new DateTime(2019, 01, 31));
            var expected = new Dictionary<string, int>()
            {
                {"201901",31 }
            };
            expected.ToExpectedObject().ShouldEqual(daysInMonth);
        }

        [Test]
        public void PartialMonth()
        {
            var daysInMonth = _help.GetDaysInMonth(new DateTime(2019, 01, 01), new DateTime(2019, 01, 07));
            var expected = new Dictionary<string, int>()
            {
                {"201901",7 }
            };
            expected.ToExpectedObject().ShouldEqual(daysInMonth);
        }

        [Test]
        public void TwoMonth()
        {
            var daysInMonth = _help.GetDaysInMonth(new DateTime(2019, 01, 01), new DateTime(2019, 02, 07));
            var expected = new Dictionary<string, int>()
            {
                {"201901",31 },
                {"201902",7 },
            };
            expected.ToExpectedObject().ShouldEqual(daysInMonth);
        }

        [Test]
        public void CrossYear()
        {
            var daysInMonth = _help.GetDaysInMonth(new DateTime(2018, 11, 15), new DateTime(2019, 02, 07));
            var expected = new Dictionary<string, int>()
            {
                {"201811",16 },
                {"201812",31 },
                {"201901",31 },
                {"201902",7 },
            };
            expected.ToExpectedObject().ShouldEqual(daysInMonth);
        }
    }
}