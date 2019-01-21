using Apache.NMS;
using System;

namespace ActiveMQ.Consumer
{
    [Serializable]
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

        static NMSConnectionFactory factory = new NMSConnectionFactory(brokerUri);

        static void Main(string[] args)
        {
            Console.WriteLine("Waiting for messages");

            // Read all messages off the queue
            while (ReadNextMessageTopic())
            {
                Console.WriteLine("Successfully read message ");
            }

            Console.WriteLine("Finished");
        }


        static bool ReadNextMessageQueue()
        {
            using (IConnection connection = factory.CreateConnection(user, password))
            {
                connection.Start();
                using (ISession session = connection.CreateSession(AcknowledgementMode.AutoAcknowledge))
                using (IDestination dest = session.GetQueue(Queue))
                using (IMessageConsumer consumer = session.CreateConsumer(dest))
                {
                    IMessage msg = consumer.Receive();
                    if (msg is ITextMessage)
                    {
                        ITextMessage txtMsg = msg as ITextMessage;
                        string body = txtMsg.Text;

                        Console.WriteLine($"Received message: {txtMsg.Text}");

                        return true;
                    }
                    if (msg is IObjectMessage)
                    {
                        IObjectMessage objectMessage = msg as IObjectMessage;
                        if (objectMessage != null)
                        {
                            Person person = objectMessage.Body as Person;
                            if (person != null)
                            {

                                Console.WriteLine($"Received message: {person.FirstName}");

                            }
                        }

                        return true;
                    }
                    else
                    {
                        Console.WriteLine("Unexpected message type: " + msg.GetType().Name);
                    }
                }
            }

            return false;
        }

        static bool ReadNextMessageTopic()
        {
            using (IConnection connection = factory.CreateConnection(user, password))
            {
                connection.Start();
                using (ISession session = connection.CreateSession(AcknowledgementMode.AutoAcknowledge))
                using (IDestination dest = session.GetTopic(topic))
                using (IMessageConsumer consumer = session.CreateConsumer(dest))
                {
                    IMessage msg = consumer.Receive();

                    if (msg is ITextMessage)
                    {
                        ITextMessage txtMsg = msg as ITextMessage;
                        string body = txtMsg.Text;
                        Console.WriteLine($"Received message: {txtMsg.Text}");

                        return true;
                    }
                    else
                    if (msg is IObjectMessage)
                    {
                        IObjectMessage objectMessage = msg as IObjectMessage;
                        if (objectMessage != null)
                        {
                            Person person = objectMessage.Body as Person;
                            if (person != null)
                            {

                                Console.WriteLine($"Received message: {person.FirstName}");

                            }
                        }

                        return true;
                    }
                    else
                    {
                        Console.WriteLine("Unexpected message type: " + msg.GetType().Name);
                    }
                }
            }

            return false;
        }
    }


}
