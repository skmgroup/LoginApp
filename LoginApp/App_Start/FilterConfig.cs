using System.Web;
using System.Web.Mvc;
using LoginApp.Filters;

namespace LoginApp
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new DosProtectionFilter());
        }
    }
}
