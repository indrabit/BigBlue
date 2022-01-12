using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Inventory_Read.Include;



namespace Inventory
{
    
    public partial class frmLogin : Form
    {
       
        public frmLogin()
        {
            InitializeComponent();
        }
        SQLConfig_Read config = new SQLConfig_Read();
        

        

        private void label2_Click(object sender, EventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (chkPass.Checked)
            {
                string a = txtPassword.Text;
                txtPassword.PasswordChar = '\0';
            }
            else
                txtPassword.PasswordChar = '*';

        }

        private void btnLogIn_Click(object sender, EventArgs e)
        {
            
            string sql = "Select user_name, pass, fullname, userId from tbluser where user_name='" + txtUserName.Text + "' and pass=sha1('" + txtPassword.Text+"')";
            config.ExecuteQuery(sql);
                    
            if (config.dt.Rows.Count>0)
            {
                this.Hide();
                frmMain fm = new frmMain();
                fm.ShowDialog();                               
            }
        }
    }
}
