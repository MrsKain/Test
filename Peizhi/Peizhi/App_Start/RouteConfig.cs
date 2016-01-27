using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Peizhi
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                   namespaces: new[] { "Peizhi.Controllers" }//指定该路由查找控制器类的命名空间 controllers
            );
            routes.MapRoute(
  name: "HomeServer",
  url: "HomeServer/{action}/{id}/{ids}",
  defaults: new { controller = "HomeServer", action = "Index", id = UrlParameter.Optional, ids = UrlParameter.Optional }
);
            routes.MapRoute(
name: "Exam",
url: "Exam/{action}/{id}/{ids}",
defaults: new { controller = "Exam", action = "ExamIndex", id = UrlParameter.Optional, ids = UrlParameter.Optional },
 namespaces: new[] { "Peizhi.Controllers" }
);
            routes.MapRoute(
name: "PC",
url: "PC/{action}/{id}/{ids}",
defaults: new { controller = "PC", action = "MemberIndex", id = UrlParameter.Optional, ids = UrlParameter.Optional }
);
            routes.MapRoute(
name: "Video",
url: "Video/{action}/{id}/{ids}",
defaults: new { controller = "Video", action = "VideoList", id = UrlParameter.Optional, ids = UrlParameter.Optional },
 namespaces: new[] { "Peizhi.Controllers" }
);
            routes.MapRoute(
name: "Home",
url: "Home/{action}/{id}/{ids}",
defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional, ids = UrlParameter.Optional },
  namespaces: new[] { "Peizhi.Controllers" }
);
        }
    }
}