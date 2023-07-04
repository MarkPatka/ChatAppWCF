using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ChatClient
{

    public partial class MainWindow : Window, ServiceChat.IServiceChatCallback
    {
        bool IsConnected = false;
        ServiceChat.ServiceChatClient client;
        int id; 


        public MainWindow()
        {
            InitializeComponent();
        }

        private void ConnectUser()
        {
            if (!IsConnected)
            {
                client = new ServiceChat.ServiceChatClient(new System.ServiceModel.InstanceContext(this));
                id = client.Connect(txtUsername.Text);

                txtUsername.IsEnabled = false;
                btnConnect.Content = "Отключить";
                IsConnected = true;
            }
        }

        private async void DisconnectUser()
        {
            if (IsConnected)
            {
                await client.DisconnectAsync(id);
                client = null;

                txtUsername.IsEnabled = true;
                btnConnect.Content = "Подключить";
                IsConnected = false;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DisconnectUser();
        }

        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            if (IsConnected) DisconnectUser();
            else ConnectUser();
        }

        private void txtMessage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (client != null)
                {
                    client?.SendMessage(txtMessage.Text, id);
                    txtMessage.Text = string.Empty;
                }
            }
        }

        public void MessageCallback(string message)
        {
            lbxChat.Items.Add(message);
        }
    }
}
