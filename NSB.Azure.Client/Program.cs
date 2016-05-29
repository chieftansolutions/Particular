using System;
using System.IO;
using System.Linq;
using NSB.Azure.Messages;
using NSB.Azure.Endpoint.Common;
using NServiceBus;

namespace WatchGuard.Integration.Test
{

    class Program
    {
        static void Main()
        {
            var busConfiguration = new BusConfiguration();
     
            busConfiguration.EndpointName("NSB.Azure.Client");
            busConfiguration.UseAzureConfig();

            using (var bus = Bus.Create(busConfiguration).Start())
            {
                SendOrder(bus);
            }
        }
        static void SendOrder(IBus bus)
        {

            Console.WriteLine(@"Press (F1)Send 1 Cmd, (F2)Send 2 Cmds, (F3)Send 3 Cmds, (F4)Send 4 Mds, (F5)Send 5 Cmds, (F6) Send 6 Cmds (Z) to send Cmds by providing file paths. (i.e. c:\temp\file1.txt|c:\file2.txt )");
            Console.WriteLine("Press Enter to exit");

            while (true)
            {
                var key = Console.ReadKey();
                Console.WriteLine();

                switch (key.Key)
                {
                    case ConsoleKey.F1:
                        bus.Send(new ProcessCommand {Id = 1});
                        break;
                    case ConsoleKey.F2:
                        bus.Send(new ProcessCommand { Id = 1 });
                        bus.Send(new ProcessCommand { Id = 2 });
                        break;
                    case ConsoleKey.F3:
                        bus.Send(new ProcessCommand { Id = 1 });
                        bus.Send(new ProcessCommand { Id = 2 });
                        bus.Send(new ProcessCommand { Id = 3 });
                        break;
                    case ConsoleKey.F4:
                        bus.Send(new ProcessCommand { Id = 1 });
                        bus.Send(new ProcessCommand { Id = 2 });
                        bus.Send(new ProcessCommand { Id = 2 });
                        bus.Send(new ProcessCommand { Id = 3 });
                        bus.Send(new ProcessCommand { Id = 4 });
                        break;
                    case ConsoleKey.F5:
                        bus.Send(new ProcessCommand { Id = 1 });
                        bus.Send(new ProcessCommand { Id = 2 });
                        bus.Send(new ProcessCommand { Id = 3 });
                        bus.Send(new ProcessCommand { Id = 4 });
                        bus.Send(new ProcessCommand { Id = 5 });

                        break;
                    case ConsoleKey.F6:
                        bus.Send(new ProcessCommand { Id = 1 });
                        bus.Send(new ProcessCommand { Id = 2 });
                        bus.Send(new ProcessCommand { Id = 3 });
                        bus.Send(new ProcessCommand { Id = 4 });
                        bus.Send(new ProcessCommand { Id = 5 });
                        bus.Send(new ProcessCommand { Id = 6 });
                        break;
                    case ConsoleKey.Enter:
                        return;
                }
            }
        }
      
    }
    
}
