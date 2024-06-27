using Microsoft.EntityFrameworkCore;
using TwitterClone.APIs;
using TwitterClone.APIs.Common;
using TwitterClone.APIs.Dtos;
using TwitterClone.APIs.Errors;
using TwitterClone.APIs.Extensions;
using TwitterClone.Infrastructure;
using TwitterClone.Infrastructure.Models;

namespace TwitterClone.APIs;

public abstract class TweetsServiceBase : ITweetsService
{
    protected readonly TwitterCloneDbContext _context;

    public TweetsServiceBase(TwitterCloneDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Create one Tweet
    /// </summary>
    public async Task<TweetDto> CreateTweet(TweetCreateInput createDto)
    {
        var tweet = new Tweet
        {
            Comment = createDto.Comment,
            Content = createDto.Content,
            CreatedAt = createDto.CreatedAt,
            UpdatedAt = createDto.UpdatedAt
        };

        if (createDto.Id != null)
        {
            tweet.Id = createDto.Id;
        }
        if (createDto.Retweets != null)
        {
            tweet.Retweets = await _context
                .Retweets.Where(retweet =>
                    createDto.Retweets.Select(t => t.Id).Contains(retweet.Id)
                )
                .ToListAsync();
        }

        if (createDto.Likes != null)
        {
            tweet.Likes = await _context
                .Likes.Where(like => createDto.Likes.Select(t => t.Id).Contains(like.Id))
                .ToListAsync();
        }

        if (createDto.User != null)
        {
            tweet.User = await _context
                .Users.Where(user => createDto.User.Id == user.Id)
                .FirstOrDefaultAsync();
        }

        _context.Tweets.Add(tweet);
        await _context.SaveChangesAsync();

        var result = await _context.FindAsync<Tweet>(tweet.Id);

        if (result == null)
        {
            throw new NotFoundException();
        }

        return result.ToDto();
    }

    /// <summary>
    /// Delete one Tweet
    /// </summary>
    public async Task DeleteTweet(TweetIdDto idDto)
    {
        var tweet = await _context.Tweets.FindAsync(idDto.Id);
        if (tweet == null)
        {
            throw new NotFoundException();
        }

        _context.Tweets.Remove(tweet);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Find many Tweets
    /// </summary>
    public async Task<List<TweetDto>> Tweets(TweetFindMany findManyArgs)
    {
        var tweets = await _context
            .Tweets.Include(x => x.Retweets)
            .Include(x => x.Likes)
            .Include(x => x.User)
            .ApplyWhere(findManyArgs.Where)
            .ApplySkip(findManyArgs.Skip)
            .ApplyTake(findManyArgs.Take)
            .ApplyOrderBy(findManyArgs.SortBy)
            .ToListAsync();
        return tweets.ConvertAll(tweet => tweet.ToDto());
    }

    /// <summary>
    /// Get one Tweet
    /// </summary>
    public async Task<TweetDto> Tweet(TweetIdDto idDto)
    {
        var tweets = await this.Tweets(
            new TweetFindMany { Where = new TweetWhereInput { Id = idDto.Id } }
        );
        var tweet = tweets.FirstOrDefault();
        if (tweet == null)
        {
            throw new NotFoundException();
        }

        return tweet;
    }

    /// <summary>
    /// Connect multiple Likes records to Tweet
    /// </summary>
    public async Task ConnectLikes(TweetIdDto idDto, LikeIdDto[] likesId)
    {
        var tweet = await _context
            .Tweets.Include(x => x.Likes)
            .FirstOrDefaultAsync(x => x.Id == idDto.Id);
        if (tweet == null)
        {
            throw new NotFoundException();
        }

        var likes = await _context
            .Likes.Where(t => likesId.Select(x => x.Id).Contains(t.Id))
            .ToListAsync();
        if (likes.Count == 0)
        {
            throw new NotFoundException();
        }

        var likesToConnect = likes.Except(tweet.Likes);

        foreach (var like in likesToConnect)
        {
            tweet.Likes.Add(like);
        }

        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Connect multiple Retweets records to Tweet
    /// </summary>
    public async Task ConnectRetweets(TweetIdDto idDto, RetweetIdDto[] retweetsId)
    {
        var tweet = await _context
            .Tweets.Include(x => x.Retweets)
            .FirstOrDefaultAsync(x => x.Id == idDto.Id);
        if (tweet == null)
        {
            throw new NotFoundException();
        }

        var retweets = await _context
            .Retweets.Where(t => retweetsId.Select(x => x.Id).Contains(t.Id))
            .ToListAsync();
        if (retweets.Count == 0)
        {
            throw new NotFoundException();
        }

        var retweetsToConnect = retweets.Except(tweet.Retweets);

        foreach (var retweet in retweetsToConnect)
        {
            tweet.Retweets.Add(retweet);
        }

        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Disconnect multiple Likes records from Tweet
    /// </summary>
    public async Task DisconnectLikes(TweetIdDto idDto, LikeIdDto[] likesId)
    {
        var tweet = await _context
            .Tweets.Include(x => x.Likes)
            .FirstOrDefaultAsync(x => x.Id == idDto.Id);
        if (tweet == null)
        {
            throw new NotFoundException();
        }

        var likes = await _context
            .Likes.Where(t => likesId.Select(x => x.Id).Contains(t.Id))
            .ToListAsync();

        foreach (var like in likes)
        {
            tweet.Likes?.Remove(like);
        }
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Disconnect multiple Retweets records from Tweet
    /// </summary>
    public async Task DisconnectRetweets(TweetIdDto idDto, RetweetIdDto[] retweetsId)
    {
        var tweet = await _context
            .Tweets.Include(x => x.Retweets)
            .FirstOrDefaultAsync(x => x.Id == idDto.Id);
        if (tweet == null)
        {
            throw new NotFoundException();
        }

        var retweets = await _context
            .Retweets.Where(t => retweetsId.Select(x => x.Id).Contains(t.Id))
            .ToListAsync();

        foreach (var retweet in retweets)
        {
            tweet.Retweets?.Remove(retweet);
        }
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Find multiple Likes records for Tweet
    /// </summary>
    public async Task<List<LikeDto>> FindLikes(TweetIdDto idDto, LikeFindMany tweetFindMany)
    {
        var likes = await _context
            .Likes.Where(m => m.TweetId == idDto.Id)
            .ApplyWhere(tweetFindMany.Where)
            .ApplySkip(tweetFindMany.Skip)
            .ApplyTake(tweetFindMany.Take)
            .ApplyOrderBy(tweetFindMany.SortBy)
            .ToListAsync();

        return likes.Select(x => x.ToDto()).ToList();
    }

    /// <summary>
    /// Find multiple Retweets records for Tweet
    /// </summary>
    public async Task<List<RetweetDto>> FindRetweets(
        TweetIdDto idDto,
        RetweetFindMany tweetFindMany
    )
    {
        var retweets = await _context
            .Retweets.Where(m => m.TweetId == idDto.Id)
            .ApplyWhere(tweetFindMany.Where)
            .ApplySkip(tweetFindMany.Skip)
            .ApplyTake(tweetFindMany.Take)
            .ApplyOrderBy(tweetFindMany.SortBy)
            .ToListAsync();

        return retweets.Select(x => x.ToDto()).ToList();
    }

    /// <summary>
    /// Get a User record for Tweet
    /// </summary>
    public async Task<UserDto> GetUser(TweetIdDto idDto)
    {
        var tweet = await _context
            .Tweets.Where(tweet => tweet.Id == idDto.Id)
            .Include(tweet => tweet.User)
            .FirstOrDefaultAsync();
        if (tweet == null)
        {
            throw new NotFoundException();
        }
        return tweet.User.ToDto();
    }

    /// <summary>
    /// Meta data about Tweet records
    /// </summary>
    public async Task<MetadataDto> TweetsMeta(TweetFindMany findManyArgs)
    {
        var count = await _context.Tweets.ApplyWhere(findManyArgs.Where).CountAsync();

        return new MetadataDto { Count = count };
    }

    /// <summary>
    /// Update multiple Likes records for Tweet
    /// </summary>
    public async Task UpdateLikes(TweetIdDto idDto, LikeIdDto[] likesId)
    {
        var tweet = await _context
            .Tweets.Include(t => t.Likes)
            .FirstOrDefaultAsync(x => x.Id == idDto.Id);
        if (tweet == null)
        {
            throw new NotFoundException();
        }

        var likes = await _context
            .Likes.Where(a => likesId.Select(x => x.Id).Contains(a.Id))
            .ToListAsync();

        if (likes.Count == 0)
        {
            throw new NotFoundException();
        }

        tweet.Likes = likes;
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Update multiple Retweets records for Tweet
    /// </summary>
    public async Task UpdateRetweets(TweetIdDto idDto, RetweetIdDto[] retweetsId)
    {
        var tweet = await _context
            .Tweets.Include(t => t.Retweets)
            .FirstOrDefaultAsync(x => x.Id == idDto.Id);
        if (tweet == null)
        {
            throw new NotFoundException();
        }

        var retweets = await _context
            .Retweets.Where(a => retweetsId.Select(x => x.Id).Contains(a.Id))
            .ToListAsync();

        if (retweets.Count == 0)
        {
            throw new NotFoundException();
        }

        tweet.Retweets = retweets;
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Update one Tweet
    /// </summary>
    public async Task UpdateTweet(TweetIdDto idDto, TweetUpdateInput updateDto)
    {
        var tweet = updateDto.ToModel(idDto);

        if (updateDto.Retweets != null)
        {
            tweet.Retweets = await _context
                .Retweets.Where(retweet =>
                    updateDto.Retweets.Select(t => t.Id).Contains(retweet.Id)
                )
                .ToListAsync();
        }

        if (updateDto.Likes != null)
        {
            tweet.Likes = await _context
                .Likes.Where(like => updateDto.Likes.Select(t => t.Id).Contains(like.Id))
                .ToListAsync();
        }

        _context.Entry(tweet).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Tweets.Any(e => e.Id == tweet.Id))
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
