namespace Newq.Tests
{
    using System.Collections.Generic;
    using Models;
    using Xunit;

    public class InsertStatementTests
    {
        [Fact]
        public void Insert1()
        {
            var customer = new Customer()
            {
                City = "New York",
                Remark = "Good Customer"
            };

            var queryBuilder = new QueryBuilder();

            queryBuilder
                .Insert(customer);

            var result = queryBuilder.ToString();
            var expected =
                "INSERT INTO " +
                    "[Customer] " +
                "(" +
                    "[Customer].[Id]" +
                    ",[Customer].[Name]" +
                    ",[Customer].[City]" +
                    ",[Customer].[Remark]" +
                    ",[Customer].[Status]" +
                    ",[Customer].[Flag]" +
                    ",[Customer].[Version]" +
                    ",[Customer].[AuthorId]" +
                    ",[Customer].[EditorId]" +
                    ",[Customer].[CreatedDate]" +
                    ",[Customer].[ModifiedDate]" +
                ") VALUES (" +
                    "''" +
                    ",''" +
                    ",'New York'" +
                    ",'Good Customer'" +
                    ",0" +
                    ",0" +
                    ",0" +
                    ",''" +
                    ",''" +
                    ",'1753-01-01 00:00:00.000'" +
                    ",'1753-01-01 00:00:00.000'" +
                ") ";

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Insert2()
        {
            var customer = new Customer()
            {
                City = "New York",
                Remark = "Good Customer"
            };

            var list = new List<Customer>();

            for (int i = 0; i < 10; i++)
            {
                list.Add(customer);
            }

            var queryBuilder = new QueryBuilder();

            queryBuilder
                .InsertMany(list);

            var result = queryBuilder.ToString();
            var expected =
                "INSERT INTO " +
                    "[Customer] " +
                "(" +
                    "[Customer].[Id]" +
                    ",[Customer].[Name]" +
                    ",[Customer].[City]" +
                    ",[Customer].[Remark]" +
                    ",[Customer].[Status]" +
                    ",[Customer].[Flag]" +
                    ",[Customer].[Version]" +
                    ",[Customer].[AuthorId]" +
                    ",[Customer].[EditorId]" +
                    ",[Customer].[CreatedDate]" +
                    ",[Customer].[ModifiedDate]" +
                ") " +
                "VALUES " +
                    "('','','New York','Good Customer',0,0,0,'','','1753-01-01 00:00:00.000','1753-01-01 00:00:00.000')" +
                    ",('','','New York','Good Customer',0,0,0,'','','1753-01-01 00:00:00.000','1753-01-01 00:00:00.000')" +
                    ",('','','New York','Good Customer',0,0,0,'','','1753-01-01 00:00:00.000','1753-01-01 00:00:00.000')" +
                    ",('','','New York','Good Customer',0,0,0,'','','1753-01-01 00:00:00.000','1753-01-01 00:00:00.000')" +
                    ",('','','New York','Good Customer',0,0,0,'','','1753-01-01 00:00:00.000','1753-01-01 00:00:00.000')" +
                    ",('','','New York','Good Customer',0,0,0,'','','1753-01-01 00:00:00.000','1753-01-01 00:00:00.000')" +
                    ",('','','New York','Good Customer',0,0,0,'','','1753-01-01 00:00:00.000','1753-01-01 00:00:00.000')" +
                    ",('','','New York','Good Customer',0,0,0,'','','1753-01-01 00:00:00.000','1753-01-01 00:00:00.000')" +
                    ",('','','New York','Good Customer',0,0,0,'','','1753-01-01 00:00:00.000','1753-01-01 00:00:00.000')" +
                    ",('','','New York','Good Customer',0,0,0,'','','1753-01-01 00:00:00.000','1753-01-01 00:00:00.000') ";

            Assert.Equal(expected, result);
        }
    }
}
