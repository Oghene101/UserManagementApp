namespace UserManagementApp.Dtos;

public class PaginationFilter(int pageNumber = 1, int pageSize = 10)
{
    public int PageNumber => pageNumber < 1 ? 1 : pageNumber;
    public int PageSize => pageSize is > 10 or < 1 ? 10 : pageSize;
}

public record PaginatorDto<T>(
    T? PageItems,
    int PageSize,
    int CurrentPage,
    int NumberOfPages);