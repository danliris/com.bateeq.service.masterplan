﻿using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Dynamic.Core;

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
            /* Default Query Order By */
            if (orderDictionary.Count.Equals(0))
            {
                orderDictionary.Add("LastModifiedUtc", "desc");
                query = query.OrderByDescending(b => b.LastModifiedUtc);
            }
            /* Custom Query Order BY */
            else
            {
                string Key = orderDictionary.Keys.First();
                string OrderByType = orderDictionary[Key];

                try
                {
                    query = query.OrderBy(string.Concat(Key.Replace(".", ""), " ", OrderByType));
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }
                
            }
            return query;
        }

        public static IQueryable<TModel> Search(IQueryable<TModel> query, List<string> searchAttributes, string keyword, bool ToLowerCase = false)
        {
            /* Search with Keyword */
            if (keyword != null)
            {
                string SearchQuery = String.Empty;
                foreach (string Attribute in searchAttributes)
                {
                    if (Attribute.Contains("."))
                    {
                        var Key = Attribute.Split(".");
                        SearchQuery = string.Concat(SearchQuery, Key[0], $".Any({Key[1]}.Contains(@0)) OR ");
                    }
                    else
                    {
                        SearchQuery = string.Concat(SearchQuery, Attribute, ".Contains(@0) OR ");
                    }
                }

                SearchQuery = SearchQuery.Remove(SearchQuery.Length - 4);

                if (ToLowerCase)
                {
                    SearchQuery = SearchQuery.Replace(".Contains(@0)", ".ToLower().Contains(@0)");
                    keyword = keyword.ToLower();
                }

                query = query.Where(SearchQuery, keyword);
            }
            return query;
        }
    }
}
