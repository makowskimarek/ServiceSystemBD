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
    public partial class AccountWindow : Form
    {
        AdministratorWindow admForm;
        int option = 0;
        PERSONEL currentAccount;
        public AccountWindow(AdministratorWindow admWindow, PERSONEL account)
        {
            InitializeComponent();
            admForm = admWindow;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AccountWindow_FormClosing);
            if (account != null)
            {
                currentAccount = account;
                InitializeAccountData(account);
                option = 1;
            }
        }

        private void AccountWindow_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void InitializeAccountData(PERSONEL account)
        {
            textBox1.Text = account.username;
            textBox2.Text = account.pass;
            textBox3.Text = account.fname;
            textBox4.Text = account.lname;
            dateTimePicker1.Value = account.dt_retire;
            switch (account.role) {
                case "adm":
                    comboBox2.SelectedIndex = 0;
                    break;
                case "man":
                    comboBox2.SelectedIndex = 1;
                    break;
                case "wrk":
                    comboBox2.SelectedIndex = 2;
                    break;
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                PersonelController.Personel new_account = new PersonelController.Personel();
                new_account.first_name = textBox3.Text;
                new_account.last_name = textBox4.Text;
                new_account.username = textBox1.Text;
                new_account.password = textBox2.Text;
                new_account.retire_date = dateTimePicker1.Value;
                new_account.role = comboBox2.SelectedItem.ToString();
                if (comboBox2.SelectedIndex == 0)
                {
                    new_account.role = "adm";
                }
                else if (comboBox2.SelectedIndex == 1)
                {
                    new_account.role = "man";
                }
                else if (comboBox2.SelectedIndex == 2)
                {
                    new_account.role = "wrk";
                }
                else
                {
                    new_account.role = "";
                }

                if (new_account.first_name.Length != 0 && new_account.last_name.Length != 0 &&
                    new_account.username.Length != 0 && new_account.password.Length != 0 &&
                    new_account.role.Length != 0)
                {

                    if (option == 0)
                    {
                        if (PersonelController.RegisterAccount(new_account))
                        {
                            admForm.PerformRefresh();
                            MessageBox.Show("New account has been registered", "Account registration",
                                 MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk);
                            this.Close();

                        }
                        else
                        {
                            MessageBox.Show("New account has not been registered. Try again...", "Account registration",
                                 MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk);
                        }
                    }
                    else
                    {
                        if (PersonelController.ChangePersonelItemData(currentAccount, new_account))
                        {
                            admForm.PerformRefreshAfterChange();
                            MessageBox.Show("Account details has been changed", "Account details edition",
                                MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk);
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Account details has not been changed. Try again...", "Account details edition",
                                 MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk);
                        }
                    }

                }
                else
                {
                    MessageBox.Show("Complete all fields, then try co execute operation.", "Account details",
                                 MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Complete all fields, then try co execute operation.", "Account details",
                                 MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk);
            }
            
        }
    }
}
