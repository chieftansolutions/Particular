using System.Net;
using NServiceBus;
using NServiceBus.Config.ConfigurationSource;
using NServiceBus.Newtonsoft.Json;

namespace NSB.Azure.Test.Common
{
    public static class BusConfigurationExtensions
    {
        public static void UseAzureConfig(this BusConfiguration configuration, IConfigurationSource endPointMapping = null, int? invisibleTime = null, int? batchSize = null, int? peekInterval = null)
        {
            ServicePointManager.DefaultConnectionLimit = 5000; // default settings only allows 2 concurrent requests per process to the same host
            ServicePointManager.UseNagleAlgorithm = false; // optimize for small requests
            ServicePointManager.Expect100Continue = false; // reduces number of http calls
            ServicePointManager.CheckCertificateRevocationList = false; // optional, only if you trust all your dependencies 

            if (endPointMapping != null)
                configuration.CustomConfigurationSource(endPointMapping);

            configuration.UseSerialization<NewtonsoftSerializer>();

            configuration.Transactions().DisableDistributedTransactions();
            configuration.EnableInstallers();

            //configuration.PurgeOnStartup(true);

            configuration.UsePersistence<AzureStoragePersistence>();
            var transport = configuration.UseTransport<AzureStorageQueueTransport>();
            if (invisibleTime.HasValue && invisibleTime > 0)
                transport.MessageInvisibleTime(invisibleTime.Value);
            if (batchSize.HasValue && batchSize > 0)
                transport.BatchSize(batchSize.Value);
            if (peekInterval.HasValue && peekInterval > 0)
                transport.PeekInterval(peekInterval.Value);
        }

        /// <summary>
        /// Configures Messaging Conventions
        /// </summary>
        /// <param name="config"></param>
        /// <para name="functionalNamespace">An optional namespace where message are defined</para>
        public static void ConfigMessagingConventions(this BusConfiguration config, string functionalNamespace = "")
        {
            var messageNamespace = "WatchGuard.Messaging";
            if (!string.IsNullOrWhiteSpace(functionalNamespace))
                messageNamespace += "." + functionalNamespace;

            var conventions = config.Conventions();
            conventions.DefiningCommandsAs(t => t.Namespace != null && t.Namespace.StartsWith(messageNamespace) && t.Name.EndsWith("Command"));
            conventions.DefiningEventsAs(t => t.Namespace != null && t.Namespace.StartsWith(messageNamespace) && t.Name.EndsWith("Event"));
            conventions.DefiningMessagesAs(t => t.Namespace != null && t.Namespace.StartsWith(messageNamespace) && (t.Name.EndsWith("Request") || t.Name.EndsWith("Response")));
            //conventions.DefiningEncryptedPropertiesAs(p => p.Name.StartsWith("Encrypted"));
            //conventions.DefiningDataBusPropertiesAs(p => p.Name.EndsWith("DataBus"));
            //conventions.DefiningExpressMessagesAs(t => t.Name.EndsWith("Express"));
            //conventions.DefiningTimeToBeReceivedAs(t => t.Name.EndsWith("Expires") ? TimeSpan.FromSeconds(30) : TimeSpan.MaxValue);
        }
    }
    
}