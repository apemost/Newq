/* Copyright 2015-2016 Andrew Lyu and Uriel Van
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

namespace Newq
{
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    public class Function : Syntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Function"/> class.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parameter"></param>
        public Function(string name, object parameter)
            : base(name, parameter)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Function"/> class.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parameters"></param>
        public Function(string name, IEnumerable<object> parameters)
            : base(name, parameters)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public string Alias
        {
            get
            {
                var alias = string.Empty;

                if (Parameters.Count > 0 && Parameters[0] is Column)
                {
                    var column = Parameters[0] as Column;

                    alias = string.Format("[{0}.{1}.{2}]", Name, column.Table.Name, column.Name);
                }
                else
                {
                    alias = string.Format("[{0}]", Name);
                }

                return alias;
            }
        }
    }
}
