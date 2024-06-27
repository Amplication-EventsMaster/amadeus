using Microsoft.AspNetCore.Mvc;
using TwitterClone.APIs.Common;
using TwitterClone.Infrastructure.Models;

namespace TwitterClone.APIs.Dtos;

[BindProperties(SupportsGet = true)]
public class TweetFindMany : FindManyInput<Tweet, TweetWhereInput> { }
