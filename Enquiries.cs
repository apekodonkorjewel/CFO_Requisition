using System;
using System.Windows.Forms;

namespace CFO_Requisition_System
{
    public partial class Enquiries : Form
    {
        EnquiriesMethods enquiriesMethods = new EnquiriesMethods();

        public Enquiries()
        {
            InitializeComponent();
        }

        public void Enquiries_Load(object sender, EventArgs e)
        {
            enquiriesMethods.LoadFolderDetails(lvFolderList);

            rbReturned.Checked = false;
            rbNotReturned.Checked = false;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            Enquiries_Load(sender, e);
        }

        private void rbIssuedBy_CheckedChanged(object sender, EventArgs e)
        {
            if (rbIssuedBy.Checked)
            {
                rbCollectedBy.Checked = false;
                rbFolderName.Checked = false;
                rbReceivedBy.Checked = false;
                rbTin.Checked = false;
            }
        }

        private void rbCollectedBy_CheckedChanged(object sender, EventArgs e)
        {
            if (rbCollectedBy.Checked)
            {
                rbIssuedBy.Checked = false;
                rbFolderName.Checked = false;
                rbReceivedBy.Checked = false;
                rbTin.Checked = false;
            }
        }

        private void rbTin_CheckedChanged_1(object sender, EventArgs e)
        {
            if (rbCollectedBy.Checked)
            {
                rbIssuedBy.Checked = false;
                rbFolderName.Checked = false;
                rbReceivedBy.Checked = false;
                rbCollectedBy.Checked = false;
            }
        }

        private void rbReceivedBy_CheckedChanged(object sender, EventArgs e)
        {
            if (rbReceivedBy.Checked)
            {
                rbIssuedBy.Checked = false;
                rbFolderName.Checked = false;
                rbCollectedBy.Checked = false;
                rbTin.Checked = false;
            }
        }

        private void rbFolderName_CheckedChanged(object sender, EventArgs e)
        {
            if (rbFolderName.Checked)
            {
                rbIssuedBy.Checked = false;
                rbCollectedBy.Checked = false;
                rbTin.Checked = false;
                rbReceivedBy.Checked = false;
            }
        }

        private void rbNotReturned_CheckedChanged(object sender, EventArgs e)
        {
            if (rbNotReturned.Checked)
                rbReturned.Checked = false;
        }

        private void rbReturned_CheckedChanged(object sender, EventArgs e)
        {
            if (rbReturned.Checked)
                rbNotReturned.Checked = false;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (txtSearch.TextLength != 0)
            {
                //Search by Issued By
                if (rbIssuedBy.Checked)
                    enquiriesMethods.SearchRequisitionByIssuedBy(lvFolderList, txtSearch, rbNotReturned, rbReturned);

                //Search by Collected By
                else if (rbCollectedBy.Checked)
                    enquiriesMethods.SearchRequisitionByCollectedBy(lvFolderList, txtSearch, rbNotReturned, rbReturned);

                //Search by TIN
                else if (rbTin.Checked)
                    enquiriesMethods.SearchRequisitionByTin(lvFolderList, txtSearch, rbNotReturned, rbReturned);

                //Search by Folder Name
                else if (rbFolderName.Checked)
                    enquiriesMethods.SearchRequisitionByFolderName(lvFolderList, txtSearch, rbNotReturned, rbReturned);

                else if (rbReceivedBy.Checked)
                    enquiriesMethods.SearchRequisitionByReceivedBy(lvFolderList, txtSearch, rbNotReturned, rbReturned);

                //No criterion selected
                else
                    MessageBox.Show("Select a criterion to search", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            else if (txtSearch.Text.Length == 0 && rbNotReturned.Checked)
            {
                //Search by not returned only
                enquiriesMethods.SearchRequisitionByNotReturned(lvFolderList, rbNotReturned);
            }

            else if (txtSearch.Text.Length == 0 && rbReturned.Checked)
            {
                //Search by not returned only
                enquiriesMethods.SearchRequisitionByReturned(lvFolderList, rbReturned);
            }

            else
            {
                MessageBox.Show("Enter value to search", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            
        }

    }
}
