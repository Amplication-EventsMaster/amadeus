namespace TwitterClone.APIs.Dtos;

public class TweetDto : TweetIdDto
{
    public string? Comment { get; set; }

    public string? Content { get; set; }

    public DateTime CreatedAt { get; set; }

    public List<LikeIdDto>? Likes { get; set; }

    public List<RetweetIdDto>? Retweets { get; set; }

    public DateTime UpdatedAt { get; set; }

    public UserIdDto? User { get; set; }
}
