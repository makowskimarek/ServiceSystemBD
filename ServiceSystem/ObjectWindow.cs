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
    public partial class ObjectWindow : Form
    {
        CLIENT currClient = null;
        OBJECT currObject = null;
        ObjectListWindow windowForm2;
        ClientListWindow windowForm1;
        public ObjectWindow(ClientListWindow form1, ObjectListWindow form2, CLIENT client, OBJECT obj)
        {
            InitializeComponent();
            windowForm1 = form1;
            windowForm2 = form2;
            if (client != null)
            {
                currClient = client;
                SetClientValues();
            }
            if (obj != null)
            {
                currObject = obj;
                Console.WriteLine(currObject.nr_obj);
                SetObjectValues();
            }
        }

        public void SetObjectValues()
        {
            textBox1.Text = currObject.code;
            switch (currObject.code_type)
            {
                case "Car":
                    comboBox1.SelectedIndex = 0;
                    break;
                case "Truck":
                    comboBox1.SelectedIndex = 1;
                    break;
            }
        }
        public void SetClientValues()
        {
            label4.Text = currClient.name;
            label5.Text = currClient.fname;
            label6.Text = currClient.lname;
        }

        private void onSaveClick(object sender, EventArgs e)
        {
            OBJECT obj = new OBJECT();
            if (windowForm2 != null)
            {
                obj.nr_obj = currObject.nr_obj;
            }
            else
            {
                obj.id_cli = currClient.id_client;
            }
            Console.WriteLine(obj.nr_obj);
            obj.code = textBox1.Text;
            obj.code_type = comboBox1.Text;
            if (currObject != null)
            {
                if (ObjectController.UpdateObject(obj))
                {
                    windowForm2.PerformRefresh();
                    this.Close();
                    
                }
                else
                {
                    MessageBox.Show("Object details have not been changed. Try again...", "Operation result",
                         MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk);
                }
            }
            else {
                if (ObjectController.AddNewObject(obj))
                {
                    windowForm1.PerformRefresh();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("The new object has not been added. Try again...", "Operation result",
                         MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk);
                }
            }
        }

        private void onBackClick(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
