using System.Web.Mvc;

namespace Peizhi.Areas.Areas
{
    public class AreasAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Areas";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Areas_default",
                "Areas/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "Peizhi.Areas.Areas.Controllers" }//指定该路由查找控制器类的命名空间 controllers
            );
        }
    }
}
