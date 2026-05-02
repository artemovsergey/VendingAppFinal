namespace VendingApp.API.Response;

public class ApiResult<T>
    where T : class
{
    public bool Success { get; set; }
    public required int Count { get; set; }
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
    public int TotalPages
    {
        get => (int)Math.Ceiling(Count / (double)PageSize);
    }

    public bool HasNext
    {
        get => PageNumber < TotalPages;
    }

    public bool HasPreview
    {
        get => PageNumber > 1;
    }
    public required List<T> Data { get; set; }
}
