using System;
using Experimental.System.Messaging;

namespace Msmq.Consumer
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
            
            const string queueName = @".\private$\TestQueue";
            while (true)
            {
                Console.WriteLine("Waiting for new Data ......... \n");
                ReceiveMessageFromQueue(queueName);
            }
        }

        private static void ReceiveMessageFromQueue(string queueName)
        {
            MessageQueue msMq = msMq = new MessageQueue(queueName);
            try
            {
                // msMq.Formatter = new XmlMessageFormatter(new Type[] {typeof(string)});
                msMq.Formatter = new XmlMessageFormatter(new Type[] { typeof(Person) });
                var message = (Person)msMq.Receive().Body;
                Console.WriteLine("FirstName: " + message.FirstName + ", LastName: " + message.LastName);
                // Console.WriteLine(message.Body.ToString());
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

            Console.WriteLine("Message received ......");

        }
    }
}
