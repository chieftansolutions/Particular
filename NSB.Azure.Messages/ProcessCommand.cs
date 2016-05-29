using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;

namespace NSB.Azure.Messages
{
    public class ProcessCommand:ICommand
    {
        public int Id { get; set; } 
    }

}
