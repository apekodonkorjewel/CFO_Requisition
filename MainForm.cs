using System;
using System.Drawing;
using System.Windows.Forms;

namespace CFO_Requisition_System
{
    public partial class MainForm : Form
    {
        private MainFormMethods mainFormMethods = new MainFormMethods();
        
        //Children of Main form (parent form)
        NewOfficer newOfficer = new NewOfficer();
        NewFolder newFolder = new NewFolder();
        ModDelOfficer modDelOfficer= new ModDelOfficer();
        ModDelFolder modDelFolder = new ModDelFolder();
        RequestFolder requestFolder = new RequestFolder();
        ReturnFolder returnFolder = new ReturnFolder();
        LoginDetails loginDetails = new LoginDetails();
        RequisitionBook requisitionBook = new RequisitionBook();
        Enquiries enquiries = new Enquiries();
        FileRequisitionForm fileRequisitionForm = new FileRequisitionForm();

        public MainForm()
        {
            InitializeComponent();

            newOfficer.MdiParent = this;
            newOfficer.StartPosition = FormStartPosition.Manual;
            newOfficer.Location = new Point(208, 180);

            newFolder.MdiParent = this;
            newFolder.StartPosition = FormStartPosition.Manual;
            newFolder.Location = new Point(208, 180);

            modDelFolder.MdiParent = this;
            modDelFolder.StartPosition = FormStartPosition.Manual;
            modDelFolder.Location = new Point(208, 180);

            modDelOfficer.MdiParent = this;
            modDelOfficer.StartPosition = FormStartPosition.Manual;
            modDelOfficer.Location = new Point(208, 180);

            requestFolder.MdiParent = this;
            requestFolder.StartPosition = FormStartPosition.Manual;
            requestFolder.Location = new Point(208, 180);

            returnFolder.MdiParent = this;
            returnFolder.StartPosition = FormStartPosition.Manual;
            returnFolder.Location = new Point(208, 180);

            loginDetails.MdiParent = this;
            loginDetails.StartPosition = FormStartPosition.Manual;
            loginDetails.Location = new Point(208, 180);

            requisitionBook.MdiParent = this;
            requisitionBook.StartPosition = FormStartPosition.Manual;
            requisitionBook.Location = new Point(208, 180);

            enquiries.MdiParent = this;
            enquiries.StartPosition = FormStartPosition.Manual;
            enquiries.Location = new Point(208, 180);

            fileRequisitionForm.MdiParent = this;
            fileRequisitionForm.StartPosition = FormStartPosition.Manual;
            fileRequisitionForm.Location = new Point(208, 180);
        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Request for confirmation of command (Are you sure you want to logout?)
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to logout?", "Information`",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            //If response is positive, save details into logfile and log user out of the system
            if (dialogResult == DialogResult.Yes)
            {
                //Save logout details in the log file   
                mainFormMethods.InsertLogoutFile();

                //Hide the other forms
                newOfficer.Hide();
                newFolder.Hide();
                modDelOfficer.Hide();
                modDelFolder.Hide();
                requestFolder.Hide();
                returnFolder.Hide();
                loginDetails.Hide();
                requisitionBook.Hide();
                fileRequisitionForm.Hide();

                //Hide the mainform (log user out of the system)
                this.Hide();
            }
        }

        private void newFolderToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //Display the New Folder form
            newFolder.Show();
            newFolder.NewFolder_Load(sender, e);
            newFolder.BringToFront();

            //Hide the other forms
            newOfficer.Hide();
            modDelOfficer.Hide();
            modDelFolder.Hide();
            requestFolder.Hide();
            returnFolder.Hide();
            loginDetails.Hide();
            requisitionBook.Hide();
            fileRequisitionForm.Hide();

            //Clear all forms
            newOfficer.btnClear_Click(sender, e);
            newFolder.btnClearDetails_Click(sender, e);
            modDelOfficer.btnClear_Click(sender, e);
            modDelFolder.btnClear_Click(sender, e);
            requestFolder.btnClearForm_Click(sender, e);
            returnFolder.btnClearForm_Click(sender, e);

            //Change Back Color of label
            Label[] labels =
            {
                lblRequisitionReport, lblNewFolder, lblNewOfficer, lblModifyFolder, lblModifyOfficer, lblRequestFolder,
                lblReturnFolder, lblPrintLog, lblPrintRequisition, lblEnquiries, lblLogout
                };

            foreach (Label label in labels)
            {
                label.BackColor = Color.White;
            }

            lblNewFolder.BackColor = Color.Gold;
        }

        private void newOfficerToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //Only the head of CFO has access to the New Officer form
            if (mainFormMethods.IsSupervisor())
            {
                //Display the New Officer form
                newOfficer.Show();
                newOfficer.NewOfficer_Load(sender, e);
                newOfficer.BringToFront();

                //Hide the other forms
                newFolder.Hide();
                modDelOfficer.Hide();
                modDelFolder.Hide();
                requestFolder.Hide();
                returnFolder.Hide();
                loginDetails.Hide();
                requisitionBook.Hide();
                fileRequisitionForm.Hide();

                //Clear all forms
                newOfficer.btnClear_Click(sender, e);
                newFolder.btnClearDetails_Click(sender, e);
                modDelOfficer.btnClear_Click(sender, e);
                modDelFolder.btnClear_Click(sender, e);
                requestFolder.btnClearForm_Click(sender, e);
                returnFolder.btnClearForm_Click(sender, e);

                //Change Back Color of label
                Label[] labels =
                {
                lblRequisitionReport, lblNewFolder, lblNewOfficer, lblModifyFolder, lblModifyOfficer, lblRequestFolder,
                lblReturnFolder, lblPrintLog, lblPrintRequisition, lblEnquiries, lblLogout
                };

                foreach (Label label in labels)
                {
                    label.BackColor = Color.White;
                }

                lblNewOfficer.BackColor = Color.Gold;
            }

            else
            {
                MessageBox.Show("****Access Denied****", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            MdiClient mdiClient;
            string exception = "Message";

            foreach (Control control in this.Controls)
            {
                try
                {
                    mdiClient = (MdiClient) control;
                    mdiClient.BackColor = this.BackColor;
                }
                catch (InvalidCastException exc)
                {
                    exception = exc.Message;
                }
            }

            //Send SMS Notification
            //mainFormMethods.sendSMS();
        }

        private void modFolderInfoToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //Display the ModDelFolder form
            modDelFolder.Show();
            modDelFolder.ModDelFolder_Load(sender, e);
            modDelFolder.BringToFront();

            //Hide the other forms
            newOfficer.Hide();
            newFolder.Hide();
            modDelOfficer.Hide();
            requestFolder.Hide();
            returnFolder.Hide();
            loginDetails.Hide();
            requisitionBook.Hide();
            fileRequisitionForm.Hide();

            //Clear all forms
            newOfficer.btnClear_Click(sender, e);
            newFolder.btnClearDetails_Click(sender, e);
            modDelOfficer.btnClear_Click(sender, e);
            modDelFolder.btnClear_Click(sender, e);
            requestFolder.btnClearForm_Click(sender, e);
            returnFolder.btnClearForm_Click(sender, e);

            //Change Back Color of label
            Label[] labels =
            {
                lblRequisitionReport, lblNewFolder, lblNewOfficer, lblModifyFolder, lblModifyOfficer, lblRequestFolder,
                lblReturnFolder, lblPrintLog, lblPrintRequisition, lblEnquiries, lblLogout
                };

            foreach (Label label in labels)
            {
                label.BackColor = Color.White;
            }

            lblModifyFolder.BackColor = Color.Gold;
        }

        private void modOfficerInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (mainFormMethods.IsSupervisor())
            {
                //Display the ModDelOfficer form
                modDelOfficer.Show();
                modDelOfficer.ModDelOfficer_Load(sender, e);
                modDelOfficer.BringToFront();

                //Hide the other forms
                newOfficer.Hide();
                newFolder.Hide();
                modDelFolder.Hide();
                requestFolder.Hide();
                returnFolder.Hide();
                loginDetails.Hide();
                requisitionBook.Hide();
                fileRequisitionForm.Hide();

                //Clear all forms
                newOfficer.btnClear_Click(sender, e);
                newFolder.btnClearDetails_Click(sender, e);
                modDelOfficer.btnClear_Click(sender, e);
                modDelFolder.btnClear_Click(sender, e);
                requestFolder.btnClearForm_Click(sender, e);
                returnFolder.btnClearForm_Click(sender, e);

                //Change Back Color of label
                Label[] labels =
                {
                lblRequisitionReport, lblNewFolder, lblNewOfficer, lblModifyFolder, lblModifyOfficer, lblRequestFolder,
                lblReturnFolder, lblPrintLog, lblPrintRequisition, lblEnquiries, lblLogout
                };

                foreach (Label label in labels)
                {
                    label.BackColor = Color.White;
                }

                lblModifyOfficer.BackColor = Color.Gold;
            }

            else
            {
                MessageBox.Show("****Access Denied****", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void requestFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Display the RequestFolder form
            requestFolder.Show();
            requestFolder.RequestFolder_Load(sender, e);
            requestFolder.BringToFront();

            //Hide the other forms
            newOfficer.Hide();
            newFolder.Hide();
            modDelOfficer.Hide();
            modDelFolder.Hide();
            returnFolder.Hide();
            loginDetails.Hide();
            requisitionBook.Hide();
            fileRequisitionForm.Hide();

            //Clear all forms
            newOfficer.btnClear_Click(sender, e);
            newFolder.btnClearDetails_Click(sender, e);
            modDelOfficer.btnClear_Click(sender, e);
            modDelFolder.btnClear_Click(sender, e);
            requestFolder.btnClearForm_Click(sender, e);
            returnFolder.btnClearForm_Click(sender, e);

            //Change Back Color of label
            Label[] labels =
            {
                lblRequisitionReport, lblNewFolder, lblNewOfficer, lblModifyFolder, lblModifyOfficer, lblRequestFolder,
                lblReturnFolder, lblPrintLog, lblPrintRequisition, lblEnquiries, lblLogout
                };

            foreach (Label label in labels)
            {
                label.BackColor = Color.White;
            }

            lblRequestFolder.BackColor = Color.Gold;
        }

        private void returnFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Display the RequestFolder form
            returnFolder.Show();
            returnFolder.ReturnFolder_Load(sender, e);
            returnFolder.BringToFront();

            //Hide the other forms
            newOfficer.Hide();
            newFolder.Hide();
            modDelOfficer.Hide();
            modDelFolder.Hide();
            requestFolder.Hide();
            loginDetails.Hide();
            requisitionBook.Hide();
            fileRequisitionForm.Hide();

            //Clear all forms
            newOfficer.btnClear_Click(sender, e);
            newFolder.btnClearDetails_Click(sender, e);
            modDelOfficer.btnClear_Click(sender, e);
            modDelFolder.btnClear_Click(sender, e);
            requestFolder.btnClearForm_Click(sender, e);
            returnFolder.btnClearForm_Click(sender, e);

            //Change Back Color of label
            Label[] labels =
            {
                lblRequisitionReport, lblNewFolder, lblNewOfficer, lblModifyFolder, lblModifyOfficer, lblRequestFolder,
                lblReturnFolder, lblPrintLog, lblPrintRequisition, lblEnquiries, lblLogout
                };

            foreach (Label label in labels)
            {
                label.BackColor = Color.White;
            }

            lblReturnFolder.BackColor = Color.Gold;
        }

        private void requisitionBookToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Display the Requisition Book form
            requisitionBook.Show();
            requisitionBook.RequisitionBook_Load(sender, e);
            requisitionBook.BringToFront();

            //Hide the other forms
            newOfficer.Hide();
            newFolder.Hide();
            modDelOfficer.Hide();
            modDelFolder.Hide();
            requestFolder.Hide();
            returnFolder.Hide();
            loginDetails.Hide();
            fileRequisitionForm.Hide();

            //Clear all forms
            newOfficer.btnClear_Click(sender, e);
            newFolder.btnClearDetails_Click(sender, e);
            modDelOfficer.btnClear_Click(sender, e);
            modDelFolder.btnClear_Click(sender, e);
            requestFolder.btnClearForm_Click(sender, e);
            returnFolder.btnClearForm_Click(sender, e);

            //Change Back Color of label
            Label[] labels =
            {
                lblRequisitionReport, lblNewFolder, lblNewOfficer, lblModifyFolder, lblModifyOfficer, lblRequestFolder,
                lblReturnFolder, lblPrintLog, lblPrintRequisition, lblEnquiries, lblLogout
                };

            foreach (Label label in labels)
            {
                label.BackColor = Color.White;
            }

            lblPrintRequisition.BackColor = Color.Gold;
        }

        private void fileRequisitionReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Display the FileRequisition form
            fileRequisitionForm.Show();
            fileRequisitionForm.FileRequisitionForm_Load(sender, e);
            fileRequisitionForm.BringToFront();

            //Hide the other forms
            newOfficer.Hide();
            newFolder.Hide();
            modDelOfficer.Hide();
            modDelFolder.Hide();
            requestFolder.Hide();
            returnFolder.Hide();
            loginDetails.Hide();

            //Clear all forms
            newOfficer.btnClear_Click(sender, e);
            newFolder.btnClearDetails_Click(sender, e);
            modDelOfficer.btnClear_Click(sender, e);
            modDelFolder.btnClear_Click(sender, e);
            requestFolder.btnClearForm_Click(sender, e);
            returnFolder.btnClearForm_Click(sender, e);

            //Change Back Color of label
            Label[] labels =
            {
                lblRequisitionReport, lblNewFolder, lblNewOfficer, lblModifyFolder, lblModifyOfficer, lblRequestFolder,
                lblReturnFolder, lblPrintLog, lblPrintRequisition, lblEnquiries, lblLogout
                };

            foreach (Label label in labels)
            {
                label.BackColor = Color.White;
            }

            lblRequisitionReport.BackColor = Color.Gold;
        }

        private void logBookToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (mainFormMethods.IsSupervisor())
            {
                //Display the Log Book form
                loginDetails.Show();
                loginDetails.LoginDetails_Load(sender, e);
                loginDetails.BringToFront();

                //Hide the other forms
                newOfficer.Hide();
                newFolder.Hide();
                modDelOfficer.Hide();
                modDelFolder.Hide();
                requestFolder.Hide();
                returnFolder.Hide();
                requisitionBook.Hide();
                fileRequisitionForm.Hide();

                //Clear all forms
                newOfficer.btnClear_Click(sender, e);
                newFolder.btnClearDetails_Click(sender, e);
                modDelOfficer.btnClear_Click(sender, e);
                modDelFolder.btnClear_Click(sender, e);
                requestFolder.btnClearForm_Click(sender, e);
                returnFolder.btnClearForm_Click(sender, e);

                //Change Back Color of label
                Label[] labels =
                {
                lblRequisitionReport, lblNewFolder, lblNewOfficer, lblModifyFolder, lblModifyOfficer, lblRequestFolder,
                lblReturnFolder, lblPrintLog, lblPrintRequisition, lblEnquiries, lblLogout
                };

                foreach (Label label in labels)
                {
                    label.BackColor = Color.White;
                }

                lblPrintLog.BackColor = Color.Gold;
            }

            else
            {
                MessageBox.Show("****Access Denied****", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void enquiriesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Display the Enquiries form
            enquiries.Show();
            enquiries.Enquiries_Load(sender, e);
            enquiries.BringToFront();

            //Hide the other forms
            newOfficer.Hide();
            newFolder.Hide();
            modDelOfficer.Hide();
            modDelFolder.Hide();
            requestFolder.Hide();
            returnFolder.Hide();
            loginDetails.Hide();
            requisitionBook.Hide();

            //Clear all forms
            newOfficer.btnClear_Click(sender, e);
            newFolder.btnClearDetails_Click(sender, e);
            modDelOfficer.btnClear_Click(sender, e);
            modDelFolder.btnClear_Click(sender, e);
            requestFolder.btnClearForm_Click(sender, e);
            returnFolder.btnClearForm_Click(sender, e);

            //Change Back Color of label
            Label[] labels =
            {
                lblRequisitionReport, lblNewFolder, lblNewOfficer, lblModifyFolder, lblModifyOfficer, lblRequestFolder,
                lblReturnFolder, lblPrintLog, lblPrintRequisition, lblEnquiries, lblLogout
            };

            foreach(Label label in labels)
            {
                label.BackColor = Color.White;
            }

            lblEnquiries.BackColor = Color.Gold;
        }

        private void lblNewFolder_Click(object sender, EventArgs e)
        {
            newFolderToolStripMenuItem1_Click(sender, e);
        }

        private void lblNewOfficer_Click(object sender, EventArgs e)
        {
            newOfficerToolStripMenuItem1_Click(sender, e);
        }

        private void lblModifyFolder_Click(object sender, EventArgs e)
        {
            modFolderInfoToolStripMenuItem1_Click(sender, e);
        }

        private void lblModifyOfficer_Click(object sender, EventArgs e)
        {
            modOfficerInfoToolStripMenuItem_Click(sender, e);
        }

        private void lblRequestFolder_Click(object sender, EventArgs e)
        {
            requestFolderToolStripMenuItem_Click(sender, e);
        }

        private void lblReturnFolder_Click(object sender, EventArgs e)
        {
            returnFolderToolStripMenuItem_Click(sender, e);
        }

        private void lblPrintRequisition_Click(object sender, EventArgs e)
        {
            requisitionBookToolStripMenuItem_Click(sender, e);
        }

        private void lblPrintLog_Click(object sender, EventArgs e)
        {
            logBookToolStripMenuItem_Click(sender, e);
        }

        private void lblEnquiries_Click(object sender, EventArgs e)
        {
            enquiriesToolStripMenuItem_Click(sender, e);
        }

        private void lblLogout_Click(object sender, EventArgs e)
        {
            logoutToolStripMenuItem_Click(sender, e);
        }

        private void lblRequisitionReport_Click(object sender, EventArgs e)
        {
            fileRequisitionReportToolStripMenuItem_Click(sender, e);
        }

        private void lblNewFolder_MouseEnter(object sender, EventArgs e)
        {
            Label[] labels =
            {
                lblRequisitionReport, lblNewFolder, lblNewOfficer, lblModifyFolder, lblModifyOfficer, lblRequestFolder,
                lblReturnFolder, lblPrintLog, lblPrintRequisition, lblEnquiries, lblLogout
            };

            foreach (Label lbl in labels)
            {
                if (lbl.BackColor != Color.Gold)
                    lbl.BackColor = Color.White;
            }
                

            if(lblNewFolder.BackColor != Color.Gold)
                lblNewFolder.BackColor = Color.LightYellow;
        }

        private void lblNewOfficer_MouseEnter(object sender, EventArgs e)
        {
            Label[] labels =
            {
                lblRequisitionReport, lblNewFolder, lblNewOfficer, lblModifyFolder, lblModifyOfficer, lblRequestFolder,
                lblReturnFolder, lblPrintLog, lblPrintRequisition, lblEnquiries, lblLogout
            };

            foreach (Label lbl in labels)
            {
                if (lbl.BackColor != Color.Gold)
                    lbl.BackColor = Color.White;
            }
                

            if (lblNewOfficer.BackColor != Color.Gold)
                lblNewOfficer.BackColor = Color.LightYellow;
        }

        private void lblModifyFolder_MouseEnter(object sender, EventArgs e)
        {
            Label[] labels =
            {
                lblRequisitionReport, lblNewFolder, lblNewOfficer, lblModifyFolder, lblModifyOfficer, lblRequestFolder,
                lblReturnFolder, lblPrintLog, lblPrintRequisition, lblEnquiries, lblLogout
            };

            foreach (Label lbl in labels)
            {
                if (lbl.BackColor != Color.Gold)
                    lbl.BackColor = Color.White;
            }
                
            if (lblModifyFolder.BackColor != Color.Gold)
                lblModifyFolder.BackColor = Color.LightYellow;
        }

        private void lblModifyOfficer_MouseEnter(object sender, EventArgs e)
        {
            Label[] labels =
            {
                lblRequisitionReport, lblNewFolder, lblNewOfficer, lblModifyFolder, lblModifyOfficer, lblRequestFolder,
                lblReturnFolder, lblPrintLog, lblPrintRequisition, lblEnquiries, lblLogout
            };

            foreach (Label lbl in labels)
            {
                if (lbl.BackColor != Color.Gold)
                    lbl.BackColor = Color.White;
            }
                
            if (lblModifyOfficer.BackColor != Color.Gold)
                lblModifyOfficer.BackColor = Color.LightYellow;
        }

        private void lblRequestFolder_MouseEnter(object sender, EventArgs e)
        {
            Label[] labels =
            {
                lblRequisitionReport, lblNewFolder, lblNewOfficer, lblModifyFolder, lblModifyOfficer, lblRequestFolder,
                lblReturnFolder, lblPrintLog, lblPrintRequisition, lblEnquiries, lblLogout
            };

            foreach (Label lbl in labels)
            {
                if (lbl.BackColor != Color.Gold)
                    lbl.BackColor = Color.White;
            }
                
            if (lblRequestFolder.BackColor != Color.Gold)
                lblRequestFolder.BackColor = Color.LightYellow;
        }

        private void lblReturnFolder_MouseEnter(object sender, EventArgs e)
        {
            Label[] labels =
            {
                lblRequisitionReport, lblNewFolder, lblNewOfficer, lblModifyFolder, lblModifyOfficer, lblRequestFolder,
                lblReturnFolder, lblPrintLog, lblPrintRequisition, lblEnquiries, lblLogout
            };

            foreach (Label lbl in labels)
            {
                if (lbl.BackColor != Color.Gold)
                    lbl.BackColor = Color.White;
            }                

            if (lblReturnFolder.BackColor != Color.Gold)
                lblReturnFolder.BackColor = Color.LightYellow;
        }

        private void lblPrintRequisition_MouseEnter(object sender, EventArgs e)
        {
            Label[] labels =
            {
                lblRequisitionReport, lblNewFolder, lblNewOfficer, lblModifyFolder, lblModifyOfficer, lblRequestFolder,
                lblReturnFolder, lblPrintLog, lblPrintRequisition, lblEnquiries, lblLogout
            };

            foreach (Label lbl in labels)
            {
                if (lbl.BackColor != Color.Gold)
                    lbl.BackColor = Color.White;
            }
                
            if (lblPrintRequisition.BackColor != Color.Gold)
                lblPrintRequisition.BackColor = Color.LightYellow;
        }

        private void lblPrintLog_MouseEnter(object sender, EventArgs e)
        {
            Label[] labels =
            {
                lblRequisitionReport, lblNewFolder, lblNewOfficer, lblModifyFolder, lblModifyOfficer, lblRequestFolder,
                lblReturnFolder, lblPrintLog, lblPrintRequisition, lblEnquiries, lblLogout
            };

            foreach (Label lbl in labels)
            {
                if (lbl.BackColor != Color.Gold)
                    lbl.BackColor = Color.White;
            }
                
            if (lblPrintLog.BackColor != Color.Gold)
                lblPrintLog.BackColor = Color.LightYellow;
        }

        private void lblLogout_MouseEnter(object sender, EventArgs e)
        {
            Label[] labels =
            {
                lblRequisitionReport, lblNewFolder, lblNewOfficer, lblModifyFolder, lblModifyOfficer, lblRequestFolder,
                lblReturnFolder, lblPrintLog, lblPrintRequisition, lblEnquiries, lblLogout
            };

            foreach (Label lbl in labels)
            {
                if (lbl.BackColor != Color.Gold)
                    lbl.BackColor = Color.White;
            }                

            if (lblLogout.BackColor != Color.Gold)
                lblLogout.BackColor = Color.LightYellow;
        }

        private void lblEnquiries_MouseEnter(object sender, EventArgs e)
        {
            Label[] labels =
            {
                lblRequisitionReport, lblNewFolder, lblNewOfficer, lblModifyFolder, lblModifyOfficer, lblRequestFolder,
                lblReturnFolder, lblPrintLog, lblPrintRequisition, lblEnquiries, lblLogout
            };

            foreach (Label lbl in labels)
            {
                if (lbl.BackColor != Color.Gold)
                    lbl.BackColor = Color.White;
            }                

            if (lblEnquiries.BackColor != Color.Gold)
                lblEnquiries.BackColor = Color.LightYellow;
        }

        private void lblRequisitionReport_MouseEnter(object sender, EventArgs e)
        {
            Label[] labels =
            {
                lblRequisitionReport, lblNewFolder, lblNewOfficer, lblModifyFolder, lblModifyOfficer, lblRequestFolder,
                lblReturnFolder, lblPrintLog, lblPrintRequisition, lblEnquiries, lblLogout
            };

            foreach (Label lbl in labels)
            {
                if (lbl.BackColor != Color.Gold)
                    lbl.BackColor = Color.White;
            }
                
            if (lblRequisitionReport.BackColor != Color.Gold)
                lblRequisitionReport.BackColor = Color.LightYellow;
        }

    }
}
