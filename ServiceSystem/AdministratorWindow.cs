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
    public partial class AdministratorWindow : Form
    {

        PersonelController.Personel srchCriteria = new PersonelController.Personel();

        public AdministratorWindow()
        {
            InitializeComponent();
            dataGridView1.DataSource = PersonelController.GetAllAccounts();
        }

        public void PerformRefresh()
        {
            dataGridView1.Refresh();
            dataGridView1.DataSource = PersonelController.GetAllAccounts();
            textBox1.Text = "";
            textBox2.Text = "";
            comboBox1.ResetText();

        }

        public void PerformRefreshAfterChange()
        {
            dataGridView1.Refresh();
            dataGridView1.DataSource = PersonelController.GetAccountsWithCriteria(srchCriteria);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form form;
            form = new AccountWindow(this, null);
            form.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
            Form form;
            form = new Login();
            form.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var selectedPerson= (PERSONEL)this.dataGridView1.CurrentRow.DataBoundItem;
            try
            {
                Form form;
                form = new AccountWindow(this, selectedPerson);
                form.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("You tried to edit non-selected row", "Dialog",
                        MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var selectedPerson = (PERSONEL)this.dataGridView1.CurrentRow.DataBoundItem;
            if (PersonelController.DeleteAccount(selectedPerson))
            {
                dataGridView1.DataSource = PersonelController.GetAllAccounts();
                MessageBox.Show("Selected row has been removed", "Delete result",
                        MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                MessageBox.Show("Selected row has not been removed. Try again...", "Delete result",
                        MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String firstName = textBox1.Text;
            String lastName = textBox2.Text;
            int role = comboBox1.SelectedIndex;
            srchCriteria = new PersonelController.Personel();
            srchCriteria.first_name = firstName;
            srchCriteria.last_name = lastName;
            switch (role)
            {
                case 0:
                    srchCriteria.role = "man";
                    break;
                case 1:
                    srchCriteria.role = "wrk";
                    break;
                case 2:
                    srchCriteria.role = "adm";
                    break;
                default:
                    srchCriteria.role = "";
                    break;
            }
            dataGridView1.DataSource = PersonelController.GetAccountsWithCriteria(srchCriteria);
        }
    }
}
