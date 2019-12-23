using Entities.Models;
using System.Collections.Generic;

namespace Entities.Helpers
{
    public interface ISortHelper<T>
    {
        IEnumerable<T> ApplySort(IEnumerable<T> entities, QueryStringParameters queryString);
    }
}
