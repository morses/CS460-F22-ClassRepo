using Microsoft.AspNetCore.Mvc.Rendering;

namespace Standups.ViewModels
{
    public class SelectGroupVM
    {
        public string GroupId { get; set; }

        public List<SelectListItem> Groups { get; set; }
    }
}
