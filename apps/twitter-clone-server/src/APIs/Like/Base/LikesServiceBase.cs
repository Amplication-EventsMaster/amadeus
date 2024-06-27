using Microsoft.EntityFrameworkCore;
using TwitterClone.APIs;
using TwitterClone.APIs.Common;
using TwitterClone.APIs.Dtos;
using TwitterClone.APIs.Errors;
using TwitterClone.APIs.Extensions;
using TwitterClone.Infrastructure;
using TwitterClone.Infrastructure.Models;

namespace TwitterClone.APIs;

public abstract class LikesServiceBase : ILikesService
{
    protected readonly TwitterCloneDbContext _context;

    public LikesServiceBase(TwitterCloneDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Create one Like
    /// </summary>
    public async Task<LikeDto> CreateLike(LikeCreateInput createDto)
    {
        var like = new Like { CreatedAt = createDto.CreatedAt, UpdatedAt = createDto.UpdatedAt };

        if (createDto.Id != null)
        {
            like.Id = createDto.Id;
        }
        if (createDto.Tweet != null)
        {
            like.Tweet = await _context
                .Tweets.Where(tweet => createDto.Tweet.Id == tweet.Id)
                .FirstOrDefaultAsync();
        }

        if (createDto.User != null)
        {
            like.User = await _context
                .Users.Where(user => createDto.User.Id == user.Id)
                .FirstOrDefaultAsync();
        }

        _context.Likes.Add(like);
        await _context.SaveChangesAsync();

        var result = await _context.FindAsync<Like>(like.Id);

        if (result == null)
        {
            throw new NotFoundException();
        }

        return result.ToDto();
    }

    /// <summary>
    /// Delete one Like
    /// </summary>
    public async Task DeleteLike(LikeIdDto idDto)
    {
        var like = await _context.Likes.FindAsync(idDto.Id);
        if (like == null)
        {
            throw new NotFoundException();
        }

        _context.Likes.Remove(like);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Find many Likes
    /// </summary>
    public async Task<List<LikeDto>> Likes(LikeFindMany findManyArgs)
    {
        var likes = await _context
            .Likes.Include(x => x.Tweet)
            .Include(x => x.User)
            .ApplyWhere(findManyArgs.Where)
            .ApplySkip(findManyArgs.Skip)
            .ApplyTake(findManyArgs.Take)
            .ApplyOrderBy(findManyArgs.SortBy)
            .ToListAsync();
        return likes.ConvertAll(like => like.ToDto());
    }

    /// <summary>
    /// Get one Like
    /// </summary>
    public async Task<LikeDto> Like(LikeIdDto idDto)
    {
        var likes = await this.Likes(
            new LikeFindMany { Where = new LikeWhereInput { Id = idDto.Id } }
        );
        var like = likes.FirstOrDefault();
        if (like == null)
        {
            throw new NotFoundException();
        }

        return like;
    }

    /// <summary>
    /// Get a Tweet record for Like
    /// </summary>
    public async Task<TweetDto> GetTweet(LikeIdDto idDto)
    {
        var like = await _context
            .Likes.Where(like => like.Id == idDto.Id)
            .Include(like => like.Tweet)
            .FirstOrDefaultAsync();
        if (like == null)
        {
            throw new NotFoundException();
        }
        return like.Tweet.ToDto();
    }

    /// <summary>
    /// Get a User record for Like
    /// </summary>
    public async Task<UserDto> GetUser(LikeIdDto idDto)
    {
        var like = await _context
            .Likes.Where(like => like.Id == idDto.Id)
            .Include(like => like.User)
            .FirstOrDefaultAsync();
        if (like == null)
        {
            throw new NotFoundException();
        }
        return like.User.ToDto();
    }

    /// <summary>
    /// Meta data about Like records
    /// </summary>
    public async Task<MetadataDto> LikesMeta(LikeFindMany findManyArgs)
    {
        var count = await _context.Likes.ApplyWhere(findManyArgs.Where).CountAsync();

        return new MetadataDto { Count = count };
    }

    /// <summary>
    /// Update one Like
    /// </summary>
    public async Task UpdateLike(LikeIdDto idDto, LikeUpdateInput updateDto)
    {
        var like = updateDto.ToModel(idDto);

        _context.Entry(like).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Likes.Any(e => e.Id == like.Id))
            {
                throw new NotFoundException();
            }
            else
            {
                throw;
            }
        }
    }
}
