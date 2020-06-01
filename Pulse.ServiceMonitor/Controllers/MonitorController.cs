using Pulse.ServiceMonitor.Models;
using Pulse.ServiceMonitor.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Pulse.ServiceMonitor.Controllers
{
    public class MonitorController : Controller
    {
        // GET: Monitor
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetStatus()
        {
            IList<ServiceModel> serviceModelList = ServiceStatusModel.GetJobServiceStatus();
            return PartialView("_ServiceView", serviceModelList);
        }

        public ActionResult StartService()
        {
            ServiceStatusModel.StartService();
            return Json("Success", JsonRequestBehavior.AllowGet);
        }

        public ActionResult StopService()
        {
            ServiceStatusModel.StopService();
            return Json("Success", JsonRequestBehavior.AllowGet);
        }

        //public ActionResult GetServiceList()
        //{
        //    IList<ServiceViewModel> serviceModelList = ServiceStatusModel.GetJobServiceStatus();
        //    return PartialView("_ServiceView", serviceModelList);
        //}
    }
}