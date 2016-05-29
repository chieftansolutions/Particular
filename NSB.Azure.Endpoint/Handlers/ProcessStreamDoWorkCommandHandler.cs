using System;
using System.Configuration;
using System.Threading;
using NSB.Azure.Messages;
using NServiceBus;
using NServiceBus.Logging;

namespace NSB.Azure.Endpoint.Handlers
{
    public class ProcessCommandHandler : IHandleMessages<ProcessCommand>
    {
        static readonly ILog Logger = LogManager.GetLogger(typeof(ProcessCommandHandler));
        
        private IBus Bus { get; set; }

        public ProcessCommandHandler(IBus bus)
        {
            Bus = bus;;
        }

        public void Handle(ProcessCommand msg)
        {
            Logger.Error($"ProcessStreamCommandHandler - Begin Processing Msg:{msg.Id}");
            try
            {
                Thread.Sleep(TimeSpan.FromMinutes(2));
               
                Logger.Error($"ProcessCommandHandler - End Processing Msg:{msg.Id}");
            }
            catch (Exception ex)
            {
                Logger.Error($"Error - Msg: {msg.Id}",
                    ex);
                
            }
            
        }
        
    }
}