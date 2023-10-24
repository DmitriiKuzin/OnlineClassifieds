namespace MQ;

public record ListingPublishRequested(long ListingId, long UserProfileId);
public record ModerationSucceed(long ListingId, long UserProfileId);
public record ModerationFailed(long ListingId, long UserProfileId);
public record ListingPublished(long ListingId, long UserProfileId);