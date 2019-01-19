using Experimental.System.Messaging;
using System;
using System.Messaging;
using System.Threading;

namespace Msmq.Producer
{
    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

    }

    class Program
    {
        static void Main(string[] args)
        {

            //const string queueName = @".\private$\TestQueue";
            const string queueName = @"FormatName:Direct=TCP:127.0.0.1\private$\TestQueue";
            for (int i = 0; i < 1000; i++)
            {
                Person p = new Person()
                {
                    FirstName = $"Name {i}",
                    LastName = $".Com {i}"
                };
                SendMessageToQueue(queueName, p);
                Thread.Sleep(1000);
            }

        }


        private static void SendMessageToQueue(string queueName, Person message)

        {

            // check if queue exists, if not create it

            MessageQueue msMq = null;

            //if (!MessageQueue.Exists(queueName))
            //{
            //    msMq = MessageQueue.Create(queueName);
            //}
            //else
            //{
            //    msMq = new MessageQueue(queueName);
            //}
            msMq = new MessageQueue(queueName);
            try
            {
                // msMq.Send("Sending data to MSMQ at " + DateTime.Now.ToString());
                msMq.Send(message);
            }
            catch (MessageQueueException ee)
            {
                Console.Write(ee.ToString());
            }
            catch (Exception eee)
            {
                Console.Write(eee.ToString());
            }
            finally
            {
                msMq.Close();
            }
            Console.WriteLine($"Message sent ...... {message.FirstName}");

        }



    }
}
