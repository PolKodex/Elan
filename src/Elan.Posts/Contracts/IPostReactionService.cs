using System.Threading.Tasks;
using Elan.Posts.Models;

namespace Elan.Posts.Contracts
{
    public interface IPostReactionService
    {
        Task SetReaction(SetPostReactionViewModel model);
    }
}
