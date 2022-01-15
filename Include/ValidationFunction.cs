using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace BigBlue.Validation
{
    class ValidationFunction
    {
        //***************Clear field or Reset field******************
        public void ResetField(Control container)
        {
            try
            {
                //'for each txt as control in this(object).control
                foreach (Control C in container.Controls)
                {
                    if (C is TextBox)
                    {
                        C.Text = "";
                    }
                    else if (C is ComboBox)
                    {
                        ((ComboBox)C).SelectedIndex = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //initialize the validating method
        /*
        static Regex Valid_Name = StringOnly();
        static Regex Valid_Contact = NumbersOnly();
        static Regex Valid_Password = ValidPassword();
        static Regex Valid_Email = Email_Address();*/
        //*********Email Validation
        public bool isValidEmail(string inputEmail)
        {
            string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                  @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                  @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            Regex re = new Regex(strRegex);
            if (re.IsMatch(inputEmail))
                return (true);
            else
                return (false);
        }
        //**************String validation**************
        public static Regex StringOnly()
        {
            string StringAndNumber_Pattern = "^[a-zA-Z]";

            return new Regex(StringAndNumber_Pattern, RegexOptions.IgnoreCase);
        }
        //***************Number Validation*************
        public static Regex NumbersOnly()
        {
            string StringAndNumber_Pattern = "^[0-9]*$";

            return new Regex(StringAndNumber_Pattern, RegexOptions.IgnoreCase);
        }
        //****************Password validation*************
        public static Regex ValidPassword()
        {
            string Password_Pattern = "(?!^[0-9]*$)(?!^[a-zA-Z]*$)^([a-zA-Z0-9]{8,15})$";

            return new Regex(Password_Pattern, RegexOptions.IgnoreCase);
        }
        public void emptymessage()
        {
            MessageBox.Show("There are empty fields left that must be filled up!", "Invalid", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }
    }
}
