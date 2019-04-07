using System;
using System.Windows.Forms;

namespace CFO_Requisition_System
{
    public partial class NewOfficer : Form
    {
        private NewOfficerMethods newOfficerMethods = new NewOfficerMethods();
        private TextBox[] textBoxes;
        private RadioButton[] radioButtons;
        private ComboBox[] comboBoxes;

        public NewOfficer()
        {
            InitializeComponent();
            textBoxes = new[] { txtSurname, txtOtherNames, txtUsername, txtPassword, txtConfirmPassword, txtContact };
            radioButtons = new [] {rbTPS, rbCEDM, rbAudit};
            comboBoxes = new [] {cbPosition, cbUnit};
            newOfficerMethods.ComboBoxLoad(cbPosition, new[] { "HEAD", "SUPERVISOR",  "STAFF", "NATIONAL SERVICE PERSONNEL", "INTERN" });
            
            cbUnit.Enabled = false;
            cbUnit.SelectedIndex = -1;
            txtPassword.Enabled = false;
            txtUsername.Enabled = false;
            txtConfirmPassword.Enabled = false;
        }

        public void btnClear_Click(object sender, EventArgs e)
        {
            //Group all textboxes on the form into one array
            TextBox[] txtBoxes = {txtSurname, txtOtherNames, txtUsername, txtPassword, txtConfirmPassword, txtContact};
            RadioButton[] radioButtons = {rbTPS, rbCEDM, rbAudit};
            ComboBox[] comboBoxes = {cbPosition, cbUnit};

            //Clear the textboxes using the Clear method
            newOfficerMethods.Clear(txtBoxes, comboBoxes, radioButtons);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //Group radio buttons
            radioButtons = new[]{rbTPS, rbCEDM, rbAudit, rbOther};

            string unit = newOfficerMethods.Unit(cbUnit.Text, radioButtons);

            //Check if all details have been entered
            bool isEmpty = newOfficerMethods.IsEmpty(textBoxes, comboBoxes, radioButtons);

            //Ask the user for authentication using a dialog box(Are you sure you want to continue?)
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to continue?", "Information",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (txtConfirmPassword.Text == txtPassword.Text)
            {
                if (dialogResult == DialogResult.Yes)
                {
                    //Save the Officer's details in the database
                    if (isEmpty && !rbOther.Checked)
                    {
                        MessageBox.Show("Make sure you complete all fields");
                    }

                    else
                    {
                        if (cbUnit.Text == "CENTRAL FILING OFFICE" && rbTPS.Checked)
                        {
                            newOfficerMethods.InsertCFOOfficer(txtSurname.Text, txtOtherNames.Text, txtUsername.Text,
                                txtPassword.Text, txtContact.Text, unit, cbPosition.Text);
                        }
                            
                        else
                        {
                            newOfficerMethods.InsertOtherOfficer(txtSurname.Text, txtOtherNames.Text, txtContact.Text, unit, cbPosition.Text);
                        }

                        //Refresh the list
                        NewOfficer_Load(sender, e);
                        btnClear_Click(sender, e);
                    }
                }
            }

            else
            {
                MessageBox.Show("Password Mismatch", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }
        
        public void NewOfficer_Load(object sender, EventArgs e)
        {
            newOfficerMethods.LoadOfficerDetails(lvOfficerList);
        }

        private void txtSurname_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = Char.ToUpper(e.KeyChar);
        }

        private void txtOtherNames_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = Char.ToUpper(e.KeyChar);
        }

        private void txtUsername_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = Char.ToLower(e.KeyChar);
        }

        private void txtContact_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Allow input of only numbers
            if(!Char.IsNumber(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void cbPosition_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void cbUnit_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void rbTPS_CheckedChanged(object sender, EventArgs e)
        {
            if (rbTPS.Checked)
            {
                cbUnit.Text = "";
                cbPosition.Text = "";
                rbCEDM.Checked = false;
                rbAudit.Checked = false;
                rbOther.Checked = false;

                //Enable the unit combo box
                cbUnit.Enabled = true;

                //Load sub-unit combo box
                cbUnit.Items.Clear();
                newOfficerMethods.ComboBoxLoad(cbPosition, new[] { "HEAD", "SUPERVISOR", "STAFF", "NATIONAL SERVICE PERSONNEL", "INTERN" });
                newOfficerMethods.ComboBoxLoad(cbUnit, new[] { "RETURN PROCESSING", "CLIENT SERVICES", "REVENUE COLLECTION/ACCOUNTING", "CENTRAL FILING OFFICE" });
            }
        }

        private void rbCEDM_CheckedChanged(object sender, EventArgs e)
        {
            if (rbCEDM.Checked)
            {
                cbUnit.Text = "";
                cbPosition.Text = "";
                rbTPS.Checked = false;
                rbAudit.Checked = false;
                rbOther.Checked = false;

                //Enable the unit combo box
                cbUnit.Enabled = true;

                //Load sub-unit combo
                cbUnit.Items.Clear();
                cbUnit.Text = "";
                newOfficerMethods.ComboBoxLoad(cbPosition, new[] { "HEAD", "SUPERVISOR", "STAFF", "NATIONAL SERVICE PERSONNEL", "INTERN" });
                newOfficerMethods.ComboBoxLoad(cbUnit, new[] { "COMPLIANCE", "ENFORCEMENT", "DEBT MANAGEMENT" });

                //Disable login credentials portion
                txtUsername.Enabled = false;
                txtPassword.Enabled = false;
                txtConfirmPassword.Enabled = false;
            }
            
        }

        private void rbAudit_CheckedChanged(object sender, EventArgs e)
        {
            if (rbAudit.Checked)
            {
                cbPosition.Text = "";
                rbTPS.Checked = false;
                rbCEDM.Checked = false;
                rbOther.Checked = false;
                comboBoxes = new[] {cbPosition};
                textBoxes = new[] { txtSurname, txtOtherNames };
                newOfficerMethods.ComboBoxLoad(cbPosition, new[] { "HEAD", "STAFF", "NATIONAL SERVICE PERSONNEL", "INTERN", "SUPERVISOR" });

                //Disable the unit combo box
                cbUnit.Enabled = false;
                cbUnit.SelectedIndex = -1;
            }
        }

        private void rbOther_CheckedChanged(object sender, EventArgs e)
        {
            if (rbOther.Checked)
            {
                cbPosition.Text = "";
                rbTPS.Checked = false;
                rbCEDM.Checked = false;
                rbAudit.Checked = false;

                comboBoxes = new[] { cbPosition };
                textBoxes = new[] { txtSurname, txtOtherNames };

                newOfficerMethods.ComboBoxLoad(cbPosition, new[] { "OFFICE MANAGER" });
                 
                //Disable the unit combo box
                cbUnit.Enabled = false;
                cbUnit.SelectedIndex = -1;
            }
        }

        private void cbPosition_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbPosition.Text == "HEAD")
            {
                cbUnit.Enabled = false;
                cbUnit.SelectedIndex = -1;
                comboBoxes = new[] {cbPosition};
                textBoxes = new[] {txtSurname, txtOtherNames};
            }

            else
            {
                if (rbAudit.Checked | rbOther.Checked)
                {
                    cbUnit.Enabled = false;
                    comboBoxes = new[] {cbPosition};
                    textBoxes = new[] {txtSurname, txtOtherNames};
                }

                else
                {
                    cbUnit.Enabled = true;
                    comboBoxes = new[] { cbPosition, cbUnit };
                }
            }
        }

        private void cbUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbUnit.Text == "CENTRAL FILING OFFICE")
            {
                txtPassword.Enabled = true;
                txtUsername.Enabled = true;
                txtConfirmPassword.Enabled = true;

                textBoxes = new[] {txtSurname, txtOtherNames, txtUsername, txtPassword, txtConfirmPassword};
            }

            else
            {
                txtPassword.Enabled = false;
                txtUsername.Enabled = false;
                txtConfirmPassword.Enabled = false;

                textBoxes = new[] { txtSurname, txtOtherNames };
            }
        }


    }
}
