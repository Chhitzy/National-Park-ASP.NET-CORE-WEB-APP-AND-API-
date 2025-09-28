using Microsoft.AspNetCore.Mvc.Rendering;

namespace National_Park_Web_App.Models.ViewModels
{
    public class TrailVM
    {
        public IEnumerable<SelectListItem> NationalParkList { get; set; }
        public Trail Trail { get; set; }    
    }
}
