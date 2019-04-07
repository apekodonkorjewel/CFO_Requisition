using System;
using System.Windows.Forms;

namespace CFO_Requisition_System
{
    public partial class RequestFolder : Form
    {
        RequestFolderMethods requestFolderMethods = new RequestFolderMethods();
        private string dateTime;

        public RequestFolder()
        {
            InitializeComponent();

            requestFolderMethods.ComboBoxLoad(cbPurpose, new[] { "TAX CLEARANCE CERTIFICATE", "RECONCILIATION", "LEDGER CLEANING", 
                "TAX POSITION", "COMPREHENSIVE AUDIT", "COMPLIANCE STATUS", "INVESTIGATION", "OTHERS" });

            //Load head of TPS with head of TPS and supervisors of TPS sub-units
            cbHeadOfTPS.Items.Clear();
            requestFolderMethods.LoadComboBoxByPosition("HEAD", "TPS", cbHeadOfTPS);
            requestFolderMethods.LoadComboBoxByPosition("SUPERVISOR", "TPS", cbHeadOfTPS);

            //Load authorizing officer with office manager and all unit heads
            cbAuthorizingOfficer.Items.Clear();
            requestFolderMethods.LoadComboBoxByPosition("OFFICE MANAGER", "", cbAuthorizingOfficer);
            requestFolderMethods.LoadComboBoxByPosition("HEAD", "", cbAuthorizingOfficer);
        }

        public void RequestFolder_Load(object sender, EventArgs e)
        {
            requestFolderMethods.LoadFolderDetails(lvFolderList);
            requestFolderMethods.LoadFormDetails(txtSerialNumber, txtRequestCode, txtIssuedBy);

            chkMerged.AutoCheck = false;
            rbName.Checked = true;
            txtSearch.Enabled = true;

            //Clear trade name textbox and all comboboxes except purpose
            TextBox[] textBoxes = {txtTradersName, txtTin};
            RadioButton[] radioButtons = {rbAudit, rbCedm, rbOthers};
            ComboBox[] comboBoxes = {cbAuthorizingOfficer, cbReceivingOfficer, cbHeadOfTPS};
            requestFolderMethods.Clear(textBoxes, chkMerged, radioButtons, comboBoxes);

            dateTime = DateTime.Today.ToString("d");
        }

        private void txtSerialNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = Char.ToUpper(e.KeyChar);
        }

        private void txtRequestCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = Char.ToUpper(e.KeyChar);
        }

        private void txtTradersName_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = Char.ToUpper(e.KeyChar);
        }

        private void txtIssuedBy_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = Char.ToUpper(e.KeyChar);
        }

        public void btnClearForm_Click(object sender, EventArgs e)
        {
            //Clear textboxes, checkboxes, radiobuttons and comboboxes
            TextBox[] textBoxes = {txtSerialNumber, txtRequestCode, txtTradersName, txtIssuedBy, txtSearch, txtTin};
            RadioButton[] radioButtons = {rbMerged, rbUnmerged, rbName, rbAudit, rbCedm, rbOthers};
            ComboBox[] comboBoxes = {cbPurpose, cbAuthorizingOfficer, cbCollectedBy, cbHeadOfTPS, cbReceivingOfficer};

            requestFolderMethods.Clear(textBoxes, chkMerged,radioButtons, comboBoxes);

            //Reload the form
            RequestFolder_Load(sender, e);
        }

        private void lvFolderList_DoubleClick(object sender, EventArgs e)
        {
            requestFolderMethods.FillUIControls(lvFolderList, txtTradersName, txtTin, chkMerged);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            //Search by Name
            if (rbName.Checked)
                requestFolderMethods.SearchFolderByName(lvFolderList, txtSearch);

            //Search by TIN
            else if(rbTin.Checked)
                requestFolderMethods.SearchFolderByTin(lvFolderList, txtSearch);

            //Clear the textboxes
            txtTradersName.Clear();
        }

        private void rbMerged_CheckedChanged(object sender, EventArgs e)
        {
            if (rbMerged.Checked)
            {
                //Clear the other radio buttons
                rbName.Checked = false;
                rbUnmerged.Checked = false;

                txtSearch.Enabled = false;
                txtSearch.Clear();

                //Clear the textboxes
                txtTradersName.Clear();

                //Search by Unavailability
                requestFolderMethods.SearchFolderByMerged(lvFolderList);

                rbMerged.Checked = true;
            }
        }

        private void rbUnmerged_CheckedChanged(object sender, EventArgs e)
        {
            if (rbUnmerged.Checked)
            {
                //Clear the other radio buttons
                rbName.Checked = false;
                rbMerged.Checked = false;

                txtSearch.Enabled = false;
                txtSearch.Clear();

                //Clear the textboxes
                txtTradersName.Clear();

                //Search by Unavailability
                requestFolderMethods.SearchFolderByUnmerged(lvFolderList);

                rbUnmerged.Checked = true;
            }
        }

        private void rbName_CheckedChanged(object sender, EventArgs e)
        {
            //Enable the search box
            txtSearch.Enabled = true;
        }

        private void rbTin_CheckedChanged(object sender, EventArgs e)
        {
            //Enable the search box
            txtSearch.Enabled = true;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            requestFolderMethods.LoadFolderDetails(lvFolderList);

            btnClearForm_Click(sender, e);
        }

        private void btnRequest_Click(object sender, EventArgs e)
        {
            //Declare variables
            string mergedStatus = "UNMERGED";
            if (chkMerged.Checked)
            {
                mergedStatus = "MERGED";
            }

            //Group controls
            TextBox[] textBoxes = { txtSerialNumber, txtRequestCode, txtTradersName, txtIssuedBy};
            RadioButton[] radioButtons = {rbAudit, rbCedm, rbOthers};
            ComboBox[] comboBoxes = {cbPurpose, cbCollectedBy, cbAuthorizingOfficer, cbReceivingOfficer, cbHeadOfTPS};

            //Check if all details have been provided
            bool isEmpty = requestFolderMethods.IsEmpty(textBoxes, comboBoxes, radioButtons);

            if (isEmpty)
            {
                MessageBox.Show("Make sure you complete all fields");
            }

            else
            {
                //Confirm transaction
                DialogResult dialogResult = MessageBox.Show("Are you sure you want to request " + txtTradersName.Text +" folder?", 
                    "Information",MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (dialogResult == DialogResult.Yes)
                {
                    //Perform transaction (request folder)
                    requestFolderMethods.RequestFolder(txtTin.Text, txtSerialNumber.Text, txtRequestCode.Text, dateTime, 
                        txtTradersName.Text, mergedStatus, txtIssuedBy.Text, cbCollectedBy.Text, cbAuthorizingOfficer.Text,
                        cbReceivingOfficer.Text, cbHeadOfTPS.Text, radioButtons, cbPurpose.Text, lvFolderList);
                    
                    //Clear the screen
                    btnClearForm_Click(sender, e);
                }
            }
        }
            
        private void rbAudit_CheckedChanged(object sender, EventArgs e)
        {
            cbPurpose.SelectedIndex = -1;
            cbCollectedBy.SelectedIndex = -1;

            if (rbAudit.Checked)
            {
                rbCedm.Checked = false;
                rbOthers.Checked = false;

                //Load collected by combo box with only names of staff in Audit unit excluding interns and national service personnel
                cbCollectedBy.Items.Clear();
                requestFolderMethods.LoadComboBoxByUnit("AUDIT", cbCollectedBy);
                
                //Load receiving officer with all unit heads, supervisors of particular unit
                cbReceivingOfficer.Items.Clear();
                requestFolderMethods.LoadComboBoxByPosition("HEAD", rbAudit.Text, cbReceivingOfficer);
                requestFolderMethods.LoadComboBoxByPosition("SUPERVISOR", rbAudit.Text, cbReceivingOfficer);
            }
        }

        private void rbCedm_CheckedChanged(object sender, EventArgs e)
        {
            cbPurpose.SelectedIndex = -1;
            cbCollectedBy.SelectedIndex = -1;

            if (rbCedm.Checked)
            {
                rbAudit.Checked = false;
                rbOthers.Checked = false;

                //Load collected by combo box with only names of staff in CEDM unit excluding interns and national service personnel
                cbCollectedBy.Items.Clear();
                requestFolderMethods.LoadComboBoxByUnit("CEDM", cbCollectedBy);

                //Load receiving officer with all unit heads, supervisors of particular unit
                cbReceivingOfficer.Items.Clear();
                requestFolderMethods.LoadComboBoxByPosition("HEAD", rbCedm.Text, cbReceivingOfficer);
                requestFolderMethods.LoadComboBoxByPosition("SUPERVISOR", rbCedm.Text, cbReceivingOfficer);
            }
        }

        private void rbOthers_CheckedChanged(object sender, EventArgs e)
        {
            cbPurpose.SelectedIndex = -1;
            cbCollectedBy.SelectedIndex = -1;

            if (rbOthers.Checked)
            {
                rbCedm.Checked = false;
                rbAudit.Checked = false;

                //Load collected by combo box with only names of staff in OTHER unit (TPS) excluding interns and national service personnel
                cbCollectedBy.Items.Clear();
                requestFolderMethods.LoadComboBoxByUnitExceptCFO("TPS", "CENTRAL FILING OFFICE", cbCollectedBy);

                //Load receiving officer with all unit heads, supervisors of particular unit
                cbReceivingOfficer.Items.Clear();
                requestFolderMethods.LoadComboBoxByPosition("HEAD", rbOthers.Text, cbReceivingOfficer);
                requestFolderMethods.LoadComboBoxByPosition("SUPERVISOR", rbOthers.Text, cbReceivingOfficer);
            }
        }

        private void cbPurpose_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }


    }
}
