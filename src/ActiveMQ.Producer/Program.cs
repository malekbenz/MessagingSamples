using Apache.NMS;
using System;
using System.Threading;

namespace ActiveMQ.Producer
{
    class Program
    {
        static string user = "admin";
        static string password = "password";

        static string brokerUri = $"activemq:tcp://localhost:61616";  // Default port

        static string topic = "TextQueue";
        static string Queue = "TEST";

        static string text = "Message  ";

        static NMSConnectionFactory factory = new NMSConnectionFactory(brokerUri);

        static void Main(string[] args)
        {
            for (int i = 0; i < 1000; i++)
            {
                SendNewMessageQueue($"Sent {text} messages -  {i}");
                Thread.Sleep(1000);
            }

        }
        private static void SendNewMessageQueue(string text)
        {
            using (IConnection connection = factory.CreateConnection(user, password))
            {
                connection.Start();

                using (ISession session = connection.CreateSession(AcknowledgementMode.AutoAcknowledge))
                using (IDestination dest = session.GetQueue(Queue))
                using (IMessageProducer producer = session.CreateProducer(dest))
                {
                    producer.DeliveryMode = MsgDeliveryMode.NonPersistent;
                    producer.Send(session.CreateTextMessage(text));
                    Console.WriteLine($"Sent : {text} ");
                }
            }
            
        }
        private static void SendNewMessageTopic(string text)
        {
            using (IConnection connection = factory.CreateConnection(user, password))
            {
                connection.Start();

                using (ISession session = connection.CreateSession(AcknowledgementMode.AutoAcknowledge))
                using (IDestination dest = session.GetTopic(topic))
                using (IMessageProducer producer = session.CreateProducer(dest))
                {
                    producer.DeliveryMode = MsgDeliveryMode.NonPersistent;
                    producer.Send(session.CreateTextMessage(text));
                    Console.WriteLine($"Sent : {text} ");
                }
            }
        }
    }
}

