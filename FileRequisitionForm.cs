using System;
using System.Windows.Forms;

namespace CFO_Requisition_System
{
    public partial class FileRequisitionForm : Form
    {
        private FileRequisitionMethods fileRequisitionMethods = new FileRequisitionMethods();
        public FileRequisitionForm()
        {
            InitializeComponent();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            if (fileRequisitionMethods.RowExists(dtpPeriod.Text))
            {
                fileRequisitionMethods.ClearCrystalReport(rptMonthlyReport);
                rptMonthlyReport.Show();
                fileRequisitionMethods.LoadMonthlyRequisitionForm(rptMonthlyReport, dtpPeriod.Text);
            }
                
            else
                MessageBox.Show("No results found", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void FileRequisitionForm_Load(object sender, EventArgs e)
        {
            fileRequisitionMethods.ClearCrystalReport(rptMonthlyReport);
            rptMonthlyReport.Show();
            rptMonthlyReport.ReportSource = null;
            rptMonthlyReport.Refresh();
        }
    }
}
