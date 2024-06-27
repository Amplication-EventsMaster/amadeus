using TwitterClone.APIs.Common;
using TwitterClone.APIs.Dtos;

namespace TwitterClone.APIs;

public interface IFollowersService
{
    /// <summary>
    /// Create one Follower
    /// </summary>
    public Task<FollowerDto> CreateFollower(FollowerCreateInput followerDto);

    /// <summary>
    /// Delete one Follower
    /// </summary>
    public Task DeleteFollower(FollowerIdDto idDto);

    /// <summary>
    /// Find many Followers
    /// </summary>
    public Task<List<FollowerDto>> Followers(FollowerFindMany findManyArgs);

    /// <summary>
    /// Connect multiple Followers records to Follower
    /// </summary>
    public Task ConnectFollowers(FollowerIdDto idDto, FollowerIdDto[] followersId);

    /// <summary>
    /// Disconnect multiple Followers records from Follower
    /// </summary>
    public Task DisconnectFollowers(FollowerIdDto idDto, FollowerIdDto[] followersId);

    /// <summary>
    /// Find multiple Followers records for Follower
    /// </summary>
    public Task<List<FollowerDto>> FindFollowers(
        FollowerIdDto idDto,
        FollowerFindMany FollowerFindMany
    );

    /// <summary>
    /// Get a Follower record for Follower
    /// </summary>
    public Task<FollowerDto> GetFollower(FollowerIdDto idDto);

    /// <summary>
    /// Meta data about Follower records
    /// </summary>
    public Task<MetadataDto> FollowersMeta(FollowerFindMany findManyArgs);

    /// <summary>
    /// Update multiple Followers records for Follower
    /// </summary>
    public Task UpdateFollowers(FollowerIdDto idDto, FollowerIdDto[] followersId);

    /// <summary>
    /// Get one Follower
    /// </summary>
    public Task<FollowerDto> Follower(FollowerIdDto idDto);

    /// <summary>
    /// Update one Follower
    /// </summary>
    public Task UpdateFollower(FollowerIdDto idDto, FollowerUpdateInput updateDto);
}
