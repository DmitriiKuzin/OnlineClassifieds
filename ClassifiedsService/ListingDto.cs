namespace ClassifiedsService;

public class ListingDto
{
    public long Id { get; set; }
    public required string Title { get; set; }
    public string? StatusText { get; set; }
    public required string Description { get; set; }
    public string? Price { get; set; }
    public int CategoryId { get; set; }
}