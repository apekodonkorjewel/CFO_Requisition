using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CFO_Requisition_System
{
    class ModDelFolderMethods
    {
        //Object of the connection class (A connection to the database)
        Connection con = new Connection();

        public bool IsEmpty(TextBox[] textBoxes)
        {
            foreach (TextBox textBox in textBoxes)
            {
                if (textBox.TextLength == 0)
                    return true;
            }
            return false;
        }

        //Load folder details into list view
        public void LoadFolderDetails(ListView lv)
        {
            lv.Clear();
            lv.Columns.Add("ID", 30, HorizontalAlignment.Left);
            lv.Columns.Add("TIN", 100, HorizontalAlignment.Left);
            lv.Columns.Add("Folder Name", 220, HorizontalAlignment.Left);
            lv.Columns.Add("Available", 80, HorizontalAlignment.Left);
            lv.Columns.Add("Merge", 50, HorizontalAlignment.Left);
            lv.Columns.Add("Sector", 30, HorizontalAlignment.Left);
            lv.Columns.Add("Location", 50, HorizontalAlignment.Left);

            string strSQL = "SELECT * FROM Folder";

            SqlCommand cmd = con.cmd();
            cmd.CommandText = strSQL;

            SqlDataAdapter dataAdapter = new SqlDataAdapter();
            dataAdapter.SelectCommand = cmd;

            DataSet dataSet = new DataSet();
            dataAdapter.Fill(dataSet, "Folder");

            DataTable dataTable = dataSet.Tables["Folder"];

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow dataRow = dataTable.Rows[i];

                if (dataRow.RowState != DataRowState.Deleted)
                {
                    ListViewItem lvi = new ListViewItem(dataRow["folderID"].ToString(), 0);
                    lvi.SubItems.Add(dataRow["tin"].ToString());
                    lvi.SubItems.Add(dataRow["folderName"].ToString());
                    lvi.SubItems.Add(dataRow["availStatus"].ToString());
                    lvi.SubItems.Add(dataRow["mergedStatus"].ToString());
                    lvi.SubItems.Add(dataRow["section"].ToString());
                    lvi.SubItems.Add(dataRow["location"].ToString());

                    lv.Items.Add(lvi);
                }
            }

        }

        //Fill UI Controls
        public void FillUIControls(ListView lv, TextBox txtTin, TextBox txtSection, TextBox txtLocation, TextBox txtFolderName, CheckBox availStatus, CheckBox mergedStatus)
        {
            txtTin.Text = lv.SelectedItems[0].SubItems[1].Text;
            txtFolderName.Text = lv.SelectedItems[0].SubItems[2].Text;
            
            if ((lv.SelectedItems[0].SubItems[3].Text) == "AVAILABLE")
            {
                availStatus.Checked = true;
            }
            else
            {
                availStatus.Checked = false;
            }

            if ((lv.SelectedItems[0].SubItems[4].Text) == "MERGED")
            {
                mergedStatus.Checked = true;
            }
            else
            {
                mergedStatus.Checked = false;
            }

            txtSection.Text = lv.SelectedItems[0].SubItems[5].Text;
            txtLocation.Text = lv.SelectedItems[0].SubItems[6].Text;
        }

        //Modify Folder
        public void ModifyFolder(string section, string tin, string folderName, string location, CheckBox chkAvailStatus, CheckBox chkMergedStatus, string folderID)
        {
            string availStatus = "UNAVAILABLE";
            if (chkAvailStatus.Checked)
                availStatus = "AVAILABLE";

            string mergedStatus = "UNMERGED";
            if (chkMergedStatus.Checked)
                mergedStatus = "MERGED";

            int intFolderID = Int32.Parse(folderID);
            string strSection = section;
            string strFolderName = folderName;
            string strLocation = location;
            string strAvailStatus = availStatus;
            string strMergedStatus = mergedStatus;
            string strTin = tin;

            SqlCommand cmd = con.cmd();

            cmd.CommandText = "EXECUTE ModifyFolder @section, @folderName, @location, @availStatus, @mergedStatus, @tin, @folderID";

            cmd.Parameters.Add("@section", SqlDbType.NChar, 5).Value = strSection;
            cmd.Parameters.Add("@folderName", SqlDbType.VarChar, 100).Value = strFolderName;
            cmd.Parameters.Add("@location", SqlDbType.VarChar, 40).Value = strLocation;
            cmd.Parameters.Add("@availStatus", SqlDbType.VarChar, 20).Value = strAvailStatus;
            cmd.Parameters.Add("@folderID", SqlDbType.Int).Value = intFolderID;
            cmd.Parameters.Add("@mergedStatus", SqlDbType.VarChar, 10).Value = strMergedStatus;
            cmd.Parameters.Add("@tin", SqlDbType.VarChar, 11).Value = strTin;

            cmd.ExecuteNonQuery();
            con.connectionClose();
        }

        //Delete Folder
        public void DeleteFolder(string folderID)
        {
            int intFolderID = Int32.Parse(folderID);

            SqlCommand cmd = con.cmd();
            cmd.CommandText = "EXECUTE DeleteFolder @folderID";

            cmd.Parameters.Add("@folderID", SqlDbType.Int).Value = intFolderID;

            cmd.ExecuteNonQuery();
            con.connectionClose();
        }

        //Search Folder by Name
        public void SearchFolderByName(ListView lv, TextBox txtSearchBox)
        {
            string search = txtSearchBox.Text;

            SqlCommand cmd = con.cmd();
            string strSQL = "SELECT * FROM Folder where folderName like '" + search + "%'";

            lv.Clear();
            lv.Columns.Add("ID", 30, HorizontalAlignment.Left);
            lv.Columns.Add("TIN", 100, HorizontalAlignment.Left);
            lv.Columns.Add("Folder Name", 220, HorizontalAlignment.Left);
            lv.Columns.Add("Available", 80, HorizontalAlignment.Left);
            lv.Columns.Add("Merge", 50, HorizontalAlignment.Left);
            lv.Columns.Add("Sector", 30, HorizontalAlignment.Left);
            lv.Columns.Add("Location", 50, HorizontalAlignment.Left);

            cmd.CommandText = strSQL;

            SqlDataAdapter dataAdapter = new SqlDataAdapter();
            dataAdapter.SelectCommand = cmd;

            DataSet dataSet = new DataSet();
            dataAdapter.Fill(dataSet, "Folder");

            DataTable dataTable = dataSet.Tables["Folder"];

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow dataRow = dataTable.Rows[i];

                if (dataRow.RowState != DataRowState.Deleted)
                {
                    ListViewItem lvi = new ListViewItem(dataRow["folderID"].ToString(), 0);
                    lvi.SubItems.Add(dataRow["tin"].ToString());
                    lvi.SubItems.Add(dataRow["folderName"].ToString());
                    lvi.SubItems.Add(dataRow["availStatus"].ToString());
                    lvi.SubItems.Add(dataRow["mergedStatus"].ToString());
                    lvi.SubItems.Add(dataRow["section"].ToString());
                    lvi.SubItems.Add(dataRow["location"].ToString());

                    lv.Items.Add(lvi);
                }
            }
        }

        //Search Folder by Section
        public void SearchFolderBySection(ListView lv, TextBox txtSearchBox)
        {
            string search = txtSearchBox.Text;

            SqlCommand cmd = con.cmd();
            string strSQL = "SELECT * FROM Folder where section like '" + search + "%'";

            lv.Clear();
            lv.Columns.Add("ID", 30, HorizontalAlignment.Left);
            lv.Columns.Add("TIN", 100, HorizontalAlignment.Left);
            lv.Columns.Add("Folder Name", 220, HorizontalAlignment.Left);
            lv.Columns.Add("Available", 80, HorizontalAlignment.Left);
            lv.Columns.Add("Merge", 50, HorizontalAlignment.Left);
            lv.Columns.Add("Sector", 30, HorizontalAlignment.Left);
            lv.Columns.Add("Location", 50, HorizontalAlignment.Left);

            cmd.CommandText = strSQL;

            SqlDataAdapter dataAdapter = new SqlDataAdapter();
            dataAdapter.SelectCommand = cmd;

            DataSet dataSet = new DataSet();
            dataAdapter.Fill(dataSet, "Folder");

            DataTable dataTable = dataSet.Tables["Folder"];

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow dataRow = dataTable.Rows[i];

                if (dataRow.RowState != DataRowState.Deleted)
                {
                    ListViewItem lvi = new ListViewItem(dataRow["folderID"].ToString(), 0);
                    lvi.SubItems.Add(dataRow["tin"].ToString());
                    lvi.SubItems.Add(dataRow["folderName"].ToString());
                    lvi.SubItems.Add(dataRow["availStatus"].ToString());
                    lvi.SubItems.Add(dataRow["mergedStatus"].ToString());
                    lvi.SubItems.Add(dataRow["section"].ToString());
                    lvi.SubItems.Add(dataRow["location"].ToString());

                    lv.Items.Add(lvi);
                }
            }
        }

        //Search Folder by Unavailability
        public void SearchFolderByUnavailability(ListView lv)
        {
            SqlCommand cmd = con.cmd();
            string strSQL = "SELECT * FROM Folder where availStatus like 'unavail%'";

            lv.Clear();
            lv.Columns.Add("ID", 30, HorizontalAlignment.Left);
            lv.Columns.Add("TIN", 100, HorizontalAlignment.Left);
            lv.Columns.Add("Folder Name", 220, HorizontalAlignment.Left);
            lv.Columns.Add("Available", 80, HorizontalAlignment.Left);
            lv.Columns.Add("Merge", 50, HorizontalAlignment.Left);
            lv.Columns.Add("Sector", 30, HorizontalAlignment.Left);
            lv.Columns.Add("Location", 50, HorizontalAlignment.Left);

            cmd.CommandText = strSQL;

            SqlDataAdapter dataAdapter = new SqlDataAdapter();
            dataAdapter.SelectCommand = cmd;

            DataSet dataSet = new DataSet();
            dataAdapter.Fill(dataSet, "Folder");

            DataTable dataTable = dataSet.Tables["Folder"];

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow dataRow = dataTable.Rows[i];

                if (dataRow.RowState != DataRowState.Deleted)
                {
                    ListViewItem lvi = new ListViewItem(dataRow["folderID"].ToString(), 0);
                    lvi.SubItems.Add(dataRow["tin"].ToString());
                    lvi.SubItems.Add(dataRow["folderName"].ToString());
                    lvi.SubItems.Add(dataRow["availStatus"].ToString());
                    lvi.SubItems.Add(dataRow["mergedStatus"].ToString());
                    lvi.SubItems.Add(dataRow["section"].ToString());
                    lvi.SubItems.Add(dataRow["location"].ToString());

                    lv.Items.Add(lvi);
                }
            }
        }

        //Search Folder by Availability
        public void SearchFolderByAvailability(ListView lv)
        {
            SqlCommand cmd = con.cmd();
            string strSQL = "SELECT * FROM Folder where availStatus like 'avail%'";

            lv.Clear();
            lv.Columns.Add("ID", 30, HorizontalAlignment.Left);
            lv.Columns.Add("TIN", 100, HorizontalAlignment.Left);
            lv.Columns.Add("Folder Name", 220, HorizontalAlignment.Left);
            lv.Columns.Add("Available", 80, HorizontalAlignment.Left);
            lv.Columns.Add("Merge", 50, HorizontalAlignment.Left);
            lv.Columns.Add("Sector", 30, HorizontalAlignment.Left);
            lv.Columns.Add("Location", 50, HorizontalAlignment.Left);

            cmd.CommandText = strSQL;

            SqlDataAdapter dataAdapter = new SqlDataAdapter();
            dataAdapter.SelectCommand = cmd;

            DataSet dataSet = new DataSet();
            dataAdapter.Fill(dataSet, "Folder");

            DataTable dataTable = dataSet.Tables["Folder"];

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow dataRow = dataTable.Rows[i];

                if (dataRow.RowState != DataRowState.Deleted)
                {
                    ListViewItem lvi = new ListViewItem(dataRow["folderID"].ToString(), 0);
                    lvi.SubItems.Add(dataRow["tin"].ToString());
                    lvi.SubItems.Add(dataRow["folderName"].ToString());
                    lvi.SubItems.Add(dataRow["availStatus"].ToString());
                    lvi.SubItems.Add(dataRow["mergedStatus"].ToString());
                    lvi.SubItems.Add(dataRow["section"].ToString());
                    lvi.SubItems.Add(dataRow["location"].ToString());

                    lv.Items.Add(lvi);
                }
            }
        }

        public void SearchFolderByMerged(ListView lv)
        {
            SqlCommand cmd = con.cmd();
            string strSQL = "SELECT * FROM Folder where mergedStatus like 'MERGED%'";

            lv.Clear();
            lv.Columns.Add("ID", 30, HorizontalAlignment.Left);
            lv.Columns.Add("TIN", 100, HorizontalAlignment.Left);
            lv.Columns.Add("Folder Name", 220, HorizontalAlignment.Left);
            lv.Columns.Add("Available", 80, HorizontalAlignment.Left);
            lv.Columns.Add("Merge", 50, HorizontalAlignment.Left);
            lv.Columns.Add("Sector", 30, HorizontalAlignment.Left);
            lv.Columns.Add("Location", 50, HorizontalAlignment.Left);

            cmd.CommandText = strSQL;

            SqlDataAdapter dataAdapter = new SqlDataAdapter();
            dataAdapter.SelectCommand = cmd;

            DataSet dataSet = new DataSet();
            dataAdapter.Fill(dataSet, "Folder");

            DataTable dataTable = dataSet.Tables["Folder"];

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow dataRow = dataTable.Rows[i];

                if (dataRow.RowState != DataRowState.Deleted)
                {
                    ListViewItem lvi = new ListViewItem(dataRow["folderID"].ToString(), 0);
                    lvi.SubItems.Add(dataRow["tin"].ToString());
                    lvi.SubItems.Add(dataRow["folderName"].ToString());
                    lvi.SubItems.Add(dataRow["availStatus"].ToString());
                    lvi.SubItems.Add(dataRow["mergedStatus"].ToString());
                    lvi.SubItems.Add(dataRow["section"].ToString());
                    lvi.SubItems.Add(dataRow["location"].ToString());

                    lv.Items.Add(lvi);
                }
            }
        }

        public void SearchFolderByUnmerged(ListView lv)
        {
            SqlCommand cmd = con.cmd();
            string strSQL = "SELECT * FROM Folder where mergedStatus like 'UNMERGED%'";

            lv.Clear();
            lv.Columns.Add("ID", 30, HorizontalAlignment.Left);
            lv.Columns.Add("TIN", 100, HorizontalAlignment.Left);
            lv.Columns.Add("Folder Name", 220, HorizontalAlignment.Left);
            lv.Columns.Add("Available", 80, HorizontalAlignment.Left);
            lv.Columns.Add("Merge", 50, HorizontalAlignment.Left);
            lv.Columns.Add("Sector", 30, HorizontalAlignment.Left);
            lv.Columns.Add("Location", 50, HorizontalAlignment.Left);

            cmd.CommandText = strSQL;

            SqlDataAdapter dataAdapter = new SqlDataAdapter();
            dataAdapter.SelectCommand = cmd;

            DataSet dataSet = new DataSet();
            dataAdapter.Fill(dataSet, "Folder");

            DataTable dataTable = dataSet.Tables["Folder"];

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow dataRow = dataTable.Rows[i];

                if (dataRow.RowState != DataRowState.Deleted)
                {
                    ListViewItem lvi = new ListViewItem(dataRow["folderID"].ToString(), 0);
                    lvi.SubItems.Add(dataRow["tin"].ToString());
                    lvi.SubItems.Add(dataRow["folderName"].ToString());
                    lvi.SubItems.Add(dataRow["availStatus"].ToString());
                    lvi.SubItems.Add(dataRow["mergedStatus"].ToString());
                    lvi.SubItems.Add(dataRow["section"].ToString());
                    lvi.SubItems.Add(dataRow["location"].ToString());

                    lv.Items.Add(lvi);
                }
            }
        }
    }
}