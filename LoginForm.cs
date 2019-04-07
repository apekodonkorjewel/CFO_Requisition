using System;
using System.Windows.Forms;

namespace CFO_Requisition_System
{

    public partial class LoginForm : Form
    {
        private LoginMethods loginMethods = new LoginMethods();
        MainForm mainForm = new MainForm();

        public LoginForm()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            //Check if user has logged out
            if (mainForm.Visible)       //user has not logged out
            {
                MessageBox.Show("Log out of the system first");
            }

            else
            {
                //Test if login credentials are correct
                bool isCorrect = loginMethods.IsCorrect(txtUsername.Text, txtPassword.Text);

                //If login credentials are correct, allow user to login and save details in log file
                if (isCorrect)
                {
                    //Allow user to login...Display the main form
                    mainForm.Show();

                    //Insert Administrator's details into Officer table
                    loginMethods.InsertAdministrator("ADMIN", "ADMIN", "admin", "admin2017", "ADMIN", "ADMINISTRATOR", "0547892015");

                    //Save Officer's details into the log file
                    loginMethods.InsertLoginFile(txtPassword.Text, txtUsername.Text);

                    //Clear the textboxes
                    btnClear_Click(sender, e);
                }

                else
                {
                    //If login credentials are incorrect, caution user
                    MessageBox.Show("Incorrect Username or Password");
                }
            }
            
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            //Group all textboxes on the login form into an array
            TextBox[] textBoxes = {txtUsername, txtPassword};

            //Clear the textboxes using the Clear method
            loginMethods.Clear(textBoxes);
        }

        private void txtUsername_KeyDown(object sender, KeyEventArgs e)
        {
            //If user has not logged out, disable login button
            if (mainForm.Visible)
            {
                btnLogin.Enabled = false;
            }

            else
            {
                btnLogin.Enabled = true;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            //Check if user has logged out
            if (mainForm.Visible)       //user has not logged out
            {
                MessageBox.Show("Log out of the system first");
            }

            else
            {
                //Request for confirmation of command (Are you sure you want to quit?)
                DialogResult dialogResult = MessageBox.Show("Are you sure you want to exit the program?", "",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                //If response is positive, close the login form
                if (dialogResult == DialogResult.Yes)
                {
                    this.Close();
                }
            }
        }

        private void txtUsername_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = Char.ToLower(e.KeyChar);
        }
    }
}
