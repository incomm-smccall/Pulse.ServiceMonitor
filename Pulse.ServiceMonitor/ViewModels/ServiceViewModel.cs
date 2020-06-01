using System;

namespace Pulse.ServiceMonitor.ViewModels
{
    public class ServiceModel
    {
        public string ServiceName { get; set; }
        public string ServiceState { get; set; }
        public bool ServiceStatus { get; set; }
    }
}