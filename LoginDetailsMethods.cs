using System;
using System.Data;
using System.Data.SqlClient;
using CrystalDecisions.Windows.Forms;

namespace CFO_Requisition_System
{
    class LoginDetailsMethods
    {
        //Object of the connection class (A connection to the database)
        Connection con = new Connection();

        public void LoadLogBook(CrystalReportViewer rptViewer, string beginDate, string endDate )
        {
            String strBeginDate = beginDate;
            String strEndDate = endDate;

            SqlCommand cmd = con.cmd();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = new SqlCommand();
            da.SelectCommand = cmd;

            cmd.CommandText = "EXECUTE repLoginDetailsBook @beginDate, @endDate";

            cmd.Parameters.Add("@beginDate", SqlDbType.Date).Value = strBeginDate;
            cmd.Parameters.Add("@endDate", SqlDbType.Date).Value = strEndDate;

            DataSet dataSet = new DataSet();
            da.Fill(dataSet, "LogBookDataTable");
            rptLogBook rpt = new rptLogBook();
            rpt.SetDataSource(dataSet);

            rptViewer.ReportSource = rpt;
        }

        public bool RowExists(string beginDate, string endDate)
        {
            String strBeginDate = beginDate;
            String strEndDate = endDate;

            SqlCommand cmd = con.cmd();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = new SqlCommand();
            da.SelectCommand = cmd;

            cmd.CommandText = "EXECUTE repLoginDetailsBook @beginDate, @endDate";

            cmd.Parameters.Add("@beginDate", SqlDbType.Date).Value = strBeginDate;
            cmd.Parameters.Add("@endDate", SqlDbType.Date).Value = strEndDate;

            DataSet dataSet = new DataSet();
            da.Fill(dataSet, "LogFile");
            DataTable dTable = dataSet.Tables["LogFile"];

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
