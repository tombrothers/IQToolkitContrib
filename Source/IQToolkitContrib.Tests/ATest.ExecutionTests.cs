using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IQToolkitContrib.Tests.Entities;

namespace IQToolkitContrib.Tests {
    public abstract partial class ATest {
        public virtual void TestWhere() {
            Assert.AreEqual(6, this.GetDataContext().List<Customer>().Where(c => c.City == "London").Count());
        }

        public virtual void TestWhereTrue() {
            Assert.AreEqual(91, this.GetDataContext().List<Customer>().Where(c => true).Count());
        }

        public virtual void TestCompareEntityEqual() {
            Customer customer = new Customer { CustomerId = "ALFKI" };
            var list = this.GetDataContext().List<Customer>().Where(c => c == customer).ToList();

            Assert.AreEqual(1, list.Count);
            Assert.AreEqual("ALFKI", list[0].CustomerId);
        }

        public virtual void TestCompareEntityNotEqual() {
            Customer customer = new Customer { CustomerId = "ALFKI" };
            Assert.AreEqual(90, this.GetDataContext().List<Customer>().Where(c => c != customer).Count());
        }

        public virtual void TestCompareConstructedEqual() {
            var query = this.GetDataContext().List<Customer>().Where(c => new { x = c.City } == new { x = "London" });
            var count = query.Count();

            Assert.AreEqual(6, count);
        }

        public virtual void TestCompareConstructedMultiValueEqual() {
            Assert.AreEqual(6, this.GetDataContext().List<Customer>().Where(c => new { x = c.City, y = c.Country } == new { x = "London", y = "UK" }).Count());
        }

        public virtual void TestCompareConstructedMultiValueNotEqual() {
            Assert.AreEqual(85, this.GetDataContext().List<Customer>().Where(c => new { x = c.City, y = c.Country } != new { x = "London", y = "UK" }).Count());
        }

        public virtual void TestSelectScalar() {
            var list = this.GetDataContext().List<Customer>().Where(c => c.City == "London").Select(c => c.City).ToList();

            Assert.AreEqual(6, list.Count);
            Assert.AreEqual("London", list[0]);
            Assert.IsTrue(list.All(x => x == "London"));
        }

        public virtual void TestSelectAnonymousOne() {
            var list = this.GetDataContext().List<Customer>().Where(c => c.City == "London").Select(c => new { c.City }).ToList();

            Assert.AreEqual(6, list.Count);
            Assert.AreEqual("London", list[0].City);
            Assert.IsTrue(list.All(x => x.City == "London"));
        }

        public virtual void TestSelectAnonymousTwo() {
            var list = this.GetDataContext().List<Customer>().Where(c => c.City == "London").Select(c => new { c.City, c.Phone }).ToList();

            Assert.AreEqual(6, list.Count);
            Assert.AreEqual("London", list[0].City);
            Assert.IsTrue(list.All(x => x.City == "London"));
            Assert.IsTrue(list.All(x => x.Phone != null));
        }

        public virtual void TestSelectCustomerTable() {
            var list = this.GetDataContext().List<Customer>().ToList();
            Assert.AreEqual(91, list.Count);
        }

        public virtual void TestSelectAnonymousWithObject() {
            var list = this.GetDataContext().List<Customer>().Where(c => c.City == "London").Select(c => new { c.City, c }).ToList();

            Assert.AreEqual(6, list.Count);
            Assert.AreEqual("London", list[0].City);
            Assert.IsTrue(list.All(x => x.City == "London"));
            Assert.IsTrue(list.All(x => x.c.City == x.City));
        }

        public virtual void TestSelectAnonymousLiteral() {
            var list = this.GetDataContext().List<Customer>().Where(c => c.City == "London").Select(c => new { X = 10 }).ToList();

            Assert.AreEqual(6, list.Count);
            Assert.IsTrue(list.All(x => x.X == 10));
        }

        public virtual void TestSelectConstantInt() {
            var list = this.GetDataContext().List<Customer>().Select(c => 10).ToList();

            Assert.AreEqual(91, list.Count);
            Assert.IsTrue(list.All(x => x == 10));
        }

        public virtual void TestSelectConstantNullString() {
            var list = this.GetDataContext().List<Customer>().Select(c => (string)null).ToList();

            Assert.AreEqual(91, list.Count);
            Assert.IsTrue(list.All(x => x == null));
        }

        public virtual void TestSelectLocal() {
            int x = 10;
            var list = this.GetDataContext().List<Customer>().Select(c => x).ToList();

            Assert.AreEqual(91, list.Count);
            Assert.IsTrue(list.All(y => y == 10));
        }

        public virtual void TestSelectNestedCollection() {
            DataContext context = this.GetDataContext();

            var list = (
                from c in context.List<Customer>()
                where c.CustomerId == "ALFKI"
                select context.List<Order>().Where(o => o.CustomerId == c.CustomerId).Select(o => o.OrderId)
                ).ToList();

            Assert.AreEqual(1, list.Count);
            Assert.AreEqual(6, list[0].Count());
        }

        public virtual void TestSelectNestedCollectionInAnonymousType() {
            DataContext context = this.GetDataContext();

            var list = (
                from c in context.List<Customer>()
                where c.CustomerId == "ALFKI"
                select new { Foos = context.List<Order>().Where(o => o.CustomerId == c.CustomerId).Select(o => o.OrderId).ToList() }
                ).ToList();

            Assert.AreEqual(1, list.Count);
            Assert.AreEqual(6, list[0].Foos.Count);
        }

        public virtual void TestJoinCustomerOrders() {
            DataContext context = this.GetDataContext();

            var list = (
                from c in context.List<Customer>()
                where c.CustomerId == "ALFKI"
                join o in context.List<Order>() on c.CustomerId equals o.CustomerId
                select new { c.ContactName, o.OrderId }
                ).ToList();

            Assert.AreEqual(6, list.Count);
        }

        public virtual void TestJoinMultiKey() {
            DataContext context = this.GetDataContext();

            var list = (
                from c in context.List<Customer>()
                where c.CustomerId == "ALFKI"
                join o in context.List<Order>() on new { a = c.CustomerId, b = c.CustomerId } equals new { a = o.CustomerId, b = o.CustomerId }
                select new { c, o }
                ).ToList();

            Assert.AreEqual(6, list.Count);
        }

        public virtual void TestJoinIntoCustomersOrdersCount() {
            DataContext context = this.GetDataContext();

            var list = (
                from c in context.List<Customer>()
                where c.CustomerId == "ALFKI"
                join o in context.List<Order>() on c.CustomerId equals o.CustomerId into ords
                select new { cust = c, ords = ords.Count() }
                ).ToList();

            Assert.AreEqual(1, list.Count);
            Assert.AreEqual(6, list[0].ords);
        }

        public virtual void TestJoinIntoDefaultIfEmpty() {
            DataContext context = this.GetDataContext();

            var list = (
                from c in context.List<Customer>()
                where c.CustomerId == "PARIS"
                join o in context.List<Order>() on c.CustomerId equals o.CustomerId into ords
                from o in ords.DefaultIfEmpty()
                select new { c, o }
                ).ToList();

            Assert.AreEqual(1, list.Count);
            Assert.AreEqual(null, list[0].o);
        }

        public virtual void TestMultipleJoinsWithJoinConditionsInWhere() {
            DataContext context = this.GetDataContext();

            // this should reduce to inner joins
            var list = (
                from c in context.List<Customer>()
                where c.CustomerId == "ALFKI"
                from o in context.List<Order>()
                from d in context.List<OrderDetail>()
                where o.CustomerId == c.CustomerId && o.OrderId == d.OrderId
                select d
                ).ToList();

            Assert.AreEqual(12, list.Count);
        }

        public virtual void TestOrderBy() {
            var list = this.GetDataContext().List<Customer>().OrderBy(c => c.CustomerId).Select(c => c.CustomerId).ToList();
            var sorted = list.OrderBy(c => c).ToList();

            Assert.AreEqual(91, list.Count);
            Assert.IsTrue(Enumerable.SequenceEqual(list, sorted));
        }

        public virtual void TestOrderByOrderBy() {
            var list = this.GetDataContext().List<Customer>().OrderBy(c => c.Phone).OrderBy(c => c.CustomerId).ToList();
            var sorted = list.OrderBy(c => c.CustomerId).ToList();

            Assert.AreEqual(91, list.Count);
            Assert.IsTrue(Enumerable.SequenceEqual(list, sorted));
        }

        public virtual void TestOrderByThenBy() {
            var list = this.GetDataContext().List<Customer>().OrderBy(c => c.CustomerId).ThenBy(c => c.Phone).ToList();
            var sorted = list.OrderBy(c => c.CustomerId).ThenBy(c => c.Phone).ToList();

            Assert.AreEqual(91, list.Count);
            Assert.IsTrue(Enumerable.SequenceEqual(list, sorted));
        }

        public virtual void TestOrderByDescending() {
            var list = this.GetDataContext().List<Customer>().OrderByDescending(c => c.CustomerId).ToList();
            var sorted = list.OrderByDescending(c => c.CustomerId).ToList();

            Assert.AreEqual(91, list.Count);
            Assert.IsTrue(Enumerable.SequenceEqual(list, sorted));
        }

        public virtual void TestOrderByDescendingThenBy() {
            var list = this.GetDataContext().List<Customer>().OrderByDescending(c => c.CustomerId).ThenBy(c => c.Country).ToList();
            var sorted = list.OrderByDescending(c => c.CustomerId).ThenBy(c => c.Country).ToList();

            Assert.AreEqual(91, list.Count);
            Assert.IsTrue(Enumerable.SequenceEqual(list, sorted));
        }

        public virtual void TestOrderByDescendingThenByDescending() {
            var list = this.GetDataContext().List<Customer>().OrderByDescending(c => c.CustomerId).ThenByDescending(c => c.Country).ToList();
            var sorted = list.OrderByDescending(c => c.CustomerId).ThenByDescending(c => c.Country).ToList();

            Assert.AreEqual(91, list.Count);
            Assert.IsTrue(Enumerable.SequenceEqual(list, sorted));
        }

        public virtual void TestOrderByJoin() {
            DataContext context = this.GetDataContext();

            var list = (
                from c in context.List<Customer>().OrderBy(c => c.CustomerId)
                join o in context.List<Order>().OrderBy(o => o.OrderId) on c.CustomerId equals o.CustomerId
                select new { c.CustomerId, o.OrderId }
                ).ToList();

            var sorted = list.OrderBy(x => x.CustomerId).ThenBy(x => x.OrderId);

            Assert.IsTrue(Enumerable.SequenceEqual(list, sorted));
        }

        public virtual void TestOrderBySelectMany() {
            DataContext context = this.GetDataContext();

            var list = (
                from c in context.List<Customer>().OrderBy(c => c.CustomerId)
                from o in context.List<Order>().OrderBy(o => o.OrderId)
                where c.CustomerId == o.CustomerId
                select new { c.CustomerId, o.OrderId }
                ).ToList();

            var sorted = list.OrderBy(x => x.CustomerId).ThenBy(x => x.OrderId).ToList();

            Assert.IsTrue(Enumerable.SequenceEqual(list, sorted));
        }

        public virtual void TestGroupBy() {
            Assert.AreEqual(69, this.GetDataContext().List<Customer>().GroupBy(c => c.City).ToList().Count);
        }

        public virtual void TestGroupByOne() {
            var list = this.GetDataContext().List<Customer>().Where(c => c.City == "London").GroupBy(c => c.City).ToList();

            Assert.AreEqual(1, list.Count);
            Assert.AreEqual(6, list[0].Count());
        }

        public virtual void TestGroupBySelectMany() {
            var list = this.GetDataContext().List<Customer>().GroupBy(c => c.City).SelectMany(g => g).ToList();
            Assert.AreEqual(91, list.Count);
        }

        public virtual void TestGroupBySum() {
            var list = this.GetDataContext().List<Order>().Where(o => o.CustomerId == "ALFKI").GroupBy(o => o.CustomerId).Select(g => g.Sum(o => (o.CustomerId == "ALFKI" ? 1 : 1))).ToList();

            Assert.AreEqual(1, list.Count);
            Assert.AreEqual(6, list[0]);
        }

        public virtual void TestGroupByCount() {
            var list = this.GetDataContext().List<Order>().Where(o => o.CustomerId == "ALFKI").GroupBy(o => o.CustomerId).Select(g => g.Count()).ToList();

            Assert.AreEqual(1, list.Count);
            Assert.AreEqual(6, list[0]);
        }

        public virtual void TestGroupByLongCount() {
            var list = this.GetDataContext().List<Order>().Where(o => o.CustomerId == "ALFKI").GroupBy(o => o.CustomerId).Select(g => g.LongCount()).ToList();

            Assert.AreEqual(1, list.Count);
            Assert.AreEqual(6L, list[0]);
        }

        public virtual void TestGroupBySumMinMaxAvg() {
            var list =
                this.GetDataContext().List<Order>().Where(o => o.CustomerId == "ALFKI").GroupBy(o => o.CustomerId).Select(g =>
                    new {
                        Sum = g.Sum(o => (o.CustomerId == "ALFKI" ? 1 : 1)),
                        Min = g.Min(o => o.OrderId),
                        Max = g.Max(o => o.OrderId),
                        Avg = g.Average(o => o.OrderId)
                    }).ToList();

            Assert.AreEqual(1, list.Count);
            Assert.AreEqual(6, list[0].Sum);
        }

        public virtual void TestGroupByWithResultSelector() {
            var list =
                this.GetDataContext().List<Order>().Where(o => o.CustomerId == "ALFKI").GroupBy(o => o.CustomerId, (k, g) =>
                    new {
                        Sum = g.Sum(o => (o.CustomerId == "ALFKI" ? 1 : 1)),
                        Min = g.Min(o => o.OrderId),
                        Max = g.Max(o => o.OrderId),
                        Avg = g.Average(o => o.OrderId)
                    }).ToList();

            Assert.AreEqual(1, list.Count);
            Assert.AreEqual(6, list[0].Sum);
        }

        public virtual void TestGroupByWithElementSelectorSum() {
            var list = this.GetDataContext().List<Order>().Where(o => o.CustomerId == "ALFKI").GroupBy(o => o.CustomerId, o => (o.CustomerId == "ALFKI" ? 1 : 1)).Select(g => g.Sum()).ToList();

            Assert.AreEqual(1, list.Count);
            Assert.AreEqual(6, list[0]);
        }

        public virtual void TestGroupByWithElementSelector() {
            // note: groups are retrieved through a separately execute subquery per row
            var list = this.GetDataContext().List<Order>().Where(o => o.CustomerId == "ALFKI").GroupBy(o => o.CustomerId, o => (o.CustomerId == "ALFKI" ? 1 : 1)).ToList();

            Assert.AreEqual(1, list.Count);
            Assert.AreEqual(6, list[0].Count());
            Assert.AreEqual(6, list[0].Sum());
        }

        public virtual void TestGroupByWithElementSelectorSumMax() {
            var list = this.GetDataContext().List<Order>().Where(o => o.CustomerId == "ALFKI").GroupBy(o => o.CustomerId, o => (o.CustomerId == "ALFKI" ? 1 : 1)).Select(g => new { Sum = g.Sum(), Max = g.Max() }).ToList();

            Assert.AreEqual(1, list.Count);
            Assert.AreEqual(6, list[0].Sum);
            Assert.AreEqual(1, list[0].Max);
        }

        public virtual void TestGroupByWithAnonymousElement() {
            var list = this.GetDataContext().List<Order>().Where(o => o.CustomerId == "ALFKI").GroupBy(o => o.CustomerId, o => new { X = (o.CustomerId == "ALFKI" ? 1 : 1) }).Select(g => g.Sum(x => x.X)).ToList();

            Assert.AreEqual(1, list.Count);
            Assert.AreEqual(6, list[0]);
        }

        public virtual void TestGroupByWithTwoPartKey() {
            var list = this.GetDataContext().List<Order>().Where(o => o.CustomerId == "ALFKI").GroupBy(o => new { o.CustomerId, o.OrderDate }).Select(g => g.Sum(o => (o.CustomerId == "ALFKI" ? 1 : 1))).ToList();

            Assert.AreEqual(6, list.Count);
        }

        public virtual void TestOrderByGroupBy() {
            // note: order-by is lost when group-by is applied (the sequence of groups is not ordered)
            var list = this.GetDataContext().List<Order>().Where(o => o.CustomerId == "ALFKI").OrderBy(o => o.OrderId).GroupBy(o => o.CustomerId).ToList();
            Assert.AreEqual(1, list.Count);

            var grp = list[0].ToList();
            var sorted = grp.OrderBy(o => o.OrderId);
            Assert.IsTrue(Enumerable.SequenceEqual(grp, sorted));
        }

        public virtual void TestOrderByGroupBySelectMany() {
            var list = this.GetDataContext().List<Order>().Where(o => o.CustomerId == "ALFKI").OrderBy(o => o.OrderId).GroupBy(o => o.CustomerId).SelectMany(g => g).ToList();
            Assert.AreEqual(6, list.Count);

            var sorted = list.OrderBy(o => o.OrderId).ToList();
            Assert.IsTrue(Enumerable.SequenceEqual(list, sorted));
        }

        public virtual void TestSumWithNoArg() {
            var sum = this.GetDataContext().List<Order>().Where(o => o.CustomerId == "ALFKI").Select(o => (o.CustomerId == "ALFKI" ? 1 : 1)).Sum();
            Assert.AreEqual(6, sum);
        }

        public virtual void TestSumWithArg() {
            var sum = this.GetDataContext().List<Order>().Where(o => o.CustomerId == "ALFKI").Sum(o => (o.CustomerId == "ALFKI" ? 1 : 1));
            Assert.AreEqual(6, sum);
        }

        public virtual void TestCountWithNoPredicate() {
            var cnt = this.GetDataContext().List<Order>().Count();
            Assert.AreEqual(830, cnt);
        }

        public virtual void TestCountWithPredicate() {
            var cnt = this.GetDataContext().List<Order>().Count(o => o.CustomerId == "ALFKI");
            Assert.AreEqual(6, cnt);
        }

        public virtual void TestDistinctNoDupes() {
            var list = this.GetDataContext().List<Customer>().Distinct().ToList();
            Assert.AreEqual(91, list.Count);
        }

        public virtual void TestDistinctScalar() {
            var list = this.GetDataContext().List<Customer>().Select(c => c.City).Distinct().ToList();
            Assert.AreEqual(69, list.Count);
        }

        public virtual void TestOrderByDistinct() {
            var list = this.GetDataContext().List<Customer>().Where(c => c.City.StartsWith("P")).OrderBy(c => c.City).Select(c => c.City).Distinct().ToList();
            var sorted = list.OrderBy(x => x).ToList();
            Assert.AreEqual(list[0], sorted[0]);
            Assert.AreEqual(list[list.Count - 1], sorted[list.Count - 1]);
        }

        public virtual void TestDistinctOrderBy() {
            var list = this.GetDataContext().List<Customer>().Where(c => c.City.StartsWith("P")).Select(c => c.City).Distinct().OrderBy(c => c).ToList();
            var sorted = list.OrderBy(x => x).ToList();
            Assert.AreEqual(list[0], sorted[0]);
            Assert.AreEqual(list[list.Count - 1], sorted[list.Count - 1]);
        }

        public virtual void TestDistinctGroupBy() {
            var list = this.GetDataContext().List<Order>().Where(o => o.CustomerId == "ALFKI").Distinct().GroupBy(o => o.CustomerId).ToList();

            Assert.AreEqual(1, list.Count);
        }

        public virtual void TestGroupByDistinct() {
            // distinct after group-by should not do anything
            var list = this.GetDataContext().List<Order>().Where(o => o.CustomerId == "ALFKI").GroupBy(o => o.CustomerId).Distinct().ToList();

            Assert.AreEqual(1, list.Count);
        }

        public virtual void TestDistinctCount() {
            var cnt = this.GetDataContext().List<Customer>().Distinct().Count();
            Assert.AreEqual(91, cnt);
        }

        public virtual void TestSelectDistinctCount() {
            // cannot do: SELECT COUNT(DISTINCT some-colum) FROM some-table
            // because COUNT(DISTINCT some-column) does not count nulls
            var cnt = this.GetDataContext().List<Customer>().Select(c => c.City).Distinct().Count();
            Assert.AreEqual(69, cnt);
        }

        public virtual void TestSelectSelectDistinctCount() {
            var cnt = this.GetDataContext().List<Customer>().Select(c => c.City).Select(c => c).Distinct().Count();
            Assert.AreEqual(69, cnt);
        }

        public virtual void TestDistinctCountPredicate() {
            var cnt = this.GetDataContext().List<Customer>().Select(c => new { c.City, c.Country }).Distinct().Count(c => c.City == "London");
            Assert.AreEqual(1, cnt);
        }

        public virtual void TestDistinctSumWithArg() {
            var sum = this.GetDataContext().List<Order>().Where(o => o.CustomerId == "ALFKI").Distinct().Sum(o => (o.CustomerId == "ALFKI" ? 1 : 1));
            Assert.AreEqual(6, sum);
        }

        public virtual void TestSelectDistinctSum() {
            var sum = this.GetDataContext().List<Order>().Where(o => o.CustomerId == "ALFKI").Select(o => o.OrderId).Distinct().Sum();
            Assert.AreEqual(64835, sum);
        }

        public virtual void TestTake() {
            var list = this.GetDataContext().List<Order>().OrderBy(o => o.CustomerId).Take(5).ToList();
            Assert.AreEqual(5, list.Count);
        }

        public virtual void TestTakeDistinct() {
            // distinct must be forced to apply after top has been computed
            var list = this.GetDataContext().List<Order>().OrderBy(o => o.CustomerId).Select(o => o.CustomerId).Take(5).Distinct().ToList();
            Assert.AreEqual(1, list.Count);
        }

        public virtual void TestDistinctTake() {
            // top must be forced to apply after distinct has been computed
            var list = this.GetDataContext().List<Order>().OrderBy(o => o.CustomerId).Select(o => o.CustomerId).Distinct().Take(5).OrderBy(o => o).ToList();
            Assert.AreEqual(5, list.Count);
        }

        public virtual void TestDistinctTakeCount() {
            var cnt = this.GetDataContext().List<Order>().Distinct().OrderBy(o => o.CustomerId).Select(o => o.CustomerId).Take(5).Count();
            Assert.AreEqual(5, cnt);
        }

        public virtual void TestTakeDistinctCount() {
            var cnt = this.GetDataContext().List<Order>().OrderBy(o => o.CustomerId).Select(o => o.CustomerId).Take(5).Distinct().Count();
            Assert.AreEqual(1, cnt);
        }

        public virtual void TestFirst() {
            var first = this.GetDataContext().List<Customer>().OrderBy(c => c.ContactName).First();
            Assert.AreNotEqual(null, first);
            Assert.AreEqual("ROMEY", first.CustomerId);
        }

        public virtual void TestFirstPredicate() {
            var first = this.GetDataContext().List<Customer>().OrderBy(c => c.ContactName).First(c => c.City == "London");
            Assert.AreNotEqual(null, first);
            Assert.AreEqual("EASTC", first.CustomerId);
        }

        public virtual void TestWhereFirst() {
            var first = this.GetDataContext().List<Customer>().OrderBy(c => c.ContactName).Where(c => c.City == "London").First();
            Assert.AreNotEqual(null, first);
            Assert.AreEqual("EASTC", first.CustomerId);
        }

        public virtual void TestFirstOrDefault() {
            var first = this.GetDataContext().List<Customer>().OrderBy(c => c.ContactName).FirstOrDefault();
            Assert.AreNotEqual(null, first);
            Assert.AreEqual("ROMEY", first.CustomerId);
        }

        public virtual void TestFirstOrDefaultPredicate() {
            var first = this.GetDataContext().List<Customer>().OrderBy(c => c.ContactName).FirstOrDefault(c => c.City == "London");
            Assert.AreNotEqual(null, first);
            Assert.AreEqual("EASTC", first.CustomerId);
        }

        public virtual void TestWhereFirstOrDefault() {
            var first = this.GetDataContext().List<Customer>().OrderBy(c => c.ContactName).Where(c => c.City == "London").FirstOrDefault();
            Assert.AreNotEqual(null, first);
            Assert.AreEqual("EASTC", first.CustomerId);
        }

        public virtual void TestFirstOrDefaultPredicateNoMatch() {
            var first = this.GetDataContext().List<Customer>().OrderBy(c => c.ContactName).FirstOrDefault(c => c.City == "SpongeBob");
            Assert.AreEqual(null, first);
        }

        public virtual void TestReverse() {
            var list = this.GetDataContext().List<Customer>().OrderBy(c => c.ContactName).Reverse().ToList();
            Assert.AreEqual(91, list.Count);
            Assert.AreEqual("WOLZA", list[0].CustomerId);
            Assert.AreEqual("ROMEY", list[90].CustomerId);
        }

        public virtual void TestReverseReverse() {
            var list = this.GetDataContext().List<Customer>().OrderBy(c => c.ContactName).Reverse().Reverse().ToList();
            Assert.AreEqual(91, list.Count);
            Assert.AreEqual("ROMEY", list[0].CustomerId);
            Assert.AreEqual("WOLZA", list[90].CustomerId);
        }

        public virtual void TestReverseWhereReverse() {
            var list = this.GetDataContext().List<Customer>().OrderBy(c => c.ContactName).Reverse().Where(c => c.City == "London").Reverse().ToList();
            Assert.AreEqual(6, list.Count);
            Assert.AreEqual("EASTC", list[0].CustomerId);
            Assert.AreEqual("BSBEV", list[5].CustomerId);
        }

        public virtual void TestReverseTakeReverse() {
            var list = this.GetDataContext().List<Customer>().OrderBy(c => c.ContactName).Reverse().Take(5).Reverse().ToList();
            Assert.AreEqual(5, list.Count);
            Assert.AreEqual("CHOPS", list[0].CustomerId);
            Assert.AreEqual("WOLZA", list[4].CustomerId);
        }

        public virtual void TestReverseWhereTakeReverse() {
            var list = this.GetDataContext().List<Customer>().OrderBy(c => c.ContactName).Reverse().Where(c => c.City == "London").Take(5).Reverse().ToList();
            Assert.AreEqual(5, list.Count);
            Assert.AreEqual("CONSH", list[0].CustomerId);
            Assert.AreEqual("BSBEV", list[4].CustomerId);
        }

        public virtual void TestLast() {
            var last = this.GetDataContext().List<Customer>().OrderBy(c => c.ContactName).Last();
            Assert.AreNotEqual(null, last);
            Assert.AreEqual("WOLZA", last.CustomerId);
        }

        public virtual void TestLastPredicate() {
            var last = this.GetDataContext().List<Customer>().OrderBy(c => c.ContactName).Last(c => c.City == "London");
            Assert.AreNotEqual(null, last);
            Assert.AreEqual("BSBEV", last.CustomerId);
        }

        public virtual void TestWhereLast() {
            var last = this.GetDataContext().List<Customer>().OrderBy(c => c.ContactName).Where(c => c.City == "London").Last();
            Assert.AreNotEqual(null, last);
            Assert.AreEqual("BSBEV", last.CustomerId);
        }

        public virtual void TestLastOrDefault() {
            var last = this.GetDataContext().List<Customer>().OrderBy(c => c.ContactName).LastOrDefault();
            Assert.AreNotEqual(null, last);
            Assert.AreEqual("WOLZA", last.CustomerId);
        }

        public virtual void TestLastOrDefaultPredicate() {
            var last = this.GetDataContext().List<Customer>().OrderBy(c => c.ContactName).LastOrDefault(c => c.City == "London");
            Assert.AreNotEqual(null, last);
            Assert.AreEqual("BSBEV", last.CustomerId);
        }

        public virtual void TestWhereLastOrDefault() {
            var last = this.GetDataContext().List<Customer>().OrderBy(c => c.ContactName).Where(c => c.City == "London").LastOrDefault();
            Assert.AreNotEqual(null, last);
            Assert.AreEqual("BSBEV", last.CustomerId);
        }

        public virtual void TestLastOrDefaultNoMatches() {
            var last = this.GetDataContext().List<Customer>().OrderBy(c => c.ContactName).LastOrDefault(c => c.City == "SpongeBob");
            Assert.AreEqual(null, last);
        }

        public virtual void TestSingleFails() {
            try {
                var single = this.GetDataContext().List<Customer>().Single();
            }
            catch (InvalidOperationException ex) {
                if (ex.Message.Contains("Sequence contains more than one element")) {
                    return;
                }

                throw;
            }
            throw new Exception("The following Exception was not thrown.\rInvalidOperationException: Sequence contains more than one element ");
        }

        public virtual void TestSinglePredicate() {
            var single = this.GetDataContext().List<Customer>().Single(c => c.CustomerId == "ALFKI");
            Assert.AreNotEqual(null, single);
            Assert.AreEqual("ALFKI", single.CustomerId);
        }

        public virtual void TestWhereSingle() {
            var single = this.GetDataContext().List<Customer>().Where(c => c.CustomerId == "ALFKI").Single();
            Assert.AreNotEqual(null, single);
            Assert.AreEqual("ALFKI", single.CustomerId);
        }

        public virtual void TestSingleOrDefaultFails() {
            try {
                var single = this.GetDataContext().List<Customer>().SingleOrDefault();
            }
            catch (InvalidOperationException ex) {
                if (ex.Message.Contains("Sequence contains more than one element")) {
                    return;
                }

                throw;
            }
            throw new Exception("The following Exception was not thrown.\rInvalidOperationException: Sequence contains more than one element ");
        }

        public virtual void TestSingleOrDefaultPredicate() {
            var single = this.GetDataContext().List<Customer>().SingleOrDefault(c => c.CustomerId == "ALFKI");
            Assert.AreNotEqual(null, single);
            Assert.AreEqual("ALFKI", single.CustomerId);
        }

        public virtual void TestWhereSingleOrDefault() {
            var single = this.GetDataContext().List<Customer>().Where(c => c.CustomerId == "ALFKI").SingleOrDefault();
            Assert.AreNotEqual(null, single);
            Assert.AreEqual("ALFKI", single.CustomerId);
        }

        public virtual void TestSingleOrDefaultNoMatches() {
            var single = this.GetDataContext().List<Customer>().SingleOrDefault(c => c.CustomerId == "SpongeBob");
            Assert.AreEqual(null, single);
        }

        public virtual void TestAnyTopLevel() {
            var any = this.GetDataContext().List<Customer>().Any();
            Assert.IsTrue(any);
        }

        public virtual void TestAnyWithSubquery() {
            var list = this.GetDataContext().List<Customer>().Where(c => c.Orders.Any(o => o.CustomerId == "ALFKI")).ToList();
            Assert.AreEqual(1, list.Count);
        }

        public virtual void TestAnyWithSubqueryNoPredicate() {
            // customers with at least one order
            var list = this.GetDataContext().List<Customer>().Where(c => this.GetDataContext().List<Order>().Where(o => o.CustomerId == c.CustomerId).Any()).ToList();
            Assert.AreEqual(89, list.Count);
        }

        public virtual void TestAnyWithLocalCollection() {
            // get customers for any one of these IDs
            string[] ids = new[] { "ALFKI", "WOLZA", "NOONE" };
            var list = this.GetDataContext().List<Customer>().Where(c => ids.Any(id => c.CustomerId == id)).ToList();
            Assert.AreEqual(2, list.Count);
        }

        public virtual void TestAllWithSubquery() {
            var list = this.GetDataContext().List<Customer>().Where(c => c.Orders.All(o => o.CustomerId == "ALFKI")).ToList();
            // includes customers w/ no orders
            Assert.AreEqual(3, list.Count);
        }

        public virtual void TestAllWithLocalCollection() {
            DataContext context = this.GetDataContext();

            // get all customers with a name that contains both 'm' and 'd'  (don't use vowels since these often depend on collation)
            string[] patterns = new[] { "m", "d" };

            var list = context.List<Customer>().Where(c => patterns.All(p => c.ContactName.ToLower().Contains(p))).Select(c => c.ContactName).ToList();
            var local = context.List<Customer>().AsEnumerable().Where(c => patterns.All(p => c.ContactName.ToLower().Contains(p))).Select(c => c.ContactName).ToList();

            Assert.AreEqual(local.Count, list.Count);
        }

        public virtual void TestAllTopLevel() {
            // all customers have name length > 0?
            var all = this.GetDataContext().List<Customer>().All(c => c.ContactName.Length > 0);
            Assert.IsTrue(all);
        }

        public virtual void TestAllTopLevelNoMatches() {
            // all customers have name with 'a'
            var all = this.GetDataContext().List<Customer>().All(c => c.ContactName.Contains("a"));
            Assert.IsFalse(all);
        }

        public virtual void TestContainsWithSubquery() {
            // this is the long-way to determine all customers that have at least one order
            var list = this.GetDataContext().List<Customer>().Where(c => this.GetDataContext().List<Order>().Select(o => o.CustomerId).Contains(c.CustomerId)).ToList();
            Assert.AreEqual(89, list.Count);
        }

        public virtual void TestContainsWithLocalCollection() {
            string[] ids = new[] { "ALFKI", "WOLZA", "NOONE" };
            var list = this.GetDataContext().List<Customer>().Where(c => ids.Contains(c.CustomerId)).ToList();
            Assert.AreEqual(2, list.Count);
        }

        public virtual void TestContainsTopLevel() {
            var contains = this.GetDataContext().List<Customer>().Select(c => c.CustomerId).Contains("ALFKI");
            Assert.IsTrue(contains);
        }

        public virtual void TestSkipTake() {
            var list = this.GetDataContext().List<Customer>().OrderBy(c => c.CustomerId).Skip(5).Take(10).ToList();
            Assert.AreEqual(10, list.Count);
            Assert.AreEqual("BLAUS", list[0].CustomerId);
            Assert.AreEqual("COMMI", list[9].CustomerId);
        }

        public virtual void TestDistinctSkipTake() {
            var list = this.GetDataContext().List<Customer>().Select(c => c.City).Distinct().OrderBy(c => c).Skip(5).Take(10).ToList();
            Assert.AreEqual(10, list.Count);
            var hs = new HashSet<string>(list);
            Assert.AreEqual(10, hs.Count);
        }

        public virtual void TestCoalesce() {
            var list = this.GetDataContext().List<Customer>().Select(c => new { City = (c.City == "London" ? null : c.City), Country = (c.CustomerId == "EASTC" ? null : c.Country) })
                         .Where(x => (x.City ?? "NoCity") == "NoCity").ToList();
            Assert.AreEqual(6, list.Count);
            Assert.AreEqual(null, list[0].City);
        }

        public virtual void TestCoalesce2() {
            var list = this.GetDataContext().List<Customer>().Select(c => new { City = (c.City == "London" ? null : c.City), Country = (c.CustomerId == "EASTC" ? null : c.Country) })
                         .Where(x => (x.City ?? x.Country ?? "NoCityOrCountry") == "NoCityOrCountry").ToList();
            Assert.AreEqual(1, list.Count);
            Assert.AreEqual(null, list[0].City);
            Assert.AreEqual(null, list[0].Country);
        }

        // framework function tests
        public virtual void TestStringLength() {
            var list = this.GetDataContext().List<Customer>().Where(c => c.City.Length == 7).ToList();
            Assert.AreEqual(9, list.Count);
        }

        public virtual void TestStringStartsWithLiteral() {
            var list = this.GetDataContext().List<Customer>().Where(c => c.ContactName.StartsWith("M")).ToList();
            Assert.AreEqual(12, list.Count);
        }

        public virtual void TestStringStartsWithColumn() {
            var list = this.GetDataContext().List<Customer>().Where(c => c.ContactName.StartsWith(c.ContactName)).ToList();
            Assert.AreEqual(91, list.Count);
        }

        public virtual void TestStringEndsWithLiteral() {
            var list = this.GetDataContext().List<Customer>().Where(c => c.ContactName.EndsWith("s")).ToList();
            Assert.AreEqual(9, list.Count);
        }

        public virtual void TestStringEndsWithColumn() {
            var list = this.GetDataContext().List<Customer>().Where(c => c.ContactName.EndsWith(c.ContactName)).ToList();
            Assert.AreEqual(91, list.Count);
        }

        public virtual void TestStringContainsLiteral() {
            DataContext context = this.GetDataContext();

            var list = context.List<Customer>().Where(c => c.ContactName.Contains("nd")).Select(c => c.ContactName).ToList();
            var local = context.List<Customer>().AsEnumerable().Where(c => c.ContactName.ToLower().Contains("nd")).Select(c => c.ContactName).ToList();
            Assert.AreEqual(local.Count, list.Count);
        }

        public virtual void TestStringContainsColumn() {
            var list = this.GetDataContext().List<Customer>().Where(c => c.ContactName.Contains(c.ContactName)).ToList();
            Assert.AreEqual(91, list.Count);
        }

        public virtual void TestStringConcatImplicit2Args() {
            var list = this.GetDataContext().List<Customer>().Where(c => c.ContactName + "X" == "Maria AndersX").ToList();
            Assert.AreEqual(1, list.Count);
        }

        public virtual void TestStringConcatExplicit2Args() {
            var list = this.GetDataContext().List<Customer>().Where(c => string.Concat(c.ContactName, "X") == "Maria AndersX").ToList();
            Assert.AreEqual(1, list.Count);
        }

        public virtual void TestStringConcatExplicit3Args() {
            var list = this.GetDataContext().List<Customer>().Where(c => string.Concat(c.ContactName, "X", c.Country) == "Maria AndersXGermany").ToList();
            Assert.AreEqual(1, list.Count);
        }

        public virtual void TestStringConcatExplicitNArgs() {
            var list = this.GetDataContext().List<Customer>().Where(c => string.Concat(new string[] { c.ContactName, "X", c.Country }) == "Maria AndersXGermany").ToList();
            Assert.AreEqual(1, list.Count);
        }

        public virtual void TestStringIsNullOrEmpty() {
            var list = this.GetDataContext().List<Customer>().Select(c => c.City == "London" ? null : c.CustomerId).Where(x => string.IsNullOrEmpty(x)).ToList();
            Assert.AreEqual(6, list.Count);
        }

        public virtual void TestStringToUpper() {
            var str = this.GetDataContext().List<Customer>().Where(c => c.CustomerId == "ALFKI").Max(c => (c.CustomerId == "ALFKI" ? "abc" : "abc").ToUpper());
            Assert.AreEqual("ABC", str);
        }

        public virtual void TestStringToLower() {
            var str = this.GetDataContext().List<Customer>().Where(c => c.CustomerId == "ALFKI").Max(c => (c.CustomerId == "ALFKI" ? "ABC" : "ABC").ToLower());
            Assert.AreEqual("abc", str);
        }

        public virtual void TestStringSubstring() {
            var list = this.GetDataContext().List<Customer>().Where(c => c.City.Substring(0, 4) == "Seat").ToList();
            Assert.AreEqual(1, list.Count);
            Assert.AreEqual("Seattle", list[0].City);
        }

        public virtual void TestStringSubstringNoLength() {
            var list = this.GetDataContext().List<Customer>().Where(c => c.City.Substring(4) == "tle").ToList();
            Assert.AreEqual(1, list.Count);
            Assert.AreEqual("Seattle", list[0].City);
        }

        public virtual void TestStringIndexOf() {
            var n = this.GetDataContext().List<Customer>().Where(c => c.CustomerId == "ALFKI").Sum(c => c.ContactName.IndexOf("ar"));
            Assert.AreEqual(1, n);
        }

        public virtual void TestStringIndexOfChar() {
            var n = this.GetDataContext().List<Customer>().Where(c => c.CustomerId == "ALFKI").Sum(c => c.ContactName.IndexOf('r'));
            Assert.AreEqual(2, n);
        }

        public virtual void TestStringIndexOfWithStart() {
            var n = this.GetDataContext().List<Customer>().Where(c => c.CustomerId == "ALFKI").Sum(c => c.ContactName.IndexOf("a", 3));
            Assert.AreEqual(4, n);
        }

        public virtual void TestStringTrim() {
            DataContext context = this.GetDataContext();

            var notrim = context.List<Customer>().Where(c => c.CustomerId == "ALFKI").Max(c => ("  " + c.City + " "));
            var trim = context.List<Customer>().Where(c => c.CustomerId == "ALFKI").Max(c => ("  " + c.City + " ").Trim());
            Assert.AreNotEqual(notrim, trim);
            Assert.AreEqual(notrim.Trim(), trim);
        }

        public virtual void TestMathAbs() {
            DataContext context = this.GetDataContext();

            var neg1 = context.List<Customer>().Where(c => c.CustomerId == "ALFKI").Sum(c => Math.Abs((c.CustomerId == "ALFKI") ? -1 : 0));
            var pos1 = context.List<Customer>().Where(c => c.CustomerId == "ALFKI").Sum(c => Math.Abs((c.CustomerId == "ALFKI") ? 1 : 0));
            Assert.AreEqual(Math.Abs(-1), neg1);
            Assert.AreEqual(Math.Abs(1), pos1);
        }

        public virtual void TestMathAtan() {
            DataContext context = this.GetDataContext();

            var zero = context.List<Customer>().Where(c => c.CustomerId == "ALFKI").Sum(c => Math.Atan((c.CustomerId == "ALFKI") ? 0.0 : 0.0));
            var one = context.List<Customer>().Where(c => c.CustomerId == "ALFKI").Sum(c => Math.Atan((c.CustomerId == "ALFKI") ? 1.0 : 1.0));
            Assert.AreEqual(Math.Atan(0.0), zero, 0.0001);
            Assert.AreEqual(Math.Atan(1.0), one, 0.0001);
        }

        public virtual void TestMathCos() {
            DataContext context = this.GetDataContext();

            var zero = context.List<Customer>().Where(c => c.CustomerId == "ALFKI").Sum(c => Math.Cos((c.CustomerId == "ALFKI") ? 0.0 : 0.0));
            var pi = context.List<Customer>().Where(c => c.CustomerId == "ALFKI").Sum(c => Math.Cos((c.CustomerId == "ALFKI") ? Math.PI : Math.PI));
            Assert.AreEqual(Math.Cos(0.0), zero, 0.0001);
            Assert.AreEqual(Math.Cos(Math.PI), pi, 0.0001);
        }

        public virtual void TestMathSin() {
            DataContext context = this.GetDataContext();

            var zero = context.List<Customer>().Where(c => c.CustomerId == "ALFKI").Sum(c => Math.Sin((c.CustomerId == "ALFKI") ? 0.0 : 0.0));
            var pi = context.List<Customer>().Where(c => c.CustomerId == "ALFKI").Sum(c => Math.Sin((c.CustomerId == "ALFKI") ? Math.PI : Math.PI));
            var pi2 = context.List<Customer>().Where(c => c.CustomerId == "ALFKI").Sum(c => Math.Sin(((c.CustomerId == "ALFKI") ? Math.PI : Math.PI) / 2.0));
            Assert.AreEqual(Math.Sin(0.0), zero);
            Assert.AreEqual(Math.Sin(Math.PI), pi, 0.0001);
            Assert.AreEqual(Math.Sin(Math.PI / 2.0), pi2, 0.0001);
        }

        public virtual void TestMathTan() {
            DataContext context = this.GetDataContext();

            var zero = context.List<Customer>().Where(c => c.CustomerId == "ALFKI").Sum(c => Math.Tan((c.CustomerId == "ALFKI") ? 0.0 : 0.0));
            var pi = context.List<Customer>().Where(c => c.CustomerId == "ALFKI").Sum(c => Math.Tan((c.CustomerId == "ALFKI") ? Math.PI : Math.PI));
            Assert.AreEqual(Math.Tan(0.0), zero, 0.0001);
            Assert.AreEqual(Math.Tan(Math.PI), pi, 0.0001);
        }

        public virtual void TestMathExp() {
            DataContext context = this.GetDataContext();

            var zero = context.List<Customer>().Where(c => c.CustomerId == "ALFKI").Sum(c => Math.Exp((c.CustomerId == "ALFKI") ? 0.0 : 0.0));
            var one = context.List<Customer>().Where(c => c.CustomerId == "ALFKI").Sum(c => Math.Exp((c.CustomerId == "ALFKI") ? 1.0 : 1.0));
            var two = context.List<Customer>().Where(c => c.CustomerId == "ALFKI").Sum(c => Math.Exp((c.CustomerId == "ALFKI") ? 2.0 : 2.0));
            Assert.AreEqual(Math.Exp(0.0), zero, 0.0001);
            Assert.AreEqual(Math.Exp(1.0), one, 0.0001);
            Assert.AreEqual(Math.Exp(2.0), two, 0.0001);
        }

        public virtual void TestMathLog() {
            DataContext context = this.GetDataContext();

            var one = context.List<Customer>().Where(c => c.CustomerId == "ALFKI").Sum(c => Math.Log((c.CustomerId == "ALFKI") ? 1.0 : 1.0));
            var e = context.List<Customer>().Where(c => c.CustomerId == "ALFKI").Sum(c => Math.Log((c.CustomerId == "ALFKI") ? Math.E : Math.E));
            Assert.AreEqual(Math.Log(1.0), one, 0.0001);
            Assert.AreEqual(Math.Log(Math.E), e, 0.0001);
        }

        public virtual void TestMathSqrt() {
            DataContext context = this.GetDataContext();

            var one = context.List<Customer>().Where(c => c.CustomerId == "ALFKI").Sum(c => Math.Sqrt((c.CustomerId == "ALFKI") ? 1.0 : 1.0));
            var four = context.List<Customer>().Where(c => c.CustomerId == "ALFKI").Sum(c => Math.Sqrt((c.CustomerId == "ALFKI") ? 4.0 : 4.0));
            var nine = context.List<Customer>().Where(c => c.CustomerId == "ALFKI").Sum(c => Math.Sqrt((c.CustomerId == "ALFKI") ? 9.0 : 9.0));
            Assert.AreEqual(1.0, one);
            Assert.AreEqual(2.0, four);
            Assert.AreEqual(3.0, nine);
        }

        public virtual void TestMathPow() {
            DataContext context = this.GetDataContext();
            // 2^n
            var zero = context.List<Customer>().Where(c => c.CustomerId == "ALFKI").Sum(c => Math.Pow((c.CustomerId == "ALFKI") ? 2.0 : 2.0, 0.0));
            var one = context.List<Customer>().Where(c => c.CustomerId == "ALFKI").Sum(c => Math.Pow((c.CustomerId == "ALFKI") ? 2.0 : 2.0, 1.0));
            var two = context.List<Customer>().Where(c => c.CustomerId == "ALFKI").Sum(c => Math.Pow((c.CustomerId == "ALFKI") ? 2.0 : 2.0, 2.0));
            var three = context.List<Customer>().Where(c => c.CustomerId == "ALFKI").Sum(c => Math.Pow((c.CustomerId == "ALFKI") ? 2.0 : 2.0, 3.0));
            Assert.AreEqual(1.0, zero);
            Assert.AreEqual(2.0, one);
            Assert.AreEqual(4.0, two);
            Assert.AreEqual(8.0, three);
        }

        public virtual void TestMathRoundDefault() {
            DataContext context = this.GetDataContext();

            var four = context.List<Customer>().Where(c => c.CustomerId == "ALFKI").Sum(c => Math.Round((c.CustomerId == "ALFKI") ? 3.4 : 3.4));
            var six = context.List<Customer>().Where(c => c.CustomerId == "ALFKI").Sum(c => Math.Round((c.CustomerId == "ALFKI") ? 3.6 : 3.6));
            Assert.AreEqual(3.0, four);
            Assert.AreEqual(4.0, six);
        }

        public virtual void TestMathFloor() {
            DataContext context = this.GetDataContext();

            // The difference between floor and truncate is how negatives are handled.  Floor drops the decimals and moves the
            // value to the more negative, so Floor(-3.4) is -4.0 and Floor(3.4) is 3.0.
            var four = context.List<Customer>().Where(c => c.CustomerId == "ALFKI").Sum(c => Math.Floor((c.CustomerId == "ALFKI" ? 3.4 : 3.4)));
            var six = context.List<Customer>().Where(c => c.CustomerId == "ALFKI").Sum(c => Math.Floor((c.CustomerId == "ALFKI" ? 3.6 : 3.6)));
            var nfour = context.List<Customer>().Where(c => c.CustomerId == "ALFKI").Sum(c => Math.Floor((c.CustomerId == "ALFKI" ? -3.4 : -3.4)));
            Assert.AreEqual(Math.Floor(3.4), four);
            Assert.AreEqual(Math.Floor(3.6), six);
            Assert.AreEqual(Math.Floor(-3.4), nfour);
        }

        public virtual void TestDecimalFloor() {
            DataContext context = this.GetDataContext();

            var four = context.List<Customer>().Where(c => c.CustomerId == "ALFKI").Sum(c => decimal.Floor((c.CustomerId == "ALFKI" ? 3.4m : 3.4m)));
            var six = context.List<Customer>().Where(c => c.CustomerId == "ALFKI").Sum(c => decimal.Floor((c.CustomerId == "ALFKI" ? 3.6m : 3.6m)));
            var nfour = context.List<Customer>().Where(c => c.CustomerId == "ALFKI").Sum(c => decimal.Floor((c.CustomerId == "ALFKI" ? -3.4m : -3.4m)));
            Assert.AreEqual(decimal.Floor(3.4m), four);
            Assert.AreEqual(decimal.Floor(3.6m), six);
            Assert.AreEqual(decimal.Floor(-3.4m), nfour);
        }

        public virtual void TestMathTruncate() {
            DataContext context = this.GetDataContext();

            // The difference between floor and truncate is how negatives are handled.  Truncate drops the decimals, 
            // therefore a truncated negative often has a more positive value than non-truncated (never has a less positive),
            // so Truncate(-3.4) is -3.0 and Truncate(3.4) is 3.0.
            var four = context.List<Customer>().Where(c => c.CustomerId == "ALFKI").Sum(c => Math.Truncate((c.CustomerId == "ALFKI") ? 3.4 : 3.4));
            var six = context.List<Customer>().Where(c => c.CustomerId == "ALFKI").Sum(c => Math.Truncate((c.CustomerId == "ALFKI") ? 3.6 : 3.6));
            var neg4 = context.List<Customer>().Where(c => c.CustomerId == "ALFKI").Sum(c => Math.Truncate((c.CustomerId == "ALFKI") ? -3.4 : -3.4));
            Assert.AreEqual(Math.Truncate(3.4), four);
            Assert.AreEqual(Math.Truncate(3.6), six);
            Assert.AreEqual(Math.Truncate(-3.4), neg4);
        }

        public virtual void TestStringCompareTo() {
            DataContext context = this.GetDataContext();

            var lt = context.List<Customer>().Where(c => c.CustomerId == "ALFKI").Sum(c => c.City.CompareTo("Seattle"));
            var gt = context.List<Customer>().Where(c => c.CustomerId == "ALFKI").Sum(c => c.City.CompareTo("Aaa"));
            var eq = context.List<Customer>().Where(c => c.CustomerId == "ALFKI").Sum(c => c.City.CompareTo("Berlin"));
            Assert.AreEqual(-1, lt);
            Assert.AreEqual(1, gt);
            Assert.AreEqual(0, eq);
        }

        public virtual void TestStringCompareToLT() {
            DataContext context = this.GetDataContext();

            var cmpLT = context.List<Customer>().Where(c => c.CustomerId == "ALFKI").SingleOrDefault(c => c.City.CompareTo("Seattle") < 0);
            var cmpEQ = context.List<Customer>().Where(c => c.CustomerId == "ALFKI").SingleOrDefault(c => c.City.CompareTo("Berlin") < 0);
            Assert.AreNotEqual(null, cmpLT);
            Assert.AreEqual(null, cmpEQ);
        }

        public virtual void TestStringCompareToLE() {
            DataContext context = this.GetDataContext();

            var cmpLE = context.List<Customer>().Where(c => c.CustomerId == "ALFKI").SingleOrDefault(c => c.City.CompareTo("Seattle") <= 0);
            var cmpEQ = context.List<Customer>().Where(c => c.CustomerId == "ALFKI").SingleOrDefault(c => c.City.CompareTo("Berlin") <= 0);
            var cmpGT = context.List<Customer>().Where(c => c.CustomerId == "ALFKI").SingleOrDefault(c => c.City.CompareTo("Aaa") <= 0);
            Assert.AreNotEqual(null, cmpLE);
            Assert.AreNotEqual(null, cmpEQ);
            Assert.AreEqual(null, cmpGT);
        }

        public virtual void TestStringCompareToGT() {
            DataContext context = this.GetDataContext();

            var cmpLT = context.List<Customer>().Where(c => c.CustomerId == "ALFKI").SingleOrDefault(c => c.City.CompareTo("Aaa") > 0);
            var cmpEQ = context.List<Customer>().Where(c => c.CustomerId == "ALFKI").SingleOrDefault(c => c.City.CompareTo("Berlin") > 0);
            Assert.AreNotEqual(null, cmpLT);
            Assert.AreEqual(null, cmpEQ);
        }

        public virtual void TestStringCompareToGE() {
            DataContext context = this.GetDataContext();

            var cmpLE = context.List<Customer>().Where(c => c.CustomerId == "ALFKI").SingleOrDefault(c => c.City.CompareTo("Seattle") >= 0);
            var cmpEQ = context.List<Customer>().Where(c => c.CustomerId == "ALFKI").SingleOrDefault(c => c.City.CompareTo("Berlin") >= 0);
            var cmpGT = context.List<Customer>().Where(c => c.CustomerId == "ALFKI").SingleOrDefault(c => c.City.CompareTo("Aaa") >= 0);
            Assert.AreEqual(null, cmpLE);
            Assert.AreNotEqual(null, cmpEQ);
            Assert.AreNotEqual(null, cmpGT);
        }

        public virtual void TestStringCompareToEQ() {
            DataContext context = this.GetDataContext();

            var cmpLE = context.List<Customer>().Where(c => c.CustomerId == "ALFKI").SingleOrDefault(c => c.City.CompareTo("Seattle") == 0);
            var cmpEQ = context.List<Customer>().Where(c => c.CustomerId == "ALFKI").SingleOrDefault(c => c.City.CompareTo("Berlin") == 0);
            var cmpGT = context.List<Customer>().Where(c => c.CustomerId == "ALFKI").SingleOrDefault(c => c.City.CompareTo("Aaa") == 0);
            Assert.AreEqual(null, cmpLE);
            Assert.AreNotEqual(null, cmpEQ);
            Assert.AreEqual(null, cmpGT);
        }

        public virtual void TestStringCompareToNE() {
            DataContext context = this.GetDataContext();

            var cmpLE = context.List<Customer>().Where(c => c.CustomerId == "ALFKI").SingleOrDefault(c => c.City.CompareTo("Seattle") != 0);
            var cmpEQ = context.List<Customer>().Where(c => c.CustomerId == "ALFKI").SingleOrDefault(c => c.City.CompareTo("Berlin") != 0);
            var cmpGT = context.List<Customer>().Where(c => c.CustomerId == "ALFKI").SingleOrDefault(c => c.City.CompareTo("Aaa") != 0);
            Assert.AreNotEqual(null, cmpLE);
            Assert.AreEqual(null, cmpEQ);
            Assert.AreNotEqual(null, cmpGT);
        }

        public virtual void TestStringCompare() {
            DataContext context = this.GetDataContext();

            var lt = context.List<Customer>().Where(c => c.CustomerId == "ALFKI").Sum(c => string.Compare(c.City, "Seattle"));
            var gt = context.List<Customer>().Where(c => c.CustomerId == "ALFKI").Sum(c => string.Compare(c.City, "Aaa"));
            var eq = context.List<Customer>().Where(c => c.CustomerId == "ALFKI").Sum(c => string.Compare(c.City, "Berlin"));
            Assert.AreEqual(-1, lt);
            Assert.AreEqual(1, gt);
            Assert.AreEqual(0, eq);
        }

        public virtual void TestStringCompareLT() {
            DataContext context = this.GetDataContext();

            var cmpLT = context.List<Customer>().Where(c => c.CustomerId == "ALFKI").SingleOrDefault(c => string.Compare(c.City, "Seattle") < 0);
            var cmpEQ = context.List<Customer>().Where(c => c.CustomerId == "ALFKI").SingleOrDefault(c => string.Compare(c.City, "Berlin") < 0);
            
            Assert.AreNotEqual(null, cmpLT);
            Assert.AreEqual(null, cmpEQ);
        }

        public virtual void TestStringCompareLE() {
            DataContext context = this.GetDataContext();

            var cmpLE = context.List<Customer>().Where(c => c.CustomerId == "ALFKI").SingleOrDefault(c => string.Compare(c.City, "Seattle") <= 0);
            var cmpEQ = context.List<Customer>().Where(c => c.CustomerId == "ALFKI").SingleOrDefault(c => string.Compare(c.City, "Berlin") <= 0);
            var cmpGT = context.List<Customer>().Where(c => c.CustomerId == "ALFKI").SingleOrDefault(c => string.Compare(c.City, "Aaa") <= 0);
            
            Assert.AreNotEqual(null, cmpLE);
            Assert.AreNotEqual(null, cmpEQ);
            Assert.AreEqual(null, cmpGT);
        }

        public virtual void TestStringCompareGT() {
            DataContext context = this.GetDataContext();

            var cmpLT = context.List<Customer>().Where(c => c.CustomerId == "ALFKI").SingleOrDefault(c => string.Compare(c.City, "Aaa") > 0);
            var cmpEQ = context.List<Customer>().Where(c => c.CustomerId == "ALFKI").SingleOrDefault(c => string.Compare(c.City, "Berlin") > 0);
            
            Assert.AreNotEqual(null, cmpLT);
            Assert.AreEqual(null, cmpEQ);
        }

        public virtual void TestStringCompareGE() {
            DataContext context = this.GetDataContext();

            var cmpLE = context.List<Customer>().Where(c => c.CustomerId == "ALFKI").SingleOrDefault(c => string.Compare(c.City, "Seattle") >= 0);
            var cmpEQ = context.List<Customer>().Where(c => c.CustomerId == "ALFKI").SingleOrDefault(c => string.Compare(c.City, "Berlin") >= 0);
            var cmpGT = context.List<Customer>().Where(c => c.CustomerId == "ALFKI").SingleOrDefault(c => string.Compare(c.City, "Aaa") >= 0);
            
            Assert.AreEqual(null, cmpLE);
            Assert.AreNotEqual(null, cmpEQ);
            Assert.AreNotEqual(null, cmpGT);
        }

        public virtual void TestStringCompareEQ() {
            DataContext context = this.GetDataContext();

            var cmpLE = context.List<Customer>().Where(c => c.CustomerId == "ALFKI").SingleOrDefault(c => string.Compare(c.City, "Seattle") == 0);
            var cmpEQ = context.List<Customer>().Where(c => c.CustomerId == "ALFKI").SingleOrDefault(c => string.Compare(c.City, "Berlin") == 0);
            var cmpGT = context.List<Customer>().Where(c => c.CustomerId == "ALFKI").SingleOrDefault(c => string.Compare(c.City, "Aaa") == 0);
            
            Assert.AreEqual(null, cmpLE);
            Assert.AreNotEqual(null, cmpEQ);
            Assert.AreEqual(null, cmpGT);
        }

        public virtual void TestStringCompareNE() {
            DataContext context = this.GetDataContext();

            var cmpLE = context.List<Customer>().Where(c => c.CustomerId == "ALFKI").SingleOrDefault(c => string.Compare(c.City, "Seattle") != 0);
            var cmpEQ = context.List<Customer>().Where(c => c.CustomerId == "ALFKI").SingleOrDefault(c => string.Compare(c.City, "Berlin") != 0);
            var cmpGT = context.List<Customer>().Where(c => c.CustomerId == "ALFKI").SingleOrDefault(c => string.Compare(c.City, "Aaa") != 0);
            
            Assert.AreNotEqual(null, cmpLE);
            Assert.AreEqual(null, cmpEQ);
            Assert.AreNotEqual(null, cmpGT);
        }

        public virtual void TestIntCompareTo() {
            DataContext context = this.GetDataContext();

            // prove that x.CompareTo(y) works for types other than string
            var eq = context.List<Customer>().Where(c => c.CustomerId == "ALFKI").Sum(c => (c.CustomerId == "ALFKI" ? 10 : 10).CompareTo(10));
            var gt = context.List<Customer>().Where(c => c.CustomerId == "ALFKI").Sum(c => (c.CustomerId == "ALFKI" ? 10 : 10).CompareTo(9));
            var lt = context.List<Customer>().Where(c => c.CustomerId == "ALFKI").Sum(c => (c.CustomerId == "ALFKI" ? 10 : 10).CompareTo(11));
            
            Assert.AreEqual(0, eq);
            Assert.AreEqual(1, gt);
            Assert.AreEqual(-1, lt);
        }

        public virtual void TestDecimalCompare() {
            DataContext context = this.GetDataContext();

            // prove that type.Compare(x,y) works with decimal
            var eq = context.List<Customer>().Where(c => c.CustomerId == "ALFKI").Sum(c => decimal.Compare((c.CustomerId == "ALFKI" ? 10m : 10m), 10m));
            var gt = context.List<Customer>().Where(c => c.CustomerId == "ALFKI").Sum(c => decimal.Compare((c.CustomerId == "ALFKI" ? 10m : 10m), 9m));
            var lt = context.List<Customer>().Where(c => c.CustomerId == "ALFKI").Sum(c => decimal.Compare((c.CustomerId == "ALFKI" ? 10m : 10m), 11m));
            
            Assert.AreEqual(0, eq);
            Assert.AreEqual(1, gt);
            Assert.AreEqual(-1, lt);
        }

        public virtual void TestDecimalAdd() {
            var onetwo = this.GetDataContext().List<Customer>().Where(c => c.CustomerId == "ALFKI").Sum(c => decimal.Add((c.CustomerId == "ALFKI" ? 1m : 1m), 2m));
            Assert.AreEqual(3m, onetwo);
        }

        public virtual void TestDecimalSubtract() {
            var onetwo = this.GetDataContext().List<Customer>().Where(c => c.CustomerId == "ALFKI").Sum(c => decimal.Subtract((c.CustomerId == "ALFKI" ? 1m : 1m), 2m));
            Assert.AreEqual(-1m, onetwo);
        }

        public virtual void TestDecimalMultiply() {
            var onetwo = this.GetDataContext().List<Customer>().Where(c => c.CustomerId == "ALFKI").Sum(c => decimal.Multiply((c.CustomerId == "ALFKI" ? 1m : 1m), 2m));
            Assert.AreEqual(2m, onetwo);
        }

        public virtual void TestDecimalDivide() {
            var onetwo = this.GetDataContext().List<Customer>().Where(c => c.CustomerId == "ALFKI").Sum(c => decimal.Divide((c.CustomerId == "ALFKI" ? 1.0m : 1.0m), 2.0m));
            Assert.AreEqual(0.5m, onetwo);
        }

        public virtual void TestDecimalNegate() {
            var one = this.GetDataContext().List<Customer>().Where(c => c.CustomerId == "ALFKI").Sum(c => decimal.Negate((c.CustomerId == "ALFKI" ? 1m : 1m)));
            Assert.AreEqual(-1m, one);
        }

        public virtual void TestDecimalRoundDefault() {
            DataContext context = this.GetDataContext();

            var four = context.List<Customer>().Where(c => c.CustomerId == "ALFKI").Sum(c => decimal.Round((c.CustomerId == "ALFKI" ? 3.4m : 3.4m)));
            var six = context.List<Customer>().Where(c => c.CustomerId == "ALFKI").Sum(c => decimal.Round((c.CustomerId == "ALFKI" ? 3.5m : 3.5m)));
            
            Assert.AreEqual(3.0m, four);
            Assert.AreEqual(4.0m, six);
        }

        public virtual void TestDecimalTruncate() {
            DataContext context = this.GetDataContext();

            // The difference between floor and truncate is how negatives are handled.  Truncate drops the decimals, 
            // therefore a truncated negative often has a more positive value than non-truncated (never has a less positive),
            // so Truncate(-3.4) is -3.0 and Truncate(3.4) is 3.0.
            var four = context.List<Customer>().Where(c => c.CustomerId == "ALFKI").Sum(c => decimal.Truncate((c.CustomerId == "ALFKI") ? 3.4m : 3.4m));
            var six = context.List<Customer>().Where(c => c.CustomerId == "ALFKI").Sum(c => Math.Truncate((c.CustomerId == "ALFKI") ? 3.6m : 3.6m));
            var neg4 = context.List<Customer>().Where(c => c.CustomerId == "ALFKI").Sum(c => Math.Truncate((c.CustomerId == "ALFKI") ? -3.4m : -3.4m));
            
            Assert.AreEqual(decimal.Truncate(3.4m), four);
            Assert.AreEqual(decimal.Truncate(3.6m), six);
            Assert.AreEqual(decimal.Truncate(-3.4m), neg4);
        }

        public virtual void TestDecimalLT() {
            // prove that decimals are treated normally with respect to normal comparison operators
            var alfki = this.GetDataContext().List<Customer>().SingleOrDefault(c => c.CustomerId == "ALFKI" && (c.CustomerId == "ALFKI" ? 1.0m : 3.0m) < 2.0m);
            Assert.AreNotEqual(null, alfki);
        }

        public virtual void TestIntLessThan() {
            DataContext context = this.GetDataContext();

            var alfki = context.List<Customer>().SingleOrDefault(c => c.CustomerId == "ALFKI" && (c.CustomerId == "ALFKI" ? 1 : 3) < 2);
            var alfkiN = context.List<Customer>().SingleOrDefault(c => c.CustomerId == "ALFKI" && (c.CustomerId == "ALFKI" ? 3 : 1) < 2);
            
            Assert.AreNotEqual(null, alfki);
            Assert.AreEqual(null, alfkiN);
        }

        public virtual void TestIntLessThanOrEqual() {
            DataContext context = this.GetDataContext();

            var alfki = context.List<Customer>().SingleOrDefault(c => c.CustomerId == "ALFKI" && (c.CustomerId == "ALFKI" ? 1 : 3) <= 2);
            var alfki2 = context.List<Customer>().SingleOrDefault(c => c.CustomerId == "ALFKI" && (c.CustomerId == "ALFKI" ? 2 : 3) <= 2);
            var alfkiN = context.List<Customer>().SingleOrDefault(c => c.CustomerId == "ALFKI" && (c.CustomerId == "ALFKI" ? 3 : 1) <= 2);
            
            Assert.AreNotEqual(null, alfki);
            Assert.AreNotEqual(null, alfki2);
            Assert.AreEqual(null, alfkiN);
        }

        public virtual void TestIntGreaterThan() {
            DataContext context = this.GetDataContext();

            var alfki = context.List<Customer>().SingleOrDefault(c => c.CustomerId == "ALFKI" && (c.CustomerId == "ALFKI" ? 3 : 1) > 2);
            var alfkiN = context.List<Customer>().SingleOrDefault(c => c.CustomerId == "ALFKI" && (c.CustomerId == "ALFKI" ? 1 : 3) > 2);

            Assert.AreNotEqual(null, alfki);
            Assert.AreEqual(null, alfkiN);
        }

        public virtual void TestIntGreaterThanOrEqual() {
            DataContext context = this.GetDataContext();

            var alfki = context.List<Customer>().Single(c => c.CustomerId == "ALFKI" && (c.CustomerId == "ALFKI" ? 3 : 1) >= 2);
            var alfki2 = context.List<Customer>().Single(c => c.CustomerId == "ALFKI" && (c.CustomerId == "ALFKI" ? 3 : 2) >= 2);
            var alfkiN = context.List<Customer>().SingleOrDefault(c => c.CustomerId == "ALFKI" && (c.CustomerId == "ALFKI" ? 1 : 3) > 2);
            
            Assert.AreNotEqual(null, alfki);
            Assert.AreNotEqual(null, alfki2);
            Assert.AreEqual(null, alfkiN);
        }

        public virtual void TestIntEqual() {
            DataContext context = this.GetDataContext();

            var alfki = context.List<Customer>().SingleOrDefault(c => c.CustomerId == "ALFKI" && (c.CustomerId == "ALFKI" ? 1 : 1) == 1);
            var alfkiN = context.List<Customer>().SingleOrDefault(c => c.CustomerId == "ALFKI" && (c.CustomerId == "ALFKI" ? 1 : 1) == 2);
            
            Assert.AreNotEqual(null, alfki);
            Assert.AreEqual(null, alfkiN);
        }

        public virtual void TestIntNotEqual() {
            DataContext context = this.GetDataContext();

            var alfki = context.List<Customer>().SingleOrDefault(c => c.CustomerId == "ALFKI" && (c.CustomerId == "ALFKI" ? 2 : 2) != 1);
            var alfkiN = context.List<Customer>().SingleOrDefault(c => c.CustomerId == "ALFKI" && (c.CustomerId == "ALFKI" ? 2 : 2) != 2);
            
            Assert.AreNotEqual(null, alfki);
            Assert.AreEqual(null, alfkiN);
        }

        public virtual void TestIntAdd() {
            var three = this.GetDataContext().List<Customer>().Where(c => c.CustomerId == "ALFKI").Sum(c => ((c.CustomerId == "ALFKI") ? 1 : 1) + 2);
            Assert.AreEqual(3, three);
        }

        public virtual void TestIntSubtract() {
            var negone = this.GetDataContext().List<Customer>().Where(c => c.CustomerId == "ALFKI").Sum(c => ((c.CustomerId == "ALFKI") ? 1 : 1) - 2);
            Assert.AreEqual(-1, negone);
        }

        public virtual void TestIntMultiply() {
            var six = this.GetDataContext().List<Customer>().Where(c => c.CustomerId == "ALFKI").Sum(c => ((c.CustomerId == "ALFKI") ? 2 : 2) * 3);
            Assert.AreEqual(6, six);
        }

        public virtual void TestIntDivide() {
            var one = this.GetDataContext().List<Customer>().Where(c => c.CustomerId == "ALFKI").Sum(c => ((c.CustomerId == "ALFKI") ? 3.0 : 3.0) / 2);
            Assert.AreEqual(1.5, one);
        }

        public virtual void TestIntModulo() {
            var three = this.GetDataContext().List<Customer>().Where(c => c.CustomerId == "ALFKI").Sum(c => ((c.CustomerId == "ALFKI") ? 7 : 7) % 4);
            Assert.AreEqual(3, three);
        }

        public virtual void TestIntLeftShift() {
            var eight = this.GetDataContext().List<Customer>().Where(c => c.CustomerId == "ALFKI").Sum(c => ((c.CustomerId == "ALFKI") ? 1 : 1) << 3);
            Assert.AreEqual(8, eight);
        }

        public virtual void TestIntRightShift() {
            var eight = this.GetDataContext().List<Customer>().Where(c => c.CustomerId == "ALFKI").Sum(c => ((c.CustomerId == "ALFKI") ? 32 : 32) >> 2);
            Assert.AreEqual(8, eight);
        }

        public virtual void TestIntBitwiseAnd() {
            var band = this.GetDataContext().List<Customer>().Where(c => c.CustomerId == "ALFKI").Sum(c => ((c.CustomerId == "ALFKI") ? 6 : 6) & 3);
            Assert.AreEqual(2, band);
        }

        public virtual void TestIntBitwiseOr() {
            var eleven = this.GetDataContext().List<Customer>().Where(c => c.CustomerId == "ALFKI").Sum(c => ((c.CustomerId == "ALFKI") ? 10 : 10) | 3);
            Assert.AreEqual(11, eleven);
        }

        public virtual void TestIntBitwiseExclusiveOr() {
            var zero = this.GetDataContext().List<Customer>().Where(c => c.CustomerId == "ALFKI").Sum(c => ((c.CustomerId == "ALFKI") ? 1 : 1) ^ 1);
            Assert.AreEqual(0, zero);
        }

        public virtual void TestIntBitwiseNot() {
            var bneg = this.GetDataContext().List<Customer>().Where(c => c.CustomerId == "ALFKI").Sum(c => ~((c.CustomerId == "ALFKI") ? -1 : -1));
            Assert.AreEqual(~-1, bneg);
        }

        public virtual void TestIntNegate() {
            var neg = this.GetDataContext().List<Customer>().Where(c => c.CustomerId == "ALFKI").Sum(c => -((c.CustomerId == "ALFKI") ? 1 : 1));
            Assert.AreEqual(-1, neg);
        }

        public virtual void TestAnd() {
            var custs = this.GetDataContext().List<Customer>().Where(c => c.Country == "USA" && c.City.StartsWith("A")).Select(c => c.City).ToList();
            Assert.AreEqual(2, custs.Count);
            Assert.IsTrue(custs.All(c => c.StartsWith("A")));
        }

        public virtual void TestOr() {
            var custs = this.GetDataContext().List<Customer>().Where(c => c.Country == "USA" || c.City.StartsWith("A")).Select(c => c.City).ToList();
            Assert.AreEqual(14, custs.Count);
        }

        public virtual void TestNot() {
            var custs = this.GetDataContext().List<Customer>().Where(c => !(c.Country == "USA")).Select(c => c.Country).ToList();
            Assert.AreEqual(78, custs.Count);
        }

        public virtual void TestEqualLiteralNull() {
            var q = this.GetDataContext().List<Customer>().Select(c => c.CustomerId == "ALFKI" ? null : c.CustomerId).Where(x => x == null);

            if (this.GetProvider() != null) {
                Assert.IsTrue(this.GetProvider().GetQueryText(q.Expression).Contains("IS NULL"));
            }

            var n = q.Count();
            Assert.AreEqual(1, n);
        }

        public virtual void TestEqualLiteralNullReversed() {
            var q = this.GetDataContext().List<Customer>().Select(c => c.CustomerId == "ALFKI" ? null : c.CustomerId).Where(x => null == x);

            if (this.GetProvider() != null) {
                Assert.IsTrue(this.GetProvider().GetQueryText(q.Expression).Contains("IS NULL"));
            }

            var n = q.Count();
            Assert.AreEqual(1, n);
        }

        public virtual void TestNotEqualLiteralNull() {
            var q = this.GetDataContext().List<Customer>().Select(c => c.CustomerId == "ALFKI" ? null : c.CustomerId).Where(x => x != null);

            if (this.GetProvider() != null) {
                Assert.IsTrue(this.GetProvider().GetQueryText(q.Expression).Contains("IS NOT NULL"));
            }

            var n = q.Count();
            Assert.AreEqual(90, n);
        }

        public virtual void TestNotEqualLiteralNullReversed() {
            var q = this.GetDataContext().List<Customer>().Select(c => c.CustomerId == "ALFKI" ? null : c.CustomerId).Where(x => null != x);

            if (this.GetProvider() != null) {
                Assert.IsTrue(this.GetProvider().GetQueryText(q.Expression).Contains("IS NOT NULL"));
            }

            var n = q.Count();
            Assert.AreEqual(90, n);
        }

        public virtual void TestConditionalResultsArePredicates() {
            bool value = this.GetDataContext().List<Order>().Where(c => c.CustomerId == "ALFKI").Max(c => (c.CustomerId == "ALFKI" ? string.Compare(c.CustomerId, "POTATO") < 0 : string.Compare(c.CustomerId, "POTATO") > 0));
            Assert.IsTrue(value);
        }

        public virtual void TestSelectManyJoined() {
            DataContext context = this.GetDataContext();

            var cods =
                (from c in context.List<Customer>()
                 from o in context.List<Order>().Where(o => o.CustomerId == c.CustomerId)
                 select new { c.ContactName, o.OrderDate }).ToList();
            Assert.AreEqual(830, cods.Count);
        }

        public virtual void TestSelectManyJoinedDefaultIfEmpty() {
            DataContext context = this.GetDataContext();

            var cods = (
                from c in context.List<Customer>()
                from o in context.List<Order>().Where(o => o.CustomerId == c.CustomerId).DefaultIfEmpty()
                select new { c.ContactName, o.OrderDate }
                ).ToList();
            Assert.AreEqual(832, cods.Count);
        }

        public virtual void TestSelectWhereAssociation() {
            var ords = (
                from o in this.GetDataContext().List<Order>()
                where o.Customer.City == "Seattle"
                select o
                ).ToList();
            Assert.AreEqual(14, ords.Count);
        }

        public virtual void TestSelectWhereAssociationTwice() {
            DataContext context = this.GetDataContext();

            var n = context.List<Order>().Where(c => c.CustomerId == "WHITC").Count();
            var ords = (
                from o in context.List<Order>()
                where o.Customer.Country == "USA" && o.Customer.City == "Seattle"
                select o
                ).ToList();
            Assert.AreEqual(n, ords.Count);
        }

        public virtual void TestSelectAssociation() {
            var custs = (
                from o in this.GetDataContext().List<Order>()
                where o.CustomerId == "ALFKI"
                select o.Customer
                ).ToList();
            Assert.AreEqual(6, custs.Count);
            Assert.IsTrue(custs.All(c => c.CustomerId == "ALFKI"));
        }

        public virtual void TestSelectAssociations() {
            var doubleCusts = (
                from o in this.GetDataContext().List<Order>()
                where o.CustomerId == "ALFKI"
                select new { A = o.Customer, B = o.Customer }
                ).ToList();

            Assert.AreEqual(6, doubleCusts.Count);
            Assert.IsTrue(doubleCusts.All(c => c.A.CustomerId == "ALFKI" && c.B.CustomerId == "ALFKI"));
        }

        public virtual void TestSelectAssociationsWhereAssociations() {
            var stuff = (
                from o in this.GetDataContext().List<Order>()
                where o.Customer.Country == "USA"
                where o.Customer.City != "Seattle"
                select new { A = o.Customer, B = o.Customer }
                ).ToList();

            Assert.AreEqual(108, stuff.Count);
        }

        //public virtual void TestCustomersIncludeOrders() {
        //    NorthwindDataContext nw = new NorthwindDataContext(this.GetDataContext().Provider.New(new TestPolicy("Orders")));

        //    var custs = nw.List<Customer>().Where(c => c.CustomerId == "ALFKI").ToList();
        //    Assert.AreEqual(1, custs.Count);
        //    Assert.AreNotEqual(null, custs[0].Orders);
        //    Assert.AreEqual(6, custs[0].Orders.Count);
        //}
                
        //public virtual void TestCustomersIncludeOrdersAndDetails() {
        //    NorthwindDataContext nw = new NorthwindDataContext(this.GetDataContext().Provider.New(new TestPolicy("Orders", "Details")));

        //    var custs = nw.List<Customer>().Where(c => c.CustomerId == "ALFKI").ToList();
        //    Assert.AreEqual(1, custs.Count);
        //    Assert.AreNotEqual(null, custs[0].Orders);
        //    Assert.AreEqual(6, custs[0].Orders.Count);
        //    Assert.IsTrue(custs[0].Orders.Any(o => o.OrderId == 10643));
        //    Assert.AreNotEqual(null, custs[0].Orders.Single(o => o.OrderId == 10643).Details);
        //    Assert.AreEqual(3, custs[0].Orders.Single(o => o.OrderId == 10643).Details.Count);
        //}
    }
}

