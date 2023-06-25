using System;
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

    public static IEnumerable<int> GetAccessiblePagesNumbers(int startPage, int currentPage, int endPage)
    {
        var pages = new List<int>();
        for (var i = Math.Max(currentPage - 3, startPage); i <= endPage && pages.Count < 5; i++)
        {
            if (i - currentPage == 1 || i - currentPage == -1) 
                continue;
            pages.Add(i);
        }
        return pages;
    }
}