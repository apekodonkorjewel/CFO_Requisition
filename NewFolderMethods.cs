using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CFO_Requisition_System
{
    class NewFolderMethods
    {
        //Object of the connection class (A connection to the database)
        Connection con = new Connection();

        //Generate ID for table
        public int FetchLastID(string tableName, string idColumn)
        {
            int lastID = 0;

            SqlCommand cmd = con.cmd();
            cmd.CommandText = "select " + idColumn + " from " + tableName;

            SqlDataAdapter dataAdapter = new SqlDataAdapter();
            dataAdapter.SelectCommand = cmd;
            DataSet dataSet = new DataSet();

            dataAdapter.Fill(dataSet, tableName);
            DataTable dTable = dataSet.Tables[tableName];

            DataRow dRow = dTable.Rows[dTable.Rows.Count - 1];
            lastID = Int32.Parse(dRow[idColumn].ToString());

            return lastID;
        }

        public int GenerateID(string tableName, string idColumn)
        {
            string strTableName = tableName;

            SqlCommand cmd = con.cmd();
            cmd.CommandText = "select * from " + strTableName;

            SqlDataAdapter dataAdapter = new SqlDataAdapter();
            dataAdapter.SelectCommand = cmd;
            DataSet dataSet = new DataSet();

            dataAdapter.Fill(dataSet, strTableName);
            DataTable dTable = dataSet.Tables[strTableName];

            int intID;

            if (dTable.Rows.Count == 0)
                intID = dTable.Rows.Count + 1;

            else
                intID = FetchLastID(tableName, idColumn) + 1;

            return intID;
        }

        //Method to save details of a new folder
        public void InsertFolder(string folderName, string tin, string section, string location, CheckBox availStatus, CheckBox mergedStatus)
        {
            int intID = GenerateID("Folder", "folderID");
            string strFolderName = folderName;
            string strTin = tin;
            string strSection = section;
            string strLocation = location;
            string strAvailStatus = "UNAVAILABLE";
            string strMergedStatus = "UNMERGED";

            if (availStatus.Checked)
            {
                strAvailStatus = "AVAILABLE";
            }
                
            if (mergedStatus.Checked)
            {
                strMergedStatus = "MERGED";
            }

            SqlCommand cmd = con.cmd();

            cmd.CommandText = "EXECUTE InsertFolder @folderID, @folderName, @location, @availStatus, @mergedStatus, @tin, @section";

            cmd.Parameters.Add("@folderID", SqlDbType.Int).Value = intID;            
            cmd.Parameters.Add("@folderName", SqlDbType.VarChar, 100).Value = strFolderName;
            cmd.Parameters.Add("@location", SqlDbType.VarChar, 40).Value = strLocation;
            cmd.Parameters.Add("@availStatus", SqlDbType.VarChar, 20).Value = strAvailStatus;
            cmd.Parameters.Add("@mergedStatus", SqlDbType.VarChar, 10).Value = strMergedStatus;
            cmd.Parameters.Add("@tin", SqlDbType.NVarChar, 11).Value = strTin;
            cmd.Parameters.Add("@section", SqlDbType.NChar, 5).Value = strSection;

            cmd.ExecuteNonQuery();
            con.connectionClose();
        }

        //Method to check if all details are filled
        public bool IsEmpty (TextBox[] textBoxes)
        {
            foreach (TextBox textBox in textBoxes)
            {
                if (textBox.TextLength == 0)
                    return true;
            }
            return false;
        }

        //Method to Clear text boxes and check boxes 
        public void Clear(TextBox[] textBoxes, CheckBox[] checkBoxes)
        {
            foreach (TextBox textBox in textBoxes)
            {
                textBox.Clear();
            }

            foreach (CheckBox checkBox in checkBoxes)
            {
                checkBox.Checked = false;
            }
        }

        //Fill listview with list of folders
        public void LoadFolderDetails(ListView lv)
        {
            lv.Clear();
            lv.Columns.Add("ID", 30, HorizontalAlignment.Left);
            lv.Columns.Add("TIN", 100, HorizontalAlignment.Left);
            lv.Columns.Add("Folder Name", 240, HorizontalAlignment.Left);
            lv.Columns.Add("Available", 80, HorizontalAlignment.Left);
            lv.Columns.Add("Merge", 50, HorizontalAlignment.Left);
            lv.Columns.Add("Sector", 50, HorizontalAlignment.Left);

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

                    lv.Items.Add(lvi);
                }
            }

        }

    }
}
