using TwitterClone.APIs.Common;
using TwitterClone.APIs.Dtos;

namespace TwitterClone.APIs;

public interface ILikesService
{
    /// <summary>
    /// Create one Like
    /// </summary>
    public Task<LikeDto> CreateLike(LikeCreateInput likeDto);

    /// <summary>
    /// Delete one Like
    /// </summary>
    public Task DeleteLike(LikeIdDto idDto);

    /// <summary>
    /// Find many Likes
    /// </summary>
    public Task<List<LikeDto>> Likes(LikeFindMany findManyArgs);

    /// <summary>
    /// Get one Like
    /// </summary>
    public Task<LikeDto> Like(LikeIdDto idDto);

    /// <summary>
    /// Get a Tweet record for Like
    /// </summary>
    public Task<TweetDto> GetTweet(LikeIdDto idDto);

    /// <summary>
    /// Get a User record for Like
    /// </summary>
    public Task<UserDto> GetUser(LikeIdDto idDto);

    /// <summary>
    /// Meta data about Like records
    /// </summary>
    public Task<MetadataDto> LikesMeta(LikeFindMany findManyArgs);

    /// <summary>
    /// Update one Like
    /// </summary>
    public Task UpdateLike(LikeIdDto idDto, LikeUpdateInput updateDto);
}
