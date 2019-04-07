using System;
using System.Windows.Forms;

namespace CFO_Requisition_System
{
    public partial class ModDelOfficer : Form
    {
        ModDelOfficerMethods modDelOfficerMethods = new ModDelOfficerMethods();
        private string officerID = "";
        private TextBox[] textBoxes;
        private RadioButton[] radioButtons;
        private ComboBox[] comboBoxes;

        public ModDelOfficer()
        {
            InitializeComponent();

            textBoxes = new[] { txtSurname, txtOtherNames, txtUsername, txtPassword, txtConfirmPassword, txtContact };
            radioButtons = new[] { rbTPS, rbCEDM, rbAudit };
            comboBoxes = new[] { cbPosition, cbUnit };
            modDelOfficerMethods.ComboBoxLoad(cbPosition, new[] { "HEAD", "SUPERVISOR", "STAFF", "NATIONAL SERVICE PERSONNEL", "INTERN" });

            cbUnit.Enabled = false;
            cbUnit.SelectedIndex = -1;
            txtPassword.Enabled = false;
            txtUsername.Enabled = false;
            txtConfirmPassword.Enabled = false;
        }

        public void ModDelOfficer_Load(object sender, EventArgs e)
        {
            modDelOfficerMethods.LoadOfficerDetails(lvOfficer);
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

        public void btnClear_Click(object sender, EventArgs e)
        {
            //Group all textboxes on the form into one array
            TextBox[] txtBoxes = { txtSurname, txtOtherNames, txtUsername, txtPassword, txtConfirmPassword, txtContact };
            RadioButton[] radioButtons = { rbTPS, rbCEDM, rbAudit };
            ComboBox[] comboBoxes = { cbPosition, cbUnit };

            //Clear the textboxes using the Clear method
            modDelOfficerMethods.Clear(txtBoxes, comboBoxes, radioButtons);
            
            //Reset the officerID
            officerID = "";
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            //Ask the user for authentication using a dialog box
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to continue?", "Information",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (dialogResult == DialogResult.Yes && officerID.Length != 0)
            {
                //Delete the officer details permanently
                modDelOfficerMethods.DeleteOfficer(officerID);

                //Clear the textboxes and the checkbox
                btnClear_Click(sender, e);

                //Refresh the list view
                btnRefresh_Click(sender, e);
            }
            else
            {
                MessageBox.Show("Double-click record in the list to delete", "Information", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            //Group radio buttons
            radioButtons = new[] { rbTPS, rbCEDM, rbAudit };

            string unit = modDelOfficerMethods.Unit(cbUnit.Text, radioButtons);

            //Check if all details have been entered
            bool isEmpty = modDelOfficerMethods.IsEmpty(textBoxes, comboBoxes, radioButtons);

            //Ask the user for authentication using a dialog box
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to continue?", "Information",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (txtPassword.Text == txtConfirmPassword.Text)
            {
                if (dialogResult == DialogResult.Yes && officerID.Length != 0)
                {
                    //Make sure all details have been provided
                    if (isEmpty)
                    {
                        MessageBox.Show("Make sure you complete all fields");
                    }

                    else
                    {
                        if (cbUnit.Text == "CENTRAL FILING OFFICE" && rbTPS.Checked)
                        {
                            modDelOfficerMethods.ModifyCFOOfficer(txtSurname.Text, txtOtherNames.Text, txtContact.Text, unit, cbPosition.Text,
                            txtUsername.Text, txtPassword.Text, officerID);
                        }

                        else
                        {
                            modDelOfficerMethods.ModifyOtherOfficer(txtSurname.Text, txtOtherNames.Text, txtContact.Text, unit, cbPosition.Text, officerID);
                        }

                        //Refresh the list
                        ModDelOfficer_Load(sender, e);
                        btnClear_Click(sender, e);
                    }
                }

                else
                {
                    MessageBox.Show("Double-click a record to modify", "Information", MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
            }

            else
            {
                MessageBox.Show("Password Mismatch!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void lvOfficer_DoubleClick(object sender, EventArgs e)
        {
            radioButtons = new[]{rbTPS, rbCEDM, rbAudit};

            string admin = lvOfficer.SelectedItems[0].SubItems[3].Text;

            if (admin == "ADMINISTRATOR")
                MessageBox.Show("Administrator details cannot be modified or deleted");

            else
            {
                officerID = lvOfficer.SelectedItems[0].SubItems[0].Text;

                modDelOfficerMethods.FillUIControls(lvOfficer, radioButtons, txtSurname, txtOtherNames, txtContact, cbPosition, 
                    cbUnit, txtUsername, txtPassword, txtConfirmPassword);
            }
        }

        private void rbSurname_CheckedChanged(object sender, EventArgs e)
        {
            if (rbSurname.Checked)
            {
                rbOtherNames.Checked = false;
                rbPosition.Checked = false;
                rbUnit.Checked = false;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            //Search by Other names
            if (rbOtherNames.Checked)
                modDelOfficerMethods.SearchOfficerByOtherNames(lvOfficer, txtSearch);

            //Search by Surname
            else if (rbSurname.Checked)
                modDelOfficerMethods.SearchOfficerBySurname(lvOfficer, txtSearch);

            else if (rbPosition.Checked)
                modDelOfficerMethods.SearchOfficerByPosition(lvOfficer, txtSearch);

            else if (rbUnit.Checked)
                modDelOfficerMethods.SearchOfficerByUnit(lvOfficer, txtSearch);
        }

        private void rbOtherNames_CheckedChanged(object sender, EventArgs e)
        {
            if (rbOtherNames.Checked)
            {
                rbSurname.Checked = false;
                rbPosition.Checked = false;
                rbUnit.Checked = false;
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            lvOfficer.Clear();
            modDelOfficerMethods.LoadOfficerDetails(lvOfficer);

            btnClear_Click(sender, e);
        }

        private void cbPosition_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void rbPosition_CheckedChanged(object sender, EventArgs e)
        {
            if (rbPosition.Checked)
            {
                rbOtherNames.Checked = false;
                rbSurname.Checked = false;
                rbUnit.Checked = false;
            }
        }

        private void rbUnit_CheckedChanged(object sender, EventArgs e)
        {
            if (rbUnit.Checked)
            {
                rbOtherNames.Checked = false;
                rbSurname.Checked = false;
                rbPosition.Checked = false;
            }
        }

        private void rbTPS_CheckedChanged(object sender, EventArgs e)
        {
            if (rbTPS.Checked)
            {
                cbUnit.Text = "";
                cbPosition.Text = "";
                rbCEDM.Checked = false;
                rbAudit.Checked = false;

                //Enable the unit combo box
                cbUnit.Enabled = true;

                if (cbPosition.Text == "HEAD")
                    cbUnit.Enabled = false;
            }

            //Load sub-unit combo box
            cbUnit.Items.Clear();
            modDelOfficerMethods.ComboBoxLoad(cbUnit, new[] { "RETURN PROCESSING", "CLIENT SERVICES", "REVENUE COLLECTION/ACCOUNTING", "CENTRAL FILING OFFICE" });
        }

        private void rbCEDM_CheckedChanged(object sender, EventArgs e)
        {
            if (rbCEDM.Checked)
            {
                cbUnit.Text = "";
                cbPosition.Text = "";
                rbTPS.Checked = false;
                rbAudit.Checked = false;

                //Enable the unit combo box
                cbUnit.Enabled = true;

                if (cbPosition.Text == "HEAD")
                    cbUnit.Enabled = false;
            }

            //Load sub-unit combo
            cbUnit.Items.Clear();
            cbUnit.Text = "";
            modDelOfficerMethods.ComboBoxLoad(cbUnit, new[] { "COMPLIANCE", "ENFORCEMENT", "DEBT MANAGEMENT" });

            //Disable login credentials portion
            txtUsername.Enabled = false;
            txtPassword.Enabled = false;
            txtConfirmPassword.Enabled = false;
        }

        private void rbAudit_CheckedChanged(object sender, EventArgs e)
        {
            if (rbAudit.Checked)
            {
                cbPosition.Text = "";
                rbTPS.Checked = false;
                rbCEDM.Checked = false;
                comboBoxes = new[] { cbPosition };

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
                comboBoxes = new[] { cbPosition };
                textBoxes = new[] { txtSurname, txtOtherNames };
            }

            else
            {
                if (rbAudit.Checked)
                {
                    cbUnit.Enabled = false;
                    comboBoxes = new[] { cbPosition };
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

                textBoxes = new[] { txtSurname, txtOtherNames, txtUsername, txtPassword, txtConfirmPassword };
            }

            else
            {
                txtPassword.Enabled = false;
                txtUsername.Enabled = false;
                txtConfirmPassword.Enabled = false;

                textBoxes = new[] { txtSurname, txtOtherNames };
            }
        }

        private void txtContact_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(!Char.IsNumber(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}