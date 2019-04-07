using System;
using System.Windows.Forms;

namespace CFO_Requisition_System
{
    public partial class ModDelFolder : Form
    {
        private ModDelFolderMethods modDelFolderMethods = new ModDelFolderMethods();
        private string folderID = " ";
        private string folderName = " ";

        public ModDelFolder()
        {
            InitializeComponent();
        }

        public void ModDelFolder_Load(object sender, EventArgs e)
        {
            modDelFolderMethods.LoadFolderDetails(lvFolderList);
            chkAvailStatus.Checked = true;
        }

        private void btnModifyFolder_Click(object sender, EventArgs e)
        {
            //Group all textboxes on the form into one array
            TextBox[] textBoxes = {txtSection, txtTradersName, txtLocation};

            //Check if all details have been entered
            bool isEmpty = modDelFolderMethods.IsEmpty(textBoxes);

            //Ask the user for authentication using a dialog box
            if (folderID.Length != 0)
            {
                DialogResult dialogResult =
                    MessageBox.Show("Are you sure you want to modify the " + folderName + " folder?", "Information",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (dialogResult == DialogResult.Yes)
                {
                    //Make sure all details have been provided
                    if (isEmpty)
                    {
                        MessageBox.Show("Make sure you complete all fields");
                    }

                    else
                    {
                        //Modify the folder details as specified
                        modDelFolderMethods.ModifyFolder(txtSection.Text, txtTin.Text, txtTradersName.Text, txtLocation.Text, chkAvailStatus, chkMergedStatus, folderID);

                        //Clear the textboxes and the checkboxes
                        btnClear_Click(sender, e);

                        //Refresh the list view
                        ModDelFolder_Load(sender, e);
                    }
                }
            }

            else
            {
                MessageBox.Show("Double-click a record to modify", "Information", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        private void btnDeleteFolder_Click(object sender, EventArgs e)
        {
            //Ask the user for authentication using a dialog box
            if (folderID.Length != 0)
            {
                DialogResult dialogResult =
                    MessageBox.Show("Are you sure you want to delete the " + folderName + " folder?", "Information",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (dialogResult == DialogResult.Yes && folderID.Length != 0)
                {
                    //Delete the folder details permanently
                    modDelFolderMethods.DeleteFolder(folderID);

                    //Clear the textboxes and the checkboxes
                    btnClear_Click(sender, e);

                    //Refresh the list view
                    ModDelFolder_Load(sender, e);
                }
            }

            else
            {
                MessageBox.Show("Double-click a record to delete", "Information", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        public void btnClear_Click(object sender, EventArgs e)
        {
            //Clear details of the form
            txtSection.Clear();
            txtTradersName.Clear();
            txtSearch.Clear();
            txtLocation.Clear();
            txtTin.Clear();

            txtSearch.Enabled = true;

            chkAvailStatus.Checked = false;
            chkMergedStatus.Checked = false;

            rbName.Checked = true;
            rbSection.Checked = false;
            rbAvailable.Checked = false;
            rbUnavailable.Checked = false;
            rbMerged.Checked = false;
            rbUnmerged.Checked = false;

            //Reset the folderID
            folderID = "";
        }

        private void lvFolderList_DoubleClick(object sender, EventArgs e)
        {
            folderID = lvFolderList.SelectedItems[0].SubItems[0].Text;
            folderName = lvFolderList.SelectedItems[0].SubItems[1].Text;

            modDelFolderMethods.FillUIControls(lvFolderList, txtTin, txtSection,  txtLocation, txtTradersName, chkAvailStatus, chkMergedStatus);
        }

        private void rbName_CheckedChanged(object sender, EventArgs e)
        {
            if (rbName.Checked)
            {
                //Clear the other radio buttons
                rbAvailable.Checked = false;
                rbSection.Checked = false;
                rbUnavailable.Checked = false;
                rbMerged.Checked = false;
                rbUnmerged.Checked = false;
                txtSearch.Enabled = true;

                //Clear the textboxes
                txtTradersName.Clear();
                txtSection.Clear();
                txtLocation.Clear();

                //Clear the checkboxes
                chkAvailStatus.Checked = false;
                chkMergedStatus.Checked = false;
            }
        }

        private void rbSection_CheckedChanged(object sender, EventArgs e)
        {
            if (rbSection.Checked)
            {
                //Clear the other radio buttons
                rbName.Checked = false;
                rbAvailable.Checked = false;
                rbUnavailable.Checked = false;
                rbMerged.Checked = false;
                rbUnmerged.Checked = false;
                txtSearch.Enabled = true;

                //Clear the textboxes
                txtTradersName.Clear();
                txtSection.Clear();
                txtLocation.Clear();

                //Clear the checkboxes
                chkAvailStatus.Checked = false;
                chkMergedStatus.Checked = false;
            }
        }

        private void rbAvailable_CheckedChanged(object sender, EventArgs e)
        {
            if (rbAvailable.Checked)
            {
                //Clear the other radio buttons
                rbName.Checked = false;
                rbSection.Checked = false;
                rbUnavailable.Checked = false;
                rbMerged.Checked = false;
                rbUnmerged.Checked = false;

                txtSearch.Enabled = false;
                txtSearch.Clear();

                //Clear the textboxes
                txtTradersName.Clear();
                txtSection.Clear();
                txtLocation.Clear();

                //Clear the checkboxes
                chkAvailStatus.Checked = false;
                chkMergedStatus.Checked = false;

                //Search by Unavailability
                modDelFolderMethods.SearchFolderByAvailability(lvFolderList);
            }
        }

        private void rbUnavailable_CheckedChanged(object sender, EventArgs e)
        {
            if (rbUnavailable.Checked)
            {
                //Clear the other radio buttons
                rbName.Checked = false;
                rbAvailable.Checked = false;
                rbSection.Checked = false;
                rbMerged.Checked = false;
                rbUnmerged.Checked = false;

                txtSearch.Enabled = false;
                txtSearch.Clear();

                //Clear the textboxes
                txtTradersName.Clear();
                txtSection.Clear();
                txtLocation.Clear();

                //Clear the checkboxes
                chkAvailStatus.Checked = false;
                chkMergedStatus.Checked = false;

                //Search by Unavailability
                modDelFolderMethods.SearchFolderByUnavailability(lvFolderList);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            //Search by Name
            if (rbName.Checked)
                modDelFolderMethods.SearchFolderByName(lvFolderList, txtSearch);

            //Search by Section
            else if (rbSection.Checked)
                modDelFolderMethods.SearchFolderBySection(lvFolderList, txtSearch);

            //Clear the textboxes
            btnClear_Click(sender, e);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            modDelFolderMethods.LoadFolderDetails(lvFolderList);

            btnClear_Click(sender, e);
        }

        private void rbMerged_CheckedChanged(object sender, EventArgs e)
        {
            if (rbMerged.Checked)
            {
                //Clear the other radio buttons
                rbName.Checked = false;
                rbSection.Checked = false;
                rbUnavailable.Checked = false;
                rbUnmerged.Checked = false;
                rbAvailable.Checked = false;

                txtSearch.Enabled = false;
                txtSearch.Clear();

                //Clear the textboxes
                txtTradersName.Clear();
                txtSection.Clear();
                txtLocation.Clear();

                //Clear the checkboxes
                chkAvailStatus.Checked = false;
                chkMergedStatus.Checked = false;

                //Search by Unavailability
                modDelFolderMethods.SearchFolderByMerged(lvFolderList);
            }
        }

        private void rbUnmerged_CheckedChanged(object sender, EventArgs e)
        {
            if (rbUnmerged.Checked)
            {
                //Clear the other radio buttons
                rbName.Checked = false;
                rbSection.Checked = false;
                rbUnavailable.Checked = false;
                rbMerged.Checked = false;
                rbAvailable.Checked = false;

                txtSearch.Enabled = false;
                txtSearch.Clear();

                //Clear the textboxes
                txtTradersName.Clear();
                txtSection.Clear();
                txtLocation.Clear();

                //Clear the checkboxes
                chkAvailStatus.Checked = false;
                chkMergedStatus.Checked = false;

                //Search by Unavailability
                modDelFolderMethods.SearchFolderByUnmerged(lvFolderList);
            }
        }

        private void txtTradersName_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = Char.ToUpper(e.KeyChar);
        }

        private void txtSection_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = Char.ToUpper(e.KeyChar);
        }

        private void txtLocation_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = Char.ToUpper(e.KeyChar);
        }

        private void chkAvailStatus_CheckedChanged(object sender, EventArgs e)
        {
            chkAvailStatus.Checked = true;
        }
    }

}