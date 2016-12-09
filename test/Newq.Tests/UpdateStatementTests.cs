namespace Newq.Tests
{
    using Models;
    using Xunit;

    public class UpdateStatementTests
    {
        [Fact]
        public void Update1()
        {
            var customer = new Customer();
            customer.City = "New York";
            customer.Remark = "Good Customer";

            var queryBuilder = new QueryBuilder();

            queryBuilder
                .Update(customer);

            var result = queryBuilder.ToString();
            var expected =
                "UPDATE " +
                    "[Customer] " +
                "SET " +
                    "[Customer].[Id] = ''" +
                    ",[Customer].[Name] = ''" +
                    ",[Customer].[City] = 'New York'" +
                    ",[Customer].[Remark] = 'Good Customer'" +
                    ",[Customer].[Status] = 0" +
                    ",[Customer].[Flag] = 0" +
                    ",[Customer].[Version] = 0" +
                    ",[Customer].[AuthorId] = ''" +
                    ",[Customer].[EditorId] = ''" +
                    ",[Customer].[CreatedDate] = '1753-01-01 00:00:00.000'" +
                    ",[Customer].[ModifiedDate] = '1753-01-01 00:00:00.000' " +
                "FROM " +
                    "[Customer] ";

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Update2()
        {
            var customer = new Customer();
            customer.City = "New York";
            customer.Remark = "Good Customer";

            var queryBuilder = new QueryBuilder();

            queryBuilder
                .Update(customer)
                .Join<Provider>((filter, context) => filter.Add(context[0]["Name"].EqualTo(context[1]["Name"])));

            var result = queryBuilder.ToString();
            var expected =
                "UPDATE " +
                    "[Customer] " +
                "SET " +
                    "[Customer].[Id] = ''" +
                    ",[Customer].[Name] = ''" +
                    ",[Customer].[City] = 'New York'" +
                    ",[Customer].[Remark] = 'Good Customer'" +
                    ",[Customer].[Status] = 0" +
                    ",[Customer].[Flag] = 0" +
                    ",[Customer].[Version] = 0" +
                    ",[Customer].[AuthorId] = ''" +
                    ",[Customer].[EditorId] = ''" +
                    ",[Customer].[CreatedDate] = '1753-01-01 00:00:00.000'" +
                    ",[Customer].[ModifiedDate] = '1753-01-01 00:00:00.000' " +
                "FROM " +
                    "[Customer] " +
                "JOIN " +
                    "[Provider] " +
                "ON " +
                    "[Customer].[Name] = [Provider].[Name] ";

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Update3()
        {
            var customer = new Customer();
            customer.City = "New York";
            customer.Remark = "Good Customer";

            var queryBuilder = new QueryBuilder();

            queryBuilder
                .Update(customer, (target, context) => {
                    target += context[0]["City"];
                });

            var result = queryBuilder.ToString();
            var expected =
                "UPDATE " +
                    "[Customer] " +
                "SET " +
                    "[Customer].[City] = 'New York' " +
                "FROM " +
                    "[Customer] ";

            Assert.Equal(expected, result);
        }
    }
}
