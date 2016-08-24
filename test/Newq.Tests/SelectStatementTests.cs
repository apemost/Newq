﻿/* Copyright 2015-2016 Andrew Lyu and Uriel Van
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */


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
                    target.Add(context["Provider", "Products"]);
                })

                .LeftJoin<Provider>((filter, context) => {
                    filter.Add(context["Customer", "Name"].EqualTo(context["Provider", "Name"]));
                })

                .Where((filter, context) => {
                    filter.Add(context["Customer", "City"].Like("New"));
                })

                .GroupBy((target, context) => {
                    target.Add(context["Provider", "Products"]);
                })

                .Having((filter, context) => {
                    filter.Add(context["Provider", "Name"].NotLike("New"));
                })

                .OrderBy((target, context) => {
                    target.Add(context["Customer", "Name", SortOrder.Desc]);
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
                    target += context["Customer", "Name"];
                    target += context["Provider", "Products"];
                })

                .LeftJoin<Provider>((filter, context) => {
                    filter += context["Customer", "Name"] == context["Provider", "Name"];
                })

                .Where((filter, context) => {
                    filter += context["Customer", "City"].Like("New");
                })

                .GroupBy((target, context) => {
                    target += context["Provider", "Products"];
                })

                .Having((filter, context) => {
                    filter += context["Provider", "Name"].NotLike("New");
                })

                .OrderBy((target, context) => {
                    target += context["Customer", "Name", SortOrder.Desc];
                    target += context["Customer", "Id", SortOrder.Desc];
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
                    target += context["Provider", "Products"];
                })

                .LeftJoin<Provider>((filter, context) => {
                    filter += context["Customer", "Name"] == context["Provider", "Name"];
                })

                .Where((filter, context) => {
                    filter += context["Customer", "City"].Like("New");
                })

                .GroupBy((target, context) => {
                    target += context["Provider", "Products"];
                })

                .Having((filter, context) => {
                    filter += context["Provider", "Name"].NotLike("New");
                })

                .OrderBy((target, context) => {
                    target += context["Customer", "Name", SortOrder.Desc];
                    target += context["Customer", "Id", SortOrder.Desc];
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
                    filter += context["Customer", "Name"] == context["Provider", "Name"];
                })

                .Where((filter, context) => {
                    filter += context["Customer", "City"].Like("New");
                })

                .OrderBy((target, context) => {
                    target += context["Customer", "Name", SortOrder.Desc];
                    target += context["Customer", "Id", SortOrder.Desc];
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
