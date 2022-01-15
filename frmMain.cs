using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BigBlue.Include;
using System.IO;
using System.Reflection;
using BigBlue.SQL;
using BigBlue.Validation;
using System.Text.RegularExpressions;

namespace Inventory
{
    public partial class frmMain : Form
    {
        string sql;
        int maxCapacity;
        int capacityleft;
        SQLConfigWrite sqlW = new SQLConfigWrite();
        public frmMain()
        {
            InitializeComponent();
        }
        //**************CALLL Class function***************
        SQLConfig_Read sqlConfigRead = new SQLConfig_Read();
        ValidationFunction validCtr = new ValidationFunction();
        private void frmMain_Load(object sender, EventArgs e)
        {

            LoadComboBox();

            LoadData();
            comboBoxSelect_Click(sender, e);
            LoadCustomer("All");

        }
        private void LoadComboBox()
        {
            DataTable dtc;
            dtc = new DataTable();
            dtc.Columns.AddRange(new DataColumn[] { new DataColumn("Text", typeof(string)), new DataColumn("Value", typeof(string)) });
            dtc.Rows.Add(" Get Hire ? -No", "N");
            dtc.Rows.Add("Get Hire ? -Yes", "Y");

            comboBox2.DataSource = dtc;
            comboBox2.DisplayMember = "Text";
            comboBox2.ValueMember = "Value";
            comboBox2.SelectedIndex = 0;
            //**************************************************
            //*********Data Load in Cert***********************
            DataTable dtCert;
            dtCert = new DataTable();
            dtCert.Columns.AddRange(new DataColumn[] { new DataColumn("Text", typeof(string)), new DataColumn("Value", typeof(string)) });
            dtCert.Rows.Add("OW", "OW");
            dtCert.Rows.Add("ADV", "ADV");

            comboBox1.DataSource = dtCert;
            comboBox1.DisplayMember = "Text";
            comboBox1.ValueMember = "Value";
            comboBox1.SelectedIndex = 0;

            //**************************************************
            //*********Data Load in Cert***********************

            DataTable dtAgency;
            dtAgency = new DataTable();
            dtAgency.Columns.AddRange(new DataColumn[] { new DataColumn("Text", typeof(string)), new DataColumn("Value", typeof(string)) });
            dtAgency.Rows.Add("PADI", "PADI");
            dtAgency.Rows.Add("NAUI", "NAUI");
            dtAgency.Rows.Add("SSI", "SSI");
            dtAgency.Rows.Add("BSAC", "BSAC");
            dtAgency.Rows.Add("NASDS", "NASDS");

            comboBox3.DataSource = dtAgency;
            comboBox3.DisplayMember = "Text";
            comboBox3.ValueMember = "Value";
            comboBox3.SelectedIndex = 0;

        }
        private void LoadData()
        {
            sql = "Select D.DiveCharter_ID,concat(S.Name,'(',D.DiveCharter_ID,')','-',D.Date) as Name from divecharters D Inner Join divesites S on D.DiveSiteID=S.DiveSite_ID order by D.DiveCharter_ID asc";
            sqlConfigRead.combo(sql, comboBoxSelect);
        }
        private void CapacityCheck()
        {
            capacityleft = maxCapacity - (dataGridViewBooking.Rows.Count);
            lblCalculateDiveCapacity.Text = "Dive charter has " + capacityleft + " place(s) available";
        }
        private void LoadDiverSite(string diveId)
        {
            /*   sql = "Select distinct b.CustomerID, CONCAT(c.fname, \" \", c.lname) AS CustName, c.CertificationLevel "
                   + "from bookings b inner join customers c on b.CustomerID=c.Customer_ID where "
                   + " b.CustomerID  NOT IN (Select CustomerID from bookings where DiveCharterID='" + diveId + "') order by b.CustomerID asc"; */

            sql = "select A.DiveSiteID,B.Name,B.Description,B.Location,B.MaxDepth,B.MinCert,A.Date as charter_date,"
                    + " C.Name as DriveBoat,CONCAT(E.FName,\" \",E.LName) as DIVEMASTERNAME,CONCAT(F.FName,\" \",F.LName) as SKIPPERNAME,"
                    + "A.BaseCost,A.GearHire, C.CAPACITY from ((((divecharters A INNER JOIN divesites B ON A.DiveSiteID=B.DiveSite_ID)\n" +
                " LEFT join diveboats C on A.DiveBoatID=C.DiveBoat_ID)\n" +
                " LEFT JOIN  employees E ON A.Divemaster=E.Employee_ID)\n" +
                " LEFT JOIN  employees F ON A.Skipper=F.Employee_ID) where A.DiveCharter_ID='" + diveId + "'";


            sqlConfigRead.dataTxtLoad(sql);
            if (sqlConfigRead.dt.Rows.Count > 0)
            {
                textBox1.Text = sqlConfigRead.dt.Rows[0].Field<string>("Name");
                textBox2.Text = sqlConfigRead.dt.Rows[0].Field<string>("Location");
                textBox3.Text = Convert.ToString(sqlConfigRead.dt.Rows[0].Field<DateTime>("charter_date"));
                textBox4.Text = sqlConfigRead.dt.Rows[0].Field<string>("MinCert");
                textBox5.Text = sqlConfigRead.dt.Rows[0].Field<string>("Name");
                textBox6.Text = sqlConfigRead.dt.Rows[0].Field<string>("Description");
                textBox7.Text = sqlConfigRead.dt.Rows[0].Field<string>("DriveBoat");
                textBox8.Text = sqlConfigRead.dt.Rows[0].Field<string>("DIVEMASTERNAME");
                textBox9.Text = sqlConfigRead.dt.Rows[0].Field<string>("SKIPPERNAME");
                textBox10.Text = Convert.ToString(sqlConfigRead.dt.Rows[0].Field<Decimal>("BaseCost"));
                textBox11.Text = Convert.ToString(sqlConfigRead.dt.Rows[0].Field<Decimal>("GearHire"));
                maxCapacity = sqlConfigRead.dt.Rows[0].Field<int>("CAPACITY");
                pictureBox2.Image = Image.FromFile(@"Img\" + sqlConfigRead.dt.Rows[0].Field<string>("DiveSiteID") + ".jpg");

                pictureBox2.Refresh();

            }

        }
        private void GetBookingDiverSite(string key)
        {
            sql = "Select b.CustomerID, CONCAT(c.fname,' ', c.lname) AS CustName,c.CertificationLevel,c.Phone,b.GearHireRequired from bookings b inner join customers c on b.CustomerID=c.Customer_ID where b.DiveCharterID='" + key + "' order by b.CustomerID asc";
            sqlConfigRead.Load_DTG(sql, dataGridViewBooking);

            dataGridViewBooking.RowHeadersVisible = false;
            dataGridViewBooking.Columns[0].HeaderText = "CustId";
            dataGridViewBooking.Columns[1].HeaderText = "Name";
            dataGridViewBooking.Columns[2].HeaderText = "Cert";
            dataGridViewBooking.Columns[3].HeaderText = "Phone";
            dataGridViewBooking.Columns[4].HeaderText = "Get Hire Y|N";


            dataGridViewBooking.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewBooking.MultiSelect = false;
            dataGridViewBooking.EnableHeadersVisualStyles = false;

            dataGridViewBooking.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridViewBooking.ColumnHeadersDefaultCellStyle.BackColor = Color.Black;

            CapacityCheck();

        }
        private void GetAvailableCustomer(string key)
        {
            sql = "Select distinct b.CustomerID, CONCAT(c.fname, ' ', c.lname) AS CustName, c.CertificationLevel from bookings b inner join customers c on b.CustomerID=c.Customer_ID where b.CustomerID  NOT IN (Select CustomerID from bookings where DiveCharterID='" + key + "') order by b.CustomerID asc";
            sqlConfigRead.Load_DTG(sql, dataGridViewAvailable);

            dataGridViewAvailable.RowHeadersVisible = false;
            dataGridViewAvailable.Columns[0].HeaderText = "CustId";
            dataGridViewAvailable.Columns[1].HeaderText = "Name";
            dataGridViewAvailable.Columns[2].HeaderText = "Cert";


            dataGridViewAvailable.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewAvailable.MultiSelect = false;
            dataGridViewAvailable.EnableHeadersVisualStyles = false;
            dataGridViewAvailable.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridViewAvailable.ColumnHeadersDefaultCellStyle.BackColor = Color.Black;



        }




        private void comboBoxSelect_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBoxSelect.SelectedValue != null)
                {
                    string strId = comboBoxSelect.SelectedValue.ToString();
                    if (strId != "System.Data.DataRowView")
                    {
                        LoadDiverSite(strId);
                        GetBookingDiverSite(strId);
                        GetAvailableCustomer(strId);
                    }

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //int rowIndex = dataGridViewBooking.SelectedRows[0].Index;
            // DataGridViewRow selectedRow = dataGridViewBooking.Rows[rowIndex];
            // string id = selectedRow.Cells[0].Value.ToString();
            string cusid = dataGridViewBooking.CurrentRow.Cells[0].Value.ToString();
            CancelBooking(cusid);

            //MessageBox.Show(id);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //int rowIndex = dataGridViewAvailable.SelectedRows[0].Index;
            // DataGridViewRow selectedRow = dataGridViewAvailable.Rows[rowIndex];
            // string id = selectedRow.Cells[0].Value.ToString();
            if (capacityleft > 0)
            {
                string cusid = dataGridViewAvailable.CurrentRow.Cells[0].Value.ToString();
                AddBooking(cusid);
            }
            else
            {
                MessageBox.Show("No Booking Please", "Dive charter Capacity", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }



        }
        private void AddBooking(string cusId)
        {
            //sql="select max(counter)as counterNo from bookings";
            sql = " SELECT (counter) as counterNo FROM bookings ORDER BY counter DESC LIMIT 1";
            sqlConfigRead.ExecuteQuery(sql);
            int result = 1 + sqlConfigRead.dt.Rows[0].Field<int>("counterNo");

            char getHire = Convert.ToChar(comboBox2.SelectedValue.ToString());
            string divecharterId = comboBoxSelect.SelectedValue.ToString();
            if (result > 0)
            {
                String bookingid = "B0" + Convert.ToString(result);

                DialogResult option = MessageBox.Show("You are about to create a new booking -'" + bookingid + "' for Cust ID:'" + cusId + "' Do you want to continue?", "New Booking", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                // 0=yes, 1=no,
                if (option == DialogResult.Yes)
                {

                    sql = "INSERT INTO bookings(Booking_ID,DiveCharterID,CustomerID,GearHireRequired,Counter) VALUES('" + bookingid + "','" + divecharterId + "','" + cusId + "','" + getHire + "'," + result + ")";
                    //MessageBox.Show(sql);
                    sqlW.Execute_Query(sql);
                    if (sqlW.result > 0)
                    {
                        MessageBox.Show("create new booking table ");
                    }

                }

            }

            GetBookingDiverSite(divecharterId);
            GetAvailableCustomer(divecharterId);
            comboBox2.SelectedIndex = 0;

            // MessageBox.Show(result.ToString());
        }
        private void CancelBooking(string cusId)
        {
            string divecharterId = comboBoxSelect.SelectedValue.ToString();
            DialogResult option = MessageBox.Show("You are about to cancel a booking for Cust ID: " + cusId + "Do you wish to continue?", "Booking Cancellation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (option == DialogResult.Yes)
            {
                sql = "Delete from bookings where CustomerID='" + cusId + "' and DiveCharterID='" + divecharterId + "'";
                // MessageBox.Show(sql);

                sqlW.Execute_Query(sql);
                if (sqlW.result > 0)
                {
                    MessageBox.Show("Delete diver charter booking from table ");
                }

            }

            GetBookingDiverSite(divecharterId);
            GetAvailableCustomer(divecharterId);
            comboBox2.SelectedIndex = 0;

        }
        private void LoadCustomer(string searchType)
        {
            if (searchType.Equals("CustomerID"))
            {
                sql = "Select Customer_ID,FName,LName,DateOfBirth,Phone, Email,CertificationLevel,CertificationNumber,CertificationDate,TrainingAgency from customers where Customer_ID like '" + txtSearchCusId.Text + "%' order by Customer_ID asc";
            }
            else if (searchType.Equals("First_Name"))
            {
                sql = "Select Customer_ID,FName,LName,DateOfBirth,Phone, Email,CertificationLevel,CertificationNumber,CertificationDate,TrainingAgency from customers where FName like '" + txtSearchFName.Text + "%' order by Customer_ID asc";
            }
            else if (searchType.Equals("Last_Name"))
            {
                sql = "Select Customer_ID,FName,LName,DateOfBirth,Phone, Email,CertificationLevel,CertificationNumber,CertificationDate,TrainingAgency from customers where LName like '" + txtSearchLName.Text + "%' order by Customer_ID asc";
            }
            else
            {
                sql = "Select Customer_ID,FName,LName,DateOfBirth,Phone, Email,CertificationLevel,CertificationNumber,CertificationDate,TrainingAgency from customers order by customer_ID";
            }

            sqlConfigRead.Load_DTG(sql, dataGridViewCustomer);

            DefaultDataGridView();

            txtSearchResult.Text = dataGridViewCustomer.Rows.Count.ToString() + " record(s) found.";
        }

        private void btnSeachbyCustomerId_Click(object sender, EventArgs e)
        {
            txtSearchFName.Text = "";
            txtSearchLName.Text = "";
            LoadCustomer("CustomerID");
        }

        private void btnSeachbyFirstName_Click(object sender, EventArgs e)
        {
            txtSearchLName.Text = "";
            txtSearchCusId.Text = "";
            LoadCustomer("First_Name");
        }

        private void btnSeachbyLastName_Click(object sender, EventArgs e)
        {
            txtSearchCusId.Text = "";
            txtSearchFName.Text = "";
            LoadCustomer("Last_Name");
        }
        private void DefaultDataGridView()
        {
            dataGridViewCustomer.RowHeadersVisible = false;



            dataGridViewCustomer.Columns[0].HeaderText = "CustId";
            dataGridViewCustomer.Columns[1].HeaderText = "FName";
            dataGridViewCustomer.Columns[2].HeaderText = "LName";
            dataGridViewCustomer.Columns[3].HeaderText = "DOB";
            dataGridViewCustomer.Columns[4].HeaderText = "Phone";
            dataGridViewCustomer.Columns[5].HeaderText = "Email";
            dataGridViewCustomer.Columns[6].HeaderText = "Cert";
            dataGridViewCustomer.Columns[7].HeaderText = "Centnbr";
            dataGridViewCustomer.Columns[8].HeaderText = "CertDate";
            dataGridViewCustomer.Columns[9].HeaderText = "Agency";


            dataGridViewCustomer.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewCustomer.MultiSelect = false;
            dataGridViewCustomer.EnableHeadersVisualStyles = false;
            dataGridViewCustomer.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridViewCustomer.ColumnHeadersDefaultCellStyle.BackColor = Color.Black;

            //****************************************************
            //**********Datagridview column data format**********
            dataGridViewCustomer.Columns[3].DefaultCellStyle.Format = "dd MMM yyyy";
            dataGridViewCustomer.Columns[8].DefaultCellStyle.Format = "dd MMM yyyy";
        }
        private string strDateFormat(string strDate)
        {
            string stdate;
            DateTime datef;
            datef = Convert.ToDateTime(strDate);
            stdate = datef.ToString("dd MMM yyyy");
            return stdate;
        }

        private void dataGridViewCustomer_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

            textBox15.Text = dataGridViewCustomer.CurrentRow.Cells[0].Value.ToString();
            textBox16.Text = dataGridViewCustomer.CurrentRow.Cells[1].Value.ToString();
            textBox17.Text = dataGridViewCustomer.CurrentRow.Cells[2].Value.ToString();
            textBox18.Text = strDateFormat(dataGridViewCustomer.CurrentRow.Cells[3].Value.ToString());

            textBox19.Text = dataGridViewCustomer.CurrentRow.Cells[4].Value.ToString();
            textBox20.Text = dataGridViewCustomer.CurrentRow.Cells[5].Value.ToString();

            comboBox1.SelectedValue = dataGridViewCustomer.CurrentRow.Cells[6].Value;

            textBox21.Text = dataGridViewCustomer.CurrentRow.Cells[7].Value.ToString();

            textBox22.Text = strDateFormat(dataGridViewCustomer.CurrentRow.Cells[8].Value.ToString());
            comboBox3.SelectedValue = dataGridViewCustomer.CurrentRow.Cells[9].Value;
            //textBox16.Text = dataGridViewCustomer.CurrentRow.Cells[9].Value.ToString();

            //string stdate = datef.ToString("dd MMMM yyyy");
            //MessageBox.Show(stdate);             
        }
        private string stringToDate(string strDate)
        {
            DateTime dtDateset = DateTime.Parse(strDate);
            string strNewDate = dtDateset.ToString("yyyy-MM-dd");
            return strNewDate;
        }
        private bool validControl()
        {
            bool str = false;
            if (textBox15.Text == "" || textBox16.Text == "" || textBox17.Text == "" || textBox18.Text == "" || textBox19.Text == "" || textBox20.Text == "" || textBox21.Text == "" || textBox22.Text == "")
            {
                validCtr.emptymessage();
                str = false;
            }
            else
            {
                if (validCtr.isValidEmail(textBox20.Text) == true)
                {
                    str = true;
                }
                else
                    MessageBox.Show("There is Invalid email, \n Please enter valid Email!", "Invalid", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }


            return str;
        }
        private void btnUpdateCustomer_Click(object sender, EventArgs e)
        {
            if (validControl() == true)
            {
                //DateTime strDate = DateTime.Parse(stringToDate(textBox18.Text));
                sql = "UPDATE `customers` "
                        + "SET `FName` = '" + textBox16.Text + "',"
                        + "`LName` = '" + textBox17.Text + "',"
                        + "`DateOfBirth` ='" + stringToDate(textBox18.Text) + "',"
                        + "`Phone` = '" + textBox19.Text + "',"
                        + "`Email` = '" + textBox20.Text + "',"
                        + "`CertificationLevel` ='" + comboBox1.SelectedValue + "',"
                        + "`CertificationNumber` = '" + textBox21.Text + "',"
                        + "`CertificationDate` ='" + stringToDate(textBox22.Text) + "',"
                        + "`TrainingAgency` = '" + comboBox3.SelectedValue + "'"
                        + " WHERE `Customer_ID` = '" + textBox15.Text + "'";

                //MessageBox.Show(sql);
                //   SQLConfigWrite.Execute_Query(sql);
                //******object intance as reference for non static field or method or property******

                sqlW.Execute_Query(sql);
                if (sqlW.result > 0)
                {
                    MessageBox.Show("Customer details update successfully");
                }
                LoadCustomer("All");
                validCtr.ResetField(groupBox3);
            }

        }
        

    } 
}
