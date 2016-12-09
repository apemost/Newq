namespace Newq.Tests
{
    using Models;
    using Xunit;

    public class SelectStatementTests
    {
        [Fact]
        public void Select1()
        {
            var queryBuilder = new QueryBuilder();

            queryBuilder
                .Select<Customer>((target, context) => {
                    target.Add(context.Table<Provider>(t => t.Products));
                })

                .LeftJoin<Provider>((filter, context) => {
                    filter.Add(context.Table<Customer>(t => t.Name).EqualTo(context.Table<Provider>(t => t.Name)));
                })

                .Where((filter, context) => {
                    filter.Add(context.Table<Customer>(t => t.City).Like("New"));
                })

                .GroupBy((target, context) => {
                    target.Add(context.Table<Provider>(t => t.Products));
                })

                .Having((filter, context) => {
                    filter.Add(context.Table<Provider>(t => t.Name).NotLike("New"));
                })

                .OrderBy((target, context) => {
                    target.Add(context.Table<Customer>(t => t.Name, SortOrder.Desc));
                });

            queryBuilder.Paginate(new Paginator());

            var result = queryBuilder.ToString();
            var expected =
                "SELECT " +
                    "[Provider.Products]" +
                    ",[Customer.Name] " +
                "FROM (" +
                    "SELECT " +
                        "ROW_NUMBER() OVER(ORDER BY [Customer.Name] DESC) AS [$ROW_NUMBER]" +
                        ",[Provider.Products]" +
                        ",[Customer.Name] " +
                    "FROM (" +
                        "SELECT " +
                            "[Provider].[Products] AS [Provider.Products]" +
                            ",[Customer].[Name] AS [Customer.Name] " +
                        "FROM " +
                            "[Customer] " +
                        "LEFT JOIN " +
                            "[Provider] " +
                        "ON " +
                            "[Customer].[Name] = [Provider].[Name] " +
                        "WHERE " +
                            "[Customer].[City] LIKE '%New%' " +
                        "GROUP BY " +
                            "[Provider].[Products] " +
                        "HAVING " +
                            "[Provider].[Name] NOT LIKE '%New%'" +
                    ") AS [$ORIGINAL_QUERY]" +
                ") AS [$PAGINATOR] " +
                "WHERE " +
                    "[$PAGINATOR].[$ROW_NUMBER] BETWEEN 1 AND 10 ";

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Select2()
        {
            var queryBuilder = new QueryBuilder();

            queryBuilder
                .Select<Customer>((target, context) => {
                    target += context.Table<Customer>(t => t.Name);
                    target += context.Table<Provider>(t => t.Products);
                })

                .LeftJoin<Provider>((filter, context) => {
                    filter += context.Table<Customer>(t => t.Name) == context.Table<Provider>(t => t.Name);
                })

                .Where((filter, context) => {
                    filter += context.Table<Customer>(t => t.City).Like("New");
                })

                .GroupBy((target, context) => {
                    target += context.Table<Provider>(t => t.Products);
                })

                .Having((filter, context) => {
                    filter += context.Table<Provider>(t => t.Name).NotLike("New");
                })

                .OrderBy((target, context) => {
                    target += context.Table<Customer>(t => t.Name, SortOrder.Desc);
                    target += context.Table<Customer>(t => t.Id, SortOrder.Desc);
                });

            queryBuilder.Paginate(new Paginator());

            var result = queryBuilder.ToString();
            var expected =
                "SELECT " +
                    "[Customer.Name]" +
                    ",[Provider.Products]" +
                    ",[Customer.Id] " +
                "FROM (" +
                    "SELECT " +
                        "ROW_NUMBER() OVER(ORDER BY [Customer.Name] DESC,[Customer.Id] DESC) AS [$ROW_NUMBER]" +
                        ",[Customer.Name]" +
                        ",[Provider.Products]" +
                        ",[Customer.Id] " +
                    "FROM (" +
                        "SELECT " +
                            "[Customer].[Name] AS [Customer.Name]" +
                            ",[Provider].[Products] AS [Provider.Products]" +
                            ",[Customer].[Id] AS [Customer.Id] " +
                        "FROM " +
                            "[Customer] " +
                        "LEFT JOIN " +
                            "[Provider] " +
                        "ON " +
                            "[Customer].[Name] = [Provider].[Name] " +
                        "WHERE " +
                            "[Customer].[City] LIKE '%New%' " +
                        "GROUP BY " +
                            "[Provider].[Products] " +
                        "HAVING " +
                            "[Provider].[Name] NOT LIKE '%New%'" +
                    ") AS [$ORIGINAL_QUERY]" +
                ") AS [$PAGINATOR] " +
                "WHERE " +
                    "[$PAGINATOR].[$ROW_NUMBER] BETWEEN 1 AND 10 ";

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Select3()
        {
            var queryBuilder = new QueryBuilder();

            queryBuilder
                .Select<Customer>((target, context) => {
                    target += context.Table<Provider>(t => t.Products);
                })

                .LeftJoin<Provider>((filter, context) => {
                    filter += context.Table<Customer>(t => t.Name) == context.Table<Provider>(t => t.Name);
                })

                .Where((filter, context) => {
                    filter += context.Table<Customer>(t => t.City).Like("New");
                })

                .GroupBy((target, context) => {
                    target += context.Table<Provider>(t => t.Products);
                })

                .Having((filter, context) => {
                    filter += context.Table<Provider>(t => t.Name).NotLike("New");
                })

                .OrderBy((target, context) => {
                    target += context.Table<Customer>(t => t.Name, SortOrder.Desc);
                    target += context.Table<Customer>(t => t.Id, SortOrder.Desc);
                });

            var result = queryBuilder.ToString();
            var expected =
                "SELECT " +
                    "[Provider].[Products] AS [Provider.Products] " +
                "FROM " +
                    "[Customer] " +
                "LEFT JOIN " +
                    "[Provider] " +
                "ON " +
                    "[Customer].[Name] = [Provider].[Name] " +
                "WHERE " +
                    "[Customer].[City] LIKE '%New%' " +
                "GROUP BY " +
                    "[Provider].[Products] " +
                "HAVING " +
                    "[Provider].[Name] NOT LIKE '%New%' " +
                "ORDER BY " +
                    "[Customer].[Name] DESC" +
                    ",[Customer].[Id] DESC ";

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Select4()
        {
            var queryBuilder = new QueryBuilder();

            queryBuilder
                .Select<Customer>()

                .LeftJoin<Provider>((filter, context) => {
                    filter += context.Table<Customer>(t => t.Name) == context.Table<Provider>(t => t.Name);
                })

                .Where((filter, context) => {
                    filter += context.Table<Customer>(t => t.City).Like("New");
                })

                .OrderBy((target, context) => {
                    target += context.Table<Customer>(t => t.Name, SortOrder.Desc);
                    target += context.Table<Customer>(t => t.Id, SortOrder.Desc);
                });

            var result = queryBuilder.ToString();
            var expected =
                "SELECT " +
                    "[Customer].[Id] AS [Customer.Id]" +
                    ",[Customer].[Name] AS [Customer.Name]" +
                    ",[Customer].[City] AS [Customer.City]" +
                    ",[Customer].[Remark] AS [Customer.Remark]" +
                    ",[Customer].[Status] AS [Customer.Status]" +
                    ",[Customer].[Flag] AS [Customer.Flag]" +
                    ",[Customer].[Version] AS [Customer.Version]" +
                    ",[Customer].[AuthorId] AS [Customer.AuthorId]" +
                    ",[Customer].[EditorId] AS [Customer.EditorId]" +
                    ",[Customer].[CreatedDate] AS [Customer.CreatedDate]" +
                    ",[Customer].[ModifiedDate] AS [Customer.ModifiedDate] " +
                "FROM " +
                    "[Customer] " +
                "LEFT JOIN " +
                    "[Provider] " +
                "ON " +
                    "[Customer].[Name] = [Provider].[Name] " +
                "WHERE " +
                    "[Customer].[City] LIKE '%New%' " +
                "ORDER BY " +
                    "[Customer].[Name] DESC" +
                    ",[Customer].[Id] DESC ";

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Select5()
        {
            var queryBuilder = new QueryBuilder();

            queryBuilder
                .Select<Person>();

            var result = queryBuilder.ToString();
            var expected =
                "SELECT " +
                    "[PERSION].[ID] AS [PERSION.ID]" +
                    ",[PERSION].[Name] AS [PERSION.Name] " +
                "FROM " +
                    "[PERSION] ";

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Select6()
        {
            var queryBuilder = new QueryBuilder();

            queryBuilder
                .Select<Man>();

            var result = queryBuilder.ToString();
            var expected =
                "SELECT " +
                    "[PERSION].[ID] AS [PERSION.ID]" +
                    ",[PERSION].[Name] AS [PERSION.Name] " +
                "FROM " +
                    "[PERSION] ";

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Select7()
        {
            var queryBuilder = new QueryBuilder();

            queryBuilder
                .Select<Woman>();

            var result = queryBuilder.ToString();
            var expected =
                "SELECT " +
                    "[Woman].[ID] AS [Woman.ID]" +
                    ",[Woman].[Name] AS [Woman.Name] " +
                "FROM " +
                    "[Woman] ";

            Assert.Equal(expected, result);
        }
    }
}
