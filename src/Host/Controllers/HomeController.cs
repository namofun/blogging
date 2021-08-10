using Blogging.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SatelliteSite.Controllers
{
    public class HomeController : ViewControllerBase
    {
        public IBloggingFacade Facade { get; }
        public HomeController(IBloggingFacade facade) => Facade = facade;


        [HttpGet("/")]
        public async Task<IActionResult> List([FromQuery] int page = 1)
        {
            if (page <= 0) return NotFound();
            int? uid = null;
            ViewBag.Page = page;

            var model = await Facade.Blogs.ListAsync(uid, 10, (page - 1) * 10);

            if (User.GetUserId() != null)
            {
                var uuid = int.Parse(User.GetUserId());
                ViewBag.PostsVote = await Facade.Blogs.StatisticsAsync(model.Select(a => a.Id), uuid);
            }

            return View(model);
        }
    }
}
