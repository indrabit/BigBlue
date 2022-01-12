using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;
using System.Windows.Forms;

namespace Inventory_Read.Include
{

    class SQLConfig_Read
    {
        
        
        private MySqlConnection Con = new MySqlConnection("datasource=localhost;port=3306; database=bigblue_divecharters;username=root;password=indra");
        private MySqlCommand cmd;
        private MySqlDataAdapter da;
        public DataTable dt;
   
        public void ExecuteQuery(string sql)
        {
            try
            {
                Con.Open();
                cmd = new MySqlCommand();
                da = new MySqlDataAdapter();
                dt = new DataTable();

                cmd.Connection = Con;
                cmd.CommandText = sql;
                da.SelectCommand = cmd;
                da.Fill(dt);
            }
            catch(Exception ex)
            {
              MessageBox.Show(ex.Message);
            }
            finally
            {
                Con.Close();
            }
        }
        public void combo(string sql, ComboBox cbo)
        {
            try
            {
                if (Con.State != ConnectionState.Open)
                {
                    Con.Open();
                }
                cmd = new MySqlCommand();
                da = new MySqlDataAdapter();
                dt = new DataTable();
                cmd.Connection = Con;
                cmd.CommandText = sql;
                da.SelectCommand = cmd;
                da.Fill(dt);
                cbo.DataSource = dt;
                cbo.ValueMember = dt.Columns[0].ColumnName;
                cbo.DisplayMember = dt.Columns[1].ColumnName;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                Con.Close();
            }
        }
        
       
    }
}
