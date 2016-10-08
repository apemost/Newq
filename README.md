# Newq

[![Build Status][travis-image]][travis-url]
[![Build Status][appveyor-image]][appveyor-url]
[![NuGet Version][nuget-image]][nuget-url]

A new query builder for CSharp on .NET and Mono.

## Usage

```csharp

// `SELECT` statement:

var builder1 = new QueryBuilder();

builder1
    .Select<Customer>();

builder1.ToString();

/*
SELECT
    [Customer].[Id] AS [Customer.Id]
    ,[Customer].[Name] AS [Customer.Name]
    ,[Customer].[City] AS [Customer.City]
    ,[Customer].[Remark] AS [Customer.Remark]
    ,[Customer].[Status] AS [Customer.Status]
    ,[Customer].[Flag] AS [Customer.Flag]
    ,[Customer].[Version] AS [Customer.Version]
    ,[Customer].[AuthorId] AS [Customer.AuthorId]
    ,[Customer].[EditorId] AS [Customer.EditorId]
    ,[Customer].[CreatedDate] AS [Customer.CreatedDate]
    ,[Customer].[ModifiedDate] AS [Customer.ModifiedDate]
FROM
    [Customer]
*/

var builder2 = new QueryBuilder();

builder2
    .Select<Customer>((target, context) => {
        target += context["Provider", "Products"];
    })

    .LeftJoin<Provider>((on, context) => {
        on += context["Customer", "Name"] == context["Provider", "Name"];
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
    });

    builder2.Paginate(new Paginator());

    var result = builder2.ToString();

/*
SELECT
    [Provider.Products]
    ,[Customer.Name]
FROM (
    SELECT
        ROW_NUMBER() OVER(ORDER BY [Customer.Name] DESC) AS [$ROW_NUMBER]
        ,[Provider.Products]
        ,[Customer.Name]
    FROM (
        SELECT
            [Provider].[Products] AS [Provider.Products]
            ,[Customer].[Name] AS [Customer.Name]
        FROM
            [Customer]
        LEFT JOIN
            [Provider]
        ON
            [Customer].[Name] = [Provider].[Name]
        WHERE
            [Customer].[City] LIKE '%New%'
        GROUP BY
            [Provider].[Products]
        HAVING
            [Provider].[Name] NOT LIKE '%New%'
    ) AS [$ORIGINAL_QUERY]
) AS [$PAGINATOR]
WHERE
    [$PAGINATOR].[$ROW_NUMBER] BETWEEN 1 AND 10
*/
```

## Installation

To install, run the following command in the `Package Manager Console`:

```
PM> Install-Package Newq
```

## License

```
Copyright 2015-2016 Andrew Lyu and Uriel Van

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
```

[travis-image]: https://travis-ci.org/apemost/Newq.svg?branch=master
[travis-url]: https://travis-ci.org/apemost/Newq
[appveyor-image]: https://ci.appveyor.com/api/projects/status/4trdjumrr47e6213/branch/master?svg=true
[appveyor-url]: https://ci.appveyor.com/project/apemost/newq/branch/master
[nuget-image]: http://img.shields.io/nuget/v/Newq.svg?style=flat
[nuget-url]: https://www.nuget.org/packages/Newq/
