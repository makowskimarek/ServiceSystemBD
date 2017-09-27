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
    public partial class ObjectListWindow : Form
    {
        public PERSONEL manager;
        public CLIENT currClient;
        public ObjectListWindow(PERSONEL man, CLIENT client)
        {
            InitializeComponent();
            manager = man;
            dataGridView1.DataSource = ObjectController.GetAllObjects();
            if (client != null)
            {
                currClient = client;
                textBox1.Text = client.name;
                textBox2.Text = client.fname;
                textBox3.Text = client.lname;
                dataGridView1.DataSource = ObjectController.GetObjectsByCriteria(currClient, null);
            }
            //dataGridView1.Columns[4].Visible = false;
            //dataGridView1.Columns[5].Visible = false;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Form form;
            ObjectAndClient oac = (ObjectAndClient)this.dataGridView1.CurrentRow.DataBoundItem;
            CLIENT client = new CLIENT();
            client.name = oac.name;
            client.fname = oac.fname;
            client.lname = oac.lname;
            OBJECT obj = new OBJECT();
            obj.code = oac.code;
            obj.code_type = oac.code_type;
            obj.nr_obj = oac.nr_obj;
            form = new RequestListWindow(manager, client, obj);
            form.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void onDeleteItemClick(object sender, EventArgs e)
        {
            var selectedObject = (ObjectAndClient)this.dataGridView1.CurrentRow.DataBoundItem;
            OBJECT obj = new OBJECT();
            obj.code = selectedObject.code;
            obj.code_type = selectedObject.code_type;
            obj.id_cli = selectedObject.id_client;
            obj.nr_obj = selectedObject.nr_obj;
            ObjectController.DeleteObject(obj);
            PerformRefresh();

        }

        public void PerformRefresh() {
            dataGridView1.DataSource = ObjectController.GetAllObjects();
        }

        private void onEditItemClick(object sender, EventArgs e)
        {
            var selectedObjectAndClient = (ObjectAndClient)this.dataGridView1.CurrentRow.DataBoundItem;
            OBJECT obj = new OBJECT();
            obj.code = selectedObjectAndClient.code;
            obj.code_type = selectedObjectAndClient.code_type;
            obj.nr_obj = selectedObjectAndClient.nr_obj;
            CLIENT client = new CLIENT();
            client.name = selectedObjectAndClient.name;
            client.fname = selectedObjectAndClient.fname;
            client.lname = selectedObjectAndClient.lname;
            client.tel = selectedObjectAndClient.tel;

            Form form;
            form = new ObjectWindow(null, this, client, obj);
            form.Show();
        }

        private void onSearchClick(object sender, EventArgs e)
        {
            CLIENT newClient = new CLIENT();
            newClient.name = textBox1.Text;
            newClient.fname = textBox2.Text;
            newClient.lname = textBox3.Text;

            OBJECT newObject = new OBJECT();
            if (textBox4.Text.Length != 0)
            {
                newObject.code = textBox4.Text.PadRight(10);
            }
            else
            {
                newObject.code = "";
            }
            newObject.code_type = comboBox1.Text;
            dataGridView1.DataSource = ObjectController.GetObjectsByCriteria(newClient, newObject);
        }

        private void onAddClick(object sender, EventArgs e)
        {
            var selectedObj = (ObjectAndClient)this.dataGridView1.CurrentRow.DataBoundItem;
            OBJECT obj = new OBJECT();
            obj.code = selectedObj.code;
            obj.code_type = selectedObj.code_type;
            obj.nr_obj = selectedObj.nr_obj;
            Console.WriteLine(obj.nr_obj);
            Form form;
            form = new RequestWindow(obj, null, manager, null, this);
            form.Show();
        }
    }
}
