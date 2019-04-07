using System;
using System.Collections;
using System.Windows.Forms;

namespace CFO_Requisition_System
{
    public partial class RequisitionBook : Form
    {
        private RequisitionBookMethods requisitionBookMethods = new RequisitionBookMethods();

        public RequisitionBook()
        {
            InitializeComponent();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            //if (rbRequestDate.Checked)
            //{
                if (requisitionBookMethods.RowExists(dtpFrom.Text, dtpTo.Text))
                {
                    requisitionBookMethods.ClearCrystalReport(crvRequisitionBook);
                    crvRequisitionBook.Show();
                    requisitionBookMethods.LoadRequisitionBook(crvRequisitionBook, dtpFrom.Text, dtpTo.Text);
                }

                else
                    MessageBox.Show("No results found", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}
  
            /*
            if (rbOfficer.Checked)
            {
                if (cbCollectedBy.SelectedIndex != -1)
                {
                    if (requisitionBookMethods.RowExists(cbCollectedBy.Text))
                    {
                        requisitionBookMethods.ClearCrystalReport(crvRequisitionBook);
                        crvRequisitionBook.Show();
                        requisitionBookMethods.LoadRequisitionBook(crvRequisitionBook, cbCollectedBy.Text);
                    }
                        

                    else
                        MessageBox.Show("No results found", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                    
                else
                    MessageBox.Show("Select an Officer", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            else
            {
                MessageBox.Show("Select a criterion to use (Check either Request Date / Officer)", "Information",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }*/
                
        }

        public void RequisitionBook_Load(object sender, EventArgs e)
        {
            //Do not show outstanding returns groupbox
            grpCollectedBy.Visible = false;

            //Disable group boxes
            //grpRequestDate.Enabled = false;
            //grpCollectedBy.Enabled = false;

            //Clear radio button selections
            rbOfficer.Checked = false;
            rbRequestDate.Checked = false;

            //Load ComboBox with all possible staff
            cbCollectedBy.Items.Clear();
            cbCollectedBy.ResetText();
            requisitionBookMethods.LoadComboBoxByUnitExceptCFO("AUDIT", "CENTRAL", cbCollectedBy);
            requisitionBookMethods.LoadComboBoxByUnitExceptCFO("CEDM", "CENTRAL", cbCollectedBy);
            requisitionBookMethods.LoadComboBoxByUnitExceptCFO("TPS", "CENTRAL", cbCollectedBy);

               
            requisitionBookMethods.ClearCrystalReport(crvRequisitionBook);
            crvRequisitionBook.Show();
            crvRequisitionBook.ReportSource = null;
            crvRequisitionBook.Refresh();
        }

        private void rbRequestDate_CheckedChanged(object sender, EventArgs e)
        {
            if (rbRequestDate.Checked)
            {
                rbOfficer.Checked = false;
                grpRequestDate.Enabled = true;
                grpCollectedBy.Enabled = false;
            }
                
        }

        private void rbOfficer_CheckedChanged(object sender, EventArgs e)
        {
            if (rbOfficer.Checked)
            {
                rbRequestDate.Checked = false;
                grpRequestDate.Enabled = false;
                grpCollectedBy.Enabled = true;
            }
                
        }

        private void cbCollectedBy_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }
    }
}
