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
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String username = textBox1.Text;
            String password = textBox2.Text;
            PERSONEL person;
            Form form;
            if (comboBox1.Text == "Administrator")
            {
                if (LoginController.Login(username, password, "adm") != null)
                {
                    form = new AdministratorWindow();
                    form.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Bad login data", "Login state",
                        MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk);
                }

            }
            else if (comboBox1.Text == "Manager")
            {
                person = LoginController.Login(username, password, "man");
                if ( person != null)
                {
                    form = new ClientListWindow(person);
                    form.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Bad login data", "Login state",
                        MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk);
                }

            }
            else if (comboBox1.Text == "Worker")
            {
                person = LoginController.Login(username, password, "wrk");
                if (person != null)
                {
                    form = new ActivityListWindow(Mode.WORKER, person.id_pers);
                    form.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Bad login data", "Login state",
                        MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk);
                }
            }
            else if(username.Length != 0 && password.Length != 0)
            {
                MessageBox.Show("Choose account type", "Login state",
                     MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 2;
        }
    }
}
