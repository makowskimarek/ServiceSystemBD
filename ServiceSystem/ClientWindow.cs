using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BizzLayer;

namespace ServiceSystem
{
    public partial class ClientWindow : Form
    {
        ClientListWindow clientListWindow;
        CLIENT currentClient;
        int option = 0;
        public ClientWindow(ClientListWindow clListWindow, CLIENT client)
        {
            clientListWindow = clListWindow;
            InitializeComponent();
            if (client != null) {
                currentClient = client;
                InitializeClientData();
                option = 1;
            }
        }

        public void InitializeClientData()
        {
            textBox1.Text = currentClient.name;
            textBox2.Text = currentClient.fname;
            textBox3.Text = currentClient.lname;
            textBox4.Text = currentClient.tel.ToString();
            ADRES currentClientAddress = ClientController.GetClientAddress(currentClient);
            textBox5.Text = currentClientAddress.street;
            textBox6.Text = currentClientAddress.post_code;
            textBox7.Text = currentClientAddress.city;
            textBox8.Text = currentClientAddress.nation;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            CLIENT client = new CLIENT();
            ADRES address = new ADRES();
            client.name = textBox1.Text;
            client.fname = textBox2.Text;
            client.lname = textBox3.Text;
            client.tel = int.Parse(textBox4.Text);
            address.street = textBox5.Text;
            address.post_code = textBox6.Text;
            address.city = textBox7.Text;
            address.nation = textBox8.Text;

            if (option == 0)
            {
                if (ClientController.InsertNewClient(client, address))
                {
                    this.Close();
                    clientListWindow.PerformRefresh();
                }
                else
                {
                    //MessageBox.Show("New client has not been inserted. Check inserted values and try again", "Insert state",
                     //MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk);
                }
            }
            else
            {
                if (ClientController.UpdateClientData(currentClient, client, address))
                {
                    this.Close();
                    clientListWindow.PerformRefresh();
                }
                else
                {

                }
            }

        }
    }
}
