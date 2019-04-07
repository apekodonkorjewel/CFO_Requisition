using System;
using System.Data;
using System.Data.SqlClient;
using CrystalDecisions.Windows.Forms;

namespace CFO_Requisition_System
{
    class FileRequisitionMethods
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

        public int GetMonthNumber(string monthName)
        {
            int monthNumber = 0;

            if (monthName == "January" || monthName == "JANUARY")
                monthNumber = 1;
            else if (monthName == "February" || monthName == "FEBRUARY")
                monthNumber = 2;
            else if (monthName == "March" || monthName == "MARCH")
                monthNumber = 3;
            else if (monthName == "April" || monthName == "APRIL")
                monthNumber = 4;
            else if (monthName == "May" || monthName == "MAY")
                monthNumber = 5;
            else if (monthName == "June" || monthName == "JUNE")
                monthNumber = 6;
            else if (monthName == "July" || monthName == "JULY")
                monthNumber = 7;
            else if (monthName == "August" || monthName == "AUGUST")
                monthNumber = 8;
            else if (monthName == "September" || monthName == "SEPTEMBER")
                monthNumber = 9;
            else if (monthName == "October" || monthName == "OCTOBER")
                monthNumber = 10;
            else if (monthName == "November" || monthName == "NOVEMBER")
                monthNumber = 11;
            else
            {
                monthNumber = 12;
            }

            return monthNumber;
        }
        
        public int CountTotal(string auditUnit, string cedmUnit, string othersUnit,  string month, string year)
        {
            int count = 0;
            count = count += CountUnit(auditUnit, month, year);
            count = count += CountUnit(cedmUnit, month, year);
            count = count += CountUnit(othersUnit, month, year);

            return count;
        }

        public int CountPurpose(string purpose, string month, string year)
        {
            string strPurpose = purpose;
            string strMonth = month;
            string strYear = year;
            int intMonth = GetMonthNumber(strMonth);

            SqlCommand cmd = con.cmd();
            cmd.CommandText = "select * from Requisition where DATEPART(month, dateCollected) = " + intMonth + " and DATEPART(year, dateCollected) ='" + strYear + "' and purpose ='" + strPurpose + "'";

            SqlDataAdapter dataAdapter = new SqlDataAdapter();
            dataAdapter.SelectCommand = cmd;
            DataSet dataSet = new DataSet();

            dataAdapter.Fill(dataSet, "Requisition");
            DataTable dTable = dataSet.Tables["Requisition"];

            int count = dTable.Rows.Count;

            return count;
        }

        public int CountUnit(string unit, string month, string year)
        {
            string strUnit = unit;
            string strMonth = month;
            string strYear = year;
            int intMonth = GetMonthNumber(strMonth);

            SqlCommand cmd = con.cmd();
            cmd.CommandText = "select * from Requisition where DATEPART(month, dateCollected) = " + intMonth + " and DATEPART(year, dateCollected) ='" + strYear + "' and unit ='" + strUnit + "'";

            SqlDataAdapter dataAdapter = new SqlDataAdapter();
            dataAdapter.SelectCommand = cmd;
            DataSet dataSet = new DataSet();

            dataAdapter.Fill(dataSet, "Requisition");
            DataTable dTable = dataSet.Tables["Requisition"];

            int count = dTable.Rows.Count;

            return count;
        }

        public int CountPurposeUnit(string purpose, string unit, string month, string year)
        {
            string strUnit = unit;
            string strPurpose = purpose;
            string strMonth = month;
            string strYear = year;
            int intMonth = GetMonthNumber(strMonth);

            SqlCommand cmd = con.cmd();
            cmd.CommandText = "select * from Requisition where DATEPART(month, dateCollected) = " + intMonth + " and DATEPART(year, dateCollected) ='" + strYear + "' and purpose ='" + strPurpose + "' and unit = '" + strUnit +"'";

            SqlDataAdapter dataAdapter = new SqlDataAdapter();
            dataAdapter.SelectCommand = cmd;
            DataSet dataSet = new DataSet();

            dataAdapter.Fill(dataSet, "Requisition");
            DataTable dTable = dataSet.Tables["Requisition"];

            int count = dTable.Rows.Count;

            return count;
        }

        public void LoadMonthlyRequisitionForm(CrystalReportViewer rptViewer, string period)
        {
            //Fetch month of period
            DateTime dtPeriod = DateTime.Parse(period);
            string strMonth = dtPeriod.ToString("MMMM");

            //Fetch year of period
            string strYear = dtPeriod.Year.ToString();

            //Fetch all details required from Requisition table
            //Fetch TCC total
            int TCCtotal = CountPurpose("TAX CLEARANCE CERTIFICATE", strMonth, strYear);

            //Fetch REC total
            int RECtotal = CountPurpose("RECONCILIATION", strMonth, strYear);

            //Fetch LEDG total
            int LEDGtotal = CountPurpose("LEDGER CLEANING", strMonth, strYear);

            //Fetch TAXP total
            int TAXPtotal = CountPurpose("TAX POSITION", strMonth, strYear);

            //Fetch COMPR total
            int COMPRtotal = CountPurpose("COMPREHENSIVE AUDIT", strMonth, strYear);

            //Fetch COMPL total
            int COMPLtotal = CountPurpose("COMPLIANCE STATUS", strMonth, strYear);

            //Fetch INV total
            int INVtotal = CountPurpose("INVESTIGATION", strMonth, strYear);

            //Fetch OTHERS total
            int OTHERStotal = CountPurpose("OTHERS", strMonth, strYear);

            //Fetch total AUDIT
            int totalAUDIT = CountUnit("AUDIT", strMonth, strYear);

            //Fetch total CEDM
            int totalCEDM = CountUnit("CEDM", strMonth, strYear);

            //Fetch total OTHERS
            int totalOTHERS = CountUnit("TPS", strMonth, strYear);

            //Fetch total TOTAL
            int totalTOTAL = CountTotal("AUDIT", "CEDM", "TPS", strMonth, strYear);

            //Fetch tcc AUDIT
            int tccAUDIT = CountPurposeUnit("TAX CLEARANCE CERTIFICATE", "AUDIT", strMonth, strYear);

            //Fetch rec AUDIT
            int recAUDIT = CountPurposeUnit("RECONCILIATION", "AUDIT", strMonth, strYear);

            //Fetch ledg AUDIT
            int ledgAUDIT = CountPurposeUnit("LEDGER CLEANING", "AUDIT", strMonth, strYear);

            //Fetch taxp AUDIT
            int taxpAUDIT = CountPurposeUnit("TAX POSITION", "AUDIT", strMonth, strYear);

            //Fetch compr AUDIT
            int comprAUDIT = CountPurposeUnit("COMPREHENSIVE AUDIT", "AUDIT", strMonth, strYear);

            //Fetch compl AUDIT
            int complAUDIT = CountPurposeUnit("COMPLIANCE STATUS", "AUDIT", strMonth, strYear);

            //Fetch inv AUDIT
            int invAUDIT = CountPurposeUnit("INVESTIGATION", "AUDIT", strMonth, strYear);

            //Fetch others AUDIT
            int othersAUDIT = CountPurposeUnit("OTHERS", "AUDIT", strMonth, strYear);

            //Fetch tcc CEDM
            int tccCEDM = CountPurposeUnit("TAX CLEARANCE CERTIFICATE", "CEDM", strMonth, strYear);

            //Fetch rec CEDM
            int recCEDM = CountPurposeUnit("RECONCILIATION", "CEDM", strMonth, strYear);

            //Fetch ledg CEDM
            int ledgCEDM = CountPurposeUnit("LEDGER CLEANING", "CEDM", strMonth, strYear);

            //Fetch taxp CEDM
            int taxpCEDM = CountPurposeUnit("TAX POSITION", "CEDM", strMonth, strYear);

            //Fetch compr CEDM
            int comprCEDM = CountPurposeUnit("COMPREHENSIVE AUDIT", "CEDM", strMonth, strYear);

            //Fetch compl CEDM
            int complCEDM = CountPurposeUnit("COMPLIANCE STATUS", "CEDM", strMonth, strYear);

            //Fetch inv CEDM
            int invCEDM = CountPurposeUnit("INVESTIGATION", "CEDM", strMonth, strYear);

            //Fetch others CEDM
            int othersCEDM = CountPurposeUnit("OTHERS", "CEDM", strMonth, strYear);

            //Fetch tcc OTHERS
            int tccOTHERS = CountPurposeUnit("TAX CLEARANCE CERTIFICATE", "TPS", strMonth, strYear);

            //Fetch rec OTHERS
            int recOTHERS = CountPurposeUnit("RECONCILIATION", "TPS", strMonth, strYear);

            //Fetch ledg OTHERS
            int ledgOTHERS = CountPurposeUnit("LEDGER CLEANING", "TPS", strMonth, strYear);

            //Fetch taxp OTHERS
            int taxpOTHERS = CountPurposeUnit("TAX POSITION", "TPS", strMonth, strYear);

            //Fetch compr OTHERS
            int comprOTHERS = CountPurposeUnit("COMPREHENSIVE AUDIT", "TPS", strMonth, strYear);

            //Fetch compl OTHERS
            int complOTHERS = CountPurposeUnit("COMPLIANCE STATUS", "TPS", strMonth, strYear);

            //Fetch inv OTHERS
            int invOTHERS = CountPurposeUnit("INVESTIGATION", "TPS", strMonth, strYear);

            //Fetch others OTHERS
            int othersOTHERS = CountPurposeUnit("OTHERS", "TPS", strMonth, strYear);

            
            //Pass fetched data to the Monthly Requisition Form
            //Insert Monthly Requisition Form
            string strPeriod = strMonth + " " + strYear;
            int intReqID = GenerateID("MonthlyReqForm", "reqID");
            SqlCommand cmd = con.cmd();

            cmd.CommandText =   "EXECUTE InsertMonthlyReq @reqID, @period, @TCCtotal, @RECtotal, @LEDGtotal, @TAXPtotal, @COMPRtotal, " +
                                "@COMPLtotal, @INVtotal, @OTHERStotal, @totalAUDIT, @totalCEDM, @totalOTHERS, @tccAUDIT, @recAUDIT, " +
                                "@ledgAUDIT, @taxpAUDIT, @comprAUDIT, @complAUDIT, @invAUDIT, @othersAUDIT, @tccCEDM, @recCEDM, " +
                                "@ledgCEDM, @taxpCEDM, @comprCEDM, @complCEDM, @invCEDM, @othersCEDM, @tccOTHERS, @recOTHERS, " +
                                "@ledgOTHERS, @taxpOTHERS, @comprOTHERS, @complOTHERS, @invOTHERS, @othersOTHERS, @totalTOTAL";

            cmd.Parameters.Add("@reqID", SqlDbType.Int).Value = intReqID;
            cmd.Parameters.Add("@period", SqlDbType.VarChar, 20).Value = strPeriod;
            cmd.Parameters.Add("@TCCtotal", SqlDbType.Int).Value = TCCtotal;
            cmd.Parameters.Add("@RECtotal", SqlDbType.Int).Value = RECtotal;
            cmd.Parameters.Add("@LEDGtotal", SqlDbType.Int).Value = LEDGtotal;
            cmd.Parameters.Add("@TAXPtotal", SqlDbType.Int).Value = TAXPtotal;
            cmd.Parameters.Add("@COMPRtotal", SqlDbType.Int).Value = COMPRtotal;
            cmd.Parameters.Add("@COMPLtotal", SqlDbType.Int).Value = COMPLtotal;
            cmd.Parameters.Add("@INVtotal", SqlDbType.Int).Value = INVtotal;
            cmd.Parameters.Add("@OTHERStotal", SqlDbType.Int).Value = OTHERStotal;
            cmd.Parameters.Add("@totalAUDIT", SqlDbType.Int).Value = totalAUDIT;
            cmd.Parameters.Add("@totalCEDM", SqlDbType.Int).Value = totalCEDM;
            cmd.Parameters.Add("@totalOTHERS", SqlDbType.Int).Value = totalOTHERS;
            cmd.Parameters.Add("@tccAUDIT", SqlDbType.Int).Value = tccAUDIT;
            cmd.Parameters.Add("@recAUDIT", SqlDbType.Int).Value = recAUDIT;
            cmd.Parameters.Add("@ledgAUDIT", SqlDbType.Int).Value = ledgAUDIT;
            cmd.Parameters.Add("@taxpAUDIT", SqlDbType.Int).Value = taxpAUDIT;
            cmd.Parameters.Add("@comprAUDIT", SqlDbType.Int).Value = comprAUDIT;
            cmd.Parameters.Add("@complAUDIT", SqlDbType.Int).Value = complAUDIT;
            cmd.Parameters.Add("@invAUDIT", SqlDbType.Int).Value = invAUDIT;
            cmd.Parameters.Add("@othersAUDIT", SqlDbType.Int).Value = othersAUDIT;
            cmd.Parameters.Add("@tccCEDM", SqlDbType.Int).Value = tccCEDM;
            cmd.Parameters.Add("@recCEDM", SqlDbType.Int).Value = recCEDM;
            cmd.Parameters.Add("@ledgCEDM", SqlDbType.Int).Value = ledgCEDM;
            cmd.Parameters.Add("@taxpCEDM", SqlDbType.Int).Value = taxpCEDM;
            cmd.Parameters.Add("@comprCEDM", SqlDbType.Int).Value = comprCEDM;
            cmd.Parameters.Add("@complCEDM", SqlDbType.Int).Value = complCEDM;
            cmd.Parameters.Add("@invCEDM", SqlDbType.Int).Value = invCEDM;
            cmd.Parameters.Add("@othersCEDM", SqlDbType.Int).Value = othersCEDM;
            cmd.Parameters.Add("@tccOTHERS", SqlDbType.Int).Value = tccOTHERS;
            cmd.Parameters.Add("@recOTHERS", SqlDbType.Int).Value = recOTHERS;
            cmd.Parameters.Add("@ledgOTHERS", SqlDbType.Int).Value = ledgOTHERS;
            cmd.Parameters.Add("@taxpOTHERS", SqlDbType.Int).Value = taxpOTHERS;
            cmd.Parameters.Add("@comprOTHERS", SqlDbType.Int).Value = comprOTHERS;
            cmd.Parameters.Add("@complOTHERS", SqlDbType.Int).Value = complOTHERS;
            cmd.Parameters.Add("@invOTHERS", SqlDbType.Int).Value = invOTHERS;
            cmd.Parameters.Add("@othersOTHERS", SqlDbType.Int).Value = othersOTHERS;
            cmd.Parameters.Add("@totalTOTAL", SqlDbType.Int).Value = totalTOTAL;

            cmd.ExecuteNonQuery();
            con.connectionClose();

            //Fetch recent data (last row) into the datatable for the report
            SqlCommand cmdd = con.cmd();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = new SqlCommand();
            da.SelectCommand = cmdd;

            cmdd.CommandText = "EXECUTE repMonthlyReqForm @reqID";

            cmdd.Parameters.Add("@reqID", SqlDbType.Int).Value = intReqID;

            DataSet dataSet = new DataSet();
            da.Fill(dataSet, "dtMonthlyRepTable");
            rptMonthlyReqForm rpt = new rptMonthlyReqForm();
            rpt.SetDataSource(dataSet);

            rptViewer.ReportSource = rpt;
        }

        public bool RowExists(string period)
        {
            //Fetch month of period
            DateTime dtPeriod = DateTime.Parse(period);
            string strMonth = dtPeriod.ToString("MMMM");

            //Fetch year of period
            string strYear = dtPeriod.Year.ToString();

            //Fetch all details required from Requisition table
            //Fetch TCC total
            int TCCtotal = CountPurpose("TAX CLEARANCE CERTIFICATE", strMonth, strYear);

            //Fetch REC total
            int RECtotal = CountPurpose("RECONCILIATION", strMonth, strYear);

            //Fetch LEDG total
            int LEDGtotal = CountPurpose("LEDGER CLEANING", strMonth, strYear);

            //Fetch TAXP total
            int TAXPtotal = CountPurpose("TAX POSITION", strMonth, strYear);

            //Fetch COMPR total
            int COMPRtotal = CountPurpose("COMPREHENSIVE AUDIT", strMonth, strYear);

            //Fetch COMPL total
            int COMPLtotal = CountPurpose("COMPLIANCE STATUS", strMonth, strYear);

            //Fetch INV total
            int INVtotal = CountPurpose("INVESTIGATION", strMonth, strYear);

            //Fetch OTHERS total
            int OTHERStotal = CountPurpose("OTHERS", strMonth, strYear);

            //Fetch total AUDIT
            int totalAUDIT = CountUnit("AUDIT", strMonth, strYear);

            //Fetch total CEDM
            int totalCEDM = CountUnit("CEDM", strMonth, strYear);

            //Fetch total OTHERS
            int totalOTHERS = CountUnit("OTHERS", strMonth, strYear);

            //Fetch total TOTAL
            int totalTOTAL = CountTotal("AUDIT", "CEDM", "TPS", strMonth, strYear);

            //Fetch tcc AUDIT
            int tccAUDIT = CountPurposeUnit("TAX CLEARANCE CERTIFICATE", "AUDIT", strMonth, strYear);

            //Fetch rec AUDIT
            int recAUDIT = CountPurposeUnit("RECONCILIATION", "AUDIT", strMonth, strYear);

            //Fetch ledg AUDIT
            int ledgAUDIT = CountPurposeUnit("LEDGER CLEANING", "AUDIT", strMonth, strYear);

            //Fetch taxp AUDIT
            int taxpAUDIT = CountPurposeUnit("TAX POSITION", "AUDIT", strMonth, strYear);

            //Fetch compr AUDIT
            int comprAUDIT = CountPurposeUnit("COMPREHENSIVE AUDIT", "AUDIT", strMonth, strYear);

            //Fetch compl AUDIT
            int complAUDIT = CountPurposeUnit("COMPLIANCE STATUS", "AUDIT", strMonth, strYear);

            //Fetch inv AUDIT
            int invAUDIT = CountPurposeUnit("INVESTIGATION", "AUDIT", strMonth, strYear);

            //Fetch others AUDIT
            int othersAUDIT = CountPurposeUnit("OTHERS", "AUDIT", strMonth, strYear);

            //Fetch tcc CEDM
            int tccCEDM = CountPurposeUnit("TAX CLEARANCE CERTIFICATE", "CEDM", strMonth, strYear);

            //Fetch rec CEDM
            int recCEDM = CountPurposeUnit("RECONCILIATION", "CEDM", strMonth, strYear);

            //Fetch ledg CEDM
            int ledgCEDM = CountPurposeUnit("LEDGER CLEANING", "CEDM", strMonth, strYear);

            //Fetch taxp CEDM
            int taxpCEDM = CountPurposeUnit("TAX POSITION", "CEDM", strMonth, strYear);

            //Fetch compr CEDM
            int comprCEDM = CountPurposeUnit("COMPREHENSIVE AUDIT", "CEDM", strMonth, strYear);

            //Fetch compl CEDM
            int complCEDM = CountPurposeUnit("COMPLIANCE STATUS", "CEDM", strMonth, strYear);

            //Fetch inv CEDM
            int invCEDM = CountPurposeUnit("INVESTIGATION", "CEDM", strMonth, strYear);

            //Fetch others CEDM
            int othersCEDM = CountPurposeUnit("OTHERS", "CEDM", strMonth, strYear);

            //Fetch tcc OTHERS
            int tccOTHERS = CountPurposeUnit("TAX CLEARANCE CERTIFICATE", "TPS", strMonth, strYear);

            //Fetch rec OTHERS
            int recOTHERS = CountPurposeUnit("RECONCILIATION", "TPS", strMonth, strYear);

            //Fetch ledg OTHERS
            int ledgOTHERS = CountPurposeUnit("LEDGER CLEANING", "TPS", strMonth, strYear);

            //Fetch taxp OTHERS
            int taxpOTHERS = CountPurposeUnit("TAX POSITION", "TPS", strMonth, strYear);

            //Fetch compr OTHERS
            int comprOTHERS = CountPurposeUnit("COMPREHENSIVE AUDIT", "TPS", strMonth, strYear);

            //Fetch compl OTHERS
            int complOTHERS = CountPurposeUnit("COMPLIANCE STATUS", "TPS", strMonth, strYear);

            //Fetch inv OTHERS
            int invOTHERS = CountPurposeUnit("INVESTIGATION", "TPS", strMonth, strYear);

            //Fetch others OTHERS
            int othersOTHERS = CountPurposeUnit("OTHERS", "TPS", strMonth, strYear);


            //Pass fetched data to the Monthly Requisition Form
            //Insert Monthly Requisition Form
            string strPeriod = strMonth + " " + strYear;
            int intReqID = GenerateID("MonthlyReqForm", "reqID");
            SqlCommand cmd = con.cmd();

            cmd.CommandText = "EXECUTE InsertMonthlyReq @reqID, @period, @TCCtotal, @RECtotal, @LEDGtotal, @TAXPtotal, @COMPRtotal, " +
                                "@COMPLtotal, @INVtotal, @OTHERStotal, @totalAUDIT, @totalCEDM, @totalOTHERS, @tccAUDIT, @recAUDIT, " +
                                "@ledgAUDIT, @taxpAUDIT, @comprAUDIT, @complAUDIT, @invAUDIT, @othersAUDIT, @tccCEDM, @recCEDM, " +
                                "@ledgCEDM, @taxpCEDM, @comprCEDM, @complCEDM, @invCEDM, @othersCEDM, @tccOTHERS, @recOTHERS, " +
                                "@ledgOTHERS, @taxpOTHERS, @comprOTHERS, @complOTHERS, @invOTHERS, @othersOTHERS, @totalTOTAL";

            cmd.Parameters.Add("@reqID", SqlDbType.Int).Value = intReqID;
            cmd.Parameters.Add("@period", SqlDbType.VarChar, 20).Value = strPeriod;
            cmd.Parameters.Add("@TCCtotal", SqlDbType.Int).Value = TCCtotal;
            cmd.Parameters.Add("@RECtotal", SqlDbType.Int).Value = RECtotal;
            cmd.Parameters.Add("@LEDGtotal", SqlDbType.Int).Value = LEDGtotal;
            cmd.Parameters.Add("@TAXPtotal", SqlDbType.Int).Value = TAXPtotal;
            cmd.Parameters.Add("@COMPRtotal", SqlDbType.Int).Value = COMPRtotal;
            cmd.Parameters.Add("@COMPLtotal", SqlDbType.Int).Value = COMPLtotal;
            cmd.Parameters.Add("@INVtotal", SqlDbType.Int).Value = INVtotal;
            cmd.Parameters.Add("@OTHERStotal", SqlDbType.Int).Value = OTHERStotal;
            cmd.Parameters.Add("@totalAUDIT", SqlDbType.Int).Value = totalAUDIT;
            cmd.Parameters.Add("@totalCEDM", SqlDbType.Int).Value = totalCEDM;
            cmd.Parameters.Add("@totalOTHERS", SqlDbType.Int).Value = totalOTHERS;
            cmd.Parameters.Add("@tccAUDIT", SqlDbType.Int).Value = tccAUDIT;
            cmd.Parameters.Add("@recAUDIT", SqlDbType.Int).Value = recAUDIT;
            cmd.Parameters.Add("@ledgAUDIT", SqlDbType.Int).Value = ledgAUDIT;
            cmd.Parameters.Add("@taxpAUDIT", SqlDbType.Int).Value = taxpAUDIT;
            cmd.Parameters.Add("@comprAUDIT", SqlDbType.Int).Value = comprAUDIT;
            cmd.Parameters.Add("@complAUDIT", SqlDbType.Int).Value = complAUDIT;
            cmd.Parameters.Add("@invAUDIT", SqlDbType.Int).Value = invAUDIT;
            cmd.Parameters.Add("@othersAUDIT", SqlDbType.Int).Value = othersAUDIT;
            cmd.Parameters.Add("@tccCEDM", SqlDbType.Int).Value = tccCEDM;
            cmd.Parameters.Add("@recCEDM", SqlDbType.Int).Value = recCEDM;
            cmd.Parameters.Add("@ledgCEDM", SqlDbType.Int).Value = ledgCEDM;
            cmd.Parameters.Add("@taxpCEDM", SqlDbType.Int).Value = taxpCEDM;
            cmd.Parameters.Add("@comprCEDM", SqlDbType.Int).Value = comprCEDM;
            cmd.Parameters.Add("@complCEDM", SqlDbType.Int).Value = complCEDM;
            cmd.Parameters.Add("@invCEDM", SqlDbType.Int).Value = invCEDM;
            cmd.Parameters.Add("@othersCEDM", SqlDbType.Int).Value = othersCEDM;
            cmd.Parameters.Add("@tccOTHERS", SqlDbType.Int).Value = tccOTHERS;
            cmd.Parameters.Add("@recOTHERS", SqlDbType.Int).Value = recOTHERS;
            cmd.Parameters.Add("@ledgOTHERS", SqlDbType.Int).Value = ledgOTHERS;
            cmd.Parameters.Add("@taxpOTHERS", SqlDbType.Int).Value = taxpOTHERS;
            cmd.Parameters.Add("@comprOTHERS", SqlDbType.Int).Value = comprOTHERS;
            cmd.Parameters.Add("@complOTHERS", SqlDbType.Int).Value = complOTHERS;
            cmd.Parameters.Add("@invOTHERS", SqlDbType.Int).Value = invOTHERS;
            cmd.Parameters.Add("@othersOTHERS", SqlDbType.Int).Value = othersOTHERS;
            cmd.Parameters.Add("@totalTOTAL", SqlDbType.Int).Value = totalTOTAL;

            cmd.ExecuteNonQuery();
            con.connectionClose();

            //Fetch recent data (last row) into the datatable for the report
            SqlCommand cmdd = con.cmd();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = new SqlCommand();
            da.SelectCommand = cmdd;

            cmdd.CommandText = "EXECUTE repMonthlyReqForm @reqID";

            cmdd.Parameters.Add("@reqID", SqlDbType.Int).Value = intReqID;

            DataSet dataSet = new DataSet();
            da.Fill(dataSet, "MonthlyReqForm");
            DataTable dTable = dataSet.Tables["MonthlyReqForm"];

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
