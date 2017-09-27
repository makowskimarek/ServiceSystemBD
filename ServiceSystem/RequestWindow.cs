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
    public partial class RequestWindow : Form
    {
        OBJECT currObject;
        PERSONEL manager;
        REQUEST currRequest;
        RequestListWindow requestForm;
        ObjectListWindow objectForm;
        public RequestWindow(OBJECT obj, REQUEST req, PERSONEL man, RequestListWindow form, ObjectListWindow form1)
        {
            InitializeComponent();
            manager = man;
            currObject = obj;
            if (req != null)
            {
                currRequest = req;
                InitializeRequestDetails();
            }
            if (form != null) {
                requestForm = form;
            }

            if (form1 != null)
            {
                objectForm = form1;
            }
            InitializeObject();
        }

        public void InitializeRequestDetails()
        {
            switch (currRequest.status)
            {
                case "OPEN":
                    comboBox1.SelectedIndex = 0;
                    break;
                case "PROG":
                    comboBox1.SelectedIndex = 1;
                    break;
                case "FIN":
                    comboBox1.SelectedIndex = 2;
                    break;
                case "CANC":
                    comboBox1.SelectedIndex = 3;
                    break;
            }
            dateTimePicker1.Value = currRequest.dt_req;
            if (currRequest.dt_fin_cancel.Equals("NULL"))
            {
                dateTimePicker2.Value = DateTime.MinValue;
            }
            else
            {
                dateTimePicker2.Value = (DateTime)currRequest.dt_fin_cancel;
            }
            richTextBox1.Text = currRequest.descr;
            richTextBox2.Text = currRequest.result;

        }

        public void InitializeObject()
        {
            label6.Text = currObject.code;
            label7.Text = currObject.code_type;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void RequestWindow_Load(object sender, EventArgs e)
        {

        }

        private void onSaveClicked(object sender, EventArgs e)
        {
            REQUEST request = new REQUEST();
            try
            {
                request.descr = richTextBox1.Text;
                request.result = richTextBox2.Text;
                request.dt_req = dateTimePicker1.Value;
                request.dt_fin_cancel = dateTimePicker2.Value;
                request.id_man = manager.id_pers;
                request.nr_obj = currObject.nr_obj;
                switch (comboBox1.Text)
                {
                    case "OPENED":
                        request.status = "OPEN";
                        break;
                    case "PROGRESS":
                        request.status = "PROG";
                        break;
                    case "FINISHED":
                        request.status = "FIN";
                        break;
                    case "CANCELLED":
                        request.status = "CANC";
                        break;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("The values inserted to fields were not correct. Please try again...", "Error details",
                     MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk);
            }
            
            if (currRequest != null)
            {
                request.id_req = currRequest.id_req;
                if (RequestController.ChangeRequestDetails(request))
                {
                    requestForm.PerformRefresh();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("The values inserted to fields were not correct. Please try again...", "Error details",
                     MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk);
                }
            }
            else
            {
                if (RequestController.AddNewRequest(request))
                {
                    objectForm.PerformRefresh();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("The values inserted to fields were not correct. Please try again...", "Error details",
                     MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk);

                }
            }
            
        }
    }
}
