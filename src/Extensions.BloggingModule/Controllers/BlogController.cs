using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SatelliteSite.BloggingModule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xylab.Blogging.Services;

namespace SatelliteSite.BloggingModule.Controllers
{
    [Area("Blog")]
    [Route("[controller]")]
    public class BlogController : ViewControllerBase
    {
        public IBloggingFacade Facade { get; }
        public BlogController(IBloggingFacade facade) => Facade = facade;


        [HttpGet("[action]/{postId}")]
        public async Task<IActionResult> Entry(int postId)
        {
            var model = await Facade.Blogs.FindAsync(postId);

            if (model == null)
                return NotFound();
            if (model.IsDeleted && !User.IsInRole("Administrator"))
                return NotFound();

            if (User.GetUserId() != null)
            {
                var uid = int.Parse(User.GetUserId());
                var userVotes = await Facade.Blogs.StatisticsAsync(new[] { postId }, uid);
                ViewBag.PostVote = userVotes.GetValueOrDefault(postId);
                var commentVotes = await Facade.Comments.StatisticsAsync(postId, uid);
                ViewBag.CommentVote = commentVotes;
            }

            return View(model);
        }


        [HttpGet("{username?}")]
        public async Task<IActionResult> List(
            [FromRoute] string username,
            [FromServices] IUserManager userManager,
            [FromQuery] int page = 1)
        {
            if (page <= 0) return NotFound();
            int? uid = null;
            ViewBag.Page = page;

            if (username != null)
            {
                var user = await userManager.FindByNameAsync(username);
                if (user == null) return NotFound();
                ViewBag.User = user;
                uid = user.Id;
            }

            var model = await Facade.Blogs.ListAsync(uid, 10, (page - 1) * 10);

            if (User.GetUserId() != null)
            {
                var uuid = int.Parse(User.GetUserId());
                ViewBag.PostsVote = await Facade.Blogs.StatisticsAsync(model.Select(a => a.Id), uuid);
            }

            return View(model);
        }


        [Authorize(Roles = "Administrator,BlogPublisher")]
        [HttpGet("[action]")]
        public IActionResult Publish()
        {
            return View(new BlogPublishModel());
        }


        [Authorize]
        [HttpGet("entry/{postId}/[action]")]
        public async Task<IActionResult> Publish(int postId)
        {
            var model = new BlogPublishModel();
            int uid = int.Parse(User.GetUserId());
            var toEdit = await Facade.Blogs.QuickFindAsync(postId);
            if (toEdit == null || toEdit.UserId != uid) return NotFound();

            var rev = await Facade.Blogs.GetLastRevisionAsync(toEdit);

            model.Title = toEdit.Title;
            model.Content = rev?.ContentRaw;
            model.ShowOnHomePage = toEdit.CommonShared;
            return View(model);
        }


        [Authorize(Roles = "Administrator,BlogPublisher")]
        [HttpPost("[action]")]
        public async Task<IActionResult> Publish(BlogPublishModel model)
        {
            if (!ModelState.IsValid) return View(model);
            int uid = int.Parse(User.GetUserId());

            var p = await Facade.Blogs.CreateAsync(uid, model.Title ?? "UNTITLED", model.Content, model.ShowOnHomePage);

            StatusMessage = "Blog has been published.";
            return RedirectToAction(nameof(Entry), new { postId = p.Id });
        }


        [Authorize]
        [HttpPost("entry/{postId}/[action]")]
        public async Task<IActionResult> Publish(int postId, BlogPublishModel model)
        {
            if (!ModelState.IsValid) return View(model);
            int uid = int.Parse(User.GetUserId());

            var toEdit = await Facade.Blogs.QuickFindAsync(postId);
            if (toEdit == null || toEdit.UserId != uid) return NotFound();

            model.ShowOnHomePage &= User.IsInRole("Administrator");
            await Facade.Blogs.ReviseAsync(toEdit, model.Title ?? "UNTITLED", model.Content, model.ShowOnHomePage);

            StatusMessage = "Blog has been edited.";
            return RedirectToAction(nameof(Entry), new { postId });
        }


        [Authorize(Policy = "EmailVerified")]
        [HttpPost("entry/{postId}/[action]")]
        public async Task<IActionResult> Comment(int postId, string content, int? replyTo = null)
        {
            if (content == null || content.Length > 1024)
                return BadRequest();

            var model = await Facade.Blogs.QuickFindAsync(postId);

            if (model == null)
                return NotFound();
            if (model.IsDeleted && !User.IsInRole("Administrator"))
                return NotFound();

            var uid = int.Parse(User.GetUserId());
            var replied = replyTo.HasValue ? await Facade.Comments.FindAsync(postId, replyTo.Value) : null;
            if (replyTo.HasValue != (replied != null))
                return NotFound();

            var c = await Facade.Comments.CreateAsync(model, uid, content, replied);
            return RedirectToAction(nameof(Entry), "Blog", new { postId }, $"C{c.Id}");
        }


        [HttpGet("entry/{postId}/comment/{commentId}/delete")]
        [ValidateAjaxWindow]
        public async Task<IActionResult> DeleteComment(int postId, int commentId)
        {
            var model = await Facade.Comments.FindAsync(postId, commentId);
            if (model == null || (!User.IsInRole("Administrator") && User.GetUserId() != model.UserId.ToString()))
                return NotFound();

            return AskPost(
                title: "Delete the comment",
                message: "Are you sure to delete this comment?",
                type: BootstrapColor.danger);
        }


        [HttpPost("entry/{postId}/comment/{commentId}/delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteComment(int postId, int commentId, bool post = true)
        {
            var model = await Facade.Comments.FindAsync(postId, commentId);
            if (model == null || (!User.IsInRole("Administrator") && User.GetUserId() != model.UserId.ToString()))
                return NotFound();
            await Facade.Comments.DeleteAsync(model);
            return RedirectToAction(nameof(Entry));
        }


        private async Task<IActionResult> Vote(int postId, int commentId, bool act, bool def)
        {
            var uid = int.Parse(User.GetUserId());
            bool? a = def, b = null;
            var result = await Facade.Comments.VoteAsync(postId, commentId, uid, act ? b : a, act ? a : b);

            if (!result) return BadRequest();
            var stat = await Facade.Comments.StatisticsAsync(postId, commentId, uid);
            return PartialView("VotingDetail", (postId, (int?)commentId, stat.Item2, stat.Item3, stat.Item1?.Up));
        }


        private async Task<IActionResult> Vote(int postId, bool act, bool def)
        {
            var uid = int.Parse(User.GetUserId());
            bool? a = def, b = null;
            var result = await Facade.Blogs.VoteAsync(postId, uid, act ? b : a, act ? a : b);

            if (!result) return BadRequest();
            var stat = await Facade.Blogs.StatisticsAsync(postId, uid);
            return PartialView("VotingDetail", (postId, (int?)null, stat.Item1, stat.Item2, stat.Item3?.Up));
        }


        [HttpGet("entry/{postId}/[action]/{act}")]
        [Authorize(Policy = "EmailVerified")]
        public Task<IActionResult> VoteUp(int postId, bool act)
            => Vote(postId, act, true);


        [HttpGet("entry/{postId}/[action]/{act}")]
        [Authorize(Policy = "EmailVerified")]
        public Task<IActionResult> VoteDown(int postId, bool act)
            => Vote(postId, act, false);


        [HttpGet("entry/{postId}/comment/{commentId}/[action]/{act}")]
        [Authorize(Policy = "EmailVerified")]
        public Task<IActionResult> VoteUp(int postId, int commentId, bool act)
            => Vote(postId, commentId, act, true);


        [HttpGet("entry/{postId}/comment/{commentId}/[action]/{act}")]
        [Authorize(Policy = "EmailVerified")]
        public Task<IActionResult> VoteDown(int postId, int commentId, bool act)
            => Vote(postId, commentId, act, false);
    }
}
