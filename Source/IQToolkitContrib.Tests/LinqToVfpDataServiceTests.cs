using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IQToolkitContrib.Tests.Entities;
using System.Data.Services;
using System.Threading;
using System.ComponentModel;

namespace IQToolkitContrib.Tests {
    [TestClass]
    #region IgnoreTestMethods

    [IgnoreTestMethods(new string[] { 
        "TestMathTruncate", 
        "TestDecimalTruncate",
        "TestWhere",
        "TestStringConcatExplicit3Args",
        "TestStringConcatExplicitNArgs",
        "TestStringIsNullOrEmpty",
        "TestStringToUpper",
        "TestStringToLower",
        "TestStringIndexOf",
        "TestStringIndexOfChar",
        "TestStringIndexOfWithStart",
        "TestStringTrim",
        "TestStringTrim",
        "TestMathAbs",
        "TestMathAtan",
        "TestMathCos",
        "TestMathSin",
        "TestMathTan",
        "TestMathExp",
        "TestMathLog",
        "TestMathSqrt",
        "TestMathPow",
        "TestMathRoundDefault",
        "TestMathFloor",
        "TestDecimalFloor",
        "TestStringCompareTo",
        "TestStringCompareToLT",
        "TestStringCompareToLE",
        "TestStringCompareToGT",
        "TestStringCompareToGE",
        "TestStringCompareToEQ",
        "TestStringCompareToNE",
        "TestStringCompare",
        "TestStringCompareLT",
        "TestStringCompareLE",
        "TestStringCompareGT",
        "TestStringCompareGE",
        "TestStringCompareEQ",
        "TestStringCompareNE",
        "TestIntCompareTo",
        "TestDecimalCompare",
        "TestDecimalAdd",
        "TestDecimalSubtract",
        "TestDecimalMultiply",
        "TestDecimalDivide",
        "TestDecimalNegate",
        "TestDecimalRoundDefault",
        "TestDecimalLT",
        "TestIntLessThan",
        "TestIntLessThanOrEqual",
        "TestIntGreaterThan",
        "TestIntGreaterThanOrEqual",
        "TestIntEqual",
        "TestIntNotEqual",
        "TestIntAdd",
        "TestIntSubtract",
        "TestIntMultiply",
        "TestIntDivide",
        "TestIntModulo",
        "TestIntLeftShift",
        "TestIntRightShift",
        "TestIntBitwiseAnd",
        "TestIntBitwiseOr",
        "TestIntBitwiseExclusiveOr",
        "TestIntBitwiseNot",
        "TestIntNegate",
        "TestAnd",
        "TestOr",
        "TestNot",
        "TestEqualLiteralNull",
        "TestEqualLiteralNullReversed",
        "TestNotEqualLiteralNull",
        "TestNotEqualLiteralNullReversed",
        "TestConditionalResultsArePredicates",
        "TestSelectManyJoined",
        "TestSelectManyJoinedDefaultIfEmpty",
        "TestSelectWhereAssociation",
        "TestSelectWhereAssociationTwice",
        "TestSelectAssociation",
        "TestSelectAssociations",
        "TestSelectAssociationsWhereAssociations",
        "TestWhereTrue",
        "TestCompareEntityEqual",
        "TestCompareEntityNotEqual",
        "TestCompareConstructedEqual",
        "TestCompareConstructedMultiValueEqual",
        "TestCompareConstructedMultiValueNotEqual",
        "TestSelectScalar",
        "TestSelectAnonymousOne",
        "TestSelectAnonymousTwo",
        "TestSelectNestedCollection",
        "TestSelectNestedCollectionInAnonymousType",
        "TestJoinCustomerOrders",
        "TestJoinMultiKey",
        "TestJoinIntoCustomersOrdersCount",
        "TestJoinIntoDefaultIfEmpty",
        "TestMultipleJoinsWithJoinConditionsInWhere",
        "TestOrderBy",
        "TestOrderByJoin",
        "TestOrderBySelectMany",
        "TestGroupBy",
        "TestGroupByOne",
        "TestGroupBySelectMany",
        "TestGroupBySum",
        "TestGroupByCount",
        "TestGroupByLongCount",
        "TestGroupBySumMinMaxAvg",
        "TestGroupByWithResultSelector",
        "TestGroupByWithElementSelectorSum",
        "TestGroupByWithElementSelector",
        "TestGroupByWithElementSelectorSumMax",
        "TestGroupByWithAnonymousElement",
        "TestGroupByWithTwoPartKey",
        "TestOrderByGroupBy",
        "TestOrderByGroupBySelectMany",
        "TestSumWithNoArg",
        "TestSumWithArg",
        "TestCountWithNoPredicate",
        "TestCountWithPredicate",
        "TestDistinctNoDupes",
        "TestDistinctScalar",
        "TestOrderByDistinct",
        "TestDistinctOrderBy",
        "TestDistinctGroupBy",
        "TestGroupByDistinct",
        "TestDistinctCount",
        "TestSelectDistinctCount",
        "TestSelectSelectDistinctCount",
        "TestDistinctCountPredicate",
        "TestDistinctSumWithArg",
        "TestSelectDistinctSum",
        "TestTakeDistinct",
        "TestDistinctTake",
        "TestDistinctTakeCount",
        "TestTakeDistinctCount",
        "TestFirstPredicate",
        "TestFirstOrDefaultPredicate",
        "TestFirstOrDefaultPredicateNoMatch",
        "TestReverse",
        "TestReverseReverse",
        "TestReverseWhereReverse",
        "TestReverseTakeReverse",
        "TestReverseWhereTakeReverse",
        "TestLast",
        "TestLastPredicate",
        "TestWhereLast",
        "TestLastOrDefault",
        "TestLastOrDefaultPredicate",
        "TestWhereLastOrDefault",
        "TestLastOrDefaultNoMatches",
        "TestSinglePredicate",
        "TestSingleOrDefaultFails",
        "TestSingleOrDefaultPredicate",
        "TestSingleOrDefaultNoMatches",
        "TestAnyTopLevel",
        "TestAnyWithSubquery",
        "TestAnyWithSubqueryNoPredicate",
        "TestAnyWithLocalCollection",
        "TestAllWithSubquery",
        "TestAllWithLocalCollection",
        "TestAllTopLevel",
        "TestAllTopLevelNoMatches",
        "TestContainsWithSubquery",
        "TestContainsWithLocalCollection",
        "TestContainsTopLevel",
        "TestDistinctSkipTake",
        "TestCoalesce",
        "TestCoalesce2",
        "TestStringStartsWithLiteral",
        "TestStringStartsWithColumn",
        "TestStringEndsWithLiteral",
        "TestStringEndsWithColumn",
        "TestStringContainsLiteral",
        "TestStringContainsColumn"
    })]

    #endregion
    public partial class LinqToVfpDataServiceTests : ATest {
        private static LinqToVfpDataContext context;
        private static DataServiceHost host;

        [TestMethod]
        public void TestRemovingOfExcessiveImmediateIfNullConditions() {
            DataContext context = this.GetDataContext();

            var query = from d in context.List<Customer>()
                        where d.CustomerId.Trim().ToUpper() == "ALFKI"
                            && d.CustomerId.Trim().ToUpper() == "ALFKI"
                        select d;
            
            var list = query.ToList();
        }

        [TestMethod, Ignore]
        public void DeleteCustomer() {
            DataContext context = this.GetDataContext();

            Customer c = new Customer {
                CustomerId = "XX1",
                CompanyName = "Company1",
                ContactName = "Contact1",
                City = "Seattle",
                Country = "USA",
                Phone = string.Empty
            };

            context.Insert<Customer>(c);
            c = context.Get<Customer>(c.CustomerId);
            Assert.IsNotNull(c);
            Assert.AreEqual(92, context.List<Customer>().ToList().Count());

            context.Delete<Customer>(c);

            c = context.Get<Customer>(c.CustomerId);
            Assert.IsNull(c);
            Assert.AreEqual(91, context.List<Customer>().ToList().Count());
        }

        [TestMethod]
        public void InsertCustomer() {
            DataContext context = this.GetDataContext();

            Customer c = new Customer {
                CustomerId = "XX1",
                CompanyName = "Company1",
                ContactName = "Contact1",
                City = "Seattle",
                Country = "USA",
                Phone = string.Empty
            };

            context.Insert<Customer>(c);
            c = context.Get<Customer>(c.CustomerId);
            Assert.IsNotNull(c);
            Assert.AreEqual(92, context.List<Customer>().ToList().Count());
        }

        [TestMethod]
        public void UpdateCustomer() {
            Customer c = this.GetDataContext().Get<Customer>("ALFKI");
            c.CompanyName = "Test";
            this.GetDataContext().Update<Customer>(c);

            c = this.GetDataContext().Get<Customer>("ALFKI");
            Assert.AreEqual("Test", c.CompanyName);
        }

        protected override DataContext GetDataContext() {
            if (context == null) {
                Uri uri = new Uri("http://localhost:12345/IQToolkitContribTest");
                host = new DataServiceHost(typeof(LinqToVfpDataService), new Uri[] { uri });
                host.Open();
                context = new LinqToVfpDataContext(uri, new TestContextWriter(this.TestContext));
            }

            return context;
        }

        [TestInitialize]
        public void TestInitialize() {
            CopyData(BackupPath, DbcPath);
        }

        [TestCleanup]
        public void TestCleanup() {
            CopyData(BackupPath, DbcPath);
        }
    }
}
