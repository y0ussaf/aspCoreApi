using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using System.Collections;
using System.Dynamic;
using Tp2.Utils;

namespace Tp2.Extensions
{
    public static class LinqExtensions
    {
       
        public static IEnumerable<T> Paginate<T>(this IEnumerable<T> source,int pageSize,int page)
        {
            return source.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public static IEnumerable<ExpandoObject> SelectByFieldsQuery<T>(this IEnumerable<T> source,string fieldsQuery)
        {
            var expandoObjectList = new List<ExpandoObject>();
            var propertyInfoList = new List<PropertyInfo>();
            if (string.IsNullOrEmpty(fieldsQuery))
            {
                var propertyInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                propertyInfoList.AddRange(propertyInfos);
            }
            else
            {
                var fields = fieldsQuery.Split(',');
                foreach (var field in fields)
                {
                    var propertyName = field.Trim();
                    var propertyInfo = typeof(T).GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                    if (propertyInfo == null)
                    {
                        continue;
                    }
                    propertyInfoList.Add(propertyInfo);
                }
                var idProperty = typeof(T).GetProperties().Where(p => p.Name.Contains("id", StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                if(idProperty != null)
                {
                    propertyInfoList.Add(idProperty);
                }
            }
            foreach ( T o in source)
            {
                var dataShapedObject = new ExpandoObject();
                foreach(var propertyInfo in propertyInfoList)
                {
                    var properyValue = propertyInfo.GetValue(o);
                    ((IDictionary<string, object>)dataShapedObject).Add(propertyInfo.Name, properyValue);

                }
                expandoObjectList.Add(dataShapedObject);
            }
            return expandoObjectList;
        }
        public static IEnumerable<T> OrderByQueryOrDefault<T>(this IQueryable<T> source,string orderByQuery,string defaultOrder,bool asc = true)
        {
            if (!source.Any())
                return source;
            var orderQuery = String.Empty;
           
            if (!string.IsNullOrWhiteSpace(orderByQuery))
            {
                var orderParams = orderByQuery.Trim().Split(',');
                var propertyInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                var orderQueryBuilder = new StringBuilder();

                foreach (var param in orderParams)
                {
                    if (string.IsNullOrWhiteSpace(param))
                        continue;

                    var propertyFromQueryName = param.Split(" ")[0];
                    var objectProperty = propertyInfos.FirstOrDefault(pi => pi.Name.Equals(propertyFromQueryName, StringComparison.InvariantCultureIgnoreCase));

                    if (objectProperty == null)
                        continue;

                    var sortingOrder = param.EndsWith(" desc") ? "descending" : "ascending";

                    orderQueryBuilder.Append($"{objectProperty.Name.ToString()} {sortingOrder}, ");
                }

                orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' ');
            }



            if (string.IsNullOrWhiteSpace(orderQuery))
            {
                orderQuery += $"{defaultOrder} {(asc ? "ascending" : "descending")}";
            }

            return source.OrderBy(orderQuery);
        }
        public static ExpandoObject shapeObject<T> (this T obj, string fieldsQuery)
        {
            var propertyInfoList = new List<PropertyInfo>();
            if (string.IsNullOrEmpty(fieldsQuery))
            {
                var propertyInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                propertyInfoList.AddRange(propertyInfos);
            }
            else
            {
                var fields = fieldsQuery.Split(',');
                foreach (var field in fields)
                {
                    var propertyName = field.Trim();
                    var propertyInfo = typeof(T).GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                    if (propertyInfo == null)
                    {
                        // we can throw here an expection to return bad request to the user
                        continue;
                    }
                    propertyInfoList.Add(propertyInfo);
                }
            }
          
            var dataShapedObject = new ExpandoObject();
            foreach (var propertyInfo in propertyInfoList)
             {
                  var properyValue = propertyInfo.GetValue(obj);
                  ((IDictionary<string, object>)dataShapedObject).Add(propertyInfo.Name, properyValue);
               

              }
            
            return dataShapedObject;
        }
      
    }
   
}
