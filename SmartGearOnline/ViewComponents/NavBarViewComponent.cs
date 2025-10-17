using Microsoft.AspNetCore.Mvc;

namespace SmartGearOnline.ViewComponents
{

    public class NavBarViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var links = new List<(string Text, string Url)>
            {
                ("Home", "/"),
                ("Privacy", "/Home/Privacy"),
                ("Categories", "/Categories")
            };

            return View(links);
        }
    }
}