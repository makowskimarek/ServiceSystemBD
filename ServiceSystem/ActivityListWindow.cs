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
    public partial class ActivityListWindow : Form
    {
        public ActivityListWindow(Mode mode, int id)
        {
            InitializeComponent();
            this.mode = mode;

            if (this.mode == Mode.WORKER)
            {
                buttonClose.Text = "Logout";
                buttonDelete.Hide();
                groupClient.Hide();
                groupWorker.Hide();
                dataGridView1.DataSource = ActivityController.GetActivitiesByWrkId(id);
            }
            else
            {
                dataGridView1.DataSource = ActivityController.GetAllActivities();
            }
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            if(mode == Mode.MANAGER)
                this.Close();
            else if(mode == Mode.WORKER)
            {
                this.Close();
                Form form;
                form = new Login();
                form.Show();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                ObjectClientActivityRequestWorker selectedOcarw = (ObjectClientActivityRequestWorker)this.dataGridView1.CurrentRow.DataBoundItem;
                REQUEST req = new REQUEST();
                req.id_req = selectedOcarw.id_req;

                ACTIVITY selectedActivity = new ACTIVITY();
                selectedActivity.id_act = selectedOcarw.id_act;
                selectedActivity.act_type = selectedOcarw.act_type;
                selectedActivity.status = selectedOcarw.status;
                selectedActivity.result = selectedOcarw.result;
                selectedActivity.descr = selectedOcarw.description;
                selectedActivity.seq_no = selectedOcarw.sequence;
                selectedActivity.dt_req = selectedOcarw.dt_req;
                selectedActivity.dt_fin_cancel = selectedOcarw.dt_fin_cancel;

                OBJECT selectedObject = new OBJECT();
                selectedObject.code = selectedOcarw.code;
                selectedObject.code_type = selectedOcarw.code_type;

                Form form;
                if (mode == Mode.MANAGER)
                    form = new ActivityWindow(Mode.MANAGER, selectedActivity, selectedObject, req, this);
                else
                    form = new ActivityWindow(Mode.WORKER, selectedActivity, selectedObject, req, this);
                form.Show();
            }
            catch(Exception ex)
            {
                MessageBox.Show("You tried to edit non-selected row", "Dialog",
                        MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            
        }

        private void OnDeleteClick(object sender, EventArgs e)
        {
            var selected = (ObjectClientActivityRequestWorker)this.dataGridView1.CurrentRow.DataBoundItem;
            int id = selected.id_act;
            ActivityController.DeleteActivity(id);
            PerformRefresh();

        }
        public void PerformRefresh()
        {
            dataGridView1.DataSource = ActivityController.GetAllActivities();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String objectType = comboBox1.Text;
            String objectName = textBox4.Text;
            String activityName = comboBox2.Text;
            String workerFname = textBox7.Text;
            String workerLname = textBox6.Text;
            String clientName = textBox1.Text;
            String clientFname = textBox2.Text;
            String clientLname = textBox3.Text;
            int id_req;
            if (!textBox5.Text.Equals(""))
            {
                id_req = Int32.Parse(textBox5.Text);
            }
            else
            {
                id_req = 0;
            }
            ActivityController.SearchActivity srchCriteria = new ActivityController.SearchActivity();
            switch (comboBox2.Text) {
                case "REPAIR":
                    srchCriteria.act_type = "REP";
                    break;
                case "DIAGNOSE":
                    srchCriteria.act_type = "DIAG";
                    break;
            }
            srchCriteria.id_req = id_req;
            srchCriteria.code_type = objectName;
            srchCriteria.name_type = objectType;
            srchCriteria.fnameC = clientFname;
            srchCriteria.lnameC = clientLname;
            srchCriteria.nameC = clientName;
            srchCriteria.lnameW = workerLname;
            srchCriteria.fnameW = workerFname;
            dataGridView1.DataSource = ActivityController.GetActivityBySearchCriteria(srchCriteria);
        }
    }
}
