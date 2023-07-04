using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Xml.Linq;

namespace ChatApp
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ServiceChat : IServiceChat
    {
        List<ServerUser> users = new List<ServerUser>();

        int nextId = 1;

        public int Connect(string name)
        {
            ServerUser user = new ServerUser()
            {
                Id = nextId++,
                Name = name,
                OperationContext = OperationContext.Current
            };
            nextId++;
            SendMessage($" {name} подключился к чату.", 0);
            users.Add(user);

            return user.Id;
        }

        public void Disconnect(int id)
        {
            var user = users.FirstOrDefault(i => i.Id == id);
            if (user != null)
            {
                if (users.Count > 1)
                    SendMessage($" {user.Name} вышел из чата.", 0);

                users.Remove(user);
            }
        }

        public void SendMessage(string message, int id)
        {
            foreach (var user in users)
            {
                string answer = DateTime.Now.ToShortTimeString();
                var targetUser = users.FirstOrDefault(i => i.Id == id);
                if (targetUser != null)
                {
                    answer +=  " [" + targetUser.Name + "]: ";
                }
                answer += message;

                user.OperationContext.GetCallbackChannel<IServerChatCallback>().MessageCallback(answer);
            }
        }
    }
}
