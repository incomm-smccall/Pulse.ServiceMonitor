using Pulse.ServiceMonitor.Utils;
using Pulse.ServiceMonitor.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Pulse.ServiceMonitor.Models
{
    public static class ServiceStatusModel
    {
        public static IList<ServiceModel> GetJobServiceStatus()
        {
            //IList<ServiceViewModel> serviceModelList = new List<ServiceViewModel>();
            IList<ServiceModel> serviceModelList = new List<ServiceModel>();

            //string result = new CommandService().ServiceCommands("ServiceStatus:PulseJob");
            string result = new CommandService().ServiceCommands("ServiceStatus:TapiSrv");
            Logging.LogMessage(LoggingLevel.Info, $"Result from getting the services status {result}");
            if (string.IsNullOrEmpty(result)) return serviceModelList;
            var results = CleanResult(result);
            ServiceModel svcJob = BuildServiceModel(results);

            CacheControls.UpsertCacheModel(svcJob, "JobServiceModel");
            serviceModelList.Add(svcJob);

            result = new CommandService().ServiceCommands("ServiceStatus:JobsMonitor");
            if (string.IsNullOrEmpty(result)) return serviceModelList;
            results = CleanResult(result);
            ServiceModel svcMonitor = BuildServiceModel(results);
            CacheControls.UpsertCacheModel(svcMonitor, "MonitorServiceModel");
            serviceModelList.Add(svcMonitor);

            return serviceModelList;
        }

        private static ServiceModel BuildServiceModel(MatchCollection results)
        {
            ServiceModel svc = new ServiceModel();
            svc.ServiceName = results[0].Value.Split(':')[1];
            svc.ServiceState = Regex.Replace(results[1].Value.Split(':')[1], @"\d+", "");
            svc.ServiceStatus = (svc.ServiceState.ToLower() == "running") ? true : false;
            return svc;
        }

        public static void StartService()
        {
            //ServiceModel svc = CacheControls.GetCacheModel<ServiceModel>("JobServiceModel");
            ////string result = new CommandService().ServiceCommands("ServiceStart:PulseJob");
            //string result = new CommandService().ServiceCommands("ServiceStart:TapiSrv");
            //Logging.LogMessage(LoggingLevel.Info, $"Result from start service: {result}");
            //if (result.ToLower() == "true")
            //{
            //    svc.ServiceStatus = true;
            //    CacheControls.UpsertCacheModel(svc, "JobServiceModel");
            //    ServiceHub.GetServiceStatus();
            //}
        }

        public static void StopService()
        {
            //ServiceModel svc = CacheControls.GetCacheModel<ServiceModel>("JobServiceModel");
            ////string result = new CommandService().ServiceCommands("ServiceStop:PulseJob");
            //string result = new CommandService().ServiceCommands("ServiceStop:TapiSrv");
            //Logging.LogMessage(LoggingLevel.Info, $"Result from stop service: {result}");
            //if (result.ToLower() == "true")
            //{
            //    svc.ServiceStatus = false;
            //    CacheControls.UpsertCacheModel(svc, "JobServiceModel");
            //    ServiceHub.GetServiceStatus();
            //}
        }

        private static MatchCollection CleanResult(string message)
        {
            string pattern = @"(SERVICE_NAME:[a-zA-Z]+|STATE:\d[a-zA-Z]+)";
            message = message.Replace(" ", "").Replace("\r\n", " ");
            return Regex.Matches(message, pattern);
        }
    }
}