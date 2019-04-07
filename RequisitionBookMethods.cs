using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using CrystalDecisions.Windows.Forms;
using FlashControlV71;

namespace CFO_Requisition_System
{
    class RequisitionBookMethods
    {
        //Object of the connection class (A connection to the database)
        Connection con = new Connection();

        public void LoadRequisitionBook(CrystalReportViewer rptViewer, string beginDate, string endDate)
        {
            var strBeginDate = beginDate;
            var strEndDate = endDate;

            SqlCommand cmd = con.cmd();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = new SqlCommand();
            da.SelectCommand = cmd;

            cmd.CommandText = "EXECUTE repRequisitionBookUsingReqDate @beginDate, @endDate";

            cmd.Parameters.Add("@beginDate", SqlDbType.Date).Value = strBeginDate;
            cmd.Parameters.Add("@endDate", SqlDbType.Date).Value = strEndDate;

            DataSet dataSet = new DataSet();
            da.Fill(dataSet, "ReqBookDataTable");
            rptReqBook rpt = new rptReqBook();
            rpt.SetDataSource(dataSet);

            rptViewer.ReportSource = rpt;
        }

        public void LoadRequisitionBook(CrystalReportViewer rptViewer, string collectedBy)
        {
            var strCollectedBy = collectedBy;

            SqlCommand cmd = con.cmd();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = new SqlCommand();
            da.SelectCommand = cmd;

            cmd.CommandText = "EXECUTE repRequisitionBookUsingOfficer @collectedBy";

            cmd.Parameters.Add("@collectedBy", SqlDbType.VarChar, 100).Value = strCollectedBy;

            DataSet dataSet = new DataSet();
            da.Fill(dataSet, "ReqBookDataTable");
            rptReqBook rpt = new rptReqBook();
            rpt.SetDataSource(dataSet);

            rptViewer.ReportSource = rpt;
        }

        public void LoadComboBoxByUnitExceptCFO(string unit, string subUnit, ComboBox cb)
        {
            string strSurname;
            string strOtherNames;
            string strFullName;

            SqlCommand cmd = con.cmd();
            cmd.CommandText = "select surname, otherNames from Officer where position <> 'NATIONAL SERVICE PERSONNEL' and " +
                              "position <> 'INTERNS' and unit like '%" + unit + "%' and unit not like '%" + subUnit + "%'";

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

        //Method to find out whether a row exists for a query
        public bool RowExists(string beginDate, string endDate)
        {
            var strBeginDate = beginDate;
            var strEndDate = endDate;

            SqlCommand cmd = con.cmd();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = new SqlCommand();
            da.SelectCommand = cmd;

            cmd.CommandText = "EXECUTE repRequisitionBookUsingReqDate @beginDate, @endDate";

            cmd.Parameters.Add("@beginDate", SqlDbType.Date).Value = strBeginDate;
            cmd.Parameters.Add("@endDate", SqlDbType.Date).Value = strEndDate;

            DataSet dataSet = new DataSet();
            da.Fill(dataSet, "Requisition");
            DataTable dTable = dataSet.Tables["Requisition"];

            if (dTable.Rows.Count == 0)
                return false;

            return true;
        }

        public bool RowExists(string collectedBy)
        {
            var strCollectedBy = collectedBy;

            SqlCommand cmd = con.cmd();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = new SqlCommand();
            da.SelectCommand = cmd;

            cmd.CommandText = "EXECUTE repRequisitionBookUsingOfficer @collectedBy";

            cmd.Parameters.Add("@collectedBy", SqlDbType.VarChar, 100).Value = strCollectedBy;

            DataSet dataSet = new DataSet();
            da.Fill(dataSet, "Requisition");
            DataTable dTable = dataSet.Tables["Requisition"];

            if (dTable.Rows.Count == 0)
                return false;

            return true;
        }

        public void ClearCrystalReport(CrystalReportViewer rptMonthlyReport)
        {
            rptMonthlyReport.ShowCloseButton = false;
            rptMonthlyReport.ShowCopyButton = false;
            rptMonthlyReport.ShowExportButton = false;
            rptMonthlyReport.ShowGotoPageButton = false;
            rptMonthlyReport.ShowGroupTreeButton = false;
            rptMonthlyReport.ShowLogo = false;
            rptMonthlyReport.ShowParameterPanelButton = false;
            rptMonthlyReport.ShowRefreshButton = false;
            rptMonthlyReport.ShowTextSearchButton = false;
            rptMonthlyReport.ShowZoomButton = false;
        }

    }
}
