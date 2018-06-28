using GalaxyHub;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class ApiController : Controller
    {
        private static GalaxyHub.GalaxyHub/*<JobContext>*/ _connection = 
            new GalaxyHub.GalaxyHub/*<JobContext>*/(ConfigurationManager.AppSettings["Url"], ESource.GalaxyService.ToString());
        
        // GET: Api
        public ActionResult Index()
        {
            return View();
        }

        //Testing function for the UnitTest
        public string Function()
        {
            return "Function";
        }

        //RunJob Function, paste from the old API
        public string RunJob(string elm)
        {
            return "run";
            using (JobContext context = new JobContext(EMessage.notifyJob))
            {
                _connection.ConnectAndRegister();
                if (!context.BuildMessage(elm))
                    return context.Result;
                context.SetSource(_connection.M2MId);
                _connection.Attach(context);
                if (!_connection.Invoke(context))
                    return context.Result;
                context.WaitMessage(ConfigurationManager.AppSettings["WaitingTime"]);

                _connection.Answer();

                return context.Result;
            }
        }
    }
}