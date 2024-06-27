using TwitterClone.APIs.Common;
using TwitterClone.APIs.Dtos;

namespace TwitterClone.APIs;

public interface ITweetsService
{
    /// <summary>
    /// Create one Tweet
    /// </summary>
    public Task<TweetDto> CreateTweet(TweetCreateInput tweetDto);

    /// <summary>
    /// Delete one Tweet
    /// </summary>
    public Task DeleteTweet(TweetIdDto idDto);

    /// <summary>
    /// Find many Tweets
    /// </summary>
    public Task<List<TweetDto>> Tweets(TweetFindMany findManyArgs);

    /// <summary>
    /// Get one Tweet
    /// </summary>
    public Task<TweetDto> Tweet(TweetIdDto idDto);

    /// <summary>
    /// Connect multiple Likes records to Tweet
    /// </summary>
    public Task ConnectLikes(TweetIdDto idDto, LikeIdDto[] likesId);

    /// <summary>
    /// Connect multiple Retweets records to Tweet
    /// </summary>
    public Task ConnectRetweets(TweetIdDto idDto, RetweetIdDto[] retweetsId);

    /// <summary>
    /// Disconnect multiple Likes records from Tweet
    /// </summary>
    public Task DisconnectLikes(TweetIdDto idDto, LikeIdDto[] likesId);

    /// <summary>
    /// Disconnect multiple Retweets records from Tweet
    /// </summary>
    public Task DisconnectRetweets(TweetIdDto idDto, RetweetIdDto[] retweetsId);

    /// <summary>
    /// Find multiple Likes records for Tweet
    /// </summary>
    public Task<List<LikeDto>> FindLikes(TweetIdDto idDto, LikeFindMany LikeFindMany);

    /// <summary>
    /// Find multiple Retweets records for Tweet
    /// </summary>
    public Task<List<RetweetDto>> FindRetweets(TweetIdDto idDto, RetweetFindMany RetweetFindMany);

    /// <summary>
    /// Get a User record for Tweet
    /// </summary>
    public Task<UserDto> GetUser(TweetIdDto idDto);

    /// <summary>
    /// Meta data about Tweet records
    /// </summary>
    public Task<MetadataDto> TweetsMeta(TweetFindMany findManyArgs);

    /// <summary>
    /// Update multiple Likes records for Tweet
    /// </summary>
    public Task UpdateLikes(TweetIdDto idDto, LikeIdDto[] likesId);

    /// <summary>
    /// Update multiple Retweets records for Tweet
    /// </summary>
    public Task UpdateRetweets(TweetIdDto idDto, RetweetIdDto[] retweetsId);

    /// <summary>
    /// Update one Tweet
    /// </summary>
    public Task UpdateTweet(TweetIdDto idDto, TweetUpdateInput updateDto);
}
