using TwitterClone.APIs.Dtos;
using TwitterClone.Infrastructure.Models;

namespace TwitterClone.APIs.Extensions;

public static class TweetsExtensions
{
    public static TweetDto ToDto(this Tweet model)
    {
        return new TweetDto
        {
            Comment = model.Comment,
            Content = model.Content,
            CreatedAt = model.CreatedAt,
            Id = model.Id,
            Likes = model.Likes?.Select(x => new LikeIdDto { Id = x.Id }).ToList(),
            Retweets = model.Retweets?.Select(x => new RetweetIdDto { Id = x.Id }).ToList(),
            UpdatedAt = model.UpdatedAt,
            User = new UserIdDto { Id = model.UserId },
        };
    }

    public static Tweet ToModel(this TweetUpdateInput updateDto, TweetIdDto idDto)
    {
        var tweet = new Tweet
        {
            Id = idDto.Id,
            Comment = updateDto.Comment,
            Content = updateDto.Content
        };

        // map required fields
        if (updateDto.CreatedAt != null)
        {
            tweet.CreatedAt = updateDto.CreatedAt.Value;
        }
        if (updateDto.UpdatedAt != null)
        {
            tweet.UpdatedAt = updateDto.UpdatedAt.Value;
        }

        return tweet;
    }
}
