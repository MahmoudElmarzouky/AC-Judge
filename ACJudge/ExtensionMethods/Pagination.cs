using System.Collections.Generic;
using System.Linq;

namespace ACJudge.ExtensionMethods;

public static class Pagination
{
    public static IEnumerable<T> Paginate<T>(this IEnumerable<T> list, int page, int size)
    {
        // validate page and size 
        if (page <= 0) page = 1;
        if (size <= 0) size = 10;
        
        var currentPage = list.Skip((page - 1) * size).Take(size).ToList();
        return currentPage;
    }
}