using ClassifiedsService;

class Filter
{
    public int[]? CategoriesIds { get; set; }
    public string? Term { get; set; }
    public string OrderingField { get; set; } = nameof(ListingDto.Price);
    public Ordering Ordering { get; set; } = Ordering.Asc;
    public int Page { get; set; }
    public int PageSize { get; set; }
}