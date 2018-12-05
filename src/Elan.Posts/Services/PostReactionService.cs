using Elan.Data.Contracts;
using Elan.Data.Models.Posts;
using Elan.Posts.Contracts;
using Elan.Posts.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Elan.Posts.Services
{
    public class PostReactionService: IPostReactionService
    {
        private readonly IDataService _dataService;

        public PostReactionService(IDataService dataService)
        {
            _dataService = dataService;
        }

        public async Task SetReaction(SetPostReactionViewModel model)
        {
            var post = await _dataService
                .GetSet<Post>()
                .Include(x => x.Reactions)
                .FirstOrDefaultAsync(x => x.Id == model.PostId);

            if (post == null)
            {
                return;
                // TODO: add error handling
            }

            var userPostReaction = post.Reactions.FirstOrDefault(x => x.UserId == model.User.Id);

            if (userPostReaction != null)
            {
                await UpdatePostReaction(model, userPostReaction);
            }
            else
            {
                await CreatePostReaction(model);
            }
        }

        private async Task UpdatePostReaction(SetPostReactionViewModel model, PostReaction userPostReaction)
        {
            var postReactions = _dataService.GetSet<PostReaction>();

            if (userPostReaction.Type == model.Type)
            {
                postReactions.Remove(userPostReaction);
            }
            else
            {
                userPostReaction.Type = model.Type;
            }

            await _dataService.SaveDbAsync();
        }

        private async Task CreatePostReaction(SetPostReactionViewModel model)
        {
            var postReactions = _dataService.GetSet<PostReaction>();

            var newReaction = new PostReaction
            {
                User = model.User,
                PostId = model.PostId,
                Type = model.Type
            };

            await postReactions.AddAsync(newReaction);
            await _dataService.SaveDbAsync();
        }

        public int GetReactionCount(int postId)
        {
            var reactionNumber = _dataService.GetSet<PostReaction>().Where(pr => pr.PostId == postId).Count();

            return reactionNumber;
        }
    }
}
