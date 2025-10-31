namespace SalesManagementSystem.Application.Dtos.Paging;

public class PagedRequest
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}

public class Meta
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int Total { get; set; }
}

public class PagedResponse<T>
{
    public required T Data { get; set; }
    public required Meta Meta { get; set; }
    public object? Error { get; set; }
}


