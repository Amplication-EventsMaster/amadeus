using TwitterClone.APIs.Common;
using TwitterClone.APIs.Dtos;

namespace TwitterClone.APIs;

public interface IUsersService
{
    /// <summary>
    /// Create one User
    /// </summary>
    public Task<UserDto> CreateUser(UserCreateInput userDto);

    /// <summary>
    /// Delete one User
    /// </summary>
    public Task DeleteUser(UserIdDto idDto);

    /// <summary>
    /// Find many Users
    /// </summary>
    public Task<List<UserDto>> Users(UserFindMany findManyArgs);

    /// <summary>
    /// Get one User
    /// </summary>
    public Task<UserDto> User(UserIdDto idDto);

    /// <summary>
    /// Update one User
    /// </summary>
    public Task UpdateUser(UserIdDto idDto, UserUpdateInput updateDto);

    /// <summary>
    /// Connect multiple Likes records to User
    /// </summary>
    public Task ConnectLikes(UserIdDto idDto, LikeIdDto[] likesId);

    /// <summary>
    /// Connect multiple Retweets records to User
    /// </summary>
    public Task ConnectRetweets(UserIdDto idDto, RetweetIdDto[] retweetsId);

    /// <summary>
    /// Connect multiple Tweets records to User
    /// </summary>
    public Task ConnectTweets(UserIdDto idDto, TweetIdDto[] tweetsId);

    /// <summary>
    /// Disconnect multiple Likes records from User
    /// </summary>
    public Task DisconnectLikes(UserIdDto idDto, LikeIdDto[] likesId);

    /// <summary>
    /// Disconnect multiple Retweets records from User
    /// </summary>
    public Task DisconnectRetweets(UserIdDto idDto, RetweetIdDto[] retweetsId);

    /// <summary>
    /// Disconnect multiple Tweets records from User
    /// </summary>
    public Task DisconnectTweets(UserIdDto idDto, TweetIdDto[] tweetsId);

    /// <summary>
    /// Find multiple Likes records for User
    /// </summary>
    public Task<List<LikeDto>> FindLikes(UserIdDto idDto, LikeFindMany LikeFindMany);

    /// <summary>
    /// Find multiple Retweets records for User
    /// </summary>
    public Task<List<RetweetDto>> FindRetweets(UserIdDto idDto, RetweetFindMany RetweetFindMany);

    /// <summary>
    /// Find multiple Tweets records for User
    /// </summary>
    public Task<List<TweetDto>> FindTweets(UserIdDto idDto, TweetFindMany TweetFindMany);

    /// <summary>
    /// Meta data about User records
    /// </summary>
    public Task<MetadataDto> UsersMeta(UserFindMany findManyArgs);

    /// <summary>
    /// Update multiple Likes records for User
    /// </summary>
    public Task UpdateLikes(UserIdDto idDto, LikeIdDto[] likesId);

    /// <summary>
    /// Update multiple Retweets records for User
    /// </summary>
    public Task UpdateRetweets(UserIdDto idDto, RetweetIdDto[] retweetsId);

    /// <summary>
    /// Update multiple Tweets records for User
    /// </summary>
    public Task UpdateTweets(UserIdDto idDto, TweetIdDto[] tweetsId);
}
