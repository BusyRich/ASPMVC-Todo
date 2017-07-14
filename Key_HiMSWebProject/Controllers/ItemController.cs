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
        public async Task<ActionResult> RequestTasks(int userId, int? page, int pageSize = 0)
        {
            ViewBag.SyncType = "Asynchronous";
            List<TaskItem> tasks;
            double pages = 1;

            if (page.HasValue)
            {
                // If a page is provided we are assuming the requester wants paginated results,
                // so we want to default to 10 results per page.
                if(pageSize == 0)
                {
                    pageSize = 10;
                }

                tasks = await GetTasks(userId, page.Value, pageSize);
                pages = Math.Ceiling((double)itemCache.FindAll(it => it.userId == userId).Count() / pageSize);

            }
            else
            {
                tasks = await GetTasks(userId);
            }
            
            if (tasks == null)
            {
                return Json(new { success = false, message = "No Tasks Found." }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
               pages = pages,
               tasks = tasks
            }, JsonRequestBehavior.AllowGet);
        }
    }
}