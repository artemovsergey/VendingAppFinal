namespace VendingApp.API.Configurations;

public class Option
{
    public int PageSize { get; set; } = 10;
    public int PageNumber { get; set; } = 1;

    public string? Search { get; set; } = string.Empty;
    public string? Filter { get; set; } = string.Empty;
    public required string? Sort { get; set; }
    public required string? SortDirection { get; set; } = "asc";
}
