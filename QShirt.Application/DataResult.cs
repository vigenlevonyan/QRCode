using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telerik.DataSource;
using Telerik.DataSource.Extensions;

namespace QShirt.Application;

public class DataResult<T>
{
    public IEnumerable<T> Data { get; set; }

    public int Total { get; set; }
}

public static class QueryableExtension
{
    public static async Task<DataResult<T>> ToDataResultAsync<T>(
        this IQueryable<T> queryable,
        DataSourceRequest request)
    {
        var dataSourceResult = await queryable.ToDataSourceResultAsync(request);

        return new DataResult<T>
        {
            Data = dataSourceResult.Data.Cast<T>(),
            Total = dataSourceResult.Total
        };
    }
}