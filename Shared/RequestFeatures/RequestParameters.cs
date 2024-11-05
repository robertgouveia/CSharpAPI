namespace Shared.RequestFeatures;

// Page
public abstract class RequestParameters
{
    private const int MaxPageSize = 50; // Setting maximum page size
    private int _pageSize = 10;
    
    public int PageNumber { get; set; } = 1; // Default
    public int PageSize
    {
        get => _pageSize;
        // If the set value is more than Max then set it to Max
        set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
    }
    
    public string? OrderBy { get; set; }
}