using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Inventory_Read.Include;

namespace Inventory
{
    public partial class frmMain : Form
    {
        string sql;
        public frmMain()
        {
            InitializeComponent();
        }
        SQLConfig_Read config = new SQLConfig_Read();
        
      


        private void frmMain_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            sql = "Select D.DiveCharter_ID,concat(S.Name,'(',D.DiveCharter_ID,')','-',D.Date) as Name from divecharters D Inner Join divesites S on D.DiveSiteID=S.DiveSite_ID order by D.DiveCharter_ID asc";
            config.combo(sql, comboBoxSelect);
        }

        private void comboBoxSelect_SelectedIndexChanged(object sender, EventArgs e)
        {

            string strId = comboBoxSelect.SelectedValue.ToString();
            if(strId != null)
            {
                LoadDiverSite(strId);
            }
            
        }
        private void LoadDiverSite(string diveId)
        {
            sql="Select distinct b.CustomerID, CONCAT(c.fname, \" \", c.lname) AS CustName, c.CertificationLevel "
                + "from bookings b inner join customers c on b.CustomerID=c.Customer_ID where "
                + " b.CustomerID  NOT IN (Select CustomerID from bookings where DiveCharterID='"+ diveId + "') order by b.CustomerID asc";
            
        }
    }
}
