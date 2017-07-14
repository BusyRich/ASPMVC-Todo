using Key_HiMSWebProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Key_HiMSWebProject.Controllers
{
    public class ItemController : Controller
    {
        private List<TaskItem> itemCache;

        //Updates the cache with data from the API
        private async Task UpdateUserCache()
        {
            if (itemCache == null)
            {
                itemCache = await DataRequestController.GetData<TaskItem>("todos");
            }
        }

        // Returns all the tasks for a user
        private async Task<List<TaskItem>> GetTasks(int userId)
        {
            await UpdateUserCache();

            return itemCache.FindAll(it => it.userId == userId).ToList();
        }

        // Returns a specific list of paginated tasks
        private async Task<List<TaskItem>> GetTasks(int userId, int page, int pageSize)
        {
            await UpdateUserCache();

            return itemCache.FindAll(it => it.userId == userId).Skip((page - 1) * pageSize).Take(pageSize).ToList();
        }

        [HttpGet]
        public async Task<ActionResult> RequestTasks(int userId, int? page, int? pageSize)
        {
            ViewBag.SyncType = "Asynchronous";
            List<TaskItem> tasks;

            if (page.HasValue)
            {
                tasks = await GetTasks(userId, page.Value, pageSize ?? 10);
            }
            else
            {
                tasks = await GetTasks(userId);
            }
            
            if (tasks == null)
            {
                return Json(new { Success = false, Message = "No Tasks Found." }, JsonRequestBehavior.AllowGet);
            }

            return Json(tasks, JsonRequestBehavior.AllowGet);
        }
    }
}