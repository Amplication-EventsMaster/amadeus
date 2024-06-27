using Microsoft.EntityFrameworkCore;
using TwitterClone.APIs;
using TwitterClone.APIs.Common;
using TwitterClone.APIs.Dtos;
using TwitterClone.APIs.Errors;
using TwitterClone.APIs.Extensions;
using TwitterClone.Infrastructure;
using TwitterClone.Infrastructure.Models;

namespace TwitterClone.APIs;

public abstract class UsersServiceBase : IUsersService
{
    protected readonly TwitterCloneDbContext _context;

    public UsersServiceBase(TwitterCloneDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Create one User
    /// </summary>
    public async Task<UserDto> CreateUser(UserCreateInput createDto)
    {
        var user = new User
        {
            CreatedAt = createDto.CreatedAt,
            Email = createDto.Email,
            FirstName = createDto.FirstName,
            LastName = createDto.LastName,
            Password = createDto.Password,
            Roles = createDto.Roles,
            UpdatedAt = createDto.UpdatedAt,
            Username = createDto.Username
        };

        if (createDto.Id != null)
        {
            user.Id = createDto.Id;
        }
        if (createDto.Tweets != null)
        {
            user.Tweets = await _context
                .Tweets.Where(tweet => createDto.Tweets.Select(t => t.Id).Contains(tweet.Id))
                .ToListAsync();
        }

        if (createDto.Retweets != null)
        {
            user.Retweets = await _context
                .Retweets.Where(retweet =>
                    createDto.Retweets.Select(t => t.Id).Contains(retweet.Id)
                )
                .ToListAsync();
        }

        if (createDto.Likes != null)
        {
            user.Likes = await _context
                .Likes.Where(like => createDto.Likes.Select(t => t.Id).Contains(like.Id))
                .ToListAsync();
        }

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var result = await _context.FindAsync<User>(user.Id);

        if (result == null)
        {
            throw new NotFoundException();
        }

        return result.ToDto();
    }

    /// <summary>
    /// Delete one User
    /// </summary>
    public async Task DeleteUser(UserIdDto idDto)
    {
        var user = await _context.Users.FindAsync(idDto.Id);
        if (user == null)
        {
            throw new NotFoundException();
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Find many Users
    /// </summary>
    public async Task<List<UserDto>> Users(UserFindMany findManyArgs)
    {
        var users = await _context
            .Users.Include(x => x.Tweets)
            .Include(x => x.Retweets)
            .Include(x => x.Likes)
            .ApplyWhere(findManyArgs.Where)
            .ApplySkip(findManyArgs.Skip)
            .ApplyTake(findManyArgs.Take)
            .ApplyOrderBy(findManyArgs.SortBy)
            .ToListAsync();
        return users.ConvertAll(user => user.ToDto());
    }

    /// <summary>
    /// Get one User
    /// </summary>
    public async Task<UserDto> User(UserIdDto idDto)
    {
        var users = await this.Users(
            new UserFindMany { Where = new UserWhereInput { Id = idDto.Id } }
        );
        var user = users.FirstOrDefault();
        if (user == null)
        {
            throw new NotFoundException();
        }

        return user;
    }

    /// <summary>
    /// Update one User
    /// </summary>
    public async Task UpdateUser(UserIdDto idDto, UserUpdateInput updateDto)
    {
        var user = updateDto.ToModel(idDto);

        if (updateDto.Tweets != null)
        {
            user.Tweets = await _context
                .Tweets.Where(tweet => updateDto.Tweets.Select(t => t.Id).Contains(tweet.Id))
                .ToListAsync();
        }

        if (updateDto.Retweets != null)
        {
            user.Retweets = await _context
                .Retweets.Where(retweet =>
                    updateDto.Retweets.Select(t => t.Id).Contains(retweet.Id)
                )
                .ToListAsync();
        }

        if (updateDto.Likes != null)
        {
            user.Likes = await _context
                .Likes.Where(like => updateDto.Likes.Select(t => t.Id).Contains(like.Id))
                .ToListAsync();
        }

        _context.Entry(user).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Users.Any(e => e.Id == user.Id))
            {
                throw new NotFoundException();
            }
            else
            {
                throw;
            }
        }
    }

    /// <summary>
    /// Connect multiple Likes records to User
    /// </summary>
    public async Task ConnectLikes(UserIdDto idDto, LikeIdDto[] likesId)
    {
        var user = await _context
            .Users.Include(x => x.Likes)
            .FirstOrDefaultAsync(x => x.Id == idDto.Id);
        if (user == null)
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

        var likesToConnect = likes.Except(user.Likes);

        foreach (var like in likesToConnect)
        {
            user.Likes.Add(like);
        }

        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Connect multiple Retweets records to User
    /// </summary>
    public async Task ConnectRetweets(UserIdDto idDto, RetweetIdDto[] retweetsId)
    {
        var user = await _context
            .Users.Include(x => x.Retweets)
            .FirstOrDefaultAsync(x => x.Id == idDto.Id);
        if (user == null)
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

        var retweetsToConnect = retweets.Except(user.Retweets);

        foreach (var retweet in retweetsToConnect)
        {
            user.Retweets.Add(retweet);
        }

        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Connect multiple Tweets records to User
    /// </summary>
    public async Task ConnectTweets(UserIdDto idDto, TweetIdDto[] tweetsId)
    {
        var user = await _context
            .Users.Include(x => x.Tweets)
            .FirstOrDefaultAsync(x => x.Id == idDto.Id);
        if (user == null)
        {
            throw new NotFoundException();
        }

        var tweets = await _context
            .Tweets.Where(t => tweetsId.Select(x => x.Id).Contains(t.Id))
            .ToListAsync();
        if (tweets.Count == 0)
        {
            throw new NotFoundException();
        }

        var tweetsToConnect = tweets.Except(user.Tweets);

        foreach (var tweet in tweetsToConnect)
        {
            user.Tweets.Add(tweet);
        }

        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Disconnect multiple Likes records from User
    /// </summary>
    public async Task DisconnectLikes(UserIdDto idDto, LikeIdDto[] likesId)
    {
        var user = await _context
            .Users.Include(x => x.Likes)
            .FirstOrDefaultAsync(x => x.Id == idDto.Id);
        if (user == null)
        {
            throw new NotFoundException();
        }

        var likes = await _context
            .Likes.Where(t => likesId.Select(x => x.Id).Contains(t.Id))
            .ToListAsync();

        foreach (var like in likes)
        {
            user.Likes?.Remove(like);
        }
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Disconnect multiple Retweets records from User
    /// </summary>
    public async Task DisconnectRetweets(UserIdDto idDto, RetweetIdDto[] retweetsId)
    {
        var user = await _context
            .Users.Include(x => x.Retweets)
            .FirstOrDefaultAsync(x => x.Id == idDto.Id);
        if (user == null)
        {
            throw new NotFoundException();
        }

        var retweets = await _context
            .Retweets.Where(t => retweetsId.Select(x => x.Id).Contains(t.Id))
            .ToListAsync();

        foreach (var retweet in retweets)
        {
            user.Retweets?.Remove(retweet);
        }
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Disconnect multiple Tweets records from User
    /// </summary>
    public async Task DisconnectTweets(UserIdDto idDto, TweetIdDto[] tweetsId)
    {
        var user = await _context
            .Users.Include(x => x.Tweets)
            .FirstOrDefaultAsync(x => x.Id == idDto.Id);
        if (user == null)
        {
            throw new NotFoundException();
        }

        var tweets = await _context
            .Tweets.Where(t => tweetsId.Select(x => x.Id).Contains(t.Id))
            .ToListAsync();

        foreach (var tweet in tweets)
        {
            user.Tweets?.Remove(tweet);
        }
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Find multiple Likes records for User
    /// </summary>
    public async Task<List<LikeDto>> FindLikes(UserIdDto idDto, LikeFindMany userFindMany)
    {
        var likes = await _context
            .Likes.Where(m => m.UserId == idDto.Id)
            .ApplyWhere(userFindMany.Where)
            .ApplySkip(userFindMany.Skip)
            .ApplyTake(userFindMany.Take)
            .ApplyOrderBy(userFindMany.SortBy)
            .ToListAsync();

        return likes.Select(x => x.ToDto()).ToList();
    }

    /// <summary>
    /// Find multiple Retweets records for User
    /// </summary>
    public async Task<List<RetweetDto>> FindRetweets(UserIdDto idDto, RetweetFindMany userFindMany)
    {
        var retweets = await _context
            .Retweets.Where(m => m.UserId == idDto.Id)
            .ApplyWhere(userFindMany.Where)
            .ApplySkip(userFindMany.Skip)
            .ApplyTake(userFindMany.Take)
            .ApplyOrderBy(userFindMany.SortBy)
            .ToListAsync();

        return retweets.Select(x => x.ToDto()).ToList();
    }

    /// <summary>
    /// Find multiple Tweets records for User
    /// </summary>
    public async Task<List<TweetDto>> FindTweets(UserIdDto idDto, TweetFindMany userFindMany)
    {
        var tweets = await _context
            .Tweets.Where(m => m.UserId == idDto.Id)
            .ApplyWhere(userFindMany.Where)
            .ApplySkip(userFindMany.Skip)
            .ApplyTake(userFindMany.Take)
            .ApplyOrderBy(userFindMany.SortBy)
            .ToListAsync();

        return tweets.Select(x => x.ToDto()).ToList();
    }

    /// <summary>
    /// Meta data about User records
    /// </summary>
    public async Task<MetadataDto> UsersMeta(UserFindMany findManyArgs)
    {
        var count = await _context.Users.ApplyWhere(findManyArgs.Where).CountAsync();

        return new MetadataDto { Count = count };
    }

    /// <summary>
    /// Update multiple Likes records for User
    /// </summary>
    public async Task UpdateLikes(UserIdDto idDto, LikeIdDto[] likesId)
    {
        var user = await _context
            .Users.Include(t => t.Likes)
            .FirstOrDefaultAsync(x => x.Id == idDto.Id);
        if (user == null)
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

        user.Likes = likes;
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Update multiple Retweets records for User
    /// </summary>
    public async Task UpdateRetweets(UserIdDto idDto, RetweetIdDto[] retweetsId)
    {
        var user = await _context
            .Users.Include(t => t.Retweets)
            .FirstOrDefaultAsync(x => x.Id == idDto.Id);
        if (user == null)
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

        user.Retweets = retweets;
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Update multiple Tweets records for User
    /// </summary>
    public async Task UpdateTweets(UserIdDto idDto, TweetIdDto[] tweetsId)
    {
        var user = await _context
            .Users.Include(t => t.Tweets)
            .FirstOrDefaultAsync(x => x.Id == idDto.Id);
        if (user == null)
        {
            throw new NotFoundException();
        }

        var tweets = await _context
            .Tweets.Where(a => tweetsId.Select(x => x.Id).Contains(a.Id))
            .ToListAsync();

        if (tweets.Count == 0)
        {
            throw new NotFoundException();
        }

        user.Tweets = tweets;
        await _context.SaveChangesAsync();
    }
}
