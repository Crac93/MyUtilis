using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace MyUtilis.Comunication
{
    class MSMQ
    {
        const string MessagerPathDefault = @".\private$\QueueName";

        public void SendMSMQ(string messageToSend, string messagerPath = MessagerPathDefault)
        {
            try
            {
                using (MessageQueue queue = new MessageQueue())
                {
                    queue.Path = messagerPath;
                    if (!MessageQueue.Exists(queue.Path))
                    {
                        MessageQueue.Create(queue.Path);
                    }
                    var message = new System.Messaging.Message();
                    message.Body = messageToSend;
                    queue.Send(message);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public string ReceiveMSMQ(string messagerPath = MessagerPathDefault)
        {
            try
            {
                using (MessageQueue queue = new MessageQueue())
                {
                    queue.Path = messagerPath;
                    var message = new System.Messaging.Message();
                    message = queue.Receive(new TimeSpan(0, 0, 3));
                    message.Formatter = new XmlMessageFormatter(new String[] { "System.String.mscorlib" });
                    string msgReturn = message.Body.ToString();
                    return msgReturn;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}

