using System;
using System.Data;
using System.Data.SqlClient;
using Zenoph.SMSLib;
using Zenoph.SMSLib.Enums;

namespace CFO_Requisition_System
{
    class MainFormMethods
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

        //Method to insert into log out file when user logs out
        public void InsertLogoutFile()
        {
            int intID = GenerateID("LogFile", "logID");
            string strPassword = FetchLastPassword();
            string strUsername = FetchLastUsername();
            string strSurname = "";
            string strOtherNames = "";
            string strOfficerName = "";
            string strLogDate = DateTime.Now.ToString("f");
            string strLogTime = DateTime.Now.ToString("h:mm:ss tt zz");
            string strLogType = "LOGOUT";

            if (strPassword == "admin2017" && strUsername == "admin")
            {
                //Save logout details in log file
                SqlCommand cmdd = con.cmd();

                //Form a string of the full officer's name
                strOfficerName = "ADMINISTRATOR";

                //Populate the logfile in the database
                cmdd.CommandText = "EXECUTE InsertLog @logID, @officerName, @username, @psword, @logDate, @logTime, @logType";

                cmdd.Parameters.Add("@logID", SqlDbType.Int).Value = intID;
                cmdd.Parameters.Add("@officerName", SqlDbType.VarChar, 100).Value = strOfficerName;
                cmdd.Parameters.Add("@username", SqlDbType.VarChar, 12).Value = strUsername;
                cmdd.Parameters.Add("@psword", SqlDbType.VarChar, 12).Value = strPassword;
                cmdd.Parameters.Add("@logDate", SqlDbType.Date).Value = strLogDate;
                cmdd.Parameters.Add("@logTime", SqlDbType.VarChar, 20).Value = strLogTime;
                cmdd.Parameters.Add("@logType", SqlDbType.VarChar, 10).Value = strLogType;

                cmdd.ExecuteNonQuery();
                con.connectionClose();
            }

            else
            {
                SqlCommand cmd = con.cmd();
                cmd.CommandText = "select * from Officer where username = '" + strUsername + "' and psword = '" + strPassword + "'";
                SqlDataReader dataRead = cmd.ExecuteReader();

                while (dataRead.Read())
                {
                    strSurname = dataRead["surname"].ToString();
                    strOtherNames = dataRead["otherNames"].ToString();
                }

                dataRead.Close();
                cmd.ExecuteNonQuery();
                con.connectionClose();

                //Save logout details in log file
                SqlCommand cmdd = con.cmd();

                //Form a string of the full officer's name
                strOfficerName = strSurname + " " + strOtherNames;

                //Populate the logfile in the database
                cmdd.CommandText = "EXECUTE InsertLog @logID, @officerName, @username, @psword, @logDate, @logTime, @logType";

                cmdd.Parameters.Add("@logID", SqlDbType.Int).Value = intID;
                cmdd.Parameters.Add("@officerName", SqlDbType.VarChar, 100).Value = strOfficerName;
                cmdd.Parameters.Add("@username", SqlDbType.VarChar, 12).Value = strUsername;
                cmdd.Parameters.Add("@psword", SqlDbType.VarChar, 12).Value = strPassword;
                cmdd.Parameters.Add("@logDate", SqlDbType.Date).Value = strLogDate;
                cmdd.Parameters.Add("@logTime", SqlDbType.VarChar, 20).Value = strLogTime;
                cmdd.Parameters.Add("@logType", SqlDbType.VarChar, 10).Value = strLogType;

                cmdd.ExecuteNonQuery();
                con.connectionClose();  
            }
        }

        //Method to fetch the last username in the log file table in the DB
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

        //Method to fetch the last password in the log file table in the DB
        public string FetchLastPassword()
        {
            string strLastPassword= "";

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

        //Method to test if the supervisor has logged in
        public bool IsSupervisor()
        {
            string password = FetchLastPassword();
            string username = FetchLastUsername();
            string strPosition = "";

            if (password == "admin2017" && username == "admin")
                return true;

            SqlCommand cmd = con.cmd();
            cmd.CommandText = "select position from Officer where psword = '"+ password +"' and username = '"+ username +"'";

            SqlDataAdapter dataAdapter = new SqlDataAdapter();
            dataAdapter.SelectCommand = cmd;
            DataSet dataSet = new DataSet();

            dataAdapter.Fill(dataSet, "Officer");
            DataTable dTable = dataSet.Tables["Officer"];

            DataRow dRow = dTable.Rows[dTable.Rows.Count - 1];
            strPosition = dRow["position"].ToString();

            if (strPosition == "SUPERVISOR")
                return true;

            return false;
        }

        //Method to find date to return a folder (for SMS notification)
        public string dateToReturnFolder(string dateRequested)
        {
            int numberOfDays = 0;
            DateTime dateToReturn = DateTime.Parse(dateRequested);

            while (numberOfDays != 5)
            {
                dateToReturn = dateToReturn.AddDays(1);

                string day = dateToReturn.DayOfWeek.ToString();

                if (day.ToLower() != "sunday" && day.ToLower() != "saturday")
                {
                    numberOfDays += 1;
                }
            }

            return (dateToReturn.Date.ToString());
        }

        //Method to send SMS provided the text and the contact
        public void sendSMS(string smsText, string contact)
        {
            try
            {
                // Initialise SMS object and perform authentication.
                ZenophSMS sms = new ZenophSMS();
                sms.setUser("apekodonkorjewel@gmail.com");
                sms.setPassword("umaticgc");
                sms.authenticate();

                // the message to be sent.
                string msg = smsText;

                sms.setMessage(msg);
                sms.setSenderId("GRA_CFO");
                sms.setMessageType(MSGTYPE.TEXT);

                sms.addRecipient(contact, false);    // if invalid, exception will not be thrown.

                // submit the message.
                sms.submit();
            }

            catch (SMSException sex)
            {
                System.Windows.Forms.MessageBox.Show(sex.Message);
            }

            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        //Method to fetch officer's contact using the officer ID
        public string fetchOfficerContact(string officerID)
        {
            string contact = "";
            int intOfficerID = int.Parse(officerID);

            SqlCommand cmd = con.cmd();
            cmd.CommandText = "select * from Officer where officerID = '" + intOfficerID + "'";

            SqlDataAdapter dataAdapter = new SqlDataAdapter();
            dataAdapter.SelectCommand = cmd;
            DataSet dataSet = new DataSet();

            dataAdapter.Fill(dataSet, "Officer");
            DataTable dTable = dataSet.Tables["Officer"];

            DataRow dRow = dTable.Rows[dTable.Rows.Count - 1];
            contact = dRow["contact"].ToString();

            return contact;
        }

        //Method to find the number of days an officer has defaulted in returning a folder
        public string noOfDefaultDays(DateTime dateToReturnFolder)
        {
            int defaultDays = (DateTime.Now.Date - dateToReturnFolder).Days;

            return defaultDays.ToString();
        }

        //Method to search the requisition table and send SMS once a day
        public void sendSMS()
        {
            string sms = "";

            //Check if login is the first time for the day using the last row in the loginDetails table
            string lastLogDate = "";

            SqlCommand cmd = con.cmd();
            cmd.CommandText = "select * from LogFile";

            SqlDataAdapter dataAdapter = new SqlDataAdapter();
            dataAdapter.SelectCommand = cmd;
            DataSet dataSet = new DataSet();

            dataAdapter.Fill(dataSet, "LogFile");
            DataTable dTable = dataSet.Tables["LogFile"];

            DataRow dRow = dTable.Rows[dTable.Rows.Count - 2];
            lastLogDate = dRow["logDate"].ToString();

            DateTime lastlogDate2 = Convert.ToDateTime(lastLogDate);

            if (DateTime.Now.ToShortDateString() != lastlogDate2.ToShortDateString())
            {
                //SMS has not already been sent

                //Fetch all rows in Requisition table that have no return dates
                cmd = con.cmd();
                cmd.CommandText = "select * from Requisition where dateReturned is null";

                dataAdapter = new SqlDataAdapter();
                dataAdapter.SelectCommand = cmd;
                dataSet = new DataSet();

                dataAdapter.Fill(dataSet, "Requisition");
                dTable = dataSet.Tables["Requisition"];

                int rowCount = dTable.Rows.Count;
                DateTime returnDate;
                string requestDate = "";
                DateTime currentDate = DateTime.Now.Date;
                string officerName = "";
                string tradersName = "";
                string officerID = "";
                string defaultDays = "";

                if (rowCount > 0)
                {
                    for (int i = 0; i < dTable.Rows.Count; i++)
                    {
                        dRow = dTable.Rows[i];

                        //Find date to Return each unreturned folder. If date to return a folder = current date, send SMS
                        requestDate = dRow["dateCollected"].ToString();
                        returnDate = DateTime.Parse(dateToReturnFolder(requestDate));
                        officerName = dRow["receivingOfficerReq"].ToString();
                        officerID = dRow["officerID"].ToString();
                        tradersName = dRow["tradersName"].ToString();

                        if (currentDate == returnDate)
                        {
                            //Return date is today; send SMS
                            sms = "Hi " + officerName +", you are hereby reminded to return the folder of " + tradersName +"by the close of today as by law required";

                            //Get Officer's contact and Send SMS
                            sendSMS(sms, fetchOfficerContact(officerID));
                        }

                        else if(currentDate > returnDate)
                        {
                            //The officer has defaulted the date
                            //Calculate number of days defaulted
                            defaultDays = noOfDefaultDays(returnDate);
                            sms = "Hi " + officerName + ", you are hereby notified that you have defaulted for "+ defaultDays +" days to return the folder of " + tradersName;

                            //Get Officer's contact and Send SMS
                            sendSMS(sms, fetchOfficerContact(officerID));
                        }
                    }
                }

            }




        }
    }
}
