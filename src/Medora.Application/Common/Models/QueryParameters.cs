namespace Medora.Application.Common.Models;

public sealed record QueryParameters(
    int PageNumber = 1,
    int PageSize = 25,
    string? Search = null,
    string? SortBy = null,
    string? SortDirection = "asc")
{
    public int SafePageNumber => PageNumber < 1 ? 1 : PageNumber;
    public int SafePageSize => PageSize is < 1 or > 200 ? 25 : PageSize;
}
