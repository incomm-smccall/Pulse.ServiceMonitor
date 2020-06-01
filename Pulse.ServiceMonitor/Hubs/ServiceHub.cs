using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace Pulse.ServiceMonitor.Hubs
{
    [HubName("serviceHub")]
    public class ServiceHub : Hub
    {
        [HubMethodName("getServiceStatus")]
        public static void GetServiceStatus()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<ServiceHub>();
            context.Clients.All.updateMessages();
        }

        [HubMethodName("getPulseProcesses")]
        public static void GetPulseProcesses()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<ServiceHub>();
            context.Clients.All.updateProcesses();
        }
    }
}