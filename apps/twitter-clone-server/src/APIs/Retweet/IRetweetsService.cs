using TwitterClone.APIs.Common;
using TwitterClone.APIs.Dtos;

namespace TwitterClone.APIs;

public interface IRetweetsService
{
    /// <summary>
    /// Create one Retweet
    /// </summary>
    public Task<RetweetDto> CreateRetweet(RetweetCreateInput retweetDto);

    /// <summary>
    /// Delete one Retweet
    /// </summary>
    public Task DeleteRetweet(RetweetIdDto idDto);

    /// <summary>
    /// Find many Retweets
    /// </summary>
    public Task<List<RetweetDto>> Retweets(RetweetFindMany findManyArgs);

    /// <summary>
    /// Get one Retweet
    /// </summary>
    public Task<RetweetDto> Retweet(RetweetIdDto idDto);

    /// <summary>
    /// Get a Tweet record for Retweet
    /// </summary>
    public Task<TweetDto> GetTweet(RetweetIdDto idDto);

    /// <summary>
    /// Get a User record for Retweet
    /// </summary>
    public Task<UserDto> GetUser(RetweetIdDto idDto);

    /// <summary>
    /// Meta data about Retweet records
    /// </summary>
    public Task<MetadataDto> RetweetsMeta(RetweetFindMany findManyArgs);

    /// <summary>
    /// Update one Retweet
    /// </summary>
    public Task UpdateRetweet(RetweetIdDto idDto, RetweetUpdateInput updateDto);
}
