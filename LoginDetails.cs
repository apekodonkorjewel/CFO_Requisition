using System;
using System.Windows.Forms;

namespace CFO_Requisition_System
{
    public partial class LoginDetails : Form
    {
        private LoginDetailsMethods loginDetailsMethods = new LoginDetailsMethods();

        public LoginDetails()
        {
            InitializeComponent();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            if (loginDetailsMethods.RowExists(dtpFrom.Text, dtpTo.Text))
            {
                loginDetailsMethods.ClearCrystalReport(rptLoginBookViewer);
                rptLoginBookViewer.Show();
                loginDetailsMethods.LoadLogBook(rptLoginBookViewer, dtpFrom.Text, dtpTo.Text);
            }

            else
            {
                MessageBox.Show("No results found", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void LoginDetails_Load(object sender, EventArgs e)
        {
            loginDetailsMethods.ClearCrystalReport(rptLoginBookViewer);
            rptLoginBookViewer.Show();
            rptLoginBookViewer.ReportSource = null;
            rptLoginBookViewer.Refresh();
        }
    }
}
