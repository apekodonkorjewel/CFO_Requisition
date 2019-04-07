using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CFO_Requisition_System
{
    class ReturnFolderMethods
    {
        //Object of the connection class (A connection to the database)
        Connection con = new Connection();

        public bool IsEmpty(TextBox[] textBoxes, ComboBox[] comboBoxes)
        {
            foreach (TextBox textBox in textBoxes)
            {
                if (textBox.TextLength == 0)
                    return true;
            }

            foreach (ComboBox comboBox in comboBoxes)
            {
                if (comboBox.SelectedIndex == -1)
                    return true;
            }
            return false;
        }

        public bool RowExists(string folderName)
        {
            string strSQL = "SELECT * FROM Requisition where tradersName = '" + folderName + "'";

            SqlCommand cmd = con.cmd();
            cmd.CommandText = strSQL;

            SqlDataAdapter dataAdapter = new SqlDataAdapter();
            dataAdapter.SelectCommand = cmd;

            DataSet dataSet = new DataSet();
            dataAdapter.Fill(dataSet, "Requisition");

            DataTable dataTable = dataSet.Tables["Requisition"];

            if (dataTable.Rows.Count == 0)
                return false;

            return true;
        }

        public string FetchUnitUsingFolderName(string folderName)
        {
            string strSQL = "SELECT * FROM Requisition where tradersName = '"+folderName+"'";

            SqlCommand cmd = con.cmd();
            cmd.CommandText = strSQL;

            SqlDataAdapter dataAdapter = new SqlDataAdapter();
            dataAdapter.SelectCommand = cmd;

            DataSet dataSet = new DataSet();
            dataAdapter.Fill(dataSet, "Requisition");

            DataTable dataTable = dataSet.Tables["Requisition"];

            DataRow dataRow = dataTable.Rows[dataTable.Rows.Count - 1];
            string unit = dataRow["unit"].ToString();

            return unit;
        }

        public string FetchCollectedByUsingFolderName(string folderName)
        {
            string strSQL = "SELECT * FROM Requisition where tradersName = '" + folderName + "'";

            SqlCommand cmd = con.cmd();
            cmd.CommandText = strSQL;

            SqlDataAdapter dataAdapter = new SqlDataAdapter();
            dataAdapter.SelectCommand = cmd;

            DataSet dataSet = new DataSet();
            dataAdapter.Fill(dataSet, "Requisition");

            DataTable dataTable = dataSet.Tables["Requisition"];

            DataRow dataRow = dataTable.Rows[dataTable.Rows.Count - 1];
            string collectedBy = dataRow["collectedBy"].ToString();

            return collectedBy;
        }

        public void LoadFolderDetails(ListView lv)
        {
            lv.Clear();
            lv.Columns.Add("ID", 30, HorizontalAlignment.Left);
            lv.Columns.Add("TIN", 70, HorizontalAlignment.Left);
            lv.Columns.Add("Folder Name", 300, HorizontalAlignment.Left);
            lv.Columns.Add("Merge", 100, HorizontalAlignment.Left);
            lv.Columns.Add("Unit/Sub-Unit", 150, HorizontalAlignment.Left);
            lv.Columns.Add("Collected By", 150, HorizontalAlignment.Left);

            string strSQL = "SELECT * FROM Requisition where returningOfficer is null";

            SqlCommand cmd = con.cmd();
            cmd.CommandText = strSQL;

            SqlDataAdapter dataAdapter = new SqlDataAdapter();
            dataAdapter.SelectCommand = cmd;

            DataSet dataSet = new DataSet();
            dataAdapter.Fill(dataSet, "Requisition");

            DataTable dataTable = dataSet.Tables["Requisition"];

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow dataRow = dataTable.Rows[i];

                if (dataRow.RowState != DataRowState.Deleted)
                {
                    ListViewItem lvi = new ListViewItem(dataRow["requestID"].ToString(), 0);
                    lvi.SubItems.Add(dataRow["tin"].ToString());
                    lvi.SubItems.Add(dataRow["tradersName"].ToString());
                    lvi.SubItems.Add(dataRow["mergedStatus"].ToString());

                    if (RowExists(dataRow["tradersName"].ToString()))
                    {
                        lvi.SubItems.Add(FetchUnitUsingFolderName(dataRow["tradersName"].ToString()));
                        lvi.SubItems.Add(FetchCollectedByUsingFolderName(dataRow["tradersName"].ToString()));
                    }

                    lv.Items.Add(lvi);
                }
            }
        }

        public string FetchLastReturnCode()
        {
            string strReturnCode = "";

            SqlCommand cmd = con.cmd();
            cmd.CommandText = "select * from SerialCodeLog";

            SqlDataAdapter dataAdapter = new SqlDataAdapter();
            dataAdapter.SelectCommand = cmd;
            DataSet dataSet = new DataSet();

            dataAdapter.Fill(dataSet, "SerialCodeLog");
            DataTable dTable = dataSet.Tables["SerialCodeLog"];

            DataRow dRow = dTable.Rows[dTable.Rows.Count - 1];
            strReturnCode = dRow["returnCode"].ToString();

            return strReturnCode;
        }

        public string FetchLastUsername()
        {
            string strLastUsername = "";

            SqlCommand cmd = con.cmd();
            cmd.CommandText = "select username from LogFile";

            SqlDataAdapter dataAdapter = new SqlDataAdapter();
            dataAdapter.SelectCommand = cmd;
            DataSet dataSet = new DataSet();

            dataAdapter.Fill(dataSet, "LogFile");
            DataTable dTable = dataSet.Tables["LogFile"];

            DataRow dRow = dTable.Rows[dTable.Rows.Count - 1];
            strLastUsername = dRow["username"].ToString();

            return strLastUsername;
        }

        public string FetchLastPassword()
        {
            string strLastPassword = "";

            SqlCommand cmd = con.cmd();
            cmd.CommandText = "select psword from LogFile";

            SqlDataAdapter dataAdapter = new SqlDataAdapter();
            dataAdapter.SelectCommand = cmd;
            DataSet dataSet = new DataSet();

            dataAdapter.Fill(dataSet, "LogFile");
            DataTable dTable = dataSet.Tables["LogFile"];

            DataRow dRow = dTable.Rows[dTable.Rows.Count - 1];
            strLastPassword = dRow["psword"].ToString();

            return strLastPassword;
        }

        public string FetchOfficerID(string username, string password)
        {
            string strOfficerID = "";

            SqlCommand cmd = con.cmd();
            cmd.CommandText = "select officerID from Officer where username = '" + username + "' and psword = '" + password + "'";

            SqlDataAdapter dataAdapter = new SqlDataAdapter();
            dataAdapter.SelectCommand = cmd;
            DataSet dataSet = new DataSet();

            dataAdapter.Fill(dataSet, "Officer");
            DataTable dTable = dataSet.Tables["Officer"];

            DataRow dRow = dTable.Rows[dTable.Rows.Count - 1];
            strOfficerID = dRow["officerID"].ToString();

            return strOfficerID;
        }

        public string FetchOfficerName(string officerID)
        {
            string strOfficerName = "";
            string strSurname = "";
            string strOtherNames = "";

            SqlCommand cmd = con.cmd();
            cmd.CommandText = "select surname, otherNames from Officer where officerID = " + officerID + "";

            SqlDataAdapter dataAdapter = new SqlDataAdapter();
            dataAdapter.SelectCommand = cmd;
            DataSet dataSet = new DataSet();

            dataAdapter.Fill(dataSet, "Officer");
            DataTable dTable = dataSet.Tables["Officer"];

            DataRow dRow = dTable.Rows[dTable.Rows.Count - 1];
            strSurname = dRow["surname"].ToString();
            strOtherNames = dRow["otherNames"].ToString();
            strOfficerName = strSurname + " " + strOtherNames;

            return strOfficerName;
        }

        public void LoadFormDetails(TextBox txtReturnCode, TextBox txtReceivedBy)
        {
            //Fetch last two digits of the year and append to the end of serial number and request code
            string year = (DateTime.Now.Year).ToString();
            year = year.Substring(2, 2);

            //Fetch last username and password
            string username = FetchLastUsername();
            string password = FetchLastPassword();

            //Get last officer ID from LogFile
            string officerID = FetchOfficerID(username, password);

            //Get Officer name using officer ID
            string officerName = FetchOfficerName(officerID);

            //Fetch last return code from the database
            string returnCode = (Int32.Parse(FetchLastReturnCode()) + 1).ToString();
            returnCode = returnCode + "/" + year;

            //Bind data from database to controls on the form
            txtReceivedBy.Text = officerName;
            txtReturnCode.Text = returnCode;
        }

        public string FetchLastSerialID()
        {
            string strSerialID = "";

            SqlCommand cmd = con.cmd();
            cmd.CommandText = "select * from SerialCodeLog";

            SqlDataAdapter dataAdapter = new SqlDataAdapter();
            dataAdapter.SelectCommand = cmd;
            DataSet dataSet = new DataSet();

            dataAdapter.Fill(dataSet, "SerialCodeLog");
            DataTable dTable = dataSet.Tables["SerialCodeLog"];

            DataRow dRow = dTable.Rows[dTable.Rows.Count - 1];
            strSerialID = dRow["serialID"].ToString();

            return strSerialID;
        }

        public void MakeFolderAvailable(ListView lv)
        {
            string folderID = lv.SelectedItems[0].SubItems[0].Text;
            int intFolderID = Int32.Parse(folderID);
            string strAvailStatus = "AVAILABLE";

            SqlCommand cmd = con.cmd();
            cmd.CommandText = "EXECUTE MakeFolderAvailable @folderID, @availStatus";

            cmd.Parameters.Add("@folderID", SqlDbType.Int).Value = intFolderID;
            cmd.Parameters.Add("@availStatus", SqlDbType.VarChar, 20).Value = strAvailStatus;

            cmd.ExecuteNonQuery();
            con.connectionClose();
        }

        public void FillUIControls(ListView lv, TextBox txtFolderName, TextBox txtReturnedBy, RadioButton[] radioButtons)
        {
            txtFolderName.Text = lv.SelectedItems[0].SubItems[2].Text;
            txtReturnedBy.Text = lv.SelectedItems[0].SubItems[5].Text;

            string unit = lv.SelectedItems[0].SubItems[4].Text;

            foreach (RadioButton radioButton in radioButtons)
            {
                if (unit.Contains(radioButton.Text))
                {
                    radioButton.Checked = true;
                }
            }
        }

        public void SearchFolderByName(ListView lv, TextBox txtSearchBox)
        {
            string search = txtSearchBox.Text;

            SqlCommand cmd = con.cmd();
            string strSQL = "SELECT * FROM Requisition where tradersName like '" + search + "%' and returningOfficer is null";

            lv.Clear();
            lv.Columns.Add("ID", 30, HorizontalAlignment.Left);
            lv.Columns.Add("TIN", 70, HorizontalAlignment.Left);
            lv.Columns.Add("Folder Name", 300, HorizontalAlignment.Left);
            lv.Columns.Add("Merge", 100, HorizontalAlignment.Left);
            lv.Columns.Add("Unit/Sub-Unit", 150, HorizontalAlignment.Left);
            lv.Columns.Add("Collected By", 150, HorizontalAlignment.Left);

            cmd.CommandText = strSQL;

            SqlDataAdapter dataAdapter = new SqlDataAdapter();
            dataAdapter.SelectCommand = cmd;

            DataSet dataSet = new DataSet();
            dataAdapter.Fill(dataSet, "Requisition");

            DataTable dataTable = dataSet.Tables["Requisition"];

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow dataRow = dataTable.Rows[i];

                if (dataRow.RowState != DataRowState.Deleted)
                {
                    ListViewItem lvi = new ListViewItem(dataRow["requestID"].ToString(), 0);
                    lvi.SubItems.Add(dataRow["tin"].ToString());
                    lvi.SubItems.Add(dataRow["tradersName"].ToString());
                    lvi.SubItems.Add(dataRow["mergedStatus"].ToString());

                    if (RowExists(dataRow["tradersName"].ToString()))
                    {
                        lvi.SubItems.Add(FetchUnitUsingFolderName(dataRow["tradersName"].ToString()));
                        lvi.SubItems.Add(FetchCollectedByUsingFolderName(dataRow["tradersName"].ToString()));
                    }

                    lv.Items.Add(lvi);
                }
            }
        }

        public void SearchFolderByTin(ListView lv, TextBox txtSearchBox)
        {
            string search = txtSearchBox.Text;

            SqlCommand cmd = con.cmd();
            string strSQL = "SELECT * FROM Requisition where tin like '" + search + "%' and returningOfficer is null";

            lv.Clear();
            lv.Columns.Add("ID", 30, HorizontalAlignment.Left);
            lv.Columns.Add("TIN", 70, HorizontalAlignment.Left);
            lv.Columns.Add("Folder Name", 300, HorizontalAlignment.Left);
            lv.Columns.Add("Merge", 100, HorizontalAlignment.Left);
            lv.Columns.Add("Unit/Sub-Unit", 150, HorizontalAlignment.Left);
            lv.Columns.Add("Collected By", 150, HorizontalAlignment.Left);

            cmd.CommandText = strSQL;

            SqlDataAdapter dataAdapter = new SqlDataAdapter();
            dataAdapter.SelectCommand = cmd;

            DataSet dataSet = new DataSet();
            dataAdapter.Fill(dataSet, "Requisition");

            DataTable dataTable = dataSet.Tables["Requisition"];

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow dataRow = dataTable.Rows[i];

                if (dataRow.RowState != DataRowState.Deleted)
                {
                    ListViewItem lvi = new ListViewItem(dataRow["requestID"].ToString(), 0);
                    lvi.SubItems.Add(dataRow["tin"].ToString());
                    lvi.SubItems.Add(dataRow["tradersName"].ToString());
                    lvi.SubItems.Add(dataRow["mergedStatus"].ToString());

                    if (RowExists(dataRow["tradersName"].ToString()))
                    {
                        lvi.SubItems.Add(FetchUnitUsingFolderName(dataRow["tradersName"].ToString()));
                        lvi.SubItems.Add(FetchCollectedByUsingFolderName(dataRow["tradersName"].ToString()));
                    }

                    lv.Items.Add(lvi);
                }
            }
        }

        public void ReturnFolder(string returnCode, string dateReturned, string receivedBy, string folderName, 
            string returningOfficer, string receivingOfficer, ListView lv )
        {
            string strReturnCode = returnCode;
            string strDateReturned = dateReturned;
            string strReceivedBy = receivedBy;
            string strFolderName = folderName;
            string strReturningOfficer = returningOfficer;
            string strReceivingOfficerRet = receivingOfficer;

            SqlCommand cmd = con.cmd();

            cmd.CommandText = "EXECUTE ReturnFolder @returnCode, @dateReturned, @receivedBy, " +
                              "@tradersName, @returningOfficer, @receivingOfficerRet";

            cmd.Parameters.Add("@returnCode", SqlDbType.VarChar, 10).Value = strReturnCode;
            cmd.Parameters.Add("@dateReturned", SqlDbType.Date).Value = strDateReturned;
            cmd.Parameters.Add("@receivedBy", SqlDbType.VarChar, 100).Value = strReceivedBy;
            cmd.Parameters.Add("@tradersName", SqlDbType.VarChar, 100).Value = strFolderName;
            cmd.Parameters.Add("@returningOfficer", SqlDbType.VarChar, 100).Value = strReturningOfficer;
            cmd.Parameters.Add("@receivingOfficerRet", SqlDbType.VarChar, 100).Value = strReceivingOfficerRet;
            

            cmd.ExecuteNonQuery();
            con.connectionClose();

            //Change Availability status of the folder to Unavailable
            MakeFolderAvailable(lv);

            //Update the SerialCodeLog table
            //Fetch last serial ID and return code from the database
            string serialID = FetchLastSerialID();
            string strreturnCode = (Int32.Parse(FetchLastReturnCode()) + 1).ToString();

            //Update with new number
            ModifyReturnCodeLog(serialID, strreturnCode);
        }

        public void ModifyReturnCodeLog(string serialID, string returnCode)
        {
            int intSerialID = Int32.Parse(serialID);
            int intReturnCode = Int32.Parse(returnCode);

            SqlCommand cmd = con.cmd();

            cmd.CommandText = "EXECUTE UpdateReturnCodeLog @serialID, @returnCode";

            cmd.Parameters.Add("@serialID", SqlDbType.Int).Value = intSerialID;
            cmd.Parameters.Add("@returnCode", SqlDbType.Int).Value = intReturnCode;

            cmd.ExecuteNonQuery();
            con.connectionClose();
        }

        public void LoadComboBoxByPosition(string position, string unit, ComboBox cb)
        {
            string strSurname;
            string strOtherNames;
            string strFullName;

            SqlCommand cmd = con.cmd();

            if (unit.Length != 0)
                cmd.CommandText = "select surname, otherNames from Officer where position like '%" + position + "%' and unit like '%" + unit + "%'";
            else
                cmd.CommandText = "select surname, otherNames from Officer where position like '%" + position + "%'";

            SqlDataAdapter dataAdapter = new SqlDataAdapter();
            dataAdapter.SelectCommand = cmd;
            DataSet dataSet = new DataSet();

            dataAdapter.Fill(dataSet, "Officer");
            DataTable dTable = dataSet.Tables["Officer"];

            int rowCount = dTable.Rows.Count;

            if (rowCount > 0)
            {
                for (int i = 0; i < dTable.Rows.Count; i++)
                {
                    DataRow dRow = dTable.Rows[i];
                    strSurname = dRow["surname"].ToString();
                    strOtherNames = dRow["OtherNames"].ToString();
                    strFullName = strSurname + " " + strOtherNames;
                    cb.Items.Add(strFullName);
                }
            }


        }

        public void LoadComboBoxByUnit(string unit, ComboBox cb)
        {
            string strSurname;
            string strOtherNames;
            string strFullName;

            SqlCommand cmd = con.cmd();
            cmd.CommandText = "select surname, otherNames from Officer where position <> 'NATIONAL SERVICE PERSONNEL' and " +
                              "position <> 'INTERNS' and unit like '%" + unit + "%'";

            SqlDataAdapter dataAdapter = new SqlDataAdapter();
            dataAdapter.SelectCommand = cmd;
            DataSet dataSet = new DataSet();

            dataAdapter.Fill(dataSet, "Officer");
            DataTable dTable = dataSet.Tables["Officer"];

            int rowCount = dTable.Rows.Count;

            if (rowCount > 0)
            {
                for (int i = 0; i < dTable.Rows.Count; i++)
                {
                    DataRow dRow = dTable.Rows[i];
                    strSurname = dRow["surname"].ToString();
                    strOtherNames = dRow["OtherNames"].ToString();
                    strFullName = strSurname + " " + strOtherNames;
                    cb.Items.Add(strFullName);
                }
            }
        }
    }
}
