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


namespace Newq.Attributes
{
    using System;

    /// <summary>
    /// Specify the table name.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    sealed class TableAttribute : Attribute
    {
        readonly string tableName;

        public TableAttribute(string tableName)
        {
            this.tableName = tableName;
        }

        public string TableName
        {
            get { return tableName; }
        }
    }
}
