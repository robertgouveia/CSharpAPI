namespace Shared.RequestFeatures;

public class PagedList<T> : List<T>
{
    public MetaData MetaData { get; set; }

    // Constructor builds the MetaData
    public PagedList(List<T> items, int count, int pageNumber, int pageSize)
    {
        MetaData = new MetaData()
        {
            CurrentPage = pageNumber,
            // Rounding up due to allowing not a full page
            TotalPages = (int)Math.Ceiling(count / (double)pageSize),
            PageSize = pageSize,
            TotalSize = count,
        };
        
        AddRange(items); // Adds Items to List
    }

    // This allows for an IEnumerable to be turned into a PagedList
    public static PagedList<T> ToPagedList(IEnumerable<T> source, int pageNumber, int pageSize)
    {
        var count = source.Count();
        var items = source
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return new PagedList<T>(items, count, pageNumber, pageSize);
    }
}