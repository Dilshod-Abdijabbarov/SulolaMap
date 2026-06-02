using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.Extensions;

public static class FilterExtensions
{
    private static readonly ConcurrentDictionary<string, PropertyInfo> PropertyCache = new();
    public static IQueryable<T> ApplyFilters<T>(this IQueryable<T> query, Dictionary<string, string>? filters)
    {
        if (filters == null || !filters.Any()) return query;

        var parameter = Expression.Parameter(typeof(T), "x");
        Expression? aggregateExpression = null;

        foreach (var filter in filters)
        {
            var property = typeof(T).GetProperty(filter.Key, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (property == null) continue;

            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            Expression comparison;

            if (property.PropertyType == typeof(string))
            {
                // String uchun optimal StartsWith
                var method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                var value = Expression.Constant(filter.Value, typeof(string));
                comparison = Expression.Call(propertyAccess, method, value);
            }
            else
            {
                // Int va boshqa turlar uchun Equals
                var converter = TypeDescriptor.GetConverter(property.PropertyType);
                var convertedValue = converter.ConvertFrom(filter.Value);
                var constant = Expression.Constant(convertedValue, property.PropertyType);
                comparison = Expression.Equal(propertyAccess, constant);
            }

            aggregateExpression = aggregateExpression == null
                ? comparison
                : Expression.AndAlso(aggregateExpression, comparison);
        }

        if (aggregateExpression == null) return query;

        var lambda = Expression.Lambda<Func<T, bool>>(aggregateExpression, parameter);
        return query.Where(lambda);
    }


    public static IQueryable<T> ApplySorting<T>(this IQueryable<T> query, string sortBy, bool isDescending)
    {
        if (string.IsNullOrWhiteSpace(sortBy)) return query;

        var type = typeof(T);
        var cacheKey = $"{type.FullName}_{sortBy}";

        // Propertyni keshdan qidiramiz yoki Reflection orqali topamiz
        if (!PropertyCache.TryGetValue(cacheKey, out var property))
        {
            property = type.GetProperty(sortBy, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (property != null) PropertyCache[cacheKey] = property;
        }

        if (property == null) return query;

        var parameter = Expression.Parameter(type, "x");
        var propertyAccess = Expression.MakeMemberAccess(parameter, property);
        var orderByExp = Expression.Lambda(propertyAccess, parameter);

        string methodName = isDescending ? "OrderByDescending" : "OrderBy";

        var resultExp = Expression.Call(
            typeof(Queryable),
            methodName,
            new Type[] { type, property.PropertyType },
            query.Expression,
            Expression.Quote(orderByExp)
        );

        return query.Provider.CreateQuery<T>(resultExp);
    }

    public static async Task<PagedResult<T>> ToPagedListAsync<T>(this IQueryable<T> query, int pageNumber, int pageSize)
    {
        var totalCount = await query.CountAsync();

        var items = await query
            .Skip((pageNumber) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResult<T>
        {
            Items = items,
            TotalItems = totalCount,
        };
    }

}