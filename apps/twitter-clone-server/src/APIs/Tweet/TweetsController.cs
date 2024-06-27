using Microsoft.AspNetCore.Mvc;

namespace TwitterClone.APIs;

[ApiController()]
public class TweetsController : TweetsControllerBase
{
    public TweetsController(ITweetsService service)
        : base(service) { }
}
