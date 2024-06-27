using Microsoft.EntityFrameworkCore;
using TwitterClone.APIs;
using TwitterClone.APIs.Common;
using TwitterClone.APIs.Dtos;
using TwitterClone.APIs.Errors;
using TwitterClone.APIs.Extensions;
using TwitterClone.Infrastructure;
using TwitterClone.Infrastructure.Models;

namespace TwitterClone.APIs;

public abstract class RetweetsServiceBase : IRetweetsService
{
    protected readonly TwitterCloneDbContext _context;

    public RetweetsServiceBase(TwitterCloneDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Create one Retweet
    /// </summary>
    public async Task<RetweetDto> CreateRetweet(RetweetCreateInput createDto)
    {
        var retweet = new Retweet
        {
            CreatedAt = createDto.CreatedAt,
            UpdatedAt = createDto.UpdatedAt
        };

        if (createDto.Id != null)
        {
            retweet.Id = createDto.Id;
        }
        if (createDto.Tweet != null)
        {
            retweet.Tweet = await _context
                .Tweets.Where(tweet => createDto.Tweet.Id == tweet.Id)
                .FirstOrDefaultAsync();
        }

        if (createDto.User != null)
        {
            retweet.User = await _context
                .Users.Where(user => createDto.User.Id == user.Id)
                .FirstOrDefaultAsync();
        }

        _context.Retweets.Add(retweet);
        await _context.SaveChangesAsync();

        var result = await _context.FindAsync<Retweet>(retweet.Id);

        if (result == null)
        {
            throw new NotFoundException();
        }

        return result.ToDto();
    }

    /// <summary>
    /// Delete one Retweet
    /// </summary>
    public async Task DeleteRetweet(RetweetIdDto idDto)
    {
        var retweet = await _context.Retweets.FindAsync(idDto.Id);
        if (retweet == null)
        {
            throw new NotFoundException();
        }

        _context.Retweets.Remove(retweet);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Find many Retweets
    /// </summary>
    public async Task<List<RetweetDto>> Retweets(RetweetFindMany findManyArgs)
    {
        var retweets = await _context
            .Retweets.Include(x => x.Tweet)
            .Include(x => x.User)
            .ApplyWhere(findManyArgs.Where)
            .ApplySkip(findManyArgs.Skip)
            .ApplyTake(findManyArgs.Take)
            .ApplyOrderBy(findManyArgs.SortBy)
            .ToListAsync();
        return retweets.ConvertAll(retweet => retweet.ToDto());
    }

    /// <summary>
    /// Get one Retweet
    /// </summary>
    public async Task<RetweetDto> Retweet(RetweetIdDto idDto)
    {
        var retweets = await this.Retweets(
            new RetweetFindMany { Where = new RetweetWhereInput { Id = idDto.Id } }
        );
        var retweet = retweets.FirstOrDefault();
        if (retweet == null)
        {
            throw new NotFoundException();
        }

        return retweet;
    }

    /// <summary>
    /// Get a Tweet record for Retweet
    /// </summary>
    public async Task<TweetDto> GetTweet(RetweetIdDto idDto)
    {
        var retweet = await _context
            .Retweets.Where(retweet => retweet.Id == idDto.Id)
            .Include(retweet => retweet.Tweet)
            .FirstOrDefaultAsync();
        if (retweet == null)
        {
            throw new NotFoundException();
        }
        return retweet.Tweet.ToDto();
    }

    /// <summary>
    /// Get a User record for Retweet
    /// </summary>
    public async Task<UserDto> GetUser(RetweetIdDto idDto)
    {
        var retweet = await _context
            .Retweets.Where(retweet => retweet.Id == idDto.Id)
            .Include(retweet => retweet.User)
            .FirstOrDefaultAsync();
        if (retweet == null)
        {
            throw new NotFoundException();
        }
        return retweet.User.ToDto();
    }

    /// <summary>
    /// Meta data about Retweet records
    /// </summary>
    public async Task<MetadataDto> RetweetsMeta(RetweetFindMany findManyArgs)
    {
        var count = await _context.Retweets.ApplyWhere(findManyArgs.Where).CountAsync();

        return new MetadataDto { Count = count };
    }

    /// <summary>
    /// Update one Retweet
    /// </summary>
    public async Task UpdateRetweet(RetweetIdDto idDto, RetweetUpdateInput updateDto)
    {
        var retweet = updateDto.ToModel(idDto);

        _context.Entry(retweet).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Retweets.Any(e => e.Id == retweet.Id))
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
