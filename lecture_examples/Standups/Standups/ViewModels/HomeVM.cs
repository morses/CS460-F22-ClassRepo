using Standups.Models;

namespace Standups.ViewModels
{
    public class HomeVM
    {
        // Is this account missing a group?
        public bool NeedsGroup { get; set; }

        public bool IsAuthenticated { get; set; }
        
        // List of current questions that are available to comment on
        public IEnumerable<Supquestion> Questions { get; set; }
    }
}
