using Apache.NMS;
using Newtonsoft.Json;
using System;
using System.Threading;


namespace ActiveMQ.Producer
{
    
    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

    }

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
                var person = new Person
                {
                    FirstName = $"malek {i}",
                    LastName = $"benzemam  {i}"
                };
                SendNewMessageQueue(person);
                SendNewMessageTopic(person);

                Thread.Sleep(1000);
            }

        }
        private static void SendNewMessageQueue(Person message)
        {
            using (IConnection connection = factory.CreateConnection(user, password))
            {
                connection.Start();

                using (ISession session = connection.CreateSession(AcknowledgementMode.AutoAcknowledge))
                using (IDestination dest = session.GetQueue(Queue))
                using (IMessageProducer producer = session.CreateProducer(dest))
                {
                    producer.DeliveryMode = MsgDeliveryMode.Persistent;
                    
                    string json = JsonConvert.SerializeObject(message);
                    
                    producer.Send(session.CreateTextMessage(json));
                    
                    Console.WriteLine($"Sent : {message.FirstName } {message.LastName } ");
                }
            }

        }

        private static void SendNewMessageTopic(Person message)
        {
            using (IConnection connection = factory.CreateConnection(user, password))
            {
                connection.Start();

                using (ISession session = connection.CreateSession(AcknowledgementMode.AutoAcknowledge))
                using (IDestination dest = session.GetTopic(topic))
                using (IMessageProducer producer = session.CreateProducer(dest))
                {
                    producer.DeliveryMode = MsgDeliveryMode.Persistent;
                    string json = JsonConvert.SerializeObject(message);

                    producer.Send(session.CreateTextMessage(json));
                    Console.WriteLine($"Sent : {message.FirstName } {message.LastName } ");
                }
            }
        }
    }
}

