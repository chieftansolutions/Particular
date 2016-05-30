using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;

namespace NSB.AzureStorage.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            //Test1();
            Test2();

            Console.ReadLine();

        }

        /// <summary>
        /// Demonstrates Deleting a message after Timout of Invisibility
        /// </summary>
        static void Test1()
        {
            var queue = GetQueueClient("queuetest");
            //Create Message
            Console.WriteLine($"{DateTime.Now} - Creating message on queue");
            var message = new CloudQueueMessage("Hello, World");
            queue.AddMessage(message);
            var retrievedMessage = queue.GetMessage();

            //Checking that message is invisible
            var peekedMessage1 = queue.PeekMessage();
            if (peekedMessage1 != null)
                Console.WriteLine($"{DateTime.Now} - Message is visible on queue: {peekedMessage1.AsString}");

            //Let Message Invisibility timeout (30sec)
            Thread.Sleep(TimeSpan.FromSeconds(90));

            //Checking that message is in queue and become visible again
            var peekedMessage = queue.PeekMessage();
            Console.WriteLine($"{DateTime.Now} - Message has become visible again: {peekedMessage.AsString}");

            Console.WriteLine($"{DateTime.Now} - Deleting message on queue");
            queue.DeleteMessage(retrievedMessage);
        }

        /// <summary>
        /// Demonstrates the invalidation of the Pop Receipt after a new Get message is executed.
        /// </summary>
        static void Test2()
        {
            var queue = GetQueueClient("queuetest");
            //Create Message
            Console.WriteLine($"{DateTime.Now} - Creating message on queue");
            var message = new CloudQueueMessage("Hello, World");
            queue.AddMessage(message);
            var retrievedMessage = queue.GetMessage();

            //Checking that message is invisible
            var peekedMessage1 = queue.PeekMessage();
            if (peekedMessage1 != null)
                Console.WriteLine($"{DateTime.Now} - Message is visible on queue: {peekedMessage1.AsString}");

            //Let Message Invisibility timeout (30sec)
            Thread.Sleep(TimeSpan.FromSeconds(90));

            //Checking that message is in queue and become visible again
            var peekedMessage = queue.PeekMessage();
            Console.WriteLine($"{DateTime.Now} - Message has become visible again: {peekedMessage.AsString}");

            //Second retrieve invalidates the first PoP Receipt
            var retrievedMessage1 = queue.GetMessage();

            //this should fail since pop reciept is not valid anylonger
            Console.WriteLine($"{DateTime.Now} - Deleting original message on queue");
            try
            {
                queue.DeleteMessage(retrievedMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            
        }


        static CloudQueue GetQueueClient(string queueName)
        {
            var storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            var queueClient = storageAccount.CreateCloudQueueClient();
            var queue = queueClient.GetQueueReference(queueName);
            queue.CreateIfNotExists();

            return queue;
        }


    }
}
