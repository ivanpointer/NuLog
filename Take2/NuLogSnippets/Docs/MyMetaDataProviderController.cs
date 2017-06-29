/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using NuLog;
using System.Web.Mvc;

namespace NuLogSnippets.Docs
{
    public class MyMetaDataProviderController : Controller
    {
        private readonly ILogger myLogger;

        public MyMetaDataProviderController()
        {
            var myMetaDataProvider = new MyControllerMetaDataProvider(this);
            myLogger = LogManager.GetLogger(myMetaDataProvider);
        }

        public ActionResult Index()
        {
            myLogger.Log("Will include my custom, request specific meta data.", "index");
            return View();
        }
    }
}