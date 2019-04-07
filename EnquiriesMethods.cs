using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CFO_Requisition_System
{
    class EnquiriesMethods
    {
        //Object of the connection class (A connection to the database)
        Connection con = new Connection();

        public void LoadFolderDetails(ListView lv)
        {
            lv.Clear();
            lv.Columns.Add("Folder Name", 200, HorizontalAlignment.Left);
            lv.Columns.Add("TIN", 100, HorizontalAlignment.Left);
            lv.Columns.Add("Date Collected", 70, HorizontalAlignment.Left);
            lv.Columns.Add("Issued By", 140, HorizontalAlignment.Left);
            lv.Columns.Add("Merge", 80, HorizontalAlignment.Left);
            lv.Columns.Add("Request Code", 50, HorizontalAlignment.Left);
            lv.Columns.Add("Serial Number", 50, HorizontalAlignment.Left);
            lv.Columns.Add("Collected By", 140, HorizontalAlignment.Left);
            lv.Columns.Add("Unit", 100, HorizontalAlignment.Left);
            lv.Columns.Add("Purpose", 150, HorizontalAlignment.Left);
            lv.Columns.Add("Date Returned", 70, HorizontalAlignment.Left);
            lv.Columns.Add("Authorizing Officer", 140, HorizontalAlignment.Left);
            lv.Columns.Add("Receiving Officer", 140, HorizontalAlignment.Left);
            lv.Columns.Add("Head of TPS", 140, HorizontalAlignment.Left);
            lv.Columns.Add("ReceivedBy", 140, HorizontalAlignment.Left);
            lv.Columns.Add("ReturningOfficer", 140, HorizontalAlignment.Left);
            lv.Columns.Add("ReceivingOfficer", 140, HorizontalAlignment.Left);
            lv.Columns.Add("Return Code", 50, HorizontalAlignment.Left);


            string strSQL = "SELECT * FROM Requisition";

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
                    ListViewItem lvi = new ListViewItem(dataRow["tradersName"].ToString(), 0);
                    lvi.SubItems.Add(dataRow["tin"].ToString());
                    DateTime date2 = DateTime.Parse(dataRow["dateCollected"].ToString());
                    string date = date2.ToShortDateString();
                    lvi.SubItems.Add(date);

                    lvi.SubItems.Add(dataRow["issuedBy"].ToString());
                    lvi.SubItems.Add(dataRow["mergedStatus"].ToString());
                    lvi.SubItems.Add(dataRow["requestCode"].ToString());
                    lvi.SubItems.Add(dataRow["serialNumber"].ToString());
                    lvi.SubItems.Add(dataRow["collectedBy"].ToString());
                    lvi.SubItems.Add(dataRow["unit"].ToString());
                    lvi.SubItems.Add(dataRow["purpose"].ToString());

                    try
                    {
                        date2 = DateTime.Parse(dataRow["dateReturned"].ToString());
                        date = date2.ToShortDateString();
                    }
                    catch (FormatException e)
                    {
                        date = "";
                    }
                    
                    lvi.SubItems.Add(date);

                    lvi.SubItems.Add(dataRow["authorizingOfficer"].ToString());
                    lvi.SubItems.Add(dataRow["receivingOfficerReq"].ToString());
                    lvi.SubItems.Add(dataRow["headOfTPS"].ToString());
                    lvi.SubItems.Add(dataRow["receivedBy"].ToString());
                    lvi.SubItems.Add(dataRow["returningOfficer"].ToString());
                    lvi.SubItems.Add(dataRow["receivingOfficerRet"].ToString());
                    lvi.SubItems.Add(dataRow["returnCode"].ToString());

                    lv.Items.Add(lvi);
                }
            }

        }

        public void SearchRequisitionByIssuedBy(ListView lv, TextBox txtSearchBox, RadioButton notReturned, RadioButton returned)
        {
            string search = txtSearchBox.Text.Trim();

            SqlCommand cmd = con.cmd();
            string strSQL;

            if (notReturned.Checked)
                strSQL = "SELECT * FROM Requisition where issuedBy like '" + search + "%' and dateReturned is null";

            else if (returned.Checked)
                strSQL = "SELECT * FROM Requisition where issuedBy like '" + search + "%' and dateReturned is not null";

            else
                strSQL = "SELECT * FROM Requisition where issuedBy like '" + search + "%'";

            lv.Clear();
            lv.Columns.Add("Folder Name", 200, HorizontalAlignment.Left);
            lv.Columns.Add("Date Collected", 70, HorizontalAlignment.Left);
            lv.Columns.Add("Issued By", 140, HorizontalAlignment.Left);
            lv.Columns.Add("Merge", 80, HorizontalAlignment.Left);
            lv.Columns.Add("Request Code", 50, HorizontalAlignment.Left);
            lv.Columns.Add("Serial Number", 50, HorizontalAlignment.Left);
            lv.Columns.Add("Collected By", 140, HorizontalAlignment.Left);
            lv.Columns.Add("Unit", 100, HorizontalAlignment.Left);
            lv.Columns.Add("Purpose", 150, HorizontalAlignment.Left);
            lv.Columns.Add("Date Returned", 70, HorizontalAlignment.Left);
            lv.Columns.Add("Authorizing Officer", 140, HorizontalAlignment.Left);
            lv.Columns.Add("Receiving Officer", 140, HorizontalAlignment.Left);
            lv.Columns.Add("Head of TPS", 140, HorizontalAlignment.Left);
            lv.Columns.Add("ReceivedBy", 140, HorizontalAlignment.Left);
            lv.Columns.Add("ReturningOfficer", 140, HorizontalAlignment.Left);
            lv.Columns.Add("ReceivingOfficer", 140, HorizontalAlignment.Left);
            lv.Columns.Add("Return Code", 50, HorizontalAlignment.Left);

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
                    ListViewItem lvi = new ListViewItem(dataRow["tradersName"].ToString(), 0);
                    DateTime date2 = DateTime.Parse(dataRow["dateCollected"].ToString());
                    string date = date2.ToShortDateString();
                    lvi.SubItems.Add(date);

                    lvi.SubItems.Add(dataRow["issuedBy"].ToString());
                    lvi.SubItems.Add(dataRow["mergedStatus"].ToString());
                    lvi.SubItems.Add(dataRow["requestCode"].ToString());
                    lvi.SubItems.Add(dataRow["serialNumber"].ToString());
                    lvi.SubItems.Add(dataRow["collectedBy"].ToString());
                    lvi.SubItems.Add(dataRow["unit"].ToString());
                    lvi.SubItems.Add(dataRow["purpose"].ToString());

                    try
                    {
                        date2 = DateTime.Parse(dataRow["dateReturned"].ToString());
                        date = date2.ToShortDateString();
                    }
                    catch (FormatException e)
                    {
                        date = "";
                    }
                    lvi.SubItems.Add(date);

                    lvi.SubItems.Add(dataRow["authorizingOfficer"].ToString());
                    lvi.SubItems.Add(dataRow["receivingOfficerReq"].ToString());
                    lvi.SubItems.Add(dataRow["headOfTPS"].ToString());
                    lvi.SubItems.Add(dataRow["receivedBy"].ToString());
                    lvi.SubItems.Add(dataRow["returningOfficer"].ToString());
                    lvi.SubItems.Add(dataRow["receivingOfficerRet"].ToString());
                    lvi.SubItems.Add(dataRow["returnCode"].ToString());

                    lv.Items.Add(lvi);
                }
            }
        }

        public void SearchRequisitionByCollectedBy(ListView lv, TextBox txtSearchBox, RadioButton notReturned, RadioButton returned)
        {
            string search = txtSearchBox.Text.Trim();

            SqlCommand cmd = con.cmd();
            string strSQL;

            if (notReturned.Checked)
                strSQL = "SELECT * FROM Requisition where collectedBy like '" + search + "%' and dateReturned is null";

            else if (returned.Checked)
                strSQL = "SELECT * FROM Requisition where collectedBy like '" + search + "%' and dateReturned is not null";

            else
                strSQL = "SELECT * FROM Requisition where collectedBy like '" + search + "%'";

            lv.Clear();
            lv.Columns.Add("Folder Name", 200, HorizontalAlignment.Left);
            lv.Columns.Add("Date Collected", 70, HorizontalAlignment.Left);
            lv.Columns.Add("Issued By", 140, HorizontalAlignment.Left);
            lv.Columns.Add("Merge", 80, HorizontalAlignment.Left);
            lv.Columns.Add("Request Code", 50, HorizontalAlignment.Left);
            lv.Columns.Add("Serial Number", 50, HorizontalAlignment.Left);
            lv.Columns.Add("Collected By", 140, HorizontalAlignment.Left);
            lv.Columns.Add("Unit", 100, HorizontalAlignment.Left);
            lv.Columns.Add("Purpose", 150, HorizontalAlignment.Left);
            lv.Columns.Add("Date Returned", 70, HorizontalAlignment.Left);
            lv.Columns.Add("Authorizing Officer", 140, HorizontalAlignment.Left);
            lv.Columns.Add("Receiving Officer", 140, HorizontalAlignment.Left);
            lv.Columns.Add("Head of TPS", 140, HorizontalAlignment.Left);
            lv.Columns.Add("ReceivedBy", 140, HorizontalAlignment.Left);
            lv.Columns.Add("ReturningOfficer", 140, HorizontalAlignment.Left);
            lv.Columns.Add("ReceivingOfficer", 140, HorizontalAlignment.Left);
            lv.Columns.Add("Return Code", 50, HorizontalAlignment.Left);

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
                    ListViewItem lvi = new ListViewItem(dataRow["tradersName"].ToString(), 0);
                    DateTime date2 = DateTime.Parse(dataRow["dateCollected"].ToString());
                    string date = date2.ToShortDateString();
                    lvi.SubItems.Add(date);

                    lvi.SubItems.Add(dataRow["issuedBy"].ToString());
                    lvi.SubItems.Add(dataRow["mergedStatus"].ToString());
                    lvi.SubItems.Add(dataRow["requestCode"].ToString());
                    lvi.SubItems.Add(dataRow["serialNumber"].ToString());
                    lvi.SubItems.Add(dataRow["collectedBy"].ToString());
                    lvi.SubItems.Add(dataRow["unit"].ToString());
                    lvi.SubItems.Add(dataRow["purpose"].ToString());

                    try
                    {
                        date2 = DateTime.Parse(dataRow["dateReturned"].ToString());
                        date = date2.ToShortDateString();
                    }
                    catch (FormatException e)
                    {
                        date = "";
                    }
                    lvi.SubItems.Add(date);

                    lvi.SubItems.Add(dataRow["authorizingOfficer"].ToString());
                    lvi.SubItems.Add(dataRow["receivingOfficerReq"].ToString());
                    lvi.SubItems.Add(dataRow["headOfTPS"].ToString());
                    lvi.SubItems.Add(dataRow["receivedBy"].ToString());
                    lvi.SubItems.Add(dataRow["returningOfficer"].ToString());
                    lvi.SubItems.Add(dataRow["receivingOfficerRet"].ToString());
                    lvi.SubItems.Add(dataRow["returnCode"].ToString());

                    lv.Items.Add(lvi);
                }
            }
        }

        public void SearchRequisitionByFolderName(ListView lv, TextBox txtSearchBox, RadioButton notReturned, RadioButton returned)
        {
            string search = txtSearchBox.Text.Trim();

            SqlCommand cmd = con.cmd();
            string strSQL;

            if (notReturned.Checked)
                strSQL = "SELECT * FROM Requisition where tradersName like '" + search + "%' and dateReturned is null";

            else if (returned.Checked)
                strSQL = "SELECT * FROM Requisition where tradersName like '" + search + "%' and dateReturned is not null";

            else
                strSQL = "SELECT * FROM Requisition where tradersName like '" + search + "%'";

            lv.Clear();
            lv.Columns.Add("Folder Name", 200, HorizontalAlignment.Left);
            lv.Columns.Add("Date Collected", 70, HorizontalAlignment.Left);
            lv.Columns.Add("Issued By", 140, HorizontalAlignment.Left);
            lv.Columns.Add("Merge", 80, HorizontalAlignment.Left);
            lv.Columns.Add("Request Code", 50, HorizontalAlignment.Left);
            lv.Columns.Add("Serial Number", 50, HorizontalAlignment.Left);
            lv.Columns.Add("Collected By", 140, HorizontalAlignment.Left);
            lv.Columns.Add("Unit", 100, HorizontalAlignment.Left);
            lv.Columns.Add("Purpose", 150, HorizontalAlignment.Left);
            lv.Columns.Add("Date Returned", 70, HorizontalAlignment.Left);
            lv.Columns.Add("Authorizing Officer", 140, HorizontalAlignment.Left);
            lv.Columns.Add("Receiving Officer", 140, HorizontalAlignment.Left);
            lv.Columns.Add("Head of TPS", 140, HorizontalAlignment.Left);
            lv.Columns.Add("ReceivedBy", 140, HorizontalAlignment.Left);
            lv.Columns.Add("ReturningOfficer", 140, HorizontalAlignment.Left);
            lv.Columns.Add("ReceivingOfficer", 140, HorizontalAlignment.Left);
            lv.Columns.Add("Return Code", 50, HorizontalAlignment.Left);

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
                    ListViewItem lvi = new ListViewItem(dataRow["tradersName"].ToString(), 0);
                    DateTime date2 = DateTime.Parse(dataRow["dateCollected"].ToString());
                    string date = date2.ToShortDateString();
                    lvi.SubItems.Add(date);

                    lvi.SubItems.Add(dataRow["issuedBy"].ToString());
                    lvi.SubItems.Add(dataRow["mergedStatus"].ToString());
                    lvi.SubItems.Add(dataRow["requestCode"].ToString());
                    lvi.SubItems.Add(dataRow["serialNumber"].ToString());
                    lvi.SubItems.Add(dataRow["collectedBy"].ToString());
                    lvi.SubItems.Add(dataRow["unit"].ToString());
                    lvi.SubItems.Add(dataRow["purpose"].ToString());

                    try
                    {
                        date2 = DateTime.Parse(dataRow["dateReturned"].ToString());
                        date = date2.ToShortDateString();
                    }
                    catch (FormatException e)
                    {
                        date = "";
                    }
                    lvi.SubItems.Add(date);

                    lvi.SubItems.Add(dataRow["authorizingOfficer"].ToString());
                    lvi.SubItems.Add(dataRow["receivingOfficerReq"].ToString());
                    lvi.SubItems.Add(dataRow["headOfTPS"].ToString());
                    lvi.SubItems.Add(dataRow["receivedBy"].ToString());
                    lvi.SubItems.Add(dataRow["returningOfficer"].ToString());
                    lvi.SubItems.Add(dataRow["receivingOfficerRet"].ToString());
                    lvi.SubItems.Add(dataRow["returnCode"].ToString());

                    lv.Items.Add(lvi);
                }
            }
        }

        public void SearchRequisitionByReceivedBy(ListView lv, TextBox txtSearchBox, RadioButton notReturned, RadioButton returned)
        {
            string search = txtSearchBox.Text.Trim();

            SqlCommand cmd = con.cmd();
            string strSQL;

            if (notReturned.Checked)
                strSQL = "SELECT * FROM Requisition where receivedBy like '" + search + "%' and dateReturned is null";

            else if (returned.Checked)
                strSQL = "SELECT * FROM Requisition where receivedBy like '" + search + "%' and dateReturned is not null";

            else
                strSQL = "SELECT * FROM Requisition where receivedBy like '" + search + "%'";

            lv.Clear();
            lv.Columns.Add("Folder Name", 200, HorizontalAlignment.Left);
            lv.Columns.Add("Date Collected", 70, HorizontalAlignment.Left);
            lv.Columns.Add("Issued By", 140, HorizontalAlignment.Left);
            lv.Columns.Add("Merge", 80, HorizontalAlignment.Left);
            lv.Columns.Add("Request Code", 50, HorizontalAlignment.Left);
            lv.Columns.Add("Serial Number", 50, HorizontalAlignment.Left);
            lv.Columns.Add("Collected By", 140, HorizontalAlignment.Left);
            lv.Columns.Add("Unit", 100, HorizontalAlignment.Left);
            lv.Columns.Add("Purpose", 150, HorizontalAlignment.Left);
            lv.Columns.Add("Date Returned", 70, HorizontalAlignment.Left);
            lv.Columns.Add("Authorizing Officer", 140, HorizontalAlignment.Left);
            lv.Columns.Add("Receiving Officer", 140, HorizontalAlignment.Left);
            lv.Columns.Add("Head of TPS", 140, HorizontalAlignment.Left);
            lv.Columns.Add("ReceivedBy", 140, HorizontalAlignment.Left);
            lv.Columns.Add("ReturningOfficer", 140, HorizontalAlignment.Left);
            lv.Columns.Add("ReceivingOfficer", 140, HorizontalAlignment.Left);
            lv.Columns.Add("Return Code", 50, HorizontalAlignment.Left);

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
                    ListViewItem lvi = new ListViewItem(dataRow["tradersName"].ToString(), 0);
                    DateTime date2 = DateTime.Parse(dataRow["dateCollected"].ToString());
                    string date = date2.ToShortDateString();
                    lvi.SubItems.Add(date);

                    lvi.SubItems.Add(dataRow["issuedBy"].ToString());
                    lvi.SubItems.Add(dataRow["mergedStatus"].ToString());
                    lvi.SubItems.Add(dataRow["requestCode"].ToString());
                    lvi.SubItems.Add(dataRow["serialNumber"].ToString());
                    lvi.SubItems.Add(dataRow["collectedBy"].ToString());
                    lvi.SubItems.Add(dataRow["unit"].ToString());
                    lvi.SubItems.Add(dataRow["purpose"].ToString());

                    try
                    {
                        date2 = DateTime.Parse(dataRow["dateReturned"].ToString());
                        date = date2.ToShortDateString();
                    }
                    catch (FormatException e)
                    {
                        date = "";
                    }
                    lvi.SubItems.Add(date);

                    lvi.SubItems.Add(dataRow["authorizingOfficer"].ToString());
                    lvi.SubItems.Add(dataRow["receivingOfficerReq"].ToString());
                    lvi.SubItems.Add(dataRow["headOfTPS"].ToString());
                    lvi.SubItems.Add(dataRow["receivedBy"].ToString());
                    lvi.SubItems.Add(dataRow["returningOfficer"].ToString());
                    lvi.SubItems.Add(dataRow["receivingOfficerRet"].ToString());
                    lvi.SubItems.Add(dataRow["returnCode"].ToString());

                    lv.Items.Add(lvi);
                }
            }
        }

        public void SearchRequisitionByTin(ListView lv, TextBox txtSearchBox, RadioButton notReturned, RadioButton returned)
        {
            string search = txtSearchBox.Text.Trim();

            SqlCommand cmd = con.cmd();
            string strSQL;

            if (notReturned.Checked)
                strSQL = "SELECT * FROM Requisition where tin like '" + search + "%' and dateReturned is null";

            else if (returned.Checked)
                strSQL = "SELECT * FROM Requisition where tin like '" + search + "%' and dateReturned is not null";

            else
                strSQL = "SELECT * FROM Requisition where tin like '" + search + "%'";

            lv.Clear();
            lv.Columns.Add("Folder Name", 200, HorizontalAlignment.Left);
            lv.Columns.Add("TIN", 100, HorizontalAlignment.Left);
            lv.Columns.Add("Date Collected", 70, HorizontalAlignment.Left);
            lv.Columns.Add("Issued By", 140, HorizontalAlignment.Left);
            lv.Columns.Add("Merge", 80, HorizontalAlignment.Left);
            lv.Columns.Add("Request Code", 50, HorizontalAlignment.Left);
            lv.Columns.Add("Serial Number", 50, HorizontalAlignment.Left);
            lv.Columns.Add("Collected By", 140, HorizontalAlignment.Left);
            lv.Columns.Add("Unit", 100, HorizontalAlignment.Left);
            lv.Columns.Add("Purpose", 150, HorizontalAlignment.Left);
            lv.Columns.Add("Date Returned", 70, HorizontalAlignment.Left);
            lv.Columns.Add("Authorizing Officer", 140, HorizontalAlignment.Left);
            lv.Columns.Add("Receiving Officer", 140, HorizontalAlignment.Left);
            lv.Columns.Add("Head of TPS", 140, HorizontalAlignment.Left);
            lv.Columns.Add("ReceivedBy", 140, HorizontalAlignment.Left);
            lv.Columns.Add("ReturningOfficer", 140, HorizontalAlignment.Left);
            lv.Columns.Add("ReceivingOfficer", 140, HorizontalAlignment.Left);
            lv.Columns.Add("Return Code", 50, HorizontalAlignment.Left);

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
                    ListViewItem lvi = new ListViewItem(dataRow["tradersName"].ToString(), 0);
                    lvi.SubItems.Add(dataRow["tin"].ToString());
                    DateTime date2 = DateTime.Parse(dataRow["dateCollected"].ToString());
                    string date = date2.ToShortDateString();
                    lvi.SubItems.Add(date);

                    lvi.SubItems.Add(dataRow["issuedBy"].ToString());
                    lvi.SubItems.Add(dataRow["mergedStatus"].ToString());
                    lvi.SubItems.Add(dataRow["requestCode"].ToString());
                    lvi.SubItems.Add(dataRow["serialNumber"].ToString());
                    lvi.SubItems.Add(dataRow["collectedBy"].ToString());
                    lvi.SubItems.Add(dataRow["unit"].ToString());
                    lvi.SubItems.Add(dataRow["purpose"].ToString());

                    try
                    {
                        date2 = DateTime.Parse(dataRow["dateReturned"].ToString());
                        date = date2.ToShortDateString();
                    }
                    catch (FormatException e)
                    {
                        date = "";
                    }
                    lvi.SubItems.Add(date);

                    lvi.SubItems.Add(dataRow["authorizingOfficer"].ToString());
                    lvi.SubItems.Add(dataRow["receivingOfficerReq"].ToString());
                    lvi.SubItems.Add(dataRow["headOfTPS"].ToString());
                    lvi.SubItems.Add(dataRow["receivedBy"].ToString());
                    lvi.SubItems.Add(dataRow["returningOfficer"].ToString());
                    lvi.SubItems.Add(dataRow["receivingOfficerRet"].ToString());
                    lvi.SubItems.Add(dataRow["returnCode"].ToString());

                    lv.Items.Add(lvi);
                }
            }
        }

        public void SearchRequisitionByReturned(ListView lv, RadioButton returned)
        {
            SqlCommand cmd = con.cmd();
            string strSQL;

            if (returned.Checked)
                strSQL = "SELECT * FROM Requisition where dateReturned is not null";

            else
                strSQL = "SELECT * FROM Requisition";

            lv.Clear();
            lv.Columns.Add("Folder Name", 200, HorizontalAlignment.Left);
            lv.Columns.Add("Date Collected", 70, HorizontalAlignment.Left);
            lv.Columns.Add("Issued By", 140, HorizontalAlignment.Left);
            lv.Columns.Add("Merge", 80, HorizontalAlignment.Left);
            lv.Columns.Add("Request Code", 50, HorizontalAlignment.Left);
            lv.Columns.Add("Serial Number", 50, HorizontalAlignment.Left);
            lv.Columns.Add("Collected By", 140, HorizontalAlignment.Left);
            lv.Columns.Add("Unit", 100, HorizontalAlignment.Left);
            lv.Columns.Add("Purpose", 150, HorizontalAlignment.Left);
            lv.Columns.Add("Date Returned", 70, HorizontalAlignment.Left);
            lv.Columns.Add("Authorizing Officer", 140, HorizontalAlignment.Left);
            lv.Columns.Add("Receiving Officer", 140, HorizontalAlignment.Left);
            lv.Columns.Add("Head of TPS", 140, HorizontalAlignment.Left);
            lv.Columns.Add("ReceivedBy", 140, HorizontalAlignment.Left);
            lv.Columns.Add("ReturningOfficer", 140, HorizontalAlignment.Left);
            lv.Columns.Add("ReceivingOfficer", 140, HorizontalAlignment.Left);
            lv.Columns.Add("Return Code", 50, HorizontalAlignment.Left);

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
                    ListViewItem lvi = new ListViewItem(dataRow["tradersName"].ToString(), 0);
                    DateTime date2 = DateTime.Parse(dataRow["dateCollected"].ToString());
                    string date = date2.ToShortDateString();
                    lvi.SubItems.Add(date);

                    lvi.SubItems.Add(dataRow["issuedBy"].ToString());
                    lvi.SubItems.Add(dataRow["mergedStatus"].ToString());
                    lvi.SubItems.Add(dataRow["requestCode"].ToString());
                    lvi.SubItems.Add(dataRow["serialNumber"].ToString());
                    lvi.SubItems.Add(dataRow["collectedBy"].ToString());
                    lvi.SubItems.Add(dataRow["unit"].ToString());
                    lvi.SubItems.Add(dataRow["purpose"].ToString());

                    try
                    {
                        date2 = DateTime.Parse(dataRow["dateReturned"].ToString());
                        date = date2.ToShortDateString();
                    }
                    catch (FormatException e)
                    {
                        date = "";
                    }
                    lvi.SubItems.Add(date);

                    lvi.SubItems.Add(dataRow["authorizingOfficer"].ToString());
                    lvi.SubItems.Add(dataRow["receivingOfficerReq"].ToString());
                    lvi.SubItems.Add(dataRow["headOfTPS"].ToString());
                    lvi.SubItems.Add(dataRow["receivedBy"].ToString());
                    lvi.SubItems.Add(dataRow["returningOfficer"].ToString());
                    lvi.SubItems.Add(dataRow["receivingOfficerRet"].ToString());
                    lvi.SubItems.Add(dataRow["returnCode"].ToString());

                    lv.Items.Add(lvi);
                }
            }
        }

        public void SearchRequisitionByNotReturned(ListView lv, RadioButton notReturned)
        {
            SqlCommand cmd = con.cmd();
            string strSQL;

            if (notReturned.Checked)
                strSQL = "SELECT * FROM Requisition where dateReturned is null";

            else
                strSQL = "SELECT * FROM Requisition";

            lv.Clear();
            lv.Columns.Add("Folder Name", 200, HorizontalAlignment.Left);
            lv.Columns.Add("Date Collected", 70, HorizontalAlignment.Left);
            lv.Columns.Add("Issued By", 140, HorizontalAlignment.Left);
            lv.Columns.Add("Merge", 80, HorizontalAlignment.Left);
            lv.Columns.Add("Request Code", 50, HorizontalAlignment.Left);
            lv.Columns.Add("Serial Number", 50, HorizontalAlignment.Left);
            lv.Columns.Add("Collected By", 140, HorizontalAlignment.Left);
            lv.Columns.Add("Unit", 100, HorizontalAlignment.Left);
            lv.Columns.Add("Purpose", 150, HorizontalAlignment.Left);
            lv.Columns.Add("Date Returned", 70, HorizontalAlignment.Left);
            lv.Columns.Add("Authorizing Officer", 140, HorizontalAlignment.Left);
            lv.Columns.Add("Receiving Officer", 140, HorizontalAlignment.Left);
            lv.Columns.Add("Head of TPS", 140, HorizontalAlignment.Left);
            lv.Columns.Add("ReceivedBy", 140, HorizontalAlignment.Left);
            lv.Columns.Add("ReturningOfficer", 140, HorizontalAlignment.Left);
            lv.Columns.Add("ReceivingOfficer", 140, HorizontalAlignment.Left);
            lv.Columns.Add("Return Code", 50, HorizontalAlignment.Left);

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
                    ListViewItem lvi = new ListViewItem(dataRow["tradersName"].ToString(), 0);
                    DateTime date2 = DateTime.Parse(dataRow["dateCollected"].ToString());
                    string date = date2.ToShortDateString();
                    lvi.SubItems.Add(date);

                    lvi.SubItems.Add(dataRow["issuedBy"].ToString());
                    lvi.SubItems.Add(dataRow["mergedStatus"].ToString());
                    lvi.SubItems.Add(dataRow["requestCode"].ToString());
                    lvi.SubItems.Add(dataRow["serialNumber"].ToString());
                    lvi.SubItems.Add(dataRow["collectedBy"].ToString());
                    lvi.SubItems.Add(dataRow["unit"].ToString());
                    lvi.SubItems.Add(dataRow["purpose"].ToString());

                    try
                    {
                        date2 = DateTime.Parse(dataRow["dateReturned"].ToString());
                        date = date2.ToShortDateString();
                    }
                    catch (FormatException e)
                    {
                        date = "";
                    }
                    lvi.SubItems.Add(date);

                    lvi.SubItems.Add(dataRow["authorizingOfficer"].ToString());
                    lvi.SubItems.Add(dataRow["receivingOfficerReq"].ToString());
                    lvi.SubItems.Add(dataRow["headOfTPS"].ToString());
                    lvi.SubItems.Add(dataRow["receivedBy"].ToString());
                    lvi.SubItems.Add(dataRow["returningOfficer"].ToString());
                    lvi.SubItems.Add(dataRow["receivingOfficerRet"].ToString());
                    lvi.SubItems.Add(dataRow["returnCode"].ToString());

                    lv.Items.Add(lvi);
                }
            }
        }
    }
}
