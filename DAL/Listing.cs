namespace DAL;

public class Listing
{
    public long Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }

    public DateTime Created { get; set; }

    public bool Moderated { get; set; }

    public ListingStatus Status { get; set; }

    public string StatusText =>
        Status == ListingStatus.Created ? "Создано" :
        Status == ListingStatus.Moderation ? "На модерации" :
        Status == ListingStatus.ModerationFailed ? "Модерация не пройдена" :
        Status == ListingStatus.ModerationSucceed ? "Модерация пройдена" : "Опубликовано";

    public string? Price { get; set; }
    public int CategoryId { get; set; }
    public required Category Category { get; set; }
    public long UserProfileId { get; set; }
    public required UserProfile UserProfile { get; set; }
}

public class Category
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string SystemName { get; set; }
    public int? ParentCategoryId { get; set; }
    public Category? ParentCategory { get; set; }
}

public enum ListingStatus
{
    Created,
    Moderation,
    ModerationFailed,
    ModerationSucceed,
    Published
}