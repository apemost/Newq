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

namespace Newq
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Target collection of statement or clause.
    /// </summary>
    public class Target : Customization, ICustomizable<Action<Target, Context>>
    {
        /// <summary>
        /// 
        /// </summary>
        protected Action<Target, Context> customization;

        /// <summary>
        /// 
        /// </summary>
        protected bool hasPerformed;

        /// <summary>
        /// Initializes a new instance of the <see cref="Target"/> class.
        /// </summary>
        /// <param name="context"></param>
        public Target(Context context) : base(context)
        {
            Items = new List<ICustomItem<Target>>();
        }

        /// <summary>
        /// 
        /// </summary>
        public List<ICustomItem<Target>> Items { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customization"></param>
        public void Customize(Action<Target, Context> customization)
        {
            this.customization = customization;
            hasPerformed = false;
        }

        /// <summary>
        /// Returns a result after customization be performed.
        /// </summary>
        public bool Perform()
        {
            if (!hasPerformed && customization != null)
            {
                Items.Clear();
                customization(this, context);
                hasPerformed = true;
            }

            return hasPerformed;
        }

        /// <summary>
        /// Returns a string that represents the current customization.
        /// </summary>
        /// <returns></returns>
        public override string GetCustomization()
        {
            var target = string.Empty;
            var targetItem = string.Empty;

            Perform();
            Items.ForEach(item => {
                if (item != null)
                {
                    targetItem = item.ToString();

                    if (!string.IsNullOrEmpty(targetItem))
                    {
                        target += string.Format(",{0}", targetItem);
                    }
                }
            });

            return target.Length > 0 ? target.Substring(1) : string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IReadOnlyList<OrderByColumn> GetOrderByColumns()
        {
            var list = new List<OrderByColumn>();

            Items.ForEach(item => {
                if (item is OrderByColumn)
                {
                    list.Add(item as OrderByColumn);
                }
            });

            return list;
        }

        /// <summary>
        /// Adds a object to the end of the target.
        /// </summary>
        /// <param name="item"></param>
        public void Add(ICustomItem<Target> item)
        {
            Items.Add(item);
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the target.
        /// </summary>
        /// <param name="item"></param>
        public void Remove(ICustomItem<Target> item)
        {
            Items.Remove(item);
        }

        /// <summary>
        /// <see cref="Add(ICustomItem{Target})"/>
        /// </summary>
        /// <param name="target"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static Target operator +(Target target, ICustomItem<Target> item)
        {
            target.Add(item);

            return target;
        }

        /// <summary>
        /// <see cref="Remove(ICustomItem{Target})"/>
        /// </summary>
        /// <param name="target"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static Target operator -(Target target, ICustomItem<Target> item)
        {
            target.Remove(item);

            return target;
        }
    }
}
