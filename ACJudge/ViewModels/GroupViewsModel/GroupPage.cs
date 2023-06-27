using System;
using System.Collections.Generic;

namespace ACJudge.ViewModels.GroupViewsModel;

public class GroupPage
{
    public GroupPage(IEnumerable<ViewGroupModel> models, int currentPage, int numberOfPages)
    {
        GroupModels = models;
        CurrentPage = currentPage;
        NumberOfPages = numberOfPages;
        PageNumbers = new List<int>();
        for (var i = Math.Max(currentPage - 3, 1); PageNumbers.Count < 4 && i <= NumberOfPages; i++)
        {
            PageNumbers.Add(i);
        }
    }
    public int CurrentPage { get; set; }
    public int NumberOfPages { get; set;}
    public bool IsFirstPage => CurrentPage == 1;
    public bool IsLastPage => CurrentPage == NumberOfPages;
    public IEnumerable<ViewGroupModel> GroupModels { get; set; }
    public List<int> PageNumbers { get; set; }
}