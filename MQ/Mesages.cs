namespace MQ;

public record ListingPublishRequested(long ListingId);
public record ModerationSucceed(long ListingId);
public record ModerationFailed(long ListingId);