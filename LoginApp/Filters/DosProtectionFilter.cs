using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Web.Mvc;

namespace LoginApp.Filters
{
    public class DosProtectionFilter : IActionFilter
    {
        private const double TimeLimitSeconds = 10;
        private const int NumberOfRequestsLimit = 10;
        
        private readonly ConcurrentDictionary<string, ConcurrentBag<DateTime>> _requestsByAddress = new ConcurrentDictionary<string, ConcurrentBag<DateTime>>();

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var ip = filterContext.HttpContext.Request.UserHostAddress;
            var now = DateTime.Now;
            if (ip == null) return;

            var requests = _requestsByAddress.GetOrAdd(ip, _ => new ConcurrentBag<DateTime>());
            requests.Add(now);
            if (requests.Count > NumberOfRequestsLimit)
            {
                filterContext.Result = new HttpStatusCodeResult(429, "Too Many Requests");
            }

            _requestsByAddress.TryUpdate(ip, new ConcurrentBag<DateTime>(requests.Where(d => (now - d).TotalSeconds < TimeLimitSeconds)), requests);
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }
    }
}