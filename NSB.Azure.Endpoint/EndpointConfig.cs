
using System;
using System.Configuration;
using NSB.Azure.Endpoint.Common;

namespace NSB.Azure.Endpoint
{
    using NServiceBus;

    public class EndpointConfig : IConfigureThisEndpoint
    {
        public void Customize(BusConfiguration configuration)
        {
            var azureStorageCn = ConfigurationManager.ConnectionStrings["NServiceBus/Transport"].ConnectionString;

            configuration.EndpointName("NSB.Azure.Endpoint");
            configuration.UseAzureConfig(null, null, 1);
            configuration.Transactions().DefaultTimeout(new TimeSpan(0, 0, 20, 0));
        }
    }
}
