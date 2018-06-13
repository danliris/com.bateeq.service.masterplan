using Com.Moonlay.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;

namespace Com.Bateeq.Service.Masterplan.Lib.Utils
{
    public static class QueryHelper<TModel>
        where TModel : StandardEntity, IValidatableObject
    {
        public static IQueryable<TModel> Filter(IQueryable<TModel> query, Dictionary<string, object> filterDictionary)
        {
            if (filterDictionary != null && !filterDictionary.Count.Equals(0))
            {
                foreach (var f in filterDictionary)
                {
                    string key = f.Key;
                    object Value = f.Value;
                    string filterQuery = string.Concat(string.Empty, key, " == @0");

                    query = query.Where(filterQuery, Value);
                }
            }
            return query;
        }

        public static IQueryable<TModel> Order(IQueryable<TModel> query, Dictionary<string, string> orderDictionary)
        {
            /* Default Order */
            if (orderDictionary.Count.Equals(0))
            {
                orderDictionary.Add("LastModifiedUtc", General.DESCENDING);

                query = query.OrderByDescending(b => b.LastModifiedUtc);
            }
            /* Custom Order */
            else
            {
                string key = orderDictionary.Keys.First();
                string orderType = orderDictionary[key];
                string transformKey = General.TransformOrderBy(key);

                BindingFlags IgnoreCase = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance;

                query = orderType.Equals(General.ASCENDING) ?
                    query.OrderBy(b => b.GetType().GetProperty(transformKey, IgnoreCase).GetValue(b)) :
                    query.OrderByDescending(b => b.GetType().GetProperty(transformKey, IgnoreCase).GetValue(b));
            }
            return query;
        }

        public static IQueryable<TModel> Search(IQueryable<TModel> query, List<string> searchAttributes, string keyword)
        {
            if (keyword != null)
            {
                query = query.Where(General.BuildSearch(searchAttributes), keyword);
            }
            return query;
        }
    }
}
