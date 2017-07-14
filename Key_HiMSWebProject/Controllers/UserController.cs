using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Key_HiMSWebProject.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace Key_HiMSWebProject.Controllers
{
    public class UserController : Controller
    {
        private List<TodoUser> userCache;

        //Updates the cache with data from the API
        private async Task UpdateUserCache()
        {
            if (userCache == null)
            {
                userCache = await DataRequestController.GetData<TodoUser>("users");
            }  
        }

        private async Task<TodoUser> FindUser(string username)
        {
            await UpdateUserCache();

            // It's a bit more user friendly to ingore case when searching.
            TodoUser user = userCache.Find(u => u.username.Equals(username, StringComparison.InvariantCultureIgnoreCase));

            return user;
        }

        [HttpGet]
        public async Task<ActionResult> RequestUserByUsername(string username)
        {
            ViewBag.SyncType = "Asynchronous";

            TodoUser user = await FindUser(username);

            if(user == null)
            {
                return Json(new { Success = false, Message = "User Not Found." }, JsonRequestBehavior.AllowGet);
            }

            return Json(user, JsonRequestBehavior.AllowGet);
        }
    }
}