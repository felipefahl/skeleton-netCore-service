using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Skeleton.ServiceName.Utils.EfExtensions
{
    public static partial class CustomExtensions
    {       
        public static IQueryable<T> Paged<T>(this IQueryable<T> query, int pageNumber, int pageSize) where T : class
        {
            query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            return query;
        }
    }
}
