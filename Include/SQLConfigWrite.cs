using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace BigBlue.SQL
{
    class SQLConfigWrite
    {
        private MySqlConnection Con = new MySqlConnection("datasource=localhost;port=3306; database=bigblue_divecharters;username=root;password=indra");
        private MySqlCommand cmd;
        public int result
        {
            get;
            set;

        }
        public void Execute_Query(string sql)
        {
            try
            {
                if (Con.State != ConnectionState.Open)
                {
                    Con.Open();
                }

                cmd = new MySqlCommand(sql, Con);
                cmd.Connection = Con;

                result = cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
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
