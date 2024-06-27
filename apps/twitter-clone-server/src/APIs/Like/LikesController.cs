using Microsoft.AspNetCore.Mvc;

namespace TwitterClone.APIs;

[ApiController()]
public class LikesController : LikesControllerBase
{
    public LikesController(ILikesService service)
        : base(service) { }
}
