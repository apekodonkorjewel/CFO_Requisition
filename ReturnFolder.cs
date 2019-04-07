using System;
using System.Windows.Forms;

namespace CFO_Requisition_System
{
    public partial class ReturnFolder : Form
    {
        ReturnFolderMethods returnFolderMethods = new ReturnFolderMethods();
        private DateTime dateTime;

        public ReturnFolder()
        {
            InitializeComponent();

            returnFolderMethods.LoadFolderDetails(lvFolderList);
            cbReceivingOfficer.Items.Clear();
            returnFolderMethods.LoadComboBoxByUnit("CENTRAL", cbReceivingOfficer);

            rbName.Checked = true;
        }

        public void ReturnFolder_Load(object sender, EventArgs e)
        {
            dateTime = dtpDateReturned.Value;

            returnFolderMethods.LoadFolderDetails(lvFolderList);
            returnFolderMethods.LoadFormDetails(txtReturnCode, txtReceivedBy);

            cbReturningOfficer.Items.Clear();

            rbAudit.AutoCheck = false;
            rbCedm.AutoCheck = false;
            rbOthers.AutoCheck = false;

            rbName.Checked = true;
        }

        private void dtpDateReturned_ValueChanged(object sender, EventArgs e)
        {
            dtpDateReturned.Value = dateTime;
        }

        private void lvFolderList_DoubleClick(object sender, EventArgs e)
        {
            RadioButton[] radioButtons = {rbAudit, rbCedm, rbOthers};

            //Clear the radio buttons
            rbAudit.Checked = false;
            rbCedm.Checked = false;
            rbOthers.Checked = false;

            returnFolderMethods.FillUIControls(lvFolderList, txtFolderName, txtReturnedBy, radioButtons);

            cbReturningOfficer.Items.Clear();
            if (rbAudit.Checked)
            {
                returnFolderMethods.LoadComboBoxByPosition("HEAD", rbAudit.Text, cbReturningOfficer);
                returnFolderMethods.LoadComboBoxByPosition("SUPERVISOR", rbAudit.Text, cbReturningOfficer);
            }

            else if (rbCedm.Checked)
            {
                returnFolderMethods.LoadComboBoxByPosition("HEAD", rbCedm.Text, cbReturningOfficer);
                returnFolderMethods.LoadComboBoxByPosition("SUPERVISOR", rbCedm.Text, cbReturningOfficer);
            }

            else if (rbOthers.Checked)
            {
                returnFolderMethods.LoadComboBoxByPosition("HEAD", "TPS", cbReturningOfficer);
                returnFolderMethods.LoadComboBoxByPosition("SUPERVISOR", "TPS", cbReturningOfficer);
            }
        }

        public void btnClearForm_Click(object sender, EventArgs e)
        {
            txtFolderName.Clear();
            txtSearch.Clear();
            cbReceivingOfficer.SelectedIndex = -1;
            cbReturningOfficer.SelectedIndex = -1;
            txtReturnedBy.Clear();

            //Clear the radio buttons
            rbAudit.Checked = false;
            rbCedm.Checked = false;
            rbOthers.Checked = false;

            //Refresh
            ReturnFolder_Load(sender, e);
        }

        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = Char.ToUpper(e.KeyChar);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            returnFolderMethods.LoadFolderDetails(lvFolderList);

            //Uncheck the unit radiobuttons
            rbAudit.Checked = false;
            rbCedm.Checked = false;
            rbOthers.Checked = false;

            btnClearForm_Click(sender, e);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            //search by name
            if(rbName.Checked)
                returnFolderMethods.SearchFolderByName(lvFolderList, txtSearch);

            //search by TIN
            else if(rbTin.Checked)
                returnFolderMethods.SearchFolderByTin(lvFolderList, txtSearch);

            //Clear the textboxes and radio buttons
            txtFolderName.Clear();
            rbAudit.Checked = false;
            rbCedm.Checked = false;
            rbOthers.Checked = false;
            cbReceivingOfficer.SelectedIndex = -1;
            cbReturningOfficer.SelectedIndex = -1;
            cbReturningOfficer.Items.Clear();
            
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            //Group controls
            TextBox[] textBoxes = { txtFolderName, txtReceivedBy };
            ComboBox[] comboBoxes = {cbReceivingOfficer, cbReturningOfficer};

            //Check if all details have been provided
            bool isEmpty = returnFolderMethods.IsEmpty(textBoxes, comboBoxes);

            if (isEmpty)
            {
                MessageBox.Show("Make sure you complete all fields");
            }

            else
            {
                //Confirm transaction
                DialogResult dialogResult = MessageBox.Show("Are you sure you want to return " + txtFolderName.Text + " folder?",
                    "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (dialogResult == DialogResult.Yes)
                {
                    //Perform transaction (return folder)
                    returnFolderMethods.ReturnFolder(txtReturnCode.Text, dtpDateReturned.Text, txtReceivedBy.Text, 
                        txtFolderName.Text, cbReturningOfficer.Text, cbReceivingOfficer.Text, lvFolderList);

                    //Clear the screen
                    btnClearForm_Click(sender, e);
                }
            }
        }

        private void cbReturningOfficer_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void txtReturnCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }
    }
}
