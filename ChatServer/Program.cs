using System;
using System.ServiceModel;

namespace ChatServer
{
    public class Program
    {
        static void Main(string[] args)
        {
            using (var host = new ServiceHost(typeof(ChatApp.ServiceChat)))
            {
                host.Open();
                Console.WriteLine("Server is running");
                Console.ReadLine();
            }
        }
    }
}
