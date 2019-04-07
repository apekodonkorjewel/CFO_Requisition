using System;
using System.Windows.Forms;

namespace CFO_Requisition_System
{
    public partial class NewFolder : Form
    {
        private NewFolderMethods newFolderMethods = new NewFolderMethods();

        public NewFolder()
        {
            InitializeComponent();
        }

        public void NewFolder_Load(object sender, EventArgs e)
        {
            //Fill listview with folder list
            newFolderMethods.LoadFolderDetails(lvFolderList);
            chkAvailStatus.Checked = true;
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

        private void btnSaveFolder_Click(object sender, EventArgs e)
        {
            //Group all textboxes on the form into one array
            TextBox[] textBoxes = { txtSection, txtTradersName, txtLocation };

            //Check if all details have been entered
            bool isEmpty = newFolderMethods.IsEmpty(textBoxes);


            //Request for confirmation of command (Are you sure you want to logout?)
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to continue?", "Information",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (dialogResult == DialogResult.Yes)
            {
                //Save the Officer's details in the database
                if (isEmpty)
                {
                    MessageBox.Show("Make sure you complete all fields");
                }

                else
                {
                    newFolderMethods.InsertFolder(txtTradersName.Text, txtTin.Text, txtSection.Text, txtLocation.Text, chkAvailStatus, chkMergedStatus);
                }
 
                //Refresh the listview
                NewFolder_Load(sender, e);

                //Clear the form
                btnClearDetails_Click(sender, e);
            }
        }

        public void btnClearDetails_Click(object sender, EventArgs e)
        {
            //Group all the controls into arrays
            TextBox[] textBoxes = { txtSection, txtTradersName, txtLocation, txtTin };
            CheckBox[] checkBoxes = {chkAvailStatus, chkMergedStatus};

            //Clear the textboxes and uncheck checkboxes
            newFolderMethods.Clear(textBoxes, checkBoxes);
        }

    }
}
