using Microsoft.AspNetCore.Mvc;

namespace TwitterClone.APIs;

[ApiController()]
public class RetweetsController : RetweetsControllerBase
{
    public RetweetsController(IRetweetsService service)
        : base(service) { }
}
