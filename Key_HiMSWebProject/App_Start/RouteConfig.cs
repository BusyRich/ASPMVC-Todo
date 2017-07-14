using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Key_HiMSWebProject
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default Route",
                url: "",
                defaults: new { controller = "Home", action = "Index" }
            );

            routes.MapRoute(
                name: "Get User by Username",
                url: "user/{username}",
                defaults: new { controller = "User", action = "RequestUserByUsername" }
            );

            // I understand the API here is inconsistant but the login requires getting a user by username 
            // but the task data is keyed on userId so for quickness I just made the speific routes I needed.
            // In a production setting I would probably use a /login route for loginning in instead of user/{username}
            routes.MapRoute(
                name: "Get Tasks for a Specific UserId",
                url: "user/{userId}/tasks",
                defaults: new { controller = "Item", action = "RequestTasks"}
            );
        }
    }
}
